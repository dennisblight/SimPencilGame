using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deanor.Structure
{
    public class Edge<T> : IEdge<T>
    {
        private readonly IVertice<T> verticeA;
        private readonly IVertice<T> verticeB;
        private readonly string id;
        private T cost;

        public IVertice<T> VerticeA => verticeA;

        public IVertice<T> VerticeB => verticeB;

        public string ID => id;

        public Edge(string id, IVertice<T> verticeA, IVertice<T> verticeB, T cost)
        {
            this.verticeA = verticeA;
            this.verticeB = verticeB;
            this.cost = cost;
            this.id = id;
        }

        public Edge(IVertice<T> verticeA, IVertice<T> verticeB, T cost)
        {
            this.verticeA = verticeA;
            this.verticeB = verticeB;
            this.cost = cost;
            var id = verticeA.ID[0] < verticeB.ID[0] ? verticeA.ID : verticeB.ID;
            id += VerticeA.ID == id ? verticeA.ID : verticeA.ID;
            this.id = id;
        }

        public T Cost
        {
            get { return cost; }
            set { cost = value; }
        }
    }

    public interface IEdge<T>
    {
        IVertice<T> VerticeA { get; }
        IVertice<T> VerticeB { get; }
        string ID { get; }
        T Cost { get; set; }
    }

    public static class EdgeExtension
    {
        public static bool HasVertice<T>(this IEdge<T> edge, IVertice<T> vertice)
        {
            return edge.VerticeA == vertice || edge.VerticeB == vertice;
        }
    }
}
