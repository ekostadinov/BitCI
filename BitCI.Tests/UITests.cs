using System;
using System.Web.Mvc;
using BitCI.Controllers;
using BitCI.Models;
using BitCI.Models.BuildSteps;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace BitCI.Tests
{
    [TestFixture]
    [Category("Frontend")]
    [Category("Front team")]
    [Category("Frontendteam")]
    class UITests
    {
        private IWebDriver _driver;
        private WebDriverWait _defaultDriverWait;
        private string serverUrl = "http://localhost:5606/";
        private IWebElement _loginUsernameInput;
        private IWebElement _loginPasswordInput;
        private IWebElement _loginButton;


        [SetUp]
        public void Setup()
        {
            _driver = new FirefoxDriver();
            _defaultDriverWait = new WebDriverWait(_driver,
                new TimeSpan(0, 0, 60));
            _driver.Navigate().GoToUrl(serverUrl);
            _driver.Manage().Window.Maximize();

        }

        [Test]
        public void Valid_User_Should_Be_Able_To_Login()
        {
            _loginUsernameInput = ReturnElementForDefaultWait(By.XPath("//input[@id='Email']"));
            _loginUsernameInput.Clear();
            _loginUsernameInput.SendKeys("foo@bar.com");

            _loginPasswordInput = ReturnElementForDefaultWait(By.XPath("//input[@id='Password']"));
            _loginPasswordInput.Clear();
            _loginPasswordInput.SendKeys("Foobar12#$");

            _loginButton = ReturnElementForDefaultWait(By.XPath("//input[@value='Log in']"));
            _loginButton.Click();


        }

        public IWebElement ReturnElementForDefaultWait(By by)
        {
            IWebElement element = null;
            var exc = "";
            var isReady = false;

            try
            {
                isReady = _defaultDriverWait.Until(d =>
                {
                    try
                    {
                        element = d.FindElement(by);
                        return true;
                    }
                    catch (Exception e)
                    {
                        exc = e.ToString();
                        return false;
                    }
                });
            }
            catch (WebDriverTimeoutException wdte)
            {
                if (!isReady)
                {
                    throw new Exception("Exception: \n'" + exc + "\n caught when searching for element!");
                }
            }

            return element;
        }

        [TearDown]
        public void Teardown()
        {
            _driver.Close();
            _driver.Quit();
        }
    }
}
