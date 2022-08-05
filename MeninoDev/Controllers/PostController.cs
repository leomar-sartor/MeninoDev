using MeninoDev.Entidades;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MeninoDev.Controllers
{
    public class PostController : Controller
    {
        public IConfiguration _configuration { get; }
        public string _connectionString { get; }

        public PostController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("database");
        }
        public IActionResult Index(string searchString, int? pageNumber = 1)
        {
            ViewData["CurrentFilter"] = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                //students = students.Where(s => s.LastName.Contains(searchString)
                //                       || s.FirstMidName.Contains(searchString));
            }

            IEnumerable<Post> posts;

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM POST";
                posts = db.Query<Post>(sql);
            }


            int pageSize = 3;
            var postes = PaginatedList<Post>.Create(posts.OrderByDescending(m => m.Date), pageNumber ?? 1, pageSize);
            

            return View(postes);
        }

        public IActionResult Form(long Id)
        {
            if (Id > 0)
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    var sqlPost = "SELECT * FROM POST WHERE Id = @PostId";
                    var Post = db.QueryFirstOrDefault<Post>(sqlPost, new { PostId = Id });

                    return View(Post);
                }
            }

            return View();
        }

        [HttpPost]
        public IActionResult Form(Post post)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Dados Inválidos!";
                return View();
            }

            if (post.Id > 0)
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    var sql = @"UPDATE POST SET Date = @Date, Title = @Title, Content = @Content OUTPUT INSERTED.* WHERE Id = @Id";
                    post.Date = DateTime.Today;

                    var retorno = db.QuerySingle<Post>(sql, new
                    {
                        post.Date,
                        post.Title,
                        post.Content,
                        post.Id
                    });
                }

                TempData["Sucesso"] = "Post atualizado!";
            }
            else
            {

                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    var sql = @"INSERT POST (Date, Title, Content) OUTPUT INSERTED.* VALUES (@Date, @Title, @Content)";
                    post.Date = DateTime.Today;

                    var retorno = db.QuerySingle<Post>(sql, new
                    {
                        post.Date,
                        post.Title,
                        post.Content
                    });
                }

                TempData["Sucesso"] = "Post salvo com sucesso!";
            }

            return RedirectToAction("Index");
        }

        public ActionResult Delete(long Id)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sql = "DELETE FROM POST WHERE Id = @PostId";

                db.Query(sql, new { PostId = Id });
            }

            TempData["Exclusao"] = "Post removido com sucesso!";

            return RedirectToAction("Index");
        }


        public IActionResult Read(long Id)
        {

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlPost = "SELECT * FROM POST WHERE Id = @PostId";
                var Post = db.QueryFirstOrDefault<Post>(sqlPost, new { PostId = Id });

                var sqlComments = "SELECT * FROM COMMENT WHERE PostId = @PostId";
                Post.Comments = db.Query<Comment>(sqlComments, new { PostId = Id }).ToList();

                return View(Post);
            }
        }

    }
}
