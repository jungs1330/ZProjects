using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ZDataStructure;

namespace ZDataStrcutureTest
{
    [TestClass]
    public class ZStackByListTest
    {
        private static List<int> list = new List<int>() { 6, 5, 4, 3, 2, 1, 7, 8, 9, 10 };

        [TestMethod]
        public void ZStackByListTestPush()
        {
            ZStackByList<int> stack = InitializeZStackByList();
            CompareList(list, stack);
        }

        [TestMethod]
        public void ZStackByListTestPop()
        {
            ZStackByList<int> stack = InitializeZStackByList();
            Assert.AreEqual(10, stack.Pop());
            Assert.AreEqual(9, stack.Pop());
            Assert.AreEqual(8, stack.Pop());
            Assert.AreEqual(7, stack.Pop());
            Assert.AreEqual(1, stack.Pop());
            Assert.AreEqual(2, stack.Pop());
            Assert.AreEqual(3, stack.Pop());
            Assert.AreEqual(4, stack.Pop());
            Assert.AreEqual(5, stack.Pop());
            Assert.AreEqual(6, stack.Pop());
            Assert.AreEqual(0, stack.Count);
        }

        [TestMethod]
        public void ZStackByListTestPeek()
        {
            ZStackByList<int> stack = InitializeZStackByList();
            Assert.AreEqual(10, stack.Peek());
            Assert.AreEqual(10, stack.Count);
            stack.Pop();
            Assert.AreEqual(9, stack.Peek());
            Assert.AreEqual(9, stack.Count);
            stack.Pop();
            Assert.AreEqual(8, stack.Peek());
            Assert.AreEqual(8, stack.Count);
            stack.Pop();
            Assert.AreEqual(7, stack.Peek());
            Assert.AreEqual(7, stack.Count);
            stack.Pop();
            Assert.AreEqual(1, stack.Peek());
            Assert.AreEqual(6, stack.Count);
            stack.Pop();
            Assert.AreEqual(2, stack.Peek());
            Assert.AreEqual(5, stack.Count);
            stack.Pop();
            Assert.AreEqual(3, stack.Peek());
            Assert.AreEqual(4, stack.Count);
            stack.Pop();
            Assert.AreEqual(4, stack.Peek());
            Assert.AreEqual(3, stack.Count);
            stack.Pop();
            Assert.AreEqual(5, stack.Peek());
            Assert.AreEqual(2, stack.Count);
            stack.Pop();
            Assert.AreEqual(6, stack.Peek());
            Assert.AreEqual(1, stack.Count);
            stack.Pop();
            Assert.AreEqual(0, stack.Count);
        }

        [TestMethod]
        public void ZStackByListTestReverse()
        {
            ZStackByList<int> stack = InitializeZStackByList();
            List<int> _list = new List<int>() { 6, 5, 4, 3, 2, 1, 7, 8, 9, 10 };
            _list.Reverse();
            stack.Reverse();
            CompareList(_list, stack);
        }

        [TestMethod]
        public void ZStackByListTestSort()
        {
            ZStackByList<int> stack = InitializeZStackByList();
            List<int> _list = new List<int>() { 6, 5, 4, 3, 2, 1, 7, 8, 9, 10 };
            _list.Sort();
            stack.Sort();
            Assert.AreEqual(_list.Count, stack.Count);
            CollectionAssert.AreEqual(_list, stack.ToList());
        }

        private ZStackByList<int> InitializeZStackByList()
        {
            ZStackByList<int> stack = new ZStackByList<int>();

            for (int i = 0; i < list.Count; i++)
                stack.Push(list[i]);

            return stack;
        }

        private void CompareList(List<int> _list, ZStackByList<int> stack)
        {
            Assert.AreEqual(_list.Count, stack.Count);
            List<int> stackList = stack.ToList();
            stackList.Reverse();
            CollectionAssert.AreEqual(_list, stackList);
        }
    }
}
