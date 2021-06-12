using AventStack.ExtentReports.Model;
using NUnit.Framework;
using System.Linq;

namespace AventStack.ExtentReports.Tests.Core
{
    public class ExtentTestAttributesTest
    {
        private ExtentReports _extent;

        [SetUp]
        public void Setup()
        {
            _extent = new ExtentReports();
        }

        [Test]
        public void ExtentTestWithNoAuthor()
        {
            Assert.AreEqual(0, _extent.CreateTest("Test").AssignAuthor().Model.Author.Count);
        }

        [Test]
        public void ExtentTestWithAuthor()
        {
            Assert.True(
                    _extent.CreateTest("Test")
                            .AssignAuthor("Author")
                            .Model
                            .Author
                            .Any(x => x.Name.Equals("Author")));
        }

        [Test]
        public void ExtentTestWithNoDevice()
        {
            Assert.AreEqual(0, _extent.CreateTest("Test").AssignDevice().Model.Device.Count);
        }

        [Test]
        public void ExtentTestWithDevice()
        {
            Assert.True(
                    _extent.CreateTest("Test")
                            .AssignDevice("Device")
                            .Model
                            .Device
                            .Any(x => x.Name.Equals("Device")));
        }

        [Test]
        public void ExtentTestWithNoCategory()
        {
            Assert.AreEqual(0, _extent.CreateTest("Test").AssignCategory().Model.Category.Count);
        }

        [Test]
        public void ExtentTestWithCategory()
        {
            Assert.True(
                    _extent.CreateTest("Test")
                            .AssignCategory("Tag")
                            .Model
                            .Category
                            .Any(x => x.Name.Equals("Tag")));
        }
    }
}
