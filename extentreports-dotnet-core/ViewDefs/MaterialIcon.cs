using System;
using System.Collections.Generic;

namespace AventStack.ExtentReports.ViewDefs
{
    public static class MaterialIcon
    {
        private static Dictionary<Status, string> _dic = new Dictionary<Status, string>();

        public static void Override(Status status, string icon)
        {
            _dic.Add(status, icon);
        }

        public static string GetIcon(Status status)
        {
            if (_dic.ContainsKey(status))
                return _dic[status];

            switch (status)
            {
                case Status.Fail:
                    return "cancel";
                case Status.Fatal:
                    return "cancel";
                case Status.Error:
                    return "error";
                case Status.Warning:
                    return"warning";
                case Status.Skip:
                    return "redo";
                case Status.Pass:
                    return "check_circle";
                case Status.Debug:
                    return "low_priority";
                case Status.Info:
                    return "info_outline";
                default:
                    return "help";
            }
        }
    }
}
