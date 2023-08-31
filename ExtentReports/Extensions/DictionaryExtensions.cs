using AventStack.ExtentReports.Util;
using System.Collections.Generic;

namespace AventStack.ExtentReports.Extensions
{
    internal static class DictionaryExtensions
    {
        public static void AddRange<T, V>(this IDictionary<T, V> source, IDictionary<T, V> collection)
        {
            Assert.NotNull(source, "Collection must not be null");

            foreach (KeyValuePair<T, V> item in collection)
            {
                source[item.Key] = item.Value;
            }
        }
    }
}
