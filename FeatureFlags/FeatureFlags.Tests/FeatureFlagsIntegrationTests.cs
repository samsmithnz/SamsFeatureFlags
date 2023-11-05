using FeatureFlags.Models;
using FeatureFlags.Service.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeatureFlags.Tests
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestClass]
    public class FeatureFlagsIntegrationTests : BaseIntegrationTests
    {
        [TestMethod]
        public void CheckFeatureFlagPRIntegrationTest()
        {
            //Arrange
            FeatureFlagsStorageTable da = new(Configuration);
            string name = "UnitTestFeatureFlag01";
            string environment = "pr";

            //Act
            bool result = da.CheckFeatureFlag(name, environment);

            //Assert
            Assert.IsTrue(result == false);
        }

        [TestMethod]
        public void CheckFeatureFlagDevIntegrationTest()
        {
            //Arrange
            FeatureFlagsStorageTable da = new(Configuration);
            string name = "UnitTestFeatureFlag01";
            string environment = "dev";

            //Act
            bool result = da.CheckFeatureFlag(name, environment);

            //Assert
            Assert.IsTrue(result == true);
        }

        [TestMethod]
        public void CheckFeatureFlagQAIntegrationTest()
        {
            //Arrange
            FeatureFlagsStorageTable da = new(Configuration);
            string name = "UnitTestFeatureFlag01";
            string environment = "qa";

            //Act
            bool result = da.CheckFeatureFlag(name, environment);

            //Assert
            Assert.IsTrue(result == false);
        }

        [TestMethod]
        public void CheckFeatureFlagProdIntegrationTest()
        {
            //Arrange
            FeatureFlagsStorageTable da = new(Configuration);
            string name = "UnitTestFeatureFlag01";
            string environment = "prod";

            //Act
            bool result = da.CheckFeatureFlag(name, environment);

            //Assert
            Assert.IsTrue(result == false);
        }

        [TestMethod]
        public void CheckFeatureFlagExceptionIntegrationTest()
        {
            //Arrange
            FeatureFlagsStorageTable da = new(Configuration);
            string name = "UnitTestFeatureFlag01";
            string environment = "exception";

            //Act
            string exceptionGenerated = "";
            try
            {
                da.CheckFeatureFlag(name, environment);
            }
            catch (Exception ex)
            {
                exceptionGenerated = ex.Message;
            }

            //Assert
            Assert.AreEqual(exceptionGenerated, "Unknown environment: " + environment + " for feature flag " + name);
        }

        [TestMethod]
        public void GetFeatureFlagsIntegrationTest()
        {
            //Arrange
            FeatureFlagsStorageTable da = new(Configuration);

            //Act
            IEnumerable<FeatureFlag> featureFlags = da.GetFeatureFlags();

            //Assert
            Assert.IsTrue(featureFlags != null);
            Assert.IsTrue(featureFlags.Count() >= 0);
        }

        [TestMethod]
        public void GetFeatureFlagIntegrationTest()
        {
            //Arrange
            FeatureFlagsStorageTable da = new(Configuration);
            string name = "UnitTestFeatureFlag01";

            //Act
            FeatureFlag featureFlag = da.GetFeatureFlag(name);

            //Assert
            Assert.IsTrue(featureFlag != null);
        }

        [TestMethod]
        public void SaveFeatureFlagIntegrationTest()
        {
            //Arrange
            FeatureFlagsStorageTable da = new(Configuration);
            string featureFlagName = "UnitTestFeatureFlag01";
            FeatureFlag featureFlag = new(featureFlagName)
            {
                Name = featureFlagName,
                Description = "Feature Flag for unit tests",
                PRIsEnabled = false,
                DevIsEnabled = true,
                DevViewCount = 1,
                QAIsEnabled = false,
                ProdIsEnabled = false,
                LastUpdated = DateTime.Now
            };

            //Act
            bool result = da.SaveFeatureFlag(featureFlag);

            //Assert
            Assert.IsTrue(result == true);
        }

        [TestMethod]
        public void DeleteFeatureFlagIntegrationTest()
        {
            //Arrange
            FeatureFlagsStorageTable da = new(Configuration);
            string featureFlagName = "TestFeatureFlag02";
            FeatureFlag featureFlag = new(featureFlagName)
            {
                Name = featureFlagName,
                Description = "Feature Flag #2",
                PRIsEnabled = false,
                DevIsEnabled = false,
                QAIsEnabled = false,
                ProdIsEnabled = false,
                LastUpdated = DateTime.Now
            };

            //Act
            da.SaveFeatureFlag(featureFlag);
            bool result = da.DeleteFeatureFlag(featureFlagName);

            //Assert
            Assert.IsTrue(result == true);
        }

    }
}
