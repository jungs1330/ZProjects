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

namespace NGenericsTests
{
	[TestFixture]
	public class DequeTest
	{
		[Test]
		public void TestSuccesfulInit()
		{
			Deque<int> d = new Deque<int>();
			Assert.AreEqual(d.Count, 0);
			Assert.AreEqual(d.IsEmpty, true);
			Assert.AreEqual(d.IsFull, false);

			VisitableStack<int> stack = new VisitableStack<int>();

			for (int i = 0; i < 3; i++)
			{
				stack.Push(i * 4);
			}

			d = new Deque<int>(stack);

			for (int i = 2; i >= 0; i--)
			{
				Assert.AreEqual(d.DequeueHead(), i * 4);
			}
		}

		[Test]
		public void TestHead()
		{
			Deque<int> d = GetTestDeque();
			int i = d.Head;
		}

		[Test]
		[ExpectedException(typeof(NotSupportedException))]
		public void TestInterfaceAdd()
		{
			ICollection<int> d = GetTestDeque();
			d.Add(5);
		}
		
		[Test]
		[ExpectedException(typeof(NotSupportedException))]
		public void TestInterfaceRemove()
		{
			ICollection<int> d = GetTestDeque();
			d.Remove(5);
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void TestEmptyHead()
		{
			Deque<int> d = new Deque<int>();
			Assert.AreEqual(d.Head, 15);
		}

		[Test]
		public void TestTail()
		{
			Deque<int> d = GetTestDeque();
			Assert.AreEqual(d.Tail, 0);
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void TestEmptytail()
		{
			Deque<int> d = new Deque<int>();
			int i = d.Tail;
		}

		[Test]
		public void TestIsFull()
		{
			Deque<int> d = GetTestDeque();
			Assert.AreEqual(d.IsFull, false);

			d.DequeueHead();

			Assert.AreEqual(d.IsFull, false);

			d = new Deque<int>();
			Assert.AreEqual(d.IsFull, false);
		}

		[Test]
		public void TestClear()
		{
			Deque<int> d = GetTestDeque();
			Assert.AreEqual(d.IsFull, false);

			d.Clear();
			Assert.AreEqual(d.IsEmpty, true);
			Assert.AreEqual(d.Count, 0);
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void TestEmptyDequeHead()
		{
			Deque<int> d = new Deque<int>();
			d.DequeueHead();
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void TestEmptyDequeTail()
		{
			Deque<int> d = new Deque<int>();
			d.DequeueTail();
		}

		[Test]
		public void TestIsEmpty()
		{
			Deque<int> d = GetTestDeque();
			Assert.AreEqual(d.IsEmpty, false);

			d.DequeueHead();

			Assert.AreEqual(d.IsEmpty, false);

			d.Clear();
			Assert.AreEqual(d.IsEmpty, true);
		}

		[Test]
		public void TestEnqueueHead()
		{
			Deque<int> d = new Deque<int>();
			d.EnqueueHead(6);

			Assert.AreEqual(d.IsEmpty, false);
			Assert.AreEqual(d.Head, 6);
			Assert.AreEqual(d.Tail, 6);
			Assert.AreEqual(d.Count, 1);

			d.EnqueueHead(3);

			Assert.AreEqual(d.Head, 3);
			Assert.AreEqual(d.Tail, 6);
			Assert.AreEqual(d.Count, 2);

			d.EnqueueHead(5);

			Assert.AreEqual(d.Head, 5);
			Assert.AreEqual(d.Tail, 6);
			Assert.AreEqual(d.Count, 3);
		}

		[Test]
		public void TestEnqueueTail()
		{
			Deque<int> d = new Deque<int>();
			d.EnqueueTail(6);

			Assert.AreEqual(d.IsEmpty, false);
			Assert.AreEqual(d.Head, 6);
			Assert.AreEqual(d.Tail, 6);

			Assert.AreEqual(d.Count, 1);

			d.EnqueueTail(3);

			Assert.AreEqual(d.Head, 6);
			Assert.AreEqual(d.Tail, 3);

			Assert.AreEqual(d.Count, 2);

			d.EnqueueTail(5);

			Assert.AreEqual(d.Head, 6);
			Assert.AreEqual(d.Tail, 5);

			Assert.AreEqual(d.Count, 3);
		}

		[Test]
		public void TestDeqeueHead()
		{
			Deque<int> d = GetTestDeque();

			for (int i = 4; i >= 0; i--)
			{
				Assert.AreEqual(d.DequeueHead(), i * 3);
				Assert.AreEqual(d.Count, i);
			}
		}

		[Test]
		public void TestDeqeueTail()
		{
			Deque<int> d = GetTestDeque();

			for (int i = 0; i < 5; i++)
			{
				Assert.AreEqual(d.DequeueTail(), i * 3);
				Assert.AreEqual(d.Count, 4 - i);
			}
		}

		[Test]
		public void TestEnumerator()
		{
			Deque<int> d = GetTestDeque();
			IEnumerator<int> enumerator = d.GetEnumerator();

			int count = 5;

			while (enumerator.MoveNext())
			{
				count--;
				Assert.AreEqual(enumerator.Current, count * 3);
			}

			enumerator.Dispose();
		}

		[Test]
		public void TestInterfaceEnumerator()
		{
			Deque<int> d = GetTestDeque();
			System.Collections.IEnumerator enumerator = d.GetEnumerator();

			int count = 5;

			while (enumerator.MoveNext())
			{
				count--;
				Assert.AreEqual(enumerator.Current, count * 3);
			}
		}

		[Test]
		public void TestIsFixedSize()
		{
			Deque<int> d = new Deque<int>();
			Assert.AreEqual(d.IsFixedSize, false);

			d = GetTestDeque();
			Assert.AreEqual(d.IsFixedSize, false);
		}

		[Test]
		public void TestReadOnly()
		{
			Deque<int> d = new Deque<int>();
			Assert.AreEqual(d.IsReadOnly, false);

			d = GetTestDeque();
			Assert.AreEqual(d.IsReadOnly, false);
		}


		[Test]
		public void TestCompareTo()
		{
			Deque<int> test1 = GetTestDeque();
			Deque<int> test2 = GetTestDeque();
			Deque<int> test3 = new Deque<int>();

			object o = new object();

			Assert.AreEqual(test1.CompareTo(test2), 0);
			Assert.AreEqual(test1.CompareTo(test3), 1);
			Assert.AreEqual(test3.CompareTo(test1), -1);

			Assert.AreEqual(test1.CompareTo(o), -1);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestInvalidCompareTo()
		{
			Deque<string> d = new Deque<string>();
			d.CompareTo(null);
		}

		[Test]
		public void TestContains()
		{
			Deque<int> test = new Deque<int>();
			Assert.AreEqual(test.Contains(5), false);

			test.EnqueueHead(5);
			test.EnqueueHead(6);

			Assert.AreEqual(test.Contains(5), true);
			Assert.AreEqual(test.Contains(6), true);
		}

		[Test]
		public void TestCopyTo()
		{
			Deque<int> d = new Deque<int>();
			
			for (int i = 1; i < 20; i++)
			{
				d.EnqueueHead(i);
			}

			int[] array = new int[19];

			d.CopyTo(array, 0);

			int counter = 1;

			for (int i = 18; i > 0; i--)
			{
				Assert.AreEqual(array[i], counter);
				counter++;
			}
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestNullArrayCopyTo()
		{
			Deque<int> d = new Deque<int>();
			d.CopyTo(null, 0);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestInvalidArrayCopyTo1()
		{
			Deque<int> d = new Deque<int>();
			
			for (int i = 1; i < 20; i++)
			{
				d.EnqueueHead(i);
			}

			int[] array = new int[19];

			d.CopyTo(array, 1);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestInvalidArrayCopyTo2()
		{
			Deque<int> d = new Deque<int>();

			for (int i = 1; i < 20; i++)
			{
				d.EnqueueHead(i);
			}

			int[] array = new int[18];

			d.CopyTo(array, 0);
		}

		[Test]
		public void TestAccept()
		{
			Deque<int> d = new Deque<int>();
			d.EnqueueHead(5);
			d.EnqueueHead(3);
			d.EnqueueHead(2);

			TrackingVisitor<int> visitor = new TrackingVisitor<int>();

			d.Accept(visitor);

			Assert.AreEqual(visitor.TrackingList.Count, 3);
			Assert.AreEqual(visitor.TrackingList.Contains(5), true);
			Assert.AreEqual(visitor.TrackingList.Contains(3), true);
			Assert.AreEqual(visitor.TrackingList.Contains(2), true);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestNullVisitor()
		{
			Deque<int> d = new Deque<int>();
			d.Accept(null);
		}
			

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestInvalidCopyToArrayTooSmall()
		{
			Deque<int> d = new Deque<int>();
			d.EnqueueHead(5);
			d.EnqueueTail(3);
			d.EnqueueTail(2);
			d.EnqueueHead(55);

			int[] array = new int[3];
			d.CopyTo(array, 0);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestInvalidCopyToNotEnoughSpaceFromIndex()
		{
			Deque<int> d = new Deque<int>();
			d.EnqueueHead(5);
			d.EnqueueTail(3);
			d.EnqueueTail(2);
			d.EnqueueHead(55);

			int[] array = new int[4];
			d.CopyTo(array, 1);
		}

		public Deque<int> GetTestDeque()
		{
			Deque<int> test = new Deque<int>();

			for (int i = 0; i < 5; i++)
			{
				test.EnqueueHead(i * 3);
			}

			return test;
		}
	}
}
