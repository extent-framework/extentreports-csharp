using System.Collections.Concurrent;
using System.Linq;

namespace AventStack.ExtentReports.Model.Context.Manager
{
    public class NamedAttributeContextManager<T> where T: NamedAttribute
    {
        public ConcurrentDictionary<string, NamedAttributeContext<T>> Context =
            new ConcurrentDictionary<string, NamedAttributeContext<T>>();

        public bool HasItems
        {
            get
            {
                return !Context.IsEmpty;
            }
        }

        public void AddContext(T attr, Test test)
        {
            if (Context.ContainsKey(attr.Name))
            {
                if (!Context[attr.Name].Tests.Any(x => x.Id == test.Id))
                {
                    Context[attr.Name].AddTest(test);
                }
            } 
            else
            {
                Context.TryAdd(attr.Name, new NamedAttributeContext<T>(attr, test));
            }
        }

        public void AddContext(T attr, ConcurrentBag<Test> bag)
        {
            foreach (var v in bag)
            {
                AddContext(attr, v);
            }
        }

        public void RemoveTest(Test test)
        {
            foreach (var context in Context)
            {
                context.Value.RemoveTest(test);
                if (context.Value.Tests.Count == 0)
                {
                    _ = Context.TryRemove(context.Value.Attr.Name, out _);
                }
            }
        }

        public void RefreshAll()
        {
            foreach (var kv in Context)
            {
                kv.Value.Refresh();
            }
        }
    }
}
