using NUnit.Framework;

using System.Linq;

namespace AventStack.ExtentReports.Tests.APITests
{
    [TestFixture]
    public class TestAttributesTests : Base
    {
        private string[] _categories = {
            "_extent",
            "git",
            "tests",
            "heroku"
        };
        private string[] _authors = {
            "anshoo",
            "viren",
            "maxi",
            "vimal"
        };

        [Test]
        public void verifyIfTestHasAddedCategory()
        {
            var test = _extent.CreateTest(TestContext.CurrentContext.Test.Name).AssignCategory(_categories[0]);
            test.Pass("Pass");

            Assert.AreEqual(test.Model.CategoryContext.Count, 1);
            Assert.AreEqual(test.Model.CategoryContext.Get(0).Name, _categories[0]);
        }

        [Test]
        public void verifyIfTestHasAddedCategories()
        {
            var test = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            _categories.ToList().ForEach(c => test.AssignCategory(c));
            test.Pass("Pass");

            Assert.AreEqual(test.Model.CategoryContext.Count, _categories.Length);

            var categoryCollection = test.Model.CategoryContext.All();
            _categories.ToList().ForEach(c => {
                var result = categoryCollection.Any(x => x.Name == c);
                Assert.True(result);
            });
        }

        [Test]
        public void verifyIfTestHasAddedAuthor()
        {
            var test = _extent.CreateTest(TestContext.CurrentContext.Test.Name).AssignAuthor(_authors[0]);
            test.Pass("Pass");

            Assert.AreEqual(test.Model.AuthorContext.Count, 1);
            Assert.AreEqual(test.Model.AuthorContext.Get(0).Name, _authors[0]);
        }

        [Test]
        public void verifyIfTestHasAddedAuthors()
        {
            var test = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            _authors.ToList().ForEach(a => test.AssignAuthor(a));
            test.Pass("Pass");

            Assert.AreEqual(test.Model.AuthorContext.Count, _authors.Length);

            var authorCollection = test.Model.AuthorContext.All();
            _authors.ToList().ForEach(a => {
                var result = authorCollection.Any(x => x.Name == a);
                Assert.True(result);
            });
        }
    }
}
