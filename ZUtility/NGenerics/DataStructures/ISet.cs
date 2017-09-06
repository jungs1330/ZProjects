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
	/// An interface for the Set data structure
	/// </summary>
	public interface ISet
	{
		/// <summary>
		/// Applies the difference operation to two sets.
		/// </summary>
		/// <param name="set">The other set.</param>
		/// <returns>The result of the difference operation.</returns>
		ISet Difference(ISet set);

		/// <summary>
		/// Applies the Intersection operation to two sets.
		/// </summary>
		/// <param name="set">The other set.</param>
		/// <returns>The result of the intersection operation.</returns>
		ISet Intersection(ISet set);

		/// <summary>
		/// Inverses this instance.
		/// </summary>
		/// <returns>The Inverse representation of the current set.</returns>
		ISet Inverse();

		/// <summary>
		/// Determines whether the specified set is equal to the current instance.
		/// </summary>
		/// <param name="set">The other set.</param>
		/// <returns>
		/// 	<c>true</c> if the specified set is equal; otherwise, <c>false</c>.
		/// </returns>
		bool IsEqual(ISet set);

		/// <summary>
		/// Determines whether the current instance is a proper subset specified set.
		/// </summary>
		/// <param name="set">The set.</param>
		/// <returns>
		/// 	<c>true</c> if [is proper subset of] [the specified set]; otherwise, <c>false</c>.
		/// </returns>
		bool IsProperSubsetOf(ISet set);

		/// <summary>
		/// Determines whether the current instance is a proper superset of specified set.
		/// </summary>
		/// <param name="set">The set.</param>
		/// <returns>
		/// 	<c>true</c> if [is proper superset of] [the specified set]; otherwise, <c>false</c>.
		/// </returns>
		bool IsProperSupersetOf(ISet set);

		/// <summary>
		/// Determines whether the current instance is a subset of the specified set.
		/// </summary>
		/// <param name="set">The set.</param>
		/// <returns>
		/// 	<c>true</c> if [is subset of] [the specified set]; otherwise, <c>false</c>.
		/// </returns>
		bool IsSubsetOf(ISet set);

		/// <summary>
		/// Determines whether the current instance is a superset of the specified set.
		/// </summary>
		/// <param name="set">The set.</param>
		/// <returns>
		/// 	<c>true</c> if [is superset of] [the specified set]; otherwise, <c>false</c>.
		/// </returns>
		bool IsSupersetOf(ISet set);

		/// <summary>
		/// Performs the union operation on two sets.
		/// </summary>
		/// <param name="set">The set.</param>
		/// <returns>The union of this set and the set specified.</returns>
		ISet Union(ISet set);
	}
}
