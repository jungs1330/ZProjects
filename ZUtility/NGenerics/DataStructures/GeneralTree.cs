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
using NGenerics.Sorting;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace NGenerics.DataStructures
{
	/// <summary>
	/// A general tree data structure that can hold any amount of nodes.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class GeneralTree<T> : IVisitableCollection<T>, ITree<T>, ISortable<GeneralTree<T>>
	{
		#region Globals

		private T nodeData;
        private GeneralTree<T> parent = null;
		private VisitableList<GeneralTree<T>> childNodes = new VisitableList<GeneralTree<T>>();

		#endregion

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="GeneralTree&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="data">The data held in this tree.</param>
		public GeneralTree(T data)
		{
			this.nodeData = data;
		}

		#endregion

		#region IVisitableCollection<T>  Members

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
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<T> GetEnumerator()
		{
			VisitableStack<GeneralTree<T>> stack = new VisitableStack<GeneralTree<T>>();

			stack.Push(this);

			while (!stack.IsEmpty)
			{
				GeneralTree<T> tree = stack.Pop();

				if (tree != null)
				{
					yield return tree.Data;

					for (int i = 0; i < tree.Degree; i++)
					{
						stack.Push(tree.GetChild(i));
					}
				}
			}
		}

		#endregion

		#region VisitableCollection<T> Members

		/// <summary>
		/// Gets the number of elements contained in the <see cref="T:System.Collections.ICollection"></see>.
		/// </summary>
		/// <value></value>
		/// <returns>The number of elements contained in the <see cref="T:System.Collections.ICollection"></see>.</returns>
		public int Count
		{
			get
			{
				return childNodes.Count;
			}
		}

		/// <summary>
		/// Clears all the objects in this instance.
		/// </summary>
		public void Clear()
		{
			childNodes.Clear();
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
				return (this.Count == 0);
			}
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
				GeneralTree<T> t = obj as GeneralTree<T>;
				return this.Count.CompareTo(t.Count);
			}
			else
			{
				return this.GetType().FullName.CompareTo(obj.GetType().FullName);
			}
		}

		/// <summary>
		/// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
		/// </summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
		public void Add(T item)
		{
            GeneralTree<T> child = new GeneralTree<T>(item);
			childNodes.Add(child);
            child.parent = this;
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
			for (int i = 0; i < childNodes.Count; i++)
			{
				if (childNodes[i].Data.Equals(item))
				{
                    childNodes[i].parent = null;
					childNodes.RemoveAt(i);
					return true;
				}
			}

			return false;
		}

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only; otherwise, false.</returns>
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

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
				return false;
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
				return false;
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
			this.Add((GeneralTree<T>)child);
		}

		/// <summary>
		/// Gets the child at the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns>The child node at the specified index.</returns>
		ITree<T> ITree<T>.GetChild(int index)
		{
			return this.GetChild(index);
		}

		/// <summary>
		/// Removes the specified child.
		/// </summary>
		/// <param name="child">The child.</param>
		/// <returns>A value indicating whether the child was found in this tree.</returns>
		bool ITree<T>.Remove(ITree<T> child)
		{
			return this.Remove((GeneralTree<T>)child);
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
        /// Retrieves the Ancestors of this node, in the same order
        /// as the path from the current node to the root.
        /// </summary>
        /// <value>The ancestors.</value>
        public GeneralTree<T>[] Ancestors
        {
            get
            {
                List<GeneralTree<T>> list = new List<GeneralTree<T>>();

                GeneralTree<T> currentNode = this;

                while (currentNode.parent != null)
                {
                    list.Add(currentNode.parent);
                    currentNode = currentNode.parent;
                }

                return list.ToArray();
            }
        }

        /// <summary>
        /// Gets the child nodes of this node.
        /// </summary>
        /// <value>The child nodes.</value>
        public IVisitableList<GeneralTree<T>> ChildNodes
        {
            get
            {
                return childNodes;
            }
        }

        /// <summary>
        /// Gets the parent of this node.
        /// </summary>
        /// <value>The parent of this node.</value>
        public GeneralTree<T> Parent
        {
            get
            {
                return parent;
            }
            internal set
            {
                parent = value;
            }
        }

        /// <summary>
		/// Gets the degree (number of childnodes).
		/// </summary>
		/// <value>The degree.</value>
		public int Degree
		{
			get
			{
				return childNodes.Count;
			}
		}

		/// <summary>
		/// Finds the node with the specified condition.  If a node is not found matching
		/// the specified condition, null is returned.
		/// </summary>
		/// <param name="condition">The condition to test.</param>
        /// <returns>The first node that matches the condition supplied.  If a node is not found, null is returned.</returns>
		public GeneralTree<T> FindNode(Predicate<T> condition)
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
				for (int i = 0; i < this.Degree; i++)
				{
					GeneralTree<T> ret = childNodes[i].FindNode(condition);

					if (ret != null)
					{
						return ret;
					}
				}
			}

			return null;
		}

		/// <summary>
		/// Gets the child at the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns>The child at the specified index.</returns>
		public GeneralTree<T> GetChild(int index)
		{
			CheckValidIndex(index);
			return childNodes[index];
		}

		/// <summary>
		/// Gets the height of the this tree.
		/// </summary>
		/// <value>The height.</value>
		public int Height
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
		public void DepthFirstTraversal(OrderedVisitor<T> orderedVisitor)
		{
			if (orderedVisitor.HasCompleted)
			{
				return;
			}
			else
			{
				orderedVisitor.VisitPreOrder(Data);

				for (int i = 0; i < Degree; i++)
				{
					if (GetChild(i) != null)
					{
						GetChild(i).DepthFirstTraversal(orderedVisitor);
					}
				}

				orderedVisitor.VisitPostOrder(Data);
			}
		}

		/// <summary>
		/// Performs a breadth first traversal on this tree with the specified visitor.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		public void BreadthFirstTraversal(IVisitor<T> visitor)
		{
			VisitableQueue<GeneralTree<T>> q = new VisitableQueue<GeneralTree<T>>();

			q.Enqueue(this);

			while (!q.IsEmpty)
			{
				GeneralTree<T> t = q.Dequeue();
				visitor.Visit(t.Data);

				for (int i = 0; i < t.Degree; i++)
				{
					GeneralTree<T> child = t.GetChild(i);

					if (child != null)
					{
						q.Enqueue(child);
					}
				}
			}
		}

		/// <summary>
		/// Gets the data.
		/// </summary>
		/// <value>The data.</value>
		public T Data
		{
			get
			{
				return nodeData;
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
		/// Adds the child tree to this node.
		/// </summary>
		/// <param name="child">The child tree to add.</param>
		public void Add(GeneralTree<T> child)
		{
            if (child.parent != null)
            {
                child.parent.Remove(child);
            }

            if (!childNodes.Contains(child))
            {
                childNodes.Add(child);
                child.parent = this;
            }
		}

        /// <summary>
        /// Removes the specified child node from the tree.
        /// </summary>
        /// <param name="child">The child tree to remove.</param>
        /// <returns>A value indicating whether the child was found (and removed) from this tree.</returns>
		public bool Remove(GeneralTree<T> child)
		{
            if (childNodes.Remove(child))
            {
                child.parent = null;
                return true;
            }
            else
            {
                return false;
            }
		}

		/// <summary>
		/// Removes the child at the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		public void RemoveAt(int index)
		{
			if ((index > childNodes.Count - 1) || (index < 0))
			{
				throw new ArgumentOutOfRangeException();
			}

            childNodes[index].parent = null;
			this.childNodes.RemoveAt(index);
		}

        /// <summary>
        /// Sorts all descendants (All nodes lower in the tree) recursively using the specified sorter.
        /// </summary>
        /// <param name="sorter">The sorter to use in the sorting process.</param>
        public void SortAllDescendants(ISorter<GeneralTree<T>> sorter)
        {
            childNodes.Sort(sorter);

            for (int i = 0; i < childNodes.Count; i++)
            {
                childNodes[i].SortAllDescendants(sorter);
            }
        }

        /// <summary>
        /// Sorts all descendants using the specified sorter.
        /// </summary>
        /// <param name="sorter">The sorter to use in the sorting process.</param>
        /// <param name="comparison">The comparison.</param>
        public void SortAllDescendants(ISorter<GeneralTree<T>> sorter, Comparison<GeneralTree<T>> comparison)
        {
            childNodes.Sort(sorter, comparison);

            for (int i = 0; i < childNodes.Count; i++)
            {
                childNodes[i].SortAllDescendants(sorter, comparison);
            }
        }

        /// <summary>
        /// Sorts all descendants using the specified sorter.
        /// </summary>
        /// <param name="sorter">The sorter to use in the sorting process.</param>
        /// <param name="comparer">The comparer.</param>
        public void SortAllDescendants(ISorter<GeneralTree<T>> sorter, IComparer<GeneralTree<T>> comparer)
        {
            childNodes.Sort(sorter, comparer);

            for (int i = 0; i < childNodes.Count; i++)
            {
                childNodes[i].SortAllDescendants(sorter, comparer);
            }
        }

		#endregion

		#region Private Members

		/// <summary>
		/// Checks if the specified index is valid.
		/// </summary>
		/// <param name="index">The index to check.</param>
		private void CheckValidIndex(int index)
		{
			if ((index < 0) || (index >= childNodes.Count))
			{
				throw new ArgumentOutOfRangeException();
			}
		}

		/// <summary>
		/// Finds the maximum height between the childnodes.
		/// </summary>
		/// <returns>The maximum height of all paths between this node and all leaf nodes.</returns>
		private int FindMaximumChildHeight()
		{
			int max = 0;

			for (int i = 0; i < Degree; i++)
			{
				int childHeight = GetChild(i).Height;

				if (childHeight > max)
				{
					max = childHeight;
				}
			}

			return max;
		}

		#endregion
                
        #region Operator Overloads


        /// <summary>
        /// Gets the <see cref="NGenerics.DataStructures.GeneralTree&lt;T&gt;"/> with the specified i.
        /// </summary>
        /// <value></value>
		public GeneralTree<T> this[int i]
		{
			get
			{
				CheckValidIndex(i);
				return GetChild(i);
			}
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion

        #region ISortable<GeneralTree<T>> Members

        /// <summary>
        /// Sorts the direct children using the specified sorter.
        /// </summary>
        /// <param name="sorter">The sorter to use in the sorting process.</param>
        void ISortable<GeneralTree<T>>.Sort(ISorter<GeneralTree<T>> sorter)
        {
            childNodes.Sort(sorter);            
        }

        /// <summary>
        /// Sorts using the specified sorter.
        /// </summary>
        /// <param name="sorter">The sorter to use in the sorting process.</param>
        /// <param name="comparison">The comparison.</param>
        public void Sort(ISorter<GeneralTree<T>> sorter, Comparison<GeneralTree<T>> comparison)
        {
            childNodes.Sort(sorter, comparison);
        }

        /// <summary>
        /// Sorts using the specified sorter.
        /// </summary>
        /// <param name="sorter">The sorter to use in the sorting process.</param>
        /// <param name="comparer">The comparer.</param>
        public void Sort(ISorter<GeneralTree<T>> sorter, IComparer<GeneralTree<T>> comparer)
        {
            childNodes.Sort(sorter, comparer);
        }

        #endregion        
    }
}
