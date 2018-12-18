using RazorEngine.Templating;

namespace AventStack.ExtentReports
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
