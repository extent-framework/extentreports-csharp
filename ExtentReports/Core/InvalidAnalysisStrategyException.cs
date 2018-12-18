using System;

namespace AventStack.ExtentReports
{
    [Serializable]
    internal class InvalidAnalysisStrategyException : Exception
    {
        public InvalidAnalysisStrategyException()
        {
        }

        public InvalidAnalysisStrategyException(string message) : base(message)
        {
        }

        public InvalidAnalysisStrategyException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}