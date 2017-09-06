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
    /// An interface for a Mathematical matrix as in Linear Algebra.
    /// </summary>
	public interface IMathematicalMatrix : IMatrix<double>
	{
		/// <summary>
		/// Gets a value indicating whether this matrix instance is symmetric.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this matrix instance is symmetric; otherwise, <c>false</c>.
		/// </value>
		bool IsSymmetric { get; }
		
		/// <summary>
		/// Inverts this instance.  All values are multiplied with -1.
		/// </summary>
		/// <returns>The inverted representation of this instance.</returns>
		IMathematicalMatrix Invert();

		/// <summary>
		/// Subtracts the matrices according to the linear algebra operator -.
		/// </summary>
		/// <param name="matrix">The result of the subtraction.</param>
		/// <returns>The result of the minus operation.</returns>
		IMathematicalMatrix Minus(IMathematicalMatrix matrix);

		/// <summary>
		/// Adds to matrices according to the linear algebra operator +.
		/// </summary>
		/// <param name="matrix">The result of the addition.</param>
		/// <returns>The result of the plus operation.</returns>
		IMathematicalMatrix Plus(IMathematicalMatrix matrix);

        /// <summary>
        /// Times the matrices according to the linear algebra operator *.
        /// </summary>
        /// <param name="matrix">The matrix.</param>
        /// <returns>The result of the times operation.</returns>
		IMathematicalMatrix Times(IMathematicalMatrix matrix);

		/// <summary>
		/// Times the matrices according to the linear algebra operator *.
		/// </summary>
		/// <param name="number">The number to multiply this matrix with.</param>
		/// <returns>The result of the times operation.</returns>
		IMathematicalMatrix Times(double number);

		/// <summary>
		/// Transposes the matrix.
		/// </summary>
		/// <returns>The transposed representation of this matrix.</returns>
		/// <value>The transposed matrix.</value>
		IMathematicalMatrix Transpose();
	}
}
