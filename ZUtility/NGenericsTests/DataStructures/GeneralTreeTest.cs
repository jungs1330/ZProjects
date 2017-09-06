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
using NGenerics.Sorting;

namespace NGenericsTests
{
	[TestFixture]
	public class GeneralTreeTest
	{
		[Test]
		public void TestSuccesfulInit()
		{
			GeneralTree<int> generalTree = new GeneralTree<int>(5);

			Assert.AreEqual(generalTree.Count, 0);
			Assert.AreEqual(generalTree.Degree, 0);
			Assert.AreEqual(generalTree.Height, 0);
			Assert.AreEqual(generalTree.IsEmpty, true);
			Assert.AreEqual(generalTree.IsFull, false);
			Assert.AreEqual(generalTree.IsLeafNode, true);
            Assert.AreEqual(generalTree.Parent, null);
		}

		[Test]
		public void TestCount()
		{
			GeneralTree<int> t = GetTestTree();

			Assert.AreEqual(t.Count, 3);

			t.Clear();
			Assert.AreEqual(t.Count, 0);

			t = new GeneralTree<int>(3);

			Assert.AreEqual(t.Count, 0);
			Assert.AreEqual(t.Degree, 0);

			t = GetTestTree();

			Assert.AreEqual(t.Count, 3);
			Assert.AreEqual(t.Degree, 3);
		}

        [Test]
        public void TestNodes()
        {
            GeneralTree<int> tree = new GeneralTree<int>(5);

            GeneralTree<int>[] originalNodes = new GeneralTree<int>[10];

            for (int i = 10; i < 20; i++)
            {
                originalNodes[i - 10] = new GeneralTree<int>(i);
                tree.Add(originalNodes[i-10]);
            }

            IList<GeneralTree<int>> childNodes = tree.ChildNodes;

            Assert.AreEqual(childNodes.Count, 10);

            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(childNodes.Contains(originalNodes[i]), true);
            }            
        }

        [Test]
        public void TestIsEmpty()
		{
			GeneralTree<int> t = GetTestTree();
			Assert.AreEqual(t.IsEmpty, false);

			t.Clear();
			Assert.AreEqual(t.IsEmpty, true);
		}

		[Test]
		public void TestIsLeafNode()
		{
			GeneralTree<int> t = GetTestTree();
			Assert.AreEqual(t.IsLeafNode, false);
			Assert.AreEqual(t.GetChild(0).GetChild(0).IsLeafNode, true);

			t.Clear();
			Assert.AreEqual(t.IsLeafNode, true);
		}

		[Test]
		public void TestBreadthFirstVisit()
		{
			GeneralTree<int> t = GetTestTree();
			TrackingVisitor<int> trackingVisitor = new TrackingVisitor<int>();

			t.BreadthFirstTraversal(trackingVisitor);

			VisitableList<int> tracks = trackingVisitor.TrackingList;

			Assert.AreEqual(tracks[0], 5);
			Assert.AreEqual(tracks[1], 2);
			Assert.AreEqual(tracks[2], 3);
			Assert.AreEqual(tracks[3], 1);
			Assert.AreEqual(tracks[4], 9);
			Assert.AreEqual(tracks[5], 12);
			Assert.AreEqual(tracks[6], 13);
		}

		[Test]
		public void TestDepthFirstVisitPre()
		{
			GeneralTree<int> t = GetTestTree();
			TrackingVisitor<int> trackingVisitor = new TrackingVisitor<int>();
			PreOrderVisitor<int> preVisitor = new PreOrderVisitor<int>(trackingVisitor);

			t.DepthFirstTraversal(preVisitor);

			VisitableList<int> tracks = trackingVisitor.TrackingList;

			Assert.AreEqual(tracks[0], 5);
			Assert.AreEqual(tracks[1], 2);
			Assert.AreEqual(tracks[2], 9);
			Assert.AreEqual(tracks[3], 12);
			Assert.AreEqual(tracks[4], 3);
			Assert.AreEqual(tracks[5], 13);
			Assert.AreEqual(tracks[6], 1);
		}

