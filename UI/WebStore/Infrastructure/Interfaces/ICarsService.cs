using System.Collections.Generic;
using WebStore.Domain.ViewModels;

namespace WebStore.Infrastructure.Interfaces
{
    public interface ICarsService
    {
        /// <summary>
        /// Посмотреть весь список
        /// </summary>
        /// <returns></returns>
        IEnumerable<CarViewModel> GetAll();

        /// <summary>
        /// Детализация по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        CarViewModel GetById(int id);

        /// <summary>
        /// Добавить
        /// </summary>
        /// <param name="model"></param>
        void AddNew(CarViewModel model);

        /// <summary>
        /// Удалить
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);
    }
}
