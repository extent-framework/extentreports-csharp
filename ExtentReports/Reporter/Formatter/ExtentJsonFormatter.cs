using AventStack.ExtentReports.Listener.Entity;
using Newtonsoft.Json;
using System;
using System.IO;

namespace AventStack.ExtentReports.Reporter
{
    public class ExtentJsonFormatter : AbstractFileReporter, IObserver<ReportEntity>
    {
        public ExtentJsonFormatter(string filePath) : base(filePath)
        {
        }

        public void OnCompleted()
        {

        }

        public void OnError(Exception error)
        {

        }

        public void OnNext(ReportEntity value)
        {
            var json = JsonConvert.SerializeObject(value.Report.Tests, Formatting.None);
            File.WriteAllText(SavePath, json);
        }
    }
}
