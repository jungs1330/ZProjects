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

namespace NGenerics.DataStructures
{
	/// <summary>
	/// An interface for a dequeue
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IDeque<T>
	{
		/// <summary>
		/// Dequeues the head.
		/// </summary>
		/// <returns>The head of the deque.</returns>
		T DequeueHead();

		/// <summary>
		/// Dequeues the tail.
		/// </summary>
		/// <returns>The tail of the deque.</returns>
		T DequeueTail();

		/// <summary>
		/// Enqueues the head.
		/// </summary>
		/// <param name="obj">The obj.</param>
		void EnqueueHead(T obj);

		/// <summary>
		/// Enqueues the tail.
		/// </summary>
		/// <param name="obj">The obj.</param>
		void EnqueueTail(T obj);

		/// <summary>
		/// Gets the head.
		/// </summary>
		/// <value>The head.</value>
		T Head { get; }

		/// <summary>
		/// Gets the tail.
		/// </summary>
		/// <value>The tail.</value>
		T Tail { get; }
	}
}
