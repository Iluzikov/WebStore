using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Clients.Base;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Clients.Employees
{
    public class EmployeesClient : BaseClient, IEmployeesService
    {
        public EmployeesClient(IConfiguration configuration) : base(configuration, WebAPI.Employees) { }

        public void Add(Employee employee) => Post(_serviceAddress, employee);

        public void Commit() { }

        public bool Delete(int id) => Delete($"{_serviceAddress}/{id}").IsSuccessStatusCode;

        public void Edit(Employee employee) => Put(_serviceAddress, employee);

        public IEnumerable<Employee> Get() => Get<IEnumerable<Employee>>(_serviceAddress);
        
        public Employee GetById(int id) => Get<Employee>($"{_serviceAddress}/{id}");
    }
}
