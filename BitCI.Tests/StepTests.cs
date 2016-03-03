using System;
using System.Web.Mvc;
using BitCI.Controllers;
using BitCI.Models;
using BitCI.Models.BuildSteps;
using NUnit.Framework;

namespace BitCI.Tests
{
    [TestFixture]
    [Category("Backend")]
    [Category("Backend team")]
    [Category("Backendteam")]
    class StepTests
    {
        private VersionControlStep _vcsStep;
        private BuildStep _buildStep;
        private RunTestsStep _runTestsStep;
        private PostBuildEmailStep _mailStep;
        private TriggerStep _triggerStep;

        [SetUp]
        public void Setup()
        {
            // Arrange
            _vcsStep = new VersionControlStep();
            _buildStep = new BuildStep();
            _runTestsStep = new RunTestsStep();
            _mailStep = new PostBuildEmailStep();
            _triggerStep = new TriggerStep();
        }

        [Test]
        public void VCS_Step_Should_Throw_Exception_When_Build_Is_Null()
        {
            Assert.Throws(typeof(NullReferenceException), () =>_vcsStep.Execute());
        }

        [Test]
        public void Build_Step_Should_Throw_Exception_When_Build_Is_Null()
        {
            Assert.Throws(typeof(NullReferenceException), () => _buildStep.Execute());
        }

        [Test]
        public void Run_Step_Should_Throw_Exception_When_Build_Is_Null()
        {
            Assert.Throws(typeof(NullReferenceException), () => _runTestsStep.Execute());
        }

        [Test]
        public void Email_Step_Should_Throw_Exception_When_Build_Is_Null()
        {
            Assert.Throws(typeof(NullReferenceException), () => _mailStep.Execute());
        }

        [Test]
        public void Trigger_Step_Should_Throw_Exception_When_Build_Is_Null()
        {
            Assert.Throws(typeof(NullReferenceException), () => _triggerStep.Execute());
        }
    }
}
