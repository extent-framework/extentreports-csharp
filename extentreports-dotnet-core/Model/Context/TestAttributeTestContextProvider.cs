using AventStack.ExtentReports.Core;

using System.Collections.Generic;
using System.Linq;

namespace AventStack.ExtentReports.Model.Context
{
    public class TestAttributeTestContextProvider<T> where T : Attribute
    {
        public TestAttributeTestContextProvider()
        {
            Context = new List<TestAttributeTestContext<T>>();
        }

        public List<TestAttributeTestContext<T>> Context { get; }

        public void AddAttributeContext(T attrib, Test test)
        {
            var context = Context.Where(x => x.Name.Equals(attrib.Name));

            if (context.Any())
            {
                if (!context.First().TestCollection.Where(x => x.TestId == test.TestId).Any())
                {
                    context.First().TestCollection.Add(test);
                }
                context.First().RefreshTestStatusCounts();
            }
            else
            {
                var testAttrTestContext = new TestAttributeTestContext<T>(attrib);
                testAttrTestContext.AddTest(test);
                Context.Add(testAttrTestContext);
            }
        }

        public void RemoveTest(Test test)
        {
            for (int i = Context.Count - 1; i >= 0; i--)
            {
                var context = Context[i];
                TestRemoveService.Remove(context.TestCollection, test);
                if (context.Count == 0)
                {
                    Context.RemoveAt(i);
                }
            }
        }
    }
}
