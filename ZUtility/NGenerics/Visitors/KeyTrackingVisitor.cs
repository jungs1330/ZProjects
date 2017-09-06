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
using NGenerics.DataStructures;

namespace NGenerics.Visitors
{
    /// <summary>
    /// A visitor that tracks (stores) keys from KeyValuePAirs in the order they were visited.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public sealed class KeyTrackingVisitor<TKey, TValue> : IVisitor<KeyValuePair<TKey, TValue>>
    {
        #region Globals

        private VisitableList<TKey> tracks;

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackingVisitor&lt;T&gt;"/> class.
        /// </summary>
        public KeyTrackingVisitor()
        {
            tracks = new VisitableList<TKey>();
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Gets the tracking list.
        /// </summary>
        /// <value>The tracking list.</value>
        public VisitableList<TKey> TrackingList
        {
            get
            {
                return tracks;
            }
        }

        #endregion

        #region IVisitor<KeyValuePair<TKey,TValue>> Members


        /// <summary>
        /// Visits the specified object.
        /// </summary>
        /// <param name="obj">The object to visit.</param>
        public void Visit(KeyValuePair<TKey, TValue> obj)
        {
            tracks.Add(obj.Key);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is done performing it's work..
        /// </summary>
        /// <value><c>true</c> if this instance is done; otherwise, <c>false</c>.</value>
        /// <returns><c>true</c> if this instance is done; otherwise, <c>false</c>.</returns>
        public bool HasCompleted
        {
            get
            {
                return false;
            }
        }

        #endregion
    }
}
