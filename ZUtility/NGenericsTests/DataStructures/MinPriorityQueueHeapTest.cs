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
using NGenerics.Comparers;
using NGenerics.Visitors;
using System.Collections;

using NGenerics.Enumerations;

namespace NGenericsTests
{
	[TestFixture]
	public class MinPriorityQueueTest
	{
		[Test]
		public void TestSuccesfulInit()
		{
			PriorityQueue<string> q = new PriorityQueue<string>(PriorityQueueType.MinPriorityQueue);
			
			Assert.AreEqual(q.Count, 0);
			Assert.AreEqual(q.IsEmpty, true);
			Assert.AreEqual(q.IsFull, false);

			q = new PriorityQueue<string>(PriorityQueueType.MinPriorityQueue, 50);
			
			Assert.AreEqual(q.Count, 0);
			Assert.AreEqual(q.IsEmpty, true);
			Assert.AreEqual(q.IsFull, false);
		}

		[Test]
		public void TestReverseSequencing()
		{
			PriorityQueue<string> q = this.GetTestPriorityQueue();

			// u or a
			string str = q.Dequeue();
			Assert.AreEqual(((str == "u") || (str == "a")), true);

			str = q.Dequeue();
			Assert.AreEqual(((str == "u") || (str == "a")), true);

			// v or b
			str = q.Dequeue();
			Assert.AreEqual(((str == "v") || (str == "b")), true);

			str = q.Dequeue();
			Assert.AreEqual(((str == "v") || (str == "b")), true);

			// w or c
			str = q.Dequeue();
			Assert.AreEqual(((str == "w") || (str == "c")), true);

			str = q.Dequeue();
			Assert.AreEqual(((str == "w") || (str == "c")), true);

			// x or d
			str = q.Dequeue();
			Assert.AreEqual(((str == "x") || (str == "d")), true);

			str = q.Dequeue();
			Assert.AreEqual(((str == "x") || (str == "d")), true);


			// y or e 
			str = q.Dequeue();
			Assert.AreEqual(((str == "y") || (str == "e")), true);

			str = q.Dequeue();
			Assert.AreEqual(((str == "y") || (str == "e")), true);

			// z or f
			str = q.Dequeue();
			Assert.AreEqual(((str == "z") || (str == "f")), true);

			str = q.Dequeue();
			Assert.AreEqual(((str == "z") || (str == "f")), true);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestNullVisitor()
		{
			PriorityQueue<string> q = GetTestPriorityQueue();
			q.Accept(null);
		}

		[Test]
		public void TestCompareTo()
		{
			PriorityQueue<string> q1 = new PriorityQueue<string>(PriorityQueueType.MinPriorityQueue);
			PriorityQueue<string> q2 = GetTestPriorityQueue();

			Assert.AreEqual(q1.CompareTo(q2), -1);
			Assert.AreEqual(q2.CompareTo(q1), 1);
			Assert.AreEqual(q1.CompareTo(q1), 0);
			Assert.AreEqual(q2.CompareTo(q2), 0);

			Assert.AreEqual(q1.CompareTo(new object()), -1);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestInvalidCompareTo()
		{
			PriorityQueue<int> p = new PriorityQueue<int>(PriorityQueueType.MinPriorityQueue);
			p.CompareTo(null);
		}

		[Test]
		public void TestAccept()
		{
			bool a = false;
			bool b = false;
			bool c = false;
			bool d = false;
			bool e = false;
			bool f = false;

			int count = 0;
			
			PriorityQueue<string> q = GetSimpleTestPriorityQueue();
			TrackingVisitor<string> visitor = new TrackingVisitor<string>();

			q.Accept(visitor);

			for (int i = 0; i < visitor.TrackingList.Count; i++)
			{
				count++;

				if (visitor.TrackingList[i] == "a")
				{
					a = true;
				}
				else if (visitor.TrackingList[i] == "b")
				{
					b = true;
				}
				else if (visitor.TrackingList[i] == "c")
				{
					c = true;
				}
				else if (visitor.TrackingList[i] == "d")
				{
					d = true;
				}
				else if (visitor.TrackingList[i] == "e")
				{
					e = true;
				}
				else if (visitor.TrackingList[i] == "f")
				{
					f = true;
				}
			}

			Assert.AreEqual(a, true);
			Assert.AreEqual(b, true);
			Assert.AreEqual(c, true);
			Assert.AreEqual(d, true);
			Assert.AreEqual(e, true);
			Assert.AreEqual(f, true);

			Assert.AreEqual(count, 6);
		}

		[Test]
		public void TestIsEmpty()
		{
			PriorityQueue<int> q = new PriorityQueue<int>(PriorityQueueType.MinPriorityQueue);
			Assert.AreEqual(q.IsEmpty, true);

			q.Add(4);
			Assert.AreEqual(q.IsEmpty, false);

			q.Add(99);
			Assert.AreEqual(q.IsEmpty, false);

			q.Clear();
			Assert.AreEqual(q.IsEmpty, true);
		}

		[Test]
		public void TestIsReadOnly()
		{
			PriorityQueue<int> q = new PriorityQueue<int>(PriorityQueueType.MinPriorityQueue);
			Assert.AreEqual(q.IsReadOnly, false);

			q.Add(4);
			Assert.AreEqual(q.IsReadOnly, false);

			q.Add(99);
			Assert.AreEqual(q.IsReadOnly, false);

			q.Clear();
			Assert.AreEqual(q.IsReadOnly, false);
		}

		[Test]
		public void TestCount()
		{
			PriorityQueue<int> q = new PriorityQueue<int>(PriorityQueueType.MinPriorityQueue);
			Assert.AreEqual(q.Count, 0);

			q.Add(4);
			Assert.AreEqual(q.Count, 1);

			q.Add(99);
			Assert.AreEqual(q.Count, 2);

			q.Clear();
			Assert.AreEqual(q.Count, 0);
		}

		[Test]
		[ExpectedException(typeof(NotSupportedException))]
		public void TestInterfaceRemove1()
		{
			ICollection<int> q = new PriorityQueue<int>(PriorityQueueType.MinPriorityQueue);
			q.Remove(5);
		}

		[Test]
		[ExpectedException(typeof(NotSupportedException))]
		public void TestInterfaceRemove2()
		{
			ICollection<string> q = GetTestPriorityQueue();
			q.Remove("a");
		}

		[Test]
		public void TestIsFixedSize()
		{
			PriorityQueue<string> q = new PriorityQueue<string>(PriorityQueueType.MinPriorityQueue);
			Assert.AreEqual(q.IsFixedSize, false);

			q = GetTestPriorityQueue();
			Assert.AreEqual(q.IsFixedSize, false);
		}

		[Test]
		public void TestAdd()
		{
			PriorityQueue<int> q = new PriorityQueue<int>(PriorityQueueType.MinPriorityQueue);
			q.Add(4);

			Assert.AreEqual(q.Count, 1);
			Assert.AreEqual(q.Dequeue(), 4);

			q.Add(5);
			q.Add(6, 2);

			Assert.AreEqual(q.Dequeue(), 5);
			Assert.AreEqual(q.Dequeue(), 6);

			q.Add(6, 2);
			q.Add(5);
			
			Assert.AreEqual(q.Dequeue(), 5);
			Assert.AreEqual(q.Dequeue(), 6);


			PriorityQueue<string> q2 = new PriorityQueue<string>(PriorityQueueType.MinPriorityQueue);

			q2.Add("a", 1);
			q2.Add("b", 2);
			q2.Add("c", 3);
			q2.Add("d", 4);
			q2.Add("e", 5);
			q2.Add("f", 6);

			q2.Add("z", 6);
			q2.Add("y", 5);
			q2.Add("x", 4);
			q2.Add("w", 3);
			q2.Add("v", 2);
			q2.Add("u", 1);

			q2.Add("z", 1);
			q2.Add("y", 2);
			q2.Add("x", 3);
			q2.Add("w", 4);
			q2.Add("v", 5);
			q2.Add("u", 6);

			Assert.AreEqual(q2.Count, 18);

			q2.Clear();

			Assert.AreEqual(q2.Count, 0);
		}

		[Test]
		public void TestEnqueue()
		{
			PriorityQueue<int> q = new PriorityQueue<int>(PriorityQueueType.MinPriorityQueue);
			q.Enqueue(4);

			Assert.AreEqual(q.Count, 1);
			Assert.AreEqual(q.Dequeue(), 4);

			q.Enqueue(5);
			q.Enqueue(6, 2);

			Assert.AreEqual(q.Dequeue(), 5);
			Assert.AreEqual(q.Dequeue(), 6);

			q.Enqueue(6, 2);
			q.Enqueue(5);

			Assert.AreEqual(q.Dequeue(), 5);
			Assert.AreEqual(q.Dequeue(), 6);

			PriorityQueue<string> q2 = new PriorityQueue<string>(PriorityQueueType.MinPriorityQueue);

			q2.Enqueue("a", 1);
			q2.Enqueue("b", 2);
			q2.Enqueue("c", 3);
			q2.Enqueue("d", 4);
			q2.Enqueue("e", 5);
			q2.Enqueue("f", 6);

			q2.Enqueue("z", 6);
			q2.Enqueue("y", 5);
			q2.Enqueue("x", 4);
			q2.Enqueue("w", 3);
			q2.Enqueue("v", 2);
			q2.Enqueue("u", 1);

			q2.Enqueue("z", 1);
			q2.Enqueue("y", 2);
			q2.Enqueue("x", 3);
			q2.Enqueue("w", 4);
			q2.Enqueue("v", 5);
			q2.Enqueue("u", 6);

			Assert.AreEqual(q2.Count, 18);

			q2.Clear();

			Assert.AreEqual(q2.Count, 0);
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void TestEmptyDequeue()
		{
			PriorityQueue<string> q = new PriorityQueue<string>(PriorityQueueType.MinPriorityQueue);
			q.Dequeue();
		}

		[Test]
		public void TestDecreasePriority()
		{
			PriorityQueue<string> q = GetSimpleTestPriorityQueue();
			q.DecreaseItemPriority(1);

			q.Enqueue("g", 6);
			Assert.AreEqual(q.Dequeue(), "a");

			q = GetSimpleTestPriorityQueue();
			q.DecreaseItemPriority(2);

			q.Enqueue("g", 5);
			Assert.AreEqual(q.Dequeue(), "a");
		}

		[Test]
		public void TestIncreasePriority()
		{
			PriorityQueue<string> q = GetSimpleTestPriorityQueue();
			q.IncreaseItemPriority(1);

			q.Enqueue("g", 6);
			Assert.AreNotEqual(q.Dequeue(), "g");

			q = GetSimpleTestPriorityQueue();
			q.IncreaseItemPriority(2);

			q.Enqueue("g", 7);
			Assert.AreNotEqual(q.Dequeue(), "g");
		}

		[Test]
		public void TestEnumerator()
		{
			int count = 0;
			bool a = false;
			bool b = false;
			bool c = false;
			bool d = false;
			bool e = false;
			bool f = false;

			PriorityQueue<string> q = GetSimpleTestPriorityQueue();
			IEnumerator<string> enumerator = q.GetEnumerator();

			while (enumerator.MoveNext())
			{
				count++;

				if (enumerator.Current == "a")
				{
					a = true;
				}
				else if (enumerator.Current == "b")
				{
					b = true;
				}
				else if (enumerator.Current == "c")
				{
					c = true;
				}
				else if (enumerator.Current == "d")
				{
					d = true;
				}
				else if (enumerator.Current == "e")
				{
					e = true;
				}
				else if (enumerator.Current == "f")
				{
					f = true;
				}
			}

			Assert.AreEqual(a, true);
			Assert.AreEqual(b, true);
			Assert.AreEqual(c, true);
			Assert.AreEqual(d, true);
			Assert.AreEqual(e, true);
			Assert.AreEqual(f, true);
			Assert.AreEqual(count, 6);
		}

		[Test]
		public void TestInterfaceEnumerator()
		{
			int count = 0;
			bool a = false;
			bool b = false;
			bool c = false;
			bool d = false;
			bool e = false;
			bool f = false;

			PriorityQueue<string> q = GetSimpleTestPriorityQueue();
			IEnumerator enumerator = ((IEnumerable) q).GetEnumerator();

			while (enumerator.MoveNext())
			{
				count++;

				if ((string) enumerator.Current == "a")
				{
					a = true;
				}
				else if ((string)enumerator.Current == "b")
				{
					b = true;
				}
				else if ((string)enumerator.Current == "c")
				{
					c = true;
				}
				else if ((string)enumerator.Current == "d")
				{
					d = true;
				}
				else if ((string)enumerator.Current == "e")
				{
					e = true;
				}
				else if ((string)enumerator.Current == "f")
				{
					f = true;
				}
			}

			Assert.AreEqual(a, true);
			Assert.AreEqual(b, true);
			Assert.AreEqual(c, true);
			Assert.AreEqual(d, true);
			Assert.AreEqual(e, true);
			Assert.AreEqual(f, true);
			Assert.AreEqual(count, 6);
		}

		[Test]
		public void TestPeek()
		{
			PriorityQueue<string> q = new PriorityQueue<string>(PriorityQueueType.MinPriorityQueue);

			q.Enqueue("g", 6);
			Assert.AreEqual(q.Peek(), "g");
						
			q.Enqueue("h", 5);
			Assert.AreEqual(q.Peek(), "h");

			q.Enqueue("i", 7);
			Assert.AreEqual(q.Peek(), "h");
		}

		private PriorityQueue<string> GetSimpleTestPriorityQueue()
		{
			PriorityQueue<string> q = new PriorityQueue<string>(PriorityQueueType.MinPriorityQueue);

			q.Add("a", 1);
			q.Add("b", 2);
			q.Add("c", 3);
			q.Add("d", 4);
			q.Add("e", 5);
			q.Add("f", 6);

			return q;
		}

		[Test]
		public void TestContains()
		{
			PriorityQueue<string> q = GetTestPriorityQueue();

			Assert.AreEqual(q.Contains("a"), true);
			Assert.AreEqual(q.Contains("b"), true);
			Assert.AreEqual(q.Contains("c"), true);
			Assert.AreEqual(q.Contains("d"), true);
			Assert.AreEqual(q.Contains("e"), true);
			Assert.AreEqual(q.Contains("f"), true);

			Assert.AreEqual(q.Contains("g"), false);
		}

		[Test]
		public void TestCopyTo()
		{
			PriorityQueue<string> q = GetTestPriorityQueue();
			string[] array = new string[q.Count];

			q.CopyTo(array, 0);

			List<string> l = new List<string>(array);
			
			Assert.AreEqual(l.Count, q.Count);

			Assert.AreEqual(l.Contains("a"), true);
			Assert.AreEqual(l.Contains("b"), true);
			Assert.AreEqual(l.Contains("c"), true);
			Assert.AreEqual(l.Contains("d"), true);
			Assert.AreEqual(l.Contains("e"), true);
			Assert.AreEqual(l.Contains("f"), true);

			Assert.AreEqual(l.Contains("u"), true);
			Assert.AreEqual(l.Contains("v"), true);
			Assert.AreEqual(l.Contains("w"), true);
			Assert.AreEqual(l.Contains("x"), true);
			Assert.AreEqual(l.Contains("y"), true);
			Assert.AreEqual(l.Contains("z"), true);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestNullCopyTo()
		{
			PriorityQueue<string> q = new PriorityQueue<string>(PriorityQueueType.MinPriorityQueue);
			q.CopyTo(null, 0);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestInvalidCopyTo1()
		{
			PriorityQueue<string> q = GetTestPriorityQueue();

			string[] array = new string[q.Count - 1];
			q.CopyTo(array, 0);
		}
		
		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestInvalidCopyTo2()
		{
			PriorityQueue<string> q = GetTestPriorityQueue();

			string[] array = new string[q.Count];
			q.CopyTo(array, 1);
		}

		[Test]
		public void TestKeyEnumerator()
		{
			PriorityQueue<string> q = GetTestPriorityQueue();
			IEnumerator<Association<int, string>> enumerator = q.GetKeyEnumerator();

			int counter = 0;

			while (enumerator.MoveNext())
			{
				counter++;
			}

			Assert.AreEqual(counter, q.Count);
		}

		private PriorityQueue<string> GetTestPriorityQueue()
		{
			PriorityQueue<string> q = new PriorityQueue<string>(PriorityQueueType.MinPriorityQueue);

			q.Add("a", 1);
			q.Add("b", 2);
			q.Add("c", 3);
			q.Add("d", 4);
			q.Add("e", 5);
			q.Add("f", 6);

			q.Add("z", 6);
			q.Add("y", 5);
			q.Add("x", 4);
			q.Add("w", 3);
			q.Add("v", 2);
			q.Add("u", 1);

			return q;
		}
	}
}

