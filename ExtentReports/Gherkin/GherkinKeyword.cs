using AventStack.ExtentReports.Core;
using AventStack.ExtentReports.Gherkin.Model;
using System.Linq;

namespace AventStack.ExtentReports.Gherkin
{
    public class GherkinKeyword
    {
        public string Name { get; private set; }

        public GherkinKeyword(string name)
        {
            if (name == "ScenarioOutline")
            {
                name = "Scenario Outline";
            }
            if (name == "Asterisk")
            {
                name = "*";
            }

            Name = name;
            CreateDomain();
        }

        private void CreateDomain()
        {
            if (Name == null)
            {
                throw new GherkinKeywordNotFoundException("Keyword " + Name + " cannot be null");
            }

            string key = Name.First().ToString().ToUpper() + Name.Substring(1);

            if (key != "*")
            {
                Name = GherkinDialectProvider.Dialect.Match(key);
            }
        }
    }
}
