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

namespace NGenericsTests
{
	[TestFixture]
	public class BinaryTreeTest
	{
		[Test]
		public void TestSuccesfulInit()
		{
			BinaryTree<int> tree = new BinaryTree<int>(5);

			Assert.AreEqual(tree.Count, 0);
			Assert.AreEqual(tree.Degree, 0);
			Assert.AreEqual(tree.Data, 5);

			tree = new BinaryTree<int>(5, new BinaryTree<int>(3), new BinaryTree<int>(4));

			Assert.AreEqual(tree.Count, 2);
			Assert.AreEqual(tree.Degree, 2);
			Assert.AreEqual(tree.Data, 5);

			Assert.AreEqual(tree.Left.Data, 3);
			Assert.AreEqual(tree.Right.Data, 4);

			tree = new BinaryTree<int>(5, 3, 4);

			Assert.AreEqual(tree.Count, 2);
			Assert.AreEqual(tree.Degree, 2);
			Assert.AreEqual(tree.Data, 5);

			Assert.AreEqual(tree.Left.Data, 3);
			Assert.AreEqual(tree.Right.Data, 4);
		}

		[Test]
		public void TestAdd()
		{
			BinaryTree<int> tree = new BinaryTree<int>(5);

			Assert.AreEqual(tree.Count, 0);
			Assert.AreEqual(tree.Degree, 0);
			Assert.AreEqual(tree.Data, 5);

			tree.Add(3);

			Assert.AreEqual(tree.Count, 1);
			Assert.AreEqual(tree.Degree, 1);
			Assert.AreEqual(tree.Data, 5);

			Assert.AreEqual(tree.Left.Data, 3);
			Assert.AreEqual(tree.Right, null);

			tree.Add(4);

			Assert.AreEqual(tree.Count, 2);
			Assert.AreEqual(tree.Degree, 2);
			Assert.AreEqual(tree.Data, 5);

			Assert.AreEqual(tree.Left.Data, 3);
			Assert.AreEqual(tree.Right.Data, 4);
		}

		[Test]
		public void TestInterfaceAdd()
		{
			ITree<int> tree = new BinaryTree<int>(5);

			BinaryTree<int> child = new BinaryTree<int>(4);
			tree.Add(child);
			Assert.AreEqual(tree.Degree, 1);
			Assert.AreEqual(tree.GetChild(0), child);
		}

		[Test]
		public void TestInterfaceGetChild()
		{
			ITree<int> tree = new BinaryTree<int>(5);

			BinaryTree<int> child1 = new BinaryTree<int>(4);
			BinaryTree<int> child2 = new BinaryTree<int>(6);

			tree.Add(child1);
			tree.Add(child2);
			Assert.AreEqual(tree.Degree, 2);
			Assert.AreEqual(tree.GetChild(0), child1);
			Assert.AreEqual(tree.GetChild(1), child2);
		}

