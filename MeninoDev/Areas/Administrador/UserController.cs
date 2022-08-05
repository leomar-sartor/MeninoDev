using Microsoft.AspNetCore.Mvc;

namespace MeninoDev.Areas.Administrador
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
