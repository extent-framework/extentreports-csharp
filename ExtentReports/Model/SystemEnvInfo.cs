namespace AventStack.ExtentReports.Model
{
    public class SystemEnvInfo : NameValuePair, IBaseEntity
    {
        public SystemEnvInfo(string name, string value) : base(name, value)
        {
        }
    }
}
