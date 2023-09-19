using Google.Protobuf.WellKnownTypes;
using MeninoDev.Contexto;
using MeninoDev.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeninoDev
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var con = Configuration.GetConnectionString("database");
            var version = ServerVersion.AutoDetect(Configuration.GetConnectionString("database"));

            services.AddDbContext<Context>(options =>
                options.UseMySql(con, version));


            services.AddDefaultIdentity<UserApp>
               (options =>
                   {
                       //options.SignIn.RequireConfirmedAccount = true;
                       options.Password.RequiredLength = 6;
                       options.Password.RequiredUniqueChars = 3;
                       options.Password.RequireNonAlphanumeric = false;
                   })
               .AddRoles<IdentityRole>()
               .AddEntityFrameworkStores<Context>();

            services.AddTransient<ICategoriaService, CategoriaService>();

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    IConfigurationSection googleAuthNSection =
                        Configuration.GetSection("Authentication:Google");
                    options.ClientId = googleAuthNSection["ClientId"];
                    options.ClientSecret = googleAuthNSection["ClientSecret"];
                });
            // .AddFacebook(opt =>
            // {
            //     IConfigurationSection googleAuthNSection =
            //         Configuration.GetSection("Authentication:Facebook");
            //     opt.ClientId = googleAuthNSection["AppId"];
            //     opt.ClientSecret = googleAuthNSection["AppSecret"];
            // })
            // .AddMicrosoftAccount(microsoftOptions =>
            // {
            //     IConfigurationSection googleAuthMsft =
            //         Configuration.GetSection("Authentication:Microsoft");
            //     microsoftOptions.ClientId = googleAuthMsft["AppId"];
            //     microsoftOptions.ClientSecret = googleAuthMsft["AppSecret"];
            // })
            //.AddTwitter(twitterOptions =>
            //{
            //    IConfigurationSection googleAuthTwt =
            //         Configuration.GetSection("Authentication:Twiter");
            //    twitterOptions.ConsumerKey = googleAuthTwt["AppId"];
            //    twitterOptions.ConsumerSecret = googleAuthTwt["AppSecret"];
            //    twitterOptions.RetrieveUserDetails = true;
            //});

            //.AddFacebook(facebookOptions =>
            //{
            //    facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
            //    facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
            //    facebookOptions.Events = new OAuthEvents()
            //    {
            //        OnRemoteFailure = loginFailureHandler =>
            //        {
            //            var authProperties = facebookOptions.StateDataFormat.Unprotect(loginFailureHandler.Request.Query["state"]);
            //            loginFailureHandler.Response.Redirect("/Identity/Account/Login");
            //            loginFailureHandler.HandleResponse();
            //            return Task.FromResult(0);
            //        }
            //    };
            //});

            //Modificar ACESSO NEGADO
            //services.ConfigureApplicationCookie(options =>
            //{
            //    options.AccessDeniedPath = new PathString("xxxx/AccessDenied");
            //});

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("AdminRolePolicy", policy => policy.RequireRole("Admin"));
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Post}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });
            
            //app.UseMvc();
        }
    }
}
