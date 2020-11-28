using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using WebStore.Clients.Employees;
using WebStore.Clients.Orders;
using WebStore.Clients.Products;
using WebStore.Clients.Values;
using WebStore.DAL;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces.Services;
using WebStore.Interfaces.TestApi;
using WebStore.Services.Products.IcCookies;
using WebStore.Services.Products.InMemory;
using WebStore.Services.Data;
using WebStore.Clients.Identity;

namespace WebStore
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration) => _configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            //Подключаем идентификацию
            services.AddIdentity<User, Role>()
                .AddDefaultTokenProviders();
            //.AddEntityFrameworkStores<WebStoreContext>()

            #region Custom Identity clients stores

            services
               .AddTransient<IUserStore<User>, UsersClient>()
               .AddTransient<IUserRoleStore<User>, UsersClient>()
               .AddTransient<IUserPasswordStore<User>, UsersClient>()
               .AddTransient<IUserEmailStore<User>, UsersClient>()
               .AddTransient<IUserPhoneNumberStore<User>, UsersClient>()
               .AddTransient<IUserTwoFactorStore<User>, UsersClient>()
               .AddTransient<IUserClaimStore<User>, UsersClient>()
               .AddTransient<IUserLoginStore<User>, UsersClient>();

            services
               .AddTransient<IRoleStore<Role>, RolesClient>();

            #endregion

            services.Configure<IdentityOptions>(opt =>
            {
#if DEBUG
                opt.Password.RequiredLength = 3;
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredUniqueChars = 3;

#endif
                opt.User.RequireUniqueEmail = false;
                opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

                opt.Lockout.AllowedForNewUsers = true;
                opt.Lockout.MaxFailedAccessAttempts = 10;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
            });
            services.ConfigureApplicationCookie(options => // необязательно
            {
                // Cookie settings
                options.Cookie.Name = "WebStore-GB";
                options.Cookie.HttpOnly = true;
                //options.Cookie.Expiration = TimeSpan.FromDays(10);
                options.LoginPath = "/Account/Login"; // If the LoginPath is not set here, ASP.NET Core will default to /Account/Login
                options.LogoutPath = "/Account/Logout"; // If the LogoutPath is not set here, ASP.NET Core will default to /Account/Logout
                options.AccessDeniedPath = "/Account/AccessDenied"; // If the AccessDeniedPath is not set here, ASP.NET Core will default to /Account/AccessDenied
                options.SlidingExpiration = true;
            });

            services.AddMvc();

            //services.AddDbContext<WebStoreContext>(options => options
            //    .UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));

            //services.AddTransient<WebStoreDBInitializer>();

            services.AddScoped<IEmployeesService, EmployeesClient>();
            services.AddSingleton<ICarsService, InMemoryCarsService>();

            services.AddScoped<IProductService, ProductsClient>(); //меняем реализацию на ProductsClient
            services.AddScoped<ICartService, CoocieCartService>();
            services.AddScoped<IOrderService, OrdersClient>();

            services.AddTransient<IValuesService, ValuesClient>();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env/*, WebStoreDBInitializer db*/)
        {
            //db.Initialize();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseWelcomePage("/welcome");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("areas", "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
