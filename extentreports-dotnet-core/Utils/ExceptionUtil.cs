using System;
using System.Text.RegularExpressions;

namespace AventStack.ExtentReports.Utils
{
    public static class ExceptionUtil
    {
        public static string GetExceptionHeadline(Exception ex)
        {
            var regex = new Regex("([\\w\\.]+)");
            var match = regex.Match(ex.ToString());

            if (match.Success)
            {
                return match.Value;
            }

            return null;
        }
    }
}
