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

using NGenerics.Algorithms.Graph;
using NGenerics.DataStructures;

using NUnit.Framework;

namespace NGenericsTests
{
	[TestFixture]
	public class DjikstraTest
	{
		[Test]
		public void TestDijkstraDirectedSimple1()
		{
			Graph<int> g = new Graph<int>(true);

			Vertex<int> a = new Vertex<int>(1);
			Vertex<int> b = new Vertex<int>(2);
			Vertex<int> c = new Vertex<int>(3);

			g.AddVertex(a);
			g.AddVertex(b);
			g.AddVertex(c);

			g.AddEdge(a, c, 5);
			g.AddEdge(a, b, 3);
			g.AddEdge(b, c, 4);
			g.AddEdge(c, a, 3);

			Graph<int> result = NGenerics.Algorithms.Graph.DijkstrasAlgorithm<int>.FindShortestPaths(g, a);

			Assert.AreEqual(result.EdgeCount, 2);

			List<Edge<int>> edges = new List<Edge<int>>();

			using (IEnumerator<Edge<int>> enumerator = result.Edges)
			{
				while (enumerator.MoveNext())
				{
					edges.Add(enumerator.Current);
				}
			}
						
			for (int i = 0; i < edges.Count; i++)
			{
				if ((edges[i].FromVertex.Data == 1) && (edges[i].ToVertex.Data == 2))
				{
					Assert.AreEqual(edges[i].Weight, 3);
					Assert.AreEqual(edges[i].FromVertex.Weight, 0);
					Assert.AreEqual(edges[i].ToVertex.Weight, 3);
				}
				else if ((edges[i].FromVertex.Data == 1) && (edges[i].ToVertex.Data == 3))
				{
					Assert.AreEqual(edges[i].Weight, 5);
					Assert.AreEqual(edges[i].FromVertex.Weight, 0);
					Assert.AreEqual(edges[i].ToVertex.Weight, 5);
				}
				else
				{
					throw new Exception("Incorrect edge found for shortest path.");
				}
			}
		}

		[Test]
		public void TestDijkstraDirectedSimple2()
		{
			Graph<int> g = new Graph<int>(true);

			Vertex<int> a = new Vertex<int>(1);
			Vertex<int> b = new Vertex<int>(2);
			Vertex<int> c = new Vertex<int>(3);

			g.AddVertex(a);
			g.AddVertex(b);
			g.AddVertex(c);

			g.AddEdge(a, c, 5);
			g.AddEdge(a, b, 3);
			g.AddEdge(b, c, 4);
			g.AddEdge(c, a, 3);

			Graph<int> result = NGenerics.Algorithms.Graph.DijkstrasAlgorithm<int>.FindShortestPaths(g, b);

			Assert.AreEqual(result.EdgeCount, 2);

			List<Edge<int>> edges = new List<Edge<int>>();

			using (IEnumerator<Edge<int>> enumerator = result.Edges)
			{
				while (enumerator.MoveNext())
				{
					edges.Add(enumerator.Current);
				}
			}

			for (int i = 0; i < edges.Count; i++)
			{
				if ((edges[i].FromVertex.Data == 2) && (edges[i].ToVertex.Data == 3))
				{
					Assert.AreEqual(edges[i].Weight, 4);
					Assert.AreEqual(edges[i].FromVertex.Weight, 0);
					Assert.AreEqual(edges[i].ToVertex.Weight, 4);
				}
				else if ((edges[i].FromVertex.Data == 3) && (edges[i].ToVertex.Data == 1))
				{
					Assert.AreEqual(edges[i].Weight, 3);
					Assert.AreEqual(edges[i].FromVertex.Weight, 4);
					Assert.AreEqual(edges[i].ToVertex.Weight, 7);
				}
				else
				{
					throw new Exception("Incorrect edge found for shortest path.");
				}
			}
		}


		[Test]
		public void TestUndirectedSimple1()
		{
			Graph<int> g = new Graph<int>(false);

			Vertex<int> a = new Vertex<int>(1);
			Vertex<int> b = new Vertex<int>(2);
			Vertex<int> c = new Vertex<int>(3);

			g.AddVertex(a);
			g.AddVertex(b);
			g.AddVertex(c);

			g.AddEdge(a, c, 1);
			g.AddEdge(a, b, 3);
			g.AddEdge(b, c, 5);

			Graph<int> result = NGenerics.Algorithms.Graph.DijkstrasAlgorithm<int>.FindShortestPaths(g, a);

			Assert.AreEqual(result.EdgeCount, 2);

			List<Edge<int>> edges = new List<Edge<int>>();

			using (IEnumerator<Edge<int>> enumerator = result.Edges)
			{
				while (enumerator.MoveNext())
				{
					edges.Add(enumerator.Current);
				}
			}

			for (int i = 0; i < edges.Count; i++)
			{
				if ((edges[i].FromVertex.Data == 1) && (edges[i].ToVertex.Data == 2))
				{
					Assert.AreEqual(edges[i].Weight, 3);
					Assert.AreEqual(edges[i].FromVertex.Weight, 0);
					Assert.AreEqual(edges[i].ToVertex.Weight, 3);
				}
				else if ((edges[i].FromVertex.Data == 1) && (edges[i].ToVertex.Data == 3))
				{
					Assert.AreEqual(edges[i].Weight, 1);
					Assert.AreEqual(edges[i].FromVertex.Weight, 0);
					Assert.AreEqual(edges[i].ToVertex.Weight, 1);
				}
				else
				{
					throw new Exception("Incorrect edge found for shortest path.");
				}
			}
		}

