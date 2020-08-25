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
                Id = 1,
                FirstName = "Петр",
                SureName = "Сидоров",
                Patronymic = "Владимирович",
                Age = 30,
                Position = "Дизайнер"
            }
        };

        public IActionResult Index()
        {
            //return Content("Heelo from Controller");
            return View();
        }
        public IActionResult Employees()
        {
            return View(_employees);
        }
    }
}
