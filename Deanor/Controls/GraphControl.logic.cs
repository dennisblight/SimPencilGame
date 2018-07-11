using Deanor.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Deanor.Controls
{
    public partial class GraphControl : UserControl, IGraph<int>
    {
        private List<IVertice<int>> vertices;
        private List<IEdge<int>> edges;

        public IList<IVertice<int>> Vertices => vertices;

        public IList<IEdge<int>> Edges => edges;
    }
}
