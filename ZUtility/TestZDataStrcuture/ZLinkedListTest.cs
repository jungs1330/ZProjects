using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ZDataStructure;


namespace ZDataStrcutureTest
{
    [TestClass]
    public class ZLinkedListTest
    {
        private static List<int> list = new List<int>() { 6, 5, 4, 3, 2, 1, 7, 8, 9, 10 };

        [AssemblyInitialize()]
        public static void AssemblyInit(TestContext context)
        {
            Console.WriteLine("AssemblyInit " + context.TestName);
        }

        [ClassInitialize()]
        public static void ClassInit(TestContext context)
        {
           Console.WriteLine("ClassInit " + context.TestName);
        }

        [TestInitialize()]
        public void Initialize()
        {
            Console.WriteLine("TestMethodInit");
        }

        [TestCleanup()]
        public void Cleanup()
        {
            Console.WriteLine("TestMethodCleanup");
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            Console.WriteLine("ClassCleanup");
        }

        [AssemblyCleanup()]
        public static void AssemblyCleanup()
        {
            Console.WriteLine("AssemblyCleanup");
        }

        [TestMethod]
        [Timeout(2000)]  // Set Timeout in Milliseconds  
        public void ZLinkedListTestAdd()
        {
            ZLinkedList<int> linkedList = InitializeZLinkedList();
            CompareList(list, linkedList);
        }

        [TestMethod]
        public void ZLinkedListTestNodeAt()
        {
            ZLinkedList<int> linkedList = InitializeZLinkedList();
            for (int i = 0; i < list.Count; i++)
                Assert.AreEqual(list[i], linkedList.NodeAt(i));
        }

        [TestMethod]
        public void ZLinkedListTestIndex()
        {
            ZLinkedList<int> linkedList = InitializeZLinkedList();
            for (int i = 0; i < list.Count; i++)
                Assert.AreEqual(list[i], linkedList[i]);
        }

        [TestMethod]
        public void ZLinkedListTestClear()
        {
            ZLinkedList<int> linkedList = InitializeZLinkedList();
            linkedList.Clear();
            Assert.AreEqual(linkedList.Count, 0);
        }

        [TestMethod]
        public void ZLinkedListTestInsert()
        {
            ZLinkedList<int> linkedList = InitializeZLinkedList();
            List<int> _list = new List<int>() { 6, 5, 4, 3, 2, 1, 7, 8, 9, 10 };

            linkedList.Insert(5, 11);
            _list.Insert(1, 11);
            CompareList(_list, linkedList);
            
            linkedList.Insert(6, 12);
            _list.Insert(0, 12);
            CompareList(_list, linkedList);
            
            linkedList.Insert(10, 13);
            _list.Insert(_list.Count - 1, 13);
            CompareList(_list, linkedList);
        }

        [TestMethod]
        public void ZLinkedListTestInsertAt()
        {
            ZLinkedList<int> linkedList = InitializeZLinkedList();
            List<int> _list = new List<int>() { 6, 5, 4, 3, 2, 1, 7, 8, 9, 10 };

            linkedList.InsertAt(1, 11);
            _list.Insert(1, 11);
            CompareList(_list, linkedList);
            
            linkedList.InsertAt(0, 12);
            _list.Insert(0, 12);
            CompareList(_list, linkedList);
            
            linkedList.InsertAt(linkedList.Count - 1, 13);
            _list.Insert(_list.Count - 1, 13);
            CompareList(_list, linkedList);

            linkedList.InsertAt(linkedList.Count, 14);
            _list.Insert(_list.Count, 14);
            CompareList(_list, linkedList);
        }

        [TestMethod]
        public void ZLinkedListTestDelete()
        {
            ZLinkedList<int> linkedList = InitializeZLinkedList();
            List<int> _list = new List<int>() { 6, 5, 4, 3, 2, 1, 7, 8, 9, 10 };

            linkedList.Delete(5);
            _list.Remove(5);
            CompareList(_list, linkedList);

            linkedList.Delete(6);
            _list.Remove(6);
            CompareList(_list, linkedList);

            linkedList.Delete(10);
            _list.Remove(10);
            CompareList(_list, linkedList);
        }

