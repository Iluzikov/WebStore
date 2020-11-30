using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;

namespace WebStore.Services.Mapping
{
    public static class EmployeeMapper
    {
        public static EmployeeViewModel ToView(this Employee employee) => new EmployeeViewModel
        {
            Id = employee.Id,
            FirstName = employee.Name,
            LastName = employee.Surname,
            Patronymic = employee.Patronymic,
            Age = employee.Age,
            EmployementDate = employee.EmployementDate
        };

        public static IEnumerable<EmployeeViewModel> ToView(this IEnumerable<Employee> employees) => employees.Select(ToView);

        public static Employee FromView(this EmployeeViewModel Model) => new Employee
        {
            Id = Model.Id,
            Surname = Model.LastName,
            Name = Model.FirstName,
            Patronymic = Model.Patronymic,
            Age = Model.Age,
            EmployementDate = Model.EmployementDate
        };
    }
}
