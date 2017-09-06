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
	public class BagTest
	{
		[Test]
		public void TestSuccesfulInit()
		{
			Bag<string> b = new Bag<string>();

			Assert.AreEqual(b.Count, 0);
			Assert.AreEqual(b.IsEmpty, true);
			Assert.AreEqual(b.IsFull, false);

			b = new Bag<string>(EqualityComparer<string>.Default);

			Assert.AreEqual(b.Count, 0);
			Assert.AreEqual(b.IsEmpty, true);
			Assert.AreEqual(b.IsFull, false);

			b = new Bag<string>(50);

			Assert.AreEqual(b.Count, 0);
			Assert.AreEqual(b.IsEmpty, true);
			Assert.AreEqual(b.IsFull, false);

			b = new Bag<string>(50);

			Assert.AreEqual(b.Count, 0);
			Assert.AreEqual(b.IsEmpty, true);
			Assert.AreEqual(b.IsFull, false);

			b = new Bag<string>(50, EqualityComparer<string>.Default);

			Assert.AreEqual(b.Count, 0);
			Assert.AreEqual(b.IsEmpty, true);
			Assert.AreEqual(b.IsFull, false);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestFaultyComparer1()
		{
			Bag<string> b = new Bag<string>(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestFaultyComparer2()
		{
			Bag<string> b = new Bag<string>(5, null);
		}

		[Test]
		public void TestCount() {
			Bag<string> b = GetTestBag();
			Assert.AreEqual(b.Count, 35);
			Assert.AreEqual(b.IsEmpty, false);
			
			b = new Bag<string>();
			Assert.AreEqual(b.Count, 0);
			Assert.AreEqual(b.IsEmpty, true);

			b.Add("aa");
			Assert.AreEqual(b.Count, 1);
			Assert.AreEqual(b.IsEmpty, false);

			b.Add("aa");
			Assert.AreEqual(b.Count, 2);
			Assert.AreEqual(b.IsEmpty, false);

			b.Add("bb");
			Assert.AreEqual(b.Count, 3);
			Assert.AreEqual(b.IsEmpty, false);
		}

		[Test]
		public void TestClear() {
			Bag<string> b = GetTestBag();

			b.Clear();
			Assert.AreEqual(b.Count, 0);
			Assert.AreEqual(b.IsEmpty, true);
			Assert.AreEqual(b.IsFull, false);

			b.Add("aa");
			b.Clear();

			Assert.AreEqual(b.Count, 0);
			Assert.AreEqual(b.IsEmpty, true);
			Assert.AreEqual(b.IsFull, false);
		}

		[Test]
		public void TestAdd()
		{
			Bag<string> b = new Bag<string>();

			b.Add("aa");
			Assert.AreEqual(b.Count, 1);
			Assert.AreEqual(b.Contains("aa"), true);
			Assert.AreEqual(b["aa"], 1);

			b.Add("bb");
			Assert.AreEqual(b.Count, 2);
			Assert.AreEqual(b.Contains("bb"), true);
			Assert.AreEqual(b["bb"], 1);

			b.Add("aa");
			Assert.AreEqual(b.Count, 3);
			Assert.AreEqual(b.Contains("aa"), true);
			Assert.AreEqual(b["aa"], 2);

			b.Add("cc", 3);
			Assert.AreEqual(b.Count, 6);
			Assert.AreEqual(b.Contains("cc"), true);
			Assert.AreEqual(b["cc"], 3);

			b.Add("cc", 2);

			Assert.AreEqual(b.Count, 8);
			Assert.AreEqual(b.Contains("cc"), true);
			Assert.AreEqual(b["cc"], 5);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestFaultyAddZero()
		{
			Bag<string> b = new Bag<string>();
			b.Add("aa", 0);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestFaultyAddNegative()
		{
			Bag<string> b = new Bag<string>();
			b.Add("aa", -1);
		}

		[Test]
		public void TestCompareTo()
		{
			Bag<string> b1 = new Bag<string>();
			Bag<string> b2 = GetTestBag();

			Assert.AreEqual(b1.CompareTo(b2), -1);
			Assert.AreEqual(b2.CompareTo(b1), 1);
			Assert.AreEqual(b2.CompareTo(b2), 0);
			Assert.AreEqual(b1.CompareTo(b1), 0);

			Assert.AreEqual(b1.CompareTo(new object()), -1);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestInvalidCompareTo()
		{
			Bag<string> b = new Bag<string>();
			b.CompareTo(null);
		}

		[Test]
		public void TestContains()
		{
			Bag<string> b = new Bag<string>();

			b.Add("aa");
			Assert.AreEqual(b.Contains("aa"), true);
			Assert.AreEqual(b["aa"], 1);

			b.Add("bb");
			Assert.AreEqual(b.Contains("bb"), true);
			Assert.AreEqual(b["aa"], 1);
			Assert.AreEqual(b["bb"], 1);

			b.Add("aa");
			Assert.AreEqual(b.Contains("aa"), true);
			Assert.AreEqual(b["aa"], 2);
			Assert.AreEqual(b["bb"], 1);

			b.Add("cc", 3);
			Assert.AreEqual(b.Contains("cc"), true);
			Assert.AreEqual(b["aa"], 2);
			Assert.AreEqual(b["bb"], 1);
			Assert.AreEqual(b["cc"], 3);
		}

		[Test]
		public void TestRemove()
		{
			Bag<string> b = new Bag<string>();

			b.Add("aa");
			b.Add("bb");
			b.Add("aa");
			b.Add("cc", 3);

			Assert.AreEqual(b.Count, 6);

			Assert.AreEqual(b.Remove("aa"), true);
			Assert.AreEqual(b.Contains("aa"), true);
			Assert.AreEqual(b["aa"], 1);
			Assert.AreEqual(b.Count, 5);

			Assert.AreEqual(b.Remove("aa"), true);
			Assert.AreEqual(b.Contains("aa"), false);
			Assert.AreEqual(b["aa"], 0);
			Assert.AreEqual(b.Count, 4);

			Assert.AreEqual(b.Remove("dd"), false);
			Assert.AreEqual(b.Count, 4);

			Assert.AreEqual(b.Remove("bb"), true);
			Assert.AreEqual(b.Contains("bb"), false);
			Assert.AreEqual(b["bb"], 0);
			Assert.AreEqual(b.Count, 3);

			Assert.AreEqual(b.Remove("cc"), true);
			Assert.AreEqual(b.Contains("cc"), true);
			Assert.AreEqual(b["cc"], 2);
			Assert.AreEqual(b.Count, 2);
		}

		[Test]
		public void TestRemoveMax()
		{
			Bag<string> b = new Bag<string>();

			b.Add("aa");
			b.Add("bb");
			b.Add("aa");
			b.Add("cc", 3);

			Assert.AreEqual(b.Count, 6);

			Assert.AreEqual(b.Remove("aa", 1), true);
			Assert.AreEqual(b.Contains("aa"), true);
			Assert.AreEqual(b["aa"], 1);
			Assert.AreEqual(b.Count, 5);

			Assert.AreEqual(b.Remove("aa", 2), true);
			Assert.AreEqual(b.Contains("aa"), false);
			Assert.AreEqual(b["aa"], 0);
			Assert.AreEqual(b.Count, 4);

			Assert.AreEqual(b.Remove("cc", 2), true);
			Assert.AreEqual(b.Contains("cc"), true);
			Assert.AreEqual(b["cc"], 1);
			Assert.AreEqual(b.Count, 2);

			Assert.AreEqual(b.Remove("dd", 2), false);
		}

		[Test]
		public void TestRemoveAll()
		{
			Bag<string> b = new Bag<string>();

			b.Add("aa");
			b.Add("bb");
			b.Add("aa");
			b.Add("cc", 3);

			Assert.AreEqual(b.Count, 6);

			Assert.AreEqual(b.RemoveAll("aa"), true);
			Assert.AreEqual(b.Contains("aa"), false);
			Assert.AreEqual(b["aa"], 0);
			Assert.AreEqual(b.Count, 4);

			Assert.AreEqual(b.RemoveAll("dd"), false);
			Assert.AreEqual(b.Count, 4);


			Assert.AreEqual(b.RemoveAll("cc"), true);
			Assert.AreEqual(b.Contains("cc"), false);
			Assert.AreEqual(b["cc"], 0);
			Assert.AreEqual(b.Count, 1);
		}

		[Test]
		public void TestAccept()
		{
			Bag<string> b = new Bag<string>();

			b.Add("5");
			b.Add("4");
			b.Add("3");
			b.Add("2");

			TrackingVisitor<string> visitor = new TrackingVisitor<string>();
			b.Accept(visitor);

			Assert.AreEqual(visitor.TrackingList.Count, 4);
			Assert.AreEqual(visitor.TrackingList.Contains("5"), true);
			Assert.AreEqual(visitor.TrackingList.Contains("4"), true);
			Assert.AreEqual(visitor.TrackingList.Contains("3"), true);
			Assert.AreEqual(visitor.TrackingList.Contains("2"), true);
		}

		[Test]
		public void TestAccept2()
		{
			Bag<string> b = new Bag<string>();

			b.Add("5");
			b.Add("4");
			b.Add("3");
			b.Add("2");

			TrackingVisitor<KeyValuePair<string, int>> visitor = new TrackingVisitor<KeyValuePair<string, int>>();
			b.Accept(visitor);

			Assert.AreEqual(visitor.TrackingList.Count, 4);
			Assert.AreEqual(visitor.TrackingList.Contains(new KeyValuePair<string, int>("5", 1)), true);
			Assert.AreEqual(visitor.TrackingList.Contains(new KeyValuePair<string, int>("4", 1)), true);
			Assert.AreEqual(visitor.TrackingList.Contains(new KeyValuePair<string, int>("3", 1)), true);
			Assert.AreEqual(visitor.TrackingList.Contains(new KeyValuePair<string, int>("2", 1)), true);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestNullVisitor1()
		{
			Bag<string> b = new Bag<string>();
			b.Accept((IVisitor<string>)null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestNullVisitor2()
		{
			Bag<string> b = new Bag<string>();
			b.Accept((IVisitor<KeyValuePair<string, int>>)null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestInvalidAccept1()
		{
			Bag<string> b = new Bag<string>();
			b.Accept((IVisitor<string>) null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestInvalidAccept2()
		{
			Bag<string> b = new Bag<string>();
			b.Accept((IVisitor<KeyValuePair<string, int>>) null);
		}

		[Test]
		public void TestCopyTo()
		{
			Bag<int> b = new Bag<int>();
			b.Add(3);
			b.Add(4);
			b.Add(5);
			b.Add(6);

			int[] array = new int[50];

			b.CopyTo(array, 0);

			Assert.AreNotEqual(array[0], 0);
			Assert.AreNotEqual(array[1], 0);
			Assert.AreNotEqual(array[2], 0);
			Assert.AreNotEqual(array[3], 0);
			Assert.AreEqual(array[4], 0);

			List<int> l = new List<int>();
			l.Add(array[0]);
			l.Add(array[1]);
			l.Add(array[2]);
			l.Add(array[3]);
			
			Assert.AreEqual(l.Contains(3), true);
			Assert.AreEqual(l.Contains(4), true);
			Assert.AreEqual(l.Contains(5), true);
			Assert.AreEqual(l.Contains(6), true);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestInvalidCopyTo1()
		{
			Bag<int> b = new Bag<int>();
			b.Add(3);
			b.Add(4);
			b.Add(5);
			b.Add(6);

			int[] array = new int[3];

			b.CopyTo(array, 0);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestInvalidCopyTo2()
		{
			Bag<int> b = new Bag<int>();
			b.Add(3);
			b.Add(4);
			b.Add(5);
			b.Add(6);

			int[] array = new int[4];

			b.CopyTo(array, 1);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestInvalidCopyTo3()
		{
			Bag<int> b = new Bag<int>();
			b.Add(3);
			b.Add(4);
			b.Add(5);
			b.Add(6);

			b.CopyTo(null, 1);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestInvalidIsEqual()
		{
			Bag<int> b1 = new Bag<int>();
			bool b = b1.IsEqual(null);
		}

		[Test]
		public void TestIsEqual()
		{
			Bag<int> b1 = new Bag<int>();
			Bag<int> b2 = new Bag<int>();

			Assert.AreEqual(b1.IsEqual(b2), true);
			Assert.AreEqual(b2.IsEqual(b1), true);

			b2.Add(5);

			Assert.AreEqual(b1.IsEqual(b2), false);
			Assert.AreEqual(b2.IsEqual(b1), false);

			b1.Add(5);

			Assert.AreEqual(b1.IsEqual(b2), true);
			Assert.AreEqual(b2.IsEqual(b1), true);

			b2.Add(6);

			Assert.AreEqual(b1.IsEqual(b2), false);
			Assert.AreEqual(b2.IsEqual(b1), false);

			b1.Add(7);

			Assert.AreEqual(b1.IsEqual(b2), false);
			Assert.AreEqual(b2.IsEqual(b1), false);
		}

		[Test]
		public void TestIsEqualInterface()
		{
			Bag<int> b1 = new Bag<int>();
			Bag<int> b2 = new Bag<int>();

			Assert.AreEqual(((IBag<int>)b1).IsEqual(b2), true);
			Assert.AreEqual(((IBag<int>)b2).IsEqual(b1), true);

			b2.Add(5);

			Assert.AreEqual(((IBag<int>)b1).IsEqual(b2), false);
			Assert.AreEqual(((IBag<int>)b2).IsEqual(b1), false);

			b1.Add(5);

			Assert.AreEqual(((IBag<int>)b1).IsEqual(b2), true);
			Assert.AreEqual(((IBag<int>)b2).IsEqual(b1), true);

			b2.Add(6);

			Assert.AreEqual(((IBag<int>)b1).IsEqual(b2), false);
			Assert.AreEqual(((IBag<int>)b2).IsEqual(b1), false);

			b1.Add(7);

			Assert.AreEqual(((IBag<int>)b1).IsEqual(b2), false);
			Assert.AreEqual(((IBag<int>)b2).IsEqual(b1), false);
		}

		[Test]
		public void TestDifference()
		{
			Bag<int> b1 = new Bag<int>();
			b1.Add(3);
			b1.Add(4);
			b1.Add(5);
			b1.Add(6);

			Bag<int> b2 = new Bag<int>();
			b2.Add(3);
			b2.Add(4);
			b2.Add(5);

			Bag<int> shouldBe = new Bag<int>();
			shouldBe.Add(6);

			Bag<int> result = b1 - b2;

			Assert.AreEqual(result.IsEqual(shouldBe), true);

			b1.Clear();
			b2.Clear();

			b1.Add(3, 3);
			b2.Add(3, 2);

			b1.Add(5, 5);
			b2.Add(5, 7);

			shouldBe.Clear();
			shouldBe.Add(3, 1);

			result = b1 - b2;

			Assert.AreEqual(result.IsEqual(shouldBe), true);
		}

		[Test]
		public void TestDifferenceInterface()
		{
			Bag<int> b1 = new Bag<int>();
			b1.Add(3);
			b1.Add(4);
			b1.Add(5);
			b1.Add(6);

			Bag<int> b2 = new Bag<int>();
			b2.Add(3);
			b2.Add(4);
			b2.Add(5);

			Bag<int> shouldBe = new Bag<int>();
			shouldBe.Add(6);

			Bag<int> result = (Bag<int>) ((IBag<int>) b1).Difference(b2);

			Assert.AreEqual(result.IsEqual(shouldBe), true);

			((Bag<int>)b1).Clear();
			((Bag<int>)b2).Clear();

			b1.Add(3, 3);
			b2.Add(3, 2);

			b1.Add(5, 5);
			b2.Add(5, 7);

			shouldBe.Clear();
			shouldBe.Add(3, 1);

			result = b1.Difference(b2);

			Assert.AreEqual(result.IsEqual(shouldBe), true);
		}		

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestNullDifference()
		{
			Bag<int> b = new Bag<int>();
			b.Difference(null);
		}

		[Test]
		public void TestEnumerator()
		{
			Bag<string> b = GetTestBag();
			IEnumerator<string> enumerator = b.GetEnumerator();

			int counter = 0;

			while (enumerator.MoveNext())
			{
				counter++;

			}

			Assert.AreEqual(counter, 20);
		}

		[Test]
		public void TestEnumeratorInterface()
		{
			IEnumerable<string> b = GetTestBag();
			IEnumerator<string> enumerator = b.GetEnumerator();
			
			int counter = 0;

			while (enumerator.MoveNext())
			{
				counter++;

			}

			Assert.AreEqual(counter, 20);
		}

		[Test]
		public void TestCountEnumerator()
		{
			Bag<string> b = GetTestBag();

			IEnumerator<KeyValuePair<string, int>> enumerator =  b.GetCountEnumerator();

			int counter = 0;

			while (enumerator.MoveNext())
			{
				if (Int32.Parse(enumerator.Current.Key) < 5)
				{
					Assert.AreEqual(enumerator.Current.Value, 3);
				}
				else if (Int32.Parse(enumerator.Current.Key) < 10)
				{
					Assert.AreEqual(enumerator.Current.Value, 2);
				}
				else if (Int32.Parse(enumerator.Current.Key) < 20)
				{
					Assert.AreEqual(enumerator.Current.Value, 1);
				}

				counter++;
			}

			Assert.AreEqual(counter, 20);
		}

		[Test]
		public void TestIsFixedSize()
		{
			Bag<string> b = GetTestBag();
			Assert.AreEqual(b.IsFixedSize, false);
		}

		[Test]
		public void TestIsReadOnly()
		{
			Bag<string> b = GetTestBag();
			Assert.AreEqual(b.IsReadOnly, false);
		}

		[Test]
		public void TestUnion()
		{
			Bag<int> b1 = new Bag<int>();
			Bag<int> b2 = new Bag<int>();

			b1.Add(2, 2);
			b1.Add(3, 3);
			b1.Add(4, 5);

			b2.Add(3, 2);
			b2.Add(4, 3);
			b2.Add(5, 2);
			b2.Add(6, 2);

			Bag<int> shouldBe = new Bag<int>();
			shouldBe.Add(2, 2);
			shouldBe.Add(3, 5);
			shouldBe.Add(4, 8);
			shouldBe.Add(5, 2);
			shouldBe.Add(6, 2);

			Bag<int> result = b1 + b2;

			Assert.AreEqual(shouldBe.IsEqual(result), true);

			result = b2 + b1;

			Assert.AreEqual(shouldBe.IsEqual(result), true);
		}

		[Test]
		public void TestUnionInterface()
		{
			Bag<int> b1 = new Bag<int>();
			Bag<int> b2 = new Bag<int>();

			b1.Add(2, 2);
			b1.Add(3, 3);
			b1.Add(4, 5);

			b2.Add(3, 2);
			b2.Add(4, 3);
			b2.Add(5, 2);

			Bag<int> shouldBe = new Bag<int>();
			shouldBe.Add(2, 2);
			shouldBe.Add(3, 5);
			shouldBe.Add(4, 8);
			shouldBe.Add(5, 2);

			Bag<int> result = (Bag<int>) ((IBag<int>)b1).Union(b2);

			Assert.AreEqual(shouldBe.IsEqual(result), true);

			result = (Bag<int>)((IBag<int>)b2).Union(b1);

			Assert.AreEqual(shouldBe.IsEqual(result), true);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestInvalidUnion()
		{
			Bag<int> b1 = new Bag<int>();
			Bag<int> result = b1 + null;
		}

		[Test]
		public void TestIntersection()
		{
			Bag<string> b1 = new Bag<string>();
			Bag<string> b2 = GetTestBag();

			Bag<string> result = b1 * b2;
			
			Assert.AreEqual(result.IsEqual(b1), true);

			b1.Add("50", 2);

			Bag<string> shouldBe = new Bag<string>();

			result = b1 * b2;

			Assert.AreEqual(shouldBe.IsEqual(result), true);

			b1.Add("2", 2);

			shouldBe.Add("2", 2);

			result = b1 * b2;

			Assert.AreEqual(shouldBe.IsEqual(result), true);
		}

		[Test]
		public void TestIntersectionInterface()
		{
			Bag<string> b1 = new Bag<string>();
			Bag<string> b2 = GetTestBag();

			Bag<string> result = (Bag<string>) ((IBag<string>) b1).Intersection(b2);

			Assert.AreEqual(result.IsEqual(b1), true);

			b1.Add("50", 2);

			Bag<string> shouldBe = new Bag<string>();

			result = (Bag<string>) ((IBag<string>)b1).Intersection(b2);

			Assert.AreEqual(shouldBe.IsEqual(result), true);

			b1.Add("2", 2);

			shouldBe.Add("2", 2);

			result = (Bag<string>) ((IBag<string>)b1).Intersection(b2);

			Assert.AreEqual(shouldBe.IsEqual(result), true);
		}	

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestInvalidIntersection()
		{
			Bag<int> b1 = new Bag<int>();
			Bag<int> result = b1 * null;
		}

		private Bag<string> GetTestBag()
		{
 			Bag<string> b = new Bag<string>();

			for (int i = 0; i< 20; i++) {
				b.Add(i.ToString());
			}

			for (int i = 0; i< 10; i++) {
				b.Add(i.ToString());
			}

			for (int i = 0; i< 5; i++) {
				b.Add(i.ToString());
			}

			return b;
		}
	}
}
