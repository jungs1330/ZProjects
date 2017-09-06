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
using NGenerics.DataStructures;
using NGenerics.Misc;
using NGenerics.Enumerations;
using NGenerics.Comparers;

namespace NGenerics.Algorithms.Graph
{
    /// <summary>
    /// An implementation of Prim's Minimal Spanning Tree Algorithm.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class PrimsAlgorithm<T>
    {
        #region Public Methods

        /// <summary>
        /// Finds the minimal spanning tree of the graph supplied.
        /// </summary>
        /// <param name="weightedGraph">The weighted graph.</param>
        /// <param name="fromVertex">The vertex to start from.</param>
        /// <returns>A graph representing the minimal spanning tree of the graph supplied.</returns>
        public static Graph<T> FindMinimalSpanningTree(Graph<T> weightedGraph, Vertex<T> fromVertex)
        {
            #region Parameter Checks

            if (weightedGraph == null)
            {
                throw new ArgumentNullException("weightedGraph");
            }

            if (fromVertex == null)
            {
                throw new ArgumentNullException("fromVertex");
            }

            if (!weightedGraph.ContainsVertex(fromVertex))
            {
                throw new ArgumentException(Resources.VertexCouldNotBeFound);
            }

            #endregion

            Heap<Association<double, Vertex<T>>> heap =
                new Heap<Association<double, Vertex<T>>>(
                    HeapType.MinHeap,
                    new Comparers.AssociationKeyComparer<double, Vertex<T>>());

            Dictionary<Vertex<T>, VertexInfo<T>> vertexStatus = new Dictionary<Vertex<T>, VertexInfo<T>>();

            // Initialise the vertex distances to 
            using (IEnumerator<Vertex<T>> verticeEnumerator = weightedGraph.Vertices)
            {
                while (verticeEnumerator.MoveNext())
                {
                    vertexStatus.Add(verticeEnumerator.Current, new VertexInfo<T>(double.MaxValue, null, false));
                }
            }

            vertexStatus[fromVertex].Distance = 0;

            // Add the source vertex to the heap - we'll be branching out from it.		
            heap.Add(new Association<double, Vertex<T>>(0, fromVertex));

            while (heap.Count > 0)
            {
                Association<double, Vertex<T>> item = heap.RemoveRoot();

                VertexInfo<T> vertexInfo = vertexStatus[item.Value];
                List<Edge<T>> edges = item.Value.IncidentEdgeList;

                vertexStatus[item.Value].IsFinalised = true;

                // Enumerate through all the edges emanating from this node					
                for (int i = 0; i < edges.Count; i++)
                {
                    Edge<T> edge = edges[i];

                    Vertex<T> partnerVertex = edge.GetPartnerVertex(item.Value);
                    
                    VertexInfo<T> newVertexInfo = vertexStatus[partnerVertex];

                    if (!newVertexInfo.IsFinalised)
                    {
                        if (edge.Weight < newVertexInfo.Distance)
                        {
                            newVertexInfo.EdgeFollowed = edge;
                            newVertexInfo.Distance = edge.Weight;

                            heap.Add(new Association<double, Vertex<T>>(edge.Weight, partnerVertex));
                        }
                    }
                }
            }

            // Now build the new graph
            Graph<T> newGraph = new Graph<T>(weightedGraph.IsDirected);

            Dictionary<Vertex<T>, VertexInfo<T>>.Enumerator enumerator = vertexStatus.GetEnumerator();

            // This dictionary is used for mapping between the old vertices and the new vertices put into the graph
            Dictionary<Vertex<T>, Vertex<T>> vertexMap = new Dictionary<Vertex<T>, Vertex<T>>(vertexStatus.Count);

            Vertex<T>[] newVertices = new Vertex<T>[vertexStatus.Count];

            while (enumerator.MoveNext())
            {
                Vertex<T> newVertex = new Vertex<T>(
                    enumerator.Current.Key.Data,
                    enumerator.Current.Key.Weight
                );

                vertexMap.Add(enumerator.Current.Key, newVertex);

                newGraph.AddVertex(newVertex);
            }

            enumerator = vertexStatus.GetEnumerator();

            while (enumerator.MoveNext())
            {
                VertexInfo<T> info = enumerator.Current.Value;

                // Check if an edge has been included to this vertex
                if ((info.EdgeFollowed != null))
                {
                    newGraph.AddEdge(
                        vertexMap[info.EdgeFollowed.GetPartnerVertex(enumerator.Current.Key)],
                        vertexMap[enumerator.Current.Key], info.Distance);
                }
            }


            return newGraph;
        }

        #endregion
    }
}
