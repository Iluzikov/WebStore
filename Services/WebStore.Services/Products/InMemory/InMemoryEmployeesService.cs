using System;
using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Services.Products.InMemory
{
    public class InMemoryEmployeesService : IEmployeesService
    {
        private readonly List<Employee> _employees = new List<Employee>
        {
            new Employee
            {
                Id = 1,
                Name = "Иван",
                Surname = "Иванов",
                Patronymic = "Иванович",
                Age = 22,
                EmployementDate = DateTime.Now
            },
            new Employee
            {
                Id = 2,
                Name = "Владислав",
                Surname = "Петров",
                Patronymic = "Иванович",
                Age = 35,
                EmployementDate = DateTime.Now
            }
        };

        public void Add(Employee employee)
        {
            if (employee is null)
                throw new ArgumentNullException(nameof(employee));
            employee.Id = _employees.Max(e => e.Id) + 1;
            _employees.Add(employee);
        }

        public void Commit() { }

        public bool Delete(int id)
        {
            var employee = GetById(id);
            if (employee is null)
                return false;

            _employees.Remove(employee);
            return true;
        }

        public IEnumerable<Employee> Get() => _employees;

        public void Edit(Employee employee)
        {
            if (employee is null)
                throw new ArgumentNullException(nameof(employee));

            if (_employees.Contains(employee)) return;

            var db_employee = GetById(employee.Id);
            if (db_employee is null) return;

            db_employee.Name = employee.Name;
            db_employee.Surname = employee.Surname;
            db_employee.Patronymic = employee.Patronymic;
            db_employee.Age = employee.Age;
        }

        public Employee GetById(int id) => _employees.FirstOrDefault(e => e.Id.Equals(id));
        
    }
}
