using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;
using System;
using FeatureFlags.Models;

namespace FeatureFlags.FunctionalTests.Service
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestClass]
    public class ServiceSmokeTest
    {
        private ChromeDriver _driver;
        private TestContext _testContextInstance;
        private string _serviceUrl = null;

        [TestMethod]
        [TestCategory("SkipWhenLiveUnitTesting")]
        [TestCategory("SmokeTest")]
        public void GotoFeatureFlagsServiceFeatureFlagsTest()
        {
            //Arrange
            bool serviceLoaded;
             
            //Act
            string serviceURL = _serviceUrl + "api/featureflags/getfeatureflags";
            Console.WriteLine(serviceURL);
            _driver.Navigate().GoToUrl(serviceURL);
            serviceLoaded = (_driver.Url == serviceURL);
            OpenQA.Selenium.IWebElement data = _driver.FindElementByXPath(@"/html/body/pre");

            //Assert
            Assert.IsTrue(serviceLoaded);
            Assert.IsTrue(data != null);
            //Convert the JSON to the feature flags model
            IEnumerable<FeatureFlag> featureFlags = JsonConvert.DeserializeObject<IEnumerable<FeatureFlag>>(data.Text);
            Assert.IsTrue(featureFlags.Count() > 0); //There is more than one owner
            Assert.IsTrue(featureFlags.FirstOrDefault().Name.Length > 0); //The first flag has an id
            Assert.IsTrue(featureFlags.FirstOrDefault().Description.Length > 0); //The first flag has an name
        }

        [TestInitialize]
        public void SetupTests()
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("headless");
            _driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), chromeOptions);

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
