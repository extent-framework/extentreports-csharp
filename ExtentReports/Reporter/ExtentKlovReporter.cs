using AventStack.ExtentReports.Core;
using AventStack.ExtentReports.MediaStorage;
using AventStack.ExtentReports.Model;

using MongoDB.Bson;
using MongoDB.Driver;

using System;
using System.Collections.Generic;
using System.Linq;

namespace AventStack.ExtentReports.Reporter
{
    public class ExtentKlovReporter : AbstractReporter
    {
        public override string ReporterName => "klov";

        public override AnalysisStrategy AnalysisStrategy { get; set; }

        public override ReportStatusStats ReportStatusStats { get; protected internal set; }

        public string ReportName { get; set; }
        public ObjectId ReportId { get; private set; }
        public string ProjectName { get; set; }
        public ObjectId ProjectId { get; private set; }

        private const string DefaultProjectName = "Default";
        private const string DefaultKlovServerName = "klov";
        private const string DatabaseName = "klov";

        public ExtentKlovReporter()
        {
            _startTime = DateTime.Now;
        }

        public ExtentKlovReporter(string projectName, string reportName) : this()
        {
            ProjectName = string.IsNullOrEmpty(projectName) ? DefaultProjectName : projectName;
            ReportName = string.IsNullOrEmpty(reportName) ? "Build " + DateTime.Now : reportName;
        }

        /// <summary>
        /// Connects to MongoDB default settings, localhost:27017
        /// </summary>
        public void InitMongoDbConnection()
        {
            _mongoClient = new MongoClient();
        }

        public void InitMongoDbConnection(string host, int port = -1)
        {
            var conn = "mongodb://" + host;
            conn += port > -1 ? ":" + port : "";
            _mongoClient = new MongoClient(conn);
        }

        /// <summary>
        /// Connects to MongoDB using a connection string.
        /// Example: mongodb://host:27017,host2:27017/?replicaSet=rs0
        /// </summary>
        /// <param name="connectionString"></param>
        public void InitMongoDbConnection(string connectionString)
        {
            _mongoClient = new MongoClient(connectionString);
        }

        public void InitMongoDbConnection(MongoClientSettings settings)
        {
            _mongoClient = new MongoClient(settings);
        }

        public void InitKlovServerConnection(string url)
        {
            _url = url;
        }

        public override void Start()
        {
            var db = _mongoClient.GetDatabase(DatabaseName);
            InitializeCollections(db);
            SetupProject();
        }

        private void InitializeCollections(IMongoDatabase db)
        {
            _projectCollection = db.GetCollection<BsonDocument>("project");
            _reportCollection = db.GetCollection<BsonDocument>("report");
            _testCollection = db.GetCollection<BsonDocument>("test");
            _logCollection = db.GetCollection<BsonDocument>("log");
            _exceptionCollection = db.GetCollection<BsonDocument>("exception");
            _mediaCollection = db.GetCollection<BsonDocument>("media");
            _categoryCollection = db.GetCollection<BsonDocument>("category");
            _authorCollection = db.GetCollection<BsonDocument>("author");
            _deviceCollection = db.GetCollection<BsonDocument>("device");
            _environmentCollection = db.GetCollection<BsonDocument>("environment");
        }

        private void SetupProject()
        {
            ProjectName = string.IsNullOrEmpty(ProjectName) ? DefaultProjectName : ProjectName;
            var document = new BsonDocument
            {
                { "name", ProjectName }
            };

            var bsonProject = _projectCollection.Find(document).FirstOrDefault();
            if (bsonProject != null)
            {
                ProjectId = bsonProject["_id"].AsObjectId;
            }
            else
            {
                document.Add("createdAt", DateTime.Now);
                _projectCollection.InsertOne(document);
                ProjectId = document["_id"].AsObjectId;
            }

            SetupReport();
        }

        private void SetupReport()
        {
            ReportName = string.IsNullOrEmpty(ReportName) ? "Build " + DateTime.Now : ReportName;

            var document = new BsonDocument
            {
                { "name", ReportName },
                { "project", ProjectId },
                { "projectName", ProjectName },
                { "startTime", DateTime.Now }
            };
            
            _reportCollection.InsertOne(document);
            ReportId = document["_id"].AsObjectId;
        }

        public override void Stop() { }

