using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtentReports.Tests.ParallelSupport
{
    [Parallelizable(ParallelScope.Children)]
    public class ParallelTest1
    {
        [Test]
        public void Test1()
        {
            Assert.AreEqual(true, true);
        }

        [Test]
        public void Test2()
        {
            Assert.AreEqual(true, true);
        }

        [Test]
        public void Test3()
        {
            Assert.AreEqual(true, true);
        }

        [Test]
        public void Test4()
        {
            Assert.AreEqual(true, true);
        }

        [Test]
        public void Test5()
        {
            Assert.AreEqual(true, true);
        }

        [Test]
        public void Test6()
        {
            Assert.AreEqual(true, true);
        }

        [Test]
        public void Test7()
        {
            Assert.AreEqual(true, true);
        }

        [Test]
        public void Test8()
        {
            Assert.AreEqual(true, true);
        }

        [Test]
        public void Test9()
        {
            Assert.AreEqual(true, true);
        }
    }
}
