using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace AventStack.ExtentReports.Model
{
    [Serializable]
    public class GenericStructure<T>
    {
        private BlockingCollection<T> _list
        {
            get
            {
                return _listv;
            }
            set
            {
                lock (_syncLock)
                {
                    _listv = value;
                }
            }
        }
        private BlockingCollection<T> _listv = new BlockingCollection<T>();

        private readonly object _syncLock = new object();

        public bool IsEmpty
        {
            get
            {
                return Count == 0;
            }
        }

        public int Count
        {
            get
            {
                return _list == null ? 0 : _list.Count;
            }
        }

        public bool Contains(T t)
        {
            return _list.Contains(t);
        }

        public void Add(T t)
        {
            _list.Add(t);
        }

        public void Remove(T t)
        {
            _list.ToList().Remove(t);
        }

        public T Get(int index)
        {
            return _list.ElementAt(index);
        }

        public T FirstOrDefault()
        {
            return _list.Count == 0 ? default(T) : Get(0);
        }

        public T LastOrDefault()
        {
            return _list.Count == 0 ? default(T) : Get(_list.Count - 1);
        }

        public BlockingCollection<T> All()
        {
            return _list;
        }

        public TIterator<T> GetEnumerator()
        {
            return new TIterator<T>(_list.GetConsumingEnumerable().ToList());
        }
    }
}
