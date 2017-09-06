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
using NGenerics.Sorting;

namespace NGenerics.DataStructures
{
	/// <summary>
	/// An extension of the default List datastructure.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public sealed class VisitableList<T> : System.Collections.Generic.List<T>, IVisitableList<T>, ISortable<T>
	{
		#region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="VisitableList&lt;T&gt;"/> class.
        /// </summary>
		public VisitableList() : base() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="VisitableList&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="collection">The collection whose elements are copied to the new list.</param>
		/// <exception cref="T:System.ArgumentNullException">collection is null.</exception>
		public VisitableList(IEnumerable<T> collection) : base(collection) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="VisitableList&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="capacity">The capacity.</param>
		public VisitableList(int capacity) : base(capacity) { }

		#endregion

		#region IVisitableCollection<T> Members

		/// <summary>
		/// Accepts the specified visitor.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		public void Accept(IVisitor<T> visitor)
		{
			if (visitor == null)
			{
				throw new ArgumentNullException("visitor");
			}

			for (int i = 0; i < this.Count; i++)
			{
				visitor.Visit(this[i]);

				if (visitor.HasCompleted)
				{
					break;
				}
			}
		}

		/// <summary>
		/// Gets a value indicating whether this collection is empty.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this collection is empty; otherwise, <c>false</c>.
		/// </value>
		public bool IsEmpty
		{
			get
			{
				return this.Count == 0;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the <see cref="T:System.Collections.IList"></see> has a fixed size.
		/// </summary>
		/// <value></value>
		/// <returns>true if the <see cref="T:System.Collections.IList"></see> has a fixed size; otherwise, false.</returns>
		public bool IsFixedSize
		{
			get {
				return false;				
			}
		}

		/// <summary>
		/// Gets a value indicating whether this collection is full.
		/// </summary>
		/// <value><c>true</c> if this collection is full; otherwise, <c>false</c>.</value>
		public bool IsFull
		{
			get
			{
				return false;
			}
		}		
		#endregion

		#region IComparable Members

		/// <summary>
		/// Compares the current instance with another object of the same type.
		/// </summary>
		/// <param name="obj">An object to compare with this instance.</param>
		/// <returns>
		/// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance is less than obj. Zero This instance is equal to obj. Greater than zero This instance is greater than obj.
		/// </returns>
		/// <exception cref="T:System.ArgumentException">obj is not the same type as this instance. </exception>
		public int CompareTo(object obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}

			if (obj.GetType() == this.GetType())
			{
				VisitableList<T> l = obj as VisitableList<T>;

				return this.Count.CompareTo(l.Count);
			}
			else
			{
				return this.GetType().FullName.CompareTo(obj.GetType().FullName);
			}
		}
		
		#endregion

        #region ISortable<T> Members

        /// <summary>
        /// Sort the list using the specified sorter.
        /// </summary>
        /// <param name="sorter">The sorter to use in the sorting process.</param>
        public void Sort(ISorter<T> sorter)
        {
            if (sorter == null)
            {
                throw new ArgumentNullException("sorter");
            }

            sorter.Sort(this);
        }

        /// <summary>
        /// Sorts using the specified sorter.
        /// </summary>
        /// <param name="sorter">The sorter to use in the sorting process.</param>
        /// <param name="comparison">The comparison.</param>
        public void Sort(ISorter<T> sorter, Comparison<T> comparison)
        {
            if (sorter == null)
            {
                throw new ArgumentNullException("sorter");
            }

            if (comparison == null)
            {
                throw new ArgumentNullException("comparison");
            }

            sorter.Sort(this, comparison);
        }

        /// <summary>
        /// Sorts using the specified sorter.
        /// </summary>
        /// <param name="sorter">The sorter to use in the sorting process.</param>
        /// <param name="comparer">The comparer.</param>
        public void Sort(ISorter<T> sorter, IComparer<T> comparer)
        {
            if (sorter == null)
            {
                throw new ArgumentNullException("sorter");
            }

            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }

            sorter.Sort(this, comparer);            
        }

        #endregion
    }
}
