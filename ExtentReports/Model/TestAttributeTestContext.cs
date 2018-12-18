using System;
using System.Collections.Generic;
using System.Linq;

namespace AventStack.ExtentReports.Model
{
    [Serializable]
    public class TestAttributeTestContext<T> where T : TestAttribute
    {
        public T TestAttribute { get; private set; }
        public List<Test> TestCollection { get; private set; } = new List<Test>();
        public string Name { get; private set; }
        public int Passed { get; private set; } = 0;
        public int Failed { get; private set; } = 0;
        public int Skipped { get; private set; } = 0;
        public int Others { get; private set; } = 0;

        public int Count
        {
            get
            {
                return TestCollection.Any() ? TestCollection.Count : 0;
            }
        }

        private readonly object _synclock = new object();

        public TestAttributeTestContext(T testAttribute)
        {
            TestAttribute = testAttribute;
            Name = testAttribute.Name;
        }

        public void AddTest(Test test)
        {
            lock (_synclock)
            {
                UpdateTestStatusCounts(test);
                TestCollection.Add(test);
            }
        }

        private void UpdateTestStatusCounts(Test test)
        {
            Passed += test.Status == Status.Pass ? 1 : 0;
            Failed += test.Status == Status.Fail || test.Status == Status.Fatal ? 1 : 0;
            Skipped += test.Status == Status.Skip ? 1 : 0;
            Others += test.Status != Status.Pass && test.Status != Status.Fail && test.Status != Status.Fatal && test.Status != Status.Skip ? 1 : 0;
        }

        public void RefreshTestStatusCounts()
        {
            Passed = Failed = Skipped = Others = 0;
            TestCollection.ForEach(x => UpdateTestStatusCounts(x));
        }
    }
}
