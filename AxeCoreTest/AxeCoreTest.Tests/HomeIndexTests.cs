using System;
using System.IO;
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
            // WebDriverFactory uses environment variables set by azure-pipelines.yml to determine which browser to use;
            // the test cases we'll write in this file will work regardless of which browser they're running against.
            //
            // This WebDriverFactory is just one example of how you might initialize Selenium; if you're adding Selenium.Axe
            // to an existing set of end to end tests that already have their own way of initializing a webdriver, you can
            // keep using that instead.
            _webDriver = WebDriverFactory.CreateFromEnvironmentVariableSettings();

            // You *must* set this timeout to use Selenium.Axe. It defaults to "0 seconds", which isn't enough time for
            // Axe to scan the page. The exact amount of time will depend on the complexity of the page you're testing.
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
            // For simplicity, we're pointing our test browser directly to a static html file on disk.
            // In a project with more complex hosting needs, you might instead start up a localhost http server
            // and then navigate to a http://localhost link.
            //string samplePageFilePath = Path.GetFullPath(@"Index.cshtml");
            string samplePageFileUrl = new Uri("https://localhost:44335/").AbsoluteUri;
            _webDriver.Navigate().GoToUrl(samplePageFileUrl);

            // It's a good practice to make sure the page's content has actually loaded *before* running any
            // accessibility tests. This acts as a sanity check that the browser initialization worked and makes
            // sure that you aren't just scanning a blank page.
            new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10))
                .Until(d => d.FindElement(By.TagName("main")));
        }

        [TestMethod]
        public void TestMethod1()
        {
            AxeResult axeResult = new AxeBuilder(_webDriver).Analyze();

            axeResult.Violations.Should().NotBeEmpty();
        }
    }
}
