using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(MeninoDev.Areas.Identity.IdentityHostingStartup))]
namespace MeninoDev.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}