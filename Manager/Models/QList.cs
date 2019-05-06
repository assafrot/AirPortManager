using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manager.Models
{
    public class QList<T> : IEnumerable<T>
    {
        List<T> list = new List<T>();
        public void Enqueue(T obj)
        {
            lock (list)
            {
                list.Add(obj);
            }
        }

        public T Dequeue()
        {
            lock (list)
            {
                var obj = list.FirstOrDefault();
                if (obj != null)
                {
                    list.RemoveAt(0);
                }
                return obj;
            }
        }

        public void Remove(T obj)
        {
            lock (list)
            {
                list.Remove(obj);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
           return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public T this[int index]
        {
            get
            {
                try
                {
                    return list[index];
                }
                catch
                {
                    throw new IndexOutOfRangeException();
                }

            }
            set
            {
                lock (list)
                {
                    if (index >= 0 && index < list.Count)
                    {
                        list[index] = value;
                    }
                }
            }
        }

    }
}
