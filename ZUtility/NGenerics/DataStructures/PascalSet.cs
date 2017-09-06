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
using System.Collections;
using System.Diagnostics;
using NGenerics.Misc;

namespace NGenerics.DataStructures
{
	/// <summary>
	/// A datastructure for representing a set of objects and common operations performed on sets.
	/// </summary>
	public class PascalSet : IVisitableCollection<int>, ISet
	{
		#region Globals

		BitArray data;
		int lowerBound;
		int upperBound;
		int count;

		#endregion

		#region Construction

		#region Public Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="PascalSet"/> class.
		/// </summary>
		/// <param name="upperBound">The upper bound.  The lower bound defaults as 0.</param>
		public PascalSet(int upperBound) : this(0, upperBound) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="PascalSet"/> class.
		/// </summary>
		/// <param name="upperBound">The upper bound.  The lower bound defaults as 0.</param>
		/// <param name="initialValues">The initial values.</param>
		public PascalSet(int upperBound, int[] initialValues) : this(0, upperBound, initialValues) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="PascalSet"/> class.
		/// </summary>
		/// <param name="lowerBound">The lower bound.</param>
		/// <param name="upperBound">The upper bound.</param>
		/// <param name="initialValues">The initial values.</param>
        /// <exception cref="System.ArgumentNullException">intialValues is equal to null.</exception>
		public PascalSet(int lowerBound, int upperBound, int[] initialValues)
			: this(lowerBound, upperBound)
		{
			if (initialValues == null)
			{
				throw new ArgumentNullException(Resources.InitialValuesNull);
			}

			for (int i = 0; i < initialValues.Length; i++)
			{
				this.Add(initialValues[i]);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PascalSet"/> class.
		/// </summary>
		/// <param name="lowerBound">The lower bound.</param>
		/// <param name="upperBound">The upper bound.</param>
		public PascalSet(int lowerBound, int upperBound)
		{
			if (lowerBound < 0)
			{
				throw new ArgumentException(Resources.LowerBoundLargerThanZero);
			}

			if (upperBound < lowerBound)
			{
				throw new ArgumentException(Resources.UpperBoundMustBeLargerThanLowerBound);
			}

			this.lowerBound = lowerBound;
			this.upperBound = upperBound;

			data = new BitArray(upperBound - lowerBound + 1, false);			
		}

		#endregion

		#region Private Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="PascalSet"/> class.
		/// </summary>
		/// <param name="initialData">The initial data.</param>
		/// <param name="lowerBound">The lower bound.</param>
		/// <param name="upperBound">The upper bound.</param>
		private PascalSet(BitArray initialData, int lowerBound, int upperBound)
		{
			#region Asserts

			Debug.Assert(lowerBound >= 0, "The lower bound must be larger or equal to zero.");
			Debug.Assert(lowerBound <= upperBound, "The upper bound must be larger than the lower bound specified.");
			Debug.Assert(initialData != null, "Initial data can not be null.");
			Debug.Assert((upperBound - lowerBound + 1) == initialData.Length, "The length of the bit array and the number of items in the universe does not match.");

			#endregion

			this.upperBound = upperBound;
			this.lowerBound = lowerBound;

			data = initialData;

			// Recalculate the count for this array
			for (int i = 0; i < data.Length; i++)
			{
				if (data[i])
				{
					count++;
				}
			}
		}

		#endregion

		#endregion

		#region Public Members

		/// <summary>
		/// Gets the lower bound.
		/// </summary>
		/// <value>The lower bound.</value>
		public int LowerBound
		{
			get
			{
				return lowerBound;
			}
		}

		/// <summary>
		/// Gets the upper bound.
		/// </summary>
		/// <value>The upper bound.</value>
		public int UpperBound
		{
			get
			{
				return upperBound;
			}
		}

		/// <summary>
		/// Computes the union of this set and the specified set.
		/// </summary>
		/// <param name="set">The set.</param>
		/// <returns>The union betwen this set and the set specified.</returns>
		public PascalSet Union(PascalSet set)
		{
			if (set == null)
			{
				throw new ArgumentNullException("set");
			}

			CheckIfUniverseTheSame(set);
			return new PascalSet(this.data.Or(set.data), lowerBound, upperBound);
		}

		/// <summary>
		/// Computes the difference between this set and the specified set.
		/// </summary>
		/// <param name="set">The set.</param>
		/// <returns>The result of the difference operation.</returns>
		public PascalSet Difference(PascalSet set)
		{
			if (set == null)
			{
				throw new ArgumentNullException("set");
			}

			CheckIfUniverseTheSame(set);
			return new PascalSet(this.data.Xor(set.data), lowerBound, upperBound);
		}

        /// <summary>
        /// Computes the intersection between this set and the specified set.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <returns>The result of the intersection operation.</returns>
		public PascalSet Intersection(PascalSet set)
		{
			if (set == null)
			{
				throw new ArgumentNullException("set");
			}

			CheckIfUniverseTheSame(set);
			return new PascalSet(this.data.And(set.data), lowerBound, upperBound);
		}

		/// <summary>
		/// Determines whether the specified set is equal to this one.
		/// </summary>
		/// <param name="set">The set.</param>
		/// <returns>
		/// 	<c>true</c> if the specified set is equal this this one; otherwise, <c>false</c>.
		/// </returns>
		public bool IsEqual(PascalSet set)
		{
			if (set == null)
			{
				throw new ArgumentNullException("set");
			}

			if (!IsUniverseTheSame(set)) {
				return false;
			}
			else {
				for (int i = 0; i< data.Length; i++) {
					if (data[i] != set.data[i]) {
						return false;
					}
				}
			}

			return true;
		}

		/// <summary>
		/// Returns a set with items not in this set.
		/// </summary>		
		/// <returns>The set with items not included in this set.</returns>
		public PascalSet Inverse()
		{
			return new PascalSet(data.Not(), lowerBound, upperBound);
		}

		/// <summary>
		/// Determines whether this set is a subset of the specified set.
		/// </summary>
		/// <param name="set">The set to be compared against.</param>
		/// <returns>
		/// 	<c>true</c> if this set is a subset of the specified set]; otherwise, <c>false</c>.
		/// </returns>
		public bool IsSubsetOf(PascalSet set)
		{
			if (set == null)
			{
				throw new ArgumentNullException("set");
			}

			CheckIfUniverseTheSame(set);

			for (int i = 0; i < data.Length; i++)
			{
				if (data[i])
				{
					if (!set.data[i])
					{
						return false;
					}
				}
			}

			return true;
		}

		/// <summary>
		/// Determines whether this set is a proper subset of the specified set.
		/// </summary>
		/// <param name="set">The set to be compared against.</param>
		/// <returns>
		/// 	<c>true</c> if this is a proper subset of the specified set; otherwise, <c>false</c>.
		/// </returns>
		public bool IsProperSubsetOf(PascalSet set)
		{
			if (set == null)
			{
				throw new ArgumentNullException("set");
			}

			CheckIfUniverseTheSame(set);

			// A is a proper subset of a if the b is a subset of a and a != b
			return (this.IsSubsetOf(set) && !set.IsSubsetOf(this));
		}

		/// <summary>
		/// Determines whether this set is a superset of the specified set.
		/// </summary>
		/// <param name="set">The set to be compared against.</param>
		/// <returns>
		/// 	<c>true</c> if this set is a superset of the specified set; otherwise, <c>false</c>.
		/// </returns>
		public bool IsSupersetOf(PascalSet set)
		{
			if (set == null)
			{
				throw new ArgumentNullException("set");
			}

			CheckIfUniverseTheSame(set);

			for (int i = 0; i < data.Length; i++)
			{
				if (set.data[i])
				{
					if (!data[i])
					{
						return false;
					}
				}
			}

			return true;
		}

		/// <summary>
		/// Determines whether this set is a proper superset of the specified set.
		/// </summary>
		/// <param name="set">The set to be compared against.</param>
		/// <returns>
		/// 	<c>true</c> if this is a proper superset of the specified set; otherwise, <c>false</c>.
		/// </returns>
		public bool IsProperSupersetOf(PascalSet set)
		{
			if (set == null)
			{
				throw new ArgumentNullException("set");
			}

			CheckIfUniverseTheSame(set);

			// B is a proper superset of a if b is a superset of a and a != b
			return (this.IsSupersetOf(set) && !set.IsSupersetOf(this));
		}

		#endregion

		#region Operator Overloads

		/// <summary>
		/// Operator + : Performs a union between two sets.
		/// </summary>
		/// <param name="s1">The left hand set.</param>
		/// <param name="s2">The right hand set.</param>
		/// <returns>The result of the union operation.</returns>
		public static PascalSet operator + (PascalSet s1, PascalSet s2) {
			return s1.Union(s2);
		}

		/// <summary>
		/// Operator - : Performs a difference operation between two sets.
		/// </summary>
		/// <param name="s1">The left hand set.</param>
		/// <param name="s2">The right hand set.</param>
		/// <returns>The result of the difference operaiton.</returns>
		public static PascalSet operator -(PascalSet s1, PascalSet s2)
		{
			return s1.Difference(s2);
		}

		/// <summary>
		/// Operator * : Performs a intersection between two sets.
		/// </summary>
		/// <param name="s1">The left hand set.</param>
		/// <param name="s2">The right hand set.</param>
		/// <returns>The result of the intersection operation.</returns>
		public static PascalSet operator *(PascalSet s1, PascalSet s2)
		{
			return s1.Intersection(s2);
		}

		/// <summary>
		/// Operator &lt;= : Checks if the left hand set is a subset of the right hand set.
		/// </summary>
		/// <param name="s1">The left hand set.</param>
		/// <param name="s2">The right hand set.</param>
		/// <returns>A value indicating whether the left hand set is a subset of the right hand set.</returns>
		public static bool operator <=(PascalSet s1, PascalSet s2)
		{
			return s1.IsSubsetOf(s2);
		}

		/// <summary>
		/// Operator &gt;= : Checks if the left hand set is a superset of the right hand set.
		/// </summary>
		/// <param name="s1">The left hand set.</param>
		/// <param name="s2">The right hand set.</param>
		/// <returns>A value indicating whether the left hand set is a superset of the right hand set.</returns>
		public static bool operator >=(PascalSet s1, PascalSet s2)
		{
			return s1.IsSupersetOf(s2);
		}


		/// <summary>
		/// Operator &lt;= : Checks if the left hand set is a proper subset of the right hand set.
		/// </summary>
		/// <param name="s1">The left hand set.</param>
		/// <param name="s2">The right hand set.</param>
		/// <returns>A value indicating whether the left hand set is a proper subset of the right hand set.</returns>
		public static bool operator <(PascalSet s1, PascalSet s2)
		{
			return s1.IsProperSubsetOf(s2);
		}

		/// <summary>
		/// Operator &gt;= : Checks if the left hand set is a proper superset of the right hand set.
		/// </summary>
		/// <param name="s1">The left hand set.</param>
		/// <param name="s2">The right hand set.</param>
		/// <returns>A value indicating whether the left hand set is a proper superset of the right hand set.</returns>
		public static bool operator >(PascalSet s1, PascalSet s2)
		{
			return s1.IsProperSupersetOf(s2);
		}
				
		/// <summary>
		/// Indicates whether an item is present in this set.
		/// </summary>
		/// <value></value>
		public bool this[int i]
		{
			get
			{
				CheckValidIndex(i);
				return data[GetOffSet(i)];
			}
		}

		/// <summary>
		/// Operator ! : Performs the inverse operation on this set.
		/// </summary>
		/// <param name="set">The inverse of set.</param>
		/// <returns>The result of the inverse operation.</returns>
		public static PascalSet operator !(PascalSet set) {
			return set.Inverse();
		}
				
		#endregion

		#region Private Members

		/// <summary>
		/// Gets the offset of the specified index.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns>The offset of the item at the specified index.</returns>
		private int GetOffSet(int item)
		{
			return item - lowerBound;
		}

		/// <summary>
		/// Determines whether [is universe the same] [the specified set].
		/// </summary>
		/// <param name="set">The set.</param>
		/// <returns>
		/// 	<c>true</c> if [is universe the same] [the specified set]; otherwise, <c>false</c>.
		/// </returns>
		private bool IsUniverseTheSame(PascalSet set)
		{
			return
				(set.lowerBound == this.lowerBound) &&
				(set.upperBound == this.upperBound);
		}

		/// <summary>
		/// Checks if the universe is the same.
		/// </summary>
		/// <param name="set">The set.</param>
		private void CheckIfUniverseTheSame(PascalSet set)
		{
			if (!IsUniverseTheSame(set))
			{
				throw new ArgumentException(Resources.UniverseNotTheSame);
			}
		}

		/// <summary>
		/// Determines whether [is index valid] [the specified index].
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns>
		/// 	<c>true</c> if [is index valid] [the specified index]; otherwise, <c>false</c>.
		/// </returns>
		private bool IsIndexValid(int index)
		{
			return ((index >= lowerBound) && (index <= upperBound));
		}

		/// <summary>
		/// Checks if the value supplied is a valid index.
		/// </summary>
		/// <param name="index">The index.</param>
		private void CheckValidIndex(int index)
		{
			if (!IsIndexValid(index))
			{
				throw new ArgumentException(Resources.ItemNotInUniverse);
			}
		}

		#endregion
				
		#region IVisitableCollection<int> Members

		/// <summary>
		/// Adds the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		public void Add(int item)
		{
			CheckValidIndex(item);

			int offset = GetOffSet(item);

			if (!data[offset])
			{
				count++;
				data[offset] = true;
			}
		}

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        /// true if item was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false. This method also returns false if item is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
		public bool Remove(int item)
		{
			CheckValidIndex(item);

			int offset = GetOffSet(item);

			if (data[offset])
			{
				count--;
				data[offset] = false;
				return true;
			}

			return false;
		}

		/// <summary>
		/// Determines whether [contains] [the specified item].
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns>
		/// 	<c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.
		/// </returns>
		public bool Contains(int item)
		{
			CheckValidIndex(item);

			return data[item];
		}

		/// <summary>
		/// Clears all the objects in this instance.
		/// </summary>
		public void Clear()
		{
			data.SetAll(false);
			this.count = 0;
		}

		/// <summary>
		/// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"></see> to an <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"></see>. The <see cref="T:System.Array"></see> must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">arrayIndex is less than 0.</exception>
		/// <exception cref="T:System.ArgumentNullException">array is null.</exception>
		/// <exception cref="T:System.ArgumentException">array is multidimensional.-or-arrayIndex is equal to or greater than the length of array.-or-The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"></see> is greater than the available space from arrayIndex to the end of the destination array.-or-Type T cannot be cast automatically to the type of the destination array.</exception>
		public void CopyTo(int[] array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}

			if ((array.Length - arrayIndex) < this.Count)
			{
				throw new ArgumentException(Resources.NotEnoughSpaceInTargetArray);
			}

			using (IEnumerator<int> enumerator = this.GetEnumerator())
			{

				while (enumerator.MoveNext())
				{
					array.SetValue(enumerator.Current, arrayIndex++);
				}
			}
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
				return count;
			}
		}

