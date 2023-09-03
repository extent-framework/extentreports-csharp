using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using RazorEngine.Text;
using System;

namespace AventStack.ExtentReports.Reporter.Templating
{
    internal sealed class RazorEngineManager
    {
        private bool _initialized = false;

        public IRazorEngineService Razor
        {
            get
            {
                return Engine.Razor;
            }
        }

        internal void InitializeRazor()
        {
            if (_initialized)
                return;

            var templateConfig = new TemplateServiceConfiguration
            {
                DisableTempFileLocking = true,
                EncodedStringFactory = new RawStringFactory(),
                CachingProvider = new DefaultCachingProvider(x => { })
            };

            var service = RazorEngineService.Create(templateConfig);
            Engine.Razor = service;

            _initialized = true;
        }

        private static readonly Lazy<RazorEngineManager> lazy = new Lazy<RazorEngineManager>(() => new RazorEngineManager());

        public static RazorEngineManager Instance { get { return lazy.Value; } }

        private RazorEngineManager()
        {
            InitializeRazor();
        }
    }
}
