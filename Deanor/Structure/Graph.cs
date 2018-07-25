using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deanor.Structure
{
    public class Graph<T> : IGraph<T>
    {
        private List<IVertice<T>> vertices;
        private List<IEdge<T>> edges;

        public IList<IVertice<T>> Vertices => vertices;
        public IList<IEdge<T>> Edges => edges;

        public Graph()
        {
            vertices = new List<IVertice<T>>();
            edges = new List<IEdge<T>>();
        }
    }

    public interface IGraph<T>
    {
        IList<IVertice<T>> Vertices { get; }
        IList<IEdge<T>> Edges { get; }
    }

    public static class GraphExtension
    {
        public static IEdge<T> FindEdge<T>(this IGraph<T> graph, IVertice<T> verticeA, IVertice<T> verticeB)
        {
            foreach (var e in verticeA.Edges)
            {
                if (e.HasVertice(verticeB)) return e;
            }
            return null;
        }

        public static IEdge<T> FindEdge<T>(this IGraph<T> graph, string id)
        {
            foreach (var e in graph.Edges)
            {
                if (e.ID.Equals(id) || e.ID.Equals(string.Concat(id[1], id[0]))) return e;
            }
            return null;
        }

        public static IGraph<T> ShallowCopy<T>(this IGraph<T> graph)
        {
            var graphClone = new Graph<T>();
            foreach (var v in graph.Vertices)
            {
                graphClone.Vertices.Add(new Vertice<T>(v.ID[0]));
            }

            for (int i = 1; i < graphClone.Vertices.Count; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    var edge = graph.FindEdge(graph.Vertices[i], graph.Vertices[j]);
                    graphClone.Edges.Add(graphClone.Vertices[i].Connect(graphClone.Vertices[j], edge.Cost));
                }
            }

            return graphClone;
        }
    }
}
