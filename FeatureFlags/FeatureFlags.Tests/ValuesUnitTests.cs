using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Moq;
using FeatureFlags.Service.Controllers;

namespace FeatureFlags.Tests
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestClass]
    public class ValuesUnitTests
    {
        [TestMethod]
        public void GetValuesTest()
        {
            //Arrange
            Mock<ValuesController> mock = new Mock<ValuesController>();
            ValuesController controller = new ValuesController();

            //Act
            IEnumerable<string> items = controller.Get();

            //Assert
            Assert.IsTrue(items != null);
            Assert.IsTrue(items.Count() == 2);
            Assert.IsTrue(items.FirstOrDefault<string>() == "value1");
        }

        [TestMethod]
        public void GetValueTest()
        {
            //Arrange
            ValuesController controller = new ValuesController();
            int id = 1;

            //Act
            string item = controller.Get(id);

            //Assert
            Assert.IsTrue(item != null);
            Assert.IsTrue(item == "value");
        }

        [TestMethod]
        public void PostValueTest()
        {
            //Arrange
            ValuesController controller = new ValuesController();
            string body = "data";

            //Act
            bool result = controller.Post(body);

            //Assert
            Assert.IsTrue(result == true);
        }

        [TestMethod]
        public void PutValueTest()
        {
            //Arrange
            ValuesController controller = new ValuesController();
            int id = 1;
            string body = "data";

            //Act
            bool result = controller.Put(id, body);

            //Assert
            Assert.IsTrue(result == true);
        }



        [TestMethod]
        public void DeleteValueTest()
        {
            //Arrange
            ValuesController controller = new ValuesController();
            int id = 1;

            //Act
            bool result = controller.Delete(id);

            //Assert
            Assert.IsTrue(result == true);
        }
    }
}
