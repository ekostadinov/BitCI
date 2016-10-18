using System;
using System.Web.Mvc;
using BitCI.Controllers;
using BitCI.Models;
using BitCI.Models.BuildSteps;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
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
        private IWebElement _logoffLink;
        private IWebElement _invalidLoginText;

        [SetUp]
        public void Setup()
        {
            _driver = new ChromeDriver(@"C:\DEV");
            _defaultDriverWait = new WebDriverWait(_driver,
                new TimeSpan(0, 0, 60));
            _driver.Navigate().GoToUrl(serverUrl);
            _driver.Manage().Window.Maximize();
        }

        [Test]
        public void Valid_User_Should_Be_Able_To_Login()
        {
            // Act
            _loginUsernameInput = ReturnElementForDefaultWait(By.XPath("//input[@id='Email']"));
            _loginUsernameInput.Clear();
            _loginUsernameInput.SendKeys("demo@gmail.com");
            _loginPasswordInput = ReturnElementForDefaultWait(By.XPath("//input[@id='Password']"));
            _loginPasswordInput.Clear();
            _loginPasswordInput.SendKeys("Test12#$");
            _loginButton = ReturnElementForDefaultWait(By.XPath("//input[@value='Log in']"));
            _loginButton.Click();
            _logoffLink = ReturnElementForDefaultWait(By.XPath("//*[text()='Log off']"));
            // Assert
            Assert.True(_logoffLink.Displayed);
        }

        [Test]
        public void InValid_User_Should_Not_Be_Able_To_Login()
        {
            // Act
            _loginUsernameInput = ReturnElementForDefaultWait(By.XPath("//input[@id='Email']"));
            _loginUsernameInput.Clear();
            _loginUsernameInput.SendKeys("Invalid@mail.com");
            _loginPasswordInput = ReturnElementForDefaultWait(By.XPath("//input[@id='Password']"));
            _loginPasswordInput.Clear();
            _loginPasswordInput.SendKeys("Foobar12#$");
            _loginButton = ReturnElementForDefaultWait(By.XPath("//input[@value='Log in']"));
            _loginButton.Click();
            _invalidLoginText = ReturnElementForDefaultWait(By.XPath("//*[text()='Invalid login attempt.']"));

            // Assert
            Assert.True(_invalidLoginText.Displayed);
        }

        [Test]
        public void Valid_User_With_InValid_Pass_Should_Not_Be_Able_To_Login()
        {
            // Act
            _loginUsernameInput = ReturnElementForDefaultWait(By.XPath("//input[@id='Email']"));
            _loginUsernameInput.Clear();
            _loginUsernameInput.SendKeys("demo@gmail.com");
            _loginPasswordInput = ReturnElementForDefaultWait(By.XPath("//input[@id='Password']"));
            _loginPasswordInput.Clear();
            _loginPasswordInput.SendKeys("Test34#$");
            _loginButton = ReturnElementForDefaultWait(By.XPath("//input[@value='Log in']"));
            _loginButton.Click();
            _invalidLoginText = ReturnElementForDefaultWait(By.XPath("//*[text()='Invalid login attempt.']"));

            // Assert
            Assert.True(_invalidLoginText.Displayed);
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
