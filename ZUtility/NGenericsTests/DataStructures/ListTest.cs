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

namespace NGenericsTests
{
	[TestFixture]
	public class ListTest
	{
		[Test]
		public void TestSuccesfulInit()
		{
			VisitableList<int> l = new VisitableList<int>();
			l = new VisitableList<int>(5);

			VisitableQueue<int> q = new VisitableQueue<int>();

			for (int i = 0; i < 3; i++)
			{
				q.Enqueue(i * 2);
			}

			l = new VisitableList<int>(q);

			for (int i = 0; i < 3; i++)
			{
				Assert.AreEqual(l[i], i * 2);
			}
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestNullVisitor()
		{
			VisitableList<int> l = new VisitableList<int>(); ;
			l.Accept(null);
		}

		[Test]
		public void TestCompareTo()
		{
			VisitableList<int>[] lists = new VisitableList<int>[3];

			for (int i = 0; i < 3; i++)
			{
				lists[i] = new VisitableList<int>();

				for (int j = 0; j < i; j++)
				{
					lists[i].Add(j * 4);
				}
			}

			Assert.AreEqual(lists[0].CompareTo(lists[1]), -1);
			Assert.AreEqual(lists[2].CompareTo(lists[1]), 1);

			VisitableList<int> sameList = lists[1];
			Assert.AreEqual(lists[1].CompareTo(sameList), 0);

			object obj = new object();

			Assert.AreEqual(lists[0].CompareTo(obj), -1);

			VisitableHashtable<int, int> h = new VisitableHashtable<int, int>();

			Assert.AreEqual(lists[0].CompareTo(h), 1);

		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestInvalidCompareTo()
		{
			VisitableList<int> l = new VisitableList<int>();
			l.CompareTo(null);
		}

		[Test]
		public void TestVisitor()
		{
			VisitableList<int> l = GetTestList();
			SumVisitor visitor = new SumVisitor();

			l.Accept(visitor);

			Assert.AreEqual(visitor.Sum, 0 + 3 + 6 + 9 + 12);
		}

		[Test]
		public void TestStoppingVisitor()
		{
			VisitableList<int> l = GetTestList();

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
			VisitableList<int> l = GetTestList();
			Assert.AreEqual(l.IsFull, false);

			l = new VisitableList<int>();
			Assert.AreEqual(l.IsFull, false);
		}

		[Test]
		public void TestIsEmpty()
		{

			VisitableList<int> l = GetTestList();
			Assert.AreEqual(l.IsEmpty, false);

			l.Clear();
			Assert.AreEqual(l.IsEmpty, true);

			l = new VisitableList<int>();
			Assert.AreEqual(l.IsEmpty, true);
		}


		[Test]
		public void TestFixedSize()
		{
			VisitableList<int> l = new VisitableList<int>();
			Assert.AreEqual(l.IsFixedSize, false);

			l = GetTestList();
			Assert.AreEqual(l.IsFixedSize, false);
		}

		private VisitableList<int> GetTestList()
		{
			VisitableList<int> l = new VisitableList<int>(5);

			for (int i = 0; i < 5; i++)
			{
				l.Add(i * 3);
			}

			return l;
		}
	}
}
