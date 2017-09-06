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
using NGenerics.Algorithms.Math;

namespace NGenericsTests
{
	[TestFixture]
	public class FibonacciTest
	{

		[Test]
		public void TestFibonacciNth() {
			List<long> fib = new List<long>();

			for (int i = 0; i< 10; i++) {
				fib.Add(Fibonacci.GenerateNthFibonacci(i));			
			}

			Assert.AreEqual(fib[0], 0);
			Assert.AreEqual(fib[1], 1);
			Assert.AreEqual(fib[2], 1);
			Assert.AreEqual(fib[3], 2);
			Assert.AreEqual(fib[4], 3);
			Assert.AreEqual(fib[5], 5);
			Assert.AreEqual(fib[6], 8);
			Assert.AreEqual(fib[7], 13);
			Assert.AreEqual(fib[8], 21);
			Assert.AreEqual(fib[9], 34);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestFibonacciNthInvalid()
		{
			Fibonacci.GenerateNthFibonacci(-1);
		}

		[Test]
		public void TestFibonacciGenerateSequence()
		{
			List<long> fib = new List<long>();

			fib.AddRange(Fibonacci.GenerateFibonacciSequence(10));

			Assert.AreEqual(fib.Count, 11);

			Assert.AreEqual(fib[0], 0);
			Assert.AreEqual(fib[1], 1);
			Assert.AreEqual(fib[2], 1);
			Assert.AreEqual(fib[3], 2);
			Assert.AreEqual(fib[4], 3);
			Assert.AreEqual(fib[5], 5);
			Assert.AreEqual(fib[6], 8);
			Assert.AreEqual(fib[7], 13);
			Assert.AreEqual(fib[8], 21);
			Assert.AreEqual(fib[9], 34);
			Assert.AreEqual(fib[10], 55);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestFibonacciGenerateSequenceInvalid()
		{
			Fibonacci.GenerateFibonacciSequence(-1);
		}
	}
}
