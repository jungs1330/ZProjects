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

namespace NGenerics.DataStructures
{
	/// <summary>
	///  An interface for a Bag data structure.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IBag<T>
	{

		/// <summary>
		/// Adds n * the specified item to the bag.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <param name="amount">The amount.</param>
		void Add(T item, int amount);

		/// <summary>
		/// Applies the Difference operation on two bags.
		/// </summary>
		/// <param name="bag">The other bag.</param>
		/// <returns>The difference between the current bag and the specified bag.</returns>
		IBag<T> Difference(IBag<T> bag);

		/// <summary>
		/// Applies the Intersection opertion on two bags.
		/// </summary>
		/// <param name="bag">The other bag.</param>
		/// <returns>The intersection of the current bag and the specified bag.</returns>
		IBag<T> Intersection(IBag<T> bag);

		/// <summary>
		/// Determines whether the specified bag is equal to this instance.
		/// </summary>
		/// <param name="bag">The bag to compare.</param>
		/// <returns>
		/// 	<c>true</c> if the specified bag is equal; otherwise, <c>false</c>.
		/// </returns>
		bool IsEqual(IBag<T> bag);

		/// <summary>
		/// Removes the specified amount of items from the bag.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <param name="max">The maximum amount of items to remove.</param>
		/// <returns>An indication of whether the items were found (and removed).</returns>
		bool Remove(T item, int max);

		/// <summary>
		/// Gets the count of the specified item contained in the bag.
		/// </summary>
		/// <value></value>
		int this[T item] { get; }

		/// <summary>
		/// Applies the Union operation with two bags.
		/// </summary>
		/// <param name="bag">The other bag.</param>
		/// <returns>The union of the current bag and the specified bag.</returns>
		IBag<T> Union(IBag<T> bag);
	}
}
