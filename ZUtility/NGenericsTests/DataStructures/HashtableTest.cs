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

namespace NGenericsTests
{
	[TestFixture]
	public class HashtableTest
	{
		[Test]
		public void TestSuccesfulInit()
		{
			// Default constructor
			VisitableHashtable<string, string> h1 = new VisitableHashtable<string, string>();

			Assert.AreEqual(h1.Count, 0);
			Assert.AreEqual(h1.IsEmpty, true);
			Assert.AreEqual(h1.IsFull, false);

			VisitableHashtable<string, string> h2 = new VisitableHashtable<string, string>();
			h2.Add("aa", "bb");
			h2.Add("cc", "dd");

			Assert.AreEqual(h2.Count, 2);
			Assert.AreEqual(h2.IsEmpty, false);
			Assert.AreEqual(h2.IsFull, false);

			h1 = new VisitableHashtable<string, string>(h2);

			Assert.AreEqual(h1.Count, 2);
			Assert.AreEqual(h1.IsEmpty, false);
			Assert.AreEqual(h1.IsFull, false);

			h1 = new VisitableHashtable<string, string>(new DummyComparer());

			Assert.AreEqual(h1.Count, 0);
			Assert.AreEqual(h1.IsEmpty, true);
			Assert.AreEqual(h1.IsFull, false);

			h1 = new VisitableHashtable<string, string>(50);

			Assert.AreEqual(h1.Count, 0);
			Assert.AreEqual(h1.IsEmpty, true);
			Assert.AreEqual(h1.IsFull, false);

			h1 = new VisitableHashtable<string, string>(h2, new DummyComparer());

			Assert.AreEqual(h1.Count, 2);
			Assert.AreEqual(h1.IsEmpty, false);
			Assert.AreEqual(h1.IsFull, false);

			h1 = new VisitableHashtable<string, string>(300, new DummyComparer());

			Assert.AreEqual(h1.Count, 0);
			Assert.AreEqual(h1.IsEmpty, true);
			Assert.AreEqual(h1.IsFull, false);
		}

		[Test]
		public void TestVisitor()
		{
			TrackingVisitor<KeyValuePair<string, string>> visitor = new TrackingVisitor<KeyValuePair<string, string>>();

			VisitableHashtable<string, string> h = GetTestHashtable();
			h.Accept(visitor);

			Assert.AreEqual(visitor.TrackingList.Count, 3);

			bool found = false;
			for (int i = 0; i < visitor.TrackingList.Count; i++)
			{
				if ((visitor.TrackingList[i].Key == "a") && (visitor.TrackingList[i].Value == "aa"))
				{
					found = true;
					break;
				}
			}

			Assert.AreEqual(found, true);

			found = false;
			for (int i = 0; i < visitor.TrackingList.Count; i++)
			{
				if ((visitor.TrackingList[i].Key == "b") && (visitor.TrackingList[i].Value == "bb"))
				{
					found = true;
					break;
				}
			}

			Assert.AreEqual(found, true);

			found = false;
			for (int i = 0; i < visitor.TrackingList.Count; i++)
			{
				if ((visitor.TrackingList[i].Key == "c") && (visitor.TrackingList[i].Value == "cc"))
				{
					found = true;
					break;
				}
			}

			Assert.AreEqual(found, true);
		}

		[Test]
		public void TestStoppingVisitor()
		{
			EqualityFindingVisitor<KeyValuePair<string, string>> visitor = new EqualityFindingVisitor<KeyValuePair<string, string>>(new KeyValuePair<string, string>("b", "bb"), new KeyValuePairEqualityComparer<string, string>());

			VisitableHashtable<string, string> h = GetTestHashtable();
			h.Accept(visitor);

			Assert.AreEqual(visitor.HasCompleted, true);
			Assert.AreEqual(visitor.Found, true);

			visitor = new EqualityFindingVisitor<KeyValuePair<string, string>>
				(
					new KeyValuePair<string, string>("d", "dd"), 
					new KeyValuePairEqualityComparer<string, string>()
				);
			h.Accept(visitor);

			Assert.AreEqual(visitor.HasCompleted, false);
			Assert.AreEqual(visitor.Found, false);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestNullVisitor()
		{
			VisitableHashtable<string, string> h = new VisitableHashtable<string, string>();
			h.Accept(null);
		}

		[Test]
		public void TestCompareTo()
		{
			VisitableHashtable<string, string> h1 = GetTestHashtable();
			VisitableHashtable<string, string> h2 = GetTestHashtable();

			h2.Add("d", "dd");

			Assert.AreEqual(h1.CompareTo(h2), -1);
			Assert.AreEqual(h2.CompareTo(h1), 1);
			Assert.AreEqual(h1.CompareTo(h1), 0);

			object o = new object();

			Assert.AreEqual(h1.CompareTo(o), -1);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestInvalidCompareTo()
		{
			VisitableHashtable<string, int> h = new VisitableHashtable<string, int>();
			h.CompareTo(null);
		}

		[Test]
		public void TestIsEmpty()
		{
			VisitableHashtable<string, string> h = GetTestHashtable();
			Assert.AreEqual(h.IsEmpty, false);

			h = new VisitableHashtable<string, string>();
			Assert.AreEqual(h.IsEmpty, true);

			h.Add("ff", "fff");
			Assert.AreEqual(h.IsEmpty, false);
		}

		[Test]
		public void TestIsFixedSize()
		{
			VisitableHashtable<string, string> h = GetTestHashtable();
			Assert.AreEqual(h.IsFixedSize, false);

			h = new VisitableHashtable<string, string>();
			Assert.AreEqual(h.IsFixedSize, false);

			h.Add("ff", "fff");
			Assert.AreEqual(h.IsFixedSize, false);
		}

		[Test]
		public void TestIsFull()
		{
			VisitableHashtable<string, string> h = GetTestHashtable();
			Assert.AreEqual(h.IsFull, false);

			h = new VisitableHashtable<string, string>();
			Assert.AreEqual(h.IsFull, false);

			h.Add("ff", "fff");
			Assert.AreEqual(h.IsFull, false);
		}

		private VisitableHashtable<string, string> GetTestHashtable()
		{
			VisitableHashtable<string, string> h = new VisitableHashtable<string, string>();
			h.Add("a", "aa");
			h.Add("b", "bb");
			h.Add("c", "cc");

			return h;
		}
	}
}
