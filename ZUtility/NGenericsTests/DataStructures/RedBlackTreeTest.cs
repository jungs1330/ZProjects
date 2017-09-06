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
using NGenerics.Comparers;

using NUnit.Framework;
using NGenerics.Visitors;
using System.Collections;

namespace NGenericsTests.DataStructures
{
	[TestFixture]
	public class RedBlackTreeTest
	{
		[Test]
		public void TestSuccesfulInit()
		{
			RedBlackTree<int, string> tree = new RedBlackTree<int, string>();
			Assert.AreEqual(tree.Count, 0);
			Assert.AreEqual(tree.Comparer, Comparer<int>.Default);

			tree = new RedBlackTree<int, string>(new ReverseComparer<int>());
			Assert.AreEqual(tree.Count, 0);
			Assert.AreEqual(tree.Comparer.GetType().IsAssignableFrom(typeof(ReverseComparer<int>)), true);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestNullComparer()
		{
			RedBlackTree<int, string> tree = new RedBlackTree<int, string>(null);
		}

		[Test]
		public void TestIsReadOnly()
		{
			RedBlackTree<int, string> tree = new RedBlackTree<int, string>();
			Assert.AreEqual(tree.IsReadOnly, false);
		}

        [Test]
        public void TestClear()
        {
            RedBlackTree<int, string> tree = GetTestTree();
            tree.Clear();
            
            Assert.AreEqual(tree.Count, 0);
            Assert.AreEqual(tree.ContainsKey(40), false);
            Assert.AreEqual(tree.ContainsKey(41), false);
        }

        [Test]
        public void TestIsEmpty()
        {
            RedBlackTree<int, string> tree = new RedBlackTree<int, string>();
            Assert.AreEqual(tree.IsEmpty, true);

            tree = GetTestTree();
            Assert.AreEqual(tree.IsEmpty, false);

            tree.Clear();
            Assert.AreEqual(tree.IsEmpty, true);
        }

        [Test]
        public void TestIsFixedSize()
        {
            RedBlackTree<int, string> tree = new RedBlackTree<int, string>();
            Assert.AreEqual(tree.IsFixedSize, false);

            tree = GetTestTree();
            Assert.AreEqual(tree.IsFixedSize, false);

            tree.Clear();
            Assert.AreEqual(tree.IsFixedSize, false);
        }

        [Test]
        public void TestIsFull()
        {
            RedBlackTree<int, string> tree = new RedBlackTree<int, string>();
            Assert.AreEqual(tree.IsFull, false);

            tree = GetTestTree();
            Assert.AreEqual(tree.IsFull, false);

            tree.Clear();
            Assert.AreEqual(tree.IsFull, false);
        }

		[Test]
		public void TestAdd()
		{
			RedBlackTree<int, string> tree = new RedBlackTree<int, string>();

			for (int i = 0; i < 100; i++)
			{
				tree.Add(i, i.ToString());

				Assert.AreEqual(tree.ContainsKey(i), true);
				Assert.AreEqual(tree.Count, i + 1);
			}

			for (int i = 300; i > 200; i--)
			{
				tree.Add(i, i.ToString());

				Assert.AreEqual(tree.ContainsKey(i), true);
				Assert.AreEqual(tree.Count, 100 + (300 - i) + 1);
			}

			for (int i = 100; i < 200; i++)
			{
				tree.Add(i, i.ToString());

				Assert.AreEqual(tree.ContainsKey(i), true);
				Assert.AreEqual(tree.Count, 100 + i + 1);
			}
		}

        [Test]
        public void TestCompareTo()
        {
            RedBlackTree<int, string> tree1 = GetTestTree();
            RedBlackTree<int, string> tree2 = new RedBlackTree<int, string>();

            Assert.AreEqual(tree1.CompareTo(tree2), 1);
            Assert.AreEqual(tree2.CompareTo(tree1), -1);
            Assert.AreEqual(tree1.CompareTo(tree1), 0);

            Assert.AreEqual(tree1.CompareTo(new object()), -1);
        }

        [Test]
        public void TestEnumerator()
        {
            List<KeyValuePair<int, string>> l = new List<KeyValuePair<int, string>>();
            RedBlackTree<int, string> t = GetTestTree();

            using (IEnumerator<KeyValuePair<int, string>> e = t.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    l.Add(e.Current);
                }
           
            }
        }

