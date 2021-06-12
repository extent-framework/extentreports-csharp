using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;

namespace AventStack.ExtentReports.Tests.Usage.Core
{
    internal class ExtentTestManager
    {
        private static Dictionary<string, ExtentTest> Parent = new Dictionary<string, ExtentTest>();
        public static ThreadLocal<ExtentTest> Test = new ThreadLocal<ExtentTest>();

        private static readonly object _synclock = new object();

        public static ExtentTest CreateTest()
        {
            var className = TestContext.CurrentContext.Test.ClassName;
            var testName = TestContext.CurrentContext.Test.Name;

            lock (_synclock)
            {
                if (!Parent.ContainsKey(className))
                {
                    Parent[className] = ExtentManager.Instance.CreateTest(className);
                }

                Test.Value = Parent[className].CreateNode(testName);
            }

            return Test.Value;
        }
    }
}
