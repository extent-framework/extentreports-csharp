using AventStack.ExtentReports.Views;

using RazorEngine.Templating;

using System.IO;

namespace AventStack.ExtentReports.Reporter.TemplateEngine
{
    internal static class TemplateLoadService
    {
        public static void LoadTemplate<T>(string template) where T : IViewsMarker
        {
            LoadTemplate<T>(new[] { template });
        }

        public static void LoadTemplate<T>(string[] templates) where T : IViewsMarker
        {
            foreach (string template in templates)
            {
                string resourceName = typeof(T).Namespace + "." + template + ".cshtml";
                using (var resourceStream = typeof(T).Assembly.GetManifestResourceStream(resourceName))
                {
                    using (var reader = new StreamReader(resourceStream))

                    {
                        if (resourceStream != null)
                        {
                            var arr = template.Split('.');
                            var name = arr.Length > 1 ? arr[arr.Length - 1] : arr[0];
                            RazorEngineManager.Instance.Razor.AddTemplate(name, reader.ReadToEnd());
                        }
                    }
                }
            }
        }
    }
}
