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
	/// A class generating Fibonacci numbers.  Note that this class is far from complete, or efficient - it
	/// will need to be implemented with the golden ratio, as seen at http://www.math.utah.edu/~beebe/software/java/fibonacci/.
	/// </summary>
	public static class Fibonacci
	{
		/// <summary>
		/// Generates the Nth Fibonacci number.
		/// </summary>
		/// <param name="n">The Nth Fibonacci number.</param>
		/// <returns>The Nth number in the Fibonacci sequence.</returns>
		public static long GenerateNthFibonacci(int n) {

			return GenerateFibonacciSequence(n)[n];
		}

		/// <summary>
		/// Generates the Nth Fibonacci number.
		/// </summary>
		/// <param name="upperBoundN">The upper bound N.</param>
		/// <returns>A series of Fibonacci numbers until the upperBoundN number.</returns>
		public static long[] GenerateFibonacciSequence(int upperBoundN) {
			
			if (upperBoundN < 0) {
				throw new ArgumentOutOfRangeException(Resources.SetIndexMustBePostive);
			}

			long[] numbers = new long[upperBoundN + 1];

			numbers[0] = 0;

			if (upperBoundN >= 1)
			{
				numbers[1] = 1;

				for (int i = 2; i <= upperBoundN; i++)
				{
					numbers[i] = numbers[i - 1] + numbers[i - 2];
				}
			}

			return numbers;
		}
	}
}
