using AventStack.ExtentReports.Gherkin.Resource;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace AventStack.ExtentReports.Gherkin
{
    public static class GherkinDialectProvider
    {
        private const string LangFileName = "lang.json";

        private static string _lang = "en";
        private static GherkinKeywords _keywords;
        private static GherkinDialect _dialect;

        public static Dictionary<string, GherkinKeywords> Dialects { get; private set; }

        public static GherkinDialect Dialect
        {
            get
            {
                if (_dialect == null)
                {
                    LoadDialects();
                }

                return _dialect;
            }
            set
            {
                _dialect = value;
            }
        }

        public static string Lang
        {
            get
            {
                return _lang;
            }
            set
            {
                _lang = value;
                SetDialect(value);
            }
        }

        private static void LoadDialects()
        {
            var gherkinResource = typeof(IGherkinResource).Assembly;
            var ns = typeof(IGherkinResource).Namespace;
            var lang = ns + "." + LangFileName;
            string json = null;

            using (Stream resourceStream = gherkinResource.GetManifestResourceStream(lang))
            {
                using (StreamReader reader = new StreamReader(resourceStream))
                {
                    if (null != resourceStream)
                    {
                        json = reader.ReadToEnd();
                    }
                }
            }

            Dialects = JsonConvert.DeserializeObject<Dictionary<string, GherkinKeywords>>(json);
            SetDialect(_lang);
        }

        private static void SetDialect(string lang)
        {
            if (Dialects == null)
            {
                LoadDialects();
            }

            if (!Dialects.ContainsKey(lang))
            {
                throw new InvalidGherkinLanguageException(lang + " is not a valid Gherkin dialect");
            }

            _keywords = Dialects[lang];
            Dialect = new GherkinDialect(lang, _keywords);
        }
    }
}
