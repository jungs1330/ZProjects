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
using NUnit.Framework;
using NGenerics.DataStructures;
using System.Diagnostics;
using NGenerics.Visitors;
using NGenerics.Enumerations;

namespace NGenericsTests
{
	[TestFixture]
	public class MaxHeapTest
	{
		[Test]
		public void TestSuccesfulInit()
		{
			Heap<int> h = new Heap<int>(HeapType.MaxHeap);

			Assert.AreEqual(h.Type, HeapType.MaxHeap);
			Assert.AreEqual(h.Count, 0);
			Assert.AreEqual(h.IsFull, false);
			Assert.AreEqual(h.IsEmpty, true);

			h = new Heap<int>(HeapType.MaxHeap, Comparer<int>.Default);

			Assert.AreEqual(h.Type, HeapType.MaxHeap);
			Assert.AreEqual(h.Count, 0);
			Assert.AreEqual(h.IsFull, false);
			Assert.AreEqual(h.IsEmpty, true);

			h = new Heap<int>(HeapType.MaxHeap, 50);

			Assert.AreEqual(h.Type, HeapType.MaxHeap);
			Assert.AreEqual(h.Count, 0);
			Assert.AreEqual(h.IsFull, false);
			Assert.AreEqual(h.IsEmpty, true);

			h = new Heap<int>(HeapType.MaxHeap, 50, Comparer<int>.Default);

			Assert.AreEqual(h.Type, HeapType.MaxHeap);
			Assert.AreEqual(h.Count, 0);
			Assert.AreEqual(h.IsFull, false);
			Assert.AreEqual(h.IsEmpty, true);
		}

