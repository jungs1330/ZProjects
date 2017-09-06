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
using NUnit.Framework;
using NGenerics.DataStructures;
using NGenerics.Visitors;
using System.Collections;

namespace NGenericsTests.DataStructures
{
    [TestFixture]
    public class ObjectMatrixTest
    {
        public void TestSuccesfulInit()
        {
            ObjectMatrix<int> m = new ObjectMatrix<int>(10, 15);
            Assert.AreEqual(m.Rows, 10);
            Assert.AreEqual(m.Columns, 15);

            m = new ObjectMatrix<int>(5, 13);
            Assert.AreEqual(m.Rows, 5);
            Assert.AreEqual(m.Columns, 13);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidIndexInit1()
        {
            ObjectMatrix<int> m = new ObjectMatrix<int>(-1, 20);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidIndexInit2()
        {
            ObjectMatrix<int> m = new ObjectMatrix<int>(50, 0);
        }

        [Test]
        public void TestIndexing()
        {
            ObjectMatrix<int> m = new ObjectMatrix<int>(10, 15);

            m[0, 0] = 5;
            Assert.AreEqual(m[0, 0], 5);

            m[3, 2] = 99;
            Assert.AreEqual(m[3, 2], 99);
        }

        [Test]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void TestInvalidIndexing1()
        {
            ObjectMatrix<int> m = new ObjectMatrix<int>(10, 15);
            m[10, 0] = 5;
        }

        [Test]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void TestInvalidIndexing2()
        {
            ObjectMatrix<int> m = new ObjectMatrix<int>(10, 15);
            m[9, 15] = 5;
        }

        [Test]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void TestInvalidIndexing3()
        {
            ObjectMatrix<int> m = new ObjectMatrix<int>(10, 15);
            int i = m[10, 0];
        }

        [Test]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void TestInvalidIndexing4()
        {
            ObjectMatrix<int> m = new ObjectMatrix<int>(10, 15);
            int i = m[9, 15];
        }

        [Test]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void TestInvalidIndexing5()
        {
            ObjectMatrix<int> m = new ObjectMatrix<int>(10, 15);
            int i = m[-1, 0];
        }

        [Test]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void TestInvalidIndexing6()
        {
            ObjectMatrix<int> m = new ObjectMatrix<int>(10, 15);
            int i = m[9, -1];
        }

        [Test]
        public void TestClear()
        {
            ObjectMatrix<int> m = GetTestMatrix();
            m.Clear();

            for (int i = 0; i < m.Rows; i++)
            {
                for (int j = 0; j < m.Columns; j++)
                {
                    Assert.AreEqual(m[i, j], 0);
                }
            }
        }

        [Test]
        public void TestContains()
        {
            ObjectMatrix<int> m = new ObjectMatrix<int>(10, 15);

            m[5, 5] = 13;

            Assert.AreEqual(m.Contains(13), true);
            Assert.AreEqual(m.Contains(15), false);

            m[2, 3] = 15;

            Assert.AreEqual(m.Contains(13), true);
            Assert.AreEqual(m.Contains(15), true);
            Assert.AreEqual(m.Contains(17), false);
        }

        [Test]
        public void TestCount()
        {
            ObjectMatrix<int> m = new ObjectMatrix<int>(10, 15);
            Assert.AreEqual(m.Count, 150);

            m = new ObjectMatrix<int>(3, 3);
            Assert.AreEqual(m.Count, 9);
        }

        [Test]
        public void TestIsSquare()
        {
            ObjectMatrix<int> m = new ObjectMatrix<int>(10, 15);
            Assert.AreEqual(m.IsSquare, false);

            m = new ObjectMatrix<int>(3, 3);
            Assert.AreEqual(m.IsSquare, true);

            m = new ObjectMatrix<int>(9, 9);
            Assert.AreEqual(m.IsSquare, true);

            m = new ObjectMatrix<int>(2, 3);
            Assert.AreEqual(m.IsSquare, false);
        }

        [Test]
        public void TestIsFull()
        {
            ObjectMatrix<int> m = new ObjectMatrix<int>(5, 5);
            Assert.AreEqual(m.IsFull, true);

            m = GetTestMatrix();
            Assert.AreEqual(m.IsFull, true);
        }

        [Test]
        public void TestIsFixedSize()
        {
            ObjectMatrix<int> m = new ObjectMatrix<int>(5, 5);
            Assert.AreEqual(m.IsFixedSize, true);

            m = GetTestMatrix();
            Assert.AreEqual(m.IsFixedSize, true);
        }

        [Test]
        public void TestIsEmpty()
        {
            ObjectMatrix<int> m = new ObjectMatrix<int>(5, 5);
            Assert.AreEqual(m.IsEmpty, false);

            m = GetTestMatrix();
            Assert.AreEqual(m.IsEmpty, false);
        }

        [Test]
        public void TestIsReadOnly()
        {
            ObjectMatrix<int> m = new ObjectMatrix<int>(5, 5);
            Assert.AreEqual(m.IsReadOnly, false);

            m = GetTestMatrix();
            Assert.AreEqual(m.IsReadOnly, false);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullAccept()
        {
            ObjectMatrix<int> m = GetTestMatrix();
            m.Accept(null);
        }

        [Test]
        public void TestAccept()
        {
            TrackingVisitor<int> visitor = new TrackingVisitor<int>();

            ObjectMatrix<int> m = GetTestMatrix();

            m.Accept(visitor);

            Assert.AreEqual(visitor.TrackingList.Count, m.Columns * m.Rows);

            for (int i = 0; i < m.Rows; i++)
            {
                for (int j = 0; j < m.Columns; j++)
                {
                    Assert.AreEqual(visitor.TrackingList.Contains(i + j), true);
                }
            }
        }

        [Test]
        public void TestCompareTo()
        {
            ObjectMatrix<int> matrix1 = GetTestMatrix();
            ObjectMatrix<int> matrix2 = new ObjectMatrix<int>(4, 5);

            Assert.AreEqual(matrix1.CompareTo(matrix2), 1);
            Assert.AreEqual(matrix2.CompareTo(matrix1), -1);
            Assert.AreEqual(matrix1.CompareTo(matrix1), 0);
            Assert.AreEqual(matrix2.CompareTo(matrix2), 0);
            Assert.AreEqual(matrix2.CompareTo(new Object()), -1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullCompareTo()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();
            matrix.CompareTo(null);
        }

        [Test]
        public void TestInterfaceGetEnumerator()
        {
            ObjectMatrix<int> m = GetTestMatrix();

            List<int> l = new List<int>();

            IEnumerator enumerator = ((IEnumerable)m).GetEnumerator();
            {
                while (enumerator.MoveNext())
                {
                    l.Add((int) enumerator.Current);
                }
            }

            Assert.AreEqual(l.Count, m.Columns * m.Rows);

            for (int i = 0; i < m.Rows; i++)
            {
                for (int j = 0; j < m.Columns; j++)
                {
                    Assert.AreEqual(l.Contains(i + j), true);
                }
            }
        }

        [Test]
        public void TestGetEnumerator()
        {
            ObjectMatrix<int> m = GetTestMatrix();

            List<int> l = new List<int>();
            
            using (IEnumerator<int> enumerator = m.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    l.Add(enumerator.Current);
                }
            }

            Assert.AreEqual(l.Count, m.Columns * m.Rows);

            for (int i = 0; i < m.Rows; i++)
            {
                for (int j = 0; j < m.Columns; j++)
                {
                    Assert.AreEqual(l.Contains(i + j), true);
                }
            }
        }


        private ObjectMatrix<int> GetTestMatrix()
        {
            ObjectMatrix<int> m = new ObjectMatrix<int>(10, 15);

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    m[i, j] = i + j;
                }
            }

            return m;
        }

        [Test]
        public void TestCopyTo()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();

            int[] array = new int[matrix.Rows * matrix.Columns];

            matrix.CopyTo(array, 0);

            List<int> l = new List<int>(array);

            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Columns; j++)
                {
                    Assert.AreEqual(l.Contains(i + j), true);
                }
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullCopyTo()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();
            matrix.CopyTo(null, 0);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidCopyTo1()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();

