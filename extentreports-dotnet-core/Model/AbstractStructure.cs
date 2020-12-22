using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace AventStack.ExtentReports.Model
{
    [Serializable]
    public class GenericStructure<T>
    {
        private List<T> _list
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
        private List<T> _listv = new List<T>();

        private readonly object _syncLock = new object();

        public bool IsEmpty
        {
            get
            {
                lock (_syncLock)
                {
                    return Count == 0;
                }
            }
        }

        public int Count
        {
            get
            {
                lock (_syncLock)
                {
                    return _list == null ? 0 : _list.Count;
                }
            }
        }

        public bool Contains(T t)
        {
            lock (_syncLock)
            {
                return _list.Contains(t);
            }
        }

        public void Add(T t)
        {
            lock (_syncLock)
            {
                _list.Add(t);
            }
        }

        public void Remove(T t)
        {
            lock (_syncLock)
            {
                _list.ToList().Remove(t);
            }
        }

        public T Get(int index)
        {
            lock (_syncLock)
            {
                return _list.ElementAt(index);
            }
        }

        public T FirstOrDefault()
        {
            lock (_syncLock)
            {
                return _list.Count == 0 ? default(T) : Get(0);
            }
        }

        public T LastOrDefault()
        {
            lock (_syncLock)
            {
                return _list.Count == 0 ? default(T) : Get(_list.Count - 1);
            }
        }

        public List<T> All()
        {
            return _list;
        }

        public TIterator<T> GetEnumerator()
        {
            return new TIterator<T>(_list);
        }
    }
}
