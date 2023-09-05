using AventStack.ExtentReports.Gherkin;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace AventStack.ExtentReports.Model.Convert
{
    internal class TestEntityParser
    {
        private readonly ExtentReports _extent;

        public TestEntityParser(ExtentReports extent)
        {
            _extent = extent;
        }

        public void CreateEntities(string jsonFilePath)
        {
            if (!File.Exists(jsonFilePath))
            {
                return;
            }

            _extent.UsingNaturalConf = false;

            var text = File.ReadAllText(jsonFilePath);
            var tests = JsonConvert.DeserializeObject<List<Test>>(text);

            foreach (var test in tests)
            {
                if (test.BddType == null)
                {
                    CreateEntity(test, _extent.CreateTest(test.Name, test.Description));
                }
                else
                {
                    var keyword = new GherkinKeyword(test.BddType.Name);
                    var et = _extent.CreateTest(keyword, test.Name, test.Description);
                    CreateEntity(test, et);
                }
            }
        }

        public void CreateEntity(Test test, ExtentTest extentTest)
        {
            extentTest.Test.StartTime = test.StartTime;
            extentTest.Test.EndTime = test.EndTime;

            if (test.Logs != null)
            {
                foreach (var log in test.Logs)
                {
                    ScreenCapture m = null;

                    if (log.Media != null)
                    {
                        m = log.Media;
                    }
                    if (log.Details != null)
                    {
                        extentTest.Log(log.Status, log.Details, m);
                    }
                    if (log.ExceptionInfo != null)
                    {
                        extentTest.Log(log.Status, log.ExceptionInfo.Exception, m);
                    }

                }
            }

            foreach (var x in test.Author)
            {
                extentTest.AssignAuthor(x.Name);
            }

            foreach (var x in test.Category)
            {
                extentTest.AssignCategory(x.Name);
            }

            foreach (var x in test.Device)
            {
                extentTest.AssignDevice(x.Name);
            }

            foreach (var x in test.Media)
            {
                if (x.Base64 != null)
                {
                    extentTest.AddScreenCaptureFromBase64String(x.Base64);
                }
                else
                {
                    var path = x.ResolvedPath ?? x.Path;
                    extentTest.AddScreenCaptureFromPath(path);
                }
            }

            if (test.HasChildren)
            {
                foreach (var node in test.Children)
                {
                    ExtentTest extentNode;

                    if (test.BddType == null)
                    {
                        extentNode = extentTest.CreateNode(node.Name, node.Description);
                    }
                    else
                    {
                        var keyword = new GherkinKeyword(node.BddType.Name);
                        extentNode = extentTest.CreateNode(keyword, node.Name, node.Description);
                    }

                    CreateEntity(node, extentNode);
                }
            }
        }
    }
}
