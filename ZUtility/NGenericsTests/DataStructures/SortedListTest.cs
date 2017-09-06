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
using NGenerics.Visitors;
using NGenerics.Comparers;
using System.Collections;

namespace NGenericsTests
{
	[TestFixture]
	public class SortedListTest
	{		
		[Test]
		public void TestSuccesfulInit()
		{
			SortedList<int> sortedList = new SortedList<int>();

			List<int> list = new List<int>();
			
			for (int i = 0; i < 200; i++)
			{
				list.Add(i);
			}

			sortedList = new SortedList<int>(list);

			for (int i = 0; i < 200; i++)
			{
				Assert.AreEqual(sortedList[i], i);
			}

			sortedList = new SortedList<int>(50);

			for (int i = 0; i < 200; i++)
			{
				sortedList.Add(i * 2);
			}

			for (int i = 0; i < 200; i++)
			{
				Assert.AreEqual(sortedList[i], i * 2);
			}

			sortedList = new SortedList<int>(50, Comparer<int>.Default);
			Assert.AreEqual(sortedList.Comparer, Comparer<int>.Default);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestInvalidIndex1()
		{
			SortedList<int> l = new SortedList<int>();
			int i = l[0];
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestInvalidIndex2()
		{
			SortedList<int> l = GetTestList();
			int i = l[l.Count];
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestInvalidIndex3()
		{
			SortedList<int> l = GetTestList();
			int i = l[-1];
		}

		[Test]
		public void TestIndexOf()
		{
			SortedList<int> l = new SortedList<int>();
			Assert.Less(l.IndexOf(2), 0);

			l.Add(5);
			Assert.AreEqual(l.IndexOf(5), 0);

			l.Add(2);
			Assert.AreEqual(l.IndexOf(5), 1);
			Assert.AreEqual(l.IndexOf(2), 0);

			Assert.Less(l.IndexOf(10), 0);
		}
		
		[Test]
		public void TestRemoveAt()
		{
			SortedList<int> l = new SortedList<int>();

			l.Add(5);

			Assert.AreEqual(l.Count, 1);

			l.RemoveAt(0);

			Assert.AreEqual(l.Count, 0);

			l.Add(5);
			l.Add(2);

			Assert.AreEqual(l.Count, 2);
			l.RemoveAt(1);

			Assert.AreEqual(l.Count, 1);

			l.Add(2);

			Assert.AreEqual(l.Count, 2);
			l.RemoveAt(0);

			Assert.AreEqual(l.Count, 1);
		}

		[Test]
		[ExpectedException(typeof(NotSupportedException))]
		public void TestInsert()
		{
			IList<int> l = new SortedList<int>();
			l.Insert(5, 2);
		}

		[Test]
		public void TestInterfaceIndexerGet()
		{
			IList<int> l = new SortedList<int>();
			l.Add(5);
			l.Add(2);

			Assert.AreEqual(l[0], 2);
			Assert.AreEqual(l[1], 5);
		}

		[Test]
		[ExpectedException(typeof(NotSupportedException))]
		public void TestInterfaceIndexerSet()
		{
			IList<int> l = new SortedList<int>();
			l.Add(2);
			
			l[0] = 5;
		}

		[Test]
		public void TestSorting()
		{
			SortedList<int> sortedList = new SortedList<int>();

			for (int i = 200; i >= 0; i--)
			{
				sortedList.Add(i * 2);
			}

			for (int i = 0; i <= 200; i++)
			{
				Assert.AreEqual(sortedList[i], i * 2);
			}
		}

		[Test]
		public void TestClear()
		{
			SortedList<int> l = GetTestList();
			Assert.AreEqual(l.Count, 51);

			l.Clear();
			Assert.AreEqual(l.Count, 0);
		}

		[Test]
		public void TestAdd()
		{
			SortedList<int> l = new SortedList<int>();
			l.Add(5);

			Assert.AreEqual(l.Count, 1);
			Assert.AreEqual(l.IsEmpty, false);
			Assert.AreEqual(l[0], 5);

			l = GetTestList();
			l.Add(-5);
			Assert.AreEqual(l[0], -5);
		}

		[Test]
		public void StressTestList()
		{
			SortedList<int> l = new SortedList<int>();

			for (int i = 1000; i > 0; i--)
			{
				l.Add(i);
			}

			for (int i = 1000; i > 0; i--)
			{
				l.Add(i);
			}

			int counter = 0;
			int val = 1;

			while (counter <= 1000)
			{
				Assert.AreEqual(l[counter], val);
				counter++;

				Assert.AreEqual(l[counter], val);
				counter++;
				val++;
			}
		}

		[Test]
		public void TestRemove()
		{
			SortedList<int> l = new SortedList<int>();

			for (int i = 50; i >= 0; i--)
			{
				l.Add(i * 2);
			}

			Assert.AreEqual(l.Count, 51);
			
			Assert.AreEqual(l.Remove(50), true);
			Assert.AreEqual(l.Count, 50);

			Assert.AreEqual(l.Remove(3), false);
			Assert.AreEqual(l.Count, 50);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestInvalidIndex()
		{
			SortedList<int> l = new SortedList<int>();
			l.Add(5);

			int test = l[1];
		}

		[Test]
		public void TestCompareTo()
		{
			SortedList<int> l1 = new SortedList<int>();

			for (int i = 0; i< 20; i++) {
				l1.Add(i);
			}

			SortedList<int> l2 = new SortedList<int>();

			for (int i = 0; i < 40; i++)
			{
				l2.Add(i);
			}

			Assert.AreEqual(l1.CompareTo(l2), -1);
			Assert.AreEqual(l2.CompareTo(l1), 1);
			Assert.AreEqual(l2.CompareTo(l2), 0);

			object o = new object();

			Assert.AreEqual(l1.CompareTo(o), -1);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestInvalidCompareTo()
		{
			SortedList<string> s = new SortedList<string>();
			s.CompareTo(null);
		}

		[Test]
		public void TestContains()
		{
			SortedList<int> l = new SortedList<int>();

			for (int i = 0; i < 20; i++)
			{
				l.Add(i);
			}

			Assert.AreEqual(l.Contains(5), true);
			Assert.AreEqual(l.Contains(50), false);
		}

		[Test]
		public void TestEnumerator()
		{
			SortedList<int> l = new SortedList<int>();

			for (int i = 0; i < 20; i++)
			{
				l.Add(i);
			}

			int counter = 0;
			IEnumerator<int> enumerator = l.GetEnumerator();

			while (enumerator.MoveNext())
			{
				Assert.AreEqual(enumerator.Current, counter);
				counter++;
			}

			Assert.AreEqual(counter, 20);
		}

		[Test]
		public void TestInterfaceEnumerator()
		{
			SortedList<int> l = new SortedList<int>();

			for (int i = 0; i < 20; i++)
			{
				l.Add(i);
			}

			int counter = 0;
			
			IEnumerator enumerator = ((IEnumerable)l).GetEnumerator();

			while (enumerator.MoveNext())
			{
				Assert.AreEqual((int)enumerator.Current, counter);
				counter++;
			}

			Assert.AreEqual(counter, 20);
		}

		[Test]
		public void TestIsFixedSize()
		{
			SortedList<int> l = new SortedList<int>();
			Assert.AreEqual(l.IsFixedSize, false);

			l = GetTestList();
			Assert.AreEqual(l.IsFixedSize, false);
		}

		[Test]
		public void TestIsFull()
		{
			SortedList<int> l = new SortedList<int>();
			Assert.AreEqual(l.IsFull, false);

			l = GetTestList();
			Assert.AreEqual(l.IsFull, false);
		}

		[Test]
		public void TestIsReadOnly()
		{
			SortedList<int> l = new SortedList<int>();
			Assert.AreEqual(l.IsReadOnly, false);

			l = GetTestList();
			Assert.AreEqual(l.IsReadOnly, false);
		}

		[Test]
		public void TestAccept()
		{
			TrackingVisitor<int> visitor = new TrackingVisitor<int>();
			SortedList<int> s = GetTestList();
			s.Accept(visitor);

			Assert.AreEqual(visitor.TrackingList.Count, s.Count);

			for (int i = 0; i <= 50; i++)
			{
				Assert.AreEqual(visitor.TrackingList.Contains(i * 2), true);
			}
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestNullVisitor()
		{
			SortedList<int> list = GetTestList();
			list.Accept(null);
		}

		[Test]
		public void TestComparer()
		{
			SortedList<int> list = new SortedList<int>(new ReverseComparer<int>());
			Assert.AreEqual(list.Comparer.GetType() == typeof(ReverseComparer<int>), true);
		}

		[Test]
		public void TestCopyTo()
		{
			SortedList<int> s = GetTestList();
			int[] array = new int[s.Count];
			s.CopyTo(array, 0);

			List<int> l = new List<int>(array);

			for (int i = 0; i <= 50; i++)
			{
				Assert.AreEqual(l.Contains(i * 2), true);
			}
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestNullCopyTo()
		{
			SortedList<int> s = GetTestList();
			s.CopyTo(null, 0);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestInvalidCopyTo1()
		{
			SortedList<int> s = GetTestList();
			int[] array = new int[s.Count - 1];
			s.CopyTo(array, 0);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestInvalidCopyTo2()
		{
			SortedList<int> s = GetTestList();
			int[] array = new int[s.Count];
			s.CopyTo(array, 1);
		}

		private SortedList<int> GetTestList()
		{
			SortedList<int> sortedList = new SortedList<int>();

			for (int i = 50; i >= 0; i--)
			{
				sortedList.Add(i * 2);
			}

			return sortedList;
		}
	}
}
