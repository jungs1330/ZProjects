using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZDataStructure
{
    public class ZStackByList<T> where T : IComparable
    {
        private List<T> items = new List<T>();

        public void Push(T data)
        {
            items.Add(data);
        }

        public T Pop()
        {
            T data;
            lock (this)
            {
                data = items[items.Count - 1];
                items.RemoveAt(items.Count - 1);
            }
            return data;
        }

        public T Peek()
        {
            T data = items[items.Count - 1];
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
            List<T> newItems = items.ToList<T>();
            newItems.Reverse();
            return newItems;
        }

        public void Reverse()
        {
            int back = items.Count - 1;
            int front = 0;

            lock (this)
            {
                while (front < back)
                {
                    T tmp = items[front];
                    items[front] = items[back];
                    items[back] = tmp;
                    front++;
                    back--;
                }
            }
        }

        public void Sort()
        {
            items.Sort();
            items.Reverse();
        }
    }
}
