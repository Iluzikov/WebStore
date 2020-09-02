using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

    }
}
