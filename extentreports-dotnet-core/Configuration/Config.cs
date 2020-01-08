namespace AventStack.ExtentReports.Configuration
{
    public class Config
    {
        public string Key { get; private set; }
        public string Value { get; private set; }

        public Config(string k, string v)
        {
            Key = k;
            Value = v;
        }
    }
}
