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
using NGenerics.Misc;
using NGenerics.Enumerations;
using NGenerics.Comparers;

namespace NGenerics.DataStructures
{
    /// <summary>
    /// An implementation of a Heap data structure.
    /// </summary>
    /// <typeparam name="T">The type of item stored in the heap.</typeparam>
	public sealed class Heap<T> : IVisitableCollection<T>, IHeap<T>
	{
		#region Globals

		private VisitableList<T> data;
		private IComparer<T> comparerToUse;
		private HeapType thisType;

		#endregion

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="Heap&lt;T&gt;"/> class.
		/// </summary>
		public Heap(HeapType type) : this(type, Comparer<T>.Default) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="Heap&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="type">The type of heap.</param>
        /// <param name="capacity">The capacity.</param>
		public Heap(HeapType type, int capacity) : this(type, capacity, Comparer<T>.Default) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="Heap&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="type">The type of heap.</param>
        /// <param name="comparer">The comparer to use.</param>
		public Heap(HeapType type, IComparer<T> comparer)
		{
			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}

			thisType = type;

			data = new VisitableList<T>();
			data.Add(default(T));  // Add a dummy item so our indexing starts at 1

			if (type == HeapType.MinHeap)
			{
				comparerToUse = comparer;
			}
			else
			{
				comparerToUse = new ReverseComparer<T>(comparer);
			}
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="Heap&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="type">The type of heap.</param>
        /// <param name="capacity">The initial capacity of the Heap.</param>
        /// <param name="comparer">The comparer to use.</param>
		public Heap(HeapType type, int capacity, IComparer<T> comparer)
		{
			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}

			thisType = type;

			data = new VisitableList<T>(capacity);
			data.Add(default(T));  // Add a dummy item so our indexing starts at 1

			if (type == HeapType.MinHeap)
			{
				comparerToUse = comparer;
			}
			else
			{
				comparerToUse = new ReverseComparer<T>(comparer);
			}
		}

		#endregion

		#region Public Members

		/// <summary>
		/// Gets the smallest item in the heap (located at the root).
		/// </summary>
		/// <returns>The value of the root of the Heap.</returns>
		public T Root
		{
			get
			{
				if (this.Count == 0)
				{
					throw new InvalidOperationException(Resources.HeapIsEmpty);
				}
				else
				{
					return data[1];
				}
			}
		}

		/// <summary>
		/// Removes the smallest item in the heap (located at the root).
		/// </summary>
		/// <returns>The value contained in the root of the Heap.</returns>
		public T RemoveRoot()
		{
			if (this.Count == 0)
			{
				throw new InvalidOperationException(Resources.HeapIsEmpty);
			}

			// The minimum item to return.
			T min = data[1];

			// The last item in the heap
			T last = data[this.Count];
			data.RemoveAt(this.Count);

			// If there's still items left in this heap, reheapify it.
			if (this.Count > 0)
			{
				// Re-heapify the binary tree to conform to the heap property 
				int counter = 1;

				while ((counter * 2) < (data.Count))
				{
					int child = counter * 2;

					if (((child + 1) < (data.Count)) &&
						(comparerToUse.Compare(data[child + 1], data[child]) < 0))
					{
						child++;
					}

					if (comparerToUse.Compare(last, data[child]) <= 0)
					{
						break;
					}

					data[counter] = data[child];
					counter = child;
				}

				data[counter] = last;
			}

			return min;
		}

		/// <summary>
		/// Gets the type of heap represented by this instance.
		/// </summary>
		/// <value>The type of heap.</value>
		public HeapType Type
		{
			get
			{
				return thisType;
			}
		}

		#endregion

		#region IVisitableCollection<T> Members

