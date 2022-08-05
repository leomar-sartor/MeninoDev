using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;

namespace MeninoDev.Models
{
    public class UsuarioViewModel
    {
        public UsuarioViewModel()
        {
            Roles = new List<IdentityRole>();
            Claims = new List<Claim>();
        }
        public string Usuario { get; set; }

        public string Apelido { get; set; }

        public string Origem { get; set; }

        public string Email { get; set; }

        public List<IdentityRole> Roles { get; set; }
        public  List<Claim> Claims { get; set; }
    }
}
