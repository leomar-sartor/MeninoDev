using MeninoDev.Entidades;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using Dapper;
using System;

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
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Form()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Form(Post post)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Dados Inválidos!";
                return View();
            }
            
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

            return RedirectToAction("Index");
        }
    }
}