		[Test]
		public void TestGetrOOT()
		{
			Heap<int> h = new Heap<int>(HeapType.MaxHeap);
			h.Add(5);
			Assert.AreEqual(h.Root, 5);
			Assert.AreEqual(h.Count, 1);
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void TestInvalidGetrOOT()
		{
			Heap<int> h = new Heap<int>(HeapType.MaxHeap);
			int i = h.Root;
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestNullComparer1()
		{
			Heap<int> h = new Heap<int>(HeapType.MaxHeap, null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestNullComparer2()
		{
			Heap<int> h = new Heap<int>(HeapType.MaxHeap, 50, null);
		}

		[Test]
		[ExpectedException(typeof(NotSupportedException))]
		public void TestInterfaceRemove()
		{
			ICollection<int> h = GetTestHeap();
			h.Remove(4);
		}

		[Test]
		public void TestIsFixedSize()
		{
			Heap<int> h = new Heap<int>(HeapType.MaxHeap);
			Assert.AreEqual(h.IsFixedSize, false);

			h = GetTestHeap();
			Assert.AreEqual(h.IsFixedSize, false);
		}

		[Test]
		public void TestIsReadOnly()
		{
			Heap<int> h = new Heap<int>(HeapType.MaxHeap);
			Assert.AreEqual(h.IsReadOnly, false);

			h = GetTestHeap();
			Assert.AreEqual(h.IsReadOnly, false);
		}

		[Test]
		public void TestInterfaceEnumerator()
		{
			Heap<int> h = new Heap<int>(HeapType.MaxHeap);

			int max = 100;

			for (int i = 0; i < max; i++)
			{
				h.Add(i);
			}

			bool[] isPresent = new bool[max];

			for (int i = 0; i < isPresent.Length; i++)
			{
				isPresent[i] = false;
			}

			System.Collections.IEnumerator enumerator = ((System.Collections.IEnumerable)h).GetEnumerator();

			Assert.AreEqual(enumerator.MoveNext(), true);

			do
			{
				isPresent[(int)enumerator.Current] = true;
			}
			while (enumerator.MoveNext());

			for (int i = 0; i < max; i++)
			{
				Assert.AreEqual(isPresent[i], true);
			}
		}

		[Test]
		public void TestRemoveRoot()
		{
			Heap<int> h = new Heap<int>(HeapType.MaxHeap);
			h.Add(5);
			Assert.AreEqual(h.Root, 5);
			Assert.AreEqual(h.Count, 1);
			Assert.AreEqual(h.IsEmpty, false);

			Assert.AreEqual(h.RemoveRoot(), 5);
			Assert.AreEqual(h.Count, 0);
			Assert.AreEqual(h.IsEmpty, true);
		}
		
		[Test]
		public void TestAdd()
		{
			Heap<int> h = new Heap<int>(HeapType.MaxHeap);
			h.Add(5);

			Assert.AreEqual(h.Count, 1);
			Assert.AreEqual(h.IsEmpty, false);
			Assert.AreEqual(h.IsFull, false);
			Assert.AreEqual(h.Root, 5);

			h.Add(2);
			Assert.AreEqual(h.Count, 2);
			Assert.AreEqual(h.IsEmpty, false);
			Assert.AreEqual(h.IsFull, false);
			Assert.AreEqual(h.Root, 5);

			h.Add(3);
			Assert.AreEqual(h.Count, 3);
			Assert.AreEqual(h.IsEmpty, false);
			Assert.AreEqual(h.IsFull, false);
			Assert.AreEqual(h.Root, 5);

			Assert.AreEqual(h.RemoveRoot(), 5);

			h.Add(1);
			Assert.AreEqual(h.Count, 3);
			Assert.AreEqual(h.IsEmpty, false);
			Assert.AreEqual(h.IsFull, false);
			Assert.AreEqual(h.Root, 3);
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void TestInvalidRemoveSmallest()
		{
			Heap<int> h = new Heap<int>(HeapType.MaxHeap);

			Assert.AreEqual(h.Count, 0);

			h.RemoveRoot();
		}

		[Test]
		public void StressTestHeap()
		{
			Heap<int> h = new Heap<int>(HeapType.MaxHeap);

			int max = 5000;

			for (int i = 1; i <= max; i++) 
			{
				h.Add(i);

				Assert.AreEqual(h.Root, i);
			}

			for (int i = max; i > 0; i--)
			{
				Assert.AreEqual(h.RemoveRoot(), i);
			}
		}

		[Test]
		public void TestClear()
		{
			Heap<int> h = new Heap<int>(HeapType.MaxHeap);

			for (int i = 1; i <= 20; i++)
			{
				h.Add(i);
				Assert.AreEqual(h.Root, i);
			}

			Assert.AreEqual(h.IsFull, false);
			Assert.AreEqual(h.IsEmpty, false);
			Assert.AreEqual(h.Count, 20);

			h.Clear();

			Assert.AreEqual(h.Count, 0);
			Assert.AreEqual(h.IsFull, false);
			Assert.AreEqual(h.IsEmpty, true);
		}

		[Test]
		public void TestCompareTo()
		{
			Heap<int> h1 = new Heap<int>(HeapType.MaxHeap);

			for (int i = 0; i < 5; i++)
			{
				h1.Add(i);
			}

			Heap<int> h2 = new Heap<int>(HeapType.MaxHeap);

			for (int i = 0; i < 10; i++)
			{
				h2.Add(i);
			}

			Assert.AreEqual(h1.CompareTo(h2), -1);
			Assert.AreEqual(h2.CompareTo(h1), 1);
			Assert.AreEqual(h1.CompareTo(h1), 0);

			Assert.AreEqual(h1.CompareTo(new object()), -1);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestInvalidCompareTo()
		{
			Heap<int> h = new Heap<int>(HeapType.MaxHeap);
			h.CompareTo(null);
		}

		[Test]
		public void TestEnumerator()
		{
			Heap<int> h = new Heap<int>(HeapType.MaxHeap);

			int max = 100;

			for (int i = 0; i < max; i++)
			{
				h.Add(i);
			}

			bool[] isPresent = new bool[max];

			for (int i = 0; i < isPresent.Length; i++)
			{
				isPresent[i] = false;
			}

			IEnumerator<int> enumerator = h.GetEnumerator();

			Assert.AreEqual(enumerator.MoveNext(), true);

			do
			{
				isPresent[enumerator.Current] = true;
			}
			while (enumerator.MoveNext());

			for (int i = 0; i < max; i++)
			{
				Assert.AreEqual(isPresent[i], true);
			}
		}

		[Test]
		public void TestAccept()
		{
			TrackingVisitor<int> visitor = new TrackingVisitor<int>();
			Heap<int> h = GetTestHeap();
			
			h.Accept(visitor);

			Assert.AreEqual(visitor.TrackingList.Count, h.Count);
			Assert.AreEqual(visitor.TrackingList.Contains(5), true);
			Assert.AreEqual(visitor.TrackingList.Contains(4), true);
			Assert.AreEqual(visitor.TrackingList.Contains(99), true);
			Assert.AreEqual(visitor.TrackingList.Contains(12), true);
			Assert.AreEqual(visitor.TrackingList.Contains(5), true);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestNullVisitor()
		{
			Heap<int> h = GetTestHeap();
			h.Accept(null);
		}

		[Test]
		public void TestContains()
		{
			Heap<int> h = GetTestHeap();

			Assert.AreEqual(h.Contains(5), true);
			Assert.AreEqual(h.Contains(4), true);
			Assert.AreEqual(h.Contains(99), true);
			Assert.AreEqual(h.Contains(12), true);
			Assert.AreEqual(h.Contains(5), true);
			Assert.AreEqual(h.Contains(3), false);
		}

		[Test]
		public void TestCopyTo()
		{
			Heap<int> h = GetTestHeap();
			int[] array = new int[h.Count];

			h.CopyTo(array, 0);

			List<int> l = new List<int>(array);
			Assert.AreEqual(l.Count, h.Count);
			Assert.AreEqual(l.Contains(5), true);
			Assert.AreEqual(l.Contains(4), true);
			Assert.AreEqual(l.Contains(99), true);
			Assert.AreEqual(l.Contains(12), true);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestNullCopyTo()
		{
			Heap<int> h = GetTestHeap();
			h.CopyTo(null, 0);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestInvalidCopyTo1()
		{
			Heap<int> h = GetTestHeap();
			int[] array = new int[h.Count - 1];
			h.CopyTo(array, 0);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestInvalidCopyTo2()
		{
			Heap<int> h = GetTestHeap();
			int[] array = new int[h.Count];
			h.CopyTo(array, 1);
		}

		private Heap<int> GetTestHeap()
		{
			Heap<int> h = new Heap<int>(HeapType.MaxHeap);

			h.Add(5);
			h.Add(4);
			h.Add(99);
			h.Add(12);
			h.Add(5);
			return h;
		}
	}
}
