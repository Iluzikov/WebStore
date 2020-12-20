using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Controllers;
using Assert = Xunit.Assert;
using WebStore.Interfaces.TestApi;
using System.Collections.Generic;
using System.Linq;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class WebApiControllerTests
    {
        [TestMethod]
        public void Index_Returns_View_with_Values()
        {
            var expected_values = new[] { "1", "2", "3" };
            var values_service_mock = new Mock<IValuesService>();
            values_service_mock
                .Setup(s => s.Get())
                .Returns(expected_values);

            // Режим "Стаб"
            var controller = new WebApiController(values_service_mock.Object);
            var result = controller.Index();

            var view_result = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<string>>(view_result.Model);

            Assert.Equal(expected_values.Length, model.Count());

            // Режим "Мок"
            values_service_mock.Verify(s => s.Get());
            values_service_mock.VerifyNoOtherCalls();
        }
    }
}
