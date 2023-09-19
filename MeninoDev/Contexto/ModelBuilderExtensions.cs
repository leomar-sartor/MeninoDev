using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace MeninoDev.Contexto
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder builder)
        {
            var pwd = "P@$$w0rd";
            var passwordHasher = new PasswordHasher<UserApp>();

            // Seed Roles
            var adminRole = new IdentityRole("Admin");
            adminRole.NormalizedName = adminRole.Name.ToUpper();

            List<IdentityRole> roles = new List<IdentityRole>() {
                adminRole
            };

            builder.Entity<IdentityRole>().HasData(roles);


            // Seed Users
            var adminUser = new UserApp
            {
                Apelido = "Administrador",
                UserName = "admin@meuagro.net",
                Email = "admin@meuagro.net",
                PhoneNumber = "4933464164",
                EmailConfirmed = true
            };
            adminUser.NormalizedUserName = adminUser.UserName.ToUpper();
            adminUser.NormalizedEmail = adminUser.Email.ToUpper();
            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, pwd);

            List<UserApp> users = new List<UserApp>() {
                adminUser
            };

            builder.Entity<UserApp>().HasData(users);

            // Seed UserRoles
            List<IdentityUserRole<string>> userRoles = new List<IdentityUserRole<string>>();

            userRoles.Add(new IdentityUserRole<string>
            {
                UserId = users[0].Id,
                RoleId = roles.First(q => q.Name == "Admin").Id
            });

            builder.Entity<IdentityUserRole<string>>().HasData(userRoles);
        }
    }
}
