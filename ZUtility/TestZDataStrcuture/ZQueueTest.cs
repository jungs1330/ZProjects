using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ZDataStructure;


namespace ZDataStrcutureTest
{
    [TestClass]
    public class ZQueueTest
    {
        private static List<int> list = new List<int>() { 6, 5, 4, 3, 2, 1, 7, 8, 9, 10 };

        [TestMethod]
        public void ZQueueTestEnqueue()
        {
            ZQueue<int> queue = InitializeZQueue();
            CompareList(list, queue);
        }

        [TestMethod]
        public void ZQueueTestPop()
        {
            ZQueue<int> queue = InitializeZQueue();
            Assert.AreEqual(6, queue.Dequeue());
            Assert.AreEqual(5, queue.Dequeue());
            Assert.AreEqual(4, queue.Dequeue());
            Assert.AreEqual(3, queue.Dequeue());
            Assert.AreEqual(2, queue.Dequeue());
            Assert.AreEqual(1, queue.Dequeue());
            Assert.AreEqual(7, queue.Dequeue());
            Assert.AreEqual(8, queue.Dequeue());
            Assert.AreEqual(9, queue.Dequeue());
            Assert.AreEqual(10, queue.Dequeue());
        }

        [TestMethod]
        public void ZQueueTestPeek()
        {
            ZQueue<int> queue = InitializeZQueue();
            Assert.AreEqual(6, queue.Peek());
            Assert.AreEqual(10, queue.Count);
            queue.Dequeue();
            Assert.AreEqual(5, queue.Peek());
            Assert.AreEqual(9, queue.Count);
            queue.Dequeue();
            Assert.AreEqual(4, queue.Peek());
            Assert.AreEqual(8, queue.Count);
            queue.Dequeue();
            Assert.AreEqual(3, queue.Peek());
            Assert.AreEqual(7, queue.Count);
            queue.Dequeue();
            Assert.AreEqual(2, queue.Peek());
            Assert.AreEqual(6, queue.Count);
            queue.Dequeue();
            Assert.AreEqual(1, queue.Peek());
            Assert.AreEqual(5, queue.Count);
            queue.Dequeue();
            Assert.AreEqual(7, queue.Peek());
            Assert.AreEqual(4, queue.Count);
            queue.Dequeue();
            Assert.AreEqual(8, queue.Peek());
            Assert.AreEqual(3, queue.Count);
            queue.Dequeue();
            Assert.AreEqual(9, queue.Peek());
            Assert.AreEqual(2, queue.Count);
            queue.Dequeue();
            Assert.AreEqual(10, queue.Peek());
            Assert.AreEqual(1, queue.Count);
            queue.Dequeue();
            Assert.AreEqual(0, queue.Count);
        }

        [TestMethod]
        public void ZQueueTestReverse()
        {
            ZQueue<int> queue = InitializeZQueue();
            List<int> _list = new List<int>() { 6, 5, 4, 3, 2, 1, 7, 8, 9, 10 };
            _list.Reverse();
            queue.Reverse();
            CompareList(_list, queue);
        }

        [TestMethod]
        public void ZQueueTestSort()
        {
            ZQueue<int> queue = InitializeZQueue();
            List<int> _list = new List<int>() { 6, 5, 4, 3, 2, 1, 7, 8, 9, 10 };
            _list.Sort();
            queue.Sort();
            Assert.AreEqual(_list.Count, queue.Count);
            CollectionAssert.AreEqual(_list, queue.ToList());
        }

        private ZQueue<int> InitializeZQueue()
        {
            ZQueue<int> stack = new ZQueue<int>();

            for (int i = 0; i < list.Count; i++)
                stack.Enqueue(list[i]);

            return stack;
        }

        private void CompareList(List<int> _list, ZQueue<int> stack)
        {
            Assert.AreEqual(_list.Count, stack.Count);
            List<int> stackList = stack.ToList();
            CollectionAssert.AreEqual(_list, stackList);
        }
    }
}