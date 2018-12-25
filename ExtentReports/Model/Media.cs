using System;
using System.IO;
using System.Threading;

using MongoDB.Bson;

namespace AventStack.ExtentReports.Model
{
    [Serializable]
    public class Media
    {
        public ObjectId ObjectId { get; set; }
        public ObjectId ReportObjectId { get; set; }
        public ObjectId TestObjectId { get; set; }
        public ObjectId LogObjectId { get; set; }
        public long FileSize { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string Base64String { get; set; }        
        public int Sequence { get; private set; } = Interlocked.Increment(ref _seq);

        public string Path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;

                Name = !string.IsNullOrEmpty(Name) ? Name : System.IO.Path.GetFileName(value);
                if (File.Exists(value))
                {
                    FileSize = new FileInfo(value).Length;
                }
            }
        }

        private static int _seq;
        private string _path;
    }
}
