using MeninoDev.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace MeninoDev.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return RedirectToAction("index", "Post");
        }

        [AllowAnonymous]
        public IActionResult PoliticaEPrivacidade()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult TermosDeUso()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult TesteErro()
        {
            long acima = 100;
            long abaixo = 0;
            var resultado = acima / abaixo;

            return RedirectToAction("index", "Post");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
