using System.Collections.Generic;
using WebStore.Domain.Entities;

namespace WebStore.Interfaces.Services
{
    public interface IEmployeesService
    {
        /// <summary>
        /// Получение списка сотрудников
        /// </summary>
        /// <returns></returns>
        IEnumerable<Employee> Get();

        /// <summary>
        /// Получение сотрудника по id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        Employee GetById(int id);

        /// <summary>
        /// Сохранить изменения
        /// </summary>
        void Commit();

        /// <summary>
        /// Добавить нового
        /// </summary>
        /// <param name="employee"></param>
        void Add(Employee employee);
        
        /// <summary>
        /// Редактирование сотрудника
        /// </summary>
        /// <param name="employee"></param>
        void Edit(Employee employee);

        /// <summary>
        /// Удалить
        /// </summary>
        /// <param name="id"></param>
        bool Delete(int id);
    }
}
