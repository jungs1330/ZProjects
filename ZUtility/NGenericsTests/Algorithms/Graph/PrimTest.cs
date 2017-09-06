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
using NGenerics.Algorithms.Graph;
using NGenerics.DataStructures;

namespace NGenericsTests.Algorithms.Graph
{
    [TestFixture]
    public class PrimTest
    {
        [Test]
        public void TestPrimsAlgorithm()
        {
            Graph<int> g = new Graph<int>(false);

            List<Vertex<int>> vertices = new List<Vertex<int>>();

            for (int i = 1; i < 15; i++)
            {
                vertices.Add(g.AddVertex(i));
            }

            AddEdge(g, vertices, 1, 2, 5);
            AddEdge(g, vertices, 1, 5, 4);
            AddEdge(g, vertices, 2, 4, 10);
            AddEdge(g, vertices, 2, 3, 6);
            AddEdge(g, vertices, 3, 4, 2);
            AddEdge(g, vertices, 3, 5, 6);
            AddEdge(g, vertices, 3, 7, 4);
            AddEdge(g, vertices, 5, 6, 1);
            AddEdge(g, vertices, 6, 7, 1);
            AddEdge(g, vertices, 6, 8, 9);
            AddEdge(g, vertices, 6, 9, 5);
            AddEdge(g, vertices, 7, 9, 3);
            AddEdge(g, vertices, 7, 10, 4);
            AddEdge(g, vertices, 9, 10, 6);
            AddEdge(g, vertices, 9, 12, 2);
            AddEdge(g, vertices, 11, 12, 9);
            AddEdge(g, vertices, 11, 13, 8);
            AddEdge(g, vertices, 13, 14, 6);

            Graph<int> result = PrimsAlgorithm<int>.FindMinimalSpanningTree(g, vertices[0]);

            Assert.AreEqual(result.ContainsEdge(1, 2), true);
            Assert.AreEqual(result.ContainsEdge(1, 5), true);
            Assert.AreEqual(result.ContainsEdge(5, 6), true);
            Assert.AreEqual(result.ContainsEdge(6, 8), true);
            Assert.AreEqual(result.ContainsEdge(6, 7), true);
            Assert.AreEqual(result.ContainsEdge(7, 3), true);
            Assert.AreEqual(result.ContainsEdge(3, 4), true);
            Assert.AreEqual(result.ContainsEdge(7, 9), true);
            Assert.AreEqual(result.ContainsEdge(7, 10), true);
            Assert.AreEqual(result.ContainsEdge(9, 12), true);
            Assert.AreEqual(result.ContainsEdge(12, 11), true);
            Assert.AreEqual(result.ContainsEdge(11, 13), true);
            Assert.AreEqual(result.ContainsEdge(13, 14), true);

            Assert.AreEqual(result.EdgeCount, 13);

            double totalCost = 0;

            using (IEnumerator<Edge<int>> enumerator = result.Edges) {
                while (enumerator.MoveNext())
                {
                    totalCost += enumerator.Current.Weight;
                }
            }

            Assert.AreEqual(totalCost, 58);
        }

        private void AddEdge(Graph<int> g, List<Vertex<int>> vertices, int value1, int value2, int weight)
        {
            g.AddEdge(vertices[value1 - 1], vertices[value2 - 1], weight);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullGraph()
        {
            PrimsAlgorithm<int>.FindMinimalSpanningTree(null, new Vertex<int>(5));
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullVertex()
        {
            PrimsAlgorithm<int>.FindMinimalSpanningTree(new Graph<int>(true), null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidVertex()
        {
            PrimsAlgorithm<int>.FindMinimalSpanningTree(new Graph<int>(true), new Vertex<int>(5));
        }
    }
}
