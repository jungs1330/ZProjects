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

namespace NGenericsTests
{
	using System;
	using System.Text;
	using NUnit.Framework;
	using NGenerics.DataStructures;
	using NGenerics.Visitors;

	namespace DataStructures.Tests
	{
		[TestFixture]
		public class QueueTest
		{
			[Test]
			public void TestSuccesfulInit()
			{
				VisitableQueue<int> q = new VisitableQueue<int>();

				Assert.AreEqual(q.Count, 0);
				Assert.AreEqual(q.IsEmpty, true);
				Assert.AreEqual(q.IsFull, false);

				VisitableStack<int> stack = new VisitableStack<int>();

				for (int i = 0; i < 3; i++)
				{
					stack.Push(i * 4);
				}

				q = new VisitableQueue<int>(stack);

				for (int i = 2; i >= 0; i--)
				{
					Assert.AreEqual(q.Dequeue(), i * 4);
				}

			}

			[Test]
			public void TestFixedSize()
			{
				VisitableQueue<int> q = new VisitableQueue<int>();
				Assert.AreEqual(q.IsFixedSize, false);

				q = GetTestQueue();
				Assert.AreEqual(q.IsFixedSize, false);
			}

			[Test]
			[ExpectedException(typeof(ArgumentNullException))]
			public void TestNullVisitor()
			{
				VisitableQueue<int> q = new VisitableQueue<int>(); ;
				q.Accept(null);
			}

			[Test]
			[ExpectedException(typeof(ArgumentOutOfRangeException))]
			public void TestUnsuccesfulInit()
			{
				VisitableQueue<int> q = new VisitableQueue<int>(-5);
			}

			[Test]
			public void TestVisitor()
			{
				VisitableQueue<int> q = GetTestQueue();
				SumVisitor visitor = new SumVisitor();

				q.Accept(visitor);

				Assert.AreEqual(visitor.Sum, 0 + 3 + 6 + 9 + 12);
			}

			[Test]
			public void TestStoppingVisitor()
			{
				VisitableQueue<int> q = GetTestQueue();

				ComparableFindingVisitor<int> visitor = new ComparableFindingVisitor<int>(6);
				q.Accept(visitor);

				Assert.AreEqual(visitor.Found, true);

				visitor = new ComparableFindingVisitor<int>(99);
				q.Accept(visitor);
				Assert.AreEqual(visitor.Found, false);
			}

			[Test]
			public void TestCompareTo()
			{
				VisitableQueue<int>[] qs = new VisitableQueue<int>[3];

				for (int i = 0; i < 3; i++)
				{
					qs[i] = new VisitableQueue<int>();

					for (int j = 0; j < i; j++)
					{
						qs[i].Enqueue(j * 4);
					}
				}

				Assert.AreEqual(qs[0].CompareTo(qs[1]), -1);
				Assert.AreEqual(qs[2].CompareTo(qs[1]), 1);

				VisitableQueue<int> sameQ = qs[1];
				Assert.AreEqual(qs[1].CompareTo(sameQ), 0);


				object obj = new object();

				Assert.AreEqual(qs[0].CompareTo(obj), -1);
			}

			[Test]
			[ExpectedException(typeof(ArgumentNullException))]
			public void TestInvalidCompareTo()
			{
				VisitableQueue<int> q = new VisitableQueue<int>();
				q.CompareTo(null);
			}

			[Test]
			public void TestIsFull()
			{
				// IsFull must always return false.
				VisitableQueue<int> q = GetTestQueue();
				Assert.AreEqual(q.IsFull, false);

				q = new VisitableQueue<int>();
				Assert.AreEqual(q.IsFull, false);
			}

			[Test]
			public void TestEnumerator()
			{
				VisitableQueue<int> q = GetTestQueue();

				IEnumerator<int> enumerator = q.GetEnumerator();

				for (int i = 0; i < 5; i++)
				{
					Assert.AreEqual(enumerator.MoveNext(), true);
					Assert.AreEqual(enumerator.Current, i * 3);
				}

				Assert.AreEqual(enumerator.MoveNext(), false);
			}

			[Test]
			public void TestAdd()
			{
				VisitableQueue<int> q = new VisitableQueue<int>();
				q.Add(5);

				Assert.AreEqual(q.Count, 1);
				Assert.AreEqual(q.Peek(), 5);
				Assert.AreEqual(q.Dequeue(), 5);

				q.Enqueue(3);
				q.Add(4);

				Assert.AreEqual(q.Count, 2);
				Assert.AreEqual(q.Dequeue(), 3);
				Assert.AreEqual(q.Dequeue(), 4);
			}

			[Test]
			public void TestReadOnly()
			{
				VisitableQueue<int> q = new VisitableQueue<int>();
				Assert.AreEqual(q.IsReadOnly, false);

				q.Add(5);
				Assert.AreEqual(q.IsReadOnly, false);
			}


			[Test]
			public void TestCompare()
			{
				VisitableQueue<int> q1 = new VisitableQueue<int>(6);
				VisitableQueue<int> q2 = new VisitableQueue<int>(4);

				VisitableQueue<int> q3 = new VisitableQueue<int>(8);
				q3.Enqueue(2);

				VisitableQueue<int> q4 = new VisitableQueue<int>(6);

				object o = new object();

				Assert.AreEqual(q1.CompareTo(q2), 0);
				Assert.AreEqual(q1.CompareTo(q3), -1);
				Assert.AreEqual(q3.CompareTo(q4), 1);
				Assert.AreEqual(q1.CompareTo(o), -1);
			}

			private VisitableQueue<int> GetTestQueue()
			{
				VisitableQueue<int> q = new VisitableQueue<int>(5);

				for (int i = 0; i < 5; i++)
				{
					q.Enqueue(i * 3);
				}

				return q;
			}
		}
	}

}
