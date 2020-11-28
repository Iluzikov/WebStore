using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IEmployeesService _employeesService;

        public EmployeeController(IEmployeesService employeesService) => _employeesService = employeesService;
        

        [AllowAnonymous]
        /// <summary>
        /// Выдает список всех сотрудников
        /// </summary>
        /// <returns></returns>
        public IActionResult Employees() => View(_employeesService.Get().ToView());
        
        //[Authorize(Roles = "Admins, Users")]
        /// <summary>
        /// Детализация сотрудника по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult EmployeeDetails(int id)
        {
            //Получаем сотрудника по Id
            var employee = _employeesService.GetById(id);

            //Если такого не существует
            if (employee == null)
                return NotFound(); // возвращаем результат 404 Not Found

            //Иначе возвращаем сотрудника
            return View(employee.ToView());
        }

        #region Edit

        [HttpGet]
        [Authorize(Roles = Role.Administrator)]
        public IActionResult Edit(int? id)
        {
            if (id is null) return View(new EmployeeViewModel());
            if (id < 0) return BadRequest();

            var employee = _employeesService.GetById(id.Value);
            if (employee == null) return NotFound();

            return View(employee.ToView());
        }

        [HttpPost]
        [Authorize(Roles = Role.Administrator)]
        public IActionResult Edit(EmployeeViewModel model)
        {
            if (model is null) throw new ArgumentNullException(nameof(model));

            if (model.Age < 18 || model.Age > 100)
                ModelState.AddModelError(nameof(Employee.Age), "Введен некорректный возраст");

            //Проверка модели на валидность
            if (!ModelState.IsValid) return View(model);

            if (model.Id == 0)
                _employeesService.Add(model.FromView());
            else
                _employeesService.Edit(model.FromView());

            _employeesService.Commit();
            return RedirectToAction(nameof(Employees));
        }

        #endregion

        #region Delete

        [Authorize(Roles = Role.Administrator)]
        public IActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest();

            var employee = _employeesService.GetById(id);
            if (employee is null) return NotFound();

            return View(employee.ToView());
        }

        [HttpPost]
        [Authorize(Roles = Role.Administrator)]
        public IActionResult DeleteConfirmed(int id)
        {
            _employeesService.Delete(id);
            _employeesService.Commit();

            return RedirectToAction(nameof(Employees));
        }

        #endregion
    }
}
