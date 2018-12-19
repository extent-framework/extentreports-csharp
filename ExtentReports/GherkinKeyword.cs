using AventStack.ExtentReports.Core;
using AventStack.ExtentReports.Gherkin;
using AventStack.ExtentReports.Gherkin.Model;

using System;
using System.Linq;
using System.Reflection;

namespace AventStack.ExtentReports
{
    public class GherkinKeyword
    {
        public IGherkinFormatterModel Model { get; private set; }

        public GherkinKeyword(string keyword)
        {
            var type = typeof(IGherkinFormatterModel);
            var language = GherkinDialectProvider.Language;
            var dialect = GherkinDialectProvider.Dialect;
            string apiKeyword = keyword.First().ToString().ToUpper() + keyword.Substring(1);
            apiKeyword = apiKeyword.Equals("*") ? Asterisk.GherkinName : apiKeyword;

            try
            {
                if (!language.ToLower().Equals(GherkinDialectProvider.DefaultDialect))
                {
                    apiKeyword = null;
                    apiKeyword = dialect.Match(keyword);
                }

                if (apiKeyword == null)
                {
                    throw new GherkinKeywordNotFoundException("Keyword " + apiKeyword + " cannot be null");
                }

                var gherkinType = Assembly.GetExecutingAssembly().GetTypes()
                    .Where(p => p.Name.Equals(apiKeyword, StringComparison.CurrentCultureIgnoreCase))
                    .First();
                
                var obj = Activator.CreateInstance(gherkinType);
                Model = (IGherkinFormatterModel)obj;
            }
            catch (InvalidOperationException e)
            {
                throw new InvalidOperationException("Invalid keyword specified: " + keyword, e);
            }
        }
    }
}
