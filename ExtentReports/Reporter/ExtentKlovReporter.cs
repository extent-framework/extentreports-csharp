using AventStack.ExtentReports.Core;
using AventStack.ExtentReports.Model;

using MongoDB.Bson;
using MongoDB.Driver;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AventStack.ExtentReports.Reporter
{
    public class ExtentKlovReporter : AbstractReporter
    {
        public override string ReporterName => "klov";

        public override AnalysisStrategy AnalysisStrategy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override ReportStatusStats ReportStatusStats { get => throw new NotImplementedException(); protected internal set => throw new NotImplementedException(); }

        public string ReportName { get; private set; }
        public ObjectId ReportId { get; private set; }
        public string ProjectName { get; private set; }
        public ObjectId ProjectId { get; private set; }

        public override void Flush(ReportAggregates reportAggregates)
        {
            if (reportAggregates.TestList == null || reportAggregates.TestList.Count == 0)
            {
                return;
            }

            //var duration = DateTime.Now.Subtract(reportAggregates.Ti).TotalMilliseconds;

            //if (duration.ToString().Contains("."))
            //    duration = Convert.ToDouble(duration.ToString().Split('.')[0]);

            List<String> categoryNameList = null;
            List<ObjectId> categoryIdList = null;

            //var filter = Builders<BsonDocument>.Filter.Eq("_id", _reportId);
            var update = Builders<BsonDocument>.Update
                .Set("endTime", DateTime.Now)
                //.Set("duration", duration)
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
                .Set("categoryIdList", categoryIdList);

            //_reportCollection.UpdateOne(filter, update);

            //InsertUpdateSystemAttribute();
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
                { "project", _projectId },
                { "report", _reportId },
                { "testName", test.Name },
                { "sequence", log.Sequence },
                { "status", log.Status.ToString().ToLower() },
                { "timestamp", log.Timestamp },
                { "details", log.Details }
            };

            if (log.HasScreenCapture && log.ScreenCaptureContext.FirstOrDefault().IsBase64)
            {
                document.Add("details", log.Details + log.ScreenCaptureContext.FirstOrDefault().Source);
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
                    { "report", _reportId },
                    { "project", _projectId },
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
                            { "project", _projectId },
                            { "report", _reportId },
                            { "name", ex.Name },
                            { "stacktrace", ex.StackTrace },
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
            throw new NotImplementedException();
        }

        public override void OnScreenCaptureAdded(Log log, ScreenCapture screenCapture)
        {
            throw new NotImplementedException();
        }

        public override void OnTestRemoved(Test test)
        {
            throw new NotImplementedException();
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
                { "project", _projectId },
                { "report", _reportId },
                { "reportName", _reportId },
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

        public override void Start()
        {
            throw new NotImplementedException();
        }

        public override void Stop() { }

        public ExtentKlovReporter(string projectName, string reportName)
        {
            if (projectName == null || reportName == null)
            {
                throw new ArgumentNullException("At least one of the passed arguments is null but should never be null");
            }

            _startTime = DateTime.Now;
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

        }

        private const string DefaultProjectName = "Default";
        private const string DefaultKlovServerName = "klov";

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

        private Dictionary<string, ObjectId> _categoryNameObjectIdCollection;
        private Dictionary<string, ObjectId> _exceptionNameObjectIdCollection;
    }
}