		/// <summary>
		/// Gets a value indicating whether this instance is of a fixed size.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is fixed size; otherwise, <c>false</c>.
		/// </value>
		public bool IsFixedSize
		{
			get {
				return false;
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
			get {
				return this.Count == 0;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this collection is full.
		/// </summary>
		/// <value><c>true</c> if this collection is full; otherwise, <c>false</c>.</value>
		public bool IsFull
		{
			get {
				return false;	
			}
		}

		/// <summary>
		/// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> contains a specific value.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
		/// <returns>
		/// true if item is found in the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false.
		/// </returns>
		public bool Contains(T item)
		{
			return data.Contains(item);
		}

		/// <summary>
		/// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"></see> to an <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"></see>. The <see cref="T:System.Array"></see> must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">arrayIndex is less than 0.</exception>
		/// <exception cref="T:System.ArgumentNullException">array is null.</exception>
		/// <exception cref="T:System.ArgumentException">array is multidimensional.-or-arrayIndex is equal to or greater than the length of array.-or-The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"></see> is greater than the available space from arrayIndex to the end of the destination array.-or-Type T cannot be cast automatically to the type of the destination array.</exception>
		public void CopyTo(T[] array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}

			if ((array.Length - arrayIndex) < this.Count)
			{
				throw new ArgumentException(Resources.NotEnoughSpaceInTargetArray);
			}

			for (int i = 1; i < data.Count; i++)
			{
				array[arrayIndex++] = data[i];
			}
		}

		/// <summary>
		/// Gets the number of elements contained in the <see cref="T:System.Collections.ICollection"></see>.
		/// </summary>
		/// <value></value>
		/// <returns>The number of elements contained in the <see cref="T:System.Collections.ICollection"></see>.</returns>
		public int Count
		{
			get {
				return data.Count - 1;
			}
		}

		/// <summary>
		/// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
		/// </summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
		public void Add(T item)
		{			
			// Add a dummy to the end of the list (it will be replaced)
			data.Add(default(T));

			int counter = data.Count - 1;

			while ((counter > 1) && (comparerToUse.Compare(data[counter / 2], item) > 0))
			{
				data[counter] = data[counter / 2];
				counter  = counter / 2;
			}

			data[counter] = item;
		}

		/// <summary>
		/// Accepts the specified visitor.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		public void Accept(NGenerics.Visitors.IVisitor<T> visitor)
		{
			if (visitor == null)
			{
				throw new ArgumentNullException("visitor");
			}

			for (int i = 1; i < data.Count; i++)
			{
				if (visitor.HasCompleted)
				{
					break;
				}

				visitor.Visit(data[i]);
			}
		}

		/// <summary>
		/// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
		/// </summary>
		/// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
		/// <returns>
		/// true if item was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false. This method also returns false if item is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"></see>.
		/// </returns>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
		bool ICollection<T>.Remove(T item)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<T> GetEnumerator()
		{
			for (int i = 1; i < data.Count; i++)
			{
				yield return data[i];
			}
		}

		/// <summary>
		/// Clears all the objects in this instance.
		/// </summary>
		public void Clear()
		{
			data.RemoveRange(1, data.Count - 1); // Clears all objects in this instance except the first dummy one.

		}

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

			if (this.GetType() == obj.GetType())
			{
				Heap<T> heap = obj as Heap<T>;
				return this.Count.CompareTo(heap.Count);
			}
			else
			{
				return this.GetType().FullName.CompareTo(obj.GetType().FullName);
			}
		}

		#endregion

		#region ICollection<T> Members

		/// <summary>
		/// Gets a value indicating whether this instance is read only.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is read only; otherwise, <c>false</c>.
		/// </value>
		public bool IsReadOnly
		{
			get {
				return false;
			}
		}

		#endregion

		#region IEnumerable Members

		/// <summary>
		/// Gets the enumerator.
		/// </summary>
		/// <returns>An enumerator for enumerating though the colleciton.</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion

		#region Internal Members

		/// <summary>
		/// Gets the underlying list.
		/// </summary>
		/// <value>The underlying list.</value>
		internal IList<T> UnderlyingList
		{
			get
			{
				return data;
			}
		}

		#endregion
	}
}
