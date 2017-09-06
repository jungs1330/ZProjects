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
using NGenerics.Sorting;

namespace NGenericsTests
{
	[TestFixture]
	public class TestSort
	{
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestBubbleSortComparisonNullList()
        {
            BubbleSorter<int> sorter = new BubbleSorter<int>();
            sorter.Sort(null, new Comparison<int>(IntComparison));
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestBubbleSortComparisonNullComparison2()
        {
            BubbleSorter<int> sorter = new BubbleSorter<int>();

            List<int> list = GetReverseSequentialTestList();
            sorter.Sort(list, (Comparison<int>)null);
        }

        [Test]
        public void TestBubbleSortComparisonNullComparison1()
        {
            BubbleSorter<int> sorter = new BubbleSorter<int>();

            List<int> list = GetReverseSequentialTestList();
            sorter.Sort(list, new Comparison<int>(IntComparison));
            AssertGeneralTestListSorted(list);
        }

        private int IntComparison(int i, int j)
        {
            if (i < j)
            {
                return -1;
            }
            else if (i > j)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

		[Test]
		public void TestBubbleSort()
		{
			BubbleSorter<int> sorter = new BubbleSorter<int>();
			TestSorter(sorter);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestBubbleSortNullList1()
		{
			BubbleSorter<int> sorter = new BubbleSorter<int>();
			sorter.Sort(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestBubbleSortNullList2()
		{
			BubbleSorter<int> sorter = new BubbleSorter<int>();
			sorter.Sort(null, Comparer<int>.Default);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestBubbleSortNullList3()
		{
			BubbleSorter<int> sorter = new BubbleSorter<int>();
			sorter.Sort(null, SortOrder.Ascending);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestBubbleSortNullComparer1()
		{
			BubbleSorter<int> sorter = new BubbleSorter<int>();
			sorter.Sort(new List<int>(), (IComparer<int>)null);
		}

		[Test]
		public void TestOddEvenTransportSort()
		{
			OddEvenTransportSorter<int> sorter = new OddEvenTransportSorter<int>();
			TestSorter(sorter);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestOddEvenTransportSortNullList1()
		{
			OddEvenTransportSorter<int> sorter = new OddEvenTransportSorter<int>();
			sorter.Sort(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestOddEvenTransportSortNullList2()
		{
			OddEvenTransportSorter<int> sorter = new OddEvenTransportSorter<int>();
			sorter.Sort(null, Comparer<int>.Default);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestOddEvenTransportSortNullList3()
		{
			OddEvenTransportSorter<int> sorter = new OddEvenTransportSorter<int>();
			sorter.Sort(null, SortOrder.Ascending);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestOddEvenTransportSortNullComparer1()
		{
			OddEvenTransportSorter<int> sorter = new OddEvenTransportSorter<int>();
			sorter.Sort(new List<int>(), (IComparer<int>)null);
		}

		[Test]
		public void TestCombSort()
		{
			CombSorter<int> sorter = new CombSorter<int>();
			TestSorter(sorter);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestCombSortNullList1()
		{
			CombSorter<int> sorter = new CombSorter<int>();
			sorter.Sort(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestCombSortNullList2()
		{
			CombSorter<int> sorter = new CombSorter<int>();
			sorter.Sort(null, Comparer<int>.Default);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestCombSortNullList3()
		{
			CombSorter<int> sorter = new CombSorter<int>();
			sorter.Sort(null, SortOrder.Ascending);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestCombSortNullComparer1()
		{
			CombSorter<int> sorter = new CombSorter<int>();
			sorter.Sort(new List<int>(), (IComparer<int>)null);
		}

		[Test]
		public void TestInsertionSorter()
		{
			InsertionSorter<int> sorter = new InsertionSorter<int>();
			TestSorter(sorter);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestInsertionSortNullList1()
		{
			InsertionSorter<int> sorter = new InsertionSorter<int>();
			sorter.Sort(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestInsertionSortNullList2()
		{
			InsertionSorter<int> sorter = new InsertionSorter<int>();
			sorter.Sort(null, Comparer<int>.Default);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestInsertionSortNullList3()
		{
			InsertionSorter<int> sorter = new InsertionSorter<int>();
			sorter.Sort(null, SortOrder.Ascending);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestInsertionSortNullComparer1()
		{
			InsertionSorter<int> sorter = new InsertionSorter<int>();
			sorter.Sort(new List<int>(), (IComparer<int>)null);
		}

		[Test]
		public void TestSelectionSorter()
		{
			SelectionSorter<int> sorter = new SelectionSorter<int>();
			TestSorter(sorter);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestSelectionSortNullList1()
		{
			SelectionSorter<int> sorter = new SelectionSorter<int>();
			sorter.Sort(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestSelectionSortNullList2()
		{
			SelectionSorter<int> sorter = new SelectionSorter<int>();
			sorter.Sort(null, Comparer<int>.Default);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestSelectionSortNullList3()
		{
			SelectionSorter<int> sorter = new SelectionSorter<int>();
			sorter.Sort(null, SortOrder.Ascending);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestSelectionSortNullComparer1()
		{
			SelectionSorter<int> sorter = new SelectionSorter<int>();
			sorter.Sort(new List<int>(), (IComparer<int>)null);
		}

		[Test]
		public void TestHeapSorter()
		{
			HeapSorter<int> sorter = new HeapSorter<int>();
			TestSorter(sorter);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestHeapSortNullList1()
		{
			HeapSorter<int> sorter = new HeapSorter<int>();
			sorter.Sort(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestHeapSortNullList2()
		{
			HeapSorter<int> sorter = new HeapSorter<int>();
			sorter.Sort(null, Comparer<int>.Default);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestHeapSortNullList3()
		{
			HeapSorter<int> sorter = new HeapSorter<int>();
			sorter.Sort(null, SortOrder.Ascending);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestHeapSortNullComparer1()
		{
			HeapSorter<int> sorter = new HeapSorter<int>();
			sorter.Sort(new List<int>(), (IComparer<int>)null);
		}

		[Test]
		public void TestGnomeSorter()
		{
			GnomeSorter<int> sorter = new GnomeSorter<int>();
			TestSorter(sorter);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestGnomeSortNullList1()
		{
			GnomeSorter<int> sorter = new GnomeSorter<int>();
			sorter.Sort(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestGnomeSortNullList2()
		{
			GnomeSorter<int> sorter = new GnomeSorter<int>();
			sorter.Sort(null, Comparer<int>.Default);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestGnomeSortNullList3()
		{
			GnomeSorter<int> sorter = new GnomeSorter<int>();
			sorter.Sort(null, SortOrder.Ascending);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestGnomeSortNullComparer1()
		{
			GnomeSorter<int> sorter = new GnomeSorter<int>();
			sorter.Sort(new List<int>(), (IComparer<int>)null);
		}

		[Test]
		public void TestMergeSorter()
		{
			MergeSorter<int> sorter = new MergeSorter<int>();
			TestSorter(sorter);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestMergeSortNullList1()
		{
			MergeSorter<int> sorter = new MergeSorter<int>();
			sorter.Sort(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestMergeSortNullList2()
		{
			MergeSorter<int> sorter = new MergeSorter<int>();
			sorter.Sort(null, Comparer<int>.Default);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestMergeSortNullList3()
		{
			MergeSorter<int> sorter = new MergeSorter<int>();
			sorter.Sort(null, SortOrder.Ascending);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestMergeSortNullComparer1()
		{
			MergeSorter<int> sorter = new MergeSorter<int>();
			sorter.Sort(new List<int>(), (IComparer<int>)null);
		}

		[Test]
		public void TestQuickSorter()
		{
			QuickSorter<int> sorter = new QuickSorter<int>();
			TestSorter(sorter);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestQuickSortNullList1()
		{
			QuickSorter<int> sorter = new QuickSorter<int>();
			sorter.Sort(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestQuickSortNullList2()
		{
			QuickSorter<int> sorter = new QuickSorter<int>();
			sorter.Sort(null, Comparer<int>.Default);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestQuickSortNullList3()
		{
			QuickSorter<int> sorter = new QuickSorter<int>();
			sorter.Sort(null, SortOrder.Ascending);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestQuickSortNullComparer1()
		{
			QuickSorter<int> sorter = new QuickSorter<int>();
			sorter.Sort(new List<int>(), (IComparer<int>)null);
		}

		[Test]
		public void TestBucketSorter()
		{
			BucketSorter sorter = new BucketSorter(500);
			TestSorter(sorter);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestBucketSortNullList1()
		{
			BucketSorter sorter = new BucketSorter(500);
			sorter.Sort(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestBucketSortNullList2()
		{
			BucketSorter sorter = new BucketSorter(500);
			sorter.Sort(null, Comparer<int>.Default);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestBucketSortNullList3()
		{
			BucketSorter sorter = new BucketSorter(500);
			sorter.Sort(null, SortOrder.Ascending);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestBucketSortNullComparer1()
		{
			BucketSorter sorter = new BucketSorter(500);
			sorter.Sort(new List<int>(), (IComparer<int>)null);
		}

		[Test]
		public void TestShellSorter()
		{
			ShellSorter<int> sorter = new ShellSorter<int>();
			TestSorter(sorter);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestShellSortNullList1()
		{
			ShellSorter<int> sorter = new ShellSorter<int>();
			sorter.Sort(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestShellSortNullList2()
		{
			ShellSorter<int> sorter = new ShellSorter<int>();
			sorter.Sort(null, Comparer<int>.Default);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestShellSortNullList3()
		{
			ShellSorter<int> sorter = new ShellSorter<int>();
			sorter.Sort(null, SortOrder.Ascending);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestShellSortNullComparer1()
		{
			ShellSorter<int> sorter = new ShellSorter<int>();
			sorter.Sort(new List<int>(), (IComparer<int>)null);
		}

		[Test]
		public void TestShakerSorter()
		{
			ShakerSorter<int> sorter = new ShakerSorter<int>();
			TestSorter(sorter);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestShakerSortNullList1()
		{
			ShakerSorter<int> sorter = new ShakerSorter<int>();
			sorter.Sort(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestShakerSortNullList2()
		{
			ShakerSorter<int> sorter = new ShakerSorter<int>();
			sorter.Sort(null, Comparer<int>.Default);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestShakerSortNullList3()
		{
			ShakerSorter<int> sorter = new ShakerSorter<int>();
			sorter.Sort(null, SortOrder.Ascending);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestShakerSortNullComparer1()
		{
			ShakerSorter<int> sorter = new ShakerSorter<int>();
			sorter.Sort(new List<int>(), (IComparer<int>)null);
		}

		[Test]
		public void TestCocktailSorter()
		{
			CocktailSorter<int> sorter = new CocktailSorter<int>();
			TestSorter(sorter);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestCocktailSortNullList1()
		{
			CocktailSorter<int> sorter = new CocktailSorter<int>();
			sorter.Sort(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestCocktailSortNullList2()
		{
			CocktailSorter<int> sorter = new CocktailSorter<int>();
			sorter.Sort(null, Comparer<int>.Default);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestCocktailSortNullList3()
		{
			CocktailSorter<int> sorter = new CocktailSorter<int>();
			sorter.Sort(null, SortOrder.Ascending);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestCocktailSortNullComparer1()
		{
			CocktailSorter<int> sorter = new CocktailSorter<int>();
			sorter.Sort(new List<int>(), (IComparer<int>)null);
		}

		private void TestSorter(Sorter<int> sorter)
		{
			// Test Reverse sequential list
			List<int> list = GetReverseSequentialTestList();
			sorter.Sort(list);
			AssertGeneralTestListSorted(list);

			// Test allready sorted list
			list = GetSortedList();
			sorter.Sort(list);
			AssertGeneralTestListSorted(list);

			// Test half sequential list
			list = GetHalfSequentialList();
			sorter.Sort(list);
			AssertGeneralTestListSorted(list);

			// Test double numbers
			list = GetDoubleNumbers();
			sorter.Sort(list);
			AssertDoubleNumbersList(list);			
		}

		private void AssertGeneralTestListSorted(List<int> sortedList) {
			for (int i = 0; i < sortedList.Count; i++)
			{
				Assert.AreEqual(sortedList[i], i);
			}
		}

		private void AssertDoubleNumbersList(List<int> sortedList) {
			for (int i = 0; i < sortedList.Count; i++)
			{
				if ((i % 2) == 0)
				{
					Assert.AreEqual(sortedList[i], i);
				}
				else
				{
					Assert.AreEqual(sortedList[i], i - 1);
				}
			}
		}

		private List<int> GetDoubleNumbers()
		{
			List<int> l = new List<int>(500);

			for (int i = 499; i >= 0; i--)
			{
				if ((i % 2) == 0)
				{
					l.Add(i);
				}
				else
				{
					l.Add(i - 1);
				}
			}

			return l;
		}

		private List<int> GetReverseSequentialTestList()
		{
			List<int> l = new List<int>(500);

			for (int i = 499; i >= 0; i--) {
				l.Add(i);
			}

			return l;
		}

		private List<int> GetHalfSequentialList()
		{
			List<int> l = new List<int>(500);
						
			for (int i = 499; i >= 200; i--)
			{
				l.Add(i);
			}

			for (int i = 199; i >= 0; i--)
			{
				l.Add(i);
			}

			return l;
		}

		private List<int> GetSortedList()
		{
			List<int> l = new List<int>(500);

			for (int i = 0; i < 500; i++)
			{
				l.Add(i);
			}

			return l;
		}
	}
}
