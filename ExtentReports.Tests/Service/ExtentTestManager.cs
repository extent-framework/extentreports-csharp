using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AventStack.ExtentReports.Tests.Parallel
{
    public class ExtentTestManager
    {
        private static Dictionary<string, ExtentTest> _parentTestMap = new Dictionary<string, ExtentTest>();
        private static ThreadLocal<ExtentTest> _parentTest = new ThreadLocal<ExtentTest>();
        private static ThreadLocal<ExtentTest> _childTest = new ThreadLocal<ExtentTest>();

        private static readonly object _synclock = new object();

        public static ExtentTest CreateTest(string testName, string description = null)
        {
            lock (_synclock)
            {
                _parentTest.Value = ExtentService.Instance.CreateTest(testName, description);
                return _parentTest.Value;
            }
        }

        public static ExtentTest CreateMethod(string parentName, string testName, string description = null)
        {
            lock (_synclock)
            {
                ExtentTest parentTest = null;
                if (!_parentTestMap.ContainsKey(parentName))
                {
                    parentTest = ExtentService.Instance.CreateTest(testName);
                    _parentTestMap.Add(parentName, parentTest);
                } else
                {
                    parentTest = _parentTestMap[parentName];
                }
                _parentTest.Value = parentTest;
                _childTest.Value = parentTest.CreateNode(testName, description);
                return _childTest.Value;
            }
        }

        public static ExtentTest CreateMethod(string testName)
        {
            lock (_synclock)
            {
                _childTest.Value = _parentTest.Value.CreateNode(testName);
                return _childTest.Value;
            }
        }

        public static ExtentTest GetTest()
        {
            lock (_synclock)
            {
                return _childTest.Value;
            }
        }
    }
}
