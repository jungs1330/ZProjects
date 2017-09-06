using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZDataStructure
{
    public class ZQueue<T> where T : IComparable
    {
        ZLinkedList<T> items = new ZLinkedList<T>();
        public void Enqueue(T data)
        {
            items.Add(data); 
        }

        public T Dequeue()
        {
            T data = items.Head.Value;
            items.Delete(items.Head.Value);
            return data;
        }

        public T Peek()
        {
            return items.Head.Value;
        }

        public int Count
        {
            get
            {
                return items.Count;
            }
        }

        public void Reverse()
        {
            items.Reverse();
        }

        public void Sort()
        {
            items.Sort();
        }

        public List<T> ToList()
        {
            return items.ToList();
        }
    }
}
