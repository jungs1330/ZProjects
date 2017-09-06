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

namespace NGenerics.DataStructures
{
	/// <summary>
	/// The Association performs the same function as a KeyValuePair, but allows the Key and 
	/// Value members to be written to.
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TValue"></typeparam>
	public sealed class Association<TKey, TValue>
	{
		#region Globals

		private TKey thisKey;
		private TValue thisValue;

		#endregion

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="Association&lt;TKey, TValue&gt;"/> class.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		public Association(TKey key, TValue value)
		{
			thisKey = key;
			thisValue = value;
		}

		#endregion

		#region Public Members

		/// <summary>
		/// Gets the key.
		/// </summary>
		/// <value>The key.</value>
		public TKey Key
		{
			get
			{
				return thisKey;
			}
			set
			{
				thisKey = value;
			}
		}

		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <value>The value.</value>
		public TValue Value
		{
			get
			{
				return thisValue;
			}
			set
			{
				thisValue = value;
			}
		}

        /// <summary>
        /// Construct a KeyValuePair object from the current values.
        /// </summary>
        /// <returns>A key value pair representation of this Association.</returns>
        public KeyValuePair<TKey, TValue> ToKeyValuePair()
        {
            return new KeyValuePair<TKey, TValue>(thisKey, thisValue);
        }

		#endregion
	}
}
