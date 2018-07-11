using Deanor.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Deanor.Controls
{
    public partial class EdgeControl : UserControl, IEdge<int>
    {
        private VerticeControl verticeA;
        private VerticeControl verticeB;

        public IVertice<int> VerticeA => verticeA;
        public IVertice<int> VerticeB => verticeB;

        public int Cost
        {
            get { return (int)GetValue(CostProperty); }
            set { SetValue(CostProperty, value); }
        }

        public EdgeControl()
        {
            InitializeComponent();
        }

        public EdgeControl(VerticeControl verticeA, VerticeControl verticeB, int cost)
            : this()
        {
            this.verticeA = verticeA;
            this.verticeB = verticeB;
            Cost = cost;
        }
    }
}
