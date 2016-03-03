using System;
using System.Web.Mvc;
using BitCI.Controllers;
using BitCI.Models;
using NUnit.Framework;

namespace BitCI.Tests.Controllers
{
    [TestFixture]
    [Category("Backend")]
    [Category("Backend team")]
    [Category("Backendteam")]
    class DashboardsControllerTest
    {
        private DashboardsController _controller;

        [SetUp]
        public void Setup()
        {
            // Arrange
            _controller = new DashboardsController();
        }

        [Test]
        public void Index()
        {
            // Act
            ViewResult result = _controller.Index() as ViewResult;
            // Assert
            Assert.AreEqual("Dashboard opened!", result.ViewBag.Message);
        }

        [Test]
        public void Create()
        {
            // Act
            ViewResult result = _controller.Create() as ViewResult;
            // Assert
            Assert.AreEqual("Dashboard created!", result.ViewBag.Message);
        }

        [Test]
        public void Create_Should_Throw_Exception_When_Invalid_Dashboard_Is_Passed()
        {
            var result = _controller.Create(new Dashboard());
            // Assert
            Assert.AreEqual("RedirectToRouteResult", result.GetType().Name);
        }

        [Test]
        public void Create_Should_Throw_Exception_When_Null_Dashboard_Is_Passed()
        {
            Assert.Throws(typeof(ArgumentNullException), () => _controller.Create(null));
        }

        [Test]
        public void Details()
        {
            Assert.DoesNotThrow( () => _controller.Details(1));
        }

        [Test]
        public void Details_Should_Throw_Exception_When_Null_Is_Passed()
        {
            var response = _controller.Details(null);
            Assert.AreEqual("HttpStatusCodeResult", response.GetType().Name);
        }

        [Test]
        public void Details_Should_Throw_Exception_When_Dashboard_Is_Not_Found()
        {
            var response = _controller.Details(-1);
            Assert.AreEqual("HttpNotFoundResult", response.GetType().Name);
        }

        [Test]
        public void Edit()
        {
            Assert.DoesNotThrow(() => _controller.Edit(1));
        }

        [Test]
        public void Edit_Should_Throw_Exception_When_Dashboard_Is_Not_Found()
        {
            var result = _controller.Edit(-1);
            Assert.AreEqual("HttpNotFoundResult", result.GetType().Name);
        }

        [Test]
        public void Edit_Should_Throw_Exception_When_Dashboard_Is_Null()
        {
            Dashboard dashboard = null;
            Assert.Throws(typeof(ArgumentNullException), () => _controller.Edit(dashboard));
        }

    }
}
