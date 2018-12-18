using System.Linq;
using System.Collections.Generic;

using AventStack.ExtentReports.Model;

namespace AventStack.ExtentReports
{
    internal class TestRemoveService
    {
        private static bool _removed = false;

        public static void Remove(List<Test> testList, Test test)
        {
            _removed = false;
            FindAndRemoveTest(testList, test);
        }

        private static void FindAndRemoveTest(List<Test> testList, Test test)
        {
            var filteredTestList = testList.Where(x => x.TestId == test.TestId).ToList();

            if (filteredTestList.Count == 1)
            {
                RemoveTest(testList, test);
                _removed = true;
                return;
            }

            foreach (var t in filteredTestList)
            {
                if (_removed)
                    return;
                FindAndRemoveTest(t.NodeContext.All(), test);
            }          
        }

        private static void RemoveTest(List<Test> testList, Test test)
        {
            testList.Remove(test);
        }
    }
}