            int[] array = new int[matrix.Rows * matrix.Columns];

            matrix.CopyTo(array, 1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidCopyTo2()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();

            int[] array = new int[matrix.Rows * matrix.Columns - 1];

            matrix.CopyTo(array, 0);
        }

        [Test]
        public void TestInterfaceGetSubMatrix()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();

            ObjectMatrix<int> result = (ObjectMatrix<int>) ((IMatrix<int>)matrix).GetSubMatrix(0, 0, 3, 3);

            Assert.AreEqual(result.Rows, 3);
            Assert.AreEqual(result.Columns, 3);

            for (int i = 0; i < result.Rows; i++)
            {
                for (int j = 0; j < result.Columns; j++)
                {
                    Assert.AreEqual(result[i, j], i + j);
                }
            }

            result = (ObjectMatrix<int>)((IMatrix<int>)matrix).GetSubMatrix(1, 2, 3, 3);

            for (int i = 0; i < result.Rows; i++)
            {
                for (int j = 0; j < result.Columns; j++)
                {
                    Assert.AreEqual(result[i, j], i + 1 + j + 2);
                }
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidGetSubMatrix1()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();
            matrix.GetSubMatrix(-1, 0, 1, 1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidGetSubMatrix2()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();
            matrix.GetSubMatrix(0, -1, 1, 1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidGetSubMatrix3()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();
            matrix.GetSubMatrix(0, 0, 0, 1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidGetSubMatrix4()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();
            matrix.GetSubMatrix(0, 0, 1, 0);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidGetSubMatrix5()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();
            matrix.GetSubMatrix(0, 0, 16, 6);
        }

        [Test]
        public void TestGetSubMatrix()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();

            ObjectMatrix<int> result = matrix.GetSubMatrix(0, 0, 3, 3);

            Assert.AreEqual(result.Rows, 3);
            Assert.AreEqual(result.Columns, 3);

            for (int i = 0; i < result.Rows; i++)
            {
                for (int j = 0; j < result.Columns; j++)
                {
                    Assert.AreEqual(result[i, j], i + j);
                }
            }

            result = matrix.GetSubMatrix(1, 2, 3, 3);

            for (int i = 0; i < result.Rows; i++)
            {
                for (int j = 0; j < result.Columns; j++)
                {
                    Assert.AreEqual(result[i, j], i + 1 + j + 2);
                }
            }
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestInterfaceRemove()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();
            ((ICollection<int>)matrix).Remove(5);
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestInterfaceAdd()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();
            ((ICollection<int>)matrix).Add(5);
        }

        [Test]
        public void TestInterchangeRows()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();

            int columnCount = matrix.Columns;
            int rowCount = matrix.Rows;

            matrix.InterchangeRows(0, 1);

            Assert.AreEqual(matrix.Columns, columnCount);
            Assert.AreEqual(matrix.Rows, rowCount);

            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Columns; j++)
                {
                    if (i == 0)
                    {
                        Assert.AreEqual(matrix[i, j], (i + 1) + (j));
                    }
                    else if (i == 1)
                    {
                        Assert.AreEqual(matrix[i, j], (i - 1) + (j));
                    }
                    else
                    {
                        Assert.AreEqual(matrix[i, j], i + j);
                    }
                }
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInterchangeRowsInvalidRow1()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();
            matrix.InterchangeRows(-1, 1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInterchangeRowsInvalidRow2()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();
            matrix.InterchangeRows(matrix.Rows, 1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInterchangeRowsInvalidRow3()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();
            matrix.InterchangeRows(0, -1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInterchangeRowsInvalidRow4()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();
            matrix.InterchangeRows(0, matrix.Rows);
        }

        [Test]
        public void TestInterchangeColumns()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();

            int columnCount = matrix.Columns;
            int rowCount = matrix.Rows;

            matrix.InterchangeColumns(0, 1);

            Assert.AreEqual(matrix.Columns, columnCount);
            Assert.AreEqual(matrix.Rows, rowCount);

            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Columns; j++)
                {
                    if (j == 0)
                    {
                        Assert.AreEqual(matrix[i, j], (i) + (j + 1));
                    }
                    else if (j == 1)
                    {
                        Assert.AreEqual(matrix[i, j], (i) + (j - 1));
                    }
                    else
                    {
                        Assert.AreEqual(matrix[i, j], i + j);
                    }
                }
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInterchangeColumnsInvalidColumn1()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();
            matrix.InterchangeColumns(-1, 1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInterchangeColumnsInvalidColumn2()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();
            matrix.InterchangeColumns(matrix.Columns, 1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInterchangeColumnsInvalidColumn3()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();
            matrix.InterchangeColumns(0, -1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInterchangeColumnsInvalidColumn4()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();
            matrix.InterchangeColumns(0, matrix.Columns);
        }

        [Test]
        public void TestGetRow()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();

            int columnCount = matrix.Columns;
            int rowCount = matrix.Rows;

            int[] row = matrix.GetRow(0);

            Assert.AreEqual(matrix.Columns, columnCount);
            Assert.AreEqual(matrix.Rows, rowCount);

            Assert.AreEqual(row.Length, matrix.Columns);

            for (int i = 0; i < row.Length; i++)
            {
                Assert.AreEqual(row[i], i);
            }

            row = matrix.GetRow(1);

            Assert.AreEqual(row.Length, matrix.Columns);

            for (int i = 0; i < row.Length; i++)
            {
                Assert.AreEqual(row[i], i + 1);
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidGetRow1()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();
            int[] row = matrix.GetRow(-1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidGetRow2()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();
            int[] row = matrix.GetRow(matrix.Rows);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidGetRow3()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();
            int[] row = matrix.GetRow(matrix.Rows + 1);
        }

        [Test]
        public void TestGetColumn()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();

            int columnCount = matrix.Columns;
            int rowCount = matrix.Rows;

            int[] column = matrix.GetColumn(0);

            Assert.AreEqual(matrix.Columns, columnCount);
            Assert.AreEqual(matrix.Rows, rowCount);

            Assert.AreEqual(column.Length, matrix.Rows);

            for (int i = 0; i < column.Length; i++)
            {
                Assert.AreEqual(column[i], i);
            }

            column = matrix.GetColumn(1);

            Assert.AreEqual(column.Length, matrix.Rows);

            for (int i = 0; i < column.Length; i++)
            {
                Assert.AreEqual(column[i], i + 1);
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidGetColumn1()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();
            int[] row = matrix.GetColumn(-1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidGetColumn2()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();
            int[] row = matrix.GetColumn(matrix.Columns);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidGetColumn3()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();
            int[] row = matrix.GetColumn(matrix.Columns + 1);
        }

        [Test]
        public void TestAddRow()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();

            int columnCount = matrix.Columns;
            int rowCount = matrix.Rows;

            matrix.AddRow();

            Assert.AreEqual(matrix.Columns, columnCount);
            Assert.AreEqual(matrix.Rows, rowCount + 1);

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    Assert.AreEqual(matrix[i, j], i + j);
                }
            }

            for (int i = 0; i < columnCount; i++)
            {
                Assert.AreEqual(matrix[rowCount, i], default(double));
            }
        }

        [Test]
        public void TestAddRowValues1()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();

            int columnCount = matrix.Columns;
            int rowCount = matrix.Rows;

            matrix.AddRow(0, -1, -2, -3, -4, -5, -6, -7, -8, -9, -10, -11, -12, -13, -14);

            Assert.AreEqual(matrix.Columns, columnCount);
            Assert.AreEqual(matrix.Rows, rowCount + 1);

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    Assert.AreEqual(matrix[i, j], i + j);
                }
            }

            for (int i = 0; i < columnCount; i++)
            {
                Assert.AreEqual(matrix[rowCount, i], -1 * i);
            }
        }

