using NUnit.Framework;
using System;
using System.Linq;

namespace AventStack.ExtentReports.Tests.APITests
{
    [TestFixture]
    public class NodeAttributesTests : Base
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
    public void verifyIfNodeHasAddedCategory() {
        var node = _extent
                .CreateTest(TestContext.CurrentContext.Test.Name)
                .CreateNode("Child")
                .AssignCategory(_categories[0])
                .Pass("pass");
        
        Assert.AreEqual(node.Model.CategoryContext.Count, 1);
        var c = node.Model.CategoryContext.Get(0);
        Assert.AreEqual(c.Name, _categories[0]);
    }
    
    [Test]
    public void verifyIfTestHasAddedCategories() {
        var node = _extent.CreateTest(TestContext.CurrentContext.Test.Name).CreateNode("Child").Pass("pass");        
        _categories.ToList().ForEach(c => node.AssignCategory(c));
               
        Assert.AreEqual(node.Model.CategoryContext.Count, _categories.Length);

        var categoryCollection = node.Model.CategoryContext.All();
        _categories.ToList().ForEach(c => {
            Boolean result = categoryCollection.Any(x => x.Name == c); 
            Assert.True(result);
        });
    }
    
    [Test]
    public void verifyIfTestHasAddedAuthor() {
        var node = _extent
                .CreateTest(TestContext.CurrentContext.Test.Name)
                .CreateNode("Child")
                .AssignAuthor(_authors[0])
                .Pass("pass");
        
        Assert.AreEqual(node.Model.AuthorContext.Count, 1);
        Assert.AreEqual(node.Model.AuthorContext.Get(0).Name, _authors[0]);
    }
    
    [Test]
    public void verifyIfTestHasAddedAuthors() {
        var node = _extent
                .CreateTest(TestContext.CurrentContext.Test.Name)
                .CreateNode("Child")
                .Pass("pass");
        _authors.ToList().ForEach(a => node.AssignAuthor(a));
               
        Assert.AreEqual(node.Model.AuthorContext.Count, _authors.Length);

        var authorCollection = node.Model.AuthorContext.All();
        _authors.ToList().ForEach(a => {
            Boolean result = authorCollection.Any(x => x.Name == a); 
            Assert.True(result);
        });
    }
    }
}