		[Test]
		public void TestUndirectedSimple2()
		{
			Graph<int> g = new Graph<int>(false);

			Vertex<int> a = new Vertex<int>(1);
			Vertex<int> b = new Vertex<int>(2);
			Vertex<int> c = new Vertex<int>(3);

			g.AddVertex(a);
			g.AddVertex(b);
			g.AddVertex(c);

			g.AddEdge(a, c, 1);
			g.AddEdge(a, b, 3);
			g.AddEdge(b, c, 5);

			Graph<int> result = NGenerics.Algorithms.Graph.DijkstrasAlgorithm<int>.FindShortestPaths(g, b);

			Assert.AreEqual(result.EdgeCount, 2);

			List<Edge<int>> edges = new List<Edge<int>>();

			using (IEnumerator<Edge<int>> enumerator = result.Edges)
			{
				while (enumerator.MoveNext())
				{
					edges.Add(enumerator.Current);
				}
			}

			for (int i = 0; i < edges.Count; i++)
			{
				if ((edges[i].FromVertex.Data == 2) && (edges[i].ToVertex.Data == 1))
				{
					Assert.AreEqual(edges[i].Weight, 3);
					Assert.AreEqual(edges[i].FromVertex.Weight, 0);
					Assert.AreEqual(edges[i].ToVertex.Weight, 3);
				}
				else if ((edges[i].FromVertex.Data == 1) && (edges[i].ToVertex.Data == 3))
				{
					Assert.AreEqual(edges[i].Weight, 1);
					Assert.AreEqual(edges[i].FromVertex.Weight, 3);
					Assert.AreEqual(edges[i].ToVertex.Weight, 4);
				}
				else
				{
					throw new Exception("Incorrect edge found for shortest path.");
				}
			}
		}

		[Test]
		public void TestDirected2()
		{
			Graph<string> graph = new Graph<string>(true);

			Vertex<string> a = new Vertex<string>("a");
			Vertex<string> b = new Vertex<string>("b");
			Vertex<string> c = new Vertex<string>("c");
			Vertex<string> d = new Vertex<string>("d");
			Vertex<string> e = new Vertex<string>("e");
			Vertex<string> f = new Vertex<string>("f");

			graph.AddVertex(a);
			graph.AddVertex(b);
			graph.AddVertex(c);
			graph.AddVertex(d);
			graph.AddVertex(e);
			graph.AddVertex(f);

			graph.AddEdge(a, b, 5);
			graph.AddEdge(a, c, 5);
			graph.AddEdge(a, d, 5);
			graph.AddEdge(a, e, 5);
			graph.AddEdge(a, f, 1);

			graph.AddEdge(b, c, 1);

			graph.AddEdge(c, d, 1);

			graph.AddEdge(e, d, 2);

			graph.AddEdge(f, e, 2);
			graph.AddEdge(f, b, 2);

			Graph<string> result = DijkstrasAlgorithm<string>.FindShortestPaths(graph, a);

			Assert.AreEqual(result.VertexCount, 6);
			Assert.AreEqual(result.EdgeCount, 5);

			List<Edge<string>> edges = new List<Edge<string>>();

			using (IEnumerator<Edge<string>> enumerator = result.Edges)
			{
				while (enumerator.MoveNext())
				{
					edges.Add(enumerator.Current);
				}
			}

			for (int i = 0; i < edges.Count; i++)
			{
				if ((edges[i].FromVertex.Data == "a") && (edges[i].ToVertex.Data == "d"))
				{
					Assert.AreEqual(edges[i].Weight, 5);
					Assert.AreEqual(edges[i].FromVertex.Weight, 0);
					Assert.AreEqual(edges[i].ToVertex.Weight, 5);
				}
				else if ((edges[i].FromVertex.Data == "a") && (edges[i].ToVertex.Data == "f"))
				{
					Assert.AreEqual(edges[i].Weight, 1);
					Assert.AreEqual(edges[i].FromVertex.Weight, 0);
					Assert.AreEqual(edges[i].ToVertex.Weight, 1);
				}
				else if ((edges[i].FromVertex.Data == "b") && (edges[i].ToVertex.Data == "c"))
				{
					Assert.AreEqual(edges[i].Weight, 1);
					Assert.AreEqual(edges[i].FromVertex.Weight, 3);
					Assert.AreEqual(edges[i].ToVertex.Weight, 4);
				}
				else if ((edges[i].FromVertex.Data == "f") && (edges[i].ToVertex.Data == "b"))
				{
					Assert.AreEqual(edges[i].Weight, 2);
					Assert.AreEqual(edges[i].FromVertex.Weight, 1);
					Assert.AreEqual(edges[i].ToVertex.Weight, 3);
				}
				else if ((edges[i].FromVertex.Data == "f") && (edges[i].ToVertex.Data == "e"))
				{
					Assert.AreEqual(edges[i].Weight, 2);
					Assert.AreEqual(edges[i].FromVertex.Weight, 1);
					Assert.AreEqual(edges[i].ToVertex.Weight, 3);
				}
				else
				{
					throw new Exception("Incorrect edge found for shortest path.");
				}
			}
		}

		[Test]
		public void MoreComplicatedDirectedGraph()
		{
			Graph<string> graph = new Graph<string>(true);


			Vertex<string> a = new Vertex<string>("a");
			Vertex<string> b = new Vertex<string>("b");
			Vertex<string> c = new Vertex<string>("c");
			Vertex<string> d = new Vertex<string>("d");
			Vertex<string> e = new Vertex<string>("e");
			Vertex<string> f = new Vertex<string>("f");
			Vertex<string> g = new Vertex<string>("g");

			graph.AddVertex(a);
			graph.AddVertex(b);
			graph.AddVertex(c);
			graph.AddVertex(d);
			graph.AddVertex(e);
			graph.AddVertex(f);
			graph.AddVertex(g);

			graph.AddEdge(a, f, 4);

			graph.AddEdge(b, a, 2);
			graph.AddEdge(b, g, 2);

			graph.AddEdge(c, b, 3);

			graph.AddEdge(d, c, 2);
			graph.AddEdge(d, e, 1);

			graph.AddEdge(e, f, 3);
			graph.AddEdge(e, g, 2);

			graph.AddEdge(f, g, 1);

			graph.AddEdge(g, f, 1);
			graph.AddEdge(g, d, 3);
			graph.AddEdge(g, c, 4);
			graph.AddEdge(g, a, 1);

			Graph<string> result = DijkstrasAlgorithm<string>.FindShortestPaths(graph, g);

			Assert.AreEqual(result.VertexCount, 7);
			Assert.AreEqual(result.EdgeCount, 6);

			List<Edge<string>> edges = new List<Edge<string>>();

			using (IEnumerator<Edge<string>> enumerator = result.Edges)
			{
				while (enumerator.MoveNext())
				{
					edges.Add(enumerator.Current);
				}
			}

			for (int i = 0; i < edges.Count; i++)
			{
				if ((edges[i].FromVertex.Data == "g") && (edges[i].ToVertex.Data == "a"))
				{
					Assert.AreEqual(edges[i].Weight, 1);
					Assert.AreEqual(edges[i].FromVertex.Weight, 0);
					Assert.AreEqual(edges[i].ToVertex.Weight, 1);
				}
				else if ((edges[i].FromVertex.Data == "c") && (edges[i].ToVertex.Data == "b"))
				{
					Assert.AreEqual(edges[i].Weight, 3);
					Assert.AreEqual(edges[i].FromVertex.Weight, 4);
					Assert.AreEqual(edges[i].ToVertex.Weight, 7);
				}
				else if ((edges[i].FromVertex.Data == "g") && (edges[i].ToVertex.Data == "c"))
				{
					Assert.AreEqual(edges[i].Weight, 4);
					Assert.AreEqual(edges[i].FromVertex.Weight, 0);
					Assert.AreEqual(edges[i].ToVertex.Weight, 4);
				}
				else if ((edges[i].FromVertex.Data == "g") && (edges[i].ToVertex.Data == "d"))
				{
					Assert.AreEqual(edges[i].Weight, 3);
					Assert.AreEqual(edges[i].FromVertex.Weight, 0);
					Assert.AreEqual(edges[i].ToVertex.Weight, 3);
				}
				else if ((edges[i].FromVertex.Data == "d") && (edges[i].ToVertex.Data == "e"))
				{
					Assert.AreEqual(edges[i].Weight, 1);
					Assert.AreEqual(edges[i].FromVertex.Weight, 3);
					Assert.AreEqual(edges[i].ToVertex.Weight, 4);
				}
				else if ((edges[i].FromVertex.Data == "g") && (edges[i].ToVertex.Data == "f"))
				{
					Assert.AreEqual(edges[i].Weight, 1);
					Assert.AreEqual(edges[i].FromVertex.Weight, 0);
					Assert.AreEqual(edges[i].ToVertex.Weight, 1);
				}
				else
				{
					throw new Exception("Incorrect edge found for shortest path.");
				}
			}
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestNullGraph()
		{
			DijkstrasAlgorithm<int>.FindShortestPaths(null, new Vertex<int>(5));
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestNullVertex()
		{
			DijkstrasAlgorithm<int>.FindShortestPaths(new Graph<int>(true), null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void TestInvalidVertex()
		{
			DijkstrasAlgorithm<int>.FindShortestPaths(new Graph<int>(true), new Vertex<int>(5));
		}
	}
}
