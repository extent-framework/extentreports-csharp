using System.Linq;
using System.Threading.Tasks;
using AventStack.ExtentReports.Model;
using AventStack.ExtentReports.Model.Context;
using NUnit.Framework;

namespace ExtentReports.Tests.Model.Context
{
    public class NamedAttributeContextTest
    {
        [Test]
        public void ConcurrentAddTest()
        {
            // initialize with a tag name and test
            var category = new Category("Category");
            var test = new Test("Test");
            var ctx = new NamedAttributeContext<Category>(category, test);
            int rangeFor = 1000;

            var rng = Enumerable.Range(0, rangeFor);
            Parallel.ForEach(rng, x =>
            {
                var test = new Test(x.ToString());
                ctx.AddTest(test);
            });
            Assert.AreEqual(1 + rangeFor, ctx.Count());
        }

        [Test]
        public void ConcurrentAddRemoveTest()
        {
            // initialize with a tag name and test
            var category = new Category("Category");
            var test = new Test("Test");
            var ctx = new NamedAttributeContext<Category>(category, test);
            int rangeFor = 1000;

            var rng = Enumerable.Range(0, rangeFor);
            Parallel.ForEach(rng, x =>
            {
                var test = new Test(x.ToString());
                ctx.AddTest(test);
                ctx.RemoveTest(test);
            });
            Assert.AreEqual(1, ctx.Count());
        }
    }
}
