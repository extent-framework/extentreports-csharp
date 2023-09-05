using AventStack.ExtentReports.Model;
using NUnit.Framework;
using System;
using System.Linq;

namespace AventStack.ExtentReports.Tests.Model
{
    class ReportStatsTest
    {
        [Test]
        public void AnalysisStrategyDefault()
        {
            var stats = new ReportStats();
            Assert.AreEqual(AnalysisStrategy.Test, stats.AnalysisStrategy);
        }

        [Test]
        public void AllLevelMapsNonNull()
        {
            var stats = new ReportStats();
            Assert.NotNull(stats.Child);
            Assert.NotNull(stats.ChildPercentage);
            Assert.NotNull(stats.Grandchild);
            Assert.NotNull(stats.GrandchildPercentage);
            Assert.NotNull(stats.Log);
            Assert.NotNull(stats.LogPercentage);
            Assert.NotNull(stats.Parent);
            Assert.NotNull(stats.ParentPercentage);
        }

        [Test]
        public void StatsCount()
        {
            var report = new Report();

            Assert.AreEqual(0, report.Stats.Parent.Count);

            report.Stats.Update(report.Tests);
            Assert.AreEqual(Enum.GetValues(typeof(Status)).Length, report.Stats.Parent.Count);
        }

        [Test]
        public void AllStatsPresent()
        {
            var report = new Report();
            report.Stats.Update(report.Tests);

            // check if all Status fields are present
            var list = Enum.GetValues(typeof(Status)).Cast<Status>().ToList();
            list.ForEach(x => Assert.True(report.Stats.Parent.ContainsKey(x)));
            list.ForEach(x => Assert.True(report.Stats.Child.ContainsKey(x)));
            list.ForEach(x => Assert.True(report.Stats.Grandchild.ContainsKey(x)));
            list.ForEach(x => Assert.True(report.Stats.Log.ContainsKey(x)));
        }

        [Test]
        public void ParentStatsCounts()
        {
            var report = new Report();
            report.Stats.Update(report.Tests);
            Assert.AreEqual(0, report.Stats.Parent[Status.Pass]);
            Assert.AreEqual(0, report.Stats.Parent[Status.Fail]);
            Assert.AreEqual(0, report.Stats.Parent[Status.Skip]);
            Assert.AreEqual(0, report.Stats.Parent[Status.Warning]);
            Assert.AreEqual(0, report.Stats.Parent[Status.Info]);
        }

        [Test]
        public void ChildStatsCounts()
        {
            var report = new Report();
            report.Stats.Update(report.Tests);
            Assert.AreEqual(0, report.Stats.Child[Status.Pass]);
            Assert.AreEqual(0, report.Stats.Child[Status.Fail]);
            Assert.AreEqual(0, report.Stats.Child[Status.Skip]);
            Assert.AreEqual(0, report.Stats.Child[Status.Warning]);
            Assert.AreEqual(0, report.Stats.Child[Status.Info]);
        }

        [Test]
        public void GrandchildStatsCounts()
        {
            var report = new Report();
            report.Stats.Update(report.Tests);
            Assert.AreEqual(0, report.Stats.Grandchild[Status.Pass]);
            Assert.AreEqual(0, report.Stats.Grandchild[Status.Fail]);
            Assert.AreEqual(0, report.Stats.Grandchild[Status.Skip]);
            Assert.AreEqual(0, report.Stats.Grandchild[Status.Warning]);
            Assert.AreEqual(0, report.Stats.Grandchild[Status.Info]);
        }

