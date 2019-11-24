using FeatureFlags.Models;
using FeatureFlags.Service.Controllers;
using FeatureFlags.Service.DataAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
        public async Task CheckFeatureFlagDevIntegrationTest()
        {
            //Arrange
            FeatureFlagsStorageTable da = new FeatureFlagsStorageTable(Configuration);
            string name = "UnitTestFeatureFlag01";
            string environment = "dev";

            //Act
            bool result = await da.CheckFeatureFlag(name, environment);

            //Assert
            Assert.IsTrue(result == true);
        }

        [TestMethod]
        public async Task CheckFeatureFlagQAIntegrationTest()
        {
            //Arrange
            FeatureFlagsStorageTable da = new FeatureFlagsStorageTable(Configuration);
            string name = "UnitTestFeatureFlag01";
            string environment = "qa";

            //Act
            bool result = await da.CheckFeatureFlag(name, environment);

            //Assert
            Assert.IsTrue(result == false);
        }

        [TestMethod]
        public async Task CheckFeatureFlagProdIntegrationTest()
        {
            //Arrange
            FeatureFlagsStorageTable da = new FeatureFlagsStorageTable(Configuration);
            string name = "UnitTestFeatureFlag01";
            string environment = "prod";

            //Act
            bool result = await da.CheckFeatureFlag(name, environment);

            //Assert
            Assert.IsTrue(result == false);
        }

        [TestMethod]
        public async Task CheckFeatureFlagExceptionIntegrationTest()
        {
            //Arrange
            FeatureFlagsStorageTable da = new FeatureFlagsStorageTable(Configuration);
            string name = "UnitTestFeatureFlag01";
            string environment = "exception";

            //Act
            string exceptionGenerated = "";
            try
            {
                await da.CheckFeatureFlag(name, environment);
            }
            catch (Exception ex)
            {
                exceptionGenerated = ex.Message;
            }

            //Assert
            Assert.AreEqual(exceptionGenerated, "Unknown environment: " + environment + " for feature flag " + name);
        }

        [TestMethod]
        public async Task GetFeatureFlagsIntegrationTest()
        {
            //Arrange
            FeatureFlagsStorageTable da = new FeatureFlagsStorageTable(Configuration);

            //Act
            IEnumerable<FeatureFlag> featureFlags = await da.GetFeatureFlags();

            //Assert
            Assert.IsTrue(featureFlags != null);
            Assert.IsTrue(featureFlags.Count() >= 0);
        }

        [TestMethod]
        public async Task GetFeatureFlagIntegrationTest()
        {
            //Arrange
            FeatureFlagsStorageTable da = new FeatureFlagsStorageTable(Configuration);
            string name = "UnitTestFeatureFlag01";

            //Act
            FeatureFlag featureFlag = await da.GetFeatureFlag(name);

            //Assert
            Assert.IsTrue(featureFlag != null);
        }

        [TestMethod]
        public async Task SaveFeatureFlagIntegrationTest()
        {
            //Arrange
            FeatureFlagsStorageTable da = new FeatureFlagsStorageTable(Configuration);
            string featureFlagName = "UnitTestFeatureFlag01";
            FeatureFlag featureFlag = new FeatureFlag(featureFlagName)
            {
                Name = featureFlagName,
                Description = "Feature Flag for unit tests",
                DevIsEnabled = true,
                DevViewCount = 1,
                QAIsEnabled = false,
                ProdIsEnabled = false,
                LastUpdated = DateTime.Now
            };

            //Act
            bool result = await da.SaveFeatureFlag(featureFlag);

            //Assert
            Assert.IsTrue(result == true);
        }

        [TestMethod]
        public async Task DeleteFeatureFlagIntegrationTest()
        {
            //Arrange
            FeatureFlagsStorageTable da = new FeatureFlagsStorageTable(Configuration);
            string featureFlagName = "TestFeatureFlag02";
            FeatureFlag featureFlag = new FeatureFlag(featureFlagName)
            {
                Name = featureFlagName,
                Description = "Feature Flag #2",
                DevIsEnabled = false,
                QAIsEnabled = false,
                ProdIsEnabled = false,
                LastUpdated = DateTime.Now
            };

            //Act
            await da.SaveFeatureFlag(featureFlag);
            bool result = await da.DeleteFeatureFlag(featureFlagName);

            //Assert
            Assert.IsTrue(result == true);
        }

    }
}
