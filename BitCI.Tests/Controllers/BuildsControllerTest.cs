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
    class BuildsControllerTest
    {
        private BuildsController _controller;
        private string _vcsStepValidValue = "https://github.com/ekostadinov/BitCI.git";
        private string _vcsStepInvalidValue = "https://github.co.git";
        private string _buildStepValidValue = "BitCI.sln";
        private string _buildStepInvalidValue = "BitCI.slnn.nn";
        private string _runStepValidValue = "/BitCI.Tests/bin/Debug/BitCI.Tests.dll";
        private string _runStepInvalidValue = "/BitCI.Tests/bin/Debug/Debug/Debug/BitCI.Tests.dll";
        private string _mailStepValidValue = "bitcimail@mail.ru";
        private string _mailStepInvalidValue = "bitcimail@mail.mailru.ru";
        private string _triggerStepValidValue = "00:00:15";
        private string _triggerStepInvalidValue = "hh:mm/ss";
        private Build _validBuild;

        [SetUp]
        public void Setup()
        {
            // Arrange
            _controller = new BuildsController();
            _validBuild = new Build { Id = 5, ProjectId = 1 };
        }

        [Test]
        public void Create()
        { 
            // Act
            ViewResult result = _controller.Create() as ViewResult;
            // Assert
            Assert.AreEqual("Build created!", result.ViewBag.Message);
        }

        [Test]
        public void Create_Should_Throw_Exception_When_Invalid_Build_Is_Passed()
        {
            Assert.Throws<NullReferenceException>(() => _controller.Create(
                new Build(), _vcsStepValidValue, _buildStepValidValue, _runStepValidValue, _mailStepValidValue, _triggerStepValidValue));
        }

        [Test]
        public void Create_Should_Throw_Exception_When_Invalid_Build_And_Invalid_Vsc_Are_Passed()
        {
            Assert.Throws<NullReferenceException>(() => _controller.Create(
                                new Build(), _vcsStepInvalidValue, _buildStepValidValue, _runStepValidValue, _mailStepValidValue, _triggerStepValidValue));
        }

        [Test]
        public void Create_Should_Throw_Exception_When_Invalid_Build_And_Invalid_Vsc_And_Invalid_Build_Are_Passed()
        {
            Assert.Throws<NullReferenceException>(() => _controller.Create(
                                new Build(), _vcsStepInvalidValue, _buildStepInvalidValue, _runStepValidValue, _mailStepValidValue, _triggerStepValidValue));
        }

        [Test]
        public void Create_Should_Throw_Exception_When_Invalid_Build_And_Invalid_Vsc_And_Invalid_Build_And_Invalid_Run_Are_Passed()
        {
            Assert.Throws<NullReferenceException>(() => _controller.Create(
                                new Build(), _vcsStepInvalidValue, _buildStepInvalidValue, _runStepInvalidValue, _mailStepValidValue, _triggerStepValidValue));
        }

        [Test]
        public void Create_Should_Throw_Exception_When_Invalid_Build_And_Invalid_Vsc_And_Invalid_Build_And_Invalid_Run_And_Invalid_Email_Are_Passed()
        {
            Assert.Throws<NullReferenceException>(() => _controller.Create(
                                new Build(), _vcsStepInvalidValue, _buildStepInvalidValue, _runStepInvalidValue, _mailStepInvalidValue, _triggerStepValidValue));
        }

        [Test]
        public void Create_Should_Throw_Exception_When_Invalid_Build_And_Invalid_Vsc_And_Invalid_Build_And_Invalid_Run_And_Invalid_Email_And_Invalid_Trigger_Are_Passed()
        {
            Assert.Throws<NullReferenceException>(() => _controller.Create(
                                new Build(), _vcsStepInvalidValue, _buildStepInvalidValue, _runStepInvalidValue, _mailStepInvalidValue, _triggerStepInvalidValue));
        }

        [Test]
        public void Create_Should_Throw_Exception_When_Invalid_Vcs_Value_Is_Passed()
        {
            Assert.Throws<NullReferenceException>(() => _controller.Create(
                            _validBuild, _vcsStepValidValue, _buildStepValidValue, _runStepValidValue, _mailStepValidValue, _triggerStepValidValue));
        }

        [Test]
        public void Create_Should_Throw_Exception_When_Valid_Build_And_Invalid_Vsc_And_Invalid_Build_Are_Passed()
        {
            Assert.Throws<NullReferenceException>(() => _controller.Create(
                                _validBuild, _vcsStepInvalidValue, _buildStepInvalidValue, _runStepValidValue, _mailStepValidValue, _triggerStepValidValue));
        }


        [Test]
        public void Create_Should_Throw_Exception_When_vcsStepValidValue_buildStepValidValue_ArePassed()
        {
            Assert.Throws<NullReferenceException>(() => _controller.Create(
                                _validBuild, _vcsStepValidValue, _buildStepValidValue, _runStepValidValue, _mailStepValidValue, _triggerStepValidValue));
        }

        [Test]
        public void Create_Should_Throw_Exception_When_vcsStepValidValue_buildStepInvalidValue_ArePassed()
        {
            Assert.Throws<NullReferenceException>(() => _controller.Create(
                                _validBuild, _vcsStepValidValue, _buildStepInvalidValue, _runStepValidValue, _mailStepValidValue, _triggerStepValidValue));
        }

        [Test]
        public void Create_Should_Throw_Exception_When_vcsStepValidValue_runStepValidValue_ArePassed()
        {
            Assert.Throws<NullReferenceException>(() => _controller.Create(
                                _validBuild, _vcsStepValidValue, _buildStepInvalidValue, _runStepValidValue, _mailStepValidValue, _triggerStepValidValue));
        }

        [Test]
        public void Create_Should_Throw_Exception_When_vcsStepValidValue_runStepInvalidValue_ArePassed()
        {
            Assert.Throws<NullReferenceException>(() => _controller.Create(
                                _validBuild, _vcsStepValidValue, _buildStepValidValue, _runStepInvalidValue, _mailStepValidValue, _triggerStepValidValue));
        }

        [Test]
        public void Create_Should_Throw_Exception_When_vcsStepValidValue_mailStepValidValue_ArePassed()
        {
            Assert.Throws<NullReferenceException>(() => _controller.Create(
                                _validBuild, _vcsStepValidValue, _buildStepValidValue, _runStepInvalidValue, _mailStepValidValue, _triggerStepValidValue));
        }

        [Test]
        public void Create_Should_Throw_Exception_When_vcsStepValidValue_mailStepInvalidValue_ArePassed()
        {
            Assert.Throws<NullReferenceException>(() => _controller.Create(
                                _validBuild, _vcsStepValidValue, _buildStepValidValue, _runStepInvalidValue, _mailStepInvalidValue, _triggerStepValidValue));
        }

        [Test]
        public void Create_Should_Throw_Exception_When_vcsStepValidValue_triggerStepValidValue_ArePassed()
        {
            Assert.Throws<NullReferenceException>(() => _controller.Create(
                                _validBuild, _vcsStepValidValue, _buildStepValidValue, _runStepInvalidValue, _mailStepInvalidValue, _triggerStepValidValue));
        } 
        
        [Test]
        public void Create_Should_Throw_Exception_When_vcsStepValidValue_triggerStepInvalidValue_ArePassed()
        {
            Assert.Throws<NullReferenceException>(() => _controller.Create(
                                _validBuild, _vcsStepValidValue, _buildStepValidValue, _runStepInvalidValue, _mailStepInvalidValue, _triggerStepInvalidValue));
        } 
        
        [Test]
        public void Create_Should_Throw_Exception_When_vcsStepValidValue_validBuild_ArePassed()
        {
            Assert.Throws<NullReferenceException>(() => _controller.Create(
                                _validBuild, _vcsStepValidValue, _buildStepValidValue, _runStepInvalidValue, _mailStepInvalidValue, _triggerStepInvalidValue));
        } 
        
        [Test]
        public void Create_Should_Throw_Exception_When_vcsStepValidValue_invalidBuild_ArePassed()
        {
            Assert.Throws<NullReferenceException>(() => _controller.Create(
                                new Build(), _vcsStepValidValue, _buildStepValidValue, _runStepInvalidValue, _mailStepInvalidValue, _triggerStepInvalidValue));
        } 
        
        [Test]
        public void Create_Should_Throw_Exception_When_vcsStepInvalidValue_buildStepValidValue_ArePassed()
        {
            Assert.Throws<NullReferenceException>(() => _controller.Create(
                                new Build(), _vcsStepValidValue, _buildStepValidValue, _runStepInvalidValue, _mailStepInvalidValue, _triggerStepInvalidValue));
        } 
        
        [Test]
        public void Create_Should_Throw_Exception_When_vcsStepInvalidValue_buildStepInvalidValue_ArePassed()
        {
            Assert.Throws<NullReferenceException>(() => _controller.Create(
                                new Build(), _vcsStepValidValue, _buildStepValidValue, _runStepInvalidValue, _mailStepInvalidValue, _triggerStepInvalidValue));
        } 
        
        [Test]
        public void Details_Should_Throw_NotFound_Exception_When_Invalid_Build_Id_Is_Passed()
        {
            var response = _controller.Details(4);
            Assert.AreEqual("HttpNotFoundResult", response.GetType().Name);
        }

        [Test]
        public void Details_Should_Throw_BadRequest__When_Null_Build_Id_Is_Passed()
        {
            var response = _controller.Details(null);
            Assert.AreEqual("HttpStatusCodeResult", response.GetType().Name);
        }

        [Test]
        public void Index_Should_Throw_Exception_When_No_Build_Is_Found()
        {
            Assert.Throws(typeof(NullReferenceException), () => _controller.Index());
        }

        [Test]
        public void Run_Should_Throw_Exception_When_Invalid_Build_Id_Is_Passed()
        {
            var response = _controller.Run(4);

            Assert.AreEqual("HttpNotFoundResult", response.GetType().Name);
        }

        [Test]
        public void Run_Should_Throw_Exception_When_Invalid_Build_Is_Passed()
        {
            Assert.Throws(typeof(NullReferenceException), () => _controller.Run(new Build()));
        }
    }
}