		[Test]
		public void TestDepthFirstStopVisitor()
		{
			GeneralTree<int> t = GetTestTree();
			ComparableFindingVisitor<int> visitor = new ComparableFindingVisitor<int>(13);
			PreOrderVisitor<int> preVisitor = new PreOrderVisitor<int>(visitor);

			t.DepthFirstTraversal(preVisitor);

			Assert.AreEqual(((ComparableFindingVisitor<int>)preVisitor.VisitorToUse).Found, true);
			Assert.AreEqual(((ComparableFindingVisitor<int>)preVisitor.VisitorToUse).HasCompleted, true);

			visitor = new ComparableFindingVisitor<int>(99);
			preVisitor = new PreOrderVisitor<int>(visitor);

			t.DepthFirstTraversal(preVisitor);
			Assert.AreEqual(((ComparableFindingVisitor<int>)preVisitor.VisitorToUse).Found, false);
			Assert.AreEqual(((ComparableFindingVisitor<int>)preVisitor.VisitorToUse).HasCompleted, false);
		}

		[Test]
		public void TestHeight()
		{
			GeneralTree<int> t = new GeneralTree<int>(5);

			Assert.AreEqual(t.Height, 0);

			GeneralTree<int> s1 = new GeneralTree<int>(3);

			t.Add(s1);

			Assert.AreEqual(t.Height, 1);

			s1.Add(new GeneralTree<int>(6));

			Assert.AreEqual(t.Height, 2);
			t.Add(new GeneralTree<int>(4));

			Assert.AreEqual(t.Height, 2);

			t = GetTestTree();

			Assert.AreEqual(t.Height, 2);
		}

		[Test]
		public void BreadthFirstSearchStopVisitor()
		{
			GeneralTree<int> t = GetTestTree();
			ComparableFindingVisitor<int> visitor = new ComparableFindingVisitor<int>(13);

			t.BreadthFirstTraversal(visitor);

			Assert.AreEqual(visitor.HasCompleted, true);
			Assert.AreEqual(visitor.Found, true);

			visitor = new ComparableFindingVisitor<int>(99);

			t.BreadthFirstTraversal(visitor);

			Assert.AreEqual(visitor.HasCompleted, false);
			Assert.AreEqual(visitor.Found, false);
		}

		[Test]
		public void TestDepthFirstVisitPost()
		{
			GeneralTree<int> t = GetTestTree();
			TrackingVisitor<int> trackingVisitor = new TrackingVisitor<int>();
			PostOrderVisitor<int> postVisitor = new PostOrderVisitor<int>(trackingVisitor);

			t.DepthFirstTraversal(postVisitor);

			VisitableList<int> tracks = trackingVisitor.TrackingList;

			Assert.AreEqual(tracks[0], 9);
			Assert.AreEqual(tracks[1], 12);
			Assert.AreEqual(tracks[2], 2);
			Assert.AreEqual(tracks[3], 13);
			Assert.AreEqual(tracks[4], 3);
			Assert.AreEqual(tracks[5], 1);
			Assert.AreEqual(tracks[6], 5);
		}

		[Test]
		public void TestAdd()
		{
			GeneralTree<int> root = new GeneralTree<int>(5);

			GeneralTree<int> child1 = new GeneralTree<int>(2);
			GeneralTree<int> child2 = new GeneralTree<int>(3);

			root.Add(child1);
			root.Add(child2);

            Assert.AreEqual(child1.Parent, root);
            Assert.AreEqual(child2.Parent, root);

			Assert.AreEqual(root.Count, 2);
			Assert.AreEqual(root.Degree, 2);

			Assert.AreEqual(root.GetChild(0), child1);
			Assert.AreEqual(root.GetChild(0).Data, child1.Data);

			Assert.AreEqual(root.GetChild(1).Data, child2.Data);
			Assert.AreEqual(root.GetChild(1), child2);

			root = new GeneralTree<int>(5);
			root.Add(2);
			root.Add(3);

			Assert.AreEqual(root.GetChild(0).Data, child1.Data);
			Assert.AreEqual(root.GetChild(1).Data, child2.Data);

            Assert.AreEqual(root.GetChild(0).Parent, root);
            Assert.AreEqual(root.GetChild(1).Parent, root);

            GeneralTree<int> anotherRoot = new GeneralTree<int>(2);

            GeneralTree<int> movedChild = root.GetChild(0);
            anotherRoot.Add(movedChild);

            Assert.AreEqual(movedChild.Parent, anotherRoot);
            Assert.AreEqual(root.Degree, 1);
            Assert.AreEqual(root.GetChild(0).Parent, root);
		}

