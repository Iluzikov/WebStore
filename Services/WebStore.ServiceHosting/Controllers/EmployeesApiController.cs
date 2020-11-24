using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    [Route(WebAPI.Employees)]
    [ApiController]
    public class EmployeesApiController : ControllerBase, IEmployeesService
    {
        private readonly IEmployeesService _employeesService;
        public EmployeesApiController(IEmployeesService employeesService)
        {
            _employeesService = employeesService;
        }

        [HttpPost]
        public void Add([FromBody] Employee employee)
        {
            _employeesService.Add(employee);
            Commit();
        }
        
        [NonAction]
        public void Commit() => _employeesService.Commit();
        
        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            var result = _employeesService.Delete(id);
            Commit();
            return result;
        }

        [HttpPut]
        public void Edit(Employee employee)
        {
            _employeesService.Edit(employee);
            Commit();
        }

        [HttpGet]
        public IEnumerable<Employee> Get()
        {
            return _employeesService.Get();
        }

        [HttpGet("{id}")]
        public Employee GetById(int id)
        {
            return _employeesService.GetById(id);
        }
    }
}
