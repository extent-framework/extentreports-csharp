using AventStack.ExtentReports.Model;

using System;

namespace AventStack.ExtentReports
{
    /// <summary>
    /// List of allowed status for <see cref="Log"/>
    /// </summary>
    [Serializable]
    public enum Status
    {
        Pass,
        Fail,
        Fatal,
        Error,
        Warning,
        Info,
        Skip,
        Debug
    }
}
