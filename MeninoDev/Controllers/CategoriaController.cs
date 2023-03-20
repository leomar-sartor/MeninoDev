using Dapper;
using MeninoDev.Entidades;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace MeninoDev.Controllers
{
    public class CategoriaController : Controller
    {
        public IConfiguration _configuration { get; }
        public string _connectionString { get; }

        public CategoriaController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("database");
        }

        public IActionResult Index(string searchString, int? pageNumber = 1)
        {
            if (pageNumber == 0)
                pageNumber = 1;

            @ViewBag.SearchString = searchString;

            ViewData["CurrentFilter"] = searchString;

            IEnumerable<Categoria> categorias;

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM POST";

                if (!String.IsNullOrEmpty(searchString))
                {
                    sql = sql + @$" WHERE Title like '%{searchString}%'";
                }

                categorias = db.Query<Categoria>(sql);
            }

            int pageSize = 20;
            //var postes = PaginatedList<Post>.Create(posts.OrderByDescending(m => m.Date), pageNumber ?? 1, pageSize);

            var retorno = categorias.OrderByDescending(m => m.Id).ToPagedList((int)pageNumber, 6);


            return View(retorno);
        }

        public async Task<IActionResult> Form(long Id)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Form(Categoria form)
        {
            if (!ModelState.IsValid)
                return View(form);


            return RedirectToAction("Index");
        }
    }
}
