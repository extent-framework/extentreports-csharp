using AventStack.ExtentReports.Model;
using System.Collections.Generic;

namespace AventStack.ExtentReports.Listener.Entity
{
    public class TestEntity : IObservedEntity
    {
        public List<Test> Tests { get; set; }
    }
}
