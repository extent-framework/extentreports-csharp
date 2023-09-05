using NUnit.Framework;

namespace AventStack.ExtentReports.Tests.Usage
{
    public class Login : Base
    {
        [Test]
        public void AssertTrue()
        {
            Assert.True(true);
        }

        [Test]
        public void AssertFalse()
        {
            //Assert.False(true);
        }
    }
}
