using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class CarController : Controller
    {
        private readonly List<CarViewModel> _cars = new List<CarViewModel>
        {
            new CarViewModel
            {
                Id = 1,
                Brand = "Audi",
                Model = "A4",
                Engine = "Б/2,0/96Kw",
                CarBody = "седан",
                ReleaseYear = 2004
            },
            new CarViewModel
            {
                Id = 2,
                Brand = "BMW",
                Model = "525i",
                Engine = "Б/2,5/141Kw",
                CarBody = "седан",
                ReleaseYear = 2005
            },
            new CarViewModel
            {
                Id = 3,
                Brand = "Mazda",
                Model = "MPV 2.5 TD",
                Engine = "Д/2,5/85Kw",
                CarBody = "минивэн",
                ReleaseYear = 2000
            },
            new CarViewModel
            {
                Id = 4,
                Brand = "Volvo",
                Model = "XC90 2.4 D5",
                Engine = "Д/2,4/120Kw",
                CarBody = "универсал",
                ReleaseYear = 2003
            }
        };

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Cars()
        {
            return View(_cars);
        }
        public IActionResult CarDetails(int id)
        {
            var car = _cars.FirstOrDefault(x => x.Id == id);
            if (car == null)
                return NotFound();
            return View(car);
        }
    }
}
