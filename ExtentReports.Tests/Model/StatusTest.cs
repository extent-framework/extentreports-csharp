using AventStack.ExtentReports.Extensions;
using AventStack.ExtentReports.Model;
using NUnit.Framework;

namespace AventStack.ExtentReports.Tests.Model
{
    public class StatusTest
    {
        private Status[] RandomHierarchy()
        {
            var arr = (Status[]) Status.GetValues(typeof(Status));
            return arr;
        }

        [Test]
        public void StatusMax()
        {
            Assert.AreEqual(Status.Fail, StatusExtensions.Max(RandomHierarchy()));
        }

        [Test]
        public void StatusMin()
        {
            Assert.AreEqual(Status.Info, StatusExtensions.Min(RandomHierarchy()));
        }
    }
}
