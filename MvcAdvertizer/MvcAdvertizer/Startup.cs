using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MvcAdvertizer.Config.Database;

using Microsoft.EntityFrameworkCore;
using MvcAdvertizer.Data.Interfaces;
using MvcAdvertizer.Data.Repositories;
using MvcAdvertizer.Services.Interfaces;
using MvcAdvertizer.Services.Implementations;
using AutoMapper;
using MvcAdvertizer.Config;

namespace MvcAdvertizer
{
    public class Startup
    {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {
            services.Configure<AppSettings>(Configuration);

            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(connection));

            services.AddAutoMapper(typeof(Startup).Assembly);

            services.AddHttpClient();

            services.AddMemoryCache();
            services.AddSession();

            services.AddSingleton<IRecaptchaService, RecaptchaService>();
            services.AddSingleton<IFileStorageService, FileStorageService>();

            services.AddTransient<IAdvertService, AdvertService>();
            services.AddTransient<IUserService, UserService>();

            services.AddScoped<IAdverts, AdvertRepository>();
            services.AddScoped<IUsers, UserRepository>();
            services.AddScoped<IUserAdvertsCounter, UserAdvertsCounterRepository>();

            services.AddRazorPages().AddMvcOptions(options => options.EnableEndpointRouting = false);

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();

            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(name: "createAdvert", template: "{controller=Advert}/{action=Create}");                
            });
        }
    }
}
