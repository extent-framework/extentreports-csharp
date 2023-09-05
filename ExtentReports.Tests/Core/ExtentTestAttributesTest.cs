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
            Assert.AreEqual(0, _extent.CreateTest("Test").AssignAuthor().Test.Author.Count);
        }

        [Test]
        public void ExtentTestWithAuthor()
        {
            Assert.True(
                    _extent.CreateTest("Test")
                            .AssignAuthor("Author")
                            .Test
                            .Author
                            .Any(x => x.Name.Equals("Author")));
        }

        [Test]
        public void ExtentTestWithNoDevice()
        {
            Assert.AreEqual(0, _extent.CreateTest("Test").AssignDevice().Test.Device.Count);
        }

        [Test]
        public void ExtentTestWithDevice()
        {
            Assert.True(
                    _extent.CreateTest("Test")
                            .AssignDevice("Device")
                            .Test
                            .Device
                            .Any(x => x.Name.Equals("Device")));
        }

        [Test]
        public void ExtentTestWithNoCategory()
        {
            Assert.AreEqual(0, _extent.CreateTest("Test").AssignCategory().Test.Category.Count);
        }

        [Test]
        public void ExtentTestWithCategory()
        {
            Assert.True(
                    _extent.CreateTest("Test")
                            .AssignCategory("Tag")
                            .Test
                            .Category
                            .Any(x => x.Name.Equals("Tag")));
        }
    }
}
