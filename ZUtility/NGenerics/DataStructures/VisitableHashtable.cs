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
using NGenerics;
using System.Runtime.Serialization;
using NGenerics.Visitors;

namespace NGenerics.DataStructures
{
	/// <summary>
	/// A custom hashtable extending the standard Generic Dictionary.
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TValue"></typeparam>
	[Serializable]
	public class VisitableHashtable<TKey, TValue> : Dictionary<TKey, TValue>, IVisitableDictionary<TKey, TValue>
	{
		#region Construction
        
        /// <summary>
        /// Initializes a new instance of the <see cref="VisitableHashtable&lt;TKey, TValue&gt;"/> class.
        /// </summary>
		public VisitableHashtable() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="VisitableHashtable&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
		public VisitableHashtable(IDictionary<TKey, TValue> dictionary) : base(dictionary) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="VisitableHashtable&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
		public VisitableHashtable(IEqualityComparer<TKey> comparer) : base(comparer) { }


        /// <summary>
        /// Initializes a new instance of the <see cref="VisitableHashtable&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
		public VisitableHashtable(int capacity) : base(capacity) { }


        /// <summary>
        /// Initializes a new instance of the <see cref="VisitableHashtable&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="comparer">The comparer.</param>
		public VisitableHashtable(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer) : base(dictionary, comparer) { }


        /// <summary>
        /// Initializes a new instance of the <see cref="VisitableHashtable&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        /// <param name="comparer">The comparer.</param>
		public VisitableHashtable(int capacity, IEqualityComparer<TKey> comparer) : base(capacity, comparer) { }


        /// <summary>
        /// Initializes a new instance of the <see cref="VisitableHashtable&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        /// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> object containing the information required to serialize the <see cref="T:System.Collections.Generic.Dictionary`2"></see>.</param>
        /// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext"></see> structure containing the source and destination of the serialized stream associated with the <see cref="T:System.Collections.Generic.Dictionary`2"></see>.</param>
		protected VisitableHashtable(SerializationInfo info, StreamingContext context) : base(info, context) { }
		
		#endregion

		#region IVisitableCollection<KeyValuePair<TKey,TValue>> Members

		/// <summary>
		/// Accepts the specified visitor.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		public void Accept(IVisitor<KeyValuePair<TKey, TValue>> visitor)
		{
			if (visitor == null)
			{
				throw new ArgumentNullException("visitor");
			}

			Dictionary<TKey, TValue>.Enumerator enumerator = this.GetEnumerator();

			while (enumerator.MoveNext())
			{
				visitor.Visit(enumerator.Current);

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

		/// <summary>
		/// Gets a value indicating whether the <see cref="T:System.Collections.IDictionary"></see> collection has a fixed size.
		/// </summary>
		/// <value></value>
		/// <returns>true if the <see cref="T:System.Collections.IDictionary"></see> collection has a fixed size; otherwise, false.</returns>
		public bool IsFixedSize
		{
			get {
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
				VisitableHashtable<TKey, TValue> h = obj as VisitableHashtable<TKey, TValue>;
				return this.Count.CompareTo(h.Count);
			}
			else
			{
				return this.GetType().FullName.CompareTo(obj.GetType().FullName);
			}
		}

		#endregion
	}
}
