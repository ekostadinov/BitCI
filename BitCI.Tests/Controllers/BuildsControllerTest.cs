using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using BitCI;
using BitCI.Controllers;
using NUnit.Framework;

namespace BitCI.Tests.Controllers
{
    [TestFixture]
    [Category("Backend")]
    [Category("Backend team")]
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
        public void Index()
        {
            // Act
            ViewResult result = _controller.Index() as ViewResult;
            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void Create()
        {
            // Act
            ViewResult result = _controller.Create() as ViewResult;
            // Assert
            Assert.AreEqual("Our goals and vision. Why us?", result.ViewBag.Message);
        }

        [Test]
        public void Details()
        {
            // Act
            ViewResult result = _controller.Details(49) as ViewResult;
            // Assert
            Assert.IsNotNull(result);
        }

    }
}
