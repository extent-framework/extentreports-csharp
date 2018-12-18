using AventStack.ExtentReports.Utils;

using System;
using System.Linq;

namespace AventStack.ExtentReports.Gherkin
{
    internal class GherkinDialect
    {
        public GherkinKeywords Keywords
        {
            get; private set;
        }

        public string Language
        {
            get; private set;
        }

        public GherkinDialect(string language, GherkinKeywords keywords)
        {
            Language = language;
            Keywords = keywords;
        }

        public string Match(string keyword)
        {
            if (Keywords.And.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                return "And";

            if (Keywords.Background.Contains(keyword, StringComparer.OrdinalIgnoreCase))
                return "Background";

            if (Keywords.But.Contains(keyword, StringComparer.OrdinalIgnoreCase))
                return "But";

            if (Keywords.Examples.Contains(keyword, StringComparer.OrdinalIgnoreCase))
                return "Examples";

            if (Keywords.Feature.Contains(keyword, StringComparer.OrdinalIgnoreCase))
                return "Feature";

            if (Keywords.Given.Contains(keyword, StringComparer.OrdinalIgnoreCase))
                return "Given";

            if (Keywords.Scenario.Contains(keyword, StringComparer.OrdinalIgnoreCase))
                return "Scenario";

            if (Keywords.ScenarioOutline.Contains(keyword, StringComparer.OrdinalIgnoreCase))
                return "ScenarioOutline";

            if (Keywords.Then.Contains(keyword, StringComparer.OrdinalIgnoreCase))
                return "Then";

            if (Keywords.When.Contains(keyword, StringComparer.OrdinalIgnoreCase))
                return "When";

            return null;
        }
    }
}
