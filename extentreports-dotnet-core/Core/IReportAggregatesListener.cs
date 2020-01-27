using AventStack.ExtentReports;
using AventStack.ExtentReports.Model;

using System.Collections.Generic;

namespace AventStack.ExtentReports.Core
{
    public interface IReportAggregatesListener
    {
        List<Test> TestList { set; }

        List<string> TestRunnerLogs { set; }
        
        ReportStatusStats ReportStatusStats { set; }

        List<Status> StatusList { set; }
    }
}
