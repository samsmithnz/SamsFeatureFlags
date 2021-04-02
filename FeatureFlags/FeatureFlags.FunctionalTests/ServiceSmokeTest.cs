using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using System.Reflection;

namespace FeatureFlags.FunctionalTests.Website
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestClass]
    public class WebServiceSmokeTest
    {
        private ChromeDriver _driver;
        private TestContext _testContextInstance;
        private string _serviceUrl = null;

        [TestMethod]
        [TestCategory("SkipWhenLiveUnitTesting")]
        [TestCategory("SmokeTest")]
        public void GotoFeatureFlagsWebAPITest()
        {
            //Arrange
            bool serviceLoaded;

            //Act
            string serviceURL = _serviceUrl + "api/featureflags/GetFeatureFlags";
            Console.WriteLine(serviceURL);
            _driver.Navigate().GoToUrl(serviceURL);
            serviceLoaded = (_driver.Url == serviceURL);
            OpenQA.Selenium.IWebElement data = _driver.FindElementByXPath(@"/html/body/pre");

            //Assert
            Assert.IsTrue(serviceLoaded);
            Assert.IsTrue(data != null);
            //Assert.AreEqual(data.Text, "[\"value1\",\"value2\"]");
        }

        [TestInitialize]
        public void SetupTests()
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("headless");
            //_driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), chromeOptions);
            _driver = new ChromeDriver(chromeOptions);

            if (TestContext.Properties == null || TestContext.Properties.Count == 0)
            {
                throw new Exception("Select test settings file to continue");
            }
            else
            {
                _serviceUrl = TestContext.Properties["ServiceUrl"].ToString();
            }
        }

        public TestContext TestContext
        {
            get
            {
                return _testContextInstance;
            }
            set
            {
                _testContextInstance = value;
            }
        }

        [TestCleanup()]
        public void CleanupTests()
        {
            _driver.Quit();
        }
    }
}