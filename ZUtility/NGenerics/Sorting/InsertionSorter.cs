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

namespace NGenerics.Sorting
{
    /// <summary>
    /// A sorter that implements the Insertion sort algorithm.
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public sealed class InsertionSorter<T> : Sorter<T>
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="InsertionSorter&lt;T&gt;"/> class.
		/// </summary>
		public InsertionSorter() { }

		#endregion

		#region Private Members

		private void Insert(IList<T> list, int sortedSequenceLength, T val, IComparer<T> comparer) {
			int i = sortedSequenceLength - 1;

			while ((i >= 0) && (comparer.Compare(list[i], val) > 0))
			{
				list[i + 1] = list[i];
				i--;
			}

			list[i + 1] = val;
		}

		#endregion

		#region Sorter<T> Members

        /// <summary>
        /// Sorts the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="comparer">The comparer to use in comparing items.</param>
		public override void Sort(IList<T> list, IComparer<T> comparer)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}

			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}

			Sort(list, comparer, 0, list.Count - 1);
		}

        /// <summary>
        /// Sorts the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="comparer">The comparer.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
		public void Sort(IList<T> list, IComparer<T> comparer, int start, int end)
		{
			if (end - start + 1 <= 1)
			{
				return;
			}

			int counter = start;

			while (counter < end + 1)
			{
				Insert(list, counter, list[counter], comparer);
				counter++;
			}
		}

		#endregion
	}
}
