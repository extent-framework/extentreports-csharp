using System.Threading;

using NUnit.Framework;

namespace AventStack.ExtentReports.Tests.Parallel.FixtureScope
{
    [TestFixture, Parallelizable(ParallelScope.Fixtures)]
    public class TestFixture2 : BaseFixture
    {
        [Test]
        public void TestMethod1()
        {
            Thread.Sleep(100);
            Assert.IsTrue(true);
        }

        [Test]
        public void TestMethod2()
        {
            Thread.Sleep(200);
            Assert.IsTrue(true);
        }

        [Test]
        public void TestMethod3()
        {
            Assert.IsTrue(true);
        }

        [Test]
        public void TestMethod4()
        {
            Assert.IsTrue(true);
        }

        [Test]
        public void TestMethod5()
        {
            Assert.IsTrue(true);
        }

        [Test]
        public void TestMethod6()
        {
            Assert.IsTrue(true);
        }

        [Test]
        public void TestMethod7()
        {
            Assert.IsTrue(true);
        }

        [Test]
        public void TestMethod8()
        {
            Assert.IsTrue(true);
        }

        [Test]
        public void TestMethod9()
        {
            Assert.IsTrue(true);
        }
    }
}
