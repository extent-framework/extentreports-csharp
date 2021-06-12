using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using AventStack.ExtentReports.Util;

namespace AventStack.ExtentReports.Model.Context
{
    public class NamedAttributeContext<T> where T : NamedAttribute
    {
        public static int adding = 0;
        public static int removing = 0;


        public ConcurrentQueue<Test> Tests { get; private set; } = new ConcurrentQueue<Test>();
        public T Attr;
        public int Passed { get; private set; }
        public int Failed { get; private set; }
        public int Skipped { get; private set; }
        public int Others { get; private set; }

        private static object _synclock = new object();

        public NamedAttributeContext(T attr, Test test)
        {
            Attr = attr;
            AddTest(test);
        }

        public void AddTest(Test test)
        {
            Assert.NotNull(test, "Test cannot be null");
            Tests.Enqueue(test);
            Compute(test);
        }

        private void Compute(Test test)
        {
            lock (_synclock)
            {
                Passed += test.Status == Status.Pass ? 1 : 0;
                Failed += test.Status == Status.Fail ? 1 : 0;
                Skipped += test.Status == Status.Skip ? 1 : 0;
                Others += test.Status != Status.Pass && test.Status != Status.Fail
                        && test.Status != Status.Skip ? 1 : 0;
            }
        }

        public void Refresh()
        {
            Reset();

            foreach (var t in Tests)
            {
                Compute(t);
            }
        }

        public void RemoveTest(Test test)
        {
            lock (_synclock)
            {
                IEnumerable<Test> enumerable = Tests.Where(x => x.Id != test.Id);
                Tests = new ConcurrentQueue<Test>(enumerable);
            }
        }

        public void Reset()
        {
            Passed = Failed = Skipped = Others = 0;
        }

        public int Count()
        {
            return Tests.Count;
        }
    }
}
