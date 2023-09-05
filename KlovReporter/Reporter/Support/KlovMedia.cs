using MongoDB.Bson;
using MongoDB.Driver;

namespace AventStack.ExtentReports.Reporter.Support
{
    internal class KlovMedia
    {
        public ObjectId ProjectId { get; set; }
        public ObjectId ReportId { get; set; }
        public IMongoCollection<BsonDocument> MediaCollection { get; set; }
    }
}
