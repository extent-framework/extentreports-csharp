using System;

namespace AventStack.ExtentReports.Gherkin
{
    [Serializable]
    public class InvalidGherkinLanguageException : ArgumentException
    {
        public InvalidGherkinLanguageException() : base() { }
        public InvalidGherkinLanguageException(string message) : base(message) { }

        protected InvalidGherkinLanguageException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
        { }
    }
}
