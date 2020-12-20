using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using WebStore.Controllers;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class EmployeeControllerTests
    {
        private Mock<IEmployeesService> _employeeServiceMock;

        [TestInitialize]
        public void TestInitialize()
        {
            Employee _employee = new Employee { Id=1, Name="Name 1",Surname="Surname 1" };

            _employeeServiceMock = new Mock<IEmployeesService>();
            _employeeServiceMock
                .Setup(c => c.GetById(It.IsAny<int>()))
                .Returns(_employee);
        }

        [TestMethod]
        public void Details_Returns_CorrectView()
        {
            const int expected_employee_id = 1;

            var expected_name = $"Name {expected_employee_id}";
            var expected_surname = $"Surname {expected_employee_id}";

            var controller = new EmployeeController(_employeeServiceMock.Object);

            var result = controller.EmployeeDetails(expected_employee_id);

            var view_result = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<EmployeeViewModel>(view_result.Model);

            Assert.Equal(expected_employee_id, model.Id);
            Assert.Equal(expected_name, model.FirstName);
            Assert.Equal(expected_surname, model.LastName);
        }

    }
}
