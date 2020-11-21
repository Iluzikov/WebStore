using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.ViewModels;
using WebStore.Infrastructure.Interfaces;

namespace WebStore.Controllers
{
    [Route("cars")]
    public class CarController : Controller
    {
        private readonly ICarsService _carsService;

        public CarController(ICarsService cars)
        {
            _carsService = cars;
        }

        /// <summary>
        /// Показать весь список
        /// </summary>
        /// <returns></returns>
        [Route("all")]
        public IActionResult Cars()
        {
            return View(_carsService.GetAll());
        }

        /// <summary>
        /// Детализация по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id}")]
        public IActionResult CarDetails(int id)
        {
            var car = _carsService.GetById(id);
            if (car == null)
                return NotFound();
            return View(car);
        }

        /// <summary>
        /// Редактирование получение
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("edit/{id?}")]
        public IActionResult Edit(int? id)
        {
            if (!id.HasValue)
                return View(new CarViewModel());

            var model = _carsService.GetById(id.Value);
            if (model == null)
                return NotFound();

            return View(model);
        }

        /// <summary>
        /// Редактирование отправка
        /// </summary>
        /// <param name="carModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("edit/{id?}")]
        public IActionResult Edit(CarViewModel carModel)
        {
            // проверка модели на валидность
            if (!ModelState.IsValid)
                return View(carModel);

            if (carModel.Id > 0) // если есть Id, то редактируем модель
            {
                var dbItem = _carsService.GetById(carModel.Id);

                if (ReferenceEquals(dbItem, null))
                    return NotFound();// возвращаем результат 404 Not Found

                dbItem.Brand = carModel.Brand;
                dbItem.Model = carModel.Model;
                dbItem.Engine = carModel.Engine;
                dbItem.ReleaseYear = carModel.ReleaseYear;
                dbItem.CarBody = carModel.CarBody;
            }
            else // иначе добавляем модель в список
            {
                _carsService.AddNew(carModel);
            }
            return RedirectToAction(nameof(Cars));
        }

        /// <summary>
        /// Удалить
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Delete(int id)
        {
            _carsService.Delete(id);

            return RedirectToAction(nameof(Cars));
        }



    }
}
