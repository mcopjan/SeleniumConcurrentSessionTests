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
            Driver =  new RemoteWebDriver(new Uri("http://localhost:4444/wd/hub/"),options.ToCapabilities(),  TimeSpan.FromMinutes(5));
        }

        [TestCase(1)]
        [TestCase(2)]
        public void SessionQueueTest(int dummyInt)
        {
            Thread.Sleep(new Random().Next(5000,10000)); // to keep node occupied and let the other test wait a bit in a queue
            Assert.Pass();
        }
        
        [TearDown]
        public void TearDown()
        {
            Driver.Dispose();
        }
    }
}