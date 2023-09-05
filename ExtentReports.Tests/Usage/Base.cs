using AventStack.ExtentReports.MarkupUtils;
using AventStack.ExtentReports.Tests.Usage.Core;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace AventStack.ExtentReports.Tests.Usage
{
    public class Base
    {
        [SetUp]
        protected void Setup()
        {
            ExtentTestManager.CreateTest();
        }

        [TearDown]
        protected void Teardown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                ExtentTestManager.Test.Value.Fail(MarkupHelper.CreateCodeBlock(TestContext.CurrentContext.Result.Message));
            }
            else if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Inconclusive || TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Warning)
            {
                ExtentTestManager.Test.Value.Warning("Warning");
            }
            else if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Skipped)
            {
                ExtentTestManager.Test.Value.Skip("Skip");
            }
            else
            {
                ExtentTestManager.Test.Value.Pass("Pass");
            }

        }
    }
}
