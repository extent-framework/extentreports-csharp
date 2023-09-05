using AventStack.ExtentReports.Model;
using System.Collections.Generic;

namespace AventStack.ExtentReports.Reporter.Filter
{
    public interface IStatusFilterable<T>
    {
        Report FilterReport(Report report, HashSet<Status> set);
    }
}
