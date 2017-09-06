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


namespace NGenericsTests.Algorithms.Math
{
	[TestFixture]
	public class EuclidTest
	{
		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestInvalid2()
		{
			Euclid.FindGreatestCommonDivisor(8, -1);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestInvalid3()
		{
			Euclid.FindGreatestCommonDivisor(-1, -4);
		}

		[Test]
		public void TestValid()
		{
			Assert.AreEqual(Euclid.FindGreatestCommonDivisor(8, 4), 4);
			Assert.AreEqual(Euclid.FindGreatestCommonDivisor(4, 8), 4);

			Assert.AreEqual(Euclid.FindGreatestCommonDivisor(5, 1), 1);

			Assert.AreEqual(Euclid.FindGreatestCommonDivisor(0, 0), 0);

			Assert.AreEqual(Euclid.FindGreatestCommonDivisor(0, 0), 0);

			Assert.AreEqual(Euclid.FindGreatestCommonDivisor(180, 640), 20);
		}
	}
}
