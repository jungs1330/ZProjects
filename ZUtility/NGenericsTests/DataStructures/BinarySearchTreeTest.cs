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
using System.Collections;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using NGenerics.DataStructures;
using NGenerics.Comparers;
using NGenerics.Visitors;

namespace NGenericsTests.DataStructures
{
	[TestFixture]
	public class BinarySearchTreeTest
	{
		[Test]
		public void TestSuccesfulInit()
		{
			BinarySearchTree<int, string> tree = new BinarySearchTree<int, string>();

			Assert.AreEqual(tree.Count, 0);
			Assert.AreEqual(tree.Comparer, Comparer<int>.Default);

			tree = new BinarySearchTree<int, string>(new ReverseComparer<int>(Comparer<int>.Default));

			Assert.AreEqual(tree.Count, 0);
			Assert.AreEqual(tree.Comparer.GetType().IsAssignableFrom(typeof(ReverseComparer<int>)), true);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestNullComparer()
		{
			BinarySearchTree<int, string> tree = new BinarySearchTree<int, string>(null);
		}

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestDuplicateAdd()
        {
            BinarySearchTree<int, string> tree = new BinarySearchTree<int, string>();
            tree.Add(4, "4");
            tree.Add(4, "4");
        }


		[Test]
		public void TestAdd()
		{
			BinarySearchTree<int, string> tree = new BinarySearchTree<int, string>();

			Dictionary<int, string> dic = new Dictionary<int, string>();

			Random rand = new Random(Convert.ToInt32(DateTime.Now.Ticks % Int32.MaxValue));

			for (int i = 0; i < 50; i++)
			{
				int gen = rand.Next(2000);

				while (dic.ContainsKey(gen))
				{
					gen = rand.Next(2000);
				}

				dic.Add(gen, null);

				tree.Add(gen, gen.ToString());

				Assert.AreEqual(tree.Count, i + 1);
				Assert.AreEqual(tree.ContainsKey(gen), true);
			}
		}

        [Test]
        public void TestTryGetValue()
        {
            BinarySearchTree<int, string> tree = new BinarySearchTree<int, string>();

            Dictionary<int, string> dic = new Dictionary<int, string>();

            Random rand = new Random(Convert.ToInt32(DateTime.Now.Ticks % Int32.MaxValue));

            for (int i = 0; i < 50; i++)
            {
                int gen = rand.Next(2000);

                while (dic.ContainsKey(gen))
                {
                    gen = rand.Next(2000);
                }

                dic.Add(gen, null);

                tree.Add(gen, gen.ToString());

                string val;

                Assert.AreEqual(tree.Count, i + 1);

                tree.TryGetValue(gen, out val);
                Assert.AreEqual(val, gen.ToString());
                Assert.AreEqual(tree[gen], gen.ToString());
            }

            string val2;
            tree.TryGetValue(2001, out val2);
            Assert.AreEqual(val2, null);
        }

        [Test]
        public void TestSet()
        {
            BinarySearchTree<int, string> tree = new BinarySearchTree<int, string>();

            for (int i = 20; i > 10; i--)
            {
                tree.Add(i, i.ToString());
            }

            for (int i = 0; i < 10; i++)
            {
                tree.Add(i, i.ToString());
            }

            Assert.AreEqual(tree[0], "0");
            tree[0] = "1";
            Assert.AreEqual(tree[0], "1");

            tree[1] = "4";
            Assert.AreEqual(tree[1], "4");
        }

        [Test]
        public void TestKeys()
        {
            BinarySearchTree<int, string> tree = new BinarySearchTree<int, string>();

            for (int i = 20; i > 10; i--)
            {
                tree.Add(i, i.ToString());
            }

            for (int i = 0; i <= 10; i++)
            {
                tree.Add(i, i.ToString());
            }

            ICollection<int> keys = tree.Keys;

            for (int i = 0; i <= 20; i++)
            {
                Assert.AreEqual(keys.Contains(i), true);
            }

            Assert.AreEqual(keys.Count, 21); 
        }

        [Test]
        public void TestValues()
        {
            BinarySearchTree<int, string> tree = new BinarySearchTree<int, string>();

            for (int i = 20; i > 10; i--)
            {
                tree.Add(i, i.ToString());
            }

            for (int i = 0; i <= 10; i++)
            {
                tree.Add(i, i.ToString());
            }

            ICollection<string> values = tree.Values;

            for (int i = 0; i <= 20; i++)
            {
                Assert.AreEqual(values.Contains(i.ToString()), true);
            }

            Assert.AreEqual(values.Count, 21);
        }

        [Test]
        public void TestAddKeyValuePair()
        {
            BinarySearchTree<int, string> tree = new BinarySearchTree<int, string>();

            Dictionary<int, string> dic = new Dictionary<int, string>();

            Random rand = new Random(Convert.ToInt32(DateTime.Now.Ticks % Int32.MaxValue));

            for (int i = 0; i < 50; i++)
            {
                int gen = rand.Next(2000);

                while (dic.ContainsKey(gen))
                {
                    gen = rand.Next(2000);
                }

                dic.Add(gen, null);

                tree.Add(new KeyValuePair<int, string>(gen, gen.ToString()));

                Assert.AreEqual(tree.Count, i + 1);
                Assert.AreEqual(tree.ContainsKey(gen), true);
            }
        }

		[Test]
		public void TestContains()
		{
			BinarySearchTree<int, string> tree = new BinarySearchTree<int, string>();

			Assert.AreEqual(tree.ContainsKey(5), false);

			tree.Add(4, "4");
            Assert.AreEqual(tree[4], "4");
			Assert.AreEqual(tree.ContainsKey(4), true);
			Assert.AreEqual(tree.ContainsKey(5), false);

			tree.Add(6, "6");
            Assert.AreEqual(tree[6], "6");
			Assert.AreEqual(tree.ContainsKey(4), true);
			Assert.AreEqual(tree.ContainsKey(5), false);
			Assert.AreEqual(tree.ContainsKey(6), true);

			tree.Add(2, "2");
            Assert.AreEqual(tree[2], "2");
			Assert.AreEqual(tree.ContainsKey(2), true);
			Assert.AreEqual(tree.ContainsKey(4), true);
			Assert.AreEqual(tree.ContainsKey(5), false);
			Assert.AreEqual(tree.ContainsKey(6), true);

			tree.Add(5, "5");
            Assert.AreEqual(tree[5], "5");
			Assert.AreEqual(tree.ContainsKey(2), true);
			Assert.AreEqual(tree.ContainsKey(4), true);
			Assert.AreEqual(tree.ContainsKey(5), true);
			Assert.AreEqual(tree.ContainsKey(6), true);
		}


        [Test]
        public void TestKeyValuePairContains()
        {
            BinarySearchTree<int, string> tree = new BinarySearchTree<int, string>();

            Assert.AreEqual(tree.Contains(new KeyValuePair<int, string>(5, "5")), false);

            tree.Add(4, "4");
            Assert.AreEqual(tree.Contains(new KeyValuePair<int, string>(4, "4")), true);
            Assert.AreEqual(tree.Contains(new KeyValuePair<int, string>(4, "5")), false);

            tree.Add(6, "6");
            Assert.AreEqual(tree.Contains(new KeyValuePair<int, string>(6, "6")), true);
            Assert.AreEqual(tree.Contains(new KeyValuePair<int, string>(6, "5")), false);
        }

		[Test]
		public void TestRemove()
		{
			BinarySearchTree<int, string> tree = new BinarySearchTree<int, string>();

			tree.Add(4, "4");
			tree.Add(6, "6");
			tree.Add(2, "2");
			tree.Add(5, "5");
			tree.Add(19, "19");
			tree.Add(1, "1");

			Assert.AreEqual(tree.Count, 6);

			Assert.AreEqual(tree.ContainsKey(4), true);
			Assert.AreEqual(tree.ContainsKey(6), true);
			Assert.AreEqual(tree.ContainsKey(2), true);
			Assert.AreEqual(tree.ContainsKey(5), true);
			Assert.AreEqual(tree.ContainsKey(19), true);
			Assert.AreEqual(tree.ContainsKey(1), true);

			Assert.AreEqual(tree.Remove(20), false);

			Assert.AreEqual(tree.Remove(4), true);
			Assert.AreEqual(tree.Count, 5);

			Assert.AreEqual(tree.ContainsKey(4), false);
			Assert.AreEqual(tree.ContainsKey(6), true);
			Assert.AreEqual(tree.ContainsKey(2), true);
			Assert.AreEqual(tree.ContainsKey(5), true);
			Assert.AreEqual(tree.ContainsKey(19), true);
			Assert.AreEqual(tree.ContainsKey(1), true);

			Assert.AreEqual(tree.Remove(2), true);
			Assert.AreEqual(tree.Count, 4);

			Assert.AreEqual(tree.ContainsKey(4), false);
			Assert.AreEqual(tree.ContainsKey(6), true);
			Assert.AreEqual(tree.ContainsKey(2), false);
			Assert.AreEqual(tree.ContainsKey(5), true);
			Assert.AreEqual(tree.ContainsKey(19), true);
			Assert.AreEqual(tree.ContainsKey(1), true);

			Assert.AreEqual(tree.Remove(19), true);
			Assert.AreEqual(tree.Count, 3);

			Assert.AreEqual(tree.ContainsKey(4), false);
			Assert.AreEqual(tree.ContainsKey(6), true);
			Assert.AreEqual(tree.ContainsKey(2), false);
			Assert.AreEqual(tree.ContainsKey(5), true);
			Assert.AreEqual(tree.ContainsKey(19), false);
			Assert.AreEqual(tree.ContainsKey(1), true);

			Assert.AreEqual(tree.Remove(20), false);


			Assert.AreEqual(tree.Remove(1), true);
			Assert.AreEqual(tree.Count, 2);

			Assert.AreEqual(tree.ContainsKey(4), false);
			Assert.AreEqual(tree.ContainsKey(6), true);
			Assert.AreEqual(tree.ContainsKey(2), false);
			Assert.AreEqual(tree.ContainsKey(5), true);
			Assert.AreEqual(tree.ContainsKey(19), false);
			Assert.AreEqual(tree.ContainsKey(1), false);

			Assert.AreEqual(tree.Remove(6), true);
			Assert.AreEqual(tree.Count, 1);

			Assert.AreEqual(tree.ContainsKey(4), false);
			Assert.AreEqual(tree.ContainsKey(6), false);
			Assert.AreEqual(tree.ContainsKey(2), false);
			Assert.AreEqual(tree.ContainsKey(5), true);
			Assert.AreEqual(tree.ContainsKey(19), false);
			Assert.AreEqual(tree.ContainsKey(1), false);

			Assert.AreEqual(tree.Remove(5), true);
			Assert.AreEqual(tree.Count, 0);

			Assert.AreEqual(tree.ContainsKey(4), false);
			Assert.AreEqual(tree.ContainsKey(6), false);
			Assert.AreEqual(tree.ContainsKey(2), false);
			Assert.AreEqual(tree.ContainsKey(5), false);
			Assert.AreEqual(tree.ContainsKey(19), false);
			Assert.AreEqual(tree.ContainsKey(1), false);

			Assert.AreEqual(tree.Remove(1), false);
		}

        [Test]
        public void TestRemoveKeyValuePair()
        {
            BinarySearchTree<int, string> tree = new BinarySearchTree<int, string>();

            tree.Add(4, "4");
            tree.Add(6, "6");
            tree.Add(2, "2");
            tree.Add(5, "5");
            tree.Add(19, "19");
            tree.Add(1, "1");

            Assert.AreEqual(tree.Count, 6);

            Assert.AreEqual(tree.ContainsKey(4), true);
            Assert.AreEqual(tree.ContainsKey(6), true);
            Assert.AreEqual(tree.ContainsKey(2), true);
            Assert.AreEqual(tree.ContainsKey(5), true);
            Assert.AreEqual(tree.ContainsKey(19), true);
            Assert.AreEqual(tree.ContainsKey(1), true);

            Assert.AreEqual(tree.Remove(new KeyValuePair<int, string>(20, "20")), false);

            Assert.AreEqual(tree.Remove(new KeyValuePair<int, string>(4, "4")), true);
            Assert.AreEqual(tree.Count, 5);

            Assert.AreEqual(tree.ContainsKey(4), false);
            Assert.AreEqual(tree.ContainsKey(6), true);
            Assert.AreEqual(tree.ContainsKey(2), true);
            Assert.AreEqual(tree.ContainsKey(5), true);
            Assert.AreEqual(tree.ContainsKey(19), true);
            Assert.AreEqual(tree.ContainsKey(1), true);
        }

		[Test]
		public void TestAccept()
		{
			BinarySearchTree<int, string> tree = GetTestTree();
			TrackingVisitor<KeyValuePair<int, string>> visitor = new TrackingVisitor<KeyValuePair<int, string>>();

			tree.Accept(visitor);

			Assert.AreEqual(visitor.TrackingList.Contains(new KeyValuePair<int, string>(4,"4")), true);
            Assert.AreEqual(visitor.TrackingList.Contains(new KeyValuePair<int, string>(6, "6")), true);
            Assert.AreEqual(visitor.TrackingList.Contains(new KeyValuePair<int, string>(2, "2")), true);
            Assert.AreEqual(visitor.TrackingList.Contains(new KeyValuePair<int, string>(5, "5")), true);
            Assert.AreEqual(visitor.TrackingList.Contains(new KeyValuePair<int, string>(19, "19")), true);
            Assert.AreEqual(visitor.TrackingList.Contains(new KeyValuePair<int, string>(1, "1")), true);

			Assert.AreEqual(visitor.TrackingList.Count, 6);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestNullVisitor()
		{
			BinarySearchTree<int, string> tree = new BinarySearchTree<int, string>();
			tree.Accept(null);
		}

		[Test]
		public void TestClear()
		{
			BinarySearchTree<int, string> tree = new BinarySearchTree<int, string>();
			tree.Clear();

			Assert.AreEqual(tree.Count, 0);

			tree = GetTestTree();
			Assert.AreEqual(tree.ContainsKey(19), true);

			tree.Clear();
			Assert.AreEqual(tree.Count, 0);
			Assert.AreEqual(tree.ContainsKey(19), false);
		}

		[Test]
		public void TestCompareTo()
		{
			BinarySearchTree<int, string> tree1 = new BinarySearchTree<int, string>();
			BinarySearchTree<int, string> tree2 = GetTestTree();

			Assert.AreEqual(tree1.CompareTo(tree2), -1);
			Assert.AreEqual(tree2.CompareTo(tree1), 1);

			Assert.AreEqual(tree1.CompareTo(tree1), 0);
			Assert.AreEqual(tree2.CompareTo(tree2), 0);

			Assert.AreEqual(tree2.CompareTo(new object()), -1);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestNullCompareTo()
		{
			BinarySearchTree<int, string> tree1 = new BinarySearchTree<int, string>();
			tree1.CompareTo(null);
		}


		[Test]
		public void TestIsEmpty()
		{
			BinarySearchTree<int, string> tree = new BinarySearchTree<int, string>();
			Assert.AreEqual(tree.IsEmpty, true);

			tree = GetTestTree();
			Assert.AreEqual(tree.IsEmpty, false);

			tree.Clear();
			Assert.AreEqual(tree.IsEmpty, true);
		}

		[Test]
		public void TestIsFixedSize()
		{
			BinarySearchTree<int, string> tree = new BinarySearchTree<int, string>();
			Assert.AreEqual(tree.IsFixedSize, false);

			tree = GetTestTree();
			Assert.AreEqual(tree.IsFixedSize, false);
		}

		[Test]
		public void TestIsFull()
		{
			BinarySearchTree<int, string> tree = new BinarySearchTree<int, string>();
			Assert.AreEqual(tree.IsFull, false);

			tree = GetTestTree();
			Assert.AreEqual(tree.IsFull, false);
		}

		[Test]
		public void TestIsReadonly()
		{
			BinarySearchTree<int, string> tree = new BinarySearchTree<int, string>();
			Assert.AreEqual(tree.IsReadOnly, false);

			tree = GetTestTree();
			Assert.AreEqual(tree.IsReadOnly, false);
		}

		[Test]
		public void TestEnumerator()
		{
			BinarySearchTree<int, string> tree = GetTestTree();
			IEnumerator<KeyValuePair<int, string>> enumerator = tree.GetEnumerator();

			List<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>();

			while (enumerator.MoveNext())
			{
				list.Add(enumerator.Current);
			}

			Assert.AreEqual(list.Contains(new KeyValuePair<int, string>(4, "4")), true);
            Assert.AreEqual(list.Contains(new KeyValuePair<int, string>(6, "6")), true);
            Assert.AreEqual(list.Contains(new KeyValuePair<int, string>(2, "2")), true);
            Assert.AreEqual(list.Contains(new KeyValuePair<int, string>(5, "5")), true);
            Assert.AreEqual(list.Contains(new KeyValuePair<int, string>(19, "19")), true);
            Assert.AreEqual(list.Contains(new KeyValuePair<int, string>(1, "1")), true);

			Assert.AreEqual(list.Count, 6);
		}

        [Test]
        public void TestSortedEnumerator()
        {
            BinarySearchTree<int, string> tree = GetTestTree();
            IEnumerator<KeyValuePair<int, string>> enumerator = tree.GetSortedEnumerator();

            List<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>();

            while (enumerator.MoveNext())
            {
                list.Add(enumerator.Current);
            }

            Assert.AreEqual(list.Count, 6);

            Assert.AreEqual(list[0], new KeyValuePair<int, string>(1, "1"));
            Assert.AreEqual(list[1], new KeyValuePair<int, string>(2, "2"));
            Assert.AreEqual(list[2], new KeyValuePair<int, string>(4, "4"));
            Assert.AreEqual(list[3], new KeyValuePair<int, string>(5, "5"));
            Assert.AreEqual(list[4], new KeyValuePair<int, string>(6, "6"));
            Assert.AreEqual(list[5], new KeyValuePair<int, string>(19, "19"));
        }

		[Test]
		public void TestInterfaceEnumerator()
		{
			BinarySearchTree<int, string> tree = GetTestTree();
			IEnumerator enumerator = ((IEnumerable)tree).GetEnumerator();

			List<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>();

			while (enumerator.MoveNext())
			{
				list.Add((KeyValuePair<int, string>)enumerator.Current);
			}

            Assert.AreEqual(list.Contains(new KeyValuePair<int, string>(4, "4")), true);
            Assert.AreEqual(list.Contains(new KeyValuePair<int, string>(6, "6")), true);
            Assert.AreEqual(list.Contains(new KeyValuePair<int, string>(2, "2")), true);
            Assert.AreEqual(list.Contains(new KeyValuePair<int, string>(5, "5")), true);
            Assert.AreEqual(list.Contains(new KeyValuePair<int, string>(19, "19")), true);
            Assert.AreEqual(list.Contains(new KeyValuePair<int, string>(1, "1")), true);

			Assert.AreEqual(list.Count, 6);
		}

		[Test]
		public void TestCopyTo()
		{
			BinarySearchTree<int, string> t = GetTestTree();

			KeyValuePair<int, string>[] array = new KeyValuePair<int, string>[6];
			t.CopyTo(array, 0);

			List<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>(array);

            Assert.AreEqual(list.Contains(new KeyValuePair<int, string>(4, "4")), true);
            Assert.AreEqual(list.Contains(new KeyValuePair<int, string>(6, "6")), true);
            Assert.AreEqual(list.Contains(new KeyValuePair<int, string>(2, "2")), true);
            Assert.AreEqual(list.Contains(new KeyValuePair<int, string>(5, "5")), true);
            Assert.AreEqual(list.Contains(new KeyValuePair<int, string>(19, "19")), true);
            Assert.AreEqual(list.Contains(new KeyValuePair<int, string>(1, "1")), true);

			Assert.AreEqual(list.Count, 6);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestNullCopyTo()
		{
			BinarySearchTree<int, string> t = GetTestTree();
			t.CopyTo(null, 1);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestInvalidCopyTo1()
		{
			BinarySearchTree<int, string> t = GetTestTree();
			KeyValuePair<int, string>[] array = new KeyValuePair<int, string>[5];
			t.CopyTo(array, 0);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestInvalidCopyTo2()
		{
			BinarySearchTree<int, string> t = GetTestTree();
			KeyValuePair<int, string>[] array = new KeyValuePair<int, string>[6];
			t.CopyTo(array, 1);
		}

		[Test]
		public void TestMin()
		{
			BinarySearchTree<int, string> tree = GetTestTree();
			KeyValuePair<int, string> i = tree.Min;

			Assert.AreEqual(i.Key, 1);
            Assert.AreEqual(i.Value, "1");
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void TestInvalidMin()
		{
			BinarySearchTree<int, string> tree = new BinarySearchTree<int, string>();
            KeyValuePair<int, string> i = tree.Min;
		}

		[Test]
		public void TestMax()
		{
			BinarySearchTree<int, string> tree = GetTestTree();
            KeyValuePair<int, string> i = tree.Max;

			Assert.AreEqual(i.Key, 19);
            Assert.AreEqual(i.Value, "19");
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void TestInvalidMax()
		{
			BinarySearchTree<int, string> tree = new BinarySearchTree<int, string>();
            KeyValuePair<int, string> i = tree.Max;
		}

		private BinarySearchTree<int, string> GetTestTree()
		{
			BinarySearchTree<int, string> tree = new BinarySearchTree<int, string>();

			tree.Add(4, "4");
			tree.Add(6, "6");
			tree.Add(2, "2");
			tree.Add(5, "5");
			tree.Add(19, "19");
			tree.Add(1, "1");

			return tree;
		}
	}
}
