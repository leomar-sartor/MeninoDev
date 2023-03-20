using MeninoDev.Entidades;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using X.PagedList;
using Microsoft.AspNetCore.Authorization;

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

        [AllowAnonymous]
        public IActionResult Index(string searchString, int? pageNumber = 1)
        {
            if (pageNumber == 0)
                pageNumber = 1;

            @ViewBag.SearchString = searchString;

            ViewData["CurrentFilter"] = searchString;

            IEnumerable<Post> posts;

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM POST";

                if (!String.IsNullOrEmpty(searchString))
                {
                    sql = sql + @$" WHERE Title like '%{searchString}%'";
                }

                posts = db.Query<Post>(sql);
            }

            int pageSize = 6;
            //var postes = PaginatedList<Post>.Create(posts.OrderByDescending(m => m.Date), pageNumber ?? 1, pageSize);

            var retorno = posts.OrderByDescending(m => m.Date).ToPagedList((int)pageNumber, 6);

            
            return View(retorno);
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

            //if (post.Imagem != null)
            //{
                //string pastaFotos = Path.Combine(webHostEnvironment.WebRootPath, "Imagens");
                //nomeUnicoArquivo = Guid.NewGuid().ToString() + "_" + model.Foto.FileName;
                //string caminhoArquivo = Path.Combine(pastaFotos, nomeUnicoArquivo);
                //using (var fileStream = new FileStream(caminhoArquivo, FileMode.Create))
                //{
                //    model.Foto.CopyTo(fileStream);
                //}
            //}

            if (post.Id > 0)
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    var sql = @"UPDATE POST SET Date = @Date, Title = @Title, Content = @Content, Url = @Url OUTPUT INSERTED.* WHERE Id = @Id";
                    post.Date = DateTime.Today;

                    var retorno = db.QuerySingle<Post>(sql, new
                    {
                        post.Date,
                        post.Title,
                        post.Content,
                        post.Url,
                        post.Id
                    });
                }

                TempData["Sucesso"] = "Post atualizado!";
            }
            else
            {

                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    var sql = @"INSERT POST (Date, Title, Content, Url) OUTPUT INSERTED.* VALUES (@Date, @Title, @Content, @Url)";
                    post.Date = DateTime.Today;

                    var retorno = db.QuerySingle<Post>(sql, new
                    {
                        post.Date,
                        post.Title,
                        post.Content,
                        post.Url
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


        public IActionResult Read(long Id, long Page = 0)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sqlPost = "SELECT * FROM POST WHERE Id = @PostId";
                var Post = db.QueryFirstOrDefault<Post>(sqlPost, new { PostId = Id });

                var sqlComments = @$"
                                    SELECT C.*, U.UserName as CommentUser FROM COMMENT C 
                                    INNER JOIN AspNetUsers U ON U.Id = C.UserId
                                    WHERE PostId = @PostId and CommentId = 0";
                var comments = db.Query<Comment>(sqlComments, new { PostId = Id }).ToList();
                Post.PostPage = Page;

                foreach (var cm in comments)
                {
                    var sqlSubComments = @$"SELECT C.*, U.UserName as CommentUser FROM COMMENT C 
                                    INNER JOIN AspNetUsers U ON U.Id = C.UserId
                                    WHERE CommentId = @CommentId";
                    var subcomments = db.Query<Comment>(sqlSubComments, new { CommentId = cm.Id }).ToList();
                    cm.SubComments = subcomments;
                }

                Post.Comments = comments;

                return View(Post);
            }
        }
    }
}
