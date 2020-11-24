using System;
using System.Collections.Generic;
using WebStore.DAL;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Services.Products.InSQL
{
    public class SqlEmployeeService : IEmployeesService
    {
        private readonly WebStoreContext _context;

        public SqlEmployeeService(WebStoreContext context) => _context = context;

        public void Add(Employee employee)
        {
            if(employee is null) throw new ArgumentNullException(nameof(employee));
            _context.Add(employee);
        }

        public void Commit() => _context.SaveChanges();

        public bool Delete(int id)
        {
            var employee = GetById(id);
            if (employee is null) return false;
            _context.Remove(employee);
            return true;
        }

        public void Edit(Employee employee)
        {
            if (employee is null) throw new ArgumentNullException(nameof(employee));
            _context.Update(employee);
        }

        public IEnumerable<Employee> Get() => _context.Employees;

        public Employee GetById(int id) => _context.Employees.Find(id);
    }
}
