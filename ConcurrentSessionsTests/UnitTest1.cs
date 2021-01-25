using System;
using System.Threading;
using NUnit.Framework;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace SeleniumQueuedSessions
{
    [Parallelizable(ParallelScope.All)]
    public class Tests
    {
        private const string GRID_URL = "http://localhost:4444/wd/hub/";
        private IWebDriver Driver
        {
            get => (IWebDriver) TestExecutionContext.CurrentContext.CurrentTest.Properties.Get("driver");
            set
            {
                if (!TestExecutionContext.CurrentContext.CurrentTest.Properties.ContainsKey("driver"))
                {
                    TestExecutionContext.CurrentContext.CurrentTest.Properties.Add("driver", value);
                }
                
            }
        }

        [SetUp]
        public void Setup()
        {
            ChromeOptions options = new ChromeOptions {Proxy = null};
            options.AddArgument("--start-maximized");
            Driver =  new RemoteWebDriver(new Uri(GRID_URL),options.ToCapabilities(),  TimeSpan.FromMinutes(5));
        }

        [TestCase(1)]
        [TestCase(2)]
        public void SessionQueueTest(int dummyInt)
        {
            Thread.Sleep(1000);
            Assert.Pass();
        }
        
        [TearDown]
        public void TearDown()
        {
            Driver.Dispose();
        }
    }
}