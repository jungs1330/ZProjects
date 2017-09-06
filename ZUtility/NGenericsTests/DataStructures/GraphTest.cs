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
	public class GraphTest
	{
		[Test]
		public void TestSuccesfulInit()
		{
			Graph<int> graph = new Graph<int>(false);

			Assert.AreEqual(graph.VertexCount, 0);
			Assert.AreEqual(graph.VertexCount, 0);
			Assert.AreEqual(graph.EdgeCount, 0);
			Assert.AreEqual(graph.IsDirected, false);

			graph = new Graph<int>(true);

			Assert.AreEqual(graph.VertexCount, 0);
			Assert.AreEqual(graph.VertexCount, 0);
			Assert.AreEqual(graph.EdgeCount, 0);
			Assert.AreEqual(graph.IsDirected, true);
			
		}        

		[Test]
		public void TestSuccesfulVertexInit()
		{
			Vertex<int> v = new Vertex<int>(4);
			Assert.AreEqual(v.Data, 4);
			Assert.AreEqual(v.Degree, 0);
			Assert.AreEqual(v.IncidentEdgesCount, 0);
			Assert.AreEqual(v.Weight, 0);

			v = new Vertex<int>(999);
			Assert.AreEqual(v.Data, 999);
			Assert.AreEqual(v.Degree, 0);
			Assert.AreEqual(v.IncidentEdgesCount, 0);
			Assert.AreEqual(v.Weight, 0);

			v = new Vertex<int>(4, 6.2);
			Assert.AreEqual(v.Data, 4);
			Assert.AreEqual(v.Degree, 0);
			Assert.AreEqual(v.IncidentEdgesCount, 0);
			Assert.AreEqual(v.Weight, 6.2);

			v = new Vertex<int>(999, 32.45);
			Assert.AreEqual(v.Data, 999);
			Assert.AreEqual(v.Degree, 0);
			Assert.AreEqual(v.IncidentEdgesCount, 0);
			Assert.AreEqual(v.Weight, 32.45);

            v.Weight = 55;
            Assert.AreEqual(v.Weight, 55);
		}

		[Test]
		public void TestSuccesfulEdgeInit()
		{
			Vertex<int> v1 = new Vertex<int>(6);
			Vertex<int> v2 = new Vertex<int>(4);

			Edge<int> edge = new Edge<int>(v1, v2, true);
			Assert.AreEqual(edge.FromVertex, v1);
			Assert.AreEqual(edge.ToVertex, v2);
			Assert.AreEqual(edge.Weight, 0);

			edge = new Edge<int>(v1, v2, 55, true);
			Assert.AreEqual(edge.FromVertex, v1);
			Assert.AreEqual(edge.ToVertex, v2);
			Assert.AreEqual(edge.Weight, 55);

			edge = new Edge<int>(v1, v2, -2, true);
			Assert.AreEqual(edge.FromVertex, v1);
			Assert.AreEqual(edge.ToVertex, v2);
			Assert.AreEqual(edge.Weight, -2);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestEdgeVertexNullDirected1()
		{
			Edge<int> edge = new Edge<int>(null, new Vertex<int>(4), true);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestEdgeVertexNullDirected2()
		{
			Edge<int> edge = new Edge<int>(new Vertex<int>(4), null, true);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestEdgeVertexNullUndirected1()
		{
			Edge<int> edge = new Edge<int>(null, new Vertex<int>(4), false);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestEdgeVertexNullUndirected2()
		{
			Edge<int> edge = new Edge<int>(new Vertex<int>(4), null, false);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestInvalidEdgeDirected()
		{
			Graph<int> g = GetTestDirectedGraph();

			IEnumerator<Vertex<int>> vertices = g.Vertices;

			vertices.MoveNext();
			Vertex<int> v1 = vertices.Current;

			vertices.MoveNext();
			Vertex<int> v2 = vertices.Current;

			Edge<int> e = new Edge<int>(v1, v2, false);
			g.AddEdge(e);
		}

		[Test]
		public void TestDepthFirstTraversalUndirectedPreVisit()
		{
			Graph<int> g = new Graph<int>(false);
			Vertex<int> v1 = new Vertex<int>(1);
			Vertex<int> v2 = new Vertex<int>(2);
			Vertex<int> v3 = new Vertex<int>(3);
			Vertex<int> v4 = new Vertex<int>(4);
			Vertex<int> v5 = new Vertex<int>(5);
			Vertex<int> v6 = new Vertex<int>(6);
			Vertex<int> v7 = new Vertex<int>(7);

			g.AddVertex(v1);
			g.AddVertex(v2);
			g.AddVertex(v3);
			g.AddVertex(v4);
			g.AddVertex(v5);
			g.AddVertex(v6);
			g.AddVertex(v7);

			g.AddEdge(v1, v2);
			g.AddEdge(v1, v3);
			g.AddEdge(v1, v5);
			g.AddEdge(v5, v3);

			g.AddEdge(v3, v6);
			g.AddEdge(v3, v4);
			g.AddEdge(v2, v7);

			TrackingVisitor<Vertex<int>> v = new TrackingVisitor<Vertex<int>>();
			PreOrderVisitor<Vertex<int>> pV = new PreOrderVisitor<Vertex<int>>(v);

			g.DepthFirstTraversal(pV, v1);

			Assert.AreEqual(v.TrackingList.Count, g.VertexCount);

			Assert.AreEqual(v.TrackingList[0].Data, 1);
			Assert.AreEqual(v.TrackingList[1].Data, 2);
			Assert.AreEqual(v.TrackingList[2].Data, 7);
			Assert.AreEqual(v.TrackingList[3].Data, 3);
			Assert.AreEqual(v.TrackingList[4].Data, 5);
			Assert.AreEqual(v.TrackingList[5].Data, 6);
			Assert.AreEqual(v.TrackingList[6].Data, 4);
		}

		[Test]
		public void TestDepthFirstTraversalUndirectedPostVisit()
		{
			Graph<int> g = new Graph<int>(false);
			Vertex<int> v1 = new Vertex<int>(1);
			Vertex<int> v2 = new Vertex<int>(2);
			Vertex<int> v3 = new Vertex<int>(3);
			Vertex<int> v4 = new Vertex<int>(4);
			Vertex<int> v5 = new Vertex<int>(5);
			Vertex<int> v6 = new Vertex<int>(6);
			Vertex<int> v7 = new Vertex<int>(7);

			g.AddVertex(v1);
			g.AddVertex(v2);
			g.AddVertex(v3);
			g.AddVertex(v4);
			g.AddVertex(v5);
			g.AddVertex(v6);
			g.AddVertex(v7);

			g.AddEdge(v1, v2);
			g.AddEdge(v1, v3);
			g.AddEdge(v1, v5);
			g.AddEdge(v5, v3);

			g.AddEdge(v3, v6);
			g.AddEdge(v3, v4);
			g.AddEdge(v2, v7);

			TrackingVisitor<Vertex<int>> v = new TrackingVisitor<Vertex<int>>();
			PostOrderVisitor<Vertex<int>> pV = new PostOrderVisitor<Vertex<int>>(v);

			g.DepthFirstTraversal(pV, v1);

			Assert.AreEqual(v.TrackingList.Count, g.VertexCount);

			Assert.AreEqual(v.TrackingList[0].Data, 7);
			Assert.AreEqual(v.TrackingList[1].Data, 2);
			Assert.AreEqual(v.TrackingList[2].Data, 5);
			Assert.AreEqual(v.TrackingList[3].Data, 6);
			Assert.AreEqual(v.TrackingList[4].Data, 4);
			Assert.AreEqual(v.TrackingList[5].Data, 3);
			Assert.AreEqual(v.TrackingList[6].Data, 1);
		}

		[Test]
		public void TestDepthFirstTraversalDirectedPreVisit()
		{
			Graph<int> g = new Graph<int>(true);
			Vertex<int> v1 = new Vertex<int>(1);
			Vertex<int> v2 = new Vertex<int>(2);
			Vertex<int> v3 = new Vertex<int>(3);
			Vertex<int> v4 = new Vertex<int>(4);
			Vertex<int> v5 = new Vertex<int>(5);
			Vertex<int> v6 = new Vertex<int>(6);
			Vertex<int> v7 = new Vertex<int>(7);

			g.AddVertex(v1);
			g.AddVertex(v2);
			g.AddVertex(v3);
			g.AddVertex(v4);
			g.AddVertex(v5);
			g.AddVertex(v6);
			g.AddVertex(v7);

			g.AddEdge(v1, v2);
			g.AddEdge(v1, v3);
			g.AddEdge(v1, v5);
			g.AddEdge(v5, v3);

			g.AddEdge(v3, v6);
			g.AddEdge(v3, v4);
			g.AddEdge(v2, v7);

			TrackingVisitor<Vertex<int>> v = new TrackingVisitor<Vertex<int>>();
			PreOrderVisitor<Vertex<int>> pV = new PreOrderVisitor<Vertex<int>>(v);

			g.DepthFirstTraversal(pV, v1);

			Assert.AreEqual(v.TrackingList.Count, g.VertexCount);

			Assert.AreEqual(v.TrackingList[0].Data, 1);
			Assert.AreEqual(v.TrackingList[1].Data, 2);
			Assert.AreEqual(v.TrackingList[2].Data, 7);
			Assert.AreEqual(v.TrackingList[3].Data, 3);
			Assert.AreEqual(v.TrackingList[4].Data, 6);
			Assert.AreEqual(v.TrackingList[5].Data, 4);
			Assert.AreEqual(v.TrackingList[6].Data, 5);
		}

		[Test]
		public void TestDepthFirstTraversalDirectedPostVisit()
		{
			Graph<int> g = new Graph<int>(true);
			Vertex<int> v1 = new Vertex<int>(1);
			Vertex<int> v2 = new Vertex<int>(2);
			Vertex<int> v3 = new Vertex<int>(3);
			Vertex<int> v4 = new Vertex<int>(4);
			Vertex<int> v5 = new Vertex<int>(5);
			Vertex<int> v6 = new Vertex<int>(6);
			Vertex<int> v7 = new Vertex<int>(7);

			g.AddVertex(v1);
			g.AddVertex(v2);
			g.AddVertex(v3);
			g.AddVertex(v4);
			g.AddVertex(v5);
			g.AddVertex(v6);
			g.AddVertex(v7);

			g.AddEdge(v1, v2);
			g.AddEdge(v1, v3);
			g.AddEdge(v1, v5);
			g.AddEdge(v5, v3);

			g.AddEdge(v3, v6);
			g.AddEdge(v3, v4);
			g.AddEdge(v2, v7);

			TrackingVisitor<Vertex<int>> v = new TrackingVisitor<Vertex<int>>();
			PostOrderVisitor<Vertex<int>> pV = new PostOrderVisitor<Vertex<int>>(v);

			g.DepthFirstTraversal(pV, v1);

			Assert.AreEqual(v.TrackingList.Count, g.VertexCount);

			Assert.AreEqual(v.TrackingList[0].Data, 7);
			Assert.AreEqual(v.TrackingList[1].Data, 2);
			Assert.AreEqual(v.TrackingList[2].Data, 6);
			Assert.AreEqual(v.TrackingList[3].Data, 4);
			Assert.AreEqual(v.TrackingList[4].Data, 3);
			Assert.AreEqual(v.TrackingList[5].Data, 5);
			Assert.AreEqual(v.TrackingList[6].Data, 1);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestBreadthFirstTraversalNullVertex()
		{
			Graph<int> g = new Graph<int>(true);
			g.BreadthFirstTraversal(new TrackingVisitor<Vertex<int>>(), null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestBreadthFirstTraversalNullVisitor()
		{
			Graph<int> g = new Graph<int>(true);
			g.BreadthFirstTraversal(null, new Vertex<int>(4));
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestDepthFirstTraversalNullVertex()
		{
			Graph<int> g = new Graph<int>(true);
			g.DepthFirstTraversal(new PreOrderVisitor<Vertex<int>>(new TrackingVisitor<Vertex<int>>()), null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestDepthFirstTraversalNullVisitor()
		{
			Graph<int> g = new Graph<int>(true);
			g.DepthFirstTraversal(null, new Vertex<int>(4));
		}

		[Test]
		public void TestBreadthFirstTraversalDirected()
		{
			Graph<int> g = new Graph<int>(true);
			Vertex<int> v1 = new Vertex<int>(1);
			Vertex<int> v2 = new Vertex<int>(2);
			Vertex<int> v3 = new Vertex<int>(3);
			Vertex<int> v4 = new Vertex<int>(4);
			Vertex<int> v5 = new Vertex<int>(5);
			Vertex<int> v6 = new Vertex<int>(6);
			Vertex<int> v7 = new Vertex<int>(7);

			g.AddVertex(v1);
			g.AddVertex(v2);
			g.AddVertex(v3);
			g.AddVertex(v4);
			g.AddVertex(v5);
			g.AddVertex(v6);
			g.AddVertex(v7);

			g.AddEdge(v1, v2);
			g.AddEdge(v1, v3);
			g.AddEdge(v1, v5);
			g.AddEdge(v5, v3);

			g.AddEdge(v3, v6);
			g.AddEdge(v3, v4);
			g.AddEdge(v2, v7);

			TrackingVisitor<Vertex<int>> v = new TrackingVisitor<Vertex<int>>();

			g.BreadthFirstTraversal(v, v1);

			Assert.AreEqual(v.TrackingList.Count, g.VertexCount);

			Assert.AreEqual(v.TrackingList[0].Data, 1);
			Assert.AreEqual(v.TrackingList[1].Data, 2);
			Assert.AreEqual(v.TrackingList[2].Data, 3);
			Assert.AreEqual(v.TrackingList[3].Data, 5);
			Assert.AreEqual(v.TrackingList[4].Data, 7);
			Assert.AreEqual(v.TrackingList[5].Data, 6);
			Assert.AreEqual(v.TrackingList[6].Data, 4);
		}

		[Test]
		public void TestBreadthFirstTraversalUndirected()
		{
			Graph<int> g = new Graph<int>(false);
			Vertex<int> v1 = new Vertex<int>(1);
			Vertex<int> v2 = new Vertex<int>(2);
			Vertex<int> v3 = new Vertex<int>(3);
			Vertex<int> v4 = new Vertex<int>(4);
			Vertex<int> v5 = new Vertex<int>(5);
			Vertex<int> v6 = new Vertex<int>(6);
			Vertex<int> v7 = new Vertex<int>(7);

			g.AddVertex(v1);
			g.AddVertex(v2);
			g.AddVertex(v3);
			g.AddVertex(v4);
			g.AddVertex(v5);
			g.AddVertex(v6);
			g.AddVertex(v7);

			g.AddEdge(v1, v2);
			g.AddEdge(v1, v3);
			g.AddEdge(v1, v5);
			g.AddEdge(v5, v3);

			g.AddEdge(v3, v6);
			g.AddEdge(v3, v4);
			g.AddEdge(v2, v7);

			TrackingVisitor<Vertex<int>> v = new TrackingVisitor<Vertex<int>>();

			g.BreadthFirstTraversal(v, v1);

			Assert.AreEqual(v.TrackingList.Count, g.VertexCount);

			Assert.AreEqual(v.TrackingList[0].Data, 1);
			Assert.AreEqual(v.TrackingList[1].Data, 2);
			Assert.AreEqual(v.TrackingList[2].Data, 3);
			Assert.AreEqual(v.TrackingList[3].Data, 5);
			Assert.AreEqual(v.TrackingList[4].Data, 7);
			Assert.AreEqual(v.TrackingList[5].Data, 6);
			Assert.AreEqual(v.TrackingList[6].Data, 4);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestInvalidEdgeUndirected()
		{
			Graph<int> g = GetTestUndirectedGraph();

			IEnumerator<Vertex<int>> vertices = g.Vertices;

			vertices.MoveNext();
			Vertex<int> v1 = vertices.Current;

			vertices.MoveNext();
			Vertex<int> v2 = vertices.Current;

			Edge<int> e = new Edge<int>(v1, v2, true);
			g.AddEdge(e);
		}

		[Test]
		public void TestClear()
		{
			Graph<int> graph = GetTestUndirectedGraph();
			graph.Clear();

			Assert.AreEqual(graph.VertexCount, 0);
			Assert.AreEqual(graph.VertexCount, 0);
			Assert.AreEqual(graph.EdgeCount, 0);
		}

		[Test]
		public void TestUndirectedAddVertex()
		{
			Graph<int> g = new Graph<int>(false);
			List<Vertex<int>> vertices = new List<Vertex<int>>();

			for (int i = 0; i < 20; i++)
			{
				Vertex<int> vertex = new Vertex<int>(i);
				g.AddVertex(vertex);

				Assert.AreEqual(g.VertexCount, i + 1);
				Assert.AreEqual(g.ContainsVertex(i), true);
				Assert.AreEqual(g.ContainsVertex(vertex), true);
				Assert.AreEqual(g.EdgeCount, 0);
			}
		}

		[Test]
		public void TestDirectedAddVertex()
		{
			Graph<int> g = new Graph<int>(true);
			List<Vertex<int>> vertices = new List<Vertex<int>>();

			for (int i = 0; i < 20; i++)
			{
				Vertex<int> vertex = new Vertex<int>(i);
				g.AddVertex(vertex);

				Assert.AreEqual(g.VertexCount, i + 1);
				Assert.AreEqual(g.ContainsVertex(i), true);
				Assert.AreEqual(g.ContainsVertex(vertex), true);
				Assert.AreEqual(g.EdgeCount, 0);
			}
		}

		[Test]
		public void TestUndirectedAddVertexValue()
		{
			Graph<int> g = new Graph<int>(false);
			List<Vertex<int>> vertices = new List<Vertex<int>>();

			for (int i = 0; i < 20; i++)
			{				
				g.AddVertex(i);

				Assert.AreEqual(g.VertexCount, i + 1);
				Assert.AreEqual(g.ContainsVertex(i), true);
				Assert.AreEqual(g.EdgeCount, 0);
			}
		}

		[Test]
		public void TestDirectedAddVertexValue()
		{
			Graph<int> g = new Graph<int>(true);
			List<Vertex<int>> vertices = new List<Vertex<int>>();

			for (int i = 0; i < 20; i++)
			{
				g.AddVertex(i);

				Assert.AreEqual(g.VertexCount, i + 1);
				Assert.AreEqual(g.ContainsVertex(i), true);
				Assert.AreEqual(g.EdgeCount, 0);
			}
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestUndirectedAddDuplicateVertex()
		{
			Vertex<int> v = new Vertex<int>(5);
			Graph<int> g = new Graph<int>(false);

			g.AddVertex(v);
			g.AddVertex(v);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestDirectedAddDuplicateVertex()
		{
			Vertex<int> v = new Vertex<int>(5);
			Graph<int> g = new Graph<int>(true);

			g.AddVertex(v);
			g.AddVertex(v);
		}

		[Test]
		public void TestContainsVertex()
		{
			Graph<int> g = new Graph<int>(false);
			List<Vertex<int>> vertices = new List<Vertex<int>>();

			for (int i = 0; i < 20; i++)
			{
				Vertex<int> vertex = new Vertex<int>(i);
				g.AddVertex(vertex);

				Assert.AreEqual(g.ContainsVertex(i), true);
				Assert.AreEqual(g.ContainsVertex(vertex), true);
			}

			Assert.AreEqual(g.ContainsVertex(new Vertex<int>(3)), false);
			Assert.AreEqual(g.ContainsVertex(21), false);
		}

		[Test]
		public void TestDirectedGetEdge()
		{
			Graph<int> g = new Graph<int>(true);
			Vertex<int> v1 = new Vertex<int>(1);
			Vertex<int> v2 = new Vertex<int>(2);
			Vertex<int> v3 = new Vertex<int>(3);

			g.AddVertex(v1);
			g.AddVertex(v2);
			g.AddVertex(v3);

			g.AddEdge(v1, v2);
			g.AddEdge(v3, v2);
			g.AddEdge(v1, v3);

			Edge<int> edge = g.GetEdge(v1, v2);

			Assert.AreNotEqual(edge, null);
			Assert.AreEqual(edge.FromVertex, v1);
			Assert.AreEqual(edge.ToVertex, v2);

			edge = g.GetEdge(v3, v2);

			Assert.AreNotEqual(edge, null);
			Assert.AreEqual(edge.FromVertex, v3);
			Assert.AreEqual(edge.ToVertex, v2);

			edge = g.GetEdge(v1, v3);

			Assert.AreNotEqual(edge, null);
			Assert.AreEqual(edge.FromVertex, v1);
			Assert.AreEqual(edge.ToVertex, v3);

			edge = g.GetEdge(v2, v1);
			Assert.AreEqual(edge, null);

			edge = g.GetEdge(v2, v3);
			Assert.AreEqual(edge, null);

			edge = g.GetEdge(v3, v1);
			Assert.AreEqual(edge, null);
		}

		[Test]
		public void TestUndirectedGetEdge()
		{
			Graph<int> g = new Graph<int>(false);
			Vertex<int> v1 = new Vertex<int>(1);
			Vertex<int> v2 = new Vertex<int>(2);
			Vertex<int> v3 = new Vertex<int>(3);

			g.AddVertex(v1);
			g.AddVertex(v2);
			g.AddVertex(v3);

			g.AddEdge(v1, v2);
			g.AddEdge(v3, v2);
			g.AddEdge(v1, v3);

			Edge<int> edge = g.GetEdge(v1, v2);

			Assert.AreNotEqual(edge, null);
			Assert.AreEqual(edge.FromVertex, v1);
			Assert.AreEqual(edge.ToVertex, v2);

			edge = g.GetEdge(v3, v2);

			Assert.AreNotEqual(edge, null);
			Assert.AreEqual(edge.FromVertex, v3);
			Assert.AreEqual(edge.ToVertex, v2);

			edge = g.GetEdge(v1, v3);

			Assert.AreNotEqual(edge, null);
			Assert.AreEqual(edge.FromVertex, v1);
			Assert.AreEqual(edge.ToVertex, v3);

			edge = g.GetEdge(v2, v1);

			Assert.AreNotEqual(edge, null);
			Assert.AreEqual(edge.FromVertex, v1);
			Assert.AreEqual(edge.ToVertex, v2);

			edge = g.GetEdge(v2, v3);

			Assert.AreNotEqual(edge, null);
			Assert.AreEqual(edge.FromVertex, v3);
			Assert.AreEqual(edge.ToVertex, v2);

			edge = g.GetEdge(v3, v1);

			Assert.AreNotEqual(edge, null);
			Assert.AreEqual(edge.FromVertex, v1);
			Assert.AreEqual(edge.ToVertex, v3);
		}

		[Test]
		public void TestUndirectedRemoveVertex()
		{
			Graph<int> g = new Graph<int>(false);
			Vertex<int> v1 = new Vertex<int>(1);
			Vertex<int> v2 = new Vertex<int>(2);
			Vertex<int> v3 = new Vertex<int>(3);
            Vertex<int> v4 = new Vertex<int>(4);

			g.AddVertex(v1);
			g.AddVertex(v2);
			g.AddVertex(v3);

			g.AddEdge(v1, v2);
			g.AddEdge(v3, v2);
			g.AddEdge(v1, v3);

			Assert.AreEqual(g.EdgeCount, 3);
			Assert.AreEqual(g.VertexCount, 3);

			Assert.AreEqual(g.RemoveVertex(v1), true);

			Assert.AreEqual(g.EdgeCount, 1);
			Assert.AreEqual(g.VertexCount, 2);

            Assert.AreEqual(g.RemoveVertex(v4), false);

            Assert.AreEqual(g.EdgeCount, 1);
            Assert.AreEqual(g.VertexCount, 2);

		}

		[Test]
		public void TestDirectedRemoveVertex()
		{
			Graph<int> g = new Graph<int>(true);
			Vertex<int> v1 = new Vertex<int>(1);
			Vertex<int> v2 = new Vertex<int>(2);
			Vertex<int> v3 = new Vertex<int>(3);
            Vertex<int> v4 = new Vertex<int>(4);

			g.AddVertex(v1);
			g.AddVertex(v2);
			g.AddVertex(v3);

			g.AddEdge(v1, v2);
			g.AddEdge(v3, v2);
			g.AddEdge(v1, v3);

			Assert.AreEqual(g.EdgeCount, 3);
			Assert.AreEqual(g.VertexCount, 3);

			Assert.AreEqual(g.RemoveVertex(v1), true);

			Assert.AreEqual(g.EdgeCount, 1);
			Assert.AreEqual(g.VertexCount, 2);

            Assert.AreEqual(g.RemoveVertex(v4), false);

            Assert.AreEqual(g.EdgeCount, 1);
            Assert.AreEqual(g.VertexCount, 2);
		}

        // IsStronglyConnected
        // IsWeaklyConnected

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestDirectedStronglyConnected()
        {
            Graph<int> g = new Graph<int>(true);
            bool val = g.IsStronglyConnected;
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestEmptyUndirectedStronglyConnected()
        {
            Graph<int> g = new Graph<int>(false);
            bool val = g.IsStronglyConnected;
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestInvalidDirectedWeaklyConnected()
        {
            Graph<int> g = new Graph<int>(true);
            bool val = g.IsWeaklyConnected;
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestEmptyUndirectedWeaklyConnected()
        {
            Graph<int> g = new Graph<int>(false);
            bool val = g.IsWeaklyConnected;
        }

        [Test]
        public void TestUndirectedStronglyConnected()
        {
            Graph<int> g = new Graph<int>(false);
            Vertex<int> v1 = new Vertex<int>(1);
            Vertex<int> v2 = new Vertex<int>(2);
            Vertex<int> v3 = new Vertex<int>(3);
            Vertex<int> v4 = new Vertex<int>(4);

            g.AddVertex(v1);
            g.AddVertex(v2);
            g.AddVertex(v3);
            g.AddVertex(v4);

            g.AddEdge(v1, v2);
            g.AddEdge(v3, v2);
            g.AddEdge(v1, v3);

            Assert.AreEqual(g.IsStronglyConnected, false);

            g.AddEdge(v2, v4);

            Assert.AreEqual(g.IsStronglyConnected, true);

            g.RemoveEdge(v2, v3);

            Assert.AreEqual(g.IsStronglyConnected, true);

            g.RemoveEdge(v1, v3);

            Assert.AreEqual(g.IsStronglyConnected, false);
        }

        [Test]
        public void TestUndirectedWeaklyConnected()
        {
            Graph<int> g = new Graph<int>(false);
            Vertex<int> v1 = new Vertex<int>(1);
            Vertex<int> v2 = new Vertex<int>(2);
            Vertex<int> v3 = new Vertex<int>(3);
            Vertex<int> v4 = new Vertex<int>(4);

            g.AddVertex(v1);
            g.AddVertex(v2);
            g.AddVertex(v3);
            g.AddVertex(v4);

            g.AddEdge(v1, v2);
            g.AddEdge(v3, v2);
            g.AddEdge(v1, v3);

            Assert.AreEqual(g.IsWeaklyConnected, false);

            g.AddEdge(v2, v4);

            Assert.AreEqual(g.IsWeaklyConnected, true);

            g.RemoveEdge(v2, v3);

            Assert.AreEqual(g.IsWeaklyConnected, true);

            g.RemoveEdge(v1, v3);

            Assert.AreEqual(g.IsWeaklyConnected, false);
        }

        [Test]
        public void TestDirectedWeaklyConnected()
        {
            Graph<int> g = new Graph<int>(true);
            Vertex<int> v1 = new Vertex<int>(1);
            Vertex<int> v2 = new Vertex<int>(2);
            Vertex<int> v3 = new Vertex<int>(3);
            Vertex<int> v4 = new Vertex<int>(4);

            g.AddVertex(v1);
            g.AddVertex(v2);
            g.AddVertex(v3);
            g.AddVertex(v4);

            g.AddEdge(v1, v2);
            g.AddEdge(v3, v2);
            g.AddEdge(v1, v3);

            Assert.AreEqual(g.IsWeaklyConnected, false);

            g.AddEdge(v2, v4);

            Assert.AreEqual(g.IsWeaklyConnected, true);

            g.RemoveEdge(v2, v3);

            Assert.AreEqual(g.IsWeaklyConnected, true);

            g.RemoveEdge(v1, v3);

            Assert.AreEqual(g.IsWeaklyConnected, false);
        }

		[Test]
		public void TestUndirectedRemoveVertexValue()
		{
			Graph<int> g = new Graph<int>(false);
			Vertex<int> v1 = new Vertex<int>(1);
			Vertex<int> v2 = new Vertex<int>(2);
			Vertex<int> v3 = new Vertex<int>(3);

			g.AddVertex(v1);
			g.AddVertex(v2);
			g.AddVertex(v3);

			g.AddEdge(v1, v2);
			g.AddEdge(v3, v2);
			g.AddEdge(v1, v3);

			Assert.AreEqual(g.EdgeCount, 3);
			Assert.AreEqual(g.VertexCount, 3);

			Assert.AreEqual(g.RemoveVertex(1), true);

			Assert.AreEqual(g.EdgeCount, 1);
			Assert.AreEqual(g.VertexCount, 2);

			Assert.AreEqual(g.RemoveVertex(4), false);
			Assert.AreEqual(g.EdgeCount, 1);
			Assert.AreEqual(g.VertexCount, 2);
		}

		[Test]
		public void TestDirectedRemoveVertexValue()
		{
			Graph<int> g = new Graph<int>(true);
			Vertex<int> v1 = new Vertex<int>(1);
			Vertex<int> v2 = new Vertex<int>(2);
			Vertex<int> v3 = new Vertex<int>(3);

			g.AddVertex(v1);
			g.AddVertex(v2);
			g.AddVertex(v3);

			g.AddEdge(v1, v2);
			g.AddEdge(v3, v2);
			g.AddEdge(v1, v3);

			Assert.AreEqual(g.EdgeCount, 3);
			Assert.AreEqual(g.VertexCount, 3);

			Assert.AreEqual(g.RemoveVertex(1), true);

			Assert.AreEqual(g.EdgeCount, 1);
			Assert.AreEqual(g.VertexCount, 2);

			Assert.AreEqual(g.RemoveVertex(4), false);
			Assert.AreEqual(g.EdgeCount, 1);
			Assert.AreEqual(g.VertexCount, 2);
		}

        [Test]
        public void TestRemoveEdgeOtherVertex()
        {
            Graph<int> g = new Graph<int>(true);
            Vertex<int> v1 = new Vertex<int>(1);
            Vertex<int> v2 = new Vertex<int>(2);
            Vertex<int> v3 = new Vertex<int>(3);

            g.AddVertex(v1);
            g.AddVertex(v2);
            g.AddVertex(v3);

            g.AddEdge(v1, v2);
            g.AddEdge(v3, v2);
            g.AddEdge(v1, v3);

            Assert.AreEqual(g.EdgeCount, 3);
            Assert.AreEqual(g.VertexCount, 3);

            Assert.AreEqual(g.RemoveEdge(v1, v2), true);
            Assert.AreEqual(g.EdgeCount, 2);
            Assert.AreEqual(v1.HasEmanatingEdgeTo(v2), false);
            Assert.AreEqual(v3.HasEmanatingEdgeTo(v2), true);

            Assert.AreEqual(g.RemoveEdge(v3, v2), true);
            Assert.AreEqual(g.EdgeCount, 1);

            Assert.AreEqual(v1.HasEmanatingEdgeTo(v2), false);
            Assert.AreEqual(v3.HasEmanatingEdgeTo(v2), false);
        }

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestRemoveNullEdge()
		{
			Graph<int> g = new Graph<int>(true);
			g.RemoveEdge(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestRemoveNullVertex()
		{
			Graph<int> g = new Graph<int>(true);
			g.RemoveVertex(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestAddEdgeNullVertex1()
		{
			Graph<int> g = new Graph<int>(true);
			g.AddEdge(null, new Vertex<int>(3));
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestAddEdgeNullVertex2()
		{
			Graph<int> g = new Graph<int>(true);
			g.AddEdge(new Vertex<int>(3), null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestRemoveEdgeNullVertex1()
		{
			Graph<int> g = new Graph<int>(true);
			g.RemoveEdge(new Vertex<int>(3), null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestRemoveEdgeNullVertex2()
		{
			Graph<int> g = new Graph<int>(true);
			g.RemoveEdge(null, new Vertex<int>(3));
		}

		[Test]
		public void TestDirectedRemoveEdgeNotInGraph()
		{
			Graph<int> g = new Graph<int>(true);
			Vertex<int> v1 = new Vertex<int>(1);
			Vertex<int> v2 = new Vertex<int>(2);
			Vertex<int> v3 = new Vertex<int>(3);

			g.AddVertex(v1);
			g.AddVertex(v2);
			g.AddVertex(v3);

			g.AddEdge(v1, v2);
			g.AddEdge(v3, v2);
			g.AddEdge(v1, v3);

			Assert.AreEqual(g.RemoveEdge(new Edge<int>(v2, v3, true)), false);
		}

		[Test]
		public void TestUndirectedRemoveEdgeNotInGraph()
		{
			Graph<int> g = new Graph<int>(true);
			Vertex<int> v1 = new Vertex<int>(1);
			Vertex<int> v2 = new Vertex<int>(2);
			Vertex<int> v3 = new Vertex<int>(3);

			g.AddVertex(v1);
			g.AddVertex(v2);
			g.AddVertex(v3);

			g.AddEdge(v1, v2);
			g.AddEdge(v3, v2);
			g.AddEdge(v1, v3);

			Assert.AreEqual(g.RemoveEdge(new Edge<int>(v2, v3, false)), false);
		}

		[Test]
		public void TestDirectedRemoveEdgeFromVerticesNotInGraph()
		{
			Graph<int> g = new Graph<int>(true);
			Vertex<int> v1 = new Vertex<int>(1);
			Vertex<int> v2 = new Vertex<int>(2);
			Vertex<int> v3 = new Vertex<int>(3);

			g.AddVertex(v1);
			g.AddVertex(v2);
			g.AddVertex(v3);

			g.AddEdge(v1, v2);
			g.AddEdge(v3, v2);
			g.AddEdge(v1, v3);

			Assert.AreEqual(g.RemoveEdge(v2, v3), false);
		}

		[Test]
		public void TestUndirectedRemoveEdgeFromVerticesNotInGraph()
		{
			Graph<int> g = new Graph<int>(false);
			Vertex<int> v1 = new Vertex<int>(1);
			Vertex<int> v2 = new Vertex<int>(2);
			Vertex<int> v3 = new Vertex<int>(3);

			g.AddVertex(v1);
			g.AddVertex(v2);
			g.AddVertex(v3);

			g.AddEdge(v1, v2);
			g.AddEdge(v3, v2);

			Assert.AreEqual(g.RemoveEdge(v2, v3), true);
			Assert.AreEqual(g.RemoveEdge(v1, v3), false);
		}

        [Test]
        public void TestGetPartnerVertex()
        {
            Graph<int> g = new Graph<int>(false);
            Vertex<int> v1 = new Vertex<int>(1);
            Vertex<int> v2 = new Vertex<int>(2);
            Vertex<int> v3 = new Vertex<int>(3);

            g.AddVertex(v1);
            g.AddVertex(v2);
            g.AddVertex(v3);

            Edge<int> v1v2 =  g.AddEdge(v1, v2);
            Edge<int> v3v2 = g.AddEdge(v3, v2);

            Assert.AreEqual(v1v2.GetPartnerVertex(v1), v2);
            Assert.AreEqual(v1v2.GetPartnerVertex(v2), v1);

            Assert.AreEqual(v3v2.GetPartnerVertex(v2), v3);
            Assert.AreEqual(v3v2.GetPartnerVertex(v3), v2);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidGetPartnerVertex()
        {
            Graph<int> g = new Graph<int>(false);
            Vertex<int> v1 = new Vertex<int>(1);
            Vertex<int> v2 = new Vertex<int>(2);
            Vertex<int> v3 = new Vertex<int>(3);

            g.AddVertex(v1);
            g.AddVertex(v2);
            g.AddVertex(v3);

            Edge<int> v1v2 = g.AddEdge(v1, v2);
            Edge<int> v3v2 = g.AddEdge(v3, v2);

            v1v2.GetPartnerVertex(v3);
        }

		[Test]
		public void TestDirectedRemoveEdgeVertices()
		{
			Graph<int> g = new Graph<int>(true);
			Vertex<int> v1 = new Vertex<int>(1);
			Vertex<int> v2 = new Vertex<int>(2);
			Vertex<int> v3 = new Vertex<int>(3);

			g.AddVertex(v1);
			g.AddVertex(v2);
			g.AddVertex(v3);

			g.AddEdge(v1, v2);
			g.AddEdge(v3, v2);
			g.AddEdge(v1, v3);

			Assert.AreEqual(g.EdgeCount, 3);
			Assert.AreEqual(g.VertexCount, 3);

			Assert.AreEqual(g.RemoveEdge(v1, v2), true);

			Assert.AreEqual(g.EdgeCount, 2);
			Assert.AreEqual(g.VertexCount, 3);

			Assert.AreEqual(g.RemoveEdge(v1, v3), true);

			Assert.AreEqual(g.EdgeCount, 1);
			Assert.AreEqual(g.VertexCount, 3);
		}

		[Test]
		public void TestUndirectedRemoveEdgeVertices()
		{
			Graph<int> g = new Graph<int>(false);
			Vertex<int> v1 = new Vertex<int>(1);
			Vertex<int> v2 = new Vertex<int>(2);
			Vertex<int> v3 = new Vertex<int>(3);

			g.AddVertex(v1);
			g.AddVertex(v2);
			g.AddVertex(v3);

			g.AddEdge(v1, v2);
			g.AddEdge(v3, v2);
			g.AddEdge(v1, v3);

			Assert.AreEqual(g.EdgeCount, 3);
			Assert.AreEqual(g.VertexCount, 3);

			Assert.AreEqual(g.RemoveEdge(v1, v2), true);

			Assert.AreEqual(g.EdgeCount, 2);
			Assert.AreEqual(g.VertexCount, 3);

			Assert.AreEqual(g.RemoveEdge(v3, v1), true);

			Assert.AreEqual(g.EdgeCount, 1);
			Assert.AreEqual(g.VertexCount, 3);
		}


		[Test]
		public void TestUndirectedCopyTo()
		{
			TestCopyTo(GetTestUndirectedGraph());
		}

		[Test]
		public void TestDirectedCopyTo()
		{
			TestCopyTo(GetTestDirectedGraph());
		}

		private void TestCopyTo(Graph<int> graph)
		{
			int[] array = new int[20];
			graph.CopyTo(array, 0);

			List<int> l = new List<int>(array);

			for (int i = 0; i < 19; i++)
			{
				Assert.AreEqual(l.Contains(i), true);
			}
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestDirectedNullCopyTo()
		{
			Graph<int> graph = new Graph<int>(true);
			graph.CopyTo(null, 0);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestUndirectedNullCopyTo()
		{
			Graph<int> graph = new Graph<int>(false);
			graph.CopyTo(null, 0);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestDirectedInvalidCopyTo1()
		{
			Graph<int> graph = GetTestDirectedGraph();

			int[] array = new int[20];
			graph.CopyTo(array, 1);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestUndirectedInvalidCopyTo1()
		{
			Graph<int> graph = GetTestUndirectedGraph();

			int[] array = new int[20];
			graph.CopyTo(array, 1);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestUndirectedInvalidCopyTo2()
		{
			Graph<int> graph = GetTestUndirectedGraph();

			int[] array = new int[19];
			graph.CopyTo(array, 0);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestDirectedInvalidCopyTo2()
		{
			Graph<int> graph = GetTestDirectedGraph();
			int[] array = new int[19];
			graph.CopyTo(array, 0);
		}

		[Test]
		public void TestUndirectedEnumerator()
		{
			TestEnumerator(GetTestUndirectedGraph());
		}

		[Test]
		public void TestDirectedEnumerator()
		{
			TestEnumerator(GetTestDirectedGraph());
		}

		[Test]
		public void TestInterfaceAdd()
		{
			Graph<int> g = new Graph<int>(true);
			((IVisitableCollection<int>)g).Add(4);
			Assert.AreEqual(g.VertexCount, 1);
		}

		[Test]
		public void TestInterfaceRemove()
		{
			Graph<int> g = new Graph<int>(true);
			((IVisitableCollection<int>)g).Add(4);
			Assert.AreEqual(g.VertexCount, 1);

			Assert.AreEqual(((IVisitableCollection<int>)g).Remove(4), true);
			Assert.AreEqual(g.VertexCount, 0);
			Assert.AreEqual(((IVisitableCollection<int>)g).Remove(3), false);
			Assert.AreEqual(g.VertexCount, 0);
		}

		[Test]
		public void TestInterfaceContains()
		{
			Graph<int> g = new Graph<int>(true);
			((IVisitableCollection<int>)g).Add(4);
			Assert.AreEqual(g.VertexCount, 1);

			Assert.AreEqual(((IVisitableCollection<int>)g).Contains(4), true);
			Assert.AreEqual(((IVisitableCollection<int>)g).Contains(3), false);
		}

		[Test]
		public void TestIsEmpty()
		{
			Graph<int> g = new Graph<int>(true);
			Assert.AreEqual(g.IsEmpty, true);

			g.AddVertex(5);
			Assert.AreEqual(g.IsEmpty, false);

			g.AddVertex(3);
			Assert.AreEqual(g.IsEmpty, false);

			g.Clear();
			Assert.AreEqual(g.IsEmpty, true);
		}

		[Test]
		public void TestIsFixedSize()
		{
			Graph<int> g = new Graph<int>(true);
			Assert.AreEqual(g.IsFixedSize, false);

			g = GetTestDirectedGraph();
			Assert.AreEqual(g.IsFixedSize, false);

			g = GetTestUndirectedGraph();
			Assert.AreEqual(g.IsFixedSize, false);
		}
		
		[Test]
		public void TestIsFull()
		{
			Graph<int> g = new Graph<int>(true);
			Assert.AreEqual(g.IsFull, false);

			g = GetTestDirectedGraph();
			Assert.AreEqual(g.IsFull, false);

			g = GetTestUndirectedGraph();
			Assert.AreEqual(g.IsFull, false);
		}

		[Test]
		public void TestIsReadOnly()
		{
			Graph<int> g = new Graph<int>(true);
			Assert.AreEqual(g.IsReadOnly, false);

			g = GetTestDirectedGraph();
			Assert.AreEqual(g.IsReadOnly, false);

			g = GetTestUndirectedGraph();
			Assert.AreEqual(g.IsReadOnly, false);
		}

		[Test]
		public void TestInterfaceEnumerator()
		{
			Graph<int> g = GetTestUndirectedGraph();
			IEnumerator enumerator = ((IEnumerable) g).GetEnumerator();

			List<int> l = new List<int>();

			while (enumerator.MoveNext())
			{
				l.Add((int) enumerator.Current);
			}

			Assert.AreEqual(l.Count, 20);

			for (int i = 0; i < l.Count; i++)
			{
				Assert.AreEqual(l.Contains(i), true);
			}
		}

		private void TestEnumerator(Graph<int> g)
		{
			IEnumerator<int> enumerator = g.GetEnumerator();

			List<int> l = new List<int>();

			while (enumerator.MoveNext())
			{
				l.Add(enumerator.Current);
			}

			Assert.AreEqual(l.Count, 20);

			for (int i = 0; i < l.Count; i++)
			{
				Assert.AreEqual(l.Contains(i), true);
			}
		}

		[Test]
		public void TestCompareTo()
		{
			Graph<int> g1 = GetTestDirectedGraph();
			Graph<int> g2 = GetTestUndirectedGraph();

			Graph<int> g3 = new Graph<int>(false);

			g3.AddVertex(4);

			Assert.AreEqual(g1.CompareTo(g2), 0);
			Assert.AreEqual(g1.CompareTo(g1), 0);

			Assert.AreEqual(g1.CompareTo(g3), 1);
			Assert.AreEqual(g3.CompareTo(g1), -1);

			Assert.AreEqual(g1.CompareTo(new object()), -1);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestInvalidCompareTo()
		{
			Graph<string> g = new Graph<string>(true);
			g.CompareTo(null);
		}

		[Test]
		public void TestDirectedContainsEdge()
		{
			Graph<int> g = GetTestDirectedGraph();

			List<Edge<int>> edges = GetEdges(g);

			for (int i = 0; i < edges.Count; i++)
			{
				Assert.AreEqual(g.ContainsEdge(edges[i].FromVertex, edges[i].ToVertex), true);
				Assert.AreEqual(g.ContainsEdge(edges[i].FromVertex.Data, edges[i].ToVertex.Data), true);

				Assert.AreEqual(g.ContainsEdge(edges[i].ToVertex, edges[i].FromVertex), false);
				Assert.AreEqual(g.ContainsEdge(edges[i].ToVertex.Data, edges[i].FromVertex.Data), false);
			}

			Assert.AreEqual(g.ContainsEdge(100, 200), false);
			Assert.AreEqual(g.ContainsEdge(new Vertex<int>(100), new Vertex<int>(200)), false);
		}

		[Test]
		public void TestUndirectedContainsEdge()
		{
			Graph<int> g = GetTestUndirectedGraph();

			List<Edge<int>> edges = GetEdges(g);

			for (int i = 0; i < edges.Count; i++)
			{
				Assert.AreEqual(g.ContainsEdge(edges[i].FromVertex, edges[i].ToVertex), true);
				Assert.AreEqual(g.ContainsEdge(edges[i].FromVertex.Data, edges[i].ToVertex.Data), true);

				Assert.AreEqual(g.ContainsEdge(edges[i].ToVertex, edges[i].FromVertex), true);
				Assert.AreEqual(g.ContainsEdge(edges[i].ToVertex.Data, edges[i].FromVertex.Data), true);
			}

			Assert.AreEqual(g.ContainsEdge(100, 200), false);
			Assert.AreEqual(g.ContainsEdge(new Vertex<int>(100), new Vertex<int>(200)), false);
		}

		private List<Vertex<int>> GetVertices(Graph<int> g)
		{
			List<Vertex<int>> vertices = new List<Vertex<int>>();

			using (IEnumerator<Vertex<int>> enumerator = g.Vertices)
			{
				while (enumerator.MoveNext())
				{
					vertices.Add(enumerator.Current);
				}
			}

			return vertices;
		}

		private List<Edge<int>> GetEdges(Graph<int> g)
		{
			List<Edge<int>> edges = new List<Edge<int>>();

			using (IEnumerator<Edge<int>> enumerator = g.Edges)
			{
				while (enumerator.MoveNext())
				{
					edges.Add(enumerator.Current);
				}
			}

			return edges;
		}
				
		[Test]
		public void TestEdgesUndirectedGraph() {
			Graph<int> g = new Graph<int>(false);

			Vertex<int>[] vertices = new Vertex<int>[20];

			for (int i = 0; i < 20; i++)
			{
				vertices[i] = new Vertex<int>(i);
				g.AddVertex(vertices[i]);
			}

			for (int i = 0; i < 17; i += 2)
			{
				g.AddEdge(vertices[i], vertices[i + 2]);
			}

			List<Edge<int>> edges = new List<Edge<int>>();

			using (IEnumerator<Edge<int>> enumerator = g.Edges)
			{
				while (enumerator.MoveNext())
				{
					edges.Add(enumerator.Current);
				}
			}

			Assert.AreEqual(edges.Count, 9);

			for (int i = 0; i < 17; i += 2)
			{
				bool found = false;

				for (int j = 0; j < edges.Count; j++)
				{
					if ((edges[j].FromVertex == vertices[i]) && (edges[j].ToVertex == vertices[i + 2]))
					{
						found = true;
						break;
					}
				}

				Assert.AreEqual(found, true);
			}
		}

		[Test]
		public void TestEdgesDirectedGraph()
		{
			Graph<int> g = new Graph<int>(true);

			Vertex<int>[] vertices = new Vertex<int>[20];

			for (int i = 0; i < 20; i++)
			{
				vertices[i] = new Vertex<int>(i);
				g.AddVertex(vertices[i]);
			}

			for (int i = 0; i < 17; i += 2)
			{
				g.AddEdge(vertices[i], vertices[i + 2]);
			}

			List<Edge<int>> edges = new List<Edge<int>>();

			using (IEnumerator<Edge<int>> enumerator = g.Edges)
			{
				while (enumerator.MoveNext())
				{
					edges.Add(enumerator.Current);
				}
			}

			Assert.AreEqual(edges.Count, 9);

			for (int i = 0; i < 17; i += 2)
			{
				bool found = false;

				for (int j = 0; j < edges.Count; j++)
				{
					if ((edges[j].FromVertex == vertices[i]) && (edges[j].ToVertex == vertices[i + 2]))
					{
						found = true;
						break;
					}
				}

				Assert.AreEqual(found, true);
			}
		}

		[Test]
		public void TestVerticesUndirectedGraph()
		{
			Graph<int> g = new Graph<int>(false);

			Vertex<int>[] vertices = new Vertex<int>[20];

			for (int i = 0; i < 20; i++)
			{
				vertices[i] = new Vertex<int>(i);
				g.AddVertex(vertices[i]);
			}

			List<Vertex<int>> enumeratedVertices = GetVertices(g);

			Assert.AreEqual(enumeratedVertices.Count, 20);

			for (int i = 0; i< enumeratedVertices.Count; i++) {
				Assert.AreEqual(enumeratedVertices.Contains(vertices[i]), true);
			}
		}

		[Test]
		public void TestVerticesDirectedGraph()
		{
			Graph<int> g = new Graph<int>(true);

			Vertex<int>[] vertices = new Vertex<int>[20];

			for (int i = 0; i < 20; i++)
			{
				vertices[i] = new Vertex<int>(i);
				g.AddVertex(vertices[i]);
			}

			List<Vertex<int>> enumeratedVertices = GetVertices(g);

			Assert.AreEqual(enumeratedVertices.Count, 20);

			for (int i = 0; i < enumeratedVertices.Count; i++)
			{
				Assert.AreEqual(enumeratedVertices.Contains(vertices[i]), true);
			}
		}

		[Test]
		public void TestDirectedVertexCount()
		{
			Graph<int> g = new Graph<int>(true);

			for (int i = 0; i < 20; i++)
			{
				Vertex<int> v = new Vertex<int>(i);
				g.AddVertex(v);

				Assert.AreEqual(g.VertexCount, i * 2 + 1);
				
				g.AddVertex(i);
				
				Assert.AreEqual(g.VertexCount, i * 2 + 2);
			}
		}

		[Test]
		public void TestUndirectedVertexCount()
		{
			Graph<int> g = new Graph<int>(false);

			for (int i = 0; i < 20; i++)
			{
				Vertex<int> v = new Vertex<int>(i);
				g.AddVertex(v);

				Assert.AreEqual(g.VertexCount, i * 2 + 1);

				g.AddVertex(i);

				Assert.AreEqual(g.VertexCount, i * 2 + 2);
			}
		}

		[Test]
		public void TestDirectedEdgeCount()
		{
			Graph<int> g = new Graph<int>(true);

			Vertex<int>[] vertices = new Vertex<int>[20];

			for (int i = 0; i < 20; i++)
			{
				vertices[i] = new Vertex<int>(i);
				g.AddVertex(vertices[i]);
			}

			int counter = 0;

			for (int i = 0; i < 17; i += 2)
			{
				g.AddEdge(vertices[i], vertices[i + 2]);
				counter ++;

				Assert.AreEqual(g.EdgeCount, counter);
			}
		}

		[Test]
		public void TestUndirectedEdgeCount()
		{
			Graph<int> g = new Graph<int>(false);

			Vertex<int>[] vertices = new Vertex<int>[20];

			for (int i = 0; i < 20; i++)
			{
				vertices[i] = new Vertex<int>(i);
				g.AddVertex(vertices[i]);
			}

			int counter = 0;

			for (int i = 0; i < 17; i += 2)
			{
				g.AddEdge(vertices[i], vertices[i + 2]);
				counter++;

				Assert.AreEqual(g.EdgeCount, counter);
			}
		}

		[Test]
		public void TestDirectedCount()
		{
			Graph<int> g = new Graph<int>(true);
			IVisitableCollection<int> c = g;

			for (int i = 0; i < 20; i++)
			{
				Vertex<int> v = new Vertex<int>(i);
				g.AddVertex(v);

				Assert.AreEqual(c.Count, i * 2 + 1);

				g.AddVertex(i);

				Assert.AreEqual(c.Count, i * 2 + 2);
			}
		}

		[Test]
		public void TestUndirectedCount()
		{
			Graph<int> g = new Graph<int>(false);
			IVisitableCollection<int> c = g;

			for (int i = 0; i < 20; i++)
			{
				Vertex<int> v = new Vertex<int>(i);
				g.AddVertex(v);

				Assert.AreEqual(c.Count, i * 2 + 1);

				g.AddVertex(i);

				Assert.AreEqual(c.Count, i * 2 + 2);
			}
		}

		[Test]
		public void TestIsDirected()
		{
			Graph<int> g = new Graph<int>(true);
			Assert.AreEqual(g.IsDirected, true);

			g = new Graph<int>(false);
			Assert.AreEqual(g.IsDirected, false);
		}

		[Test]
		public void TestAddEdgeUndirected()
		{
			Graph<int> g = new Graph<int>(false);

			Vertex<int>[] vertices = new Vertex<int>[20];

			for (int i = 0; i < 20; i++)
			{
				vertices[i] = new Vertex<int>(i);
				g.AddVertex(vertices[i]);
			}

			int counter = 0;

			for (int i = 0; i < 17; i += 2)
			{
				Edge<int> e = new Edge<int>(vertices[i], vertices[i + 2], false);
				g.AddEdge(e);

				counter++;

				Assert.AreEqual(g.EdgeCount, counter);
				Assert.AreEqual(g.ContainsEdge(e), true);
				Assert.AreEqual(vertices[i].HasEmanatingEdgeTo(vertices[i + 2]), true);
				Assert.AreEqual(vertices[i].HasIncidentEdgeWith(vertices[i + 2]), true);
				
			}
		}

		[Test]
		public void TestAddEdgeVerticesAndWeightDirected()
		{
			Graph<int> g = new Graph<int>(true);
			Vertex<int> v1 = new Vertex<int>(4);
			Vertex<int> v2 = new Vertex<int>(5);
			Vertex<int> v3 = new Vertex<int>(6);

			g.AddVertex(v1);
			g.AddVertex(v2);
			g.AddVertex(v3);

			g.AddEdge(v1, v2, 10);
			g.AddEdge(v1, v3, 12);
			g.AddEdge(v2, v3, 14);

			Assert.AreEqual(g.EdgeCount, 3);

			Assert.AreEqual(v1.HasEmanatingEdgeTo(v2), true);
			Assert.AreEqual(v1.HasEmanatingEdgeTo(v3), true);
			Assert.AreEqual(v2.HasEmanatingEdgeTo(v3), true);

			Assert.AreEqual(v2.HasEmanatingEdgeTo(v1), false);
			Assert.AreEqual(v3.HasEmanatingEdgeTo(v1), false);
			Assert.AreEqual(v3.HasEmanatingEdgeTo(v2), false);

			Assert.AreEqual(g.ContainsEdge(v1, v2), true);
			Assert.AreEqual(g.ContainsEdge(v1, v3), true);
			Assert.AreEqual(g.ContainsEdge(v2, v3), true);

			Assert.AreEqual(g.ContainsEdge(v2, v1), false);
			Assert.AreEqual(g.ContainsEdge(v3, v1), false);
			Assert.AreEqual(g.ContainsEdge(v3, v2), false);
		}

		[Test]
		public void TestAddEdgeVerticesAndWeightUndirected()
		{
			Graph<int> g = new Graph<int>(false);
			Vertex<int> v1 = new Vertex<int>(4);
			Vertex<int> v2 = new Vertex<int>(5);
			Vertex<int> v3 = new Vertex<int>(6);

			g.AddVertex(v1);
			g.AddVertex(v2);
			g.AddVertex(v3);

			g.AddEdge(v1, v2, 10);
			g.AddEdge(v1, v3, 12);
			g.AddEdge(v2, v3, 14);

			Assert.AreEqual(g.EdgeCount, 3);

			Assert.AreEqual(v1.HasEmanatingEdgeTo(v2), true);
			Assert.AreEqual(v1.HasEmanatingEdgeTo(v3), true);
			Assert.AreEqual(v2.HasEmanatingEdgeTo(v3), true);

			Assert.AreEqual(v2.HasEmanatingEdgeTo(v1), true);
			Assert.AreEqual(v3.HasEmanatingEdgeTo(v1), true);
			Assert.AreEqual(v3.HasEmanatingEdgeTo(v2), true);

			Assert.AreEqual(g.ContainsEdge(v1, v2), true);
			Assert.AreEqual(g.ContainsEdge(v1, v3), true);
			Assert.AreEqual(g.ContainsEdge(v2, v3), true);

			Assert.AreEqual(g.ContainsEdge(v2, v1), true);
			Assert.AreEqual(g.ContainsEdge(v3, v1), true);
			Assert.AreEqual(g.ContainsEdge(v3, v2), true);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestAddEdgeNull()
		{
			Graph<int> g = new Graph<int>(false);
			g.AddEdge(null);
		}


		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestDirectedAddEdgeVertexNotInGraph()
		{
			Graph<int> g = GetTestUndirectedGraph();

			Edge<int> e = new Edge<int>(new Vertex<int>(1), new Vertex<int>(1), false);
			g.AddEdge(e);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestUndirectedAddEdgeVertexNotInGraph()
		{
			Graph<int> g = GetTestDirectedGraph();

			Edge<int> e = new Edge<int>(new Vertex<int>(1), new Vertex<int>(1), true);
			g.AddEdge(e);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestAddDuplicateEdgeUndirected1()
		{
			Graph<int> g = new Graph<int>(false);

			Vertex<int>[] vertices = new Vertex<int>[20];

			for (int i = 0; i < 20; i++)
			{
				vertices[i] = new Vertex<int>(i);
				g.AddVertex(vertices[i]);
			}

			Edge<int> e = new Edge<int>(vertices[0], vertices[2], false);
			g.AddEdge(e);

			g.AddEdge(e);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestAddDuplicateEdgeUndirected2()
		{
			Graph<int> g = new Graph<int>(false);

			Vertex<int>[] vertices = new Vertex<int>[20];

			for (int i = 0; i < 20; i++)
			{
				vertices[i] = new Vertex<int>(i);
				g.AddVertex(vertices[i]);
			}

			Edge<int> e = new Edge<int>(vertices[0], vertices[2], false);
			g.AddEdge(e);

			g.AddEdge(new Edge<int>(vertices[2], vertices[0], false));
		}

		[Test]
		public void TestAccept()
		{
			Graph<int> g = new Graph<int>(false);

			Vertex<int>[] vertices = new Vertex<int>[20];

			for (int i = 0; i < 20; i++)
			{
				vertices[i] = new Vertex<int>(i);
				g.AddVertex(vertices[i]);
			}

			for (int i = 0; i < 17; i += 2)
			{
				Edge<int> e = new Edge<int>(vertices[i], vertices[i + 2], false);
				g.AddEdge(e);
			}

			TrackingVisitor<int> v = new TrackingVisitor<int>();

			g.Accept(v);

			Assert.AreEqual(v.TrackingList.Count, 20);

			for (int i = 0; i < 20; i++)
			{
				Assert.AreEqual(v.TrackingList.Contains(i), true);
			}
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestNullVisitor()
		{
			Graph<int> g = new Graph<int>(false);
			g.Accept(null);
		}

		// AddEdge
		// GetEdge
		// Remove
		// RemoveEdge
		// get_IsConnected
		// get_IsCyclic

		private Graph<int> GetTestUndirectedGraph()
		{
			Graph<int> g = new Graph<int>(false);

			Vertex<int>[] vertices = new Vertex<int>[20];

			for (int i = 0; i < 20; i++)
			{
				vertices[i] = new Vertex<int>(i);
				g.AddVertex(vertices[i]);
			}

			for (int i = 0; i < 17; i += 2)
			{
				g.AddEdge(vertices[i], vertices[i + 2]);				
			}

			return g;
		}

		[Test]
		public void TestUndirectedGetEmanatingEdgeTo()
		{
			Vertex<int> v1 = new Vertex<int>(3);
			Vertex<int> v2 = new Vertex<int>(5);
			Vertex<int> v3 = new Vertex<int>(8);

			Graph<int> g = new Graph<int>(false);
			g.AddVertex(v1);
			g.AddVertex(v2);

			g.AddEdge(v1, v2);

			Edge<int> edge = v1.GetEmanatingEdgeTo(v2);
			Assert.AreEqual(edge.FromVertex, v1);
			Assert.AreEqual(edge.ToVertex, v2);

			edge = v2.GetEmanatingEdgeTo(v1);
			Assert.AreEqual(edge.FromVertex, v1);
			Assert.AreEqual(edge.ToVertex, v2);

			Assert.AreEqual(v1.GetEmanatingEdgeTo(v3), null);
		}

		[Test]
		public void TestDirectedGetEmanatingEdgeTo()
		{
			Vertex<int> v1 = new Vertex<int>(3);
			Vertex<int> v2 = new Vertex<int>(5);
			Vertex<int> v3 = new Vertex<int>(8);

			Graph<int> g = new Graph<int>(true);
			g.AddVertex(v1);
			g.AddVertex(v2);

			g.AddEdge(v1, v2);

			Edge<int> edge = v1.GetEmanatingEdgeTo(v2);
			Assert.AreEqual(edge.FromVertex, v1);
			Assert.AreEqual(edge.ToVertex, v2);

			edge = v2.GetEmanatingEdgeTo(v1);
			Assert.AreEqual(edge, null);
			Assert.AreEqual(v1.GetEmanatingEdgeTo(v3), null);
		}

		[Test]
		public void TestUndirectedGetIncidentEdgeWith()
		{
			Vertex<int> v1 = new Vertex<int>(3);
			Vertex<int> v2 = new Vertex<int>(5);
			Vertex<int> v3 = new Vertex<int>(8);

			Graph<int> g = new Graph<int>(false);
			g.AddVertex(v1);
			g.AddVertex(v2);

			g.AddEdge(v1, v2);

			Edge<int> edge = v1.GetIncidentEdgeWith(v2);
			Assert.AreEqual(edge.FromVertex, v1);
			Assert.AreEqual(edge.ToVertex, v2);

			edge = v2.GetIncidentEdgeWith(v1);
			Assert.AreEqual(edge.FromVertex, v1);
			Assert.AreEqual(edge.ToVertex, v2);

			Assert.AreEqual(v1.GetIncidentEdgeWith(v3), null);
		}

		[Test]
		public void TestDirectedGetIncidentEdges()
		{
			Vertex<int> v1 = new Vertex<int>(3);
			Vertex<int> v2 = new Vertex<int>(5);
			Vertex<int> v3 = new Vertex<int>(8);

			Graph<int> g = new Graph<int>(true);
			g.AddVertex(v1);
			g.AddVertex(v2);
			g.AddVertex(v3);

			g.AddEdge(v1, v2);
			g.AddEdge(v3, v1);

			IEnumerator<Edge<int>> edges = v3.IncidentEdges;

			List<Edge<int>> edgeList = GetEdgeList(edges);

			Assert.AreEqual(edgeList.Count, 1);
			AssertContainsEdges(edgeList, true,
				v3.GetEmanatingEdgeTo(v1)
			);

			edges = v1.IncidentEdges;
			edgeList = GetEdgeList(edges);

			Assert.AreEqual(edgeList.Count, 2);
			AssertContainsEdges(edgeList, true,
				v1.GetEmanatingEdgeTo(v2),
				v3.GetEmanatingEdgeTo(v1)
			);
		}

		[Test]
		public void TestUndirectedGetIncidentEdges()
		{
			Vertex<int> v1 = new Vertex<int>(3);
			Vertex<int> v2 = new Vertex<int>(5);
			Vertex<int> v3 = new Vertex<int>(8);

			Graph<int> g = new Graph<int>(false);
			g.AddVertex(v1);
			g.AddVertex(v2);
			g.AddVertex(v3);

			g.AddEdge(v1, v2);
			g.AddEdge(v3, v1);

			IEnumerator<Edge<int>> edges = v3.IncidentEdges;

			List<Edge<int>> edgeList = GetEdgeList(edges);

			Assert.AreEqual(edgeList.Count, 1);
			AssertContainsEdges(edgeList, true,
				v3.GetEmanatingEdgeTo(v1)
			);

			edges = v1.IncidentEdges;
			edgeList = GetEdgeList(edges);

			Assert.AreEqual(edgeList.Count, 2);
			AssertContainsEdges(edgeList, true,
				v1.GetEmanatingEdgeTo(v2),
				v3.GetEmanatingEdgeTo(v1)
			);
		}

		[Test]
		public void TestUndirectedGetEmanatingEdges()
		{
			Vertex<int> v1 = new Vertex<int>(3);
			Vertex<int> v2 = new Vertex<int>(5);
			Vertex<int> v3 = new Vertex<int>(8);

			Graph<int> g = new Graph<int>(false);
			g.AddVertex(v1);
			g.AddVertex(v2);
			g.AddVertex(v3);

			g.AddEdge(v1, v2);
			g.AddEdge(v3, v1);

			IEnumerator<Edge<int>> edges = v1.EmanatingEdges;

			List<Edge<int>> edgeList = GetEdgeList(edges);

			Assert.AreEqual(edgeList.Count, 2);

			AssertContainsEdges(edgeList, true,
				v1.GetEmanatingEdgeTo(v2),
				v1.GetEmanatingEdgeTo(v3),
				v3.GetEmanatingEdgeTo(v1));
		}

		private List<Edge<int>> GetEdgeList(IEnumerator<Edge<int>> edges)
		{
			List<Edge<int>> edgeList = new List<Edge<int>>();

			while (edges.MoveNext())
			{
				edgeList.Add(edges.Current);
			}
			return edgeList;
		}

		private void AssertContainsEdges(List<Edge<int>> edgeList, bool containsValue, params Edge<int>[] edges)
		{
			for (int i = 0; i < edges.Length; i++)
			{
				Assert.AreEqual(edgeList.Contains(edges[i]), containsValue);
			}
		}

		[Test]
		public void TestDirectedGetEmanatingEdges()
		{
			Vertex<int> v1 = new Vertex<int>(3);
			Vertex<int> v2 = new Vertex<int>(5);
			Vertex<int> v3 = new Vertex<int>(8);

			Graph<int> g = new Graph<int>(true);
			g.AddVertex(v1);
			g.AddVertex(v2);
			g.AddVertex(v3);

			g.AddEdge(v1, v2);
			g.AddEdge(v3, v1);

			IEnumerator<Edge<int>> edges = v3.EmanatingEdges;

			List<Edge<int>> edgeList = GetEdgeList(edges);

			Assert.AreEqual(edgeList.Count, 1);
			AssertContainsEdges(edgeList, true,
				v3.GetEmanatingEdgeTo(v1)
			);

			edges = v1.EmanatingEdges;
			edgeList = GetEdgeList(edges);

			Assert.AreEqual(edgeList.Count, 1);
			AssertContainsEdges(edgeList, true,
				v1.GetEmanatingEdgeTo(v2)
			);
		}

		[Test]
		public void TestDirectedGetIncidentEdgeWith()
		{
			Vertex<int> v1 = new Vertex<int>(3);
			Vertex<int> v2 = new Vertex<int>(5);
			Vertex<int> v3 = new Vertex<int>(8);

			Graph<int> g = new Graph<int>(true);
			g.AddVertex(v1);
			g.AddVertex(v2);

			g.AddEdge(v1, v2);

			Edge<int> edge = v1.GetIncidentEdgeWith(v2);
			Assert.AreEqual(edge.FromVertex, v1);
			Assert.AreEqual(edge.ToVertex, v2);

			edge = v2.GetIncidentEdgeWith(v1);
			Assert.AreEqual(edge.FromVertex, v1);
			Assert.AreEqual(edge.ToVertex, v2);

			Assert.AreEqual(v1.GetIncidentEdgeWith(v3), null);
		}

		[Test]
		public void TestVertexSetData()
		{
			Vertex<int> v = new Vertex<int>(5);
			Assert.AreEqual(v.Data, 5);

			v.Data = 2;
			Assert.AreEqual(v.Data, 2);

			v.Data = 10;
			Assert.AreEqual(v.Data, 10);
		}

		private Graph<int> GetTestDirectedGraph()
		{
			Graph<int> g = new Graph<int>(true);

			Vertex<int>[] vertices = new Vertex<int>[20];

			for (int i = 0; i < 20; i++)
			{
				vertices[i] = new Vertex<int>(i);
				g.AddVertex(vertices[i]);
			}

			for (int i = 0; i < 17; i += 2)
			{
				g.AddEdge(vertices[i], vertices[i + 2]);
			}

			return g;
		}

	}
}
