using System.Linq;
using System.Threading.Tasks;
using AventStack.ExtentReports.Model;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;

namespace AventStack.ExtentReports.Tests.Core
{
    public class ConcurrentExtentTestTest
    {
        private ExtentReports _extent;

        [SetUp]
        public void Setup()
        {
            _extent = new ExtentReports();
        }

        [Test]
        public void ParallelCreateTests()
        {
            var rng = Enumerable.Range(0, 1000);
            Parallel.ForEach(rng, x => _extent.CreateTest("Test").Info(""));
        }

        [Test]
        public void ParallelCreateTestsWithTags()
        {
            var rng = Enumerable.Range(0, 1000);
            Parallel.ForEach(rng, x => {
                _extent.CreateTest("Test")
                    .AssignCategory(new string[] { "t1", "t2" })
                    .Pass("");
            });
        }

        [Test]
        public void ParallelCreateTestsWithReporter()
        {
            _extent.AttachReporter(new ExtentSparkReporter(""));
            var rng = Enumerable.Range(0, 10000);
            Parallel.ForEach(rng, x => _extent.CreateTest("Test").Info(""));
        }

        [Test]
        public void ParallelLogs()
        {
            var test = _extent.CreateTest("Test");
            var rng = Enumerable.Range(0, 1000);
            Parallel.ForEach(rng, x => test.Info(""));
        }
    }
}
