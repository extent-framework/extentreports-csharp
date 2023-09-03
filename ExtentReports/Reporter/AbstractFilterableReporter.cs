using AventStack.ExtentReports.Extensions;
using AventStack.ExtentReports.Model;
using AventStack.ExtentReports.Model.ExtensionMethods;
using AventStack.ExtentReports.Reporter.Filter;
using AventStack.ExtentReports.Util;
using System.Collections.Generic;
using System.Linq;

namespace AventStack.ExtentReports.Reporter
{
    public abstract class AbstractFilterableReporter<T> : AbstractReporter, IStatusFilterable<T>
    {
        public Report FilterReport(Report report, HashSet<Status> set)
        {
            Assert.NotNull(report, "Report must not be empty");

            if (set != null)
            {
                var cloned = report.DeepClone();
                cloned.Tests.Clear();

                var tests = report.Tests.Where(x => set.Contains(x.Status));

                foreach (Test test in tests)
                {
                    cloned.AddTest(test);
                }

                RefreshContext(cloned);

                return cloned;
            }

            return report;
        }

        private void RefreshContext(Report cloned)
        {
            var list = cloned.Tests.ToList();

            foreach (var test in list)
            {
                foreach (var x in test.Author)
                {
                    cloned.AuthorCtx.AddContext(x, test);
                }
                foreach (var x in test.Category)
                {
                    cloned.CategoryCtx.AddContext(x, test);
                }
                foreach (var x in test.Device)
                {
                    cloned.DeviceCtx.AddContext(x, test);
                }
                foreach (var x in test.ExceptionInfo)
                {
                    cloned.ExceptionInfoCtx.AddContext(x, test);
                }
            }

            cloned.Refresh();
        }
    }
}
