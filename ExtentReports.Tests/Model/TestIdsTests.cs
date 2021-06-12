using System.Collections.Generic;
using System.Linq;
using AventStack.ExtentReports.Model;
using NUnit.Framework;

namespace AventStack.ExtentReports.Tests.Model
{
    public class TestIdsTests
    {
        [Test]
        public void TestIdTest()
        {
            var incr = 100;
            var set = new HashSet<int>();
            Enumerable.Range(0, incr).ToList().ForEach(x => set.Add(new Test(Name).Id));
            Assert.AreEqual(incr, set.Count);
        }

        private const string Name = "Test";
    }
}
