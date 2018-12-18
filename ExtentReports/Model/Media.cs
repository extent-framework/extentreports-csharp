using System;
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
        public string Name { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public string Title { get; set; }
        public string Base64String { get; set; }        
        public int Sequence { get; private set; } = Interlocked.Increment(ref _seq);

        private static int _seq;
    }
}
