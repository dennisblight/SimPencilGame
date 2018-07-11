using Deanor.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Deanor.Controls
{
    public partial class VerticeControl : UserControl, IVertice<int>
    {
        private readonly List<IEdge<int>> edges;

        public IList<IEdge<int>> Edges => edges;

        public VerticeControl()
        {
            InitializeComponent();
            edges = new List<IEdge<int>>();
        }

        public IEdge<int> Connect(IVertice<int> vertice, int cost)
        {
            if (vertice != this && !this.Connected(vertice))
            {
                var edge = new EdgeControl(this, (VerticeControl)vertice, cost);
                edges.Add(edge);
                vertice.Edges.Add(edge);
                return edge;
            }
            return null;
        }
    }
}
