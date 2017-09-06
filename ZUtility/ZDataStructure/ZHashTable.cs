using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZDataStructure
{
    public class ZHashTable<TKey, TValue>
    {
        private ZLinkedList<Tuple<TKey, TValue>>[] items;
        private int fillFactor = 3;
        private int size;

        public ZHashTable()
        {
            items = new ZLinkedList<Tuple<TKey, TValue>>[4];
        }

        public void Add(TKey key, TValue value)
        {
            int pos = GetPosition(key, items.Length);
            
            if (items[pos] == null)
                items[pos] = new ZLinkedList<Tuple<TKey, TValue>>();

            ZLinkedListNode<Tuple<TKey, TValue>> current = items[pos].Head;
            while (current != null)
            {
                if (current.Value.Item1.Equals(key))
                {
                    throw new Exception("Duplicate key, cannot insert.");
                }
                current = current.NextNode;
            }

            size++;
            if(NeedToGrow())
                GrowAndRehash();

            pos = GetPosition(key, items.Length);

            if (items[pos] == null)
                items[pos] = new ZLinkedList<Tuple<TKey, TValue>>();

            items[pos].Add(new Tuple<TKey, TValue>(key, value));
        }

        public void Remove(TKey key)
        {
            int pos = GetPosition(key, items.Length);
            if (items[pos] != null)
            {
                ZLinkedListNode<Tuple<TKey, TValue>> current = items[pos].Head;
                while (current != null)
                {
                    if (current.Value.Item1.Equals(key))
                    {
                        items[pos].Delete(current.Value);
                        size--;
                        return;
                    }
                    current = current.NextNode;
                }
            }
            else
                throw new Exception("Value does not exist in Hashtable.");
        }

        public TValue Get(TKey key)
        {
            int pos = GetPosition(key, items.Length);
            if (items[pos] != null)
            {
                ZLinkedListNode<Tuple<TKey, TValue>> current = items[pos].Head;
                while (current != null)
                {
                    if (current.Value.Item1.Equals(key))
                        return current.Value.Item2;
                    current = current.NextNode;
                }
            }
            throw new Exception("Key does not exist in Hashtable.");
        }

        private void GrowAndRehash()
        {
            fillFactor *= 2;

            ZLinkedList<Tuple<TKey, TValue>>[] newItems = new ZLinkedList<Tuple<TKey, TValue>>[items.Length * 2];
            foreach(var item in items.Where(x => x != null))
            {
                ZLinkedListNode<Tuple<TKey, TValue>> current = item.Head;
                while (current != null)
                {
                    var pos = GetPosition(current.Value.Item1, newItems.Length);
                    if (newItems[pos] == null)
                        newItems[pos] = new ZLinkedList<Tuple<TKey, TValue>>();
                    newItems[pos].Add(new Tuple<TKey, TValue>(current.Value.Item1, current.Value.Item2));
                    current = current.NextNode;
                }
            }
            items = newItems;
        }

        private int GetPosition(TKey key, int length)
        {
            int hash = key.GetHashCode();
            return Math.Abs(hash % length);
        }

        private bool NeedToGrow()
        {
            return size >= fillFactor;
        }
    }
}
