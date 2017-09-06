﻿/*  
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
    /// An interface for a general matrix-type data structure.
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public interface IMatrix<T>
	{
		/// <summary>
		/// Gets the number of columns in this matrix.
		/// </summary>
		/// <value>The columns.</value>
		int Columns { get; }

		/// <summary>
		/// Gets the number of rows in this matrix.
		/// </summary>
		int Rows { get; }

		/// <summary>
		/// Gets a value indicating whether this matrix is square.
		/// </summary>
		/// <value><c>true</c> if this matrix is square; otherwise, <c>false</c>.</value>
		bool IsSquare { get; }
		
		/// <summary>
		/// Gets or sets the value at the specified index.
		/// </summary>
		/// <value></value>
		T this[int i, int j] { get; set; }

		/// <summary>
		/// Gets the specified sub matrix of the current matrix
		/// </summary>
		/// <param name="rowStart">The start row.</param>
		/// <param name="columnStart">The start column.</param>
		/// <param name="rowCount">The number of rows.</param>
		/// <param name="columnCount">The number of columns.</param>
		/// <returns>A submatrix of the current matrix.</returns>
		IMatrix<T> GetSubMatrix(int rowStart, int columnStart, int rowCount, int columnCount);

        /// <summary>
        /// Interchanges (swap) one row with another.
        /// </summary>
        /// <param name="firstRow">The index of the first row.</param>
        /// <param name="secondRow">The index of the second row.</param>
        void InterchangeRows(int firstRow, int secondRow);

        /// <summary>
        /// Interchanges (swap) one column with another.
        /// </summary>
        /// <param name="firstColumn">The index of the first column.</param>
        /// <param name="secondColumn">The index of the second column.</param>
        void InterchangeColumns(int firstColumn, int secondColumn);

        /// <summary>
        /// Gets the row at the specified index.
        /// </summary>
        /// <param name="rowIndex">Index of the row.</param>
        /// <returns>An array containing the values of the requested row.</returns>
        T[] GetRow(int rowIndex);

        /// <summary>
        /// Gets the column at the specified index.
        /// </summary>
        /// <param name="columnIndex">Index of the column.</param>
        /// <returns>An array containing the values of the requested column.</returns>
        T[] GetColumn(int columnIndex);


        /// <summary>
        /// Adds the specified number of rows to the matrix.
        /// </summary>
        /// <param name="rowCount">The number of rows to add.</param>        
        void AddRows(int rowCount);
        
        /// <summary>
        /// Adds a single row to the matrix.
        /// </summary>
        void AddRow();
        
        /// <summary>
        /// Adds a single row to the matrix, and populates the values
        /// accordingly.
        /// </summary>
        /// <param name="values">The values to populate the row with.</param>
        void AddRow(params T[] values);


        /// <summary>
        /// Adds the specified number of columns to the matrix.
        /// </summary>
        /// <param name="columnCount">The number of columns to add.</param>        
        void AddColumns(int columnCount);

        /// <summary>
        /// Adds a single column to the matrix.
        /// </summary>
        void AddColumn();

        /// <summary>
        /// Adds a single column to the matrix, and populates the values
        /// accordingly.
        /// </summary>
        /// <param name="values">The values to populate the column with.</param>
        void AddColumn(params T[] values);

        /// <summary>
        /// Resizes the matrix to the specified size.
        /// </summary>
        /// <param name="newNumberOfRows">The new number of rows.</param>
        /// <param name="newNumberOfColumns">The new number of columns.</param>
        void Resize(int newNumberOfRows, int newNumberOfColumns);
	}
}
