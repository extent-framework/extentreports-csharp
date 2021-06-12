using System.Linq;
using System.Threading.Tasks;
using AventStack.ExtentReports.Model;
using AventStack.ExtentReports.Model.Context.Manager;
using NUnit.Framework;

namespace ExtentReports.Tests.Model.Context.Manager
{
    public class NamedAttributeContextManagerTest
    {
        [Test]
        public void ConcurrentAddContextTest()
        {
            var mgr = new NamedAttributeContextManager<Category>();
            int rangeFor = 100;

            var rng = Enumerable.Range(0, rangeFor);
            Parallel.ForEach(rng, x =>
            {
                var category = new Category(x.ToString());
                var test = new Test(x.ToString());
                mgr.AddContext(category, test);
            });

            for (int i = 0; i < rangeFor; i++)
            {
                Assert.AreEqual(1, mgr.Context[i.ToString()].Count());
            }            
        }

        [Test]
        public void ConcurrentAddContextTests()
        {
            var mgr = new NamedAttributeContextManager<Category>();
            var category = new Category("Category");
            var test = new Test("Test");
            mgr.AddContext(category, test);
            int rangeFor = 100;

            var rng = Enumerable.Range(0, rangeFor);
            Parallel.ForEach(rng, x =>
            {
                var test = new Test(x.ToString());
                mgr.AddContext(category, test);
            });

            Assert.AreEqual(1 + rangeFor, mgr.Context[category.Name].Count());
        }
    }
}
