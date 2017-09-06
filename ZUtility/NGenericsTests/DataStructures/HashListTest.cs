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

namespace NGenericsTests.DataStructures
{
    [TestFixture]
    public class HashListTest
    {
        [Test]
        public void TestSuccesfulInit()
        {
            HashList<string, int> h = new HashList<string, int>();

            Assert.AreEqual(h.Count, 0);

            Dictionary<string, IList<int>> dict = new Dictionary<string, IList<int>>();

            dict.Add("aa", new List<int>());
            dict.Add("bb", new List<int>());
            dict.Add("cc", new List<int>());

            dict["bb"].Add(5);
            dict["bb"].Add(6);
            dict["cc"].Add(2);

            h = new HashList<string, int>(dict);

            Assert.AreEqual(h.Count, 3);
            Assert.AreEqual(h.ValueCount, 3);

            Assert.AreEqual(h["aa"].Count, 0);
            Assert.AreEqual(h["bb"].Count, 2);
            Assert.AreEqual(h["cc"].Count, 1);

            Assert.AreEqual(h["bb"][0], 5);
            Assert.AreEqual(h["bb"][1], 6);
            Assert.AreEqual(h["cc"][0], 2);

            h = new HashList<string, int>(50);

            Assert.AreEqual(h.Count, 0);
            Assert.AreEqual(h.ValueCount, 0);
        }

        [Test]
        public void TestCounts()
        {
            HashList<int, string> h = new HashList<int, string>();

            h.Add(2, "a");
            
            Assert.AreEqual(h.ValueCount, 1);
            Assert.AreEqual(h.KeyCount, 1);

            h.Add(4, new List<string>(new string[] { "2", "3", "4", "5" }));

            Assert.AreEqual(h.ValueCount, 5);
            Assert.AreEqual(h.KeyCount, 2);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestInvalidRangeAdd()
        {
            HashList<int, string> h = new HashList<int, string>();
            h.Add(3, (List<string>) null);
        }

        [Test]
        public void TestAdd()
        {
            HashList<int, string> h = new HashList<int, string>();

            h.Add(2, "a");

            Assert.AreEqual(h.ValueCount, 1);
            Assert.AreEqual(h.KeyCount, 1);

            h.Add(4, new List<string>(new string[] { "2", "3", "4", "5" }));

            Assert.AreEqual(h.ValueCount, 5);
            Assert.AreEqual(h.KeyCount, 2);
        }


        [Test]
        public void TestRemove()
        {
            HashList<int, string> h = new HashList<int, string>();

            h.Add(2, "a");

            Assert.AreEqual(h.ValueCount, 1);
            Assert.AreEqual(h.KeyCount, 1);
            
            h.Add(4, new List<string>(new string[] { "2", "3", "4", "5" }));

            Assert.AreEqual(h.ValueCount, 5);
            Assert.AreEqual(h.KeyCount, 2);

            Assert.AreEqual(h.Remove(2), true);
            Assert.AreEqual(h.KeyCount, 1);
            Assert.AreEqual(h.ValueCount, 4);

            Assert.AreEqual(h.Remove(2), false);
            Assert.AreEqual(h.KeyCount, 1);
            Assert.AreEqual(h.ValueCount, 4);

            Assert.AreEqual(h.Remove("2"), true);

            Assert.AreEqual(h.KeyCount, 1);
            Assert.AreEqual(h.ValueCount, 3);

            Assert.AreEqual(h.Remove(3, "2"), false);

            Assert.AreEqual(h.KeyCount, 1);
            Assert.AreEqual(h.ValueCount, 3);

            Assert.AreEqual(h.Remove(4, "2"), false);

            Assert.AreEqual(h.KeyCount, 1);
            Assert.AreEqual(h.ValueCount, 3);

            Assert.AreEqual(h.Remove(4, "5"), true);

            Assert.AreEqual(h.KeyCount, 1);
            Assert.AreEqual(h.ValueCount, 2);

            h.Add(4, "4");

            Assert.AreEqual(h.KeyCount, 1);
            Assert.AreEqual(h.ValueCount, 3);

            h.RemoveAll("4");

            Assert.AreEqual(h.KeyCount, 1);
            Assert.AreEqual(h.ValueCount, 1);

        }

        [Test]
        public void TestGetValueEnumerator()
        {
            HashList<int, string> h = new HashList<int, string>();

            h.Add(2, "a");

            Assert.AreEqual(h.ValueCount, 1);
            Assert.AreEqual(h.KeyCount, 1);
                        
            h.Add(4, new List<string>(new string[] { "2", "3", "4", "5" }));

            Assert.AreEqual(h.ValueCount, 5);
            Assert.AreEqual(h.KeyCount, 2);

            IEnumerator<string> enumerator = h.GetValueEnumerator();

            List<string> list = new List<string>();

            while (enumerator.MoveNext())
            {
                list.Add(enumerator.Current);
            }

            Assert.AreEqual(list.Count, 5);
            Assert.AreEqual(list.Contains("a"), true);
            Assert.AreEqual(list.Contains("2"), true);
            Assert.AreEqual(list.Contains("3"), true);
            Assert.AreEqual(list.Contains("4"), true);
            Assert.AreEqual(list.Contains("5"), true);
        }
    }
}
