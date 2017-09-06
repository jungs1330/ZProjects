using System;
using System.Collections.Generic;
using System.Text;
using NGenerics.DataStructures;

namespace NGenerics.Comparers
{
    /// <summary>
    /// A comparer for comparing weights on graph edges.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class EdgeWeightComparer<T> : IComparer<Edge<T>>
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="EdgeWeightComparer&lt;T&gt;"/> class.
        /// </summary>
        public EdgeWeightComparer() { }

        #endregion

        #region IComparer<Edge<T>> Members

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// Value Condition Less than zerox is less than y.Zerox equals y.Greater than zerox is greater than y.
        /// </returns>
        public int Compare(Edge<T> x, Edge<T> y)
        {
            return x.Weight.CompareTo(y.Weight);
        }

        #endregion
    }
}
