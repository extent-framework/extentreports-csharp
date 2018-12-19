using System;

namespace AventStack.ExtentReports.Core
{
    [Serializable]
    internal class GherkinKeywordNotFoundException : Exception
    {
        public GherkinKeywordNotFoundException()
        {
        }

        public GherkinKeywordNotFoundException(string message) : base(message)
        {
        }

        public GherkinKeywordNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}