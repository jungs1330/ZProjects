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
using NGenerics.Comparers;

namespace NGenerics.Sorting
{
    /// <summary>
    /// The base class used for all Sorters in the library.
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public abstract class Sorter<T> : ISorter<T>
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="Sorter&lt;T&gt;"/> class.
		/// </summary>
		protected Sorter()
		{
			
		}		

		#endregion
								
		#region ISorter<T> Members

		/// <summary>
		/// Sorts the specified list.
		/// </summary>
		/// <param name="list">The list.</param>
		/// <param name="comparer">The comparer to use in comparing items.</param>
		public abstract void Sort(IList<T> list, IComparer<T> comparer);

        /// <summary>
        /// Sorts the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="comparison">The comparison to use.</param>
        public void Sort(IList<T> list, Comparison<T> comparison)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            if (comparison == null)
            {
                throw new ArgumentNullException("comparison");
            }

            Sort(list, new ComparisonComparer<T>(comparison));
        }


		/// <summary>
		/// Sorts the specified list.
		/// </summary>
		/// <param name="list">The list.</param>
		public void Sort(IList<T> list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}

			Sort(list, Comparer<T>.Default);
		}

        /// <summary>
        /// Sorts the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="order">The order in which to sort the list.</param>
		public void Sort(IList<T> list, SortOrder order)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}

			if (order == SortOrder.Ascending)
			{
				Sort(list, Comparer<T>.Default);
			}
			else if (order == SortOrder.Descending)
			{
				Sort(list, new ReverseComparer<T>(Comparer<T>.Default));
			}
		}

		#endregion

		#region Protected Members

		/// <summary>
		/// Swaps items in the specified list.
		/// </summary>
		/// <param name="list">The list.</param>
		/// <param name="pos1">The position of the first item.</param>
		/// <param name="pos2">The position of the last item.</param>
		protected void Swap(IList<T> list, int pos1, int pos2)
		{
			T tmp = list[pos1];
			list[pos1] = list[pos2];
			list[pos2] = tmp;
		}
		
		#endregion
	}
}
