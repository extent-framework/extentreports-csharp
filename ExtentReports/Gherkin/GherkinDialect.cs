using AventStack.ExtentReports.Core;

namespace AventStack.ExtentReports.Gherkin
{
    public class GherkinDialect
    {
        public GherkinKeywords Keywords { get; private set; }
        public string Language { get; private set; }

        public GherkinDialect(string language, GherkinKeywords keywords)
        {
            Language = language;
            Keywords = keywords;
        }

        public string Match(string keyword)
        {
            if (Keywords.And.Contains(keyword))
            {
                return "And";
            }
            else if (Keywords.Background.Contains(keyword))
            {
                return "Background";
            }
            else if (Keywords.But.Contains(keyword))
            {
                return "But";
            }
            else if (Keywords.Examples.Contains(keyword))
            {
                return "Examples";
            }
            else if (Keywords.Feature.Contains(keyword))
            {
                return "Feature";
            }
            else if (Keywords.Given.Contains(keyword))
            {
                return "Given";
            }
            else if (Keywords.Scenario.Contains(keyword))
            {
                return "Scenario";
            }
            else if (Keywords.ScenarioOutline.Contains(keyword))
            {
                return "ScenarioOutline";
            }
            else if (Keywords.Then.Contains(keyword))
            {
                return "Then";
            }
            else if (Keywords.When.Contains(keyword))
            {
                return "When";
            }

            throw new GherkinKeywordNotFoundException("The supplied keyword " + keyword + " is invalid for lang=" + Language);
        }
    }
}
