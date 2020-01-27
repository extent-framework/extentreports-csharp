using RazorEngine.Templating;

namespace AventStack.ExtentReports.Core
{
    public class RazorEngineTemplate<T> : TemplateBase<T>
    {
        public new T Model
        {
            get { return base.Model; }
            set { base.Model = value; }
        }
    }
}
