using AventStack.ExtentReports.Listener.Entity;
using AventStack.ExtentReports.Model;
using AventStack.ExtentReports.Model.Context.Manager;
using AventStack.ExtentReports.Reporter.Support;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AventStack.ExtentReports.Reporter
{
    public class ExtentKlovReporter : IObserver<ReportEntity>
    {
        private const string DefaultProjectName = "Default";
        private const string DatabaseName = "klov";

        public string ReportName { get; set; }
        public ObjectId ReportId { get; private set; }
        public string ProjectName { get; set; }
        public ObjectId ProjectId { get; private set; }

        private string _url;
        private bool _initialized = false;

        private NamedAttributeContextManager<Author> _authorContext;
        private NamedAttributeContextManager<Category> _categoryContext;
        private NamedAttributeContextManager<Device> _deviceContext;
        private Dictionary<string, ObjectId> _categoryNameObjectIdCollection = new Dictionary<string, ObjectId>();
        private Dictionary<string, ObjectId> _authorNameObjectIdCollection = new Dictionary<string, ObjectId>();
        private Dictionary<string, ObjectId> _deviceNameObjectIdCollection = new Dictionary<string, ObjectId>();
        private Dictionary<string, ObjectId> _exceptionNameObjectIdCollection = new Dictionary<string, ObjectId>();

        private KlovMediaStorageHandler _mediaStorageHandler;

        private long _reportSeq;
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

        public ExtentKlovReporter(string projectName, string reportName)
        {
            ProjectName = string.IsNullOrEmpty(projectName) ? DefaultProjectName : projectName;
            ReportName = string.IsNullOrEmpty(reportName) ? "Build " + DateTime.Now : reportName;
        }

        public ExtentKlovReporter(string projectName) : this(projectName, null) { }

        /// <summary>
        /// Initializes KlovReporter with default Klov and MongoDB settings. This default
        /// the Klov server and MongoDB to LOCALHOST and also uses default ports 80 and 27017
        /// respectively.
        /// </summary>
        /// <returns>ExtentKlovReporter</returns>
        public ExtentKlovReporter InitWithDefaultSettings()
        {
            InitMongoDbConnection();
            InitKlovServerConnection();
            return this;
        }

        public void InitKlovServerConnection(string url = "http://localhost")
        {
            _url = url;
        }

        /// <summary>
        /// Connects to MongoDB default settings, localhost:27017
        /// </summary>
        public void InitMongoDbConnection()
        {
            _mongoClient = new MongoClient();
        }

        public void InitMongoDbConnection(string host = "localhost", int port = 27017)
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

        /// <summary>
        /// Connects to MongoDB using MongoClientSettings
        /// </summary>
        /// <param name="settings">The settings for MongoDB client</param>
        public void InitMongoDbConnection(MongoClientSettings settings)
        {
            _mongoClient = new MongoClient(settings);
        }

        /// <summary>
        /// Connects to MongoDB using a MongoUrl
        /// </summary>
        /// <param name="url">Represents an immutable URL style connection string</param>
        public void InitMongoDbConnection(MongoUrl url)
        {
            _mongoClient = new MongoClient(url);
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(ReportEntity value)
        {
            var report = value.Report;
            var stats = report.Stats;

            if (report.TestQueue.Count == 0)
            {
                return;
            }

            Init();
            UpdateReport(report, stats);
        }

        private void Init()
        {
            if (!_initialized)
            {
                _initialized = true;
                var db = _mongoClient.GetDatabase(DatabaseName);
                InitializeCollections(db);
                SetupProject();
            }
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

            CreateReport();
        }

        private void CreateReport()
        {
            ReportName = string.IsNullOrEmpty(ReportName) ? "Build " + DateTime.Now : ReportName;
            _reportSeq = _reportCollection.CountDocuments(new BsonDocument { { "project", ProjectId } }) + 1;

            var document = new BsonDocument
            {
                { "name", ReportName },
                { "project", ProjectId },
                { "projectName", ProjectName },
                { "startTime", DateTime.Now },
                { "seq", _reportSeq }
            };

            _reportCollection.InsertOne(document);
            ReportId = document["_id"].AsObjectId;
        }

        private void UpdateReport(Report report, ReportStats stats)
        {
            var doc = Builders<BsonDocument>.Update
                .Set("endTime", report.EndTime)
                .Set("duration", report.TimeTaken.TotalMilliseconds)
                .Set("status", report.Status.ToString().ToLower())
                .Set("parentLength", stats.SumStat(stats.Parent))
                .Set("passParentLength", stats.Parent[Status.Pass])
                .Set("failParentLength", stats.Parent[Status.Fail])
                .Set("warningParentLength", stats.Parent[Status.Warning])
                .Set("skipParentLength", stats.Parent[Status.Skip])
                .Set("childLength", stats.SumStat(stats.Child))
                .Set("passChildLength", stats.Child[Status.Pass])
                .Set("failChildLength", stats.Child[Status.Fail])
                .Set("warningChildLength", stats.Child[Status.Warning])
                .Set("skipChildLength", stats.Child[Status.Skip])
                .Set("grandChildLength", stats.SumStat(stats.Grandchild))
                .Set("passGrandChildLength", stats.Grandchild[Status.Pass])
                .Set("failGrandChildLength", stats.Grandchild[Status.Fail])
                .Set("warningGrandChildLength", stats.Grandchild[Status.Warning])
                .Set("skipGrandChildLength", stats.Grandchild[Status.Skip])
                .Set("analysisStrategy", stats.AnalysisStrategy.ToString().ToUpper())
                .Set("bdd", report.IsBDD);

            if (report.HasAuthors)
            {
                var x = report.AuthorCtx.Context.Keys;
                doc = doc.Set("authorNameList", x);
            }
            if (report.HasCategories)
            {
                var x = report.CategoryCtx.Context.Keys;
                doc = doc.Set("categoryNameList", x);
            }
            if (report.HasDevices)
            {
                var x = report.DeviceCtx.Context.Keys;
                doc = doc.Set("deviceNameList", x);
            }
            if (report.HasExceptions)
            {
                var x = report.ExceptionInfoCtx.Context.Keys;
                doc = doc.Set("exceptions", x);
            }

            _reportCollection.UpdateOne(
                filter: new BsonDocument("_id", ReportId),
                update: doc
            );

            UpdateSystemEnvInfo(report.SystemEnvInfo);
            UpdateTests(report.TestQueue.ToList());
        }

        private void UpdateSystemEnvInfo(List<SystemEnvInfo> env)
        {
            foreach (var attr in env)
            {
                var document = new BsonDocument
                {
                    { "project", ProjectId },
                    { "report", ReportId },
                    { "name", attr.Name }
                };

                var findResult = _environmentCollection.Find(document);

                if (findResult == null && !findResult.Any())
                {
                    document.Add("value", attr.Value);
                }
                else
                {
                    var id = findResult.First()["_id"].AsObjectId;
                    document = new BsonDocument
                    {
                        { "_id", id },
                        { "value", attr.Value }
                    };

                    var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
                    _environmentCollection.UpdateOne(filter, document);
                }
            }
        }

        public void UpdateTests(List<Test> tests)
        {
            foreach (var test in tests)
            {
                var doc = new BsonDocument
                {
                    { "project", ProjectId },
                    { "report", ReportId },
                    { "reportSeq", _reportSeq },
                    { "reportName", ReportName }
                };

                doc["level"] = test.Level;
                doc["name"] = test.Name;
                doc["status"] = test.Status.ToString().ToLower();
                doc["duration"] = test.TimeTaken;
                doc["description"] = test.Description ?? "";
                doc["startTime"] = test.StartTime;
                doc["endTime"] = test.EndTime;
                doc["bdd"] = test.IsBdd;
                doc["leaf"] = test.Leaf;
                doc["childNodesLength"] = test.Children.Count;
                doc["mediaCount"] = test.Media.Count;
                doc["logCount"] = test.Logs.Count + test.GeneratedLog.Count;
                doc["categorized"] = test.HasAttributes;

                if (test.IsBdd)
                {
                    doc["bddType"] = test.BddType.Name;
                }
                if (test.HasAuthor)
                {
                    doc = doc.Set("authorNameList", new BsonArray(test.Author.Select(x => x.Name).ToArray()));
                }
                if (test.HasCategory)
                {
                    doc = doc.Set("categoryNameList", new BsonArray(test.Category.Select(x => x.Name).ToArray()));
                }
                if (test.HasDevice)
                {
                    doc = doc.Set("deviceNameList", new BsonArray(test.Device.Select(x => x.Name).ToArray()));
                }

                if (test.Parent != null)
                {
                    doc["parent"] = new ObjectId(test.Parent.Info["ObjectId"].ToString());
                    doc["parentName"] = test.Parent.Name;
                }

                if (test.Info.ContainsKey("ObjectId"))
                {
                    _testCollection.UpdateOne(
                        filter: new BsonDocument("_id", new ObjectId(test.Info["ObjectId"].ToString())),
                        update: doc
                    );
                }
                else
                {
                    _testCollection.InsertOne(doc);
                    test.Info["ObjectId"] = doc["_id"].AsObjectId;
                }

                UpdateLogs(test);
                UpdateExceptions(test.ExceptionInfo, test);
                UpdateAttributes(test, test.Author, _authorNameObjectIdCollection, _authorCollection, _authorContext);
                UpdateAttributes(test, test.Category, _categoryNameObjectIdCollection, _categoryCollection, _categoryContext);
                UpdateAttributes(test, test.Device, _deviceNameObjectIdCollection, _deviceCollection, _deviceContext);
                UploadMedia(test);

                if (test.HasChildren)
                {
                    UpdateTests(test.Children.ToList());
                }
            }
        }

        private void UpdateLogs(Test test)
        {
            var logs = test.Logs.ToList();
            logs.AddRange(test.GeneratedLog.ToList());

            foreach (var log in logs)
            {
                var doc = new BsonDocument
                {
                    { "project", ProjectId },
                    { "report", ReportId },
                    { "test", new ObjectId(test.Info["ObjectId"].ToString()) },
                    { "testName", test.Name },
                    { "sequence", log.Seq },
                    { "status", log.Status.ToString().ToLower() },
                    { "timestamp", log.Timestamp },
                    { "mediaCount", log.HasMedia ? 1 : 0 },
                    { "details", log.Details },
                };

                if (log.HasException)
                {
                    doc["exception"] = log.ExceptionInfo.Name;
                    doc["stacktrace"] = GetException(log.ExceptionInfo.Exception);

                    var updateTest = Builders<BsonDocument>.Update
                        .Set("exceptionName", log.ExceptionInfo.Name);
                    _testCollection.UpdateOne(
                        filter: new BsonDocument("_id", new ObjectId(test.Info["ObjectId"].ToString())),
                        update: updateTest
                    );
                }

                if (log.HasMedia && !string.IsNullOrEmpty(log.Media.Base64))
                {
                    doc["details"] = log.Details + log.Media.Base64;
                }

                if (log.Info.ContainsKey("ObjectId"))
                {
                    _logCollection.UpdateOne(
                        filter: new BsonDocument("_id", new ObjectId(log.Info["ObjectId"].ToString())),
                        update: doc
                    );
                }
                else
                {
                    _logCollection.InsertOne(doc);
                    log.Info["ObjectId"] = doc["_id"].AsObjectId;
                }
            }
        }

        private string GetException(Exception exception)
        {
            var ex = exception.Message ?? "";
            ex += exception.StackTrace ?? "";
            return ex;
        }

        private void UpdateExceptions(List<ExceptionInfo> exceptionInfo, Test test)
        {
            foreach (var ex in exceptionInfo)
            {
                var doc = new BsonDocument
                {
                    { "project", ProjectId },
                    { "report", ReportId }
                };

                doc["name"] = ex.Name;

                var findResult = _exceptionCollection.Find(doc).FirstOrDefault();

                if (!_exceptionNameObjectIdCollection.ContainsKey(ex.Name))
                {
                    if (findResult != null)
                    {
                        _exceptionNameObjectIdCollection.Add(ex.Name, findResult["_id"].AsObjectId);
                    }
                    else
                    {
                        doc = new BsonDocument
                        {
                            { "project", ProjectId },
                            { "report", ReportId },
                            { "name", ex.Name },
                            { "stacktrace", GetException(ex.Exception) },
                            { "testCount", 0 }
                        };

                        _exceptionCollection.InsertOne(doc);

                        var exId = doc["_id"].AsObjectId;

                        doc = new BsonDocument
                        {
                            { "_id", exId }
                        };
                        findResult = _exceptionCollection.Find(doc).FirstOrDefault();

                        _exceptionNameObjectIdCollection.Add(ex.Name, exId);
                    }
                }

                var testCount = ((int)findResult["testCount"]) + 1;
                var filter = Builders<BsonDocument>.Filter.Eq("_id", findResult["_id"].AsObjectId);
                var update = Builders<BsonDocument>.Update.Set("testCount", testCount);
                _exceptionCollection.UpdateOne(filter, update);

                filter = Builders<BsonDocument>.Filter.Eq("_id", test.Info["ObjectId"]);
                update = Builders<BsonDocument>.Update.Set("exception", _exceptionNameObjectIdCollection[ex.Name]);
                _testCollection.UpdateOne(filter, update);
            }
        }

        private void UpdateAttributes<T>(Test test, ISet<T> set, Dictionary<string, ObjectId> nameObjectIdCollection,
            IMongoCollection<BsonDocument> mongoCollection, NamedAttributeContextManager<T> attributeContext) where T : NamedAttribute
        {
            foreach (var attribute in set)
            {
                BsonDocument doc;

                if (!nameObjectIdCollection.ContainsKey(attribute.Name))
                {
                    doc = new BsonDocument
                    {
                        { "report", ReportId },
                        { "project", ProjectId },
                        { "name", attribute.Name }
                    };

                    var findResult = mongoCollection.Find(doc).FirstOrDefault();

                    if (findResult != null)
                    {
                        nameObjectIdCollection.Add(attribute.Name, findResult["_id"].AsObjectId);
                    }
                    else
                    {
                        doc = new BsonDocument
                        {
                            { "testIdList", new BsonArray { new ObjectId(test.Info["ObjectId"].ToString()) } },
                            { "testNameList", new BsonArray { test.Name } },
                            { "testLength", 1 },
                            { "project", ProjectId },
                            { "report", ReportId },
                            { "name", attribute.Name },
                            { "timeTaken", Convert.ToInt64(0) }
                        };

                        mongoCollection.InsertOne(doc);
                        var id = doc["_id"].AsObjectId;
                        nameObjectIdCollection.Add(attribute.Name, id);
                    }
                }
                else
                {
                    var id = nameObjectIdCollection[attribute.Name];
                    int testLength = 1;

                    if (attributeContext != null)
                    {
                        var context = attributeContext.Context.Where(x => x.Key.Equals(attribute.Name));
                        if (context.Any())
                        {
                            testLength = context.Count() + 1;
                        }
                    }

                    var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
                    var push = Builders<BsonDocument>.Update
                        .Push("testIdList", new ObjectId(test.Info["ObjectId"].ToString()))
                        .Push("testNameList", test.Name);
                    mongoCollection.UpdateOne(filter, push);

                    var update = Builders<BsonDocument>.Update
                        .Set("testLength", testLength);
                    mongoCollection.UpdateOne(filter, update);
                }
            }
        }

        private void UploadMedia(Test test)
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

            if (test.HasScreenCapture)
            {
                foreach (var m in test.Media)
                {
                    var path = m.ResolvedPath ?? m.Path;

                    if (!m.Info.ContainsKey("TestObjectId") && !string.IsNullOrEmpty(path))
                    {
                        m.Info["TestObjectId"] = test.Info["ObjectId"];
                        m.Info["TestName"] = test.Name;
                        _mediaStorageHandler.SaveScreenCapture(m);
                    }
                }
            }

            foreach (var log in test.Logs.ToList())
            {
                if (log.HasMedia)
                {
                    var path = log.Media.ResolvedPath ?? log.Media.Path;

                    if (!log.Media.Info.ContainsKey("LogObjectId") && !string.IsNullOrEmpty(path))
                    {
                        log.Media.Info["TestObjectId"] = test.Info["ObjectId"];
                        log.Media.Info["LogObjectId"] = log.Info["ObjectId"];
                        log.Media.Info["TestName"] = test.Name;
                        _mediaStorageHandler.SaveScreenCapture(log.Media);
                    }
                }
            }
        }
    }
}
