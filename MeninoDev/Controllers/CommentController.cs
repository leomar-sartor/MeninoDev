using Microsoft.AspNetCore.Mvc;
using MeninoDev.Entidades;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using Dapper;
using System.Linq;

namespace MeninoDev.Controllers
{
    public class CommentController : Controller
    {
        public IConfiguration _configuration { get; }
        public string _connectionString { get; }

        public CommentController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("database");
        }

        public IActionResult Create(long postId, long commentId = 0)
        {
            var comment = new Comment();
            comment.PostId = postId;
            comment.UserId = User.Claims.FirstOrDefault().Value;
            comment.CommentId = commentId;

            return View(comment);
        }

        [HttpPost]
        public IActionResult Create(Comment comment)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sql = @"INSERT COMMENT (PostId, Date, Content, UserId, CommentId) OUTPUT INSERTED.* VALUES (@PostId, @Date, @Content, @UserId, @CommentId)";
                comment.Date = DateTime.Today;

                var retorno = db.QuerySingle<Comment>(sql, new
                {
                    comment.PostId,
                    comment.Date,
                    comment.Content,
                    comment.UserId,
                    comment.CommentId
                });
            }

            return RedirectToAction("Read", "Post", new { Id = comment.PostId });
        }

        [Route("/Comment/Delete/{PostId}/{CommentId}")]
        public IActionResult Delete(long PostId, long CommentId)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sql = "DELETE FROM COMMENT WHERE Id = @CommentId";

                db.Query(sql, new { CommentId });
            }

            return RedirectToAction("Read", "Post", new { Id = PostId });
        }
    }
}
