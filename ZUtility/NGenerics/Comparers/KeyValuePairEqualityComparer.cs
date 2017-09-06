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

namespace NGenerics.Comparers
{
	/// <summary>
	/// An Equality Comparer for comparing KeyValue pairs.
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
	public sealed class KeyValuePairEqualityComparer<TKey, TValue> : IEqualityComparer<KeyValuePair<TKey, TValue>> 
		where TKey : IComparable 
		where TValue: IComparable
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyValuePairEqualityComparer&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        public KeyValuePairEqualityComparer() { }

        #endregion

        #region IEqualityComparer<KeyValuePair<TKey,TValue>> Members

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object of type T to compare.</param>
        /// <param name="y">The second object of type T to compare.</param>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
		public bool Equals(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y)
		{
			return (x.Key.CompareTo(y.Key) == 0) && (x.Value.CompareTo(y.Value) == 0);
		}

        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"></see> for which a hash code is to be returned.</param>
        /// <returns>A hash code for the specified object.</returns>
        /// <exception cref="T:System.ArgumentNullException">The type of obj is a reference type and obj is null.</exception>
		public int GetHashCode(KeyValuePair<TKey, TValue> obj)
		{
			return base.GetHashCode();
		}

		#endregion
	}
}
