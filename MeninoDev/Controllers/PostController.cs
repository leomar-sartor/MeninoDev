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
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MeninoDev.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        public IConfiguration _configuration { get; }
        public string _connectionString { get; }

        public PostController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("database");
        }
        
        [AllowAnonymous]
        public IActionResult Index(string searchString, int? pageNumber = 1, long? categoriaId = null)
        {
            if (pageNumber == 0)
                pageNumber = 1;

            @ViewBag.SearchString = searchString;

            ViewData["CurrentFilter"] = searchString;

            IEnumerable<Post> posts;

            using (IDbConnection db = new MySqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM Post";

                var entrou = false;
                if(categoriaId > 0)
                {
                    entrou = true;
                    sql = sql + @$" WHERE CategoriaId = {categoriaId}";
                }

                if (!String.IsNullOrEmpty(searchString))
                {
                    if(entrou)
                        sql = sql + @$" AND Title LIKE  '%{searchString}%'";
                    else
                        sql = sql + @$" WHERE Title LIKE  '%{searchString}%'";
                }

                posts = db.Query<Post>(sql);
            }

            int pageSize = 6;

            var retorno = posts.OrderByDescending(m => m.Date).ToPagedList((int)pageNumber, 6);

            
            return View(retorno);
        }

        public IActionResult Form(long Id)
        {
            using (IDbConnection db = new MySqlConnection(_connectionString))
            {
                var sqlCategorias = "SELECT * FROM Category";
                var Categorias = db.Query<Categoria>(sqlCategorias);

                var itens = Categorias.Select(m => new SelectListItem()
                {
                    Value = m.Id.ToString(),
                    Text = m.Name
                }).ToList();

                @ViewBag.Categorias = itens;
            }

            if (Id > 0)
            {
                using (IDbConnection db = new MySqlConnection(_connectionString))
                {
                    var sqlPost = "SELECT * FROM Post WHERE Id = @PostId";
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

                using (IDbConnection db = new MySqlConnection(_connectionString))
                {
                    var sqlCategorias = "SELECT * FROM Category";
                    var Categorias = db.Query<Categoria>(sqlCategorias);

                    var itens = Categorias.Select(m => new SelectListItem()
                    {
                        Value = m.Id.ToString(),
                        Text = m.Name
                    }).ToList();

                    @ViewBag.Categorias = itens;
                }

                return View();
            }

            if (post.Id > 0)
            {
                using (IDbConnection db = new MySqlConnection(_connectionString))
                {
                    var sql = @"UPDATE Post SET Date = @Date, Title = @Title, Descricao = @Descricao, CategoriaId = @CategoriaId, Content = @Content, Url = @Url WHERE Id = @Id";
                    post.Date = DateTime.Today;

                    var retorno = db.QuerySingleOrDefault<Post>(sql, new
                    {
                        post.Date,
                        post.Title,
                        post.Descricao,
                        post.CategoriaId,
                        Content = post.Content ?? "",
                        post.Url,
                        post.Id
                    });
                }

                TempData["Sucesso"] = "Post atualizado!";
            }
            else
            {

                using (IDbConnection db = new MySqlConnection(_connectionString))
                {
                    var sql = @"INSERT INTO Post (Date, Title, Descricao, CategoriaId, Content, Url) VALUES (@Date, @Title, @Descricao,  @CategoriaId, @Content, @Url)";
                    post.Date = DateTime.Today;

                    var retorno = db.QuerySingleOrDefault<Post>(sql, new
                    {
                        post.Date,
                        post.Title,
                        post.Descricao,
                        post.CategoriaId,
                        Content = post.Content ?? "",
                        post.Url
                    });
                }

                TempData["Sucesso"] = "Post salvo com sucesso!";
            }

            return RedirectToAction("Index");
        }

        public ActionResult Delete(long Id)
        {
            using (IDbConnection db = new MySqlConnection(_connectionString))
            {
                var sql = "DELETE FROM Post WHERE Id = @PostId";

                db.Query(sql, new { PostId = Id });
            }

            TempData["Exclusao"] = "Post removido com sucesso!";

            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public IActionResult Read(long Id, long Page = 0)
        {
            using (IDbConnection db = new MySqlConnection(_connectionString))
            {
                var sqlPost = "SELECT * FROM Post WHERE Id = @PostId";
                var Post = db.QueryFirstOrDefault<Post>(sqlPost, new { PostId = Id });

                var sqlComments = @$"
                                    SELECT C.*, U.UserName as CommentUser FROM Comment C 
                                    INNER JOIN AspNetUsers U ON U.Id = C.UserId
                                    WHERE PostId = @PostId and CommentId = 0";
                var comments = db.Query<Comment>(sqlComments, new { PostId = Id }).ToList();
                Post.PostPage = Page;

                foreach (var cm in comments)
                {
                    var sqlSubComments = @$"SELECT C.*, U.UserName as CommentUser FROM Comment C 
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
