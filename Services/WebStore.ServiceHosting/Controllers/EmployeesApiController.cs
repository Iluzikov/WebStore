using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    /// <summary>API управления сотрудниками</summary>
    [Route(WebAPI.Employees)]
    [ApiController]
    public class EmployeesApiController : ControllerBase, IEmployeesService
    {
        private readonly IEmployeesService _employeesService;
        public EmployeesApiController(IEmployeesService employeesService) =>
            _employeesService = employeesService;

        /// <summary>Добавление сотрудника</summary>
        /// <param name="employee">Новый сотрудник</param>
        [HttpPost]
        public void Add([FromBody] Employee employee)
        {
            _employeesService.Add(employee);
            Commit();
        }
        
        [NonAction]
        public void Commit() => _employeesService.Commit();
        
        /// <summary>Удаление сотрудника</summary>
        /// <param name="id">Идентификатор сотрудника</param>
        /// <returns>Истина если удаление успешно</returns>
        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            var result = _employeesService.Delete(id);
            Commit();
            return result;
        }

        /// <summary>Редактирование сотрудника</summary>
        /// <param name="employee">Сотрудник</param>
        [HttpPut]
        public void Edit(Employee employee)
        {
            _employeesService.Edit(employee);
            Commit();
        }

        /// <summary>Получение списка всех сотрудников</summary>
        /// <returns>Список всех сотрудников</returns>
        [HttpGet]
        public IEnumerable<Employee> Get() => _employeesService.Get();

        /// <summary>Получение сотрудника по идентификатору</summary>
        /// <param name="id">Идентификатор сотрудника</param>
        /// <returns>Сотрудник</returns>
        [HttpGet("{id}")]
        public Employee GetById(int id) => _employeesService.GetById(id);
    }
}
