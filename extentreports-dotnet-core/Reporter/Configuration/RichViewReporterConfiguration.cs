using System;

namespace AventStack.ExtentReports.Reporter.Configuration
{
    public class RichViewReporterConfiguration : BasicFileConfiguration
    {
        public RichViewReporterConfiguration(AbstractReporter reporter) : base(reporter)
        { }

        public Theme Theme
        {
            get
            {
                return _theme;
            }
            set
            {
                _theme = value;
                UserConfigurationMap["theme"] = Enum.GetName(typeof(Theme), _theme).ToLower();
            }
        }
        
        public string CSS
        {
            get
            {
                return _css;
            }
            set
            {
                _css = value;
                UserConfigurationMap["css"] = _css;
            }
        }

        public string JS
        {
            get
            {
                return _js;
            }
            set
            {
                _js = value;
                UserConfigurationMap["js"] = _js;
            }
        }

        public Boolean EnableTimeline
        {
            get
            {
                return _enableTimeline;
            }
            set
            {
                _enableTimeline = value;
                UserConfigurationMap["enableTimeline"] = value.ToString();
            }
        }

        private Theme _theme;
        private string _css;
        private string _js;
        private bool _enableTimeline;
    }
}
