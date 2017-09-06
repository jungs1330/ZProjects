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

namespace NGenerics.DataStructures
{
    /// <summary>
    /// A data structure representing a matrix of objects.
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public class ObjectMatrix<T> : IMatrix<T>, IVisitableCollection<T>
    {
        #region Globals

        private int noOfColumns;
        private int noOfRows;
        private T[,] data;

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectMatrix&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="rows">The rows.</param>
        /// <param name="columns">The columns.</param>
        public ObjectMatrix(int rows, int columns)
        {
            if ((rows <= 0) || (columns <= 0))
            {
                throw new ArgumentException(Resources.RowsOrColumnsInvalid);
            }

            noOfColumns = columns;
            noOfRows = rows;

            data = new T[noOfRows, noOfColumns];
        }

        #endregion

        #region IMatrix<T> Members

        /// <summary>
        /// Gets the number of noOfColumns in this matrix.
        /// </summary>
        /// <value>The noOfColumns.</value>
        public int Columns
        {
            get {
                return noOfColumns;
            }
        }

        /// <summary>
        /// Gets the number of rows in this matrix.
        /// </summary>
        /// <value></value>
        public int Rows
        {
            get {
                return noOfRows;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this matrix is square.
        /// </summary>
        /// <value><c>true</c> if this matrix is square; otherwise, <c>false</c>.</value>
        public bool IsSquare
        {
            get {
                return noOfRows == noOfColumns;
            }
        }

        /// <summary>
        /// Gets or sets the value at the specified row and column.
        /// </summary>
        /// <value></value>
        public T this[int i, int j]
        {
            get
            {
                CheckIndexValid(i, j);

                return data[i, j];
            }
            set
            {
                CheckIndexValid(i, j);

                data[i, j] = value;
            }
        }
               

        /// <summary>
        /// Gets the sub matrix.
        /// </summary>
        /// <param name="rowStart">The row start.</param>
        /// <param name="noOfColumnstart">The no of columnstart.</param>
        /// <param name="rowCount">The row count.</param>
        /// <param name="columnCount">The column count.</param>
        /// <returns>The submatrix of the current matrix.</returns>
        IMatrix<T> IMatrix<T>.GetSubMatrix(int rowStart, int noOfColumnstart, int rowCount, int columnCount)
        {
            return this.GetSubMatrix(rowStart, noOfColumnstart, rowCount, columnCount);
        }

        #endregion

        #region IVisitableCollection<T> Members

        /// <summary>
        /// Gets a value indicating whether this instance is of a fixed size.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is fixed size; otherwise, <c>false</c>.
        /// </value>
        public bool IsFixedSize
        {
            get {
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
            get { 
                return false; 
            }
        }

        /// <summary>
        /// Gets a value indicating whether this collection is full.
        /// </summary>
        /// <value><c>true</c> if this collection is full; otherwise, <c>false</c>.</value>
        public bool IsFull
        {
            get {
                return true;  
            }
        }

        #endregion

        #region ICollection<T> Members

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
        void ICollection<T>.Add(T item)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only. </exception>
        public void Clear()
        {
            for (int i = 0; i < noOfRows; i++)
            {
                for (int j = 0; j < noOfColumns; j++)
                {
                    data[i, j] = default(T);
                }
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
            for (int i = 0; i < noOfRows; i++)
            {
                for (int j = 0; j < noOfColumns; j++)
                {
                    if (data[i, j].Equals(item))
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

            if ((array.Length - arrayIndex) < this.Count)
            {
                throw new ArgumentException(Resources.NotEnoughSpaceInTargetArray);
            }

            int counter = arrayIndex;

            for (int i = 0; i < noOfRows; i++)
            {
                for (int j = 0; j < noOfColumns; j++)
                {
                    array.SetValue(this[i, j], counter);
                    counter++;
                }
            }
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <value></value>
        /// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</returns>
        public int Count
        {
            get {
                return noOfRows * noOfColumns;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only; otherwise, false.</returns>
        public bool IsReadOnly
        {
            get {
                return false; 
            }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        /// <returns>
        /// true if item was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false. This method also returns false if item is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
        bool ICollection<T>.Remove(T item)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region IEnumerable<T> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < noOfRows; i++)
            {
                for (int j = 0; j < noOfColumns; j++)
                {
                    yield return data[i, j];
                }
            }
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IComparable Members

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
                ObjectMatrix<T> matrix = obj as ObjectMatrix<T>;
                return this.Count.CompareTo(matrix.Count);
            }
            else
            {
                return this.GetType().FullName.CompareTo(obj.GetType().FullName);
            }
        }

        #endregion

        #region IVisitable<T> Members

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public void Accept(NGenerics.Visitors.IVisitor<T> visitor)
        {
            if (visitor == null)
            {
                throw new ArgumentNullException("visitor");
            }

            for (int i = 0; i < noOfRows; i++)
            {
                for (int j = 0; j < noOfColumns; j++)
                {
                    visitor.Visit(data[i, j]);
                }
            }
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Gets the sub matrix.
        /// </summary>
        /// <param name="rowStart">The row start.</param>
        /// <param name="columnStart">The column start.</param>
        /// <param name="rowCount">The row count.</param>
        /// <param name="columnCount">The column count.</param>
        /// <returns>The sub matrix of the current matrix.</returns>
        public ObjectMatrix<T> GetSubMatrix(int rowStart, int columnStart, int rowCount, int columnCount)
        {
            if ((rowCount <= 0) || (columnCount <= 0))
            {
                throw new ArgumentException(Resources.ColumnAndRowCountBiggerThan0);
            }

            if ((rowStart < 0) || (columnStart < 0))
            {
                throw new ArgumentOutOfRangeException();
            }

            if (((rowStart + rowCount) > Rows) || ((columnStart + columnCount) > Columns))
            {
                throw new ArgumentOutOfRangeException();
            }

            ObjectMatrix<T> ret = new ObjectMatrix<T>(rowCount, columnCount);

            for (int i = rowStart; i < rowStart + rowCount; i++)
            {
                for (int j = columnStart; j < columnStart + columnCount; j++)
                {
                    ret[i - rowStart, j - columnStart] = this[i, j];
                }
            }

            return ret;

        }

        /// <summary>
        /// Interchanges (swap) one row with another.
        /// </summary>
        /// <param name="firstRow">The index of the first row.</param>
        /// <param name="secondRow">The index of the second row.</param>
        public void InterchangeRows(int firstRow, int secondRow)
        {
            if ((firstRow < 0) || (firstRow > noOfRows - 1))
            {
                throw new ArgumentOutOfRangeException("firstRow");
            }

            if ((secondRow < 0) || (secondRow > noOfRows - 1))
            {
                throw new ArgumentOutOfRangeException("secondRow");
            }

            /// Nothing to do
            if (firstRow == secondRow)
            {
                return;
            }

            T temp;

            for (int i = 0; i < noOfColumns; i++)
            {
                temp = data[firstRow, i];
                data[firstRow, i] = data[secondRow, i];
                data[secondRow, i] = temp;
            }
        }

        /// <summary>
        /// Interchanges (swap) one column with another.
        /// </summary>
        /// <param name="firstColumn">The index of the first column.</param>
        /// <param name="secondColumn">The index of the second column.</param>
        public void InterchangeColumns(int firstColumn, int secondColumn)
        {
            if ((firstColumn < 0) || (firstColumn > noOfColumns - 1))
            {
                throw new ArgumentOutOfRangeException("firstRow");
            }

            if ((secondColumn < 0) || (secondColumn > noOfColumns - 1))
            {
                throw new ArgumentOutOfRangeException("secondRow");
            }

            // Nothing to do
            if (firstColumn == secondColumn)
            {
                return;
            }

            T temp;

            for (int i = 0; i < noOfRows; i++)
            {
                temp = data[i, firstColumn];
                data[i, firstColumn] = data[i, secondColumn];
                data[i, secondColumn] = temp;
            }
        }

        /// <summary>
        /// Gets the row at the specified index.
        /// </summary>
        /// <param name="rowIndex">Index of the row.</param>
        /// <returns>
        /// An array containing the values of the requested row.
        /// </returns>
        public T[] GetRow(int rowIndex)
        {
            if ((rowIndex < 0) || (rowIndex > noOfRows - 1))
            {
                throw new ArgumentOutOfRangeException("rowIndex");
            }

            T[] ret = new T[noOfColumns];
            
            for (int i = 0; i < noOfColumns; i++)
            {
                ret[i] = data[rowIndex, i];
            }

            return ret;
        }

        /// <summary>
        /// Gets the column at the specified index.
        /// </summary>
        /// <param name="columnIndex">Index of the column.</param>
        /// <returns>
        /// An array containing the values of the requested column.
        /// </returns>
        public T[] GetColumn(int columnIndex)
        {
            if ((columnIndex < 0) || (columnIndex > noOfColumns - 1))
            {
                throw new ArgumentOutOfRangeException("columnIndex");
            }

            T[] ret = new T[noOfRows];

            for (int i = 0; i < noOfRows; i++)
            {
                ret[i] = data[columnIndex, i];
            }

            return ret;
        }

        /// <summary>
        /// Adds the specified number of rows to the matrix.
        /// </summary>
        /// <param name="rowCount">The number of rows to add.</param>        
        public void AddRows(int rowCount)
        {
            if (rowCount <= 0)
            {
                throw new ArgumentOutOfRangeException("columnCount");
            }

            int newRowCount = noOfRows + rowCount;

            // Create a new matrix of the specified size
            T[,] newData = new T[newRowCount, noOfColumns];

            CopyData(newData);

            noOfRows = newRowCount;
            data = newData;
        }

        /// <summary>
        /// Adds a single row to the matrix.
        /// </summary>
        public void AddRow()
        {
            AddRows(1);
        }

        /// <summary>
        /// Adds a single row to the matrix, and populates the values
        /// accordingly.
        /// </summary>
        /// <param name="values">The values to populate the row with.</param>
        public void AddColumn(params T[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            if (values.Length > noOfRows)
            {
                throw new ArgumentException(Resources.NumberOfValuesDoNotAgreeWithNumberOfRows);
            }

            AddColumn();

            for (int i = 0; i < values.Length; i++)
            {
                data[i, noOfColumns - 1] = values[i];
            }
        }


        /// <summary>
        /// Adds the specified number of rows to the matrix.
        /// </summary>
        /// <param name="columnCount">The number of rows to add.</param>        
        public void AddColumns(int columnCount)
        {
            if (columnCount <= 0)
            {
                throw new ArgumentOutOfRangeException("columnCount");
            }

            int newColumnCount = noOfColumns + columnCount;

            // Create a new matrix of the specified size
            T[,] newData = new T[noOfRows, newColumnCount];

            CopyData(newData);

            noOfColumns = newColumnCount;
            data = newData;
        }

        /// <summary>
        /// Adds a single row to the matrix.
        /// </summary>
        public void AddColumn()
        {
            AddColumns(1);
        }

        /// <summary>
        /// Adds a single row to the matrix, and populates the values
        /// accordingly.
        /// </summary>
        /// <param name="values">The values to populate the row with.</param>
        public void AddRow(params T[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            if (values.Length > noOfColumns)
            {
                throw new ArgumentException(Resources.NumberOfValuesDoNotAgreeWithNumberOfColumns);
            }

            AddRow();

            for (int i = 0; i < values.Length; i++)
            {
                data[noOfRows - 1, i] = values[i];
            }
        }

        /// <summary>
        /// Resizes the matrix to the specified size.
        /// </summary>
        /// <param name="newNumberOfRows">The new number of rows.</param>
        /// <param name="newNumberOfColumns">The new number of columns.</param>
        public void Resize(int newNumberOfRows, int newNumberOfColumns)
        {
            if ((newNumberOfRows <= 0) || (newNumberOfColumns <= 0))
            {
                throw new ArgumentException(Resources.RowsOrColumnsInvalid);
            }

            T[,] newData = new T[newNumberOfRows, newNumberOfColumns];

            // Find the minimum of the rows and the columns.
            // Case 1 : Target array is smaller than original - don't cross boundaries of target.
            // Case 2 : Original is smaller than target - don't cross boundaries of original.
            int minRows = System.Math.Min(noOfRows, newNumberOfRows);
            int minColumns = System.Math.Min(noOfColumns, newNumberOfColumns);

            for (int i = 0; i < minRows; i++)
            {
                for (int j = 0; j < minColumns; j++)
                {
                    newData[i, j] = data[i, j];
                }
            }

            data = newData;

            noOfRows = newNumberOfRows;
            noOfColumns = newNumberOfColumns;
        }

        #endregion

        #region Private Members

        /// <summary>
        /// Checks if the index is in range.
        /// </summary>
        /// <param name="i">The i.</param>
        /// <param name="j">The j.</param>
        private void CheckIndexValid(int i, int j)
        {
            if ((i > noOfRows) || (j > noOfColumns) || (i < 0) || (j < 0))
            {
                throw new IndexOutOfRangeException();
            }
        }
                
        private void CopyData(T[,] newData)
        {
            // Copy all the original data over the new matrix
            for (int i = 0; i < noOfRows; i++)
            {
                for (int j = 0; j < noOfColumns; j++)
                {
                    newData[i, j] = data[i, j];
                }
            }
        }
                
        #endregion
    }
}
