using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IEmployeesService _employeesService;

        public EmployeeController(IEmployeesService employeesService)
        {
            _employeesService = employeesService;
        }

        [AllowAnonymous]
        /// <summary>
        /// Выдает список всех сотрудников
        /// </summary>
        /// <returns></returns>
        public IActionResult Employees()
        {
            return View(_employeesService.GetAll());
        }

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
            return View(employee);
        }

        [HttpGet]
        [Authorize(Roles = WebStoreRole.Admins)]
        public IActionResult Edit(int? id)
        {
            if (!id.HasValue)
                return View(new EmployeeViewModel());

            var model = _employeesService.GetById(id.Value);
            if (model == null)
                return NotFound();

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = WebStoreRole.Admins)]
        public IActionResult Edit(EmployeeViewModel model)
        {
            if (model.Age < 18 || model.Age > 100)
                ModelState.AddModelError("Age", "Введен некорректный возраст");

            //Проверка модели на валидность
            if (!ModelState.IsValid)
                return View(model);

            if (model.Id > 0) // если есть Id, то редактируем модель
            {
                var dbItem = _employeesService.GetById(model.Id);

                if (ReferenceEquals(dbItem, null))
                    return NotFound();// возвращаем результат 404 Not Found

                dbItem.FirstName = model.FirstName;
                dbItem.SurName = model.SurName;
                dbItem.Age = model.Age;
                dbItem.Patronymic = model.Patronymic;
                dbItem.Position = model.Position;
            }
            else // иначе добавляем модель в список
            {
                _employeesService.AddNew(model);
            }
            _employeesService.Commit(); // станет актуальным позднее (когда добавим БД)

            return RedirectToAction(nameof(Employees));
        }

        [Authorize(Roles = WebStoreRole.Admins)]
        public IActionResult Delete(int id)
        {
            _employeesService.Delete(id);

            return RedirectToAction(nameof(Employees));
        }

    }
}
