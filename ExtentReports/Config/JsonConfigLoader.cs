using Newtonsoft.Json;
using System.IO;

namespace AventStack.ExtentReports.Config
{
    public class JsonConfigLoader
    {
        public void LoadJSONConfig<T>(ref T config, string filePath)
        {
            var json = File.ReadAllText(filePath);
            var serializer = new JsonSerializer();
            config = serializer.Deserialize<T>(new JsonTextReader(new StringReader(json)));
        }
    }
}
