using System.Collections.Generic;
using System.Threading;

namespace AventStack.ExtentReports.Model
{
    public class Media : IBaseEntity
    {
        private static int _cntr;

        public int Id = Interlocked.Increment(ref _cntr);
        public string Path { get; set; }
        public string Title { get; set; }
        public string ResolvedPath { get; set; }
        public IDictionary<string, object> Info = new Dictionary<string, object>();

        public Media(string path = null, string title = null)
        {
            Path = path;
            Title = title;
        }
    }
}
