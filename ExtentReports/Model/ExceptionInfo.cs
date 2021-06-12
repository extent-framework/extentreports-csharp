using System;

namespace AventStack.ExtentReports.Model
{
    public class ExceptionInfo: NamedAttribute
    {
        public Exception Exception { get; set; }

        public ExceptionInfo(Exception ex) : base(ex.GetType().FullName)
        {
            Exception = ex;
        }
    }
}
