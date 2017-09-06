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
using System.Collections;
using NGenerics.Visitors;

namespace NGenericsTests
{
	[TestFixture]
	public class SetTest
	{
		[Test]
		public void TestUpperboundConstructor()
		{
			PascalSet s = new PascalSet(50);
			Assert.AreEqual(s.LowerBound, 0);
			Assert.AreEqual(s.UpperBound, 50);
			Assert.AreEqual(s.Count, 0);

			for (int i = 0; i <= 50; i++)
			{
				Assert.AreEqual(s[i], false);
			}
		}

		[Test]
		public void TestLowerBoundUpperBoundConstructor()
		{
			PascalSet s = new PascalSet(10, 50);

			Assert.AreEqual(s.LowerBound, 10);
			Assert.AreEqual(s.UpperBound, 50);
			Assert.AreEqual(s.Count, 0);

			for (int i = 10; i <= 50; i++)
			{
				Assert.AreEqual(s[i], false);
			}
		}

		[Test]
		public void TestIsEmpty()
		{
			PascalSet s = new PascalSet(0, 500);
			Assert.AreEqual(s.IsEmpty, true);

			s.Add(50);
			Assert.AreEqual(s.IsEmpty, false);

			s.Add(100);
			Assert.AreEqual(s.IsEmpty, false);

			s.Clear();
			Assert.AreEqual(s.IsEmpty, true);
		}

		[Test]
		public void TestIsFull()
		{
			PascalSet s = new PascalSet(0, 500);
			Assert.AreEqual(s.IsFull, false);

			s.Add(50);
			Assert.AreEqual(s.IsFull, false);

			s.Add(100);
			Assert.AreEqual(s.IsFull, false);
		}

		[Test]
		public void TestIsReadOnly()
		{
			PascalSet s = new PascalSet(0, 500);
			Assert.AreEqual(s.IsReadOnly, false);

			s.Add(50);
			Assert.AreEqual(s.IsReadOnly, false);

			s.Add(100);
			Assert.AreEqual(s.IsReadOnly, false);
		}
	

		[Test]
		public void TestUpperBoundInitialValuesConstructor()
		{
			int[] values = new int[] { 20, 30, 40 };

			PascalSet s = new PascalSet(50, values);

			Assert.AreEqual(s.LowerBound, 0);
			Assert.AreEqual(s.UpperBound, 50);
			Assert.AreEqual(s.Count, 3);

			for (int i = 0; i <= 50; i++)
			{
				if ((i == 20) || (i == 30) || (i == 40))
				{
					Assert.AreEqual(s[i], true);
				}
				else
				{
					Assert.AreEqual(s[i], false);
				}
			}
		}

