/*  
 Author: Riaan Hanekom

 Copyright 2007 Riaan Hanekom

 Permission is hereby granted, free of charge, to any person obtaining
 a copy of this software and associated documentation files (the
 "Software"), to deal in the Software without restriction, including
 without limitation the rights to use, copy, modify, merge, publish,
 distribute, sublicense, and/or sell copies of the Software, and to
 permit persons to whom the Software is furnished to do so, subject to
 the following conditions:

 The above copyright notice and this permission notice shall be
 included in all copies or substantial portions of the Software.

 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
 LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
 OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
 WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.Text;

using NGenerics.DataStructures;
using NUnit.Framework;
using NGenerics.Visitors;

namespace ValueStructuresDotNetTests
{
	[TestFixture]
	public class CustomLinkedListTest
	{
		[Test]
		public void TestSuccesfulInit()
		{
			VisitableLinkedList<int> list = new VisitableLinkedList<int>();

			Assert.AreEqual(list.First, null);
			Assert.AreEqual(list.Last, null);
		}

		[Test]
		public void TestIsEmpty()
		{
			VisitableLinkedList<int> list = new VisitableLinkedList<int>();
			Assert.AreEqual(list.IsEmpty, true);

			list.AddLast(5);
			Assert.AreEqual(list.IsEmpty, false);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestNullVisitor()
		{
			VisitableLinkedList<int> l = new VisitableLinkedList<int>();
			l.Accept(null);
		}

		[Test]
		public void TestVisitor()
		{
			VisitableLinkedList<int> l = GetTestCustomLinkedList();
			SumVisitor visitor = new SumVisitor();

			l.Accept(visitor);

			Assert.AreEqual(visitor.Sum, 0 + 3 + 6 + 9 + 12);
		}

		[Test]
		public void TestStoppingVisitor()
		{
			VisitableLinkedList<int> l = GetTestCustomLinkedList();

			ComparableFindingVisitor<int> visitor = new ComparableFindingVisitor<int>(6);
			l.Accept(visitor);

			Assert.AreEqual(visitor.Found, true);

			visitor = new ComparableFindingVisitor<int>(99);
			l.Accept(visitor);
			Assert.AreEqual(visitor.Found, false);
		}

		[Test]
		public void TestIsFull()
		{
			VisitableLinkedList<int> list = new VisitableLinkedList<int>();
			Assert.AreEqual(list.IsFull, false);

			list.AddLast(5);
			Assert.AreEqual(list.IsFull, false);
		}

		[Test]
		public void TestInsertItems()
		{
			VisitableLinkedList<int> list = GetTestCustomLinkedList();

			LinkedListNode<int> currentElement = list.First;

			// Verify the list items and their contents
			for (int i = 0; i < 5; i++)
			{
				Assert.AreEqual(currentElement.Value, i * 3);
				currentElement = currentElement.Next;
			}
		}

		[Test]
		public void TestFirst()
		{
			VisitableLinkedList<int> testList = GetTestCustomLinkedList();
			Assert.AreEqual(testList.First.Value, 0);
		}

		[Test]
		public void TestLast()
		{
			VisitableLinkedList<int> testList = GetTestCustomLinkedList();
			Assert.AreEqual(testList.Last.Value, 12);
		}
				
		[Test]		
		public void TestFirstListEmpty()
		{
			VisitableLinkedList<int> list = new VisitableLinkedList<int>();
			Assert.AreEqual(list.First, null);
		}
				
		[Test]		
		public void TestLastListEmpty()
		{
			VisitableLinkedList<int> list = new VisitableLinkedList<int>();
			Assert.AreEqual(list.Last, null);
		}

		[Test]
		public void TestAddFirstFullList()
		{
			VisitableLinkedList<int> testList = GetTestCustomLinkedList();
			testList.AddFirst(15);

			Assert.AreEqual(testList.First.Value, 15);
			Assert.AreNotEqual(testList.First.Next, null);
			Assert.AreEqual(testList.First.Next.Value, 0);
		}

		[Test]
		public void TestAddLastFullList()
		{
			VisitableLinkedList<int> testList = GetTestCustomLinkedList();
			testList.AddLast(15);

			Assert.AreEqual(testList.Last.Value, 15);
			Assert.AreEqual(testList.Last.Next, null);
		}

		[Test]
		public void TestAddFirstEmptyList()
		{
			VisitableLinkedList<int> testList = new VisitableLinkedList<int>();
			testList.AddFirst(16);

			Assert.AreNotEqual(testList.First, null);
			Assert.AreEqual(testList.First.Value, 16);
			Assert.AreNotEqual(testList.Last, null);
			Assert.AreEqual(testList.Last.Value, 16);
			Assert.AreEqual(testList.First.Next, null);
		}

		[Test]
		public void TestAddLastEmptyList()
		{
			VisitableLinkedList<int> testList = new VisitableLinkedList<int>();
			testList.AddLast(16);

			Assert.AreNotEqual(testList.First, null);
			Assert.AreEqual(testList.First.Value, 16);
			Assert.AreNotEqual(testList.Last, null);
			Assert.AreEqual(testList.Last.Value, 16);
			Assert.AreEqual(testList.First.Next, null);
		}

		[Test]
		public void TestInsertAfter()
		{
			VisitableLinkedList<int> list = GetTestCustomLinkedList();

			LinkedListNode<int> currentElement = list.First;
			LinkedListNode<int> secondElement = currentElement.Next;

			list.AddAfter(secondElement, 23);

			// Verify the list items and their contents
			for (int i = 0; i < 6; i++)
			{
				if (i < 2)
				{
					Assert.AreEqual(currentElement.Value, i * 3);
				}
				else if (i == 2)
				{
					Assert.AreEqual(currentElement.Value, 23);
				}
				else
				{
					Assert.AreEqual(currentElement.Value, (i - 1) * 3);
				}

				currentElement = currentElement.Next;
			}
		}

		[Test]
		public void TestInsertBefore()
		{
			VisitableLinkedList<int> list = GetTestCustomLinkedList();

			LinkedListNode<int> thirdElement = list.First.Next.Next;

			list.AddBefore(thirdElement, 55);

			LinkedListNode<int> currentElement = list.First;

			// Verify the list items and their contents
			for (int i = 0; i < 6; i++)
			{
				if (i < 2)
				{
					Assert.AreEqual(currentElement.Value, i * 3);
				}
				else if (i == 2)
				{
					Assert.AreEqual(currentElement.Value, 55);
				}
				else
				{
					Assert.AreEqual(currentElement.Value, (i - 1) * 3);
				}

				currentElement = currentElement.Next;
			}
		}

		[Test]
		public void TestInsertBefore1Item()
		{
			VisitableLinkedList<int> list = new VisitableLinkedList<int>();
			list.AddLast(5);

			list.AddBefore(list.First, 6);			

			Assert.AreEqual(list.First.Value, 6);
			Assert.AreEqual(list.First.Next.Value, 5);
			Assert.AreEqual(list.Last.Value, 5);
		}

		[Test]
		public void TestCompareTo()
		{
			VisitableLinkedList<int> l1 = new VisitableLinkedList<int>();
			l1.AddFirst(5);

			VisitableLinkedList<int> l2 = new VisitableLinkedList<int>();
			l2.AddFirst(3);
			l2.AddFirst(4);

			Assert.AreEqual(l1.CompareTo(l2), -1);
			Assert.AreEqual(l2.CompareTo(l1), 1);
			Assert.AreEqual(l1.CompareTo(l1), 0);

			object o = new object();

			Assert.AreEqual(l1.CompareTo(o), -1);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestInvalidCompareTo()
		{
			VisitableLinkedList<string> l = new VisitableLinkedList<string>();
			l.CompareTo(null);
		}

		[Test]
		public void TestFixedSize()
		{
			VisitableLinkedList<int> l = new VisitableLinkedList<int>();
			Assert.AreEqual(l.IsFixedSize, false);

			l = new VisitableLinkedList<int>();
			l.AddFirst(4);

			Assert.AreEqual(l.IsFixedSize, false);
		}

		[Test]
		public void TestInsertBeforeFirst()
		{
			VisitableLinkedList<int> list = GetTestCustomLinkedList();
			LinkedListNode<int> currentElement = list.First;

			list.AddBefore(currentElement, 657);
			Assert.AreEqual(list.First.Value, 657);

			currentElement = list.First;

			// Verify the list items and their contents
			for (int i = 0; i < 6; i++)
			{
				if (i == 0)
				{
					Assert.AreEqual(currentElement.Value, 657);
				}
				else
				{
					Assert.AreEqual(currentElement.Value, (i - 1) * 3);
				}

				currentElement = currentElement.Next;
			}
		}

		[Test]
		public void TestClear()
		{
			VisitableLinkedList<int> list = GetTestCustomLinkedList();
			list.Clear();

			Assert.AreEqual(list.First, null);
			Assert.AreEqual(list.Last, null);
			Assert.AreEqual(list.IsEmpty, true);
		}

		[Test]
		public void TestRemove()
		{
			VisitableLinkedList<int> list = GetTestCustomLinkedList();
			LinkedListNode<int> currentElement = list.First;

			list.Remove(9);

			// Verify the list items and their contents
			for (int i = 0; i < 5; i++)
			{
				if (i == 3)
				{
					continue;
				}
				else
				{
					Assert.AreEqual(currentElement.Value, i * 3);
					currentElement = currentElement.Next;
				}
			}
		}

		[Test]
		public void TestRemoveNotInList()
		{
			// This shoudln't throw any exceptions
			VisitableLinkedList<int> list = GetTestCustomLinkedList();
			list.Remove(999);
		}

		[Test]
		public void TestRemoveFirst()
		{
			VisitableLinkedList<int> list = GetTestCustomLinkedList();

			list.Remove(0);

			LinkedListNode<int> currentElement = list.First;

			// Verify the list items and their contents
			for (int i = 0; i < 4; i++)
			{
				Assert.AreEqual(currentElement.Value, (i + 1) * 3);
				currentElement = currentElement.Next;
			}
		}

		[Test]
		public void TestRemoveLast()
		{
			VisitableLinkedList<int> list = GetTestCustomLinkedList();

			list.Remove(12);

			LinkedListNode<int> currentElement = list.First;

			// Verify the list items and their contents
			for (int i = 0; i < 4; i++)
			{
				Assert.AreEqual(currentElement.Value, i * 3);
				currentElement = currentElement.Next;
			}
		}

		private VisitableLinkedList<int> GetTestCustomLinkedList()
		{
			VisitableLinkedList<int> list = new VisitableLinkedList<int>();

			for (int i = 0; i < 5; i++)
			{
				list.AddLast(i * 3);
			}

			return list;
		}
	}
}
