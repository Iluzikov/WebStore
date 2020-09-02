using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Infrastructure.Interfaces;
using WebStore.ViewModels;

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

        [Route("all")]
        public IActionResult Cars()
        {
            return View(_carsService.GetAll());
        }

        [Route("{id}")]
        /// <summary>
        /// Детализация по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult CarDetails(int id)
        {
            var car = _carsService.GetById(id);
            if (car == null)
                return NotFound();
            return View(car);
        }

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

        [HttpPost]
        [Route("edit/{id?}")]
        public IActionResult Edit(CarViewModel carModel)
        {
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

        public IActionResult Delete(int id)
        {
            _carsService.Delete(id);

            return RedirectToAction(nameof(Cars));
        }



    }
}
