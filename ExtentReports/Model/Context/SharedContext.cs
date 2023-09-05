using AventStack.ExtentReports.Model.Context.Manager;

namespace AventStack.ExtentReports.Model.Context
{
    public class SharedContext<T> where T : NamedAttribute
    {
        public NamedAttributeContextManager<T> Ctx { get; set; }
        public string View { get; set; }
    }
}
