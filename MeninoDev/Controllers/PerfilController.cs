using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MeninoDev.Controllers
{
    public class PerfilController : Controller
    {
        public async Task<IActionResult> Form()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Form(string form)
        {
            if (!ModelState.IsValid)
                return View();


            return RedirectToAction("Index");
        }
    }
}
