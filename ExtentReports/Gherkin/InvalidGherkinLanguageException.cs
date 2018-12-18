using System;

namespace AventStack.ExtentReports.Gherkin
{
    [Serializable]
    public class InvalidGherkinLanguageException : ArgumentException
    {
        public InvalidGherkinLanguageException() : base() { }
        public InvalidGherkinLanguageException(string message) : base(message) { }
        public InvalidGherkinLanguageException(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client. 
        protected InvalidGherkinLanguageException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
        { }
    }
}
