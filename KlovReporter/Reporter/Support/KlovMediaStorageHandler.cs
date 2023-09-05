using AventStack.ExtentReports.Model;
using MongoDB.Bson;
using System;

namespace AventStack.ExtentReports.Reporter.Support
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

        public void SaveScreenCapture(ScreenCapture media)
        {
            var doc = new BsonDocument
            {
                { "project", _klovmedia.ProjectId },
                { "report", _klovmedia.ReportId },
                { "test", new ObjectId(media.Info["TestObjectId"].ToString()) }
            };

            if (media.Info.ContainsKey("LogObjectId"))
            {
                doc.Add("log", new ObjectId(media.Info["LogObjectId"].ToString()));
            }
            else
            {
                doc.Add("testName", media.Info["TestName"].ToString());
            }

            if (media.IsBase64)
            {
                doc.Add("base64String", media.Base64);
            }

            _klovmedia.MediaCollection.InsertOne(doc);
            var id = doc["_id"].AsObjectId;
            media.Info["ObjectId"] = id;
            media.Info["ReportObjectId"] = _klovmedia.ReportId;

            if (media.IsBase64)
            {
                return;
            }

            _mediamanager.StoreMedia(media);
        }

        private KlovHttpMediaManager _mediamanager;
        private KlovMedia _klovmedia;
    }
}
