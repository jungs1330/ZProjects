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
using System.Text;
using System.Collections.Generic;
using NGenerics.Misc;

namespace NGenerics.DataStructures
{
	/// <summary>
	/// A datastructure much like a queue, except that you can enqueue and dequeue to both the head and the tail.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public sealed class Deque<T> : IVisitableCollection<T>, IDeque<T>
	{
		#region Globals

		VisitableLinkedList<T> list;

		#endregion

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="Deque&lt;T&gt;"/> class.
		/// </summary>
		public Deque()
		{
			list = new VisitableLinkedList<T>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Deque&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="collection">A collection implementing the IEnumerable&lt;T&gt; interface.</param>
		public Deque(System.Collections.Generic.IEnumerable<T> collection)
		{
			list = new VisitableLinkedList<T>(collection);
		}

		#endregion

		#region Public Members

		/// <summary>
		/// Enqueues the head.
		/// </summary>
		/// <param name="obj">The object to enqueue.</param>
		public void EnqueueHead(T obj)
		{
			list.AddFirst(obj);
		}

		/// <summary>
		/// Dequeues the head.
		/// </summary>
		/// <returns>The item at the head of the deque.</returns>
		public T DequeueHead()
		{
			if (list.Count == 0)
			{
				throw new InvalidOperationException(Resources.DequeIsEmpty);
			}
			else
			{
				T ret = list.First.Value;
				list.RemoveFirst();
				return ret;
			}
		}

		/// <summary>
		/// Enqueues the tail.
		/// </summary>
		/// <param name="obj">The obj.</param>
		public void EnqueueTail(T obj)
		{
			list.AddLast(obj);
		}

		/// <summary>
		/// Deqeues the tail.
		/// </summary>
		/// <returns>The item at the tail of the deque.</returns>
		public T DequeueTail()
		{
			if (list.Count == 0)
			{
				throw new InvalidOperationException(Resources.DequeIsEmpty);
			}
			else
			{
				T ret = list.Last.Value;
				list.RemoveLast();
				return ret;
			}
		}

		/// <summary>
		/// Gets the head.
		/// </summary>
		/// <value>The head.</value>
		public T Head
		{
			get
			{
				if (list.Count == 0)
				{
					throw new InvalidOperationException(Resources.DequeIsEmpty);
				}
				else
				{
					return list.First.Value;
				}
			}
		}

		/// <summary>
		/// Gets the tail.
		/// </summary>
		/// <value>The tail.</value>
		public T Tail
		{
			get
			{
				if (list.Count == 0)
				{
					throw new InvalidOperationException(Resources.DequeIsEmpty);
				}
				else
				{
					return list.Last.Value;
				}
			}
		}

		#endregion

		#region IVisitableCollection<T> Members

		/// <summary>
		/// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> contains a specific value.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
		/// <returns>
		/// true if item is found in the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false.
		/// </returns>
		public bool Contains(T item)
		{
			return list.Contains(item);
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
			list.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Gets the number of elements contained in the <see cref="T:System.Collections.ICollection"></see>.
		/// </summary>
		/// <value></value>
		/// <returns>The number of elements contained in the <see cref="T:System.Collections.ICollection"></see>.</returns>
		public int Count
		{
			get
			{
				return list.Count;
			}
		}

		/// <summary>
		/// Clears all the objects in this instance.
		/// </summary>
		public void Clear()
		{
			list.Clear();
		}

		/// <summary>
		/// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
		/// </summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
		void ICollection<T>.Add(T item)
		{
			throw new NotSupportedException();
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
		public System.Collections.Generic.IEnumerator<T> GetEnumerator()
		{
			return list.GetEnumerator();
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

			list.Accept(visitor);
		}

		/// <summary>
		/// Gets a value indicating whether this instance is of a fixed size.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is fixed size; otherwise, <c>false</c>.
		/// </value>
		public bool IsFixedSize
		{
			get
			{
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
				Deque<T> d = obj as Deque<T>;

				return this.Count.CompareTo(d.Count);
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
		/// <returns>An enumerator that enumerates through the collection.</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion
	}
}
