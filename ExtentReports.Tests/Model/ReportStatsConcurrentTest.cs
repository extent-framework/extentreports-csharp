using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AventStack.ExtentReports.Model;
using NUnit.Framework;

namespace AventStack.ExtentReports.Tests.Model
{
    public class ReportStatsConcurrentTest
    {
        [Test]
        public void ConcurrentUpdateFlush()
        {
            var extent = new ExtentReports();
            IEnumerable<int> rng = Enumerable.Range(0, 1000);
            Parallel.ForEach(rng, x => {
                extent.CreateTest("Test").Pass("");
                extent.Flush();
            });
            Assert.AreEqual(1000, extent.Report.Tests.Count);
        }
    }
}