		[Test]
		public void TestRemove()
		{
			GeneralTree<int> root = new GeneralTree<int>(5);

			GeneralTree<int> child1 = new GeneralTree<int>(2);
			GeneralTree<int> child2 = new GeneralTree<int>(3);
			GeneralTree<int> child3 = new GeneralTree<int>(5);
			GeneralTree<int> child4 = new GeneralTree<int>(7);

			root.Add(child1);
			root.Add(child2);

			Assert.AreEqual(root.Count, 2);
			Assert.AreEqual(root.Degree, 2);

			root.RemoveAt(0);

            Assert.AreEqual(child1.Parent, null);

			Assert.AreEqual(root.Count, 1);
			Assert.AreEqual(root.Degree, 1);
			Assert.AreEqual(root.GetChild(0), child2);
			Assert.AreEqual(root.GetChild(0).Data, 3);

			root.Add(child1);

            Assert.AreEqual(child1.Parent, root);
			Assert.AreEqual(root.Count, 2);
			Assert.AreEqual(root.Degree, 2);

			Assert.AreEqual(root.Remove(child1), true);
            Assert.AreEqual(child1.Parent, null);

			Assert.AreEqual(root.Count, 1);
			Assert.AreEqual(root.Degree, 1);
			Assert.AreEqual(root.GetChild(0), child2);
			Assert.AreEqual(root.GetChild(0).Data, 3);

			Assert.AreEqual(root.Remove(child3), false);

			root.Add(child3);
			root.Add(child4);

			Assert.AreEqual(root.Count, 3);
			Assert.AreEqual(root.Degree, 3);

            GeneralTree<int> fiveNode = root.FindNode(delegate(int target) { return target == 5; });
			Assert.AreEqual(root.Remove(5), true);

            Assert.AreEqual(fiveNode.Parent, null);

			Assert.AreEqual(root.Count, 2);
			Assert.AreEqual(root.Degree, 2);
			Assert.AreEqual(root.GetChild(1).Data, 7);

			Assert.AreEqual(root.Remove(13), false);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestInvalidIndexRemoveAt1()
		{
			GeneralTree<int> t = new GeneralTree<int>(3);
			t.RemoveAt(1);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestInvalidIndexRemoveAt2()
		{
			GeneralTree<int> t = GetTestTree();
			t.RemoveAt(5);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestInvalidIndex1()
		{
			GeneralTree<int> root = new GeneralTree<int>(5);

			GeneralTree<int> child1 = new GeneralTree<int>(2);
			GeneralTree<int> child2 = new GeneralTree<int>(3);

			root.Add(child1);
			root.Add(child2);

			int i = root[3].Data;
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestInvalidIndex2()
		{
			GeneralTree<int> root = new GeneralTree<int>(5);
			int i = root[0].Data;
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestInvalidSubTree()
		{
			GeneralTree<int> root = new GeneralTree<int>(5);

			GeneralTree<int> child1 = new GeneralTree<int>(2);
			GeneralTree<int> child2 = new GeneralTree<int>(3);

			root.Add(child1);
			root.Add(child2);

			root.GetChild(3);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestInvalidSubTreeNegative()
		{
			GeneralTree<int> root = new GeneralTree<int>(5);
			GeneralTree<int> child1 = new GeneralTree<int>(2);

			root.Add(child1);
			root.GetChild(-1);
		}

		[Test]
		public void TestCompareTo()
		{
			GeneralTree<int> t = GetTestTree();

			GeneralTree<int> biggerTree = GetTestTree();
			biggerTree.Add(new GeneralTree<int>(4));

			GeneralTree<int> smallerTree = new GeneralTree<int>(3);
			smallerTree.Add(new GeneralTree<int>(2));

			Assert.AreEqual(biggerTree.CompareTo(t), 1);
			Assert.AreEqual(t.CompareTo(biggerTree), -1);
			Assert.AreEqual(t.CompareTo(t), 0);

			object o = new object();
			Assert.AreEqual(t.CompareTo(o), -1);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestInvalidCompareTo()
		{
			GeneralTree<string> g = new GeneralTree<string>("aa");
			g.CompareTo(null);
		}

		[Test]
		public void TestEnumerator()
		{
			GeneralTree<int> t = GetTestTree();
			IEnumerator<int> enumerator = t.GetEnumerator();

			int itemCount = 7;

			while (enumerator.MoveNext())
			{
				itemCount--;
			}

			Assert.AreEqual(itemCount, 0);
		}

		[Test]
		public void TestIsReadOnly()
		{
			GeneralTree<int> t = new GeneralTree<int>(4);
			Assert.AreEqual(t.IsReadOnly, false);

			t = GetTestTree();
			Assert.AreEqual(t.IsReadOnly, false);
		}

		[Test]
		public void TestIsFixedSize()
		{
			GeneralTree<int> t = new GeneralTree<int>(4);
			Assert.AreEqual(t.IsFixedSize, false);

			t = GetTestTree();
			Assert.AreEqual(t.IsFixedSize, false);
		}

		[Test]
		public void TestInterfaceEnumerator()
		{
			GeneralTree<int> t = GetTestTree();
			System.Collections.IEnumerator enumerator = ((IEnumerable) t).GetEnumerator();

			int itemCount = 7;

			while (enumerator.MoveNext())
			{
				itemCount--;
			}

			Assert.AreEqual(itemCount, 0);
		}

		[Test]
		public void TestAccept()
		{
			GeneralTree<int> t = GetTestTree();

			TrackingVisitor<int> v = new TrackingVisitor<int>();
			t.Accept(v);

			Assert.AreEqual(v.TrackingList.Count, 7);

			Assert.AreEqual(v.TrackingList.Contains(5), true);
			Assert.AreEqual(v.TrackingList.Contains(2), true);
			Assert.AreEqual(v.TrackingList.Contains(3), true);
			Assert.AreEqual(v.TrackingList.Contains(1), true);
			Assert.AreEqual(v.TrackingList.Contains(9), true);
			Assert.AreEqual(v.TrackingList.Contains(12), true);
			Assert.AreEqual(v.TrackingList.Contains(13), true);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestNullVisitor()
		{
			GeneralTree<int> t = GetTestTree();
			t.Accept(null);
		}

		[Test]
		public void TestInterfaceAdd()
		{
			GeneralTree<int> root = new GeneralTree<int>(5);

			GeneralTree<int> child1 = new GeneralTree<int>(2);
			GeneralTree<int> child2 = new GeneralTree<int>(3);

			((ITree<int>)root).Add(child1);
			((ITree<int>)root).Add(child2);

			Assert.AreEqual(root.Count, 2);
			Assert.AreEqual(root.Degree, 2);

			Assert.AreEqual(root.GetChild(0), child1);
			Assert.AreEqual(root.GetChild(0).Data, child1.Data);

			Assert.AreEqual(root.GetChild(1).Data, child2.Data);
			Assert.AreEqual(root.GetChild(1), child2);
		}

		[Test]
		public void TestInterfaceGetChild()
		{
			ITree<int> t = GetTestTree();
			Assert.AreEqual(t.GetChild(0).Data, 2);
			Assert.AreEqual(t.GetChild(1).Data, 3);
			Assert.AreEqual(t.GetChild(2).Data, 1);
		}

		[Test]
		public void TestInterfaceRemove()
		{
			ITree<int> tree = new GeneralTree<int>(5);

			GeneralTree<int> child1 = new GeneralTree<int>(4);
			GeneralTree<int> child2 = new GeneralTree<int>(6);
			GeneralTree<int> child3 = new GeneralTree<int>(7);

			tree.Add(child1);
			tree.Add(child2);
			Assert.AreEqual(tree.Degree, 2);
			Assert.AreEqual(tree.Remove(child1), true);
			Assert.AreEqual(tree.Degree, 1);
			Assert.AreEqual(tree.Remove(child3), false);
			Assert.AreEqual(tree.Degree, 1);
			Assert.AreEqual(tree.Remove(child2), true);
			Assert.AreEqual(tree.Degree, 0);
		}

		[Test]
		public void TestCopyTo()
		{
			GeneralTree<int> t = GetTestTree();

			int[] array = new int[7];
			t.CopyTo(array, 0);

			List<int> l = new List<int>(array);
			Assert.AreEqual(l.Contains(5), true);
			Assert.AreEqual(l.Contains(2), true);
			Assert.AreEqual(l.Contains(3), true);
			Assert.AreEqual(l.Contains(1), true);
			Assert.AreEqual(l.Contains(9), true);
			Assert.AreEqual(l.Contains(12), true);
			Assert.AreEqual(l.Contains(13), true);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestNullCopyTo()
		{
			GeneralTree<int> t = GetTestTree();
			t.CopyTo(null, 1);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestInvalidCopyTo1()
		{
			GeneralTree<int> t = GetTestTree();
			int[] array = new int[6];
			t.CopyTo(array, 0);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestInvalidCopyTo2()
		{
			GeneralTree<int> t = GetTestTree();
			int[] array = new int[7];
			t.CopyTo(array, 1);
		}

		[Test]
		public void TestContains()
		{
			GeneralTree<int> t = GetTestTree();
			Assert.AreEqual(t.Contains(5), true);
			Assert.AreEqual(t.Contains(2), true);
			Assert.AreEqual(t.Contains(3), true);
			Assert.AreEqual(t.Contains(1), true);
			Assert.AreEqual(t.Contains(9), true);
			Assert.AreEqual(t.Contains(12), true);
			Assert.AreEqual(t.Contains(13), true);
			Assert.AreEqual(t.Contains(4), false);
		}

		[Test]
		public void TestIndexer()
		{
			GeneralTree<int> t = GetTestTree();
			Assert.AreEqual(t[0].Data, 2);
			Assert.AreEqual(t[1].Data, 3);
			Assert.AreEqual(t[2].Data, 1);
		}

		[Test]
		public void TestFind()
		{
			GeneralTree<int> root = new GeneralTree<int>(5);

			GeneralTree<int> child1 = new GeneralTree<int>(2);
			GeneralTree<int> child2 = new GeneralTree<int>(3);
			GeneralTree<int> child3 = new GeneralTree<int>(1);

			root.Add(child1);
			root.Add(child2);
			root.Add(child3);

			GeneralTree<int> child4 = new GeneralTree<int>(9);
			GeneralTree<int> child5 = new GeneralTree<int>(12);
			GeneralTree<int> child6 = new GeneralTree<int>(13);

			child1.Add(child4);
			child1.Add(child5);
			child2.Add(child6);

			Assert.AreEqual(root.FindNode(
				delegate(int target)
				{
					return target == 13;
				})
				,
				child6
			);

			Assert.AreEqual(root.FindNode(
				delegate(int target)
				{
					return target == 2;
				})
				,
				child1
			);

			Assert.AreEqual(root.FindNode(
				delegate(int target)
				{
					return target == 9;
				})
				,
				child4
			);

			Assert.AreEqual(root.FindNode(
				delegate(int target)
				{
					return target == 57;
				})
				,
				null
			);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestNullFind()
		{
			GeneralTree<int> tree = GetTestTree();
			tree.FindNode(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestInterfaceNullFind()
		{
			GeneralTree<int> tree = GetTestTree();
			((ITree<int>)tree).FindNode(null);
		}

		[Test]
		public void TestInterfaceFind()
		{
			GeneralTree<int> root = new GeneralTree<int>(5);

			GeneralTree<int> child1 = new GeneralTree<int>(2);
			GeneralTree<int> child2 = new GeneralTree<int>(3);
			GeneralTree<int> child3 = new GeneralTree<int>(1);

			root.Add(child1);
			root.Add(child2);
			root.Add(child3);

			GeneralTree<int> child4 = new GeneralTree<int>(9);
			GeneralTree<int> child5 = new GeneralTree<int>(12);
			GeneralTree<int> child6 = new GeneralTree<int>(13);

			child1.Add(child4);
			child1.Add(child5);
			child2.Add(child6);

			Assert.AreEqual(((ITree<int>)root).FindNode(
				delegate(int target)
				{
					return target == 13;
				})
				,
				child6
			);

			Assert.AreEqual(((ITree<int>)root).FindNode(
				delegate(int target)
				{
					return target == 2;
				})
				,
				child1
			);

			Assert.AreEqual(((ITree<int>)root).FindNode(
				delegate(int target)
				{
					return target == 9;
				})
				,
				child4
			);

			Assert.AreEqual(((ITree<int>)root).FindNode(
				delegate(int target)
				{
					return target == 57;
				})
				,
				null
			);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestFindNull()
		{
			GeneralTree<int> tree = GetTestTree();
			tree.FindNode(null);
		}


        [Test]
        public void TestSort1()
        {
            GeneralTree<int> tree = GetTestTree();
            
            tree.Sort(new InsertionSorter<GeneralTree<int>>(),
                delegate(GeneralTree<int> x, GeneralTree<int> y)
                {
                    return x.Data.CompareTo(y.Data);
                }
            );

            Assert.AreEqual(tree.ChildNodes[0].Data, 1);
            Assert.AreEqual(tree.ChildNodes[1].Data, 2);
            Assert.AreEqual(tree.ChildNodes[2].Data, 3);
        }

        [Test]
        public void TestSort2()
        {
            GeneralTree<int> tree = GetTestTree();

            tree.Sort(new InsertionSorter<GeneralTree<int>>(),
                new GeneralTreeComparer<int>());

            Assert.AreEqual(tree.ChildNodes[0].Data, 1);
            Assert.AreEqual(tree.ChildNodes[1].Data, 2);
            Assert.AreEqual(tree.ChildNodes[2].Data, 3);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullSorter1()
        {
            GeneralTree<int> tree = GetTestTree();

            tree.Sort(null,
                delegate(GeneralTree<int> x, GeneralTree<int> y)
                {
                    return x.Data.CompareTo(y.Data);
                }
            );
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullSorter2()
        {
            GeneralTree<int> tree = GetTestTree();

            tree.Sort(null,
                new GeneralTreeComparer<int>());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSortNullComparison()
        {
            GeneralTree<int> tree = GetTestTree();
            tree.Sort(new InsertionSorter<GeneralTree<int>>(), (Comparison<GeneralTree<int>>) null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSortNullComparer()
        {
            GeneralTree<int> tree = GetTestTree();

            tree.Sort(new InsertionSorter<GeneralTree<int>>(),
               (IComparer<GeneralTree<int>>) null);
        }

        [Test]
        public void TestRecursiveSort1()
        {
            GeneralTree<int> tree = GetMixedTestTree();

            tree.SortAllDescendants(new InsertionSorter<GeneralTree<int>>(),
                delegate(GeneralTree<int> x, GeneralTree<int> y)
                {
                    return x.Data.CompareTo(y.Data);
                }
            );

            Assert.AreEqual(tree.ChildNodes[0].Data, 1);
            Assert.AreEqual(tree.ChildNodes[1].Data, 2);
            Assert.AreEqual(tree.ChildNodes[2].Data, 3);

            Assert.AreEqual(tree[1].ChildNodes[0].Data, 11);
            Assert.AreEqual(tree[1].ChildNodes[1].Data, 15);

            Assert.AreEqual(tree[2].ChildNodes[0].Data, 13);
        }

        [Test]
        public void TestRecursiveSort2()
        {
            GeneralTree<int> tree = GetMixedTestTree();

            tree.SortAllDescendants(new InsertionSorter<GeneralTree<int>>(),
                new GeneralTreeComparer<int>());

            Assert.AreEqual(tree.ChildNodes[0].Data, 1);
            Assert.AreEqual(tree.ChildNodes[1].Data, 2);
            Assert.AreEqual(tree.ChildNodes[2].Data, 3);

            Assert.AreEqual(tree[1].ChildNodes[0].Data, 11);
            Assert.AreEqual(tree[1].ChildNodes[1].Data, 15);

            Assert.AreEqual(tree[2].ChildNodes[0].Data, 13);
        }

        [Test]
        public void TestAncestors()
        {
            GeneralTree<int> tree = GetTestTree();
            GeneralTree<int>[] ancestors = tree.GetChild(0).GetChild(0).Ancestors;

            Assert.AreEqual(ancestors.Length, 2);
            Assert.AreEqual(ancestors[0], tree.GetChild(0));
            Assert.AreEqual(ancestors[1], tree);

            GeneralTree<int> root = new GeneralTree<int>(5);

            GeneralTree<int> child1 = new GeneralTree<int>(2);
            GeneralTree<int> child2 = new GeneralTree<int>(3);
            GeneralTree<int> child3 = new GeneralTree<int>(1);

            root.Add(child1);
            root.Add(child2);
            root.Add(child3);

            GeneralTree<int> child4 = new GeneralTree<int>(9);
            GeneralTree<int> child5 = new GeneralTree<int>(12);
            GeneralTree<int> child6 = new GeneralTree<int>(13);
            GeneralTree<int> child7 = new GeneralTree<int>(15);

            child1.Add(child4);
            child1.Add(child5);
            child2.Add(child6);

            child4.Add(child7);

            ancestors = child7.Ancestors;

            Assert.AreEqual(ancestors.Length, 3);
            Assert.AreEqual(ancestors[0], child4);
            Assert.AreEqual(ancestors[1], child1);
            Assert.AreEqual(ancestors[2], root);

        }
     
        private GeneralTree<int> GetTestTree()
		{
			GeneralTree<int> root = new GeneralTree<int>(5);

			GeneralTree<int> child1 = new GeneralTree<int>(2);
			GeneralTree<int> child2 = new GeneralTree<int>(3);
			GeneralTree<int> child3 = new GeneralTree<int>(1);

			root.Add(child1);
			root.Add(child2);
			root.Add(child3);

			GeneralTree<int> child4 = new GeneralTree<int>(9);
			GeneralTree<int> child5 = new GeneralTree<int>(12);
			GeneralTree<int> child6 = new GeneralTree<int>(13);

			child1.Add(child4);
			child1.Add(child5);
			child2.Add(child6);

			return root;
		}

        private GeneralTree<int> GetMixedTestTree()
        {
            GeneralTree<int> root = new GeneralTree<int>(5);

            GeneralTree<int> child1 = new GeneralTree<int>(2);
            GeneralTree<int> child2 = new GeneralTree<int>(3);
            GeneralTree<int> child3 = new GeneralTree<int>(1);

            root.Add(child1);
            root.Add(child2);
            root.Add(child3);

            GeneralTree<int> child4 = new GeneralTree<int>(15);
            GeneralTree<int> child5 = new GeneralTree<int>(11);
            GeneralTree<int> child6 = new GeneralTree<int>(13);

            child1.Add(child4);
            child1.Add(child5);
            child2.Add(child6);

            return root;
        }
	}

    internal class GeneralTreeComparer<T> : IComparer<GeneralTree<T>> where T : IComparable
    {
        #region IComparer<GeneralTree<T>> Members

        public int Compare(GeneralTree<T> x, GeneralTree<T> y)
        {
            return x.Data.CompareTo(y.Data);
        }

        #endregion
    }
}


