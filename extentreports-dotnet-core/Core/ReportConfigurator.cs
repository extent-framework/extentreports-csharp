using System;

namespace AventStack.ExtentReports.Core
{
    public class ReportConfigurator
    {
        private static readonly Lazy<ReportConfigurator> lazy = new Lazy<ReportConfigurator>(() => new ReportConfigurator());

        public static ReportConfigurator I { get { return lazy.Value; } }

        private ReportConfigurator()
        {
        }

        public StatusConfigurator StatusConfigurator
        {
            get
            {
                return StatusConfigurator.I;
            }
        }
    }
}