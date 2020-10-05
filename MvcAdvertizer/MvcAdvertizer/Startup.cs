using Microsoft.AspNetCore.Builder;
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
using MvcAdvertizer.Config.Middlewares;

namespace MvcAdvertizer
{
    public class Startup
    {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {

            LoadAppSettings(services);
            LoadCommonServices(services);
            LoadDomainServices(services);
            LoadDomainRepositories(services);

            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(connection));

            services.AddHttpClient();

            services.AddMemoryCache();
            services.AddSession();

            services.AddRazorPages().AddMvcOptions(options => options.EnableEndpointRouting = false);

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app) {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();

            app.UseSession();

            app.UseMiddleware<ImageResizeMiddleware>();

            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(name: "createAdvert", template: "{controller=Advert}/{action=Create}");
            });
        }

        private void LoadAppSettings(IServiceCollection services) {

            services.Configure<FileStorageSettings>(Configuration.GetSection("FileStorageSettings"));
            services.Configure<RecaptchaSettings>(Configuration.GetSection("RecaptchaSettings"));
            services.Configure<UsersAdvertsSettings>(Configuration.GetSection("UsersAdvertsSettings"));
        }

        private void LoadDomainRepositories(IServiceCollection services) {

            services.AddScoped<IAdverts, AdvertRepository>();
            services.AddScoped<IUsers, UserRepository>();
            services.AddScoped<IUserAdvertsCounter, UserAdvertsCounterRepository>();
        }

        private void LoadDomainServices(IServiceCollection services) {

            services.AddTransient<IAdvertService, AdvertService>();
            services.AddTransient<IUserService, UserService>();
        }

        private void LoadCommonServices(IServiceCollection services) {

            services.AddAutoMapper(typeof(Startup).Assembly);
            services.AddSingleton<IRecaptchaService, RecaptchaService>();
            services.AddSingleton<IFileStorageService, FileStorageService>();
        }
    }
}
