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
using System.Diagnostics;
using NGenerics.Misc;

namespace NGenerics.DataStructures
{
    /// <summary>
    /// A Bag data structure.
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public sealed class Bag<T> : IVisitableCollection<T>, IBag<T>
	{
		#region Globals

		VisitableHashtable<T, int> data;
		int count;

		#endregion

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="Bag&lt;T&gt;"/> class.
		/// </summary>
		public Bag()
		{
			data = new VisitableHashtable<T, int>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Bag&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="capacity">The initial capacity of the bag.</param>
		public Bag(int capacity)
		{
			data = new VisitableHashtable<T, int>(capacity);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Bag&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="comparer">The comparer to use when testing equality.</param>
		public Bag(IEqualityComparer<T> comparer) {
			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}

			data = new VisitableHashtable<T, int>(comparer);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Bag&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="capacity">The initial capacity of the bag.</param>
		/// <param name="comparer">The comparer to use when testing equality.</param>
		public Bag(int capacity, IEqualityComparer<T> comparer)
		{
			if (comparer == null)
			{
				throw new ArgumentNullException("comparer");
			}

			data = new VisitableHashtable<T, int>(capacity, comparer);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Bag&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="dictionary">The dictionary to copy values from.</param>
		private Bag(IDictionary<T, int> dictionary)
		{
			#region Asserts

			Debug.Assert(dictionary != null);

			#endregion

			data = new VisitableHashtable<T, int>(dictionary);

			// Update the count
			using (Dictionary<T, int>.Enumerator enumerator = data.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					count += enumerator.Current.Value;
				}
			}
		}

		#endregion

		#region Public Members

		/// <summary>
		/// Determines whether the specified bag is equal to this one.
		/// </summary>
		/// <param name="bag">The bag.</param>
		/// <returns>
		/// 	<c>true</c> if the specified bag is equal this this one; otherwise, <c>false</c>.
		/// </returns>
		public bool IsEqual(Bag<T> bag)
		{
			if (bag == null)
			{
				throw new ArgumentNullException("bag");
			}

			if (bag.count != this.count)
			{
				return false;
			}
			else
			{
				Dictionary<T, int>.Enumerator enumerator = data.GetEnumerator();

				while (enumerator.MoveNext())
				{
					if (!bag.Contains(enumerator.Current.Key))
					{
						return false;
					}
					else
					{
						if (bag[enumerator.Current.Key] != enumerator.Current.Value)
						{
							return false; 
						}
					}
				}

				return true;
			}
		}

        /// <summary>
        /// Removes all instances of  the specified item in the bag.
        /// </summary>
        /// <param name="item">The item to be removed.</param>
        /// <returns>A value indicating if the item was found (and removed) from the bag.</returns>
		public bool RemoveAll(T item) {
			if (data.ContainsKey(item))
			{
				count -= data[item];
				data.Remove(item);

				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Removes count occurrences of the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <param name="max">The maximum number of items to remove.</param>
		/// <returns>A value indicating whether the items have been removed (was found).</returns>
		public bool Remove(T item, int max)
		{
			if (data.ContainsKey(item))
			{
				if (max >= data[item])
				{
					count -= data[item];
					data.Remove(item);
				}
				else
				{
					count -= max;
					data[item] -= max;
				}

				return true;
			}
			else
			{
				return false;
			}

		}

		/// <summary>
		/// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
		/// </summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
		/// <param name="amount">The amount.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
		public void Add(T item, int amount)
		{
			if (amount <= 0)
			{
				throw new ArgumentOutOfRangeException(Resources.OnlyAddPositiveAmount);
			}

			if (data.ContainsKey(item))
			{
				data[item]+= amount;
			}
			else
			{
				data.Add(item, amount);
			}

			count+= amount;
		}

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<KeyValuePair<T, int>> GetCountEnumerator()
		{
			return data.GetEnumerator();
		}

		/// <summary>
		/// Computes the union of this bag and the specified bag.
		/// </summary>
		/// <param name="bag">The bag.</param>
		/// <returns>The union of this bag and the bag specified.</returns>
		public Bag<T> Union(Bag<T> bag)
		{
			if (bag == null)
			{
				throw new ArgumentNullException("bag");
			}

			Bag<T> result;
			Dictionary<T, int>.Enumerator enumerator;

			// A small optimisation for big Bags - make a copy of the biggest Bag
			if (bag.data.Count > this.data.Count)
			{
				result = new Bag<T>(bag.data);
				enumerator = data.GetEnumerator();
			}
			else
			{
				result = new Bag<T>(data);
				enumerator = bag.data.GetEnumerator();
			}

			while (enumerator.MoveNext()) {
				result.Add(enumerator.Current.Key, enumerator.Current.Value);
			}

			enumerator.Dispose();

			return result;
		}

		/// <summary>
		/// Computes the difference between this bag and the specified bag.
		/// </summary>
		/// <param name="bag">The bag.</param>
		/// <returns>The difference between this bag and the bag specified.</returns>
		public Bag<T> Difference(Bag<T> bag)
		{
			if (bag == null)
			{
				throw new ArgumentNullException("bag");
			}

			Bag<T> result = new Bag<T>(this.data);
			
			using (Dictionary<T, int>.Enumerator enumerator = bag.data.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (result.data.ContainsKey(enumerator.Current.Key))
					{
						int itemCount = result.data[enumerator.Current.Key];

						if (itemCount - enumerator.Current.Value <= 0)
						{
							result.RemoveAll(enumerator.Current.Key);
						}
						else
						{
							result.Remove(enumerator.Current.Key, enumerator.Current.Value);
						}
					}
				}
			}

			return result;
		}

		/// <summary>
		/// Computes the intersection between this bag and the specified bag.
		/// </summary>
		/// <param name="bag">The bag.</param>
		/// <returns>The intersection between this bag and the specified bag.</returns>
		public Bag<T> Intersection(Bag<T> bag)
		{
			if (bag == null)
			{
				throw new ArgumentNullException("bag");
			}

			Bag<T> result = new Bag<T>();

			Dictionary<T, int>.Enumerator enumerator = bag.data.GetEnumerator();

			while (enumerator.MoveNext())
			{
				if (this.data.ContainsKey(enumerator.Current.Key))
				{
					result.Add(enumerator.Current.Key,
						Math.Min(enumerator.Current.Value, this.data[enumerator.Current.Key])
					);
				}
			}

			return result;
		}

		/// <summary>
		/// Accepts the specified visitor.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		public void Accept(NGenerics.Visitors.IVisitor<KeyValuePair<T, int>> visitor) 
		{
			if (visitor == null)
			{
				throw new ArgumentNullException("visitor");
			}

			data.Accept(visitor);
		}

		#endregion

		#region Private Members

		#endregion

		#region Operator Overloads

		/// <summary>
		/// Operator + : Performs a union between two Bags.
		/// </summary>
		/// <param name="b1">The left hand Bag.</param>
		/// <param name="b2">The right hand Bag.</param>
		/// <returns>The union between this bag and the bag specified.</returns>
		public static Bag<T> operator +(Bag<T> b1, Bag<T> b2)
		{
			return b1.Union(b2);
		}

		/// <summary>
		/// Operator - : Performs a difference operation between two Bags.
		/// </summary>
		/// <param name="b1">The left hand Bag.</param>
		/// <param name="b2">The right hand Bag.</param>
		/// <returns>The difference between this bag and the bag specified.</returns>
		public static Bag<T> operator -(Bag<T> b1, Bag<T> b2)
		{
			return b1.Difference(b2);
		}

		/// <summary>
		/// Operator * : Performs a intersection between two Bags.
		/// </summary>
		/// <param name="b1">The left hand Bag.</param>
		/// <param name="b2">The right hand Bag.</param>
		/// <returns>The intersection between this bag and the bag specified.</returns>
		public static Bag<T> operator *(Bag<T> b1, Bag<T> b2)
		{
			return b1.Intersection(b2);
		}

		/// <summary>
		/// Indicates whether an item is present in this Bag and returns the count.
		/// </summary>
		/// <value></value>
		public int this[T item]
		{
			get
			{
				if (data.ContainsKey(item))
				{
					return data[item];
				}
				else
				{
					return 0;
				}
			}
		}

		#endregion

		#region IVisitableCollection<T> Members

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

			using (IEnumerator<T> enumerator = this.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					visitor.Visit(enumerator.Current);

					if (visitor.HasCompleted)
					{
						break;
					}
				}
			}
		}

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
			
			Dictionary<T, int>.Enumerator enumerator = data.GetEnumerator();

			int counter = arrayIndex;

			while (enumerator.MoveNext())
			{
				int itemCount = enumerator.Current.Value;
				T obj = enumerator.Current.Key;

				for (int i = 0; i < itemCount; i++)
				{
					array.SetValue(obj, counter++);
				}
			}

			enumerator.Dispose();
		}

		/// <summary>
		/// Gets the number of elements contained in the <see cref="T:System.Collections.ICollection"></see>.
		/// </summary>
		/// <value></value>
		/// <returns>The number of elements contained in the <see cref="T:System.Collections.ICollection"></see>.</returns>
		public int Count
		{
			get {
				return count;
			}
		}

		/// <summary>
		/// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
		/// </summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
		public void Add(T item)
		{
			if (data.ContainsKey(item))
			{
				data[item]++;
			}
			else
			{
				data.Add(item, 1);
			}

			count++;
		}

		/// <summary>
		/// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
		/// </summary>
		/// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
		/// <returns>
		/// true if item was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false. This method also returns false if item is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"></see>.
		/// </returns>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
		public bool Remove(T item)
		{
			if (data.ContainsKey(item))
			{
				data[item]--;

				if (data[item] == 0)
				{
					data.Remove(item);
				}

				count--;

				return true;
			}
			else
			{
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
			return data.ContainsKey(item);
		}

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<T> GetEnumerator()
		{
			Dictionary<T, int>.Enumerator enumerator = data.GetEnumerator();

			while (enumerator.MoveNext())
			{
				yield return enumerator.Current.Key;
			}
		}

		/// <summary>
		/// Clears all the objects in this instance.
		/// </summary>
		public void Clear()
		{
			data.Clear();
			count = 0;
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
				Bag<T> bag = obj as Bag<T>;
				return this.Count.CompareTo(bag.Count);
			}
			else
			{
				return this.GetType().FullName.CompareTo(obj.GetType().FullName);
			}
		}

		#endregion

		#region ICollection<T> Members

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only; otherwise, false.</returns>
		public bool IsReadOnly
		{
			get {
				return false;
			}
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion

		#region IBag<T> Members

		IBag<T> IBag<T>.Difference(IBag<T> bag)
		{
			return this.Difference((Bag<T>)bag);
		}

		IBag<T> IBag<T>.Intersection(IBag<T> bag)
		{
			return this.Intersection((Bag<T>)bag);
		}

		bool IBag<T>.IsEqual(IBag<T> bag)
		{
			return this.IsEqual((Bag<T>)bag);
		}

		IBag<T> IBag<T>.Union(IBag<T> bag)
		{
			return this.Union((Bag<T>)bag);
		}

		#endregion
	}
}
