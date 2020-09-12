using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MvcAdvertizer.Config.Database;

using Microsoft.EntityFrameworkCore;
using MvcAdvertizer.Data.Interfaces;
using MvcAdvertizer.Data.Repositories;

namespace MvcAdvertizer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
                
        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(connection));

            services.AddTransient<IUsers, UserRepository>();
            services.AddTransient<IAdverts, AdvertRepository>();

            services.AddRazorPages().AddMvcOptions(options => options.EnableEndpointRouting = false);

            services.AddMvc();                
        }
                
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();           

            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(name: "createAdvert", template: "{controller=Advert}/{action=Create}/{id?}");
                
                routes.MapRoute(name: "advertDetails", template: "{controller=Advert}/{action=Details}/{id}");
                routes.MapRoute(name: "advertEdit", template: "{controller=Advert}/{action=Edit}/{id}");
                routes.MapRoute(name: "advertUpdate", template: "{controller=Advert}/{action=Update}/{id?}");

                routes.MapRoute(name: "advertShowImage", template: "{controller=Advert}/{action=ShowImage}/{id}");
            });
        }
    }
}
