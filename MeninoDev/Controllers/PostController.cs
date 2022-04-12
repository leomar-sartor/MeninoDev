using Microsoft.AspNetCore.Mvc;

namespace MeninoDev.Controllers
{
    public class PostController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
