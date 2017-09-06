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

namespace NGenerics.Algorithms.Math
{
	/// <summary>
	/// A class using Euclid's algorithm for providing the greatest common divisor.
	/// </summary>
	public static class Euclid
	{
		#region Public Methods

        /// <summary>
        /// Finds the greatest common divisor.
        /// </summary>
        /// <param name="x">The first number.  Must be larger than y.</param>
        /// <param name="y">The second number</param>
        /// <returns>The greatest common divisor between the two integers supplied.</returns>
		public static int FindGreatestCommonDivisor(int x, int y)
		{
			if ((y < 0) || (x < 0))
			{
				throw new ArgumentException(Resources.NumbersGreaterThanZero);
			}

			// This algorithm only works if x >= y.
			// If x < y, swap the two variables.
			if (x < y)
			{
				int tmp = x;
				x = y;
				y = tmp;
			}			

			return FindGreatestCommonDivisorInternal(x, y);
		}

		#endregion

		#region Private Members

		private static int FindGreatestCommonDivisorInternal(int x, int y)
		{
			if (y == 0)
			{
				return x;
			}
			else
			{
				return FindGreatestCommonDivisorInternal(y, x % y);
			}
		}
			
		#endregion

	}
}
