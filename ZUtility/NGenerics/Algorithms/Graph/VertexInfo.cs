using System;
using System.Collections.Generic;
using System.Text;
using NGenerics.DataStructures;

namespace NGenerics.Algorithms.Graph
{
    internal class VertexInfo<T>
    {
        #region Globals

        private double distance;
        private bool isFinalised;
        private Edge<T> edgeFollowed;

        #endregion

        #region Construction

        public VertexInfo(double d, Edge<T> edgeFollowed, bool isFinalised)
        {
            this.distance = d;
            this.edgeFollowed = edgeFollowed;
            this.isFinalised = isFinalised;
        }

        #endregion

        #region Properties

        public double Distance
        {
            get
            {
                return distance;
            }
            set
            {
                distance = value;
            }
        }

        public Edge<T> EdgeFollowed
        {
            get
            {
                return edgeFollowed;
            }
            set
            {
                edgeFollowed = value;
            }
        }

        public bool IsFinalised
        {
            get
            {
                return isFinalised;
            }
            set
            {
                isFinalised = value;
            }
        }

        #endregion
    }
}
