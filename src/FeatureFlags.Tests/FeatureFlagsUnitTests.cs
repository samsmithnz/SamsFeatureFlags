using FeatureFlags.Models;
using FeatureFlags.Service.Controllers;
using FeatureFlags.Service.DataAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
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
        public void CheckFeatureFlagUnitTest()
        {
            //Arrange
            IConfiguration mockConfiguration = Substitute.For<IConfiguration>();
            FeatureFlagsStorageTable context = new(mockConfiguration);
            var mock = Substitute.For<IFeatureFlagsStorageTable>();
            mock.CheckFeatureFlag(Arg.Any<string>(), Arg.Any<string>()).Returns(true);
            FeatureFlagsController controller = new FeatureFlagsController(mock);
            string featureFlagName = "abc";
            string environment = "def";

            //Act
            bool result = controller.CheckFeatureFlag(featureFlagName, environment);

            //Assert
            Assert.IsTrue(result == true);
        }

        [TestMethod]
        public void GetFeatureFlagsUnitTest()
        {
            //Arrange
            IConfiguration mockConfiguration = Substitute.For<IConfiguration>();
            FeatureFlagsStorageTable context = new(mockConfiguration);
            IFeatureFlagsStorageTable mock = Substitute.For<IFeatureFlagsStorageTable>();
            mock.GetFeatureFlags().Returns(GetFeatureFlagsTestData());
            FeatureFlagsController controller = new(mock);

            //Act
            IEnumerable<FeatureFlag> results = controller.GetFeatureFlags();

            //Assert
            Assert.IsTrue(results != null);
            Assert.IsTrue(results?.Count() == 1);
            TestFeatureFlag(results.FirstOrDefault());
        }

        [TestMethod]
        public void GetFeatureFlagUnitTest()
        {
            //Arrange
            IConfiguration mockConfiguration = Substitute.For<IConfiguration>();
            FeatureFlagsStorageTable context = new(mockConfiguration);
            IFeatureFlagsStorageTable mock = Substitute.For<IFeatureFlagsStorageTable>();
            mock.GetFeatureFlag(Arg.Any<string>()).Returns(GetTestRow());
            FeatureFlagsController controller = new(mock);
            string featureFlagName = "abc";

            //Act
            FeatureFlag result = controller.GetFeatureFlag(featureFlagName);

            //Assert
            Assert.IsTrue(result != null);
            TestFeatureFlag(result ?? new FeatureFlag());
        }

        [TestMethod]
        public void SaveFeatureFlagsUnitTest()
        {
            //Arrange
            IConfiguration mockConfiguration = Substitute.For<IConfiguration>();
            FeatureFlagsStorageTable context = new(mockConfiguration);
            IFeatureFlagsStorageTable mock = Substitute.For<IFeatureFlagsStorageTable>();
            mock.SaveFeatureFlag(Arg.Any<FeatureFlag>()).Returns(true);
            FeatureFlagsController controller = new(mock);
            FeatureFlag featureFlag = GetTestRow();

            //Act
            bool result = controller.SaveFeatureFlag(featureFlag);

            //Assert
            Assert.IsTrue(result == true);
        }

        [TestMethod]
        public void SaveFeatureFlagsStatePRUnitTest()
        {
            //Arrange
            IConfiguration mockConfiguration = Substitute.For<IConfiguration>();
            FeatureFlagsStorageTable context = new FeatureFlagsStorageTable(mockConfiguration);
            IFeatureFlagsStorageTable mock = Substitute.For<IFeatureFlagsStorageTable>();
            mock.GetFeatureFlag(Arg.Any<string>()).Returns(GetTestRow());
            FeatureFlagsController controller = new FeatureFlagsController(mock);
            FeatureFlag featureFlag = GetTestRow();

            //Act
            bool result = controller.SaveFeatureFlagState(featureFlag.Name, "pr", true);

            //Assert
            Assert.IsTrue(result == true);
        }

        [TestMethod]
        public void SaveFeatureFlagsStateDevUnitTest()
        {
            //Arrange
            IConfiguration mockConfiguration = Substitute.For<IConfiguration>();
            FeatureFlagsStorageTable context = new FeatureFlagsStorageTable(mockConfiguration);
            IFeatureFlagsStorageTable mock = Substitute.For<IFeatureFlagsStorageTable>();
            mock.GetFeatureFlag(Arg.Any<string>()).Returns(GetTestRow());
            FeatureFlagsController controller = new FeatureFlagsController(mock);
            FeatureFlag featureFlag = GetTestRow();

            //Act
            bool result = controller.SaveFeatureFlagState(featureFlag.Name, "dev", true);

            //Assert
            Assert.IsTrue(result == true);
        }

        [TestMethod]
        public void SaveFeatureFlagsStateQAUnitTest()
        {
            //Arrange
            IConfiguration mockConfiguration = Substitute.For<IConfiguration>();
            FeatureFlagsStorageTable context = new FeatureFlagsStorageTable(mockConfiguration);
            IFeatureFlagsStorageTable mock = Substitute.For<IFeatureFlagsStorageTable>();
            mock.GetFeatureFlag(Arg.Any<string>()).Returns(GetTestRow());
            FeatureFlagsController controller = new FeatureFlagsController(mock);
            FeatureFlag featureFlag = GetTestRow();

            //Act
            bool result = controller.SaveFeatureFlagState(featureFlag.Name, "qa", true);

            //Assert
            Assert.IsTrue(result == true);
        }

        [TestMethod]
        public void SaveFeatureFlagsStateProdUnitTest()
        {
            //Arrange
            IConfiguration mockConfiguration = Substitute.For<IConfiguration>();
            FeatureFlagsStorageTable context = new FeatureFlagsStorageTable(mockConfiguration);
            IFeatureFlagsStorageTable mock = Substitute.For<IFeatureFlagsStorageTable>();
            mock.GetFeatureFlag(Arg.Any<string>()).Returns(GetTestRow());
            FeatureFlagsController controller = new FeatureFlagsController(mock);
            FeatureFlag featureFlag = GetTestRow();

            //Act
            bool result = controller.SaveFeatureFlagState(featureFlag.Name, "prod", true);

            //Assert
            Assert.IsTrue(result == true);
        }

        [TestMethod]
        public void SaveFeatureFlagsStateExceptionUnitTest()
        {
            //Arrange
            IConfiguration mockConfiguration = Substitute.For<IConfiguration>();
            FeatureFlagsStorageTable context = new FeatureFlagsStorageTable(mockConfiguration);
            IFeatureFlagsStorageTable mock = Substitute.For<IFeatureFlagsStorageTable>();
            mock.GetFeatureFlag(Arg.Any<string>()).Returns(GetTestRow());
            FeatureFlagsController controller = new FeatureFlagsController(mock);
            FeatureFlag featureFlag = GetTestRow();

            //Act
            try
            {
                bool result = controller.SaveFeatureFlagState(featureFlag.Name, "exception", true);
            }
            catch (Exception)
            {
                //Assert
                Assert.IsTrue(true);
            }                 
        }


        [TestMethod]
        public void DeleteFeatureFlagsUnitTest()
        {
            //Arrange
            IConfiguration mockConfiguration = Substitute.For<IConfiguration>();
            FeatureFlagsStorageTable context = new FeatureFlagsStorageTable(mockConfiguration);
            IFeatureFlagsStorageTable mock = Substitute.For<IFeatureFlagsStorageTable>();
            mock.DeleteFeatureFlag(Arg.Any<string>()).Returns(true);
            FeatureFlagsController controller = new FeatureFlagsController(mock);
            FeatureFlag featureFlag = GetTestRow();

            //Act
            bool result = controller.DeleteFeatureFlag(featureFlag.Name);

            //Assert
            Assert.IsTrue(result == true);
        }

        private static void TestFeatureFlag(FeatureFlag featureFlags)
        {
            Assert.IsTrue(featureFlags.Name == "abc");
            Assert.IsTrue(featureFlags.Description == "def");
            Assert.IsTrue(featureFlags.PRIsEnabled == true);
            Assert.IsTrue(featureFlags.PRViewCount > 0);
            Assert.IsTrue(featureFlags.PRLastViewDate > DateTime.MinValue);
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

        private static IEnumerable<FeatureFlag> GetFeatureFlagsTestData()
        {
            List<FeatureFlag> featureFlags = new List<FeatureFlag>
            {
                GetTestRow()
            };
            return featureFlags;
        }

        private static FeatureFlag GetTestRow()
        {
            return new FeatureFlag("abc")
            {
                Name = "abc",
                Description = "def",
                PRIsEnabled = true,
                PRViewCount = 1,
                PRLastViewDate = DateTime.Now.AddDays(-1),
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
