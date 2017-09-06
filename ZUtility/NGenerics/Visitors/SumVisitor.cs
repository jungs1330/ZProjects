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

namespace NGenerics.Visitors
{
	/// <summary>
	/// A visitor that sums integers in any collection of type int.
	/// </summary>
	public sealed class SumVisitor : IVisitor<int>
	{
		#region Globals

		private int sum;

		#endregion

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="SumVisitor"/> class.
		/// </summary>
		public SumVisitor() { }

		#endregion

		#region IVisitor<int> Members

		/// <summary>
		/// Visits the specified object.
		/// </summary>
		/// <param name="obj">The object.</param>
		public void Visit(int obj)
		{
			sum += obj;
		}

		/// <summary>
		/// Gets a value indicating whether this instance is done performing it's work..
		/// </summary>
        /// <returns><c>true</c> if this instance is done; otherwise, <c>false</c>.</returns>
		/// <value><c>true</c> if this instance is done; otherwise, <c>false</c>.</value>
		public bool HasCompleted
		{
			get
			{
				return false;
			}
		}

		#endregion

		#region Public Members

		/// <summary>
		/// Gets the sum accumulated by this SumVisitor.
		/// </summary>
		/// <value>The sum.</value>
		public int Sum
		{
			get
			{
				return sum;
			}
		}

		#endregion
	}
}
