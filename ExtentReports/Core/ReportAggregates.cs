using AventStack.ExtentReports.Model;

using System.Collections.Generic;

namespace AventStack.ExtentReports
{
    public class ReportAggregates
    {
        public List<Test> TestList { get; internal set; }

        public List<string> TestRunnerLogs { get; internal set; }

        public ReportStatusStats ReportStatusStats { get; internal set; }

        public List<Status> StatusList { get; internal set; }

        public TestAttributeTestContextProvider<Author> AuthorContext { get; internal set; }

        public TestAttributeTestContextProvider<Category> CategoryContext { get; internal set; }

        public TestAttributeTestContextProvider<Device> DeviceContext { get; internal set; }

        public TestAttributeTestContextProvider<ExceptionInfo> ExceptionInfoContext { get; internal set; }
        
        public SystemAttributeContext SystemAttributeContext { get; internal set; }
    }
}
