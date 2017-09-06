using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZDataStructure
{
    public class ZLinkedListNode<T> where T : IComparable
    {
        public ZLinkedListNode<T> NextNode { get; set; }

        public T Value { get; set; }

        public ZLinkedListNode()
        {
        }

        public ZLinkedListNode(T data)
        {
            this.Value = data;
        }
    }
}
