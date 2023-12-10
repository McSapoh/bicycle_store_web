using bicycle_store_web.Interfaces;
using bicycle_store_web.Models;
using bicycle_store_web.Repositories;
using bicycle_store_web.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Globalization;
using System.IO;

namespace bicycle_store_web
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
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Information()
				.WriteTo.Console()
			    .WriteTo.File(Path.Combine(Directory.GetCurrentDirectory(), "logs.txt"), rollingInterval: RollingInterval.Month)
			.CreateLogger();
            services.AddLocalization(oprions =>
            {
                oprions.ResourcesPath = "Resources";
            });
            services.Configure<RequestLocalizationOptions>(options => { 
                var supportedCultures = new[] { new CultureInfo("en"), new CultureInfo("uk"), }; 
                options.DefaultRequestCulture = new RequestCulture("en"); 
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures; 
            });
            //var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));
            //services.AddDbContext<bicycle_storeContext>(options => 
            //    options.UseMySql(Configuration.GetConnectionString("mssql"), serverVersion));
            services.AddDbContext<bicycle_storeContext>(options =>
					options.UseSqlServer(Configuration.GetConnectionString("mssql")));

			services.AddControllersWithViews()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization(options => {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                        factory.Create(typeof(SharedResource));
                });

            // Adding services
            services.AddScoped<IBicycleService, BicycleService>();
            services.AddScoped<ITypeService, TypeService>();
            services.AddScoped<IProducerService, ProducerService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IShoppingCartService, ShoppingCartService>();
            services.AddScoped<IShoppingCartOrderService, ShoppingCartOrderService>();
            services.AddScoped<IOrderService, OrderService>();

            // Adding repositories
            services.AddScoped<IBicycleRepository, BicycleRepository>();
            services.AddScoped<IBicycleOrderRepository, BicycleOrderRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IProducerRepository, ProducerRepository>();
            services.AddScoped<IShoppingCartOrderRepository, ShoppingCartOrderRepository>();
            services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
            services.AddScoped<ITypeRepository, TypeRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => 
                    {
                        options.LoginPath = "/User/Login";
                        options.ExpireTimeSpan = TimeSpan.FromHours(8);
                    }
                );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRequestLocalization();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Admin/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see
                // https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    //pattern: "{controller=Admin}/{action=Index}/{id?}");
                    pattern: "{controller=User}/{action=Bicycles}/{id?}");
            });  
        }
    }
}
