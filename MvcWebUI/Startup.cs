using System;
using AppCore.Business.Utils;
using AppCore.Business.Utils.Bases;
using AppCore.DataAccess.Configs;
using Business.Services;
using Business.Services.Bases;
using DataAccess.EntityFramework.Contexts;
using DataAccess.EntityFramework.Repositories;
using DataAccess.EntityFramework.Repositories.Bases;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MvcWebUI.Settings;

namespace MvcWebUI
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
            services.AddControllersWithViews();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(config =>
                {
                    config.LoginPath = "/Accounts/Login";
                    config.AccessDeniedPath = "/Accounts/AccessDenied";
                    config.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    config.SlidingExpiration = true;
                });

            //services.AddSession();
            services.AddSession(config =>
            {
                config.IdleTimeout = TimeSpan.FromMinutes(30); // default: 20 dakika
            });

            #region IoC Container
            // IoC Container kütüphaneleri: Autofac ve Ninject
            // services.AddScoped() // istek (request) boyunca objenin referansýný (genelde interface veya abstract class) kullandýðýmýz yerde obje (somut class'tan oluþturulacak) bir kere oluþturulur ve yanýt (response) dönene kadar bu obje hayatta kalýr.
            // services.AddSingleton() // web uygulamasý baþladýðýnda objenin referansýný (genelde interface veya abstract class) kullandýðýmýz yerde obje (somut class'tan oluþuturulacak) bir kere oluþturulur ve uygulama çalýþtýðý (IIS üzerinden uygulama durdurulmadýðý veya yeniden baþlatýlmadýðý) sürece bu obje hayatta kalýr.
            // services.AddTransient() // istek (request) baðýmsýz ihtiyaç olan objenin referansýný (genelde interface veya abstract class) kullandýðýmýz her yerde bu objeyi new'ler.

            ConnectionConfig.ConnectionString = Configuration.GetConnectionString("ETradeContext");

            // Unable to resolve service hatalarý burada giderilir!
            services.AddScoped<DbContext, ETradeContext>();
            services.AddScoped<ProductRepositoryBase, ProductRepository>();
            services.AddScoped<CategoryRepositoryBase, CategoryRepository>();
            services.AddScoped<UserRepositoryBase, UserRepository>();
            services.AddScoped<CountryRepositoryBase, CountryRepository>();
            services.AddScoped<CityRepositoryBase, CityRepository>();
            services.AddScoped<RoleRepositoryBase, RoleRepository>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IAccountService, AccountService>();

            // ASP.NET Core kullanýmý:
            //IConfigurationSection section = Configuration.GetSection("AppSettings");
            //AppSettings appSettings = new AppSettings();
            //section.Bind(appSettings);

            // AppCore üzerinden kullaným:
            AppSettingsUtilBase appSettingsUtil = new AppSettingsUtil(Configuration);
            appSettingsUtil.Bind<AppSettings>();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Sen kimsin?
            app.UseAuthentication();

            // Sen iþlem için yetkili misin?
            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
