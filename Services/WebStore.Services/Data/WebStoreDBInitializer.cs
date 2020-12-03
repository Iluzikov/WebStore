using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL;
using WebStore.Domain.Entities.Identity;

namespace WebStore.Services.Data
{
    public class WebStoreDBInitializer
    {
        private readonly WebStoreContext _db;
        private readonly UserManager<User> _UserManager;
        private readonly RoleManager<Role> _RoleManager;
        private readonly ILogger<WebStoreDBInitializer> _logger;

        public WebStoreDBInitializer(
            WebStoreContext db,
            UserManager<User> UserManager,
            RoleManager<Role> RoleManager,
            ILogger<WebStoreDBInitializer> logger)
        {
            _db = db;
            _UserManager = UserManager;
            _RoleManager = RoleManager;
            _logger = logger;
        }

        public void Initialize()
        {
            var timer = Stopwatch.StartNew();
            _logger.LogInformation("Инициализация БД...");
            var db = _db.Database;
            if (db.GetPendingMigrations().Any())
            {
                _logger.LogInformation("Миграция БД выполнена успешно {0}мс", timer.ElapsedMilliseconds);
                db.Migrate();
            }
            else
                _logger.LogInformation("Миграция БД не требуется");

            InitializeProducts();
            InitializeEmployees();
            InitializeIdentityAsync().Wait();

            _logger.LogInformation("Инициализация Бд выполнена успешно {0:0.###}c", timer.Elapsed.TotalSeconds);
        }

        private void InitializeProducts()
        {
            var timer = Stopwatch.StartNew();

            _logger.LogInformation("Инициализация каталога товаров...");
            if (_db.Products.Any()) 
            {
                _logger.LogInformation("Инициализация каталога товаров не требуется");
                return;
            }

            var db = _db.Database;
            using (db.BeginTransaction())
            {
                _db.Categories.AddRange(TestData.Categories);

                db.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Categories] ON");
                _db.SaveChanges();
                db.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Categories] OFF");

                db.CommitTransaction();
            }
            _logger.LogInformation("Инициализация категорий выполнена {0}мс",timer.ElapsedMilliseconds);


            using (db.BeginTransaction())
            {
                _db.Brands.AddRange(TestData.Brands);

                db.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Brands] ON");
                _db.SaveChanges();
                db.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Brands] OFF");

                db.CommitTransaction();
            }
            _logger.LogInformation("Инициализация брендов выполнена {0}мс", timer.ElapsedMilliseconds);

            using (db.BeginTransaction())
            {
                _db.Products.AddRange(TestData.Products);

                db.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Products] ON");
                _db.SaveChanges();
                db.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Products] OFF");

                db.CommitTransaction();
            }
            _logger.LogInformation("Инициализация товаров выполнена {0}мс", timer.ElapsedMilliseconds);
        }

        private void InitializeEmployees()
        {
            if (_db.Employees.Any()) return;

            using (_db.Database.BeginTransaction())
            {
                TestData.Employees.ForEach(employee => employee.Id = 0);

                _db.Employees.AddRange(TestData.Employees);

                _db.SaveChanges();

                _db.Database.CommitTransaction();
            }
        }

        private async Task InitializeIdentityAsync()
        {
            _logger.LogInformation("Инициализация Identity...");
            var timer = Stopwatch.StartNew();
            async Task CheckRoleExist(string RoleName)
            {
                if (!await _RoleManager.RoleExistsAsync(RoleName))
                {
                    _logger.LogInformation("Додавление роли {0} {1} мс", RoleName, timer.ElapsedMilliseconds);
                    await _RoleManager.CreateAsync(new Role { Name = RoleName });
                }
            }

            await CheckRoleExist(Role.Administrator);
            await CheckRoleExist(Role.User);

            if (await _UserManager.FindByNameAsync(User.Administrator) is null)
            {
                _logger.LogInformation("Добавление администратора...");
                var admin = new User { UserName = User.Administrator };
                var creation_result = await _UserManager.CreateAsync(admin, User.DefaultAdminPassword);
                if (creation_result.Succeeded)
                {
                    _logger.LogInformation("Добавление администратора выполнено успешно");
                    var role_arr_result = await _UserManager.AddToRoleAsync(admin, Role.Administrator);
                    if (role_arr_result.Succeeded)
                        _logger.LogInformation("Добавление администратору роли Администратор, выполнено успешно");
                    else
                    {
                        var error = string.Join(",", role_arr_result.Errors.Select(e => e.Description));
                        _logger.LogError("Ошибка при добавлении администратору роли Администратор {0}", error);
                        throw new InvalidOperationException($"Ошибка при добавлении администратору роли Администратор {error}");
                    }
                }
                else
                {
                    var error = string.Join(",", creation_result.Errors.Select(e => e.Description));
                    _logger.LogError("Ошибка при создании пользователя Администратор {0}", error);
                    throw new InvalidOperationException($"Ошибка при создании пользователя Администратор: {string.Join(", ", error)}");
                }
            }
        }
    }
}