		[Test]
		public void TestInterfaceRemove()
		{
			ITree<int> tree = new BinaryTree<int>(5);

			BinaryTree<int> child1 = new BinaryTree<int>(4);
			BinaryTree<int> child2 = new BinaryTree<int>(6);
			BinaryTree<int> child3 = new BinaryTree<int>(7);

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
		[ExpectedException(typeof(InvalidOperationException))]
		public void TestBadAdd1()
		{
			BinaryTree<int> tree = new BinaryTree<int>(5, new BinaryTree<int>(3), new BinaryTree<int>(4));

			tree.Add(99);
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void TestBadAdd2()
		{
			BinaryTree<int> tree = new BinaryTree<int>(5, new BinaryTree<int>(3), new BinaryTree<int>(4));
			tree.Add(new BinaryTree<int>(99));
		}

		[Test]
		public void TestIsLeafNode()
		{
			BinaryTree<int> t = GetTestTree();
			Assert.AreEqual(t.IsLeafNode, false);
			Assert.AreEqual(t.GetChild(0).GetChild(0).IsLeafNode, true);

			t.Clear();
			Assert.AreEqual(t.IsLeafNode, true);
		}


		[Test]
		public void TestBreadthFirstVisit()
		{
			BinaryTree<int> t = GetTestTree();
			TrackingVisitor<int> trackingVisitor = new TrackingVisitor<int>();

			t.BreadthFirstTraversal(trackingVisitor);

			VisitableList<int> tracks = trackingVisitor.TrackingList;

			Assert.AreEqual(tracks[0], 5);
			Assert.AreEqual(tracks[1], 2);
			Assert.AreEqual(tracks[2], 3);
			Assert.AreEqual(tracks[3], 9);
			Assert.AreEqual(tracks[4], 12);
			Assert.AreEqual(tracks[5], 13);
		}

		[Test]
		public void TestDepthFirstVisitPre()
		{
			BinaryTree<int> t = GetTestTree();
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
		}

		[Test]
		public void TestDepthFirstVisitIn()
		{
			BinaryTree<int> t = GetTestTree();
			TrackingVisitor<int> trackingVisitor = new TrackingVisitor<int>();
			InOrderVisitor<int> inVisitor = new InOrderVisitor<int>(trackingVisitor);

			t.DepthFirstTraversal(inVisitor);

			VisitableList<int> tracks = trackingVisitor.TrackingList;

			Assert.AreEqual(tracks[0], 9);
			Assert.AreEqual(tracks[1], 2);
			Assert.AreEqual(tracks[2], 12);
			Assert.AreEqual(tracks[3], 5);
			Assert.AreEqual(tracks[4], 13);
			Assert.AreEqual(tracks[5], 3);
		}

		[Test]
		public void TestDepthFirstStopVisitor()
		{
			BinaryTree<int> t = GetTestTree();
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
			BinaryTree<int> t = new BinaryTree<int>(5);

			Assert.AreEqual(t.Height, 0);

			t.Add(3);

			BinaryTree<int> s1 = t.Left;


			Assert.AreEqual(t.Height, 1);

			s1.Add(6);

			Assert.AreEqual(t.Height, 2);
			t.Add(4);

			Assert.AreEqual(t.Height, 2);

			t = GetTestTree();

			Assert.AreEqual(t.Height, 2);
		}

		[Test]
		public void BreadthFirstSearchStopVisitor()
		{
			BinaryTree<int> t = GetTestTree();
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
			BinaryTree<int> t = GetTestTree();
			TrackingVisitor<int> trackingVisitor = new TrackingVisitor<int>();
			PostOrderVisitor<int> postVisitor = new PostOrderVisitor<int>(trackingVisitor);

			t.DepthFirstTraversal(postVisitor);

			VisitableList<int> tracks = trackingVisitor.TrackingList;

			Assert.AreEqual(tracks[0], 9);
			Assert.AreEqual(tracks[1], 12);
			Assert.AreEqual(tracks[2], 2);
			Assert.AreEqual(tracks[3], 13);
			Assert.AreEqual(tracks[4], 3);
			Assert.AreEqual(tracks[5], 5);
		}

		[Test]
		public void TestClear()
		{
			BinaryTree<int> tree = new BinaryTree<int>(5);
			tree.Clear();

			Assert.AreEqual(tree.Count, 0);
			Assert.AreEqual(tree.IsEmpty, true);

			tree = GetTestTree();
			tree.Clear();

			Assert.AreEqual(tree.Count, 0);
			Assert.AreEqual(tree.IsEmpty, true);
		}

		[Test]
		public void TestCompareTo()
		{
			BinaryTree<int> tree1 = new BinaryTree<int>(5);
			BinaryTree<int> tree2 = new BinaryTree<int>(5, 3, 4);

			Assert.AreEqual(tree1.CompareTo(tree2), -1);
			Assert.AreEqual(tree2.CompareTo(tree1), 1);

			Assert.AreEqual(tree1.CompareTo(tree1), 0);
			Assert.AreEqual(tree1.CompareTo(new object()), -1);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestInvalidCompareTo()
		{
			BinaryTree<string> b = new BinaryTree<string>("3");
			b.CompareTo(null);
		}

		[Test]
		public void TestContains()
		{
			BinaryTree<int> tree = GetTestTree();
			Assert.AreEqual(tree.Contains(9), true);
			Assert.AreEqual(tree.Contains(12), true);
			Assert.AreEqual(tree.Contains(13), true);
			Assert.AreEqual(tree.Contains(15), false);
		}

		[Test]
		public void TestCopyTo()
		{
			BinaryTree<int> tree = GetTestTree();

			int[] array = new int[6];

			tree.CopyTo(array, 0);

			for (int i = 0; i < array.Length; i++)
			{
				Assert.AreNotEqual(array[i], 0);
			}

			List<int> list = new List<int>();
			list.AddRange(array);

			Assert.AreEqual(list.Contains(2), true);
			Assert.AreEqual(list.Contains(3), true);
			Assert.AreEqual(list.Contains(5), true);
			Assert.AreEqual(list.Contains(9), true);
			Assert.AreEqual(list.Contains(12), true);
			Assert.AreEqual(list.Contains(13), true);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestInvalidCopyTo1()
		{
			BinaryTree<int> tree = GetTestTree();

			int[] array = new int[5];

			tree.CopyTo(array, 0);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestInvalidCopyTo2()
		{
			BinaryTree<int> tree = GetTestTree();

			int[] array = new int[6];

			tree.CopyTo(array, 1);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestInvalidCopyToNull()
		{
			BinaryTree<int> tree = GetTestTree();
			tree.CopyTo(null, 1);
		}

		[Test]
		public void TestIndexer()
		{
			BinaryTree<int> tree = GetTestTree();

			Assert.AreEqual(tree[0].Data, 2);
			Assert.AreEqual(tree[1].Data, 3);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestBadIndexer()
		{
			BinaryTree<int> tree = GetTestTree();

			int i = tree[2].Data;
		}

		[Test]
		public void TestReadOnly()
		{
			BinaryTree<int> tree = GetTestTree();
			Assert.AreEqual(tree.IsReadOnly, false);
		}

		[Test]
		public void TestEnumerator()
		{
			BinaryTree<int> t = GetTestTree();
			IEnumerator<int> enumerator = t.GetEnumerator();

			int itemCount = 6;

			while (enumerator.MoveNext())
			{
				itemCount--;
			}

			Assert.AreEqual(itemCount, 0);
		}

		[Test]
		public void TestIsFixedSize()
		{
			BinaryTree<int> t = new BinaryTree<int>(5);
			Assert.AreEqual(t.IsFixedSize, true);

			t = GetTestTree();
			Assert.AreEqual(t.IsFixedSize, true);
		}

		[Test]
		public void TestIsFull()
		{
			BinaryTree<int> t = new BinaryTree<int>(4);
			Assert.AreEqual(t.IsFull, false);

			t.Left = new BinaryTree<int>(3);
			Assert.AreEqual(t.IsFull, false);

			t.Right = new BinaryTree<int>(4);
			Assert.AreEqual(t.IsFull, true);

			t.RemoveLeft();
			Assert.AreEqual(t.IsFull, false);
		}

		[Test]
		public void TestInterfaceEnumerator()
		{
			IEnumerable<int> t = GetTestTree();
			System.Collections.IEnumerator enumerator = t.GetEnumerator();

			int itemCount = 6;

			while (enumerator.MoveNext())
			{
				itemCount--;
			}

			Assert.AreEqual(itemCount, 0);
		}

		[Test]
		public void TestAccept()
		{
			BinaryTree<int> t = GetTestTree();
			TrackingVisitor<int> v = new TrackingVisitor<int>();
			t.Accept(v);

			Assert.AreEqual(v.TrackingList.Count, 6);

			Assert.AreEqual(v.TrackingList.Contains(5), true);
			Assert.AreEqual(v.TrackingList.Contains(2), true);
			Assert.AreEqual(v.TrackingList.Contains(3), true);
			Assert.AreEqual(v.TrackingList.Contains(9), true);
			Assert.AreEqual(v.TrackingList.Contains(12), true);
			Assert.AreEqual(v.TrackingList.Contains(13), true);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestNullVisitor()
		{
			BinaryTree<int> t = GetTestTree();
			t.Accept(null);
		}

		[Test]
		public void TestRemove()
		{
			BinaryTree<int> t = GetTestTree();

			Assert.AreEqual(t.Remove(2), true);
			Assert.AreEqual(t.Remove(5), false);
			Assert.AreEqual(t.Remove(3), true);

			Assert.AreEqual(t.Left, null);
			Assert.AreEqual(t.Right, null);
		}

		[Test]
		public void TestRemoveLeft()
		{
			BinaryTree<int> t = GetTestTree();

			t.RemoveLeft();

			Assert.AreEqual(t.Left, null);
			Assert.AreNotEqual(t.Right, null);
		}

		[Test]
		public void TestRemoveRight()
		{
			BinaryTree<int> t = GetTestTree();

			t.RemoveRight();

			Assert.AreEqual(t.Right, null);
			Assert.AreNotEqual(t.Left, null);
		}

		[Test]
		public void TestSetValue()
		{
			BinaryTree<int> t = GetTestTree();
			t.Data = 2;

			Assert.AreEqual(t.Data, 2);
		}

		[Test]
		public void TestSetLeft()
		{
			BinaryTree<int> t = GetTestTree();
			t.Left = new BinaryTree<int>(99);
			Assert.AreEqual(t.Left.Data, 99);
			Assert.AreNotEqual(t.Right, null);
			Assert.AreNotEqual(t.Right.Data, 99);
		}

		[Test]
		public void TestSetRight()
		{
			BinaryTree<int> t = GetTestTree();
			t.Right = new BinaryTree<int>(99);

			Assert.AreNotEqual(t.Left, null);
			Assert.AreNotEqual(t.Left.Data, 99);
			Assert.AreEqual(t.Right.Data, 99);
		}

		[Test]
		public void TestFind()
		{
			BinaryTree<int> root = new BinaryTree<int>(5);

			BinaryTree<int> child1 = new BinaryTree<int>(2);
			BinaryTree<int> child2 = new BinaryTree<int>(3);

			root.Add(child1);
			root.Add(child2);

			BinaryTree<int> child4 = new BinaryTree<int>(9);
			BinaryTree<int> child5 = new BinaryTree<int>(12);
			BinaryTree<int> child6 = new BinaryTree<int>(13);

			child1.Add(child4);
			child1.Add(child5);
			child2.Add(child6);

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
					return target == 13;
				})
				,
				child6
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
		public void TestInterfaceFind()
		{
			BinaryTree<int> root = new BinaryTree<int>(5);

			BinaryTree<int> child1 = new BinaryTree<int>(2);
			BinaryTree<int> child2 = new BinaryTree<int>(3);

			root.Add(child1);
			root.Add(child2);

			BinaryTree<int> child4 = new BinaryTree<int>(9);
			BinaryTree<int> child5 = new BinaryTree<int>(12);
			BinaryTree<int> child6 = new BinaryTree<int>(13);

			child1.Add(child4);
			child1.Add(child5);
			child2.Add(child6);

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
					return target == 13;
				})
				,
				child6
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
		public void TestNullFind()
		{
			BinaryTree<int> tree = GetTestTree();
			tree.FindNode(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestInterfaceNullFind()
		{
			BinaryTree<int> tree = GetTestTree();
			((ITree<int>)tree).FindNode(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestFindNull()
		{
			BinaryTree<int> tree = GetTestTree();
			tree.FindNode(null);
		}

		private BinaryTree<int> GetTestTree()
		{
			BinaryTree<int> root = new BinaryTree<int>(5);

			BinaryTree<int> child1 = new BinaryTree<int>(2);
			BinaryTree<int> child2 = new BinaryTree<int>(3);

			root.Add(child1);
			root.Add(child2);

			BinaryTree<int> child4 = new BinaryTree<int>(9);
			BinaryTree<int> child5 = new BinaryTree<int>(12);
			BinaryTree<int> child6 = new BinaryTree<int>(13);

			child1.Add(child4);
			child1.Add(child5);
			child2.Add(child6);

			return root;
		}

	}
}
