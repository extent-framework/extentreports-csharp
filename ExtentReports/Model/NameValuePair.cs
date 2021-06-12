namespace AventStack.ExtentReports.Model
{
    public abstract class NameValuePair : NamedAttribute, IBaseEntity
    {
        public string Value { get; set; }

        public NameValuePair(string name, string value) : base(name)
        {
            Value = value;
        }
    }
}
