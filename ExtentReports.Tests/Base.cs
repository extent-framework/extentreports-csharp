using System;
using System.IO;

using AventStack.ExtentReports.Reporter;

using NUnit.Framework;

namespace AventStack.ExtentReports.Tests
{
    [SetUpFixture]
    public abstract class Base
    {
        protected ExtentReports _extent;

        [OneTimeSetUp]
        protected void Setup()
        {
            string dir = TestContext.CurrentContext.TestDirectory + "\\";
            var fileName = this.GetType().ToString() + ".html";
            ExtentV3HtmlReporter htmlReporter = new ExtentV3HtmlReporter(dir + fileName);

            _extent = new ExtentReports();
            _extent.AttachReporter(htmlReporter);
        }

        [OneTimeTearDown]
        protected void TearDown()
        {
            Console.WriteLine("in teardown");
            _extent.Flush();
        }
    }
}
