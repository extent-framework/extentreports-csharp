using AventStack.ExtentReports.Utils;

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

        public static void SetStatusHierarchy(List<Status> statusHierarchy)
        {
            _statusHierarchy = statusHierarchy;
        }

        public static Status GetHighestStatus(List<Status> list)
        {
            var highestStatus = Status.Pass;
            if (list.IsNullOrEmpty())
            {
                return highestStatus;
            }

            foreach (var status in list)
            {
                highestStatus = GetStatusHierarchy().IndexOf(status) < GetStatusHierarchy().IndexOf(highestStatus)? status: highestStatus;
            }
            return highestStatus;
        }
    }
}
