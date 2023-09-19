using Dapper;
using MeninoDev.Contexto;
using MeninoDev.Models.Perfil;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Authorization;

namespace MeninoDev.Controllers
{
    [Authorize]
    public class PerfilController : Controller
    {
        public IConfiguration _configuration { get; }
        public string _connectionString { get; }
        private readonly UserManager<UserApp> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PerfilController(IConfiguration configuration, UserManager<UserApp> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
            _connectionString = configuration.GetConnectionString("database");
        }
        public async Task<IActionResult> Form()
        {
            var meuPerfil = new PerfilViewModel();

            var cadastro = new CadastroViewModel();
            var IdUser = User.FindFirst(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(IdUser.Value);
            var res = _userManager.GetUserName(User);
            var email = await _userManager.GetEmailAsync(user);
            cadastro.Apelido = user.Apelido;
            cadastro.Email = email;
            cadastro.UrlFoto = user.UrlFoto?? "";

            var trocaSenha = new TrocarSenhaViewModel();

            meuPerfil.Cadastro = cadastro;
            meuPerfil.TrocaSenha = trocaSenha;

            return View(meuPerfil);
        }

        [HttpPost]
        public async Task<IActionResult> Cadastro(CadastroViewModel form)
        {
            if (!ModelState.IsValid)
                return View(form);

            var IdUser = User.FindFirst(ClaimTypes.NameIdentifier);
            using (IDbConnection db = new MySqlConnection(_connectionString))
            {
                var sql = @"UPDATE AspNetUsers SET Apelido = @Apelido WHERE Id = @Id";

                db.Execute(sql, new
                {
                    Apelido = form.Apelido,
                    Id = IdUser.Value,
                });
            }

            return RedirectToAction("Form");
        }

        [HttpPost]
        public async Task<IActionResult> TrocarSenha(TrocarSenhaViewModel form)
        {
            if (!ModelState.IsValid)
                return View(form);

            if(form.novaSenha == form.confirmacaoNovaSenha && (!String.IsNullOrEmpty(form.novaSenha)))
            {
                var IdUser = User.FindFirst(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(IdUser.Value);

                var result = await _userManager.ChangePasswordAsync(user, form.senhaAtual, form.novaSenha);
            }

            return RedirectToAction("Form");
        }

        public async Task<IActionResult> Foto()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Foto(IFormFile file)
        {
            if (file != null)
            {
                string pastaDocumentos = Path.Combine(_webHostEnvironment.WebRootPath, "Documentos");
                var nomeUnicoArquivo = file.FileName;
                string caminhoArquivo = Path.Combine(pastaDocumentos, nomeUnicoArquivo);
                using (var fileStream = new FileStream(caminhoArquivo, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                var IdUser = User.FindFirst(ClaimTypes.NameIdentifier);
                using (IDbConnection db = new MySqlConnection(_connectionString))
                {
                    var sql = @"UPDATE AspNetUsers SET UrlFoto = @UrlFoto WHERE Id = @Id";

                    db.Execute(sql, new
                    {
                        UrlFoto = file.FileName,
                        Id = IdUser.Value,
                    });
                }
            }

            return RedirectToAction("Form");
        }

        public async Task<IActionResult> FotoRemove()
        {
            var IdUser = User.FindFirst(ClaimTypes.NameIdentifier);
            using (IDbConnection db = new MySqlConnection(_connectionString))
            {
                var sql = @"UPDATE AspNetUsers SET UrlFoto = @UrlFoto WHERE Id = @Id";

                db.Execute(sql, new
                {
                    UrlFoto = "",
                    Id = IdUser.Value,
                });
            }

            return RedirectToAction("Form");
        }
    }
}
