using AventStack.ExtentReports.Model;
using NUnit.Framework;

namespace AventStack.ExtentReports.Tests.Model
{
    class AuthorEntityTest
    {
        [Test]
        public void AuthorName()
        {
            var name = "Anshoo";
            var author = new Author(name);
            Assert.AreEqual(name, author.Name);
        }
    }
}
