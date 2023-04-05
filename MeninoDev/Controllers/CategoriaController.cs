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
                var sql = "SELECT * FROM CATEGORY";

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
            var categoria = new Categoria(); // await _rTipoProduto.Buscar(Id);

            if (Id > 0)
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
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
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    var sql = @"UPDATE CATEGORY SET Date = @Date, Name = @Name OUTPUT INSERTED.* WHERE Id = @Id";
                    form.Date = DateTime.Today;

                    var retorno = db.QuerySingle<Categoria>(sql, new
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

                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    var sql = @"INSERT CATEGORY (Name, Date) OUTPUT INSERTED.* VALUES (@Name, @Date)";
                    form.Date = DateTime.Today;

                    var retorno = db.QuerySingle<Categoria>(sql, new
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
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var sql = "DELETE FROM CATEGORY WHERE Id = @Id";

                db.Query(sql, new { Id = Id });
            }

            TempData["Exclusao"] = "Categoria removida com sucesso!";

            return RedirectToAction("Index");
        }

        
    }
}
