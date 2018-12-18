using System;
using System.Collections.Generic;

namespace AventStack.ExtentReports.Model
{
    [Serializable]
    public class GenericStructure<T>
    {
        private List<T> _list
        {
            get
            {
                lock (_syncLock)
                {
                    return _listv;
                }
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
            lock (_syncLock)
            {
                _list.Add(t);
            }
        }

        public void Remove(T t)
        {
            lock (_syncLock)
            {
                _list.Remove(t);
            }
        }

        public T Get(int index)
        {
            lock (_syncLock)
            {
                return _list[index];
            }
        }

        public T FirstOrDefault()
        {
            lock (_syncLock)
            {
                return _list.Count == 0 ? default(T) : _list[0];
            }
        }

        public T LastOrDefault()
        {
            lock (_syncLock)
            {
                return _list.Count == 0 ? default(T) : _list[_list.Count - 1];
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
