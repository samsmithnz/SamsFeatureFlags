using FeatureFlags.Models;
using FeatureFlags.Service.Controllers;
using FeatureFlags.Service.DataAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage.Table;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeatureFlags.Tests
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestClass]
    public class FeatureFlagsUnitTests
    {

        [TestMethod]
        public async Task CheckFeatureFlagUnitTest()
        {
            //Arrange
            Mock<IConfiguration> mockConfiguration = new Mock<IConfiguration>();
            FeatureFlagsStorageTable context = new FeatureFlagsStorageTable(mockConfiguration.Object);
            Mock<IFeatureFlagsStorageTable> mock = new Mock<IFeatureFlagsStorageTable>();
            mock.Setup(repo => repo.CheckFeatureFlag(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(true));
            FeatureFlagsController controller = new FeatureFlagsController(mock.Object);
            string featureFlagName = "abc";
            string environment = "def";

            //Act
            bool result = await controller.CheckFeatureFlag(featureFlagName, environment);

            //Assert
            Assert.IsTrue(result == true);
        }

        [TestMethod]
        public async Task GetFeatureFlagsUnitTest()
        {
            //Arrange
            Mock<IConfiguration> mockConfiguration = new Mock<IConfiguration>();
            FeatureFlagsStorageTable context = new FeatureFlagsStorageTable(mockConfiguration.Object);
            Mock<IFeatureFlagsStorageTable> mock = new Mock<IFeatureFlagsStorageTable>();
            mock.Setup(repo => repo.GetFeatureFlags()).Returns(Task.FromResult(GetFeatureFlagsTestData()));
            FeatureFlagsController controller = new FeatureFlagsController(mock.Object);

            //Act
            IEnumerable<FeatureFlag> results = await controller.GetFeatureFlags();

            //Assert
            Assert.IsTrue(results != null);
            Assert.IsTrue(results.Count() == 1);
            TestFeatureFlag(results.FirstOrDefault());
        }

        [TestMethod]
        public async Task GetFeatureFlagUnitTest()
        {
            //Arrange
            Mock<IConfiguration> mockConfiguration = new Mock<IConfiguration>();
            FeatureFlagsStorageTable context = new FeatureFlagsStorageTable(mockConfiguration.Object);
            Mock<IFeatureFlagsStorageTable> mock = new Mock<IFeatureFlagsStorageTable>();
            mock.Setup(repo => repo.GetFeatureFlag(It.IsAny<string>())).Returns(Task.FromResult(GetTestRow()));
            FeatureFlagsController controller = new FeatureFlagsController(mock.Object);
            string featureFlagName = "abc";

            //Act
            FeatureFlag result = await controller.GetFeatureFlag(featureFlagName);

            //Assert
            Assert.IsTrue(result != null);
            TestFeatureFlag(result ?? new FeatureFlag());
        }

        [TestMethod]
        public async Task SaveFeatureFlagsUnitTest()
        {
            //Arrange
            Mock<IConfiguration> mockConfiguration = new Mock<IConfiguration>();
            FeatureFlagsStorageTable context = new FeatureFlagsStorageTable(mockConfiguration.Object);
            Mock<IFeatureFlagsStorageTable> mock = new Mock<IFeatureFlagsStorageTable>();
            mock.Setup(repo => repo.SaveFeatureFlag(It.IsAny<FeatureFlag>())).Returns(Task.FromResult(true));
            FeatureFlagsController controller = new FeatureFlagsController(mock.Object);
            FeatureFlag featureFlag = GetTestRow();

            //Act
            bool result = await controller.SaveFeatureFlag(featureFlag);

            //Assert
            Assert.IsTrue(result == true);
        }

        [TestMethod]
        public async Task SaveFeatureFlagsStateDevUnitTest()
        {
            //Arrange
            Mock<IConfiguration> mockConfiguration = new Mock<IConfiguration>();
            FeatureFlagsStorageTable context = new FeatureFlagsStorageTable(mockConfiguration.Object);
            Mock<IFeatureFlagsStorageTable> mock = new Mock<IFeatureFlagsStorageTable>();
            mock.Setup(repo => repo.GetFeatureFlag(It.IsAny<string>())).Returns(Task.FromResult(GetTestRow()));
            FeatureFlagsController controller = new FeatureFlagsController(mock.Object);
            FeatureFlag featureFlag = GetTestRow();

            //Act
            bool result = await controller.SaveFeatureFlagState(featureFlag.Name, "dev", true);

            //Assert
            Assert.IsTrue(result == true);
        }

        [TestMethod]
        public async Task SaveFeatureFlagsStateQAUnitTest()
        {
            //Arrange
            Mock<IConfiguration> mockConfiguration = new Mock<IConfiguration>();
            FeatureFlagsStorageTable context = new FeatureFlagsStorageTable(mockConfiguration.Object);
            Mock<IFeatureFlagsStorageTable> mock = new Mock<IFeatureFlagsStorageTable>();
            mock.Setup(repo => repo.GetFeatureFlag(It.IsAny<string>())).Returns(Task.FromResult(GetTestRow()));
            FeatureFlagsController controller = new FeatureFlagsController(mock.Object);
            FeatureFlag featureFlag = GetTestRow();

            //Act
            bool result = await controller.SaveFeatureFlagState(featureFlag.Name, "qa", true);

            //Assert
            Assert.IsTrue(result == true);
        }

        [TestMethod]
        public async Task SaveFeatureFlagsStateProdUnitTest()
        {
            //Arrange
            Mock<IConfiguration> mockConfiguration = new Mock<IConfiguration>();
            FeatureFlagsStorageTable context = new FeatureFlagsStorageTable(mockConfiguration.Object);
            Mock<IFeatureFlagsStorageTable> mock = new Mock<IFeatureFlagsStorageTable>();
            mock.Setup(repo => repo.GetFeatureFlag(It.IsAny<string>())).Returns(Task.FromResult(GetTestRow()));
            FeatureFlagsController controller = new FeatureFlagsController(mock.Object);
            FeatureFlag featureFlag = GetTestRow();

            //Act
            bool result = await controller.SaveFeatureFlagState(featureFlag.Name, "prod", true);

            //Assert
            Assert.IsTrue(result == true);
        }

        [TestMethod]
        public async Task SaveFeatureFlagsStateExceptionUnitTest()
        {
            //Arrange
            Mock<IConfiguration> mockConfiguration = new Mock<IConfiguration>();
            FeatureFlagsStorageTable context = new FeatureFlagsStorageTable(mockConfiguration.Object);
            Mock<IFeatureFlagsStorageTable> mock = new Mock<IFeatureFlagsStorageTable>();
            mock.Setup(repo => repo.GetFeatureFlag(It.IsAny<string>())).Returns(Task.FromResult(GetTestRow()));
            FeatureFlagsController controller = new FeatureFlagsController(mock.Object);
            FeatureFlag featureFlag = GetTestRow();

            //Act
            try
            {
                bool result = await controller.SaveFeatureFlagState(featureFlag.Name, "exception", true);
            }
            catch (Exception)
            {
                //Assert
                Assert.IsTrue(true);
            }                 
        }


        [TestMethod]
        public async Task DeleteFeatureFlagsUnitTest()
        {
            //Arrange
            Mock<IConfiguration> mockConfiguration = new Mock<IConfiguration>();
            FeatureFlagsStorageTable context = new FeatureFlagsStorageTable(mockConfiguration.Object);
            Mock<IFeatureFlagsStorageTable> mock = new Mock<IFeatureFlagsStorageTable>();
            mock.Setup(repo => repo.DeleteFeatureFlag(It.IsAny<string>())).Returns(Task.FromResult(true));
            FeatureFlagsController controller = new FeatureFlagsController(mock.Object);
            FeatureFlag featureFlag = GetTestRow();

            //Act
            bool result = await controller.DeleteFeatureFlag(featureFlag.Name);

            //Assert
            Assert.IsTrue(result == true);
        }

        private void TestFeatureFlag(FeatureFlag featureFlags)
        {
            Assert.IsTrue(featureFlags.Name == "abc");
            Assert.IsTrue(featureFlags.Description == "def");
            Assert.IsTrue(featureFlags.DevIsEnabled == true);
            Assert.IsTrue(featureFlags.DevViewCount > 0);
            Assert.IsTrue(featureFlags.DevLastViewDate > DateTime.MinValue);
            Assert.IsTrue(featureFlags.QAIsEnabled == true);
            Assert.IsTrue(featureFlags.QAViewCount > 0);
            Assert.IsTrue(featureFlags.QALastViewDate > DateTime.MinValue);
            Assert.IsTrue(featureFlags.ProdIsEnabled == true);
            Assert.IsTrue(featureFlags.ProdViewCount > 0);
            Assert.IsTrue(featureFlags.ProdLastViewDate > DateTime.MinValue);
            Assert.IsTrue(featureFlags.LastUpdated > DateTime.MinValue);
        }

        private IEnumerable<FeatureFlag> GetFeatureFlagsTestData()
        {
            List<FeatureFlag> featureFlags = new List<FeatureFlag>
            {
                GetTestRow()
            };
            return featureFlags;
        }

        private FeatureFlag GetTestRow()
        {
            return new FeatureFlag("abc")
            {
                Name = "abc",
                Description = "def",
                DevIsEnabled = true,
                DevViewCount = 1,
                DevLastViewDate = DateTime.Now.AddDays(-1),
                QAIsEnabled = true,
                QAViewCount = 1,
                QALastViewDate = DateTime.Now.AddDays(-1),
                ProdIsEnabled = true,
                ProdViewCount = 1,
                ProdLastViewDate = DateTime.Now.AddDays(-1),
                LastUpdated = DateTime.Now
            };
        }

    }
}