        [Test]
        public void TestAccept()
        {
            RedBlackTree<int, string> tree = GetTestTree();

            TrackingVisitor<KeyValuePair<int, string>> visitor = new TrackingVisitor<KeyValuePair<int, string>>();

            tree.Accept(visitor);

            Assert.AreEqual(visitor.TrackingList.Count, 100);

            for (int i = 0; i < 100; i++)
            {
                Assert.AreEqual(visitor.TrackingList.Contains(new KeyValuePair<int, string>(i, i.ToString())), true);
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullVisitor()
        {
            RedBlackTree<int, string> tree = GetTestTree();
            tree.Accept(null);
        }

        [Test]
        public void StressTestRandomData() {
            List<int> data = new List<int>();
            RedBlackTree<int, string> tree = new RedBlackTree<int, string>();

            Random rand = new Random(System.Convert.ToInt32(System.DateTime.Now.Ticks % Int32.MaxValue));

            for (int i = 0; i < 2000; i++)
            {
                int randomNumber = rand.Next(100000);

                while (data.Contains(randomNumber))
                {
                    randomNumber = rand.Next(100000);
                }
                 
                data.Add(randomNumber);
                tree.Add(randomNumber, randomNumber.ToString());

                Assert.AreEqual(tree.Count, i + 1);
                
                for (int j = 0; j < data.Count; j++)
                {
                    Assert.AreEqual(tree.ContainsKey(data[j]), true);
                }
            }

            while (data.Count != 0) 
            {
                Assert.AreEqual(tree.Remove(data[0]), true);

                Assert.AreEqual(tree.ContainsKey(data[0]), false);

                data.RemoveAt(0);
                
                Assert.AreEqual(tree.Count, data.Count);

                for (int j = 0; j < data.Count; j++)
                {
                    Assert.AreEqual(tree.ContainsKey(data[j]), true);
                }
            }
        }

        [Test]
        public void TestGetEnumerator()
        {
            RedBlackTree<int, string> tree = GetTestTree();
            List<KeyValuePair<int, string>> l = new List<KeyValuePair<int, string>>();

            using (IEnumerator<KeyValuePair<int, string>> enumerator = tree.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    l.Add(enumerator.Current);
                }
            }

            Assert.AreEqual(l.Count, 100);

            for (int i = 0; i < 100; i++)
            {
                Assert.AreEqual(l.Contains(new KeyValuePair<int, string>(i, i.ToString())), true);
            }
        }

        [Test]
        public void TestInterfaceGetEnumerator()
        {
            RedBlackTree<int, string> tree = GetTestTree();
            List<KeyValuePair<int, string>> l = new List<KeyValuePair<int, string>>();

            IEnumerator enumerator = ((IEnumerable)tree).GetEnumerator();
            {
                while (enumerator.MoveNext())
                {
                    l.Add((KeyValuePair<int, string>)enumerator.Current);
                }
            }

            Assert.AreEqual(l.Count, 100);

            for (int i = 0; i < 100; i++)
            {
                Assert.AreEqual(l.Contains(new KeyValuePair<int, string>(i, i.ToString())), true);
            }
        }

        

        [Test]
        public void TestCount()
        {
            RedBlackTree<int, string> tree = GetTestTree();
            Assert.AreEqual(tree.IsEmpty, false);

            tree.Clear();
            Assert.AreEqual(tree.IsEmpty, true);

            tree = new RedBlackTree<int, string>();
            Assert.AreEqual(tree.IsEmpty, true);
        }

        [Test]
        public void TestSmallRemove()
        {
            RedBlackTree<int, string> tree = GetTestTree(5);

            for (int i = 0; i < 5; i++)
            {
                Assert.AreEqual(tree.Remove(i), true);
                Assert.AreEqual(tree.Count, 5 - i - 1);
                Assert.AreEqual(tree.ContainsKey(i), false);

                for (int j = i + 1; j < 5; j++)
                {
                    Assert.AreEqual(tree.ContainsKey(j), true);
                }
            }
        }

        
        [Test]
        public void TestBigRemove()
        {
            RedBlackTree<int, string> tree = GetTestTree();

            for (int i = 0; i < 100; i++)
            {
                Assert.AreEqual(tree.Remove(i), true);
                Assert.AreEqual(tree.Count, 100 - i - 1);
                Assert.AreEqual(tree.ContainsKey(i), false);

                for (int j = i + 1; j < 100; j++)
                {
                    Assert.AreEqual(tree.ContainsKey(j), true);
                }
            }
        }

        [Test]
        public void TestCopyTo()
        {
            RedBlackTree<int, string> t = GetTestTree(10);

            KeyValuePair<int, string>[] array = new KeyValuePair<int, string>[10];
            t.CopyTo(array, 0);

            List<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>(array);

            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(list.Contains(new KeyValuePair<int, string>(i, i.ToString())), true);
            }

            Assert.AreEqual(list.Count, 10);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullCopyTo()
        {
            RedBlackTree<int, string> t = GetTestTree(10);
            t.CopyTo(null, 1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidCopyTo1()
        {
            RedBlackTree<int, string> t = GetTestTree(10);
            KeyValuePair<int, string>[] array = new KeyValuePair<int, string>[9];
            t.CopyTo(array, 0);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidCopyTo2()
        {
            RedBlackTree<int, string> t = GetTestTree(10);
            KeyValuePair<int, string>[] array = new KeyValuePair<int, string>[10];
            t.CopyTo(array, 1);
        }

        [Test]
        public void TestMin()
        {
            RedBlackTree<int, string> tree = GetTestTree(20);
            Assert.AreEqual(tree.Min, 0);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestInvalidMin()
        {
            RedBlackTree<int, string> tree = new RedBlackTree<int, string>();
            int i = tree.Min;
        }

        [Test]
        public void TestMax()
        {
            RedBlackTree<int, string> tree = GetTestTree(20);
            Assert.AreEqual(tree.Max, 19);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestInvalidMax()
        {
            RedBlackTree<int, string> tree = new RedBlackTree<int, string>();
            int i = tree.Max;
        }

        [Test]
        public void TestRemoveNotFound()
        {
            RedBlackTree<int, string> tree = GetTestTree(20);

            for (int i = 20; i < 40; i++)
            {
                Assert.AreEqual(tree.Remove(i), false);
                Assert.AreEqual(tree.Count, 20);
            }
        }

        [Test]
        public void TestIndexing()
        {
            RedBlackTree<int, string> tree = GetTestTree(20);

            for (int i = 0; i < 20; i++)
            {
                Assert.AreEqual(tree[i], i.ToString());
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidIndexing()
        {
            RedBlackTree<int, string> tree = GetTestTree(20);

            for (int i = 0; i < 20; i++)
            {
                Assert.AreEqual(tree[i], i.ToString());
            }


            string s = tree[20];
        }

        [Test]
        public void TestKeys()
        {
            RedBlackTree<int, string> tree = GetTestTree(20);
            ICollection<int> keys = tree.Keys;

            Assert.AreEqual(keys.Count, 20);

            int counter = 0;

            using (IEnumerator<int> enumerator = keys.GetEnumerator()) {
                while (enumerator.MoveNext())
                {
                    Assert.AreEqual(enumerator.Current, counter);
                    counter++;
                }
            }

            tree = new RedBlackTree<int, string>();
            keys = tree.Keys;

            Assert.AreNotEqual(keys, null);
            Assert.AreEqual(keys.Count, 0);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestDepthFirstTraversalNullVisitor()
        {
            RedBlackTree<int, string> tree = GetTestTree(20);
            tree.DepthFirstTraversal(null);
        }

        [Test]
        public void TestValues()
        {
            RedBlackTree<int, string> tree = GetTestTree(20);
            ICollection<string> values = tree.Values;

            Assert.AreEqual(values.Count, 20);

            int counter = 0;

            using (IEnumerator<string> enumerator = values.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    Assert.AreEqual(enumerator.Current, counter.ToString());
                    counter++;
                }
            }

            tree = new RedBlackTree<int, string>();
            values = tree.Values;

            Assert.AreNotEqual(values, null);
            Assert.AreEqual(values.Count, 0);
        }

        private RedBlackTree<int, string> GetTestTree()
        {
            RedBlackTree<int, string> tree = new RedBlackTree<int, string>();

            for (int i = 0; i < 100; i++)
            {
                tree.Add(i, i.ToString());
            }

            return tree;
        }

        private RedBlackTree<int, string> GetTestTree(int noOfItems)
        {
            RedBlackTree<int, string> tree = new RedBlackTree<int, string>();

            for (int i = 0; i < noOfItems; i++)
            {
                tree.Add(i, i.ToString());
            }

            return tree;
        }

	}
}