		/// <summary>
		/// Accepts the specified visitor.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		public void Accept(NGenerics.Visitors.IVisitor<int> visitor)
		{
			if (visitor == null)
			{
				throw new ArgumentNullException("visitor");
			}

			using (IEnumerator<int> enumerator = this.GetEnumerator()) {
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
				return true;
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

		#endregion

		#region IEnumerable<T> Members

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<int> GetEnumerator()
		{
			for (int i = 0; i < data.Count; i++)
			{
				if (data[i])
				{
					yield return i + lowerBound;
				}
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

			if (this.GetType() == obj.GetType())
			{
				PascalSet set = obj as PascalSet;
				return this.Count.CompareTo(set.Count);
			}
			else
			{
				return this.GetType().FullName.CompareTo(obj.GetType().FullName);
			}
		}

		#endregion

		#region ICollection<int> Members

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
		/// <returns>An enumerator for enumerating though the collection.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion

		#region ISet Members

        /// <summary>
        /// Applies the difference operation to two sets.
        /// </summary>
        /// <param name="set">The other set.</param>
        /// <returns>The result of the difference operation.</returns>
		ISet ISet.Difference(ISet set)
		{
			return this.Difference((PascalSet)set);
		}

        /// <summary>
        /// Applies the Intersection operation to two sets.
        /// </summary>
        /// <param name="set">The other set.</param>
        /// <returns>
        /// The result of the intersection operation.
        /// </returns>
		ISet ISet.Intersection(ISet set)
		{
			return this.Intersection((PascalSet)set);
		}

        /// <summary>
        /// Inverses this instance.
        /// </summary>
        /// <returns>
        /// The Inverse representation of the current set.
        /// </returns>
		ISet ISet.Inverse()
		{
			return this.Inverse();
		}

		/// <summary>
		/// Determines whether the specified set is equal to the current instance.
		/// </summary>
		/// <param name="set">The other set.</param>
		/// <returns>
		/// 	<c>true</c> if the specified set is equal; otherwise, <c>false</c>.
		/// </returns>
		bool ISet.IsEqual(ISet set)
		{
			return this.IsEqual((PascalSet)set);
		}

		/// <summary>
		/// Determines whether the current instance is a proper subset specified set.
		/// </summary>
		/// <param name="set">The set.</param>
		/// <returns>
		/// 	<c>true</c> if [is proper subset of] [the specified set]; otherwise, <c>false</c>.
		/// </returns>
		bool ISet.IsProperSubsetOf(ISet set)
		{
			return this.IsProperSubsetOf((PascalSet)set);
		}

		/// <summary>
		/// Determines whether the current instance is a proper superset of specified set.
		/// </summary>
		/// <param name="set">The set.</param>
		/// <returns>
		/// 	<c>true</c> if [is proper superset of] [the specified set]; otherwise, <c>false</c>.
		/// </returns>
		bool ISet.IsProperSupersetOf(ISet set)
		{
			return this.IsProperSupersetOf((PascalSet)set);
		}

		/// <summary>
		/// Determines whether the current instance is a subset of the specified set.
		/// </summary>
		/// <param name="set">The set.</param>
		/// <returns>
		/// 	<c>true</c> if [is subset of] [the specified set]; otherwise, <c>false</c>.
		/// </returns>
		bool ISet.IsSubsetOf(ISet set)
		{
			return this.IsSubsetOf((PascalSet)set);
		}

		/// <summary>
		/// Determines whether the current instance is a superset of the specified set.
		/// </summary>
		/// <param name="set">The set.</param>
		/// <returns>
		/// 	<c>true</c> if [is superset of] [the specified set]; otherwise, <c>false</c>.
		/// </returns>
		bool ISet.IsSupersetOf(ISet set)
		{
			return this.IsSupersetOf((PascalSet)set);
		}

        /// <summary>
        /// Performs the union operation on two sets.
        /// </summary>
        /// <param name="set">The set.</param>
        /// <returns>
        /// The union of this set and the set specified.
        /// </returns>
		ISet ISet.Union(ISet set)
		{
			return this.Union((PascalSet)set);
		}

		#endregion
	}
}
