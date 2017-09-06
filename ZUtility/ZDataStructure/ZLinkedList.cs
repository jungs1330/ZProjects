using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZDataStructure
{
    public class ZLinkedList<T> where T : IComparable
    {
        private int count = 0;
        public ZLinkedListNode<T> Head { get; set; }

        public ZLinkedListNode<T> Tail { get; set; }

        public int Count { get { return count; } }

        public void Clear()
        {
            lock(this)
            {
                this.Head = null;
                count = 0;
            }
        }
        public void Add(T data)
        {
            ZLinkedListNode<T> node = new ZLinkedListNode<T>();
            node.Value = data;

            lock (this)
            {
                if (this.Head == null)
                {
                    this.Head = node;
                    this.Tail = node;
                }
                else
                {
                    this.Tail.NextNode = node;
                    this.Tail = node;
                }
                count++;
            }
        }

        public T NodeAt(int index)
        {
            ZLinkedListNode<T> temp = this.Head;

            for (int i = 0; i < index; i++)
                temp = temp.NextNode;

            return temp.Value;
        }

        public T this[int index]
        {
            get { return this.NodeAt(index); }
            set { this.InsertAt(index, value);  }
        }

        public int Search(T data)
        {
            ZLinkedListNode<T> temp = this.Head;
            int index = 0;
            while(temp != null)
            {
                if (temp.Value.CompareTo(data) == 0)
                    return index;

                temp = temp.NextNode;
                index++;
            }
                
            return -1;
        }

        public void Insert(T dataBefore, T data)
        {
            ZLinkedListNode<T> node = new ZLinkedListNode<T>(data);
            ZLinkedListNode<T> temp = this.Head;

            if (this.Head.Value.CompareTo(dataBefore) == 0)
            {
                node.NextNode = this.Head;
                this.Head = node;
            }
            else
            {

                while (temp.NextNode != null && temp.NextNode.Value.CompareTo(dataBefore) != 0)
                {
                    temp = temp.NextNode;
                }

                lock (this)
                {
                    if (temp.NextNode.Value.CompareTo(dataBefore) == 0)
                    {
                        node.NextNode = temp.NextNode;
                        temp.NextNode = node;
                    }
                }
            }
            count++;
        }

        public void InsertAt(int index, T data)
        {
            ZLinkedListNode<T> node = new ZLinkedListNode<T>(data);
            ZLinkedListNode<T> temp;

            lock (this)
            {
                if (index == 0)
                {
                    node.NextNode = this.Head;
                    this.Head = node;
                    if (Count == 0)
                        this.Tail = this.Head;
                }
                else
                {
                    temp = this.Head;
                    for (int i = 0; i < index - 1; i++)
                        temp = temp.NextNode;

                    node.NextNode = temp.NextNode;
                    temp.NextNode = node;

                    if (temp == this.Tail)
                        this.Tail = node;
                }
                count++;
            }
        }

        public void Delete(T data)
        {
            if (this.Head.Value.CompareTo(data) == 0)
            {
                lock (this)
                {
                    this.Head = this.Head.NextNode;
                    count--;
                }
            }
            else
            {
                ZLinkedListNode<T> temp = this.Head.NextNode;
                ZLinkedListNode<T> pNode = this.Head;

                lock (this)
                {
                    while (temp != null && temp.Value.CompareTo(data) != 0)
                    {
                        pNode = temp;
                        temp = temp.NextNode;
                    }

                    if (temp.Value.CompareTo(this.Tail.Value) == 0)
                    {
                        pNode.NextNode = null;
                        this.Tail = pNode;
                        count--;
                    }
                    else if (temp.Value.CompareTo(data) == 0)
                    {
                        pNode.NextNode = temp.NextNode;
                        count--;
                    }
                }
            }
        }

        public List<T> ToList()
        { 
            List<T> output = new List<T>();

            ZLinkedListNode<T> temp = this.Head;

            while (temp != null)
            {
                output.Add(temp.Value);
                temp = temp.NextNode;
            }
            
            return output;
        }

        public void Reverse()
        {
            ZLinkedListNode<T> currentNode = this.Head;
            ZLinkedListNode<T> nextNode = this.Head.NextNode;
            ZLinkedListNode<T> temp;

            lock (this)
            {
                while (nextNode != null)
                {
                    temp = nextNode.NextNode;
                    nextNode.NextNode = currentNode;
                    currentNode = nextNode;
                    nextNode = temp;
                }
            }

            currentNode = this.Tail;
            this.Tail = this.Head;
            this.Head = currentNode;
            this.Tail.NextNode = null;
        }

        public ZLinkedListNode<T> FindStartOfLoop()
        {
            ZLinkedListNode<T> slow = this.Head;
            ZLinkedListNode<T> fast = this.Head;

            if (fast.NextNode == null || fast.NextNode.NextNode == null)
                return null;

            slow = slow.NextNode;
            fast = fast.NextNode.NextNode;

            while (slow != fast && slow != null && fast != null)
            {
                slow = slow.NextNode;
                fast = fast.NextNode != null ? fast.NextNode.NextNode : null;
            }

            if (slow != null && fast != null && slow == fast)
            {
                fast = this.Head;
                
                while (fast != slow)
                {
                    fast = fast.NextNode;
                    slow = slow.NextNode;
                }
                return slow;
            }
            else
                return null;
        }

        public ZLinkedListNode<T> FindMiddle()
        {
            ZLinkedListNode<T> slow = this.Head;
            ZLinkedListNode<T> fast = this.Head;

            if (fast.NextNode == null || fast.NextNode.NextNode == null)
                return this.Head;

            while (fast.NextNode != null && fast.NextNode.NextNode != null)
            {
                slow = slow.NextNode;
                fast = fast.NextNode.NextNode;
            }

            return slow;
        }

        public ZLinkedList<T> MergeSortedLinkedList(ZLinkedList<T> linkedList)
        {
            ZLinkedList<T> newList = new ZLinkedList<T>();

            ZLinkedListNode<T> node1 = this.Head;
            ZLinkedListNode<T> node2 = linkedList.Head;

            while (node1 != null || node2 != null)
            {
                if ((node1 != null && node1.Value.CompareTo(node2.Value) >= 1) || node2 == null)
                {
                    newList.Add(node1.Value);

                    if (node1 != null && node1.NextNode != null)
                        node1 = node1.NextNode;
                }
                else
                {
                    newList.Add(node2.Value);

                    if (node2 != null && node2.NextNode != null)
                        node2 = node2.NextNode;
                }
            }

            return newList;
        }

        //Uses Merge Sort algorithm
        public void Sort()
        {
            this.Head = MergeSort(this.Head);

            this.Tail = this.Head;
            while (this.Tail.NextNode != null)
                this.Tail = this.Tail.NextNode;
        }

        private ZLinkedListNode<T> MergeSort(ZLinkedListNode<T> startNode)
        {
            //Break the list until list is null or only 1 element is present in List.
            if (startNode == null || startNode.NextNode == null)
            {
                return startNode;
            }

            //Break the linklist into 2 list.
            //Finding Middle node and then breaking the Linled list in 2 parts.
            //Now 2 list are, 1st list from start to middle and 2nd list from middle+1 to last.

            ZLinkedListNode<T> middle = FindMiddle(startNode);
            ZLinkedListNode<T> nextOfMiddle = middle.NextNode;
            middle.NextNode = null;

            //Again breaking the List until there is only 1 element in each list.
            ZLinkedListNode<T> left = MergeSort(startNode);
            ZLinkedListNode<T> right = MergeSort(nextOfMiddle);

            //Once complete list is divided and contains only single element, 
            //Start merging left and right half by sorting them and passing Sorted list further. 
            ZLinkedListNode<T> sortedList = MergeTwoListRecursive(left, right);

            return sortedList;
        }

        //Recursive Approach for Merging Two Sorted List
        private ZLinkedListNode<T> MergeTwoListRecursive(ZLinkedListNode<T> leftStart, ZLinkedListNode<T> rightStart)
        {
            if (leftStart == null)
                return rightStart;

            if (rightStart == null)
                return leftStart;

            ZLinkedListNode<T> temp = null;

            //if (leftStart.Value < rightStart.Value)
            if (leftStart.Value.CompareTo(rightStart.Value) < 0)
            {
                temp = leftStart;
                temp.NextNode = MergeTwoListRecursive(leftStart.NextNode, rightStart);
            }
            else
            {
                temp = rightStart;
                temp.NextNode = MergeTwoListRecursive(leftStart, rightStart.NextNode);
            }
            return temp;
        }

        public ZLinkedListNode<T> FindMiddle(ZLinkedListNode<T> head)
        {
            if (head == null) { return head; }
            ZLinkedListNode<T> slow = head;
            ZLinkedListNode<T> fast = head;

            while(fast.NextNode != null && fast.NextNode.NextNode != null)
            {
                slow = slow.NextNode;
                fast = fast.NextNode.NextNode;
            }

            return slow;
        }
    }
}
