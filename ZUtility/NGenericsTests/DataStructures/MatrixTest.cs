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

namespace NGenericsTests
{
	[TestFixture]
	public class MatrixTest
	{
		[Test]
		public void TestSuccesfulInit()
		{
			Matrix matrix = new Matrix(2, 3);
			Assert.AreEqual(matrix.Rows, 2);
			Assert.AreEqual(matrix.Columns, 3);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestUnsuccesfulInitRowNegative()
		{
			Matrix matrix = new Matrix(-1, 2);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestUnsuccesfulInitRowZero()
		{
			Matrix matrix = new Matrix(0, 2);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestUnsuccesfulInitColumnNegative()
		{
			Matrix matrix = new Matrix(2, -1);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestUnsuccesfulInitColumnZero()
		{
			Matrix matrix = new Matrix(2, 0);
		}

		[Test]
		[ExpectedException(typeof(NotSupportedException))]
		public void TestInterfaceAdd()
		{
			ICollection<double> matrix = GetTestMatrix();
			matrix.Add(5);
		}

		[Test]
		[ExpectedException(typeof(NotSupportedException))]
		public void TestInterfaceRemove()
		{
			ICollection<double> matrix = GetTestMatrix();
			matrix.Remove(5);
		}

		[Test]
		public void TestIsSquare()
		{
			Matrix matrix = new Matrix(10, 10);

			Assert.AreEqual(matrix.IsSquare, true);

			matrix = new Matrix(3, 4);
			Assert.AreEqual(matrix.IsSquare, false);

			matrix = new Matrix(35, 35);
			Assert.AreEqual(matrix.IsSquare, true);

			matrix = new Matrix(45, 44);
			Assert.AreEqual(matrix.IsSquare, false);
		}
		
		[Test]
		public void TestIndexes()
		{
			Matrix matrix = new Matrix(4, 5);

			for (int i = 0; i < matrix.Rows; i++)
			{
				for (int j = 0; j < matrix.Columns; j++)
				{
					matrix[i, j] = i + j;
				}
			}

			for (int i = 0; i < matrix.Rows; i++)
			{
				for (int j = 0; j < matrix.Columns; j++)
				{
					Assert.AreEqual(matrix[i, j], i + j);
				}
			}
		}

		[Test]
		public void TestClear()
		{
			Matrix matrix = GetTestMatrix();

			matrix.Clear();

			for (int i = 0; i < matrix.Rows; i++)
			{
				for (int j = 0; j < matrix.Columns; j++)
				{
					Assert.AreEqual(matrix[i, j], 0);
				}
			}
		}

		[Test]
		public void TestCompareTo()
		{
			Matrix matrix1 = new Matrix(2, 3);
			Matrix matrix2 = new Matrix(2, 3);
			Matrix matrix3 = new Matrix(3, 2);
			Matrix matrix4 = new Matrix(3, 4);
			Matrix matrix5 = new Matrix(2, 2);

			object obj = new object();

			Assert.AreEqual(matrix1.CompareTo(matrix2), 0);
			Assert.AreEqual(matrix1.CompareTo(matrix3), 0);
			Assert.AreEqual(matrix1.CompareTo(matrix4), -1);
			Assert.AreEqual(matrix1.CompareTo(matrix5), 1);

			Assert.AreEqual(matrix1.CompareTo(obj), -1);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestInvalidCompareTo()
		{
			Matrix m = new Matrix(10, 10);
			m.CompareTo(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestNullCompareTo()
		{
			Matrix matrix1 = new Matrix(2, 3);
			matrix1.CompareTo(null);
		}


		[Test]
		public void TestIsEmpty()
		{
			Matrix matrix = new Matrix(5, 5);
			Assert.AreEqual(matrix.IsEmpty, false);

			matrix = GetTestMatrix();
			Assert.AreEqual(matrix.IsEmpty, false);
		}

		[Test]
		public void TestIsFull()
		{
			Matrix matrix = new Matrix(5, 5);
			Assert.AreEqual(matrix.IsFull, false);

			matrix = GetTestMatrix();
			Assert.AreEqual(matrix.IsFull, false);
		}

		[Test]
		public void TestIsFixedSize()
		{
			Matrix matrix = new Matrix(5, 5);
			Assert.AreEqual(matrix.IsFixedSize, true);

			matrix = GetTestMatrix();
			Assert.AreEqual(matrix.IsFixedSize, true);
		}

		[Test]
		public void TestIsReadOnly()
		{
			Matrix matrix = new Matrix(5, 5);
			Assert.AreEqual(matrix.IsReadOnly, false);

			matrix = GetTestMatrix();
			Assert.AreEqual(matrix.IsReadOnly, false);
		}


		[Test]
		public void TestInterfaceEnumerator()
		{
			Matrix matrix = GetTestMatrix();

			IEnumerator enumerator = ((System.Collections.IEnumerable)matrix).GetEnumerator();

			bool moved = false;

			int i = 0;
			int j = 0;

			int totalCount = 0;

			while (enumerator.MoveNext())
			{
				moved = true;
				totalCount++;

				Assert.AreEqual((double)enumerator.Current, matrix[i, j]);

				j++;

				if (j > matrix.Columns - 1)
				{
					j = 0;
					i++;
				}
			}

			Assert.AreEqual(totalCount, matrix.Columns * matrix.Rows);
			Assert.AreEqual(i, matrix.Columns - 1);
			Assert.AreEqual(j, 0);

			// Test that we did indeed move through the enumerator
			Assert.AreEqual(moved, true);
		}

		[Test]
		public void TestEnumerator()
		{
			Matrix matrix = GetTestMatrix();

			IEnumerator<double> enumerator = matrix.GetEnumerator();

			bool moved = false;

			int i = 0;
			int j = 0;

			int totalCount = 0;
			
			while (enumerator.MoveNext())
			{
				moved = true;
				totalCount++;

				Assert.AreEqual(enumerator.Current, matrix[i, j]);

				j++;

				if (j > matrix.Columns - 1) {
					j = 0;
					i++;
				}
			}

			Assert.AreEqual(totalCount, matrix.Columns * matrix.Rows);
			Assert.AreEqual(i, matrix.Columns - 1);
			Assert.AreEqual(j, 0);

			// Test that we did indeed move through the enumerator
			Assert.AreEqual(moved, true);
		}

		[Test]
		public void TestPlus()
		{
			Matrix matrix1 = new Matrix(2, 3);
			matrix1[0, 0] = 1;
			matrix1[0, 1] = 2;
			matrix1[0, 2] = -4;
			matrix1[1, 0] = 0;
			matrix1[1, 1] = 3;
			matrix1[1, 2] = -1;

			Matrix matrix2 = new Matrix(2, 3);
			matrix2[0, 0] = -1;
			matrix2[0, 1] = 0;
			matrix2[0, 2] = 2;
			matrix2[1, 0] = 1;
			matrix2[1, 1] = -5;
			matrix2[1, 2] = 3;

			Matrix matrixShouldBe = new Matrix(2, 3);
			matrixShouldBe[0, 0] = 0;
			matrixShouldBe[0, 1] = 2;
			matrixShouldBe[0, 2] = -2;
			matrixShouldBe[1, 0] = 1;
			matrixShouldBe[1, 1] = -2;
			matrixShouldBe[1, 2] = 2;

			Matrix result = matrix1 + matrix2;

			Assert.AreEqual(result.IsEqual(matrixShouldBe), true);
		}

        [Test]
        public void TestInterfacePlus()
        {
            Matrix matrix1 = new Matrix(2, 3);
            matrix1[0, 0] = 1;
            matrix1[0, 1] = 2;
            matrix1[0, 2] = -4;
            matrix1[1, 0] = 0;
            matrix1[1, 1] = 3;
            matrix1[1, 2] = -1;

            Matrix matrix2 = new Matrix(2, 3);
            matrix2[0, 0] = -1;
            matrix2[0, 1] = 0;
            matrix2[0, 2] = 2;
            matrix2[1, 0] = 1;
            matrix2[1, 1] = -5;
            matrix2[1, 2] = 3;

            Matrix matrixShouldBe = new Matrix(2, 3);
            matrixShouldBe[0, 0] = 0;
            matrixShouldBe[0, 1] = 2;
            matrixShouldBe[0, 2] = -2;
            matrixShouldBe[1, 0] = 1;
            matrixShouldBe[1, 1] = -2;
            matrixShouldBe[1, 2] = 2;

            Matrix result = (Matrix) ((IMathematicalMatrix)matrix1).Plus(matrix2);

            Assert.AreEqual(result.IsEqual(matrixShouldBe), true);
        }

		[Test]
		public void TestMinus()
		{
			Matrix matrix1 = new Matrix(2, 3);
			matrix1[0, 0] = 1;
			matrix1[0, 1] = 2;
			matrix1[0, 2] = -4;
			matrix1[1, 0] = 0;
			matrix1[1, 1] = 3;
			matrix1[1, 2] = -1;

			Matrix matrix2 = new Matrix(2, 3);
			matrix2[0, 0] = -1;
			matrix2[0, 1] = 0;
			matrix2[0, 2] = 2;
			matrix2[1, 0] = 1;
			matrix2[1, 1] = -5;
			matrix2[1, 2] = 3;

			Matrix matrixShouldBe = new Matrix(2, 3);
			matrixShouldBe[0, 0] = 2;
			matrixShouldBe[0, 1] = 2;
			matrixShouldBe[0, 2] = -6;
			matrixShouldBe[1, 0] = -1;
			matrixShouldBe[1, 1] = 8;
			matrixShouldBe[1, 2] = -4;

			Matrix result = matrix1 - matrix2;

			Assert.AreEqual(result.IsEqual(matrixShouldBe), true);
		}

        [Test]
        public void TestInterfaceMinus()
        {
            Matrix matrix1 = new Matrix(2, 3);
            matrix1[0, 0] = 1;
            matrix1[0, 1] = 2;
            matrix1[0, 2] = -4;
            matrix1[1, 0] = 0;
            matrix1[1, 1] = 3;
            matrix1[1, 2] = -1;

            Matrix matrix2 = new Matrix(2, 3);
            matrix2[0, 0] = -1;
            matrix2[0, 1] = 0;
            matrix2[0, 2] = 2;
            matrix2[1, 0] = 1;
            matrix2[1, 1] = -5;
            matrix2[1, 2] = 3;

            Matrix matrixShouldBe = new Matrix(2, 3);
            matrixShouldBe[0, 0] = 2;
            matrixShouldBe[0, 1] = 2;
            matrixShouldBe[0, 2] = -6;
            matrixShouldBe[1, 0] = -1;
            matrixShouldBe[1, 1] = 8;
            matrixShouldBe[1, 2] = -4;

            Matrix result = (Matrix)((IMathematicalMatrix) matrix1).Minus(matrix2);

            Assert.AreEqual(result.IsEqual(matrixShouldBe), true);
        }

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestNullMinus()
		{
			Matrix matrix1 = new Matrix(2, 3);
			Matrix result = matrix1 - null;
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestPlusFailure()
		{
			Matrix matrix1 = new Matrix(2, 3);
			Matrix matrix2 = new Matrix(3, 2);

			// Should throw an ArgumentException since the matrices are of different sizes
			Matrix result = matrix1 + matrix2;
		}

		[Test]
		public void TestMultiplication()
		{
			Matrix matrix1 = new Matrix(2, 3);
			matrix1[0, 0] = -2;
			matrix1[0, 1] = 3;
			matrix1[0, 2] = 2;
			matrix1[1, 0] = 4;
			matrix1[1, 1] = 6;
			matrix1[1, 2] = -2;

			Matrix matrix2 = new Matrix(3, 4);
			matrix2[0, 0] = 4;
			matrix2[0, 1] = -1;
			matrix2[0, 2] = 2;
			matrix2[0, 3] = 5;
			matrix2[1, 0] = 3;
			matrix2[1, 1] = 0;
			matrix2[1, 2] = 1;
			matrix2[1, 3] = 1;
			matrix2[2, 0] = -2;
			matrix2[2, 1] = 3;
			matrix2[2, 2] = 5;
			matrix2[2, 3] = -3;

			Matrix matrixShouldBe = new Matrix(2, 4);
			matrixShouldBe[0, 0] = -3;
			matrixShouldBe[0, 1] = 8;
			matrixShouldBe[0, 2] = 9;
			matrixShouldBe[0, 3] = -13;
			matrixShouldBe[1, 0] = 38;
			matrixShouldBe[1, 1] = -10;
			matrixShouldBe[1, 2] = 4;
			matrixShouldBe[1, 3] = 32;

			Matrix result = matrix1 * matrix2;

			Assert.AreEqual(result.IsEqual(matrixShouldBe), true);
		}

        [Test]
        public void TestInterfaceMultiplication()
        {
            Matrix matrix1 = new Matrix(2, 3);
            matrix1[0, 0] = -2;
            matrix1[0, 1] = 3;
            matrix1[0, 2] = 2;
            matrix1[1, 0] = 4;
            matrix1[1, 1] = 6;
            matrix1[1, 2] = -2;

            Matrix matrix2 = new Matrix(3, 4);
            matrix2[0, 0] = 4;
            matrix2[0, 1] = -1;
            matrix2[0, 2] = 2;
            matrix2[0, 3] = 5;
            matrix2[1, 0] = 3;
            matrix2[1, 1] = 0;
            matrix2[1, 2] = 1;
            matrix2[1, 3] = 1;
            matrix2[2, 0] = -2;
            matrix2[2, 1] = 3;
            matrix2[2, 2] = 5;
            matrix2[2, 3] = -3;

            Matrix matrixShouldBe = new Matrix(2, 4);
            matrixShouldBe[0, 0] = -3;
            matrixShouldBe[0, 1] = 8;
            matrixShouldBe[0, 2] = 9;
            matrixShouldBe[0, 3] = -13;
            matrixShouldBe[1, 0] = 38;
            matrixShouldBe[1, 1] = -10;
            matrixShouldBe[1, 2] = 4;
            matrixShouldBe[1, 3] = 32;

            Matrix result = (Matrix) ((IMathematicalMatrix) matrix1).Times(matrix2);

            Assert.AreEqual(result.IsEqual(matrixShouldBe), true);
        }


		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestMultiplicationFailure()
		{
			Matrix matrix1 = new Matrix(2, 3);
			Matrix matrix2 = new Matrix(4, 2);
			Matrix result = matrix1 * matrix2;							// Should throw an ArgumentException since the matrices are of incompatible sizes
		}

		[Test]
		public void TestEquals()
		{
			Matrix matrix1 = new Matrix(5, 6);
			Matrix matrix2 = new Matrix(5, 6);

			Assert.AreEqual(matrix1.IsEqual(matrix2), true);

			for (int i = 0; i < matrix1.Rows; i++)
			{
				for (int j = 0; j < matrix1.Columns; j++)
				{
					matrix1[i, j] = i + j;
					matrix2[i, j] = i + j;
				}
			}

			Assert.AreEqual(matrix1.IsEqual(matrix2), true);
			Assert.AreEqual(matrix1.IsEqual(null), false);
		}

		[Test]
		public void TestNotEquals()
		{
			Matrix matrix1 = new Matrix(5, 6);
			Matrix matrix2 = new Matrix(5, 7);
			Matrix matrix3 = new Matrix(5, 6);
			Matrix matrix4 = new Matrix(6, 6);

			// Columns are not the same
			Assert.AreNotEqual(matrix1.IsEqual(matrix2), true);

			// Rows are not the same
			Assert.AreNotEqual(matrix1.IsEqual(matrix4), true);

			for (int i = 0; i < matrix1.Rows; i++)
			{
				for (int j = 0; j < matrix1.Columns; j++)
				{
					matrix1[i, j] = i + j;
					matrix3[i, j] = i + j;
				}
			}

			// One value is different
			matrix3[4, 4] = 0;

			Assert.AreNotEqual(matrix1.IsEqual(matrix3), true);

		}

		[Test]
		public void TestTranspose()
		{
			Matrix matrix1 = new Matrix(2, 3);
			matrix1[0, 0] = 1;
			matrix1[0, 1] = 4;
			matrix1[0, 2] = 5;
			matrix1[1, 0] = -3;
			matrix1[1, 1] = 2;
			matrix1[1, 2] = 7;

			//            T			[ 1, -3]
			// [ 1, 4,  5]    =		[ 4,  2]
			// [-3, 2,  7]			[ 5,  7]

			Matrix matrixShouldBe = new Matrix(3, 2);
			matrixShouldBe[0, 0] = 1;
			matrixShouldBe[0, 1] = -3;
			matrixShouldBe[1, 0] = 4;
			matrixShouldBe[1, 1] = 2;
			matrixShouldBe[2, 0] = 5;
			matrixShouldBe[2, 1] = 7;

			Matrix result = matrix1.Transpose();

			Assert.AreEqual(result.IsEqual(matrixShouldBe), true);
		}
        [Test]
        public void TestInterfaceTranspose()
        {
            Matrix matrix1 = new Matrix(2, 3);
            matrix1[0, 0] = 1;
            matrix1[0, 1] = 4;
            matrix1[0, 2] = 5;
            matrix1[1, 0] = -3;
            matrix1[1, 1] = 2;
            matrix1[1, 2] = 7;

            //            T			[ 1, -3]
            // [ 1, 4,  5]    =		[ 4,  2]
            // [-3, 2,  7]			[ 5,  7]

            Matrix matrixShouldBe = new Matrix(3, 2);
            matrixShouldBe[0, 0] = 1;
            matrixShouldBe[0, 1] = -3;
            matrixShouldBe[1, 0] = 4;
            matrixShouldBe[1, 1] = 2;
            matrixShouldBe[2, 0] = 5;
            matrixShouldBe[2, 1] = 7;

            Matrix result = (Matrix) ((IMathematicalMatrix) matrix1).Transpose();

            Assert.AreEqual(result.IsEqual(matrixShouldBe), true);
        }


		[Test]
		public void TestTimesDouble()
		{
			Matrix matrix1 = new Matrix(2, 3);
			matrix1[0, 0] = 1;
			matrix1[0, 1] = 4;
			matrix1[0, 2] = 5;
			matrix1[1, 0] = -3;
			matrix1[1, 1] = 2;
			matrix1[1, 2] = 7;

			Matrix matrixShouldBe = new Matrix(2, 3);
			matrixShouldBe[0, 0] = 2;
			matrixShouldBe[0, 1] = 8;
			matrixShouldBe[0, 2] = 10;
			matrixShouldBe[1, 0] = -6;
			matrixShouldBe[1, 1] = 4;
			matrixShouldBe[1, 2] = 14;

			Matrix result = matrix1 * 2;
			Assert.AreEqual(result.IsEqual(matrixShouldBe), true);

			result = 2 * matrix1;
			Assert.AreEqual(result.IsEqual(matrixShouldBe), true);
		}

        [Test]
        public void TestInterfaceTimesDouble()
        {
            Matrix matrix1 = new Matrix(2, 3);
            matrix1[0, 0] = 1;
            matrix1[0, 1] = 4;
            matrix1[0, 2] = 5;
            matrix1[1, 0] = -3;
            matrix1[1, 1] = 2;
            matrix1[1, 2] = 7;

            Matrix matrixShouldBe = new Matrix(2, 3);
            matrixShouldBe[0, 0] = 2;
            matrixShouldBe[0, 1] = 8;
            matrixShouldBe[0, 2] = 10;
            matrixShouldBe[1, 0] = -6;
            matrixShouldBe[1, 1] = 4;
            matrixShouldBe[1, 2] = 14;

            Matrix result = (Matrix) ((IMathematicalMatrix) matrix1).Times(2);
            Assert.AreEqual(result.IsEqual(matrixShouldBe), true);
        }

		[Test]
		public void TestIsSymmetric()
		{
			// Columns != Rows
			Matrix matrix1 = new Matrix(2, 3);
			matrix1[0, 0] = -2;
			matrix1[0, 1] = 3;
			matrix1[0, 2] = 2;
			matrix1[1, 0] = 4;
			matrix1[1, 1] = 6;
			matrix1[1, 2] = -2;

			Assert.AreEqual(matrix1.IsSymmetric, false);

			// Not symmetric because of values
			Matrix matrix2 = new Matrix(2, 2);
			matrix2[0, 0] = -2;
			matrix2[0, 1] = 3;
			matrix2[1, 0] = 4;
			matrix2[1, 1] = 6;

			Assert.AreEqual(matrix2.IsSymmetric, false);

			// Symmetric
			Matrix matrix3 = new Matrix(2, 2);
			matrix3[0, 0] = 1;
			matrix3[0, 1] = 1;
			matrix3[1, 0] = 1;
			matrix3[1, 1] = 1;

			Assert.AreEqual(matrix3.IsSymmetric, true);

			// Symmetric
			Matrix matrix4 = new Matrix(3, 3);
			matrix4[0, 0] = 1;
			matrix4[0, 1] = 2;
			matrix4[0, 2] = 3;
			matrix4[1, 0] = 2;
			matrix4[1, 1] = -4;
			matrix4[1, 2] = 5;
			matrix4[2, 0] = 3;
			matrix4[2, 1] = 5;
			matrix4[2, 2] = 6;

			Assert.AreEqual(matrix4.IsSymmetric, true);
		}

		[Test]
		public void TestInvert()
		{
			Matrix matrix1 = new Matrix(2, 3);
			matrix1[0, 0] = 1;
			matrix1[0, 1] = 4;
			matrix1[0, 2] = 5;
			matrix1[1, 0] = -3;
			matrix1[1, 1] = 2;
			matrix1[1, 2] = 7;

			Matrix matrixShouldBe = new Matrix(2, 3);
			matrixShouldBe[0, 0] = -1;
			matrixShouldBe[0, 1] = -4;
			matrixShouldBe[0, 2] = -5;
			matrixShouldBe[1, 0] = 3;
			matrixShouldBe[1, 1] = -2;
			matrixShouldBe[1, 2] = -7;

			Matrix result = matrix1.Invert();

			Assert.AreEqual(result.IsEqual(matrixShouldBe), true);
		}

        [Test]
        public void TestInterfaceInvert()
        {
            Matrix matrix1 = new Matrix(2, 3);
            matrix1[0, 0] = 1;
            matrix1[0, 1] = 4;
            matrix1[0, 2] = 5;
            matrix1[1, 0] = -3;
            matrix1[1, 1] = 2;
            matrix1[1, 2] = 7;

            Matrix matrixShouldBe = new Matrix(2, 3);
            matrixShouldBe[0, 0] = -1;
            matrixShouldBe[0, 1] = -4;
            matrixShouldBe[0, 2] = -5;
            matrixShouldBe[1, 0] = 3;
            matrixShouldBe[1, 1] = -2;
            matrixShouldBe[1, 2] = -7;

            Matrix result = (Matrix) ((IMathematicalMatrix)matrix1).Invert();

            Assert.AreEqual(result.IsEqual(matrixShouldBe), true);
        }

		[Test]
		public void TestAccept()
		{
			TrackingVisitor<double> visitor = new TrackingVisitor<double>();
			Matrix matrix = GetTestMatrix();

			matrix.Accept(visitor);

			Assert.AreEqual(visitor.TrackingList.Count, matrix.Rows * matrix.Columns);

			for (int i = 0; i < matrix.Rows; i++)
			{
				for (int j = 0; j < matrix.Columns; j++)
				{
					Assert.AreEqual(visitor.TrackingList.Contains(i + j), true);
				}
			}
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestNullVisitor()
		{
			Matrix matrix = GetTestMatrix();
			matrix.Accept(null);
		}

		[Test]
		public void TestContains()
		{
			Matrix matrix = GetTestMatrix();

			for (int i = 0; i < matrix.Rows; i++)
			{
				for (int j = 0; j < matrix.Columns; j++)
				{
					Assert.AreEqual(matrix.Contains(i + j), true);
				}
			}

			Assert.AreEqual(matrix.Contains(-5), false);
		}

		[Test]
		public void TestCopyTo()
		{
			Matrix matrix = GetTestMatrix();

			double[] array = new double[matrix.Rows * matrix.Columns];

			matrix.CopyTo(array, 0);

			List<double> l = new List<double>(array);

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
			Matrix matrix = GetTestMatrix();
			matrix.CopyTo(null, 0);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestInvalidCopyTo1()
		{
			Matrix matrix = GetTestMatrix();

			double[] array = new double[matrix.Rows * matrix.Columns];

			matrix.CopyTo(array, 1);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestInvalidCopyTo2()
		{
			Matrix matrix = GetTestMatrix();

			double[] array = new double[matrix.Rows * matrix.Columns - 1];

			matrix.CopyTo(array, 0);
		}

		[Test]
		public void TestGetSubMatrix()
		{
			Matrix matrix = GetTestMatrix();

			Matrix result = matrix.GetSubMatrix(0, 0, 3, 3);

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
        public void TestInterfaceGetSubMatrix()
        {
            Matrix matrix = GetTestMatrix();

            Matrix result = matrix.GetSubMatrix(0, 0, 3, 3);

            Assert.AreEqual(result.Rows, 3);
            Assert.AreEqual(result.Columns, 3);

            for (int i = 0; i < result.Rows; i++)
            {
                for (int j = 0; j < result.Columns; j++)
                {
                    Assert.AreEqual(result[i, j], i + j);
                }
            }

            result = (Matrix) ((IMatrix<double>)matrix).GetSubMatrix(1, 2, 3, 3);

            for (int i = 0; i < result.Rows; i++)
            {
                for (int j = 0; j < result.Columns; j++)
                {
                    Assert.AreEqual(result[i, j], i + 1 + j + 2);
                }
            }
        }

        [Test]
        public void TestClone()
        {
            Matrix matrix = GetTestMatrix();
            Matrix clone = matrix.Clone();


            Assert.AreEqual(matrix.Rows, clone.Rows);
            Assert.AreEqual(matrix.Columns, clone.Columns);

            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Columns; j++)
                {
                    Assert.AreEqual(matrix[i, j], clone[i, j]);
                }
            }
        }

        [Test]
        public void TestInterfaceClone()
        {
            Matrix matrix = GetTestMatrix();
            Matrix clone = (Matrix)((ICloneable)matrix).Clone();


            Assert.AreEqual(matrix.Rows, clone.Rows);
            Assert.AreEqual(matrix.Columns, clone.Columns);

            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Columns; j++)
                {
                    Assert.AreEqual(matrix[i, j], clone[i, j]);
                }
            }
        }

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestInvalidGetSubMatrix1()
		{
			Matrix matrix = GetTestMatrix();
			matrix.GetSubMatrix(-1, 0, 1, 1);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestInvalidGetSubMatrix2()
		{
			Matrix matrix = GetTestMatrix();
			matrix.GetSubMatrix(0, -1, 1, 1);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestInvalidGetSubMatrix3()
		{
			Matrix matrix = GetTestMatrix();
			matrix.GetSubMatrix(0, 0, 0, 1);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestInvalidGetSubMatrix4()
		{
			Matrix matrix = GetTestMatrix();
			matrix.GetSubMatrix(0, 0, 1, 0);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestInvalidGetSubMatrix5()
		{
			Matrix matrix = GetTestMatrix();
			matrix.GetSubMatrix(0, 0, 6, 6);
		}

        [Test]        
        public void TestInterchangeRows()
        {
            Matrix matrix = GetTestMatrix();

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
            Matrix matrix = GetTestMatrix();
            matrix.InterchangeRows(-1, 1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInterchangeRowsInvalidRow2()
        {
            Matrix matrix = GetTestMatrix();
            matrix.InterchangeRows(matrix.Rows, 1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInterchangeRowsInvalidRow3()
        {
            Matrix matrix = GetTestMatrix();
            matrix.InterchangeRows(0, -1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInterchangeRowsInvalidRow4()
        {
            Matrix matrix = GetTestMatrix();
            matrix.InterchangeRows(0, matrix.Rows);
        }

        [Test]
        public void TestInterchangeColumns()
        {
            Matrix matrix = GetTestMatrix();

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
            Matrix matrix = GetTestMatrix();
            matrix.InterchangeColumns(-1, 1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInterchangeColumnsInvalidColumn2()
        {
            Matrix matrix = GetTestMatrix();
            matrix.InterchangeColumns(matrix.Columns, 1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInterchangeColumnsInvalidColumn3()
        {
            Matrix matrix = GetTestMatrix();
            matrix.InterchangeColumns(0, -1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInterchangeColumnsInvalidColumn4()
        {
            Matrix matrix = GetTestMatrix();
            matrix.InterchangeColumns(0, matrix.Columns);
        }

        [Test]
        public void TestGetRow()
        {
            Matrix matrix = GetTestMatrix();

            int columnCount = matrix.Columns;
            int rowCount = matrix.Rows;

            double[] row = matrix.GetRow(0);

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
            Matrix matrix = GetTestMatrix();
            double[] row = matrix.GetRow(-1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidGetRow2()
        {
            Matrix matrix = GetTestMatrix();
            double[] row = matrix.GetRow(matrix.Rows);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidGetRow3()
        {
            Matrix matrix = GetTestMatrix();
            double[] row = matrix.GetRow(matrix.Rows + 1);
        }

        [Test]
        public void TestGetColumn()
        {
            Matrix matrix = GetTestMatrix();

            int columnCount = matrix.Columns;
            int rowCount = matrix.Rows;

            double[] column = matrix.GetColumn(0);

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
            Matrix matrix = GetTestMatrix();
            double[] row = matrix.GetColumn(-1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidGetColumn2()
        {
            Matrix matrix = GetTestMatrix();
            double[] row = matrix.GetColumn(matrix.Columns);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidGetColumn3()
        {
            Matrix matrix = GetTestMatrix();
            double[] row = matrix.GetColumn(matrix.Columns + 1);
        }

        [Test]
        public void TestAddRow()
        {
            Matrix matrix = GetTestMatrix();
            
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
            Matrix matrix = GetTestMatrix();

            int columnCount = matrix.Columns;
            int rowCount = matrix.Rows;

            matrix.AddRow(0, -1, -2, -3, -4);

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
            Matrix matrix = GetTestMatrix();

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
            Matrix matrix = GetTestMatrix();
            matrix.AddRow((double[]) null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidAddRowValues2()
        {
            Matrix matrix = GetTestMatrix();
            matrix.AddRow(1, 2, 3, 4, 5, 6);
        }

        [Test]
        public void TestAddMultipleRows()
        {
            Matrix matrix = GetTestMatrix();

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
            Matrix matrix = GetTestMatrix();

            int columnCount = matrix.Columns;
            int rowCount = matrix.Rows;

            matrix.AddRows(-1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidAddMultipleRows2()
        {
            Matrix matrix = GetTestMatrix();

            int columnCount = matrix.Columns;
            int rowCount = matrix.Rows;

            matrix.AddRows(0);
        }

        [Test]
        public void TestAddColumn()
        {
            Matrix matrix = GetTestMatrix();

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
            Matrix matrix = GetTestMatrix();

            int columnCount = matrix.Columns;
            int rowCount = matrix.Rows;

            matrix.AddColumn(0, -1, -2, -3);

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
            Matrix matrix = GetTestMatrix();

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
            Matrix matrix = GetTestMatrix();
            matrix.AddColumn((double[])null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidAddColumnValues2()
        {
            Matrix matrix = GetTestMatrix();
            matrix.AddColumn(1, 2, 3, 4, 5);
        }

        [Test]
        public void TestAddMultipleColumns()
        {
            Matrix matrix = GetTestMatrix();

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
            Matrix matrix = GetTestMatrix();
            matrix.AddColumns(-1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestInvalidAddMultipleColumns2()
        {
            Matrix matrix = GetTestMatrix();
            matrix.AddColumns(0);
        }

        [Test]
        public void TestResizeSmaller()
        {
            Matrix matrix = GetTestMatrix();

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
            Matrix matrix = GetTestMatrix();

            int columnCount = matrix.Columns;
            int rowCount = matrix.Rows;

            matrix.Resize(8, 8);

            Assert.AreEqual(matrix.Columns, 8);
            Assert.AreEqual(matrix.Rows, 8);

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    Assert.AreEqual(matrix[i, j], i + j);
                }
            }

            for (int i = rowCount; i < 8; i++)
            {
                for (int j = columnCount; j < 8; j++)
                {
                    Assert.AreEqual(matrix[i, j], default(double));
                }
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidSize1()
        {
            Matrix matrix = GetTestMatrix();
            matrix.Resize(-1, 8);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidSize2()
        {
            Matrix matrix = GetTestMatrix();
            matrix.Resize(8, -1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidSize3()
        {
            Matrix matrix = GetTestMatrix();
            matrix.Resize(8, 0);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidSize4()
        {
            Matrix matrix = GetTestMatrix();
            matrix.Resize(0, 8);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidSize5()
        {
            Matrix matrix = GetTestMatrix();
            matrix.Resize(-1, -1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidSize6()
        {
            Matrix matrix = GetTestMatrix();
            matrix.Resize(0, 0);
        }

        private Matrix GetTestMatrix()
		{
			Matrix matrix = new Matrix(4, 5);

			for (int i = 0; i < matrix.Rows; i++)
			{
				for (int j = 0; j < matrix.Columns; j++)
				{
					matrix[i, j] = i + j;
				}
			}

			return matrix;
		}
	}
}
