using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly List<EmployeeViewModel> _employees = new List<EmployeeViewModel>
        {
            new EmployeeViewModel
            {
                Id = 1,
                FirstName = "Иван",
                SureName = "Иванов",
                Patronymic = "Иванович",
                Age = 35,
                Position = "Начальник отдела"
            },
            new EmployeeViewModel
            {
                Id = 2,
                FirstName = "Петр",
                SureName = "Сидоров",
                Patronymic = "Владимирович",
                Age = 30,
                Position = "Дизайнер"
            },
            new EmployeeViewModel
            {
                Id = 3,
                FirstName = "Эдуард",
                SureName = "Суровый",
                Patronymic = "Вениаминович",
                Age = 30,
                Position = "Бард"
            }
        };

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Blog()
        {
            return View();
        }

        public IActionResult Employees()
        {
            return View(_employees);
        }
        public IActionResult EmployeeDetails(int id)
        {
            var employee = _employees.FirstOrDefault(x => x.Id == id);
            if (employee == null)
                return NotFound();

            return View(employee);
        }
    }
}
