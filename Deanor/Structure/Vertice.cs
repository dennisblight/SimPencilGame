using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deanor.Structure
{
    public class Vertice<T> : IVertice<T>
    {
        private IList<IEdge<T>> edges;

        public Vertice()
        {
            edges = new List<IEdge<T>>();
        }

        public IList<IEdge<T>> Edges => edges;

        public IEdge<T> Connect(IVertice<T> vertice, T cost)
        {
            if(vertice != this && !this.Connected(vertice))
            {
                var edge = new Edge<T>(this, vertice, cost);
                edges.Add(edge);
                vertice.Edges.Add(edge);
                return edge;
            }
            return null;
        }
    }

    public interface IVertice<T>
    {
        IList<IEdge<T>> Edges { get; }
        IEdge<T> Connect(IVertice<T> vertice, T cost);
    }

    public static class VerticeHelper
    {
        public static bool Connected<T>(this IVertice<T> vertice, IVertice<T> adjacent)
        {
            foreach (var e in vertice.Edges)
            {
                if (e.HasVertice(adjacent)) return true;
            }
            return false;
        }

        public static IEnumerable<IVertice<T>> GetAdjacents<T>(this IVertice<T> vertice)
        {
            foreach (var e in vertice.Edges)
            {
                yield return e.VerticeA == vertice ? e.VerticeB : e.VerticeA;
            }
        }
    }
}
