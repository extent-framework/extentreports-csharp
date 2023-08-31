using System.Linq;

namespace AventStack.ExtentReports.Extensions
{
    public static class StatusExtensions
    {
        public static Status Min(this Status s1, Status s2)
        {
            return Max(s1, s2) == s1 ? s2 : s1;
        }

        public static Status Min(Status[] status)
        {
            return status.Min();
        }

        public static Status Max(this Status s1, Status s2)
        {
            return s1 > s2 ? s1 : s2;
        }

        public static Status Max(Status[] status)
        {
            return status.Max();
        }
    }
}