		[Test]
		public void TestLowerBoundUpperBoundInitialValuesConstructor()
		{
			int[] values = new int[] { 20, 30, 40 };

			PascalSet s = new PascalSet(10, 50, values);

			Assert.AreEqual(s.LowerBound, 10);
			Assert.AreEqual(s.UpperBound, 50);
			Assert.AreEqual(s.Count, 3);

			for (int i = 10; i <= 50; i++)
			{
				if ((i == 20) || (i == 30) || (i == 40))
				{
					Assert.AreEqual(s[i], true);
				}
				else
				{
					Assert.AreEqual(s[i], false);
				}
			}
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestInvalidLowerBounds1()
		{
			PascalSet s = new PascalSet(-1, 10);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestInvalidLowerBounds2()
		{
			PascalSet s = new PascalSet(-1, 10, new int[] { 3, 4 });
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestUpperBoundSmallerThanLowerBound1()
		{
			PascalSet s = new PascalSet(12, 10);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestUpperBoundSmallerThanLowerBound2()
		{
			PascalSet s = new PascalSet(12, 10, new int[] { 3, 4 });
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestInitialValuesNull1()
		{
			PascalSet s = new PascalSet(5, 10, null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestInitialValuesNull2()
		{
			PascalSet s = new PascalSet(10, null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestInvalidInitialValues1()
		{
			PascalSet s = new PascalSet(10, 20, new int[] { 3, 4, 15, 16});
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestInvalidInitialValues2()
		{
			PascalSet s = new PascalSet(10, 20, new int[] { 22, 12, 15, 16 });
		}

		[Test]
		public void TestClear()
		{
			int[] values = new int[] { 20, 30, 40 };
			PascalSet s = new PascalSet(0, 50, values);

			Assert.AreEqual(s.Count, 3);

			s.Clear();

			for (int i = 0; i <= 50; i++)
			{
				Assert.AreEqual(s[i], false);
			}

			Assert.AreEqual(s.Count, 0);
		}

		[Test]
		public void TestCompareTo()
		{
			PascalSet s1 = new PascalSet(0, 50, new int[] { 20, 30, 40 });
			PascalSet s2 = new PascalSet(0, 50, new int[] { 20, 25, 30, 40 });

			Assert.AreEqual(s1.CompareTo(s2), -1);
			Assert.AreEqual(s2.CompareTo(s1), 1);
			Assert.AreEqual(s1.CompareTo(s1), 0);

			object obj = new object();

			Assert.AreEqual(s1.CompareTo(obj), -1);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestInvalidCompareTo()
		{
			PascalSet p = new PascalSet(0, 50);
			p.CompareTo(null);
		}

		[Test]
		public void TestContains()
		{
			PascalSet s1 = new PascalSet(0, 50, new int[] { 20, 30, 40 });

			Assert.AreEqual(s1.Contains(20), true);
			Assert.AreEqual(s1.Contains(30), true);
			Assert.AreEqual(s1.Contains(40), true);

			Assert.AreEqual(s1.Contains(25), false);
			Assert.AreEqual(s1.Contains(35), false);
			Assert.AreEqual(s1.Contains(45), false);
		}

		[Test]
		public void TestDifference()
		{
			PascalSet s1 = new PascalSet(0, 50, new int[] { 20, 30, 40 });
			PascalSet s2 = new PascalSet(0, 50, new int[] { 20, 25, 30, 35, 40 });

			PascalSet difference = s1.Difference(s2);

			for (int i = 0; i < 50; i++)
			{
				if ((i == 25) || (i == 35))
				{
					Assert.AreEqual(difference[i], true);
				}
				else
				{
					Assert.AreEqual(difference[i], false);
				}
			}

			PascalSet difference2 = s1 - s2;
			Assert.AreEqual(difference.IsEqual(difference2), true);
		}

		[Test]
		public void TestInterfaceDifference()
		{
			PascalSet s1 = new PascalSet(0, 50, new int[] { 20, 30, 40 });
			PascalSet s2 = new PascalSet(0, 50, new int[] { 20, 25, 30, 35, 40 });

			PascalSet difference = (PascalSet) ((ISet) s1).Difference(s2);

			for (int i = 0; i < 50; i++)
			{
				if ((i == 25) || (i == 35))
				{
					Assert.AreEqual(difference[i], true);
				}
				else
				{
					Assert.AreEqual(difference[i], false);
				}
			}
		}
				
		[Test]
		public void TestGetEnumerator()
		{
			PascalSet s = new PascalSet(0, 50, new int[] { 20, 25, 30, 35, 40 });

			IEnumerator<int> enumerator = s.GetEnumerator();

			Assert.AreEqual(enumerator.MoveNext(), true);
			Assert.AreEqual(enumerator.Current, 20);

			Assert.AreEqual(enumerator.MoveNext(), true);
			Assert.AreEqual(enumerator.Current, 25);

			Assert.AreEqual(enumerator.MoveNext(), true);
			Assert.AreEqual(enumerator.Current, 30);

			Assert.AreEqual(enumerator.MoveNext(), true);
			Assert.AreEqual(enumerator.Current, 35);

			Assert.AreEqual(enumerator.MoveNext(), true);
			Assert.AreEqual(enumerator.Current, 40);

			Assert.AreEqual(enumerator.MoveNext(), false);
		}

		[Test]
		public void TestInterfaceEnumerator()
		{
			PascalSet s = new PascalSet(0, 50, new int[] { 20, 25, 30, 35, 40 });

			IEnumerator enumerator = ((IEnumerable)s).GetEnumerator();

			Assert.AreEqual(enumerator.MoveNext(), true);
			Assert.AreEqual((int)enumerator.Current, 20);

			Assert.AreEqual(enumerator.MoveNext(), true);
			Assert.AreEqual((int)enumerator.Current, 25);

			Assert.AreEqual(enumerator.MoveNext(), true);
			Assert.AreEqual((int)enumerator.Current, 30);

			Assert.AreEqual(enumerator.MoveNext(), true);
			Assert.AreEqual((int)enumerator.Current, 35);

			Assert.AreEqual(enumerator.MoveNext(), true);
			Assert.AreEqual((int)enumerator.Current, 40);

			Assert.AreEqual(enumerator.MoveNext(), false);
		}

		[Test]
		public void TestAccept()
		{
			PascalSet s = new PascalSet(0, 50, new int[] { 15, 20, 30, 40, 34 });

			TrackingVisitor<int> v = new TrackingVisitor<int>();

			s.Accept(v);

			Assert.AreEqual(v.TrackingList.Count, 5);
			Assert.AreEqual(v.TrackingList.Contains(15), true);
			Assert.AreEqual(v.TrackingList.Contains(20), true);
			Assert.AreEqual(v.TrackingList.Contains(30), true);
			Assert.AreEqual(v.TrackingList.Contains(40), true);
			Assert.AreEqual(v.TrackingList.Contains(34), true);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestNullVisitor()
		{
			PascalSet s1 = new PascalSet(10);
			s1.Accept(null);
		}

		[Test]
		public void TestInterfaceIntersection()
		{
			PascalSet s1 = new PascalSet(0, 50, new int[] { 15, 20, 30, 40, 34 });
			PascalSet s2 = new PascalSet(0, 50, new int[] { 20, 25, 30, 35, 40 });

			PascalSet intersection = (PascalSet) ((ISet)s1).Intersection(s2);

			for (int i = 0; i <= 50; i++)
			{
				if ((i == 20) || (i == 30) || (i == 40))
				{
					Assert.AreEqual(intersection[i], true);
				}
				else
				{
					Assert.AreEqual(intersection[i], false);
				}
			}
		}

		[Test]
		public void TestIntersection()
		{
			PascalSet s1 = new PascalSet(0, 50, new int[] { 15, 20, 30, 40, 34 });
			PascalSet s2 = new PascalSet(0, 50, new int[] { 20, 25, 30, 35, 40 });

			PascalSet intersection = s1.Intersection(s2);

			for (int i = 0; i <= 50; i++)
			{
				if ((i == 20) || (i == 30) || (i == 40))
				{
					Assert.AreEqual(intersection[i], true);
				}
				else
				{
					Assert.AreEqual(intersection[i], false);
				}
			}

			PascalSet intersection2 = s1 * s2;
			Assert.AreEqual(intersection.IsEqual(intersection2), true);
		}		

		[Test]
		public void TestInterfaceInverse()
		{
			PascalSet s = new PascalSet(0, 50, new int[] { 15, 20, 30 });

			PascalSet inverse = (PascalSet) ((ISet)s).Inverse();

			for (int i = 0; i <= 50; i++)
			{
				if ((i == 15) || (i == 20) || (i == 30))
				{
					Assert.AreEqual(inverse[i], false);
				}
				else
				{
					Assert.AreEqual(inverse[i], true);
				}
			}
		}

		[Test]
		public void TestInverse()
		{
			PascalSet s = new PascalSet(0, 50, new int[] { 15, 20, 30 });

			PascalSet inverse = s.Inverse();

			for (int i = 0; i <= 50; i++)
			{
				if ((i == 15) || (i == 20) || (i == 30))
				{
					Assert.AreEqual(inverse[i], false);
				}
				else
				{
					Assert.AreEqual(inverse[i], true);
				}
			}

			PascalSet inverse2 = !s;
			Assert.AreEqual(inverse.IsEqual(inverse2), true);
		}		

		[Test]
		public void TestInterfaceIsEqual()
		{
			PascalSet s1 = new PascalSet(0, 50, new int[] { 15, 20, 30, 40, 34 });
			PascalSet s2 = new PascalSet(0, 50, new int[] { 20, 25, 30, 35, 40 });
			PascalSet s3 = new PascalSet(0, 50, new int[] { 15, 20, 30, 40, 34 });
			PascalSet s4 = new PascalSet(10, 50, new int[] { 15, 20, 30, 40, 34 });

			Assert.AreEqual(((ISet)s1).IsEqual(s3), true);
			Assert.AreEqual(((ISet)s1).IsEqual(s2), false);
			Assert.AreEqual(((ISet)s1).IsEqual(s4), false);
		}

		[Test]
		public void TestIsEqual()
		{
			PascalSet s1 = new PascalSet(0, 50, new int[] { 15, 20, 30, 40, 34 });
			PascalSet s2 = new PascalSet(0, 50, new int[] { 20, 25, 30, 35, 40 });
			PascalSet s3 = new PascalSet(0, 50, new int[] { 15, 20, 30, 40, 34 });
			PascalSet s4 = new PascalSet(10, 50, new int[] { 15, 20, 30, 40, 34 });

			Assert.AreEqual(s1.IsEqual(s3), true);
			Assert.AreEqual(s1.IsEqual(s2), false);
			Assert.AreEqual(s1.IsEqual(s4), false);
		}
		
		[Test]
		public void TestInterfaceIsSubsetOf()
		{
			PascalSet s1 = new PascalSet(0, 50, new int[] { 15, 20, 30, 40, 34 });
			PascalSet s2 = new PascalSet(0, 50, new int[] { 15, 20, 30 });
			PascalSet s3 = new PascalSet(0, 50, new int[] { 15, 20, 30, 40, 34 });

			Assert.AreEqual(((ISet)s1).IsSubsetOf(s2), false);
			Assert.AreEqual(((ISet)s2).IsSubsetOf(s1), true);
			Assert.AreEqual(((ISet)s3).IsSubsetOf(s1), true);
			Assert.AreEqual(((ISet)s1).IsSubsetOf(s3), true);
		}

		[Test]
		public void TestIsSubsetOf()
		{
			PascalSet s1 = new PascalSet(0, 50, new int[] { 15, 20, 30, 40, 34 });
			PascalSet s2 = new PascalSet(0, 50, new int[] { 15, 20, 30 });
			PascalSet s3 = new PascalSet(0, 50, new int[] { 15, 20, 30, 40, 34 });

			Assert.AreEqual(s1.IsSubsetOf(s2), false);
			Assert.AreEqual(s2.IsSubsetOf(s1), true);
			Assert.AreEqual(s3.IsSubsetOf(s1), true);
			Assert.AreEqual(s1.IsSubsetOf(s3), true);

			Assert.AreEqual(s1 <= s2, false);
			Assert.AreEqual(s2 <= s1, true);
			Assert.AreEqual(s3 <= s1, true);
			Assert.AreEqual(s1 <= s3, true);
		}

		[Test]
		public void TestIsProperSubsetOf()
		{
			PascalSet s1 = new PascalSet(0, 50, new int[] { 15, 20, 30, 40, 34 });
			PascalSet s2 = new PascalSet(0, 50, new int[] { 15, 20, 30 });
			PascalSet s3 = new PascalSet(0, 50, new int[] { 15, 20, 30, 40, 34 });

			Assert.AreEqual(s1.IsProperSubsetOf(s2), false);
			Assert.AreEqual(s2.IsProperSubsetOf(s1), true);
			Assert.AreEqual(s3.IsProperSubsetOf(s1), false);
			Assert.AreEqual(s1.IsProperSubsetOf(s3), false);

			Assert.AreEqual(s1 < s2, false);
			Assert.AreEqual(s2 < s1, true);
			Assert.AreEqual(s3 < s1, false);
			Assert.AreEqual(s1 < s3, false);
		}

		[Test]
		public void TestInterfaceIsProperSubsetOf()
		{
			PascalSet s1 = new PascalSet(0, 50, new int[] { 15, 20, 30, 40, 34 });
			PascalSet s2 = new PascalSet(0, 50, new int[] { 15, 20, 30 });
			PascalSet s3 = new PascalSet(0, 50, new int[] { 15, 20, 30, 40, 34 });

			Assert.AreEqual(((ISet)s1).IsProperSubsetOf(s2), false);
			Assert.AreEqual(((ISet)s2).IsProperSubsetOf(s1), true);
			Assert.AreEqual(((ISet)s3).IsProperSubsetOf(s1), false);
			Assert.AreEqual(((ISet)s1).IsProperSubsetOf(s3), false);
		}

		[Test]
		public void TestIsFixedSize()
		{
			PascalSet s = new PascalSet(100);
			Assert.AreEqual(s.IsFixedSize, true);

			s = new PascalSet(0, 50, new int [] {4, 5, 6});
			Assert.AreEqual(s.IsFixedSize, true);
		}

		[Test]
		public void TestInterfaceIsSupersetOf()
		{
			PascalSet s1 = new PascalSet(0, 50, new int[] { 15, 20, 30, 40, 34 });
			PascalSet s2 = new PascalSet(0, 50, new int[] { 15, 20, 30 });
			PascalSet s3 = new PascalSet(0, 50, new int[] { 15, 20, 30, 40, 34 });

			Assert.AreEqual(((ISet) s1).IsSupersetOf(s2), true);
			Assert.AreEqual(((ISet) s2).IsSupersetOf(s1), false);
			Assert.AreEqual(((ISet) s3).IsSupersetOf(s1), true);
			Assert.AreEqual(((ISet) s1).IsSupersetOf(s3), true);
		}

		[Test]
		public void TestIsSupersetOf()
		{
			PascalSet s1 = new PascalSet(0, 50, new int[] { 15, 20, 30, 40, 34 });
			PascalSet s2 = new PascalSet(0, 50, new int[] { 15, 20, 30 });
			PascalSet s3 = new PascalSet(0, 50, new int[] { 15, 20, 30, 40, 34 });

			Assert.AreEqual(s1.IsSupersetOf(s2), true);
			Assert.AreEqual(s2.IsSupersetOf(s1), false);
			Assert.AreEqual(s3.IsSupersetOf(s1), true);
			Assert.AreEqual(s1.IsSupersetOf(s3), true);

			Assert.AreEqual(s1 >= s2, true);
			Assert.AreEqual(s2 >= s1, false);
			Assert.AreEqual(s3 >= s1, true);
			Assert.AreEqual(s1 >= s3, true);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestInvalidProperSubsetOf()
		{
			PascalSet s1 = new PascalSet(0, 50, new int[] { 15, 20, 30 });
			PascalSet s2 = new PascalSet(10, 60, new int[] { 15, 20, 60 });

			s1.IsProperSubsetOf(s2);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestInvalidProperSupersetOf()
		{
			PascalSet s1 = new PascalSet(0, 50, new int[] { 15, 20, 30 });
			PascalSet s2 = new PascalSet(10, 60, new int[] { 15, 20, 60 });

			s1.IsProperSupersetOf(s2);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestInvalidSubsetOf()
		{
			PascalSet s1 = new PascalSet(0, 50, new int[] { 15, 20, 30 });
			PascalSet s2 = new PascalSet(10, 60, new int[] { 15, 20, 60 });

			s1.IsSubsetOf(s2);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestInvalidSupersetOf()
		{
			PascalSet s1 = new PascalSet(0, 50, new int[] { 15, 20, 30 });
			PascalSet s2 = new PascalSet(10, 60, new int[] { 15, 20, 60 });

			s1.IsSupersetOf(s2);
		}

		[Test]
		public void TestInterfaceIsProperSupersetOf()
		{
			PascalSet s1 = new PascalSet(0, 50, new int[] { 15, 20, 30, 40, 34 });
			PascalSet s2 = new PascalSet(0, 50, new int[] { 15, 20, 30 });
			PascalSet s3 = new PascalSet(0, 50, new int[] { 15, 20, 30, 40, 34 });

			Assert.AreEqual(((ISet)s1).IsProperSupersetOf(s2), true);
			Assert.AreEqual(((ISet)s2).IsProperSupersetOf(s1), false);
			Assert.AreEqual(((ISet)s3).IsProperSupersetOf(s1), false);
			Assert.AreEqual(((ISet)s1).IsProperSupersetOf(s3), false);
		}			

		[Test]
		public void TestIsProperSupersetOf()
		{
			PascalSet s1 = new PascalSet(0, 50, new int[] { 15, 20, 30, 40, 34 });
			PascalSet s2 = new PascalSet(0, 50, new int[] { 15, 20, 30 });
			PascalSet s3 = new PascalSet(0, 50, new int[] { 15, 20, 30, 40, 34 });

			Assert.AreEqual(s1.IsProperSupersetOf(s2), true);
			Assert.AreEqual(s2.IsProperSupersetOf(s1), false);
			Assert.AreEqual(s3.IsProperSupersetOf(s1), false);
			Assert.AreEqual(s1.IsProperSupersetOf(s3), false);

			Assert.AreEqual(s1 > s2, true);
			Assert.AreEqual(s2 > s1, false);
			Assert.AreEqual(s3 > s1, false);
			Assert.AreEqual(s1 > s3, false);
		}
		
		[Test]
		public void TestRemove()
		{
			PascalSet s1 = new PascalSet(0, 50, new int[] { 15, 20, 30, 40, 34 });

			Assert.AreEqual(s1.Count, 5);
			Assert.AreEqual(s1.Remove(20), true);
			Assert.AreEqual(s1.Count, 4);

			Assert.AreEqual(s1.Remove(20), false);
			Assert.AreEqual(s1.Count, 4);

			Assert.AreEqual(s1.Remove(40), true);
			Assert.AreEqual(s1.Count, 3);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestInvalidUnion()
		{
			PascalSet s1 = new PascalSet(0, 50, new int[] { 15, 20, 30 });
			PascalSet s2 = new PascalSet(10, 60, new int[] { 15, 20, 60 });

			s1.Union(s2);
		}


		[Test]
		public void TestUnion()
		{
			PascalSet s1 = new PascalSet(0, 50, new int[] { 15, 20, 30});
			PascalSet s2 = new PascalSet(0, 50, new int[] { 15, 25, 30 });
			PascalSet s3 = new PascalSet(0, 50, new int[] { 1, 2, 3, 4});

			PascalSet union = s1.Union(s2);

			Assert.AreEqual(union.Count, 4);

			for (int i = 0; i <= 50; i++)
			{
				if ((i == 15) || (i == 20) || (i == 25) || (i == 30))
				{
					Assert.AreEqual(union[i], true);
				}
				else
				{
					Assert.AreEqual(union[i], false);
				}
			}

			union = s2.Union(s3);

			Assert.AreEqual(union.Count, 7);

			for (int i = 0; i <= 50; i++)
			{
				if ((i == 15) || (i == 25) || (i == 30) || (i == 1) || (i == 2) || (i == 3) || (i == 4))
				{
					Assert.AreEqual(union[i], true);
				}
				else
				{
					Assert.AreEqual(union[i], false);
				}
			}

			PascalSet union2 = s2 + s3;
			Assert.AreEqual(union2.IsEqual(union), true);
		}

		[Test]
		public void TestInterfaceUnion()
		{
			PascalSet s1 = new PascalSet(0, 50, new int[] { 15, 20, 30 });
			PascalSet s2 = new PascalSet(0, 50, new int[] { 15, 25, 30 });
			PascalSet s3 = new PascalSet(0, 50, new int[] { 1, 2, 3, 4 });

			PascalSet union = (PascalSet) ((ISet) s1).Union(s2);

			Assert.AreEqual(union.Count, 4);

			for (int i = 0; i <= 50; i++)
			{
				if ((i == 15) || (i == 20) || (i == 25) || (i == 30))
				{
					Assert.AreEqual(union[i], true);
				}
				else
				{
					Assert.AreEqual(union[i], false);
				}
			}

			union  = (PascalSet) ((ISet) s2).Union(s3);

			Assert.AreEqual(union.Count, 7);

			for (int i = 0; i <= 50; i++)
			{
				if ((i == 15) || (i == 25) || (i == 30) || (i == 1) || (i == 2) || (i == 3) || (i == 4))
				{
					Assert.AreEqual(union[i], true);
				}
				else
				{
					Assert.AreEqual(union[i], false);
				}
			}
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestNullDifference()
		{
			PascalSet set = new PascalSet(20);
			set.Difference(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestNullIntersection()
		{
			PascalSet set = new PascalSet(20);
			set.Intersection(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestNullIsEqual()
		{
			PascalSet set = new PascalSet(20);
			set.IsEqual(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestNullIsProperSubsetOf()
		{
			PascalSet set = new PascalSet(20);
			set.IsProperSubsetOf(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestNullIsProperSupersetOf()
		{
			PascalSet set = new PascalSet(20);
			set.IsProperSupersetOf(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestNullIsSupersetOf()
		{
			PascalSet set = new PascalSet(20);
			set.IsSupersetOf(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestNullIsSubsetOf()
		{
			PascalSet set = new PascalSet(20);
			set.IsSubsetOf(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestNullUnion()
		{
			PascalSet set = new PascalSet(20);
			set.Union(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestNullCopyTo()
		{
			PascalSet set = new PascalSet(20);
			set.CopyTo(null, 0);
		}

		[Test]
		public void TestCopyTo()
		{
			PascalSet set = new PascalSet(10, new int[] {1, 2, 5, 6});

			int[] array = new int[4];
			set.CopyTo(array, 0);

			List<int> l = new List<int>(array);

			Assert.AreEqual(l.Contains(1), true);
			Assert.AreEqual(l.Contains(2), true);
			Assert.AreEqual(l.Contains(5), true);
			Assert.AreEqual(l.Contains(6), true);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestInvalidCopyTo1()
		{
			PascalSet set = new PascalSet(10, new int[] {1, 2, 5, 6});

			int[] array = new int[3];
			set.CopyTo(array, 0);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestInvalidCopyTo2()
		{
			PascalSet set = new PascalSet(10, new int[] {1, 2, 5, 6});

			int[] array = new int[4];
			set.CopyTo(array, 1);
		}
	}
}
