using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
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
        private readonly RoleManager<IdentityRole> _RoleManager;

        public WebStoreDBInitializer(WebStoreContext db, UserManager<User> UserManager, RoleManager<IdentityRole> RoleManager)
        {
            _db = db;
            _UserManager = UserManager;
            _RoleManager = RoleManager;
        }

        public void Initialize()
        {
            var db = _db.Database;

            db.Migrate();

            InitializeProducts();
            InitializeEmployees();
            InitializeIdentityAsync().Wait();
        }

        private void InitializeProducts()
        {
            if (_db.Products.Any()) return;

            var db = _db.Database;
            using (db.BeginTransaction())
            {
                _db.Categories.AddRange(TestData.Categories);

                db.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Categories] ON");
                _db.SaveChanges();
                db.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Categories] OFF");

                db.CommitTransaction();
            }

            using (db.BeginTransaction())
            {
                _db.Brands.AddRange(TestData.Brands);

                db.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Brands] ON");
                _db.SaveChanges();
                db.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Brands] OFF");

                db.CommitTransaction();
            }

            using (db.BeginTransaction())
            {
                _db.Products.AddRange(TestData.Products);

                db.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Products] ON");
                _db.SaveChanges();
                db.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Products] OFF");

                db.CommitTransaction();
            }
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
            async Task CheckRoleExist(string RoleName)
            {
                if (!await _RoleManager.RoleExistsAsync(RoleName))
                    await _RoleManager.CreateAsync(new Role { Name = RoleName });
            }

            await CheckRoleExist(Role.Administrator);
            await CheckRoleExist(Role.User);

            if (await _UserManager.FindByNameAsync(User.Administrator) is null)
            {
                var admin = new User { UserName = User.Administrator };
                var creation_result = await _UserManager.CreateAsync(admin, User.DefaultAdminPassword);
                if (creation_result.Succeeded)
                    await _UserManager.AddToRoleAsync(admin, Role.Administrator);
                else
                {
                    var errors = creation_result.Errors.Select(e => e.Description);
                    throw new InvalidOperationException($"Ошибка при создании пользователя Администратор: {string.Join(", ", errors)}");
                }
            }
        }
    }
}
