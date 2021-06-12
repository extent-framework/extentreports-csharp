namespace AventStack.ExtentReports.Reporter.Filter
{
    public class EntityFilters<T> where T : AbstractReporter
    {
        public StatusFilter<T> StatusFilter; 
        private T _reporter;

        public EntityFilters(T reporter)
        {
            _reporter = reporter;
            StatusFilter = new StatusFilter<T>(this);
        }

        public T Apply()
        {
            return _reporter;
        }
    }
}
