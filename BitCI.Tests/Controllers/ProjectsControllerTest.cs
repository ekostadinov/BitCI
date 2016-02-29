using System.Web.Mvc;
using BitCI.Controllers;
using NUnit.Framework;

namespace BitCI.Tests.Controllers
{
    [TestFixture]
    [Category("Backend")]
    [Category("Backend team")]
    [Category("Backendteam")]
    class ProjectsControllerTest
    {
        private ProjectsController _controller;

        [SetUp]
        public void Setup()
        {
            // Arrange
            _controller = new ProjectsController();
        }

        [Test]
        public void Index()
        {
            // Act
            ViewResult result = _controller.Index() as ViewResult;
            // Assert
            Assert.AreEqual("Projects openeded!", result.ViewBag.Message);
        }

        [Test]
        public void Create()
        {
            // Act
            ViewResult result = _controller.Create() as ViewResult;
            // Assert
            Assert.AreEqual("Project created!", result.ViewBag.Message);
        }
    }
}
