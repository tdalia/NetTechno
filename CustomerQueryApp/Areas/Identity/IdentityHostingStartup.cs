using CustomerQueryData;
using CustomerQueryData.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(CustomerQueryApp.Areas.Identity.IdentityHostingStartup))]
namespace CustomerQueryApp.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<UserContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("CqtCrmDb")));    // UserContextConnection

                services.AddDefaultIdentity<ApplicationUser>()
                    .AddEntityFrameworkStores<UserContext>();
            });
        }
    }
}