using MeninoDev.Contexto;
using MeninoDev.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace MeninoDev.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UserManager<UserApp> _userManager;
        private readonly SignInManager<UserApp> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsuarioController(SignInManager<UserApp> signInManager,
            UserManager<UserApp> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            var usuarios = new List<UsuarioViewModel>();
            var users = _userManager.Users.AsEnumerable();

            foreach (var item in users)
            {
                var user = new UsuarioViewModel();

                user.Usuario = item.UserName;
                user.Apelido = item.Apelido;
                user.Email = item.Email;

                user.Origem = "";

                var dados = User as ClaimsPrincipal;
                

                
                //user.Claims

            }

            //Claim claim = new Claim("", "", ClaimValueTypes.String);
            //IdentityRole role = new IdentityRole("");





            return View(users);
        }
    }
}
