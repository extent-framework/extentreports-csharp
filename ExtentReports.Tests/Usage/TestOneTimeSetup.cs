using AventStack.ExtentReports.Tests.Usage.Core;
using NUnit.Framework;

namespace AventStack.ExtentReports.Tests.Usage
{
    [SetUpFixture]
    public class TestOneTimeSetup
    {
        [OneTimeTearDown]
        protected void Teardown()
        {
            ExtentManager.Instance.Flush();
        }
    }
}
