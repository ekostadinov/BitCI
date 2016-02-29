using System.Web.Mvc;
using BitCI.Controllers;
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
    }
}
