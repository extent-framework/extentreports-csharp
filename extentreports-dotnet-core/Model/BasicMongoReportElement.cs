using MongoDB.Bson;

namespace AventStack.ExtentReports.Model
{
    interface BasicMongoReportElement
    {
        ObjectId ObjectId { get; set; }
    }
}
