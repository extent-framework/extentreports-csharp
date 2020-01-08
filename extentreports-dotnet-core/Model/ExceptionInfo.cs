using System;

namespace AventStack.ExtentReports.Model
{
    [Serializable]
    public class ExceptionInfo : Attribute
    {
        public Exception Exception { get; private set; }
        
        public ExceptionInfo(Exception ex) : base(ex.GetType().FullName)
        {
            Exception = ex;
        }
    }
}