        [TestMethod]
        public void ZLinkedListTestFindStartOfLoop()
        {
            ZLinkedList<int> linkedList = InitializeZLinkedList();

            ZLinkedListNode<int> node = linkedList.FindStartOfLoop();
            Assert.IsNull(node);

            linkedList.Tail.NextNode = linkedList.Head;
            node = linkedList.FindStartOfLoop();
            Assert.AreSame(linkedList.Head, node);

            linkedList.Tail.NextNode = linkedList.Head.NextNode.NextNode;
            node = linkedList.FindStartOfLoop();
            Assert.AreSame(linkedList.Head.NextNode.NextNode, node);
        }

        [TestMethod]
        public void ZLinkedListTestSearch()
        {
            ZLinkedList<int> linkedList = InitializeZLinkedList();

            Assert.AreEqual(list.IndexOf(6), linkedList.Search(6));
            Assert.AreEqual(list.IndexOf(2), linkedList.Search(2));
            Assert.AreEqual(list.IndexOf(10), linkedList.Search(10));
        }

        [TestMethod]
        public void ZLinkedListTestReverse()
        {
            ZLinkedList<int> linkedList = InitializeZLinkedList();
            List<int> _list = new List<int>() { 6, 5, 4, 3, 2, 1, 7, 8, 9, 10 };

            linkedList.Reverse();
            _list.Reverse();

            CompareList(_list, linkedList);
        }

        [TestMethod]
        public void ZLinkedListTestFindMiddle()
        {
            ZLinkedList<int> linkedList = new ZLinkedList<int>();
            linkedList.Add(1);
            Assert.AreEqual(1, linkedList.FindMiddle().Value);

            linkedList.Add(2);
            Assert.AreEqual(1, linkedList.FindMiddle().Value);

            linkedList.Add(3);
            Assert.AreEqual(2, linkedList.FindMiddle().Value);

            linkedList.Add(4);
            Assert.AreEqual(2, linkedList.FindMiddle().Value);

            linkedList.Add(5);
            Assert.AreEqual(3, linkedList.FindMiddle().Value);

            linkedList.Add(6);
            Assert.AreEqual(3, linkedList.FindMiddle().Value);
        }

        [TestMethod]
        public void ZLinkedListTestSort()
        {
            ZLinkedList<int> linkedList = new ZLinkedList<int>();
            List<int> _list = new List<int>() { 3 };
            linkedList.Add(3);
            linkedList.Sort();
            _list.Sort();
            CompareList(_list, linkedList);

            linkedList = new ZLinkedList<int>();
            _list = new List<int>() { 3, 2 };
            linkedList.Add(3);
            linkedList.Add(2);
            linkedList.Sort();
            _list.Sort();
            CompareList(_list, linkedList);

            linkedList = InitializeZLinkedList();
            _list = new List<int>() { 6, 5, 4, 3, 2, 1, 7, 8, 9, 10 };
            linkedList.Sort();
            _list.Sort();
            CompareList(_list, linkedList);
        }

        private ZLinkedList<int> InitializeZLinkedList()
        {
            ZLinkedList<int> linkedList = new ZLinkedList<int>();

            for (int i = 0; i < list.Count; i++ )
                linkedList.Add(list[i]);

            return linkedList;
        }

        private void CompareList(List<int> _list, ZLinkedList<int> _linkedList)
        {
            Assert.AreEqual(_list.Count, _linkedList.Count);
            Assert.AreEqual(_list[0], _linkedList.Head.Value);
            Assert.AreEqual(_list[_list.Count - 1], _linkedList.Tail.Value);
            CollectionAssert.AreEqual(_list, _linkedList.ToList());
        }
    }
}
