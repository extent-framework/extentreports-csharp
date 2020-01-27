using System;
using System.Collections.Generic;

namespace AventStack.ExtentReports.Core
{
    public class StatusConfigurator
    {
        private static readonly Lazy<StatusConfigurator> lazy = new Lazy<StatusConfigurator>(() => new StatusConfigurator());

        public static StatusConfigurator I { get { return lazy.Value; } }

        private StatusConfigurator()
        {
        }

        public List<Status> StatusHierarchy
        {
            get
            {
                return Model.StatusHierarchy.GetStatusHierarchy();
            }
            set
            {
                Model.StatusHierarchy.SetStatusHierarchy(value);
            }
        }
    }
}