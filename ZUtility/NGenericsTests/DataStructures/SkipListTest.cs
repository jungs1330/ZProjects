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

namespace NGenericsTests
{
	[TestFixture]
	public class SkipListTest
	{
		[Test]
		public void TestSuccesfulInit()
		{
			SkipList<int, string> s = new SkipList<int, string>();

			Assert.AreEqual(s.Count, 0);
			Assert.AreEqual(s.Comparer, Comparer<int>.Default);

			s = new SkipList<int, string>(new ReverseComparer<int>(Comparer<int>.Default));
			
			Assert.AreEqual(s.Count, 0);
			Assert.AreEqual(typeof(ReverseComparer<int>).IsAssignableFrom(s.Comparer.GetType()), true);

			s = new SkipList<int, string>(14, 0.7, new ReverseComparer<int>(Comparer<int>.Default));
			Assert.AreEqual(s.Count, 0);
			Assert.AreEqual(typeof(ReverseComparer<int>).IsAssignableFrom(s.Comparer.GetType()), true);
			Assert.AreEqual(s.Probability, 0.7);
			Assert.AreEqual(s.MaxListLevel, 14);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestUnsuccesfullInit1()
		{
			SkipList<int, string> s = new SkipList<int, string>(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestUnsuccesfullInit2()
		{
			SkipList<int, string> s = new SkipList<int, string>(-1, 0.5, Comparer<int>.Default);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestUnsuccesfullInit3()
		{
			SkipList<int, string> s = new SkipList<int, string>(0, 0.5, Comparer<int>.Default);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestUnsuccesfullInit4()
		{
			SkipList<int, string> s = new SkipList<int, string>(2, 0.6, null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestUnsuccesfullInit5()
		{
			SkipList<int, string> s = new SkipList<int, string>(5, 0, Comparer<int>.Default);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestUnsuccesfullInit6()
		{
			SkipList<int, string> s = new SkipList<int, string>(0, 1, Comparer<int>.Default);
		}

		[Test]
		public void TestSequentialAdd()
		{
			SkipList<int, string> s = new SkipList<int, string>();

			for (int i = 0; i < 500; i++)
			{
				if ((i % 2) == 0)
				{
					s.Add(i, i.ToString());
				}
				else
				{
					s.Add(new KeyValuePair<int, string>(i, i.ToString()));
				}

				Assert.AreEqual(s.Count, i + 1);
				Assert.AreEqual(s[i], i.ToString());
			}
		}

		[Test]
		public void TestReverseAdd()
		{
			SkipList<int, string> s = new SkipList<int, string>();

			int counter = 0;

			for (int i = 499; i >= 0; i--)
			{
				if ((i % 2) == 0)
				{
					s.Add(i, i.ToString());
				}
				else
				{
					s.Add(new KeyValuePair<int, string>(i, i.ToString()));
				}

				counter++;

				Assert.AreEqual(s.Count, counter);
				Assert.AreEqual(s[i], i.ToString());
			}
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestInvalidItemGet1()
		{
			SkipList<int, string> s = new SkipList<int, string>();
			string v = s[10];
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestInvalidItemGet2()
		{
			SkipList<int, string> s = new SkipList<int, string>();
			s.Add(1, "aa");
			string v = s[2];
		}

		[Test]
		public void TestCompareTo()
		{
			SkipList<int, string> s1 = new SkipList<int, string>();
			s1.Add(1, "aa");
			s1.Add(2, "bb");

			SkipList<int, string> s2 = new SkipList<int, string>();
			s2.Add(1, "aa");

			Assert.AreEqual(s1.CompareTo(s2), 1);
			Assert.AreEqual(s2.CompareTo(s1), -1);
			Assert.AreEqual(s2.CompareTo(s2), 0);

			Assert.AreEqual(s1.CompareTo(new object()), -1);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestInvalidCompareTo()
		{
			SkipList<int, string> s = new SkipList<int, string>();
			s.CompareTo(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestInvalidItemset1()
		{
			SkipList<int, string> s = new SkipList<int, string>();
			s[10] = "2";
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestInvalidItemset2()
		{
			SkipList<int, string> s = new SkipList<int, string>();
			s.Add(1, "aa");
			s[10] = "2";
		}

		[Test]
		public void TestCount()
		{
			SkipList<int, string> s = new SkipList<int, string>();
			Assert.AreEqual(s.Count, 0);
			
			s.Add(2, "2");
			Assert.AreEqual(s.Count, 1);

			s.Add(3, "3");
			Assert.AreEqual(s.Count, 2);
		}



		[Test]
		public void TestRemove()
		{
			SkipList<int, string> s = new SkipList<int, string>();

			for (int i = 0; i < 100; i++)
			{
				s.Add(i, i.ToString());
			}

			for (int i = 0; i < 100; i++)
			{
				if ((i % 2) == 0)
				{
					Assert.AreEqual(s.Remove(i), true);
				}
				else
				{
					Assert.AreEqual(s.Remove(new KeyValuePair<int, string>(i, "a")), true);
				}

				Assert.AreEqual(s.Count, 99 - i);
				Assert.AreEqual(s.ContainsKey(i), false);
			}
		}

		[Test]
		public void TestIsReadOnly()
		{
			SkipList<int, string> s = new SkipList<int, string>();
			Assert.AreEqual(s.IsReadOnly, false);

			s.Add(4, "a");
			Assert.AreEqual(s.IsReadOnly, false);
		}

		[Test]
		public void TestIsFull()
		{
			SkipList<int, string> s = new SkipList<int, string>();
			Assert.AreEqual(s.IsFull, false);

			s.Add(4, "a");
			Assert.AreEqual(s.IsFull, false);
		}

		[Test]
		public void TestIsFixedSize()
		{
			SkipList<int, string> s = new SkipList<int, string>();
			Assert.AreEqual(s.IsFixedSize, false);

			s.Add(4, "a");
			Assert.AreEqual(s.IsFixedSize, false);
		}

		[Test]
		public void TestClear()
		{
			SkipList<int, string> s = new SkipList<int, string>();
			Assert.AreEqual(s.Count, 0);

			s.Add(4, "a");
			Assert.AreEqual(s.Count, 1);

			s.Clear();
			Assert.AreEqual(s.Count, 0);

			s.Add(5, "a");
			s.Add(6, "a");
			Assert.AreEqual(s.Count, 2);

			s.Clear();
			Assert.AreEqual(s.Count, 0);
		}

		[Test]
		public void TestIsEmpty()
		{
			SkipList<int, string> s = new SkipList<int, string>();
			Assert.AreEqual(s.IsEmpty, true);

			s.Add(4, "a");
			Assert.AreEqual(s.IsEmpty, false);

			s.Clear();
			Assert.AreEqual(s.IsEmpty, true);
		}

		[Test]
		public void TestGetKeys()
		{
			SkipList<int, string> s = new SkipList<int, string>();

			for (int i = 0; i < 100; i++)
			{
				s.Add(i, i.ToString());
			}

			List<int> list = new List<int>();
			list.AddRange(s.Keys);

			Assert.AreEqual(list.Count, 100);

			for (int i = 0; i < 100; i++)
			{
				Assert.AreEqual(list.Contains(i), true);
			}
		}

		[Test]
		public void TestGetValues()
		{
			SkipList<int, string> s = new SkipList<int, string>();

			for (int i = 0; i < 100; i++)
			{
				s.Add(i, i.ToString());
			}

			List<string> list = new List<string>();
			list.AddRange(s.Values);

			Assert.AreEqual(list.Count, 100);

			for (int i = 0; i < 100; i++)
			{
				Assert.AreEqual(list.Contains(i.ToString()), true);
			}
		}

		[Test]
		public void TestAccept()
		{
			SkipList<int, string> s = new SkipList<int, string>();

			for (int i = 0; i < 100; i++)
			{
				s.Add(i, i.ToString());
			}
						
			TrackingVisitor<KeyValuePair<int, string>> visitor = new TrackingVisitor<KeyValuePair<int, string>>();

			s.Accept(visitor);

			Assert.AreEqual(visitor.TrackingList.Count, 100);

			using (IEnumerator<KeyValuePair<int, string>> e = s.GetEnumerator())
			{
				while (e.MoveNext())
				{
					Assert.AreEqual(visitor.TrackingList.Contains(e.Current), true);
				}
			}
		}

		[Test]
		public void TestContainsKeyValuePair()
		{
			SkipList<int, string> s = new SkipList<int, string>();

			for (int i = 0; i < 100; i++)
			{
				s.Add(i, i.ToString());
			}

			Assert.AreEqual(s.Contains(new KeyValuePair<int, string>(5, "5")), true);
			Assert.AreEqual(s.Contains(new KeyValuePair<int, string>(6, "6")), true);

			Assert.AreEqual(s.Contains(new KeyValuePair<int, string>(5, "6")), false);
            Assert.AreEqual(s.Contains(new KeyValuePair<int, string>(100, "100")), false);
		}


		[Test]
		public void TestContainsKey()
		{
			SkipList<int, string> s = new SkipList<int, string>();

			for (int i = 0; i < 100; i++)
			{
				s.Add(i, i.ToString());
				Assert.AreEqual(s.ContainsKey(i), true);
			}

			for (int i = 100; i < 150; i++)
			{
				Assert.AreEqual(s.ContainsKey(i), false);
			}
		}

		[Test]
		public void TestEnumerator()
		{
			SkipList<int, string> s = new SkipList<int, string>();

			List<KeyValuePair<int, string>> originalList = new List<KeyValuePair<int, string>>();

			for (int i = 0; i < 100; i++)
			{
				originalList.Add(new KeyValuePair<int, string>(i, i.ToString()));
				s.Add(originalList[i]);
			}

			List<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>();

			using (IEnumerator<KeyValuePair<int, string>> e = s.GetEnumerator())
			{
				while (e.MoveNext())
				{
					list.Add(e.Current);
				}
			}

			Assert.AreEqual(list.Count, 100);

			for (int i = 0; i < 100; i++)
			{
				Assert.AreEqual(list.Contains(originalList[i]), true);
			}
		}

		[Test]
		public void TestInterfaceEnumerator()
		{
			SkipList<int, string> s = new SkipList<int, string>();

			List<KeyValuePair<int, string>> originalList = new List<KeyValuePair<int, string>>();

			for (int i = 0; i < 100; i++)
			{
				originalList.Add(new KeyValuePair<int, string>(i, i.ToString()));
				s.Add(originalList[i]);
			}

			List<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>();

			IEnumerator e = ((IEnumerable)s).GetEnumerator();
			{
				while (e.MoveNext())
				{
					list.Add((KeyValuePair<int, string>)e.Current);
				}
			}

			Assert.AreEqual(list.Count, 100);

			for (int i = 0; i < 100; i++)
			{
				Assert.AreEqual(list.Contains(originalList[i]), true);
			}
		}

		[Test]
		public void TestTryGetValue()
		{
			SkipList<int, string> s = new SkipList<int, string>();
			string val;
			Assert.AreEqual(s.TryGetValue(5, out val), false);

			s.Add(3, "4");
			Assert.AreEqual(s.TryGetValue(5, out val), false);
			Assert.AreEqual(s.TryGetValue(3, out val), true);
			Assert.AreEqual(val, "4");
		}

		[Test]
		public void TestSetItem()
		{
			SkipList<int, string> s = new SkipList<int, string>();
			s.Add(1, "1");

			s[1] = "2";

			Assert.AreEqual(s[1], "2");

			s.Add(2, "2");

			s[2] = "3";

			Assert.AreEqual(s[2], "3");
		}

		[Test]
		public void TestCurrentListLevel()
		{
			SkipList<int, string> s = new SkipList<int, string>();
			Assert.AreEqual(s.CurrentListLevel, 0);

			for (int i = 0; i < 100; i++)
			{
				s.Add(new KeyValuePair<int, string>(i, i.ToString()));				
			}

			Assert.Greater(s.CurrentListLevel, 0);
		}

		[Test]
		public void TestRemoveItemNotInList1()
		{
			SkipList<int, string> s = new SkipList<int, string>();
			Assert.AreEqual(s.Remove(4), false);
		}

		[Test]
		public void TestRemoveItemNotInList2()
		{
			SkipList<int, string> s = new SkipList<int, string>();
			Assert.AreEqual(s.Remove(new KeyValuePair<int, string>(3, "3")), false);
		}

		[Test]
		public void TestCopyTo()
		{
			SkipList<int, string> s = new SkipList<int, string>();

			KeyValuePair<int, string>[] pairs = new KeyValuePair<int, string>[5];

			for (int i = 0; i < pairs.Length; i++)
			{
				pairs[i] = new KeyValuePair<int, string>(i, i.ToString());
				s.Add(pairs[i]);
			}

			KeyValuePair<int, string>[] array = new KeyValuePair<int, string>[50];

			s.CopyTo(array, 0);

			List<KeyValuePair<int, string>> l = new List<KeyValuePair<int, string>>();
			l.Add(array[0]);
			l.Add(array[1]);
			l.Add(array[2]);
			l.Add(array[3]);
			l.Add(array[4]);
			l.Add(array[5]);

			for (int i = 0; i < pairs.Length; i++)
			{
				Assert.AreEqual(l.Contains(pairs[i]), true);
			}
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestInvalidCopyTo1()
		{
			SkipList<int, string> s = new SkipList<int, string>();

			KeyValuePair<int, string>[] pairs = new KeyValuePair<int, string>[5];

			for (int i = 0; i < pairs.Length; i++)
			{
				pairs[i] = new KeyValuePair<int, string>(i, i.ToString());
				s.Add(pairs[i]);
			}

			KeyValuePair<int, string>[] array = new KeyValuePair<int, string>[4];

			s.CopyTo(array, 0);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestInvalidCopyTo2()
		{
			SkipList<int, string> s = new SkipList<int, string>();

			KeyValuePair<int, string>[] pairs = new KeyValuePair<int, string>[5];

			for (int i = 0; i < pairs.Length; i++)
			{
				pairs[i] = new KeyValuePair<int, string>(i, i.ToString());
				s.Add(pairs[i]);
			}

			KeyValuePair<int, string>[] array = new KeyValuePair<int, string>[4];

			s.CopyTo(array, 0);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestInvalidCopyTo3()
		{
			SkipList<int, string> s = new SkipList<int, string>();

			KeyValuePair<int, string>[] pairs = new KeyValuePair<int, string>[5];

			for (int i = 0; i < pairs.Length; i++)
			{
				pairs[i] = new KeyValuePair<int, string>(i, i.ToString());
				s.Add(pairs[i]);
			}

			KeyValuePair<int, string>[] array = new KeyValuePair<int, string>[4];

			s.CopyTo(null, 0);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestAcceptNullVisitor()
		{
			SkipList<int, string> s = new SkipList<int, string>();
			s.Accept(null);
		}

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestDuplicateItem1()
        {
            SkipList<int, string> s = new SkipList<int, string>();

            for (int i = 0; i < 20; i++)
            {
                s.Add(i, i.ToString());
            }

            s.Add(5, "5");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestDuplicateItem2()
        {
            SkipList<int, string> s = new SkipList<int, string>();

            for (int i = 0; i < 20; i++)
            {
                s.Add(i, i.ToString());
            }

            s.Add(0, "0");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestDuplicateItem3()
        {
            SkipList<int, string> s = new SkipList<int, string>();

            for (int i = 0; i < 20; i++)
            {
                s.Add(i, i.ToString());
            }

            s.Add(19, "19");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestDuplicateItem4()
        {
            SkipList<int, string> s = new SkipList<int, string>();

            for (int i = 0; i < 20; i++)
            {
                s.Add(i, i.ToString());
            }

            s.Add(10, "15");
        }
    }
}