        public override void Flush(ReportAggregates reportAggregates)
        {
            if (reportAggregates.TestList == null || reportAggregates.TestList.Count == 0)
            {
                return;
            }
            this._reportAggregates = reportAggregates;
            
            var filter = Builders<BsonDocument>.Filter.Eq("_id", ReportId);
            var update = Builders<BsonDocument>.Update
                .Set("endTime", DateTime.Now)
                .Set("duration", (DateTime.Now - _startTime).Milliseconds)
                .Set("status", StatusHierarchy.GetHighestStatus(reportAggregates.StatusList).ToString().ToLower())
                .Set("parentLength", reportAggregates.ReportStatusStats.ParentCount)
                .Set("passParentLength", reportAggregates.ReportStatusStats.ParentCountPass)
                .Set("failParentLength", reportAggregates.ReportStatusStats.ParentCountFail)
                .Set("fatalParentLength", reportAggregates.ReportStatusStats.ParentCountFatal)
                .Set("errorParentLength", reportAggregates.ReportStatusStats.ParentCountError)
                .Set("warningParentLength", reportAggregates.ReportStatusStats.ParentCountWarning)
                .Set("skipParentLength", reportAggregates.ReportStatusStats.ParentCountSkip)
                .Set("exceptionsParentLength", reportAggregates.ReportStatusStats.ParentCountExceptions)
                .Set("childLength", reportAggregates.ReportStatusStats.ChildCount)
                .Set("passChildLength", reportAggregates.ReportStatusStats.ChildCountPass)
                .Set("failChildLength", reportAggregates.ReportStatusStats.ChildCountFail)
                .Set("fatalChildLength", reportAggregates.ReportStatusStats.ChildCountFatal)
                .Set("errorChildLength", reportAggregates.ReportStatusStats.ChildCountError)
                .Set("warningChildLength", reportAggregates.ReportStatusStats.ChildCountWarning)
                .Set("skipChildLength", reportAggregates.ReportStatusStats.ChildCountSkip)
                .Set("infoChildLength", reportAggregates.ReportStatusStats.ChildCountInfo)
                .Set("exceptionsChildLength", reportAggregates.ReportStatusStats.ChildCountExceptions)
                .Set("grandChildLength", reportAggregates.ReportStatusStats.GrandChildCount)
                .Set("passGrandChildLength", reportAggregates.ReportStatusStats.GrandChildCountPass)
                .Set("failGrandChildLength", reportAggregates.ReportStatusStats.GrandChildCountFail)
                .Set("fatalGrandChildLength", reportAggregates.ReportStatusStats.GrandChildCountFatal)
                .Set("errorGrandChildLength", reportAggregates.ReportStatusStats.GrandChildCountError)
                .Set("warningGrandChildLength", reportAggregates.ReportStatusStats.GrandChildCountWarning)
                .Set("skipGrandChildLength", reportAggregates.ReportStatusStats.GrandChildCountSkip)
                .Set("exceptionsGrandChildLength", reportAggregates.ReportStatusStats.GrandChildCountExceptions)
                .Set("analysisStrategy", AnalysisStrategy.ToString().ToUpper());

            _reportCollection.UpdateOne(filter, update);
        }

        public override void OnAuthorAssigned(Test test, Author author)
        {
        }

        public override void OnCategoryAssigned(Test test, Category category)
        {
        }

        public override void OnDeviceAssigned(Test test, Device device)
        {
        }

        public override void OnLogAdded(Test test, Log log)
        {
            var document = new BsonDocument
            {
                { "test", test.ObjectId },
                { "project", ProjectId },
                { "report", ReportId },
                { "testName", test.Name },
                { "sequence", log.Sequence },
                { "status", log.Status.ToString().ToLower() },
                { "timestamp", log.Timestamp },
                { "details", log.Details }
            };

            if (log.HasScreenCapture && log.ScreenCaptureContext.FirstOrDefault().IsBase64)
            {
                document["details"] = log.Details + log.ScreenCaptureContext.FirstOrDefault().Source;
            }

            _logCollection.InsertOne(document);

            var id = document["_id"].AsObjectId;
            log.ObjectId = id;

            if (test.HasException)
            {
                if (_exceptionNameObjectIdCollection == null)
                    _exceptionNameObjectIdCollection = new Dictionary<string, ObjectId>();

                var ex = test.ExceptionInfoContext.FirstOrDefault();

                document = new BsonDocument
                {
                    { "report", ReportId },
                    { "project", ProjectId },
                    { "name", ex.Name }
                };

                var findResult = _exceptionCollection.Find(document).FirstOrDefault();

                if (!_exceptionNameObjectIdCollection.ContainsKey(ex.Name))
                {
                    if (findResult != null)
                    {
                        _exceptionNameObjectIdCollection.Add(ex.Name, findResult["_id"].AsObjectId);
                    }
                    else
                    {
                        document = new BsonDocument
                        {
                            { "project", ProjectId },
                            { "report", ReportId },
                            { "name", ex.Name },
                            { "stacktrace", ((ExceptionInfo)ex).Exception.StackTrace },
                            { "testCount", 0 }
                        };

                        _exceptionCollection.InsertOne(document);

                        var exId = document["_id"].AsObjectId;

                        document = new BsonDocument
                        {
                            { "_id", exId }
                        };
                        findResult = _exceptionCollection.Find(document).FirstOrDefault();

                        _exceptionNameObjectIdCollection.Add(ex.Name, exId);
                    }
                }

                var testCount = ((int)(findResult["testCount"])) + 1;
                var filter = Builders<BsonDocument>.Filter.Eq("_id", findResult["_id"].AsObjectId);
                var update = Builders<BsonDocument>.Update.Set("testCount", testCount);
                _exceptionCollection.UpdateOne(filter, update);
                
                filter = Builders<BsonDocument>.Filter.Eq("_id", test.ObjectId);
                update = Builders<BsonDocument>.Update.Set("exception", _exceptionNameObjectIdCollection[ex.Name]);
                _testCollection.UpdateOne(filter, update);
            }

            EndTestRecursive(test);
        }

