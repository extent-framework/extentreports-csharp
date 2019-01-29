using AventStack.ExtentReports.Model;

using MongoDB.Bson;

using System;

namespace AventStack.ExtentReports.MediaStorage
{
    internal class KlovMediaStorageHandler
    {
        public KlovMediaStorageHandler(string url, KlovMedia media)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("Invalid URL or resource not found");
            }

            _klovmedia = media;
            _mediamanager = new KlovHttpMediaManager();
            _mediamanager.Init(url);
        }

        public void SaveScreenCapture(BasicMongoReportElement el, ScreenCapture media)
        {
            var document = new BsonDocument
            {
                { "project", _klovmedia.ProjectId },
                { "report", _klovmedia.ReportId },
                { "sequence", media.Sequence },
                { "mediaType", "img" },
                { "test", media.TestObjectId }
            };

            if (el is Test)
            {
                document.Add("testName", ((Test)el).Name);
            }
            else
            {
                document.Add("log", el.ObjectId);
            }

            _klovmedia.MediaCollection.InsertOne(document);
            var id = document["_id"].AsObjectId;
            media.ObjectId = id;
            media.ReportObjectId = _klovmedia.ReportId;
            _mediamanager.StoreMedia(media);
        }

        private KlovHttpMediaManager _mediamanager;
        private KlovMedia _klovmedia;
    }
}
