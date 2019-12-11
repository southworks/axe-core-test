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
            string samplePageFilePath = Path.GetFullPath(@"Sample.html");
            string samplePageFileUrl = new Uri(samplePageFilePath).AbsoluteUri;
            _webDriver.Navigate().GoToUrl(samplePageFileUrl);

            new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10))
                .Until(d => d.FindElement(By.TagName("main")));
        }

        [TestMethod]
        public void Should_Fail_Because_Is_Not_Empty()
        {
            AxeResult axeResult = new AxeBuilder(_webDriver).Analyze();

            axeResult.Violations.Should().BeEmpty();
        }

        [TestMethod]
        public void Should_Pass_Because_Is_Not_Empty()
        {
            AxeResult axeResult = new AxeBuilder(_webDriver).Analyze();

            axeResult.Violations.Should().NotBeEmpty();
        }
    }
}
