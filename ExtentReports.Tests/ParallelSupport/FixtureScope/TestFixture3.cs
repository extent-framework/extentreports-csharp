using System.Threading;

using NUnit.Framework;

namespace AventStack.ExtentReports.Tests.Parallel.FixtureScope
{
    [TestFixture, Parallelizable(ParallelScope.All)]
    public class TestFixture3 : BaseFixture
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
            Thread.Sleep(100);
            Assert.IsTrue(true);
        }

        [Test]
        public void TestMethod8()
        {
            Thread.Sleep(200);
            Assert.IsTrue(true);
        }

        [Test]
        public void TestMethod9()
        {
            Assert.IsTrue(true);
        }

        [Test]
        public void TestMethod10()
        {
            Thread.Sleep(100);
            Assert.IsTrue(true);
        }

        [Test]
        public void TestMethod11()
        {
            Thread.Sleep(200);
            Assert.IsTrue(true);
        }

        [Test]
        public void TestMethod12()
        {
            Assert.IsTrue(true);
        }

        [Test]
        public void TestMethod13()
        {
            Assert.IsTrue(true);
        }

        [Test]
        public void TestMethod14()
        {
            Assert.IsTrue(true);
        }

        [Test]
        public void TestMethod15()
        {
            Assert.IsTrue(true);
        }

        [Test]
        public void TestMethod16()
        {
            Thread.Sleep(100);
            Assert.IsTrue(true);
        }

        [Test]
        public void TestMethod17()
        {
            Thread.Sleep(200);
            Assert.IsTrue(true);
        }

        [Test]
        public void TestMethod18()
        {
            Assert.IsTrue(true);
        }

        [Test]
        public void TestMethod19()
        {
            Assert.IsTrue(true);
        }

        [Test]
        public void TestMethod20()
        {
            Assert.IsTrue(true);
        }
    }
}
