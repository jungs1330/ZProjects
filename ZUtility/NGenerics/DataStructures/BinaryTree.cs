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
using NGenerics.Visitors;
using NGenerics.Misc;

namespace NGenerics.DataStructures
{
    /// <summary>
    /// An implementation of a Binary Tree data structure.
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public class BinaryTree<T> : IVisitableCollection<T>, ITree<T>
	{
		#region Globals

		private BinaryTree<T> leftSubtree;
		private BinaryTree<T> rightSubtree;
		private T data;

		#endregion

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryTree&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="data">The data contained in this node.</param>
		public BinaryTree(T data) : this(data, null, null) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryTree&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <param name="left">The data of the left subtree.</param>
		/// <param name="right">The data of the right subtree.</param>
		public BinaryTree(T data, T left, T right) : this(data, new BinaryTree<T>(left), new BinaryTree<T>(right)) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryTree&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="data">The data contained in this node.</param>
		/// <param name="left">The left subtree.</param>
		/// <param name="right">The right subtree.</param>
		public BinaryTree(T data, BinaryTree<T> left, BinaryTree<T> right)
		{
			leftSubtree = left;
			rightSubtree = right;
			this.data = data;
		}

		#endregion

		#region IVisitableCollection<T> Members

		/// <summary>
		/// Accepts the specified visitor.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		public void Accept(IVisitor<T> visitor)
		{
			if (visitor == null)
			{
				throw new ArgumentNullException("visitor");
			}

			using (IEnumerator<T> enumerator = this.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					visitor.Visit(enumerator.Current);

					if (visitor.HasCompleted)
					{
						break;
					}
				}
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is of a fixed size.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is fixed size; otherwise, <c>false</c>.
		/// </value>
		public bool IsFixedSize
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this collection is empty.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this collection is empty; otherwise, <c>false</c>.
		/// </value>
		public bool IsEmpty
		{
			get
			{
				return this.Count == 0;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this collection is full.
		/// </summary>
		/// <value><c>true</c> if this collection is full; otherwise, <c>false</c>.</value>
		public bool IsFull
		{
			get
			{
				return (this.leftSubtree != null) && (this.rightSubtree != null);
			}
		}

		/// <summary>
		/// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> contains a specific value.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
		/// <returns>
		/// true if item is found in the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false.
		/// </returns>
		public bool Contains(T item)
		{
			using (IEnumerator<T> enumerator = this.GetEnumerator())
			{

				while (enumerator.MoveNext())
				{
					if (item.Equals(enumerator.Current))
					{
						return true;
					}
				}
			}


			return false;
		}

		/// <summary>
		/// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"></see> to an <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"></see>. The <see cref="T:System.Array"></see> must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">arrayIndex is less than 0.</exception>
		/// <exception cref="T:System.ArgumentNullException">array is null.</exception>
		/// <exception cref="T:System.ArgumentException">array is multidimensional.-or-arrayIndex is equal to or greater than the length of array.-or-The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"></see> is greater than the available space from arrayIndex to the end of the destination array.-or-Type T cannot be cast automatically to the type of the destination array.</exception>
		public void CopyTo(T[] array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}

			using (IEnumerator<T> enumerator = this.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (arrayIndex >= array.Length)
					{
						throw new ArgumentException(Resources.NotEnoughSpaceInTargetArray);
					}

					array[arrayIndex++] = enumerator.Current;
				}
			}
		}

		/// <summary>
		/// Gets the number of elements contained in the <see cref="T:System.Collections.ICollection"></see>.
		/// </summary>
		/// <value></value>
		/// <returns>The number of elements contained in the <see cref="T:System.Collections.ICollection"></see>.</returns>
		public int Count
		{
			get
			{
				int count = 0;

				if (leftSubtree != null)
				{
					count++;
				}

				if (rightSubtree != null)
				{
					count++;
				}

				return count;
			}
		}

		/// <summary>
		/// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
		/// </summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
		public void Add(T item)
		{
			Add(new BinaryTree<T>(item));
		}

		/// <summary>
		/// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
		/// </summary>
		/// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
		/// <returns>
		/// true if item was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false. This method also returns false if item is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"></see>.
		/// </returns>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
		public bool Remove(T item)
		{
			if (leftSubtree != null)
			{
				if (leftSubtree.data.Equals(item))
				{
					leftSubtree = null;
					return true;
				}
			}

			if (rightSubtree != null)
			{
				if (rightSubtree.data.Equals(item))
				{
					rightSubtree = null;
					return true;
				}
			}

			return false;
		}

        /// <summary>
        /// Removes the specified child.
        /// </summary>
        /// <param name="child">The child.</param>
        /// <returns>A value indicating whether the child was found (and removed) from this tree.</returns>
		public bool Remove(BinaryTree<T> child)
		{
			if (leftSubtree != null)
			{
				if (leftSubtree == child)
				{
					RemoveLeft();
					return true;
				}
			}

			if (rightSubtree != null)
			{
				if (rightSubtree == child)
				{
					RemoveRight();
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<T> GetEnumerator()
		{
			VisitableStack<BinaryTree<T>> stack = new VisitableStack<BinaryTree<T>>();

			stack.Push(this);

			while (!stack.IsEmpty)
			{
				BinaryTree<T> tree = stack.Pop();

				yield return tree.Data;

				if (tree.leftSubtree != null)
				{
					stack.Push(tree.leftSubtree);
				}

				if (tree.rightSubtree != null)
				{
					stack.Push(tree.rightSubtree);
				}
			}
		}

		/// <summary>
		/// Clears all the objects in this instance.
		/// </summary>
		public void Clear()
		{
			leftSubtree = null;
			rightSubtree = null;
		}

		/// <summary>
		/// Compares the current instance with another object of the same type.
		/// </summary>
		/// <param name="obj">An object to compare with this instance.</param>
		/// <returns>
		/// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance is less than obj. Zero This instance is equal to obj. Greater than zero This instance is greater than obj.
		/// </returns>
		/// <exception cref="T:System.ArgumentException">obj is not the same type as this instance. </exception>
		public int CompareTo(object obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}

			if (obj.GetType() == this.GetType())
			{
				BinaryTree<T> tree = obj as BinaryTree<T>;

				return this.Count.CompareTo(tree.Count);
			}
			else
			{
				return this.GetType().FullName.CompareTo(obj.GetType().FullName);
			}
		}

		#endregion

		#region ITree<T> Members

		/// <summary>
		/// Adds the specified child to the tree.
		/// </summary>
		/// <param name="child">The child to add..</param>
		void ITree<T>.Add(ITree<T> child)
		{
			this.Add((BinaryTree<T>)child);
		}

		/// <summary>
		/// Gets the child at the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns>The child at the specific index.</returns>
		ITree<T> ITree<T>.GetChild(int index)
		{
			return this.GetChild(index);
		}

		/// <summary>
		/// Removes the specified child.
		/// </summary>
		/// <param name="child">The child.</param>
		/// <returns>A value indicating whether the specified tree was found.</returns>
		bool ITree<T>.Remove(ITree<T> child)
		{
			return this.Remove((BinaryTree<T>)child);
		}

		/// <summary>
		/// Finds the node for which the given predicate holds true.
		/// </summary>
		/// <param name="condition">The condition to test on the data item.</param>
		/// <returns>The first node that matches the condition supplied.  If a node is not found, null is returned.</returns>
		ITree<T> ITree<T>.FindNode(Predicate<T> condition)
		{
			return this.FindNode(condition);
		}

		#endregion

		#region Public Members

		/// <summary>
		/// Finds the node with the specified condition.  If a node is not found matching
		/// the specified condition, null is returned.
		/// </summary>
		/// <param name="condition">The condition to test.</param>
        /// <returns>The first node that matches the condition supplied.  If a node is not found, null is returned.</returns>
		public BinaryTree<T> FindNode(Predicate<T> condition)
		{
			if (condition == null)
			{
				throw new ArgumentNullException("condition");
			}

			if (condition.Invoke(this.Data))
			{
				return this;
			}
			else
			{
				if (leftSubtree != null)
				{
					BinaryTree<T> ret = leftSubtree.FindNode(condition);

					if (ret != null)
					{
						return ret;
					}
				}

				if (rightSubtree != null)
				{
					BinaryTree<T> ret = rightSubtree.FindNode(condition);

					if (ret != null)
					{
						return ret;
					}
				}
			}

			return null;
		}

		/// <summary>
		/// Gets or sets the left subtree.
		/// </summary>
		/// <value>The left subtree.</value>
		public virtual BinaryTree<T> Left
		{
			get
			{
				return leftSubtree;
			}
			set
			{
				leftSubtree = value;
			}
		}

		/// <summary>
		/// Gets or sets the right subtree.
		/// </summary>
		/// <value>The right subtree.</value>
		public virtual BinaryTree<T> Right
		{
			get
			{
				return rightSubtree;
			}
			set
			{
				rightSubtree = value;
			}
		}

		/// <summary>
		/// Gets or sets the value contained in this node.
		/// </summary>
		/// <value>The value contained in this node.</value>
		public virtual T Data
		{
			get
			{
				return data;
			}
			set
			{
				data = value;
			}
		}

		/// <summary>
		/// Gets the degree (number of childnodes) of this node.
		/// </summary>
		/// <value>The degree of this node.</value>
		public int Degree
		{
			get
			{
				return Count;
			}
		}

        /// <summary>
        /// Gets the child at the specified index.
        /// </summary>
        /// <param name="index">The index of the child in question.</param>
        /// <returns>The child at the specified index.</returns>
		public BinaryTree<T> GetChild(int index)
		{
			if (index == 0)
			{
				return leftSubtree;
			}
			else if (index == 1)
			{
				return rightSubtree;
			}
			else
			{
				throw new ArgumentOutOfRangeException();
			}
		}
		/// <summary>
		/// Gets the height of the this tree.
		/// </summary>
		/// <value>The height.</value>
		public virtual int Height
		{
			get
			{
				if (this.Degree == 0)
				{
					return 0;
				}
				else
				{
					return 1 + FindMaximumChildHeight();
				}
			}
		}


		/// <summary>
		/// Performs a depth first traversal on this tree with the specified visitor.
		/// </summary>
		/// <param name="orderedVisitor">The ordered visitor.</param>
		public virtual void DepthFirstTraversal(OrderedVisitor<T> orderedVisitor)
		{
			if (orderedVisitor.HasCompleted)
			{
				return;
			}
			else
			{
				// Preorder visit
				orderedVisitor.VisitPreOrder(Data);

				if (leftSubtree != null)
				{
					leftSubtree.DepthFirstTraversal(orderedVisitor);
				}

				// Inorder visit
				orderedVisitor.VisitInOrder(data);

				if (rightSubtree != null)
				{
					rightSubtree.DepthFirstTraversal(orderedVisitor);
				}

				// PostOrder visit
				orderedVisitor.VisitPostOrder(Data);
			}
		}

		/// <summary>
		/// Performs a breadth first traversal on this tree with the specified visitor.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		public virtual void BreadthFirstTraversal(IVisitor<T> visitor)
		{
			VisitableQueue<BinaryTree<T>> q = new VisitableQueue<BinaryTree<T>>();

			q.Enqueue(this);

			while (!q.IsEmpty)
			{
				BinaryTree<T> t = q.Dequeue();
				visitor.Visit(t.Data);

				for (int i = 0; i < t.Degree; i++)
				{
					BinaryTree<T> child = t.GetChild(i);

					if (child != null)
					{
						q.Enqueue(child);
					}
				}
			}
		}


		/// <summary>
		/// Gets a value indicating whether this instance is leaf node.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is leaf node; otherwise, <c>false</c>.
		/// </value>
		public virtual bool IsLeafNode
		{
			get
			{
				return this.Degree == 0;
			}
		}

		/// <summary>
		/// Removes the left child.
		/// </summary>
		public virtual void RemoveLeft()
		{
			leftSubtree = null;
		}

		/// <summary>
		/// Removes the left child.
		/// </summary>
		public virtual void RemoveRight()
		{
			rightSubtree = null;
		}

		/// <summary>
		/// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
		/// </summary>
		/// <param name="subtree">The subtree.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
		public void Add(BinaryTree<T> subtree)
		{
			if (leftSubtree == null)
			{
				leftSubtree = subtree;
			}
			else if (rightSubtree == null)
			{
				rightSubtree = subtree;
			}
			else
			{
				throw new InvalidOperationException("This binary tree is full.");
			}
		}


		#endregion

		#region Private Members

		/// <summary>
		/// Finds the maximum height between the childnodes.
		/// </summary>
		/// <returns>The maximum height of the tree between all paths from this node and all leaf nodes.</returns>
		protected virtual int FindMaximumChildHeight()
		{
			int leftHeight = 0;
			int rightHeight = 0;

			if (leftSubtree != null)
			{
				leftHeight = leftSubtree.Height;
			}

			if (rightSubtree != null)
			{
				rightHeight = rightSubtree.Height;
			}

			return (leftHeight > rightHeight) ? leftHeight : rightHeight;
		}

		#endregion

		#region Operator Overloads

		/// <summary>
		/// Gets the <see cref="BinaryTree&lt;T&gt;"/> at the specified index.
		/// </summary>
		/// <value></value>
		public BinaryTree<T> this[int i]
		{
			get
			{
				return GetChild(i);
			}
		}

		#endregion

		#region ICollection<T> Members

		/// <summary>
		/// Gets a value indicating whether this instance is read only.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is read only; otherwise, <c>false</c>.
		/// </value>
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		#endregion

		#region IEnumerable Members

		/// <summary>
		/// Gets the enumerator.
		/// </summary>
		/// <returns>A enumerator to enumerate though the collection.</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion
	}
}
