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
    /// An interface for a Sorter.
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public interface ISorter<T>
	{
        /// <summary>
        /// Sorts the specified list.
        /// </summary>
        /// <param name="list">The list to sort.</param>
		void Sort(IList<T> list);

        /// <summary>
        /// Sorts the specified list.
        /// </summary>
        /// <param name="list">The list to sort.</param>
        /// <param name="order">The order in which to sort the list.</param>
        void Sort(IList<T> list, SortOrder order);

        /// <summary>
        /// Sorts the specified list.
        /// </summary>
        /// <param name="list">The list to sort.</param>
        /// <param name="comparer">The comparer to use.</param>
        void Sort(IList<T> list, IComparer<T> comparer);

        /// <summary>
        /// Sorts the specified list.
        /// </summary>
        /// <param name="list">The list to sort.</param>
        /// <param name="comparison">The comparison to use.</param>
        void Sort(IList<T> list, Comparison<T> comparison);
	}
}
