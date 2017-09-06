using System;
using System.Collections.Generic;
using System.Text;

namespace NGenerics.DataStructures
{
    /// <summary>
    /// An interface combining the IVisitableCollection and IList interface.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IVisitableList<T> : IVisitableCollection<T>, IList<T>
    {
    }
}
