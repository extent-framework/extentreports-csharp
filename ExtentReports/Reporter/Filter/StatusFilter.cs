using System.Collections.Generic;
using System.Linq;

namespace AventStack.ExtentReports.Reporter.Filter
{
    public class StatusFilter<T> where T : AbstractReporter
    {
        private EntityFilters<T> _configurer;

        public HashSet<Status> Status { get; private set; }

        public StatusFilter(EntityFilters<T> configurer)
        {
            _configurer = configurer;
        }

        public EntityFilters<T> As(HashSet<Status> status)
        {
            Status = status;
            return _configurer;
        }

        public EntityFilters<T> As(List<Status> status)
        {
            return As(new HashSet<Status>(status));
        }

        public EntityFilters<T> As(Status[] status)
        {
            return As(status.ToList());
        }
    }
}