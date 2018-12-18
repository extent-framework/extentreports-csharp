using System.Collections.Generic;

namespace AventStack.ExtentReports.Model
{
    internal static class StatusHierarchy
    {
        private static List<Status> _statusHierarchy = new List<Status>() {
            Status.Fatal,
            Status.Fail,
            Status.Error,
            Status.Warning,
            Status.Skip,
            Status.Pass,
            Status.Info,
            Status.Debug
        };

        public static List<Status> GetStatusHierarchy()
        {
            return _statusHierarchy;
        }
    }
}
