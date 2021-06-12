using AventStack.ExtentReports.Model;
using NUnit.Framework;

namespace AventStack.ExtentReports.Tests.Model
{
    class CategoryEntityTest
    {
        [Test]
        public void CategoryName()
        {
            var name = "TagName";
            var category = new Category(name);
            Assert.AreEqual(name, category.Name);
        }
    }
}