        [Test]
        public void StatsTestStatus()
        {
            var test = new Test("Test");
            var report = new Report();
            report.AddTest(test);
            report.Stats.Update(report.Tests);

            // check if all Status fields are present
            var list = Enum.GetValues(typeof(Status)).Cast<Status>().ToList();
            list.ForEach(x => Assert.True(report.Stats.Parent.ContainsKey(x)));
            list.ForEach(x => Assert.True(report.Stats.Child.ContainsKey(x)));
            list.ForEach(x => Assert.True(report.Stats.Grandchild.ContainsKey(x)));
            list.ForEach(x => Assert.True(report.Stats.Log.ContainsKey(x)));

            test.Status = Status.Fail;
            report.Stats.Update(report.Tests);
            Assert.AreEqual(0, report.Stats.Parent[Status.Pass]);
            Assert.AreEqual(1, report.Stats.Parent[Status.Fail]);
        }

        [Test]
        public void StatsChildStatus()
        {
            var test = new Test("Test");
            var node = new Test("Node")
            {
                Status = Status.Skip
            };
            test.AddChild(node);

            var report = new Report();
            report.AddTest(test);

            report.Stats.Update(report.Tests);

            Assert.AreEqual(0, report.Stats.Parent[Status.Pass]);
            Assert.AreEqual(1, report.Stats.Parent[Status.Skip]);
            Assert.AreEqual(0, report.Stats.Child[Status.Pass]);
            Assert.AreEqual(1, report.Stats.Child[Status.Skip]);
        }

        [Test]
        public void StatsMultipleChildStatus()
        {
            var test = new Test("Test");
            var node1 = new Test("Node1");
            var node2 = new Test("Node2");
            node1.Status = Status.Skip;
            node2.Status = Status.Fail;
            test.AddChild(node1);
            test.AddChild(node2);

            var report = new Report();
            report.AddTest(test);

            report.Stats.Update(report.Tests);

            Assert.AreEqual(0, report.Stats.Parent[Status.Pass]);
            Assert.AreEqual(0, report.Stats.Parent[Status.Skip]);
            Assert.AreEqual(1, report.Stats.Parent[Status.Fail]);
            Assert.AreEqual(0, report.Stats.Child[Status.Pass]);
            Assert.AreEqual(1, report.Stats.Child[Status.Skip]);
            Assert.AreEqual(1, report.Stats.Child[Status.Fail]);
        }

        [Test]
        public void StatsGrandchildStatus()
        {
            var test = new Test("Test");
            var node = new Test("Node");
            var grandchild = new Test("Grandchild")
            {
                Status = Status.Fail
            };
            node.AddChild(grandchild);
            test.AddChild(node);

            var report = new Report();
            report.AddTest(test);

            report.Stats.Update(report.Tests);

            Assert.AreEqual(0, report.Stats.Parent[Status.Pass]);
            Assert.AreEqual(1, report.Stats.Parent[Status.Fail]);
            Assert.AreEqual(0, report.Stats.Child[Status.Pass]);
            Assert.AreEqual(1, report.Stats.Child[Status.Fail]);
            Assert.AreEqual(0, report.Stats.Grandchild[Status.Pass]);
            Assert.AreEqual(1, report.Stats.Grandchild[Status.Fail]);
        }

        [Test]
        public void StatsLogStatus()
        {
            var test = new Test("Test");
            var node = new Test("Node");
            test.AddChild(node);

            test.AddLog(new Log(Status.Skip));
            test.AddLog(new Log(Status.Skip));
            test.AddLog(new Log(Status.Info));
            test.AddLog(new Log(Status.Pass));

            node.AddLog(new Log(Status.Fail));
            node.AddLog(new Log(Status.Fail));
            node.AddLog(new Log(Status.Info));
            node.AddLog(new Log(Status.Pass));

            test.UpdateResult();

            var report = new Report();
            report.AddTest(test);

            report.Stats.Update(report.Tests);

            Assert.AreEqual(1, report.Stats.Parent[Status.Fail]);
            Assert.AreEqual(1, report.Stats.Child[Status.Fail]);
            Assert.AreEqual(2, report.Stats.Log[Status.Skip]);
            Assert.AreEqual(2, report.Stats.Log[Status.Fail]);
            Assert.AreEqual(2, report.Stats.Log[Status.Pass]);
            Assert.AreEqual(2, report.Stats.Log[Status.Info]);
        }
    }
}
