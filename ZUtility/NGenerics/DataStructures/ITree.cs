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
	/// An interface for the tree data structure
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface ITree<T>
	{
		/// <summary>
		/// Adds the specified child to the tree.
		/// </summary>
		/// <param name="child">The child to add..</param>
		void Add(ITree<T> child);

        /// <summary>
        /// Gets the data held in this node.
        /// </summary>
        /// <value>The data.</value>
		T Data { get; }

		/// <summary>
		/// Gets the degree of this node.
		/// </summary>
		/// <value>The degree of this node.</value>
		int Degree { get; }

		/// <summary>
		/// Gets the child at the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns>The child at the specified index.</returns>
		ITree<T> GetChild(int index);

		/// <summary>
		/// Gets the height of this tree..
		/// </summary>
		/// <value>The height of this tree.</value>
		int Height { get; }

		/// <summary>
		/// Gets a value indicating whether this instance is leaf node.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is leaf node; otherwise, <c>false</c>.
		/// </value>
		bool IsLeafNode { get; }

		/// <summary>
		/// Removes the specified child.
		/// </summary>
		/// <param name="child">The child.</param>
		/// <returns>An indication of whether the child was found (and removed) from this tree.</returns>
		bool Remove(ITree<T> child);

		/// <summary>
		/// Finds the node for which the given predicate holds true.
		/// </summary>
		/// <param name="condition">The condition to test on the data item.</param>
		/// <returns>The fist node that matches the condition if found, otherwise null.</returns>
		ITree<T> FindNode(Predicate<T> condition);
	}
}
