﻿using System.Web.Mvc;
using BitCI.Controllers;
using NUnit.Framework;

namespace BitCI.Tests.Controllers
{
    [TestFixture]
    [Category("Backend")]
    [Category("Backend team")]
    [Category("Backendteam")]
    public class HomeControllerTest
    {
        private HomeController _controller;

        [SetUp]
        public void Setup()
        {
            // Arrange
            _controller = new HomeController();
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
        public void About()
        {
            // Act
            ViewResult result = _controller.About() as ViewResult;
            // Assert
            Assert.AreEqual("Our goals and vision. Why us?", result.ViewBag.Message);
        }

        [Test]
        public void Contact()
        {
            // Act
            ViewResult result = _controller.Contact() as ViewResult;
            // Assert
            Assert.IsNotNull(result);
        }
    }
}
