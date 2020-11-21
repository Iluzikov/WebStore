using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.ViewModels;
using WebStore.Infrastructure.Interfaces;

namespace WebStore.Infrastructure.Services
{
    public class InMemoryCarsService : ICarsService
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

        public void AddNew(CarViewModel model)
        {
            model.Id = _cars.Max(e => e.Id) + 1;
            _cars.Add(model);
        }

        public void Delete(int id)
        {
            var car = GetById(id);
            if (car is null)
                return;
            _cars.Remove(car);
        }

        public IEnumerable<CarViewModel> GetAll()
        {
            return _cars;
        }

        public CarViewModel GetById(int id)
        {
            return _cars.FirstOrDefault(e => e.Id.Equals(id));
        }
    }
}
