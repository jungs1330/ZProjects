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

namespace NGenericsTests
{
	[TestFixture]
	public class AssociationTest
	{
		[Test]
		public void TestInit()
		{
			Association<int, string> assoc = new Association<int, string>(5, "aa");

			Assert.AreEqual(assoc.Key, 5);
			Assert.AreEqual(assoc.Value, "aa");
		}

		[Test]
		public void TestSet()
		{
			Association<int, string> assoc = new Association<int, string>(5, "aa");

			Assert.AreEqual(assoc.Key, 5);
			Assert.AreEqual(assoc.Value, "aa");

			assoc.Key = 2;
			assoc.Value = "bla";

			Assert.AreEqual(assoc.Key, 2);
			Assert.AreEqual(assoc.Value, "bla");
		}
	}
}
