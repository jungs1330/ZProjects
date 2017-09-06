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
using NGenerics.Visitors;

namespace NGenerics.Visitors
{
	/// <summary>
	/// A visitor that searches objects for an equality, using the IComparable interface.
	/// </summary>	
	public sealed class ComparableFindingVisitor<T> : IVisitor<T>, IFindingIVisitor<T>  where T:IComparable
	{
		#region Globals

		private bool found = false;
		private T searchValue;

		#endregion

		#region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="ComparableFindingVisitor&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="searchValue">The search value.</param>
		public ComparableFindingVisitor(T searchValue)
		{
			this.searchValue = searchValue;
		}

		#endregion

		#region IVisitor<T> Members

		/// <summary>
		/// Gets a value indicating whether this instance is done performing it's work..
		/// </summary>
		/// <value><c>true</c> if this instance is done; otherwise, <c>false</c>.</value>
		public bool HasCompleted
		{
			get
			{
				return found;
			}
		}

		/// <summary>
		/// Visits the specified object.
		/// </summary>
		/// <param name="obj">The object.</param>
		public void Visit(T obj)
		{
			if (obj.CompareTo(searchValue) == 0) {
				found = true;
			}
		}

		#endregion

		#region IFindingIVisitor<T> Members

		/// <summary>
		/// Gets a value indicating whether the value being searched for has been found.
		/// </summary>
		/// <value><c>true</c> if found; otherwise, <c>false</c>.</value>
		public bool Found
		{
			get
			{
				return found;
			}
		}

		/// <summary>
		/// Gets the search value.
		/// </summary>
		/// <value>The search value.</value>
		public T SearchValue
		{
			get
			{
				return searchValue;
			}			
		}

		#endregion
	}
}
