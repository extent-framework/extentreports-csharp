using System.Linq;
using System.Threading.Tasks;
using AventStack.ExtentReports.Model;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace AventStack.ExtentReports.Tests.Reporter
{
    public class SparkReporterConcurrentTest
    {
        private ExtentReports _extent;

        [SetUp]
        public void Setup()
        {
            _extent = new ExtentReports();
            _extent.AttachReporter(new ExtentSparkReporter("SparkReporterConcurrentTest.html"));
        }

        [Test]
        public void ParallelCreateTestsAndFlush()
        {
            var rng = Enumerable.Range(0, 100);
            Parallel.ForEach(rng, new ParallelOptions { MaxDegreeOfParallelism = 4 }, x => {
                _extent.CreateTest("Test").Info("");
                if (x % 5 == 0)
                {
                    _extent.Flush();
                }
            });
        }
    }
}
