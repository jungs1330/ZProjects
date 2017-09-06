using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZDataStructure
{
    public class ZStackByLinkedList<T> where T : IComparable
    {
        private ZLinkedList<T> items = new ZLinkedList<T>();

        public void Push(T data)
        {
            items.InsertAt(0, data);
        }

        public T Pop()
        {
            T data;
            lock (this)
            {
                data = items.Head.Value;
                items.Delete(items.Head.Value);
            }
            return data;
        }

        public T Peek()
        {
            T data = items.Head.Value;
            return data;
        }

        public int Count 
        { 
            get
            {
                return items.Count;
            }
        }

        public List<T> ToList()
        {
            return items.ToList();
        }

        public void Reverse()
        {
            items.Reverse();
        }

        public void Sort()
        {
            items.Sort();
        }
    }
}
