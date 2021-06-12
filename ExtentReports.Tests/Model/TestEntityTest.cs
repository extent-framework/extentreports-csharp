using System;
using System.Linq;
using AventStack.ExtentReports.Model;
using NUnit.Framework;

namespace AventStack.ExtentReports.Tests.Model
{
    public class TestEntityTest
    {
        private const string Name = "Test";
        private Test _test;

        [SetUp]
        public void Setup()
        {
            _test = new Test(Name);
        }

        [Test]
        public void LogSeqIncrements()
        {
            var log = new Log();
            Enumerable.Range(0, 99).ToList().ForEach(x =>
            {
                _test.AddLog(log);
                Assert.AreEqual(x+1, log.Seq);
            });
        }

        [Test]
        public void TestEntities()
        {
            Assert.AreEqual(_test.Author.Count, 0);
            Assert.AreEqual(_test.Device.Count, 0);
            Assert.AreEqual(_test.Category.Count, 0);
            Assert.AreEqual(_test.Children.Count, 0);
            Assert.True(_test.Leaf);
            Assert.AreEqual(_test.Level, 0);
            Assert.AreEqual(_test.Status, Status.Pass);
            Assert.Null(_test.Description);
        }

        [Test]
        public void AddNullChildToTest()
        {
            Assert.Throws(typeof(ArgumentNullException), () => _test.AddChild(null));
        }

        [Test]
        public void AddChildToTest()
        {
            var node = new Test("Node");
            _test.AddChild(node);
            Assert.AreEqual(_test.Children.Count, 1);
            _test.AddChild(node);
            Assert.AreEqual(_test.Children.Count, 2);
        }

        [Test]
        public void AddChildToTestLevel()
        {
            var node = new Test("Node");
            _test.AddChild(node);
            Assert.AreEqual(_test.Level, 0);
            Assert.AreEqual(node.Level, 1);
        }

        [Test]
        public void AddChildToTestLeaf()
        {
            var node = new Test("Node");
            _test.AddChild(node);
            Assert.False(_test.Leaf);
            Assert.True(node.Leaf);
        }

        [Test]
        public void AddNullLogToTest()
        {
            Assert.Throws(typeof(ArgumentNullException), () => _test.AddLog(null));
        }

        [Test]
        public void AddLogToTest()
        {
            var log = new Log();
            var test = new Test(Name);
            test.AddLog(log);
            Assert.AreEqual(1, log.Seq);
            Assert.AreEqual(1, test.Logs.Count);
            Assert.AreEqual(Status.Pass, test.Status);
            Assert.AreEqual(Status.Pass, log.Status);
        }

        [Test]
        public void AddSkipLogToTest()
        {
            var log = new Log(Status.Skip);
            _test.AddLog(log);
            Assert.AreEqual(_test.Status, Status.Skip);
            Assert.AreEqual(log.Status, Status.Skip);
        }

        [Test]
        public void AddFailLogToTest()
        {
            var log = new Log(Status.Fail);
            _test.AddLog(log);
            Assert.AreEqual(_test.Status, Status.Fail);
            Assert.AreEqual(log.Status, Status.Fail);
        }

        [Test]
        public void TestHasLog()
        {
            Assert.False(_test.HasLog);
            var log = new Log();
            _test.AddLog(log);
            Assert.True(_test.HasLog);
        }

        [Test]
        public void IsTestBDD()
        {
            Assert.False(_test.IsBdd);
            _test.BddType = new Gherkin.GherkinKeyword("given");
            Assert.True(_test.IsBdd);
        }

        [Test]
        public void TestHasChildren()
        {
            Assert.False(_test.HasChildren);
            var node = new Test(Name);
            _test.AddChild(node);
            Assert.True(_test.HasChildren);
        }

        [Test]
        public void TestStatusWithoutLog()
        {
            Assert.AreEqual(_test.Status, Status.Pass);
        }

        [Test]
        public void TestStatusWithLog()
        {
            Assert.AreEqual(_test.Status, Status.Pass);
            var log = new Log(Status.Fail);
            _test.AddLog(log);
            Assert.AreEqual(_test.Status, Status.Fail);
        }

        [Test]
        public void TestStatusWithLogStatusChanged()
        {
            Assert.AreEqual(_test.Status, Status.Pass);
            var log = new Log(Status.Skip);
            _test.AddLog(log);
            Assert.AreEqual(_test.Status, Status.Skip);
            log.Status = (Status.Fail);
            //_test.UpdateResult();
            //Assert.AreEqual(_test.Status, Status.Fail);
        }

        [Test]
        public void HasAuthor()
        {
            Assert.False(_test.HasAuthor);
            _test.Author.Add(new Author("x"));
            Assert.True(_test.HasAuthor);
        }

        [Test]
        public void HasCategory()
        {
            Assert.False(_test.HasCategory);
            _test.Category.Add(new Category("x"));
            Assert.True(_test.HasCategory);
        }

