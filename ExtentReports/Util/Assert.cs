using System;

namespace AventStack.ExtentReports.Util
{
    internal static class Assert
    {
        public static void NotNull(Object o, string message)
        {
            if (o == null)
            {
                throw new ArgumentNullException(message);
            }
        }

        public static void NotEmpty(string s, string message)
        {
            if (s == null || s.Length == 0)
            {
                throw new ArgumentException(message);
            }
        }
    }
}
