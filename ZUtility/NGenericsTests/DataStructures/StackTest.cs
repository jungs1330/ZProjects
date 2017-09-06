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
	public class StackTest
	{
		[Test]
		public void TestSuccesfulInit()
		{
			VisitableStack<int> stack = new VisitableStack<int>();

			Assert.AreEqual(stack.Count, 0);
			Assert.AreEqual(stack.IsEmpty, true);
			Assert.AreEqual(stack.IsFull, false);

			stack = new VisitableStack<int>(5);
			VisitableStack<int> stack2 = new VisitableStack<int>(stack);

		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestNullVisitor()
		{
			VisitableStack<int> s = new VisitableStack<int>(); ;
			s.Accept(null);
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void TestEmptyPop()
		{
			VisitableStack<int> stack = new VisitableStack<int>();
			stack.Pop();
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void TestEmptyPeek()
		{
			VisitableStack<int> stack = new VisitableStack<int>();
			int i = stack.Peek();
		}

		[Test]
		public void TestCompareTo()
		{
			VisitableStack<int> stack1 = new VisitableStack<int>();
			VisitableStack<int> stack2 = new VisitableStack<int>();
			VisitableStack<int> stack3 = new VisitableStack<int>();

			Assert.AreEqual(stack1.CompareTo(stack2), 0);
			Assert.AreEqual(stack1.CompareTo(stack3), 0);
			Assert.AreEqual(stack3.CompareTo(stack1), 0);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestInvalidCompareTo()
		{
			VisitableStack<string> s = new VisitableStack<string>();
			s.CompareTo(null);
		}

		[Test]
		public void TestCompareToOther()
		{
			VisitableStack<int> stack1 = new VisitableStack<int>();
			object o = new object();

			Assert.AreEqual(stack1.CompareTo(o), -1);
		}

		[Test]
		public void TestPushPop()
		{
			VisitableStack<int> stack = new VisitableStack<int>();
			stack.Push(2);
			stack.Push(4);
			stack.Push(6);

			Assert.AreEqual(stack.Count, 3);


			Assert.AreEqual(stack.Pop(), 6);
			Assert.AreEqual(stack.Pop(), 4);
			Assert.AreEqual(stack.Pop(), 2);
			Assert.AreEqual(stack.Count, 0);
		}

		[Test]
		public void TestIsEmpty()
		{
			VisitableStack<int> stack = new VisitableStack<int>();
			Assert.AreEqual(stack.IsEmpty, true);

			stack.Push(2);
			stack.Push(4);
			stack.Push(6);
			stack.Push(4);

			Assert.AreEqual(stack.IsEmpty, false);

			stack.Clear();
			Assert.AreEqual(stack.IsEmpty, true);
		}


		[Test]
		public void TestPeek()
		{
			VisitableStack<int> stack = new VisitableStack<int>();
			stack.Push(2);
			stack.Push(4);
			stack.Push(6);

			Assert.AreEqual(stack.Peek(), 6);

			stack.Pop();

			Assert.AreEqual(stack.Peek(), 4);
		}

		[Test]
		public void Testcount()
		{
			VisitableStack<int> stack = new VisitableStack<int>();

			for (int i = 0; i < 5; i++)
			{
				stack.Push(i * 3);
				Assert.AreEqual(stack.Count, i + 1);
			}
		}

		[Test]
		public void TestClear()
		{
			VisitableStack<int> stack = new VisitableStack<int>();
			stack.Push(2);
			stack.Push(4);
			stack.Push(6);

			stack.Clear();

			Assert.AreEqual(stack.IsEmpty, true);
			Assert.AreEqual(stack.IsFull, false);
			Assert.AreEqual(stack.Count, 0);
		}

		[Test]
		public void TestVisitor()
		{
			VisitableStack<int> stack = new VisitableStack<int>();
			stack.Push(2);
			stack.Push(4);
			stack.Push(9);
			stack.Push(3);

			ComparableFindingVisitor<int> visitor = new ComparableFindingVisitor<int>(9);
			stack.Accept(visitor);
			
			Assert.AreEqual(visitor.Found, true);

			visitor = new ComparableFindingVisitor<int>(5);
			stack.Accept(visitor);
			Assert.AreEqual(visitor.Found, false);
		}

		[Test]
		public void TestEnumeratorValues()
		{
			VisitableStack<int> stack = new VisitableStack<int>();
			stack.Push(2);
			stack.Push(4);
			stack.Push(6);

			IEnumerator<int> enumerator = stack.GetEnumerator();

			Assert.AreEqual(enumerator.MoveNext(), true);
			Assert.AreEqual(enumerator.Current, 6);

			Assert.AreEqual(enumerator.MoveNext(), true);
			Assert.AreEqual(enumerator.Current, 4);

			Assert.AreEqual(enumerator.MoveNext(), true);
			Assert.AreEqual(enumerator.Current, 2);

			Assert.AreEqual(enumerator.MoveNext(), false);
		}

		[Test]
		public void TestEmptyEnumerator()
		{
			VisitableStack<int> stack = new VisitableStack<int>();
			IEnumerator<int> enumerator = stack.GetEnumerator();
			Assert.AreEqual(enumerator.MoveNext(), false);
		}

		[Test]
		public void TestReadOnly()
		{
			VisitableStack<int> stack = new VisitableStack<int>();
			Assert.AreEqual(stack.IsReadOnly, false);

			stack.Push(5);
			Assert.AreEqual(stack.IsReadOnly, false);
		}

		[Test]
		public void TestAdd()
		{
			VisitableStack<int> stack = new VisitableStack<int>();
			stack.Add(5);

			Assert.AreEqual(stack.Count, 1);
			Assert.AreEqual(stack.Peek(), 5);
			Assert.AreEqual(stack.Pop(), 5);

			stack.Push(2);
			stack.Push(4);

			Assert.AreEqual(stack.Count, 2);
			Assert.AreEqual(stack.Peek(), 4);
			Assert.AreEqual(stack.Pop(), 4);
				
			Assert.AreEqual(stack.Count, 1);
			Assert.AreEqual(stack.Peek(), 2);
			Assert.AreEqual(stack.Pop(), 2);;
		}

		[Test]
		public void TestIsFixedSize()
		{
			VisitableStack<int> stack = new VisitableStack<int>();

			Assert.AreEqual(stack.IsFixedSize, false);

			stack.Add(5);

			Assert.AreEqual(stack.IsFixedSize, false);

		}
	}
}
