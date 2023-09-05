using Newtonsoft.Json;

namespace AventStack.ExtentReports.Model.ExtensionMethods
{
    public static class ClonedExtensions
    {
        public static T DeepClone<T>(this T obj)
        {
            // Don't serialize a null object, simply return the default for that object
            if (obj == null)
            {
                return default;
            }

            // initialize inner objects individually
            // for example in default constructor some list property initialized with some values,
            // but in 'source' these items are cleaned -
            // without ObjectCreationHandling.Replace default constructor values will be added to result
            var deserializeSettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj), deserializeSettings);
        }
    }
}
