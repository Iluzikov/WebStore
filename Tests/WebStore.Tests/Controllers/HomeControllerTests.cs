﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WebStore.Controllers;
using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void Index_Returns_View()
        {
            var controller = new HomeController();
            var result = controller.Index();

            Assert.IsType<ViewResult>(result);
        }

        [TestMethod]
        public void Blog_Returns_View()
        {
            var controller = new HomeController();

            var result = controller.Blog();

            Assert.IsType<ViewResult>(result);
        }

        [TestMethod]
        public void BlogSingle_Returns_View()
        {
            var controller = new HomeController();
            var result = controller.BlogSingle();

            Assert.IsType<ViewResult>(result);
        }

        [TestMethod]
        public void ContactUs_Returns_View()
        {
            var controller = new HomeController();

            var result = controller.ContactUs();

            Assert.IsType<ViewResult>(result);
        }

        [TestMethod]
        public void PageNotFound_Returns_View()
        {
            var controller = new HomeController();

            var result = controller.PageNotFound();

            Assert.IsType<ViewResult>(result);
        }

        [TestMethod]
        public void Cart_Returns_View()
        {
            var controller = new HomeController();

            var result = controller.Cart();

            Assert.IsType<ViewResult>(result);
        }

        [TestMethod]
        public void Login_Returns_View()
        {
            var controller = new HomeController();

            var result = controller.Login();

            Assert.IsType<ViewResult>(result);
        }

        [TestMethod, ExpectedException(typeof(ApplicationException))]
        public void Throw_thrown_ApplicationException()
        {
            var controller = new HomeController();

            const string exception_message = "";
            controller.Throw(exception_message);
        }

        [TestMethod]
        public void Throw_thrown_ApplicationException2()
        {
            var controller = new HomeController();

            Exception error = null;
            const string exception_message = "";
            try
            {
                controller.Throw(exception_message);
            }
            catch (Exception e)
            {
                error = e;
            }

            var application_exception = Assert.IsType<ApplicationException>(error);
            Assert.Equal($"Исключение: {exception_message}", application_exception.Message);
        }

        [TestMethod]
        public void Throw_thrown_ApplicationException3()
        {
            var controller = new HomeController();

            const string exception_message = "";

            var error = Assert.Throws<ApplicationException>(() => controller.Throw(exception_message));
            Assert.Equal($"Исключение: {exception_message}", error.Message);
        }


    }
}
