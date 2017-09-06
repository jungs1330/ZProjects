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
	/// An Interface for the Stack data structure
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IStack<T> 
	{
		/// <summary>
		/// Pushes the specified item onto the stack.
		/// </summary>
		/// <param name="item">The item.</param>
		void Push(T item);

		/// <summary>
		/// Pops this instance from the stack.
		/// </summary>
		/// <returns>The item at the top of the stack.</returns>
		T Pop();

		/// <summary>
		/// Peeks at the top item without popping it from the stack.
		/// </summary>
		/// <returns>The item at the top of the stack.</returns>
		T Peek();
	}
}
