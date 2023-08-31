using System.Collections.Generic;
using System.Linq;
using AventStack.ExtentReports.Model;
using NUnit.Framework;

namespace AventStack.ExtentReports.Tests.Core
{
    class TestIdTest
    {
        private static readonly int Attempts = 10000;

        [Test]
        public void AllTestsHaveUniqueId()
        {
            var extent = new ExtentReports();
            var set = new HashSet<int>();
            Enumerable.Range(0, Attempts).ToList().ForEach(x => set.Add(extent.CreateTest("test").Test.Id));
            Assert.AreEqual(Attempts, set.Count);
        }
    }
}