        [Test]
        public void HasDevice()
        {
            Assert.False(_test.HasDevice);
            _test.Device.Add(new Device("x"));
            Assert.True(_test.HasDevice);
        }

        [Test]
        public void HasAttributes()
        {
            Assert.False(_test.HasAttributes);
            
            _test.Author.Add(new Author("x"));
            Assert.True(_test.HasAttributes);

            _test = new Test(Name);
            _test.Device.Add(new Device("x"));
            Assert.True(_test.HasAttributes);

            _test = new Test(Name);
            _test.Category.Add(new Category("x"));
            Assert.True(_test.HasAttributes);
        }
        
        [Test]
        public void TestFullName()
        {
            var name = new String[] { "Test", "Child", "Grandchild" };
            var test = new Test(name[0]);
            var child = new Test(name[1]);
            var grandchild = new Test(name[2]);
            test.AddChild(child);
            child.AddChild(grandchild);
            Assert.AreEqual(name[0], test.FullName);
            Assert.AreEqual(name[0] + "." + name[1], child.FullName);
            Assert.AreEqual(grandchild.FullName,
                    name[0] + "." + name[1] + "." + name[2]);
        }
        
        [Test]
        public void HasScreenCapture()
        {
            Assert.False(_test.HasScreenCapture);
            _test.AddMedia(new ScreenCapture());
            Assert.False(_test.HasScreenCapture);
            _test.AddMedia(new ScreenCapture("/img"));
            Assert.True(_test.HasScreenCapture);
        }

        [Test]
        public void ComputeTestStatusNoLog()
        {
            _test.UpdateResult();
            Assert.AreEqual(Status.Pass, _test.Status);
        }

        [Test]
        public void ComputeTestStatusSkipLog()
        {
            _test.AddLog(new Log(Status.Skip));
            _test.UpdateResult();
            Assert.AreEqual(Status.Skip, _test.Status);
        }

        [Test]
        public void ComputeTestStatusSkipAndFailLog()
        {
            _test.AddLog(new Log(Status.Skip));
            _test.AddLog(new Log(Status.Fail));
            _test.UpdateResult();
            Assert.AreEqual(Status.Fail, _test.Status);
        }
        
        [Test]
        public void ComputeTestStatusNode()
        {
            var parent = new Test("");
            var node = new Test("");
            parent.AddChild(node);
            node.AddLog(new Log(Status.Skip));
            parent.UpdateResult();
            Assert.AreEqual(Status.Skip, parent.Status);
            Assert.AreEqual(Status.Skip, node.Status);
            node.AddLog(new Log(Status.Fail));
            parent.UpdateResult();
            Assert.AreEqual(Status.Fail, parent.Status);
            Assert.AreEqual(Status.Fail, node.Status);
        }

        [Test]
        public void ComputeTestStatusAndNodeStatus()
        {
            var parent = new Test("");
            var node = new Test("");
            parent.AddChild(node);
            node.AddLog(new Log(Status.Skip));
            parent.UpdateResult();
            Assert.AreEqual(Status.Skip, parent.Status);
            Assert.AreEqual(Status.Skip, node.Status);
            parent.AddLog(new Log(Status.Fail));
            parent.UpdateResult();
            Assert.AreEqual(Status.Fail, parent.Status);
            Assert.AreEqual(Status.Skip, node.Status);
        }

        [Test]
        public void Ancestor()
        {
            var parent = new Test("");
            var node = new Test("");
            var child = new Test("");
            parent.AddChild(node);
            node.AddChild(child);
            Assert.AreEqual(parent.Ancestor, parent);
            Assert.AreEqual(node.Ancestor, parent);
            Assert.AreEqual(child.Ancestor, parent);
        }
        /*
        [Test]
        public void generatedLog()
        {
            Test test = getTest();
            Logs log = Logs.builder().status(Status.Skip).details("details").build();
            _test.addGeneratedLog(log);
            Assert.AreEqual(_test.getGeneratedLog.Count, 1);
            Assert.AreEqual(_test.Logs.Count, 0);
            Assert.AreEqual(_test.Status, Status.Skip);
        }

        [Test]
        public void testHasAngLogWithNoLogs()
        {
            Test test = getTest();
            Assert.False(_test.hasAnyLog());
        }

        [Test]
        public void testHasAngLogWithLog()
        {
            Test test = getTest();
            Logs log = Logs.builder().status(Status.Skip).details("details").build();
            _test.AddLog(log);
            Assert.True(_test.hasAnyLog());
        }

        [Test]
        public void testHasAngLogWithGeneratedLog()
        {
            Test test = getTest();
            Logs log = Logs.builder().status(Status.Skip).details("details").build();
            _test.addGeneratedLog(log);
            Assert.True(_test.hasAnyLog());
        }
        */
        [Test]
        public void TimeTaken()
        {
            double duration = _test.TimeTaken;
            Assert.True(duration < 5);
        }
    }
}