        private void EndTestRecursive(Test test)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", test.ObjectId);
            var update = Builders<BsonDocument>.Update
                .Set("status", test.Status.ToString().ToLower())
                .Set("endTime", test.EndTime)
                .Set("duration", test.RunDuration.Milliseconds)
                .Set("leaf", test.HasChildren)
                .Set("childNodesLength", test.NodeContext.Count)
                .Set("categorized", test.HasCategory)
                .Set("description", test.Description);
            
            _testCollection.FindOneAndUpdate(filter, update);

            if (test.Level > 0)
            {
                EndTestRecursive(test.Parent);
            }
        }

        public override void OnScreenCaptureAdded(Test test, ScreenCapture screenCapture)
        {
            SaveScreenCapture(test, screenCapture);
        }

        public override void OnScreenCaptureAdded(Log log, ScreenCapture screenCapture)
        {
            SaveScreenCapture(log, screenCapture);
        }

        private void SaveScreenCapture(BasicMongoReportElement el, ScreenCapture screenCapture)
        {
            if (_mediaStorageHandler == null)
            {
                KlovMedia klovMedia = new KlovMedia()
                {
                    ProjectId = ProjectId,
                    ReportId = ReportId,
                    MediaCollection = _mediaCollection
                };
                _mediaStorageHandler = new KlovMediaStorageHandler(_url, klovMedia);
            }

            _mediaStorageHandler.SaveScreenCapture(el, screenCapture);
        }

        public override void OnTestRemoved(Test test)
        {
        }

        public override void OnTestStarted(Test test)
        {
            OnTestStartedHelper(test);
        }

        public override void OnNodeStarted(Test node)
        {
            OnTestStartedHelper(node);
        }

        private void OnTestStartedHelper(Test test)
        {
            var document = new BsonDocument
            {
                { "project", ProjectId },
                { "report", ReportId },
                { "reportName", ReportName },
                { "level", test.Level },
                { "name", test.Name },
                { "status", test.Status.ToString().ToLower() },
                { "description", test.Description },
                { "startTime", test.StartTime },
                { "endTime", test.EndTime },
                { "bdd", test.IsBehaviorDrivenType },
                { "leaf", test.HasChildren },
                { "childNodesLength", test.NodeContext.Count }
            };

            if (test.IsBehaviorDrivenType)
            {
                document.Add("bddType", test.BehaviorDrivenType.GetType().Name);
            }

            if (test.Parent != null)
            {
                document.Add("parent", test.Parent.ObjectId);
                document.Add("parentName", test.Parent.Name);
                UpdateTestChildrenCount(test.Parent);
                UpdateTestDescription(test.Parent);
            }

            _testCollection.InsertOne(document);
            test.ObjectId = document["_id"].AsObjectId;
        }

        private void UpdateTestChildrenCount(Test test)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", test.ObjectId);
            var update = Builders<BsonDocument>.Update
                .Set("childNodesLength", test.NodeContext.Count);
            _testCollection.FindOneAndUpdate(filter, update);
        }

        private void UpdateTestDescription(Test test)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", test.ObjectId);
            var update = Builders<BsonDocument>.Update
                .Set("description", test.Description);
            _testCollection.FindOneAndUpdate(filter, update);
        }

        private string _url;

        private KlovMediaStorageHandler _mediaStorageHandler;

        private ReportAggregates _reportAggregates;

        private DateTime _startTime;
        private MongoClient _mongoClient;
        private IMongoCollection<BsonDocument> _projectCollection;
        private IMongoCollection<BsonDocument> _reportCollection;
        private IMongoCollection<BsonDocument> _testCollection;
        private IMongoCollection<BsonDocument> _logCollection;
        private IMongoCollection<BsonDocument> _exceptionCollection;
        private IMongoCollection<BsonDocument> _mediaCollection;
        private IMongoCollection<BsonDocument> _categoryCollection;
        private IMongoCollection<BsonDocument> _authorCollection;
        private IMongoCollection<BsonDocument> _deviceCollection;
        private IMongoCollection<BsonDocument> _environmentCollection;

        private Dictionary<string, ObjectId> _exceptionNameObjectIdCollection;
    }
}