        [Test]
        public void TestAddRowValues2()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();

            int columnCount = matrix.Columns;
            int rowCount = matrix.Rows;

            matrix.AddRow(0, -1, -2);

            Assert.AreEqual(matrix.Columns, columnCount);
            Assert.AreEqual(matrix.Rows, rowCount + 1);

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    Assert.AreEqual(matrix[i, j], i + j);
                }
            }

            Assert.AreEqual(matrix[rowCount, 0], 0);
            Assert.AreEqual(matrix[rowCount, 1], -1);
            Assert.AreEqual(matrix[rowCount, 2], -2);
            Assert.AreEqual(matrix[rowCount, 3], 0);
            Assert.AreEqual(matrix[rowCount, 4], 0);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestInvalidAddRowValues1()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();
            matrix.AddRow((int[])null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidAddRowValues2()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();
            matrix.AddRow(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
        }

        [Test]
        public void TestAddMultipleRows()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();

            int columnCount = matrix.Columns;
            int rowCount = matrix.Rows;

            matrix.AddRows(3);

            Assert.AreEqual(matrix.Columns, columnCount);
            Assert.AreEqual(matrix.Rows, rowCount + 3);

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    Assert.AreEqual(matrix[i, j], i + j);
                }
            }

            for (int i = 0; i < columnCount; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Assert.AreEqual(matrix[rowCount + j, i], default(double));
                }
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidAddMultipleRows1()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();

            int columnCount = matrix.Columns;
            int rowCount = matrix.Rows;

            matrix.AddRows(-1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidAddMultipleRows2()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();

            int columnCount = matrix.Columns;
            int rowCount = matrix.Rows;

            matrix.AddRows(0);
        }

        [Test]
        public void TestAddColumn()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();

            int columnCount = matrix.Columns;
            int rowCount = matrix.Rows;

            matrix.AddColumn();

            Assert.AreEqual(matrix.Columns, columnCount + 1);
            Assert.AreEqual(matrix.Rows, rowCount);

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    Assert.AreEqual(matrix[i, j], i + j);
                }
            }

            for (int i = 0; i < rowCount; i++)
            {
                Assert.AreEqual(matrix[i, columnCount], default(double));
            }
        }

        [Test]
        public void TestAddColumnValues1()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();

            int columnCount = matrix.Columns;
            int rowCount = matrix.Rows;

            matrix.AddColumn(0, -1, -2, -3, -4, -5, -6, -7, -8, -9);

            Assert.AreEqual(matrix.Columns, columnCount + 1);
            Assert.AreEqual(matrix.Rows, rowCount);

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    Assert.AreEqual(matrix[i, j], i + j);
                }
            }

            for (int i = 0; i < rowCount; i++)
            {
                Assert.AreEqual(matrix[i, columnCount], -1 * i);
            }
        }

        [Test]
        public void TestAddColumnValues2()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();

            int columnCount = matrix.Columns;
            int rowCount = matrix.Rows;

            matrix.AddColumn(0, -1, -2);

            Assert.AreEqual(matrix.Columns, columnCount + 1);
            Assert.AreEqual(matrix.Rows, rowCount);

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    Assert.AreEqual(matrix[i, j], i + j);
                }
            }

            Assert.AreEqual(matrix[0, columnCount], 0);
            Assert.AreEqual(matrix[1, columnCount], -1);
            Assert.AreEqual(matrix[2, columnCount], -2);
            Assert.AreEqual(matrix[3, columnCount], 0);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestInvalidAddColumnValues1()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();
            matrix.AddColumn((int[])null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidAddColumnValues2()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();
            matrix.AddColumn(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
        }

        [Test]
        public void TestAddMultipleColumns()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();

            int columnCount = matrix.Columns;
            int rowCount = matrix.Rows;

            matrix.AddColumns(3);

            Assert.AreEqual(matrix.Columns, columnCount + 3);
            Assert.AreEqual(matrix.Rows, rowCount);

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    Assert.AreEqual(matrix[i, j], i + j);
                }
            }

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Assert.AreEqual(matrix[i, matrix.Columns - j - 1], default(double));
                }
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidAddMultipleColumns1()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();
            matrix.AddColumns(-1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidAddMultipleColumns2()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();
            matrix.AddColumns(0);
        }

        [Test]
        public void TestResizeSmaller()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();

            matrix.Resize(2, 2);

            Assert.AreEqual(matrix.Columns, 2);
            Assert.AreEqual(matrix.Rows, 2);

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Assert.AreEqual(matrix[i, j], i + j);
                }
            }
        }

        [Test]
        public void TestResizeLarger()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();

            int columnCount = matrix.Columns;
            int rowCount = matrix.Rows;

            matrix.Resize(20, 20);

            Assert.AreEqual(matrix.Columns, 20);
            Assert.AreEqual(matrix.Rows, 20);

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    Assert.AreEqual(matrix[i, j], i + j);
                }
            }

            for (int i = rowCount; i < 20; i++)
            {
                for (int j = columnCount; j < 20; j++)
                {
                    Assert.AreEqual(matrix[i, j], default(double));
                }
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidSize1()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();
            matrix.Resize(-1, 8);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidSize2()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();
            matrix.Resize(8, -1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidSize3()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();
            matrix.Resize(8, 0);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidSize4()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();
            matrix.Resize(0, 8);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidSize5()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();
            matrix.Resize(-1, -1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidSize6()
        {
            ObjectMatrix<int> matrix = GetTestMatrix();
            matrix.Resize(0, 0);
        }
    }
}
