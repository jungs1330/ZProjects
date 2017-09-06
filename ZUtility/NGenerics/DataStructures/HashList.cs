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
using System.Runtime.Serialization;

namespace NGenerics.DataStructures
{
    /// <summary>
    /// A Dictionary that accepts multiple values for a unique key.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    [Serializable]
    public sealed class HashList<TKey, TValue> : VisitableHashtable<TKey, IList<TValue>>
    {
        #region Globals


        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="HashList&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        public HashList() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HashList&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        public HashList(IDictionary<TKey, IList<TValue>> dictionary) : base(dictionary) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HashList&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        public HashList(IEqualityComparer<TKey> comparer) : base(comparer) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HashList&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        public HashList(int capacity) : base(capacity) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HashList&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        /// <param name="comparer">The comparer.</param>
        public HashList(int capacity, IEqualityComparer<TKey> comparer) : base(capacity, comparer) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HashList&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        /// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> object containing the information required to serialize the <see cref="T:System.Collections.Generic.Dictionary`2"></see>.</param>
        /// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext"></see> structure containing the source and destination of the serialized stream associated with the <see cref="T:System.Collections.Generic.Dictionary`2"></see>.</param>
        private HashList(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #endregion

        #region Public Members

        /// <summary>
        /// Gets the count of values in this HashList.
        /// </summary>
        /// <value>The count of values.</value>
        public int ValueCount
        {
            get
            {
                int count = 0;

                using (Dictionary<TKey, IList<TValue>>.Enumerator enumerator = this.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        if (enumerator.Current.Value != null)
                        {
                            count += enumerator.Current.Value.Count;
                        }
                    }
                }

                return count;
            }
        }

        /// <summary>
        /// Gets the count of values in this HashList.
        /// </summary>
        /// <value>The count of values.</value>
        public int KeyCount
        {
            get
            {
                return this.Count;
            }
        }

        /// <summary>
        /// Gets an enumerator for enumerating though values.
        /// </summary>
        /// <returns>A enumerator for enumerating through values in the Hash IList.</returns>
        public IEnumerator<TValue> GetValueEnumerator()
        {
            // Note :
            // Can not use using {} for the enumerator here.
            // It appears that a reference is kept to the enumerator and the enumeration only happens
            // after the enumerator has been disposed - some interesting behaviour follows.  To do : Investigate IL.
            Dictionary<TKey, IList<TValue>>.Enumerator enumerator = base.GetEnumerator();
            
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Value != null)
                {
                    for (int i = 0; i < enumerator.Current.Value.Count; i++)
                    {
                        yield return enumerator.Current.Value[i];
                    }
                }
            }
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Add(TKey key, TValue value)
        {
            IList<TValue> list;

            if (this.ContainsKey(key))
            {
                list = this[key];

                if (list == null)
                {
                    list = new List<TValue>();
                    this[key] = list;
                }
            }
            else
            {
                list = new List<TValue>();
                this[key] = list;
            }

            list.Add(value);
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="values">The values.</param>
        public void Add(TKey key, ICollection<TValue> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            IList<TValue> list;

            if (this.ContainsKey(key))
            {
                list = this[key];

                if (list == null)
                {
                    list = new List<TValue>();
                    this[key] = list;
                }
            }
            else
            {
                list = new List<TValue>();
                this[key] = list;
            }

            ((List<TValue>)list).AddRange(values);
        }

        /// <summary>
        /// Removes the first occurrence of the value found.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>A indication of whether the item has been found (and removed) in the Hash IList.</returns>
        public bool Remove(TValue item)
        {
            Dictionary<TKey, IList<TValue>>.KeyCollection dictKeys = this.Keys;
            IList<TKey> keys = new List<TKey>(dictKeys);

            for (int i = 0; i < keys.Count; i++)
            {
                IList<TValue> values = this[keys[i]];

                if (values != null)
                {
                    if (values.Remove(item)) {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Removes all the ocurrences of the item in the HashList
        /// </summary>
        /// <param name="item">The item.</param>
        public void RemoveAll(TValue item)
        {
            Dictionary<TKey, IList<TValue>>.KeyCollection dictKeys = this.Keys;
            IList<TKey> keys = new List<TKey>(dictKeys);

            for (int i = 0; i < keys.Count; i++)
            {
                IList<TValue> values = this[keys[i]];

                if (values != null)
                {
                    for (int j = 0; j < values.Count; j++)
                    {
                        if (values[j].Equals(item))
                        {
                            values.RemoveAt(j);
                            j--;
                        }
                    }                    
                }
            }
        }


        /// <summary>
        /// Removes all the ocurrences of the item in the HashList
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        /// <returns>An indeication of whether the key and value pair has been found (and removed).</returns>
        public bool Remove(TKey key, TValue item)
        {
            if (!this.ContainsKey(key))
            {
                return false;
            }
            else
            {
                IList<TValue> values = this[key];

                if (values != null)
                {
                    return values.Remove(item);
                }
                else
                {
                    return false;
                }
            }
        }

        #endregion
    }
}
