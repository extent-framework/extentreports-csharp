using Newtonsoft.Json;

using System.Collections.Generic;

namespace AventStack.ExtentReports.Gherkin
{
    internal static class GherkinDialectProvider
    {
        private static Dictionary<string, GherkinKeywords> _dialects;
        private static GherkinKeywords _keywords;
        private static string _dialect;

        static GherkinDialectProvider()
        {
            var json = Resources.GherkinLangs.Languages;
            _dialects = JsonConvert.DeserializeObject<Dictionary<string, GherkinKeywords>>(json);
        }

        public static string DefaultDialect { get; } = "en";

        public static GherkinDialect Dialect { get; private set; }

        public static string Language
        {
            get
            {
                if (string.IsNullOrEmpty(_dialect))
                    _dialect = DefaultDialect;
                return _dialect;
            }
            set
            {
                _dialect = value;

                if (!_dialects.ContainsKey(_dialect))
                    throw new InvalidGherkinLanguageException(_dialect + " is not a valid Gherkin dialect");

                _keywords = _dialects[_dialect];
                Dialect = new GherkinDialect(_dialect, _keywords);
            }
        }
    }
}
