using Dapper;
using MeninoDev.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace MeninoDev.Controllers
{
    [Authorize]
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

            using (IDbConnection db = new MySqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM Category";

                if (!String.IsNullOrEmpty(searchString))
                {
                    sql = sql + @$" WHERE Name like '%{searchString}%'";
                }

                categorias = db.Query<Categoria>(sql);
            }

            int pageSize = 20;

            var retorno = categorias.OrderByDescending(m => m.Id).ToPagedList((int)pageNumber, pageSize);

            return View(retorno);
        }

        public async Task<IActionResult> Form(long Id)
        {
            var categoria = new Categoria(); 

            if (Id > 0)
            {
                using (IDbConnection db = new MySqlConnection(_connectionString))
                {
                    var sql = "SELECT * FROM Category WHERE Id = @Id";
                    categoria = await db.QueryFirstOrDefaultAsync<Categoria>(sql, new { Id = Id });
                }
            }

            return View(categoria);
        }

        [HttpPost]
        public async Task<IActionResult> Form(Categoria form)
        {
            if (!ModelState.IsValid)
                return View(form);

            if (form.Id > 0)
            {
                using (IDbConnection db = new MySqlConnection(_connectionString))
                {
                    var sql = @"UPDATE Category SET Date = @Date, Name = @Name WHERE Id = @Id";
                    form.Date = DateTime.Today;

                    var retorno = db.QuerySingleOrDefault<Categoria>(sql, new
                    {
                        form.Date,
                        form.Name,
                        form.Id
                    });
                }

                TempData["Sucesso"] = "Post atualizado!";
            }
            else
            {

                using (IDbConnection db = new MySqlConnection(_connectionString))
                {
                    var sql = @"INSERT Category (Name, Date) VALUES (@Name, @Date)";
                    form.Date = DateTime.Today;

                    var retorno = db.QuerySingleOrDefault<Categoria>(sql, new
                    {
                        form.Date,
                        form.Name
                    });
                }

                TempData["Sucesso"] = "Post salvo com sucesso!";
            }
            
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(long Id)
        {
            using (IDbConnection db = new MySqlConnection(_connectionString))
            {
                var sql = "DELETE FROM Category WHERE Id = @Id";

                db.Query(sql, new { Id });
            }

            TempData["Exclusao"] = "Categoria removida com sucesso!";

            return RedirectToAction("Index");
        }
    }
}
