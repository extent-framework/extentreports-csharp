using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AventStack.ExtentReports.Model
{
    [Serializable]
    public class TIterator<T> : IEnumerable<T>
    {
        private List<T> _list;

        public TIterator(List<T> list)
        {
            _list = list;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int ix = 0; ix < _list.Count(); ix++)
            {
                yield return _list[ix];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
