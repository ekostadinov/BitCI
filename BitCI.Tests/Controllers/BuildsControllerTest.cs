using System.Web.Mvc;
using BitCI.Controllers;
using NUnit.Framework;

namespace BitCI.Tests.Controllers
{
    [TestFixture]
    [Category("Backend")]
    [Category("Backend team")]
    [Category("Backendteam")]
    class BuildsControllerTest
    {
        private BuildsController _controller;

        [SetUp]
        public void Setup()
        {
            // Arrange
            _controller = new BuildsController();
        }

        [Test]
        public void Create()
        { 
            // Act
            ViewResult result = _controller.Create() as ViewResult;
            // Assert
            Assert.AreEqual("Build created!", result.ViewBag.Message);
        }
    }
}
