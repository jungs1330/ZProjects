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

/*
 * Adapted from http://www.cs.rit.edu/~atk/Java/Sorting/sorting.html
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace NGenerics.Sorting
{
	/// <summary>
	/// An OddEvenTransport sorter.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public sealed class OddEvenTransportSorter<T> : Sorter<T>
	{
		#region Sorter<T> Members

		/// <summary>
		/// Sorts the specified list.
		/// </summary>
		/// <param name="list">The list.</param>
		/// <param name="comparer">The comparer to use in comparing items.</param>
		public override void Sort(IList<T> list, IComparer<T> comparer) {
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}

			if (comparer == null) {
				throw new ArgumentNullException("comparer");
			}

			if (list.Count < 2)
			{
				return;
			}

			for (int i = 0; i < list.Count / 2; ++i)
			{
				for (int j = 0; j + 1 < list.Count; j += 2)
				{
					if (comparer.Compare(list[j], list[j + 1]) > 0)
					{
						this.Swap(list, j, j + 1);
					}
				}

				for (int j = 1; j + 1 < list.Count; j += 2)
				{
					if (comparer.Compare(list[j], list[j + 1]) > 0)
					{
						this.Swap(list, j, j + 1);
					}
				}
			}
		}

		#endregion
	}
}
