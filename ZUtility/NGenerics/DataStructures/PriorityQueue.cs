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
using NGenerics.Misc;
using NGenerics.Enumerations;

namespace NGenerics.DataStructures
{
	#region PriorityQueue<T>

    /// <summary>
    /// An inplementation of a Priority Queue (can be min or max).
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public sealed class PriorityQueue<T> : IVisitableCollection<T>, IQueue<T>
	{
		#region Globals

		private Heap<Association<int, T>> heap;

		#endregion

		#region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="PriorityQueue&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="queueType">Type of the queue.</param>
		public PriorityQueue(PriorityQueueType queueType)
		{
			if (queueType == PriorityQueueType.MaxPriorityQueue)
			{
				heap = new Heap<Association<int, T>>(HeapType.MaxHeap, new AssociationKeyComparer<int, T>());
			}
			else
			{
				heap = new Heap<Association<int, T>>(HeapType.MinHeap, new AssociationKeyComparer<int, T>());
			}
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="PriorityQueue&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="queueType">Type of the queue.</param>
        /// <param name="capacity">The initial capacity of the Priority Queue.</param>
		public PriorityQueue(PriorityQueueType queueType, int capacity)
		{
			if (queueType == PriorityQueueType.MaxPriorityQueue)
			{
				heap = new Heap<Association<int, T>>(HeapType.MaxHeap, capacity, new AssociationKeyComparer<int, T>());
			}
			else
			{
				heap = new Heap<Association<int, T>>(HeapType.MaxHeap, capacity, new AssociationKeyComparer<int, T>());
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
		/// Gets the number of elements contained in the <see cref="T:System.Collections.ICollection"></see>.
		/// </summary>
		/// <value></value>
		/// <returns>The number of elements contained in the <see cref="T:System.Collections.ICollection"></see>.</returns>
		public int Count
		{
			get {
				return heap.Count;
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
			IList<Association<int, T>> list = heap.UnderlyingList;
			
			for (int i = 1; i < heap.Count + 1; i++)
			{
				if (list[i].Value.Equals(item))
				{
					return true;
				}				
			}

			return false;
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

			IList<Association<int, T>> list = heap.UnderlyingList;

			for (int i = 1; i < list.Count; i++)
			{
				array.SetValue(list[i].Value, arrayIndex++);
			}
		}

		/// <summary>
		/// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
		/// </summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
		public void Add(T item)
		{
			heap.Add(new Association<int, T>(0, item));
		}

		/// <summary>
		/// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
		/// </summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
		/// <param name="priority">The priority of the item.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
		public void Add(T item, int priority)
		{
			heap.Add(new Association<int, T>(priority, item));
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
		/// Accepts the specified visitor.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		public void Accept(NGenerics.Visitors.IVisitor<T> visitor)
		{
			if (visitor == null)
			{
				throw new ArgumentNullException("visitor");
			}

			IList<Association<int, T>> list = heap.UnderlyingList;

			for (int i = 1; i < list.Count; i++)
			{
				visitor.Visit(list[i].Value);

				if (visitor.HasCompleted)
				{
					break;
				}
			}
		}

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<T> GetEnumerator()
		{
			IEnumerator<Association<int, T>> enumerator = heap.GetEnumerator();

			while (enumerator.MoveNext())
			{
				yield return enumerator.Current.Value;
			}
		}

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<Association<int, T>> GetKeyEnumerator()
		{
			return heap.GetEnumerator();
		}

		/// <summary>
		/// Clears all the objects in this instance.
		/// </summary>
		public void Clear()
		{
			heap.Clear();
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

			if (obj.GetType() == this.GetType())
			{
				PriorityQueue<T> q = obj as PriorityQueue<T>;
				return this.Count.CompareTo(q.Count);
			}
			else
			{
				return this.GetType().FullName.CompareTo(obj.GetType().FullName);
			}
		}

		#endregion

		#region IQueue<T> Members

		/// <summary>
		/// Enqueues the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		public void Enqueue(T item)
		{
			Add(item);
		}

		/// <summary>
		/// Enqueues the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <param name="priority">The priority.</param>
		public void Enqueue(T item, int priority)
		{
			Add(item, priority);
		}

		/// <summary>
		/// Dequeues the item from the head of the queue.
		/// </summary>
		/// <returns>The item at the head of the queue.</returns>
		public T Dequeue()
		{
			CheckListNotEmpty();
			return heap.RemoveRoot().Value;
		}

		/// <summary>
		/// Returns the first item in the queue without removing it from the queue.
		/// </summary>
		/// <returns>The item at the head of the queue.</returns>
		public T Peek()
		{
			CheckListNotEmpty();

			return heap.Root.Value;			
		}

		#endregion

		#region Public Members

		/// <summary>
		/// Increases the priority of all the items in the queue.
		/// </summary>
		/// <param name="by">The number that gets added to the priority of each item.</param>
		public void IncreaseItemPriority(int by)
		{
			IList<Association<int, T>> list = heap.UnderlyingList;

			for (int i = 1; i < list.Count; i++)
			{
				list[i].Key += by;
			}
		}

		/// <summary>
		/// Decreases the priority of all the items in the queue.
		/// </summary>
		/// <param name="by">The number that gets subtracted to the priority of each item.</param>
		public void DecreaseItemPriority(int by)
		{
			IList<Association<int, T>> list = heap.UnderlyingList;

			for (int i = 1; i < list.Count; i++)
			{
				list[i].Key -= by;
			}
		}


		#endregion

		#region Private Members

		/// <summary>
		/// Checks if the list is not empty, and if it is, throw an exception.
		/// </summary>
		private void CheckListNotEmpty()
		{
			if (heap.Count == 0)
			{
				throw new InvalidOperationException("The Priority Queue is empty.");
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
		/// <returns>An enumerator for enumerating through the collection.</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion
	}

	#endregion	
}
