using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Experiment
{
    /// <summary>
    /// Interaction logic for GraphControl.xaml
    /// </summary>
    public partial class GraphControl : UserControl
    {
        private const int MinVertices = 4;
        private const int MaxVertices = 9;
        private const int NodesZIndex = 9;
        private const int SupportLineZIndex = 5;
        private const int EdgesZIndex = 1;

        public static readonly DependencyProperty VerticesCountProperty;
        public static readonly DependencyProperty VerticesSizeProperty;
        public static readonly DependencyProperty SpreadDistanceProperty;

        public int VerticesCount
        {
            get { return (int)GetValue(VerticesCountProperty); }
            set { SetValue(VerticesCountProperty, value); }
        }

        public double VerticesSize
        {
            get { return (double)GetValue(VerticesSizeProperty); }
            set { SetValue(VerticesSizeProperty, value); }
        }

        public double SpreadDistance
        {
            get { return (double)GetValue(SpreadDistanceProperty); }
            set { SetValue(SpreadDistanceProperty, value); }
        }

        static GraphControl()
        {
            VerticesCountProperty = DependencyProperty.Register("VerticesCount", typeof(int), typeof(GraphControl), new PropertyMetadata(6, OnVerticesCountChanged));
            VerticesSizeProperty = DependencyProperty.Register("VerticesSize", typeof(double), typeof(GraphControl), new PropertyMetadata(20.0));
            SpreadDistanceProperty = DependencyProperty.Register("SpreadDistance", typeof(double), typeof(GraphControl), new PropertyMetadata(120.0, OnSpreadDistanceChanged));
        }

        private static void OnSpreadDistanceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var graphControl = sender as GraphControl;
            if (graphControl.rendered)
            {
                throw new InvalidOperationException("Can't change spread distance after method Render() called.");
            }
        }

        private static void OnVerticesCountChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var graphControl = sender as GraphControl;
            if(graphControl.rendered)
            {
                throw new InvalidOperationException("Can't change vertices count after method Render() called.");
            }

            var newValue = (int)e.NewValue;
            if (MinVertices > newValue || newValue > MaxVertices)
            {
                graphControl.VerticesCount = (int)e.OldValue;
                throw new ArgumentOutOfRangeException($"Value of VerticesCount must between {MinVertices} and {MaxVertices} inclusive.");
            }
        }

        private bool rendered;
        private List<NodeControl> vertices;
        private NodeControl nodeOrigin;

        public GraphControl()
        {
            InitializeComponent();
        }

        public void Render()
        {
            if(!rendered)
            {
                vertices = new List<NodeControl>(VerticesCount);
                foreach(var pt in SpreadPoints())
                {
                    var node = new NodeControl();
                    node.MouseLeave += NodeControl_MouseLeave;
                    node.MouseLeftButtonUp += NodeControl_MouseLeftButtonUp;
                    node.InputStateChanged += NodeControl_InputStateChanged;
                    Canvas.SetLeft(node, pt.X);
                    Canvas.SetTop(node, pt.Y);
                    Panel.SetZIndex(node, NodesZIndex);
                    node.SetBinding(BackgroundProperty, new Binding("Background") { Source = this });
                    node.SetBinding(ForegroundProperty, new Binding("Foreground") { Source = this });
                    vertices.Add(node);
                    canvas.Children.Add(node);
                }
                rendered = true;
            }
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (nodeOrigin != null)
            {
                var pt = e.GetPosition(nodeOrigin.canvas);
            }
        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (nodeOrigin != null)
            {
                // transisi dari PICKED ke ORIGIN
                Origin();
            }
        }

        private void NodeControl_MouseLeave(object sender, MouseEventArgs e)
        {
            //#warning Kode dibawah tidak pernah dijalankan
            if (nodeOrigin != null && nodeOrigin != sender)
            {
                var node = sender as NodeControl;
                // transisi dari NODE_HOVERED ke PICKED
                node.InputState = InputState.Origin;
                //nodeAdjacent = null;
            }
        }

        private void NodeControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (nodeOrigin != null && nodeOrigin != sender)
            {
                var node = sender as NodeControl;
                // transisi dari NODE_HOVERED ke ORIGIN
                NodeDrop(nodeOrigin, node);
                Origin();
                if (node.IsMouseOver)
                {
                    node.InputState = InputState.Hovered;
                }
                else
                {
                    node.InputState = InputState.Origin;
                }
            }
        }

        private void NodeControl_InputStateChanged(object sender, RoutedPropertyChangedEventArgs<InputState> e)
        {
            var node = sender as NodeControl;
            switch (e.NewValue)
            {
                case InputState.Clicked:
                    //node.PreviewColor = pickedColor;
                    break;
                case InputState.Dragged:
                    if (nodeOrigin == null)
                    {
                        // transisi dari ORIGIN ke PICKED
                        nodeOrigin = node;
                    }
                    break;
                case InputState.Hovered:
                    if (nodeOrigin != null && nodeOrigin != node)
                    {
                        // transisi dari PICKED ke NODE_HOVERED
                        node.InputState = InputState.Dragged;
                        //nodeAdjacent = node;
                    }
                    break;
            }
        }

        private void Origin()
        {
            nodeOrigin.InputState = InputState.Origin;
            nodeOrigin = null;
            //nodeAdjacent = null;
        }

        private void NodeDrop(NodeControl origin, NodeControl adjecent)
        {

        }

        private IEnumerable<Point> SpreadPoints()
        {
            var step = 2 * Math.PI / VerticesCount;
            var offset = Math.PI / 2;
            for (int i = 0; i < VerticesCount; i++)
            {
                var angle = i * step - offset;
                yield return new Point
                {
                    X = SpreadDistance * Math.Cos(angle),
                    Y = SpreadDistance * Math.Sin(angle)
                };
            }
        }
    }
}
