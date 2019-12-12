using System;
using System.IO;
using System.Reflection;
using AxeCoreTest.Controllers;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Selenium.Axe;

namespace AxeCoreTest.Tests
{
    [TestClass]
    public class HomeIndexTests
    {
        private static IWebDriver _webDriver;

        [ClassInitialize]
        public static void StartBrowser(TestContext testContext)
        {
            _webDriver = WebDriverFactory.CreateFromEnvironmentVariableSettings();

            _webDriver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(20);
        }

        [ClassCleanup]
        public static void StopBrowser()
        {
            _webDriver?.Quit();
        }

        [TestInitialize]
        public void LoadTestPage()
        {
            //string samplePageFilePath = Path.GetFullPath(@"Sample.html");
            string samplePageFileUrl = new Uri("https://azure.microsoft.com/en-us/").AbsoluteUri;
            _webDriver.Navigate().GoToUrl(samplePageFileUrl);

            new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10))
                .Until(d => d.FindElement(By.TagName("body")));
        }

        [TestMethod]
        public void Should_Pass_With_Aria_Allowed_Role_Rule_Disabled()
        {
            AxeResult axeResult = new AxeBuilder(_webDriver)
                .DisableRules("aria-allowed-role")
                .Analyze();

            axeResult.Violations.Should().BeEmpty();
        }
    }
}
