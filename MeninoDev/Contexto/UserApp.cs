using Microsoft.AspNetCore.Identity;

namespace MeninoDev.Contexto
{
    public class UserApp : IdentityUser
    {
        public string Apelido { get; set; }
    }
}
