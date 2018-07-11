using Deanor.Structure;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Deanor.Controls
{
    /// <summary>
    /// Interaction logic for GraphControl.xaml
    /// </summary>
    public partial class GraphControl : UserControl, IGraph<int>
    {
        private const int MinVertices = 4;
        private const int MaxVertices = 9;
        private const int VerticeZIndex = 100;
        private const int SupportLineZIndex = 80;
        private const int EdgesZIndex = 0;

        public static readonly DependencyProperty VerticesCountProperty;
        public static readonly DependencyProperty VerticesSizeProperty;
        public static readonly DependencyProperty SpreadDistanceProperty;
        public static readonly DependencyProperty PreviewColorProperty;
        public static readonly DependencyProperty PreviewBrushProperty;
        private static readonly DependencyPropertyKey ReadOnlyPreviewBrushProperty;

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

        public Color? PreviewColor
        {
            get { return (Color?)GetValue(PreviewColorProperty); }
            set { SetValue(PreviewColorProperty, value); }
        }

        public SolidColorBrush PreviewBrush
        {
            get { return (SolidColorBrush)GetValue(PreviewBrushProperty); }
            set { SetValue(ReadOnlyPreviewBrushProperty, value); }
        }

        static GraphControl()
        {
            VerticesCountProperty = DependencyProperty.Register("VerticesCount", typeof(int), typeof(GraphControl), new PropertyMetadata(6, OnVerticesCountChanged));
            VerticesSizeProperty = DependencyProperty.Register("VerticesSize", typeof(double), typeof(GraphControl), new PropertyMetadata(20.0));
            SpreadDistanceProperty = DependencyProperty.Register("SpreadDistance", typeof(double), typeof(GraphControl), new PropertyMetadata(120.0, OnSpreadDistanceChanged));
            PreviewColorProperty = DependencyProperty.Register("PreviewColor", typeof(Color?), typeof(GraphControl), new PropertyMetadata(null, OnPreviewColorChanged));
            ReadOnlyPreviewBrushProperty = DependencyProperty.RegisterReadOnly("PreviewBrush", typeof(SolidColorBrush), typeof(GraphControl), new PropertyMetadata(null));
            PreviewBrushProperty = ReadOnlyPreviewBrushProperty.DependencyProperty;
        }

        private static void OnPreviewColorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var graphControl = sender as GraphControl;
            var newValue = (Color?)e.NewValue;
            if(newValue.HasValue)
            {
                graphControl.PreviewBrush = new SolidColorBrush(newValue.Value);
            }
            else
            {
                graphControl.PreviewBrush = new SolidColorBrush(((SolidColorBrush)graphControl.Foreground).Color);
            }
            //graphControl.PreviewBrush = newValue.HasValue ? new SolidColorBrush(newValue.Value) : (SolidColorBrush)graphControl.Foreground;
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
            if (graphControl.rendered)
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
        private bool snapped;
        private VerticeControl verticeOrigin;
        private VerticeControl verticeAdjacent;
        internal EdgeControl supportLine;

        public GraphControl()
        {
            InitializeComponent();
        }

        public void Render()
        {
            if (!rendered)
            {
                vertices = new List<IVertice<int>>(VerticesCount);
                foreach (var pt in SpreadPoints())
                {
                    var vertice = new VerticeControl();
                    vertice.MouseLeave += VerticeControl_MouseLeave;
                    vertice.MouseLeftButtonUp += VerticeControl_MouseLeftButtonUp;
                    vertice.InputStateChanged += VerticeControl_InputStateChanged;
                    vertice.IsEnabledChanged += VerticeControl_IsEnabledChanged;
                    vertice.CanvasLeft = pt.X;
                    vertice.CanvasTop = pt.Y;
                    Panel.SetZIndex(vertice, VerticeZIndex);
                    //vertice.PreviewColor = Colors.AliceBlue;
                    vertice.SetBinding(VerticeControl.BackgroundProperty, new Binding("Background") { Source = this });
                    vertice.SetBinding(VerticeControl.ForegroundProperty, new Binding("Foreground") { Source = this, Converter = new CloneConverter() });
                    vertice.SetBinding(VerticeControl.SizeProperty, new Binding("VerticesSize") { Source = this });
                    vertice.SetBinding(VerticeControl.PreviewColorProperty, new Binding("PreviewColor") { Source = this });
                    vertices.Add(vertice);
                    canvas.Children.Add(vertice);
                }

                edges = new List<IEdge<int>>((VerticesCount * (VerticesCount - 1)) / 2);
                for (int i = 1; i < VerticesCount; i++)
                {
                    var v1 = Vertices[i] as VerticeControl;
                    for (int j = 0; j < i; j++)
                    {
                        var v2 = Vertices[j] as VerticeControl;
                        if(v1.Connect(v2, 0) is EdgeControl edgeControl)
                        {
                            edgeControl.X1 = v1.CanvasLeft;
                            edgeControl.Y1 = v1.CanvasTop;
                            edgeControl.X2 = v2.CanvasLeft;
                            edgeControl.Y2 = v2.CanvasTop;
                            edgeControl.MouseLeftButtonUp += Grid_MouseLeftButtonUp;
                            edgeControl.MouseMove += Grid_MouseMove;
                            Panel.SetZIndex(edgeControl, EdgesZIndex);
                            var binding = new Binding("VerticesSize")
                            {
                                Source = this,
                                Converter = new SizeRatioConverter() { Ratio = 0.2 }
                            };
                            edgeControl.SetBinding(EdgeControl.SizeProperty, binding);
                            edgeControl.SetBinding(EdgeControl.ForegroundProperty, new Binding("Foreground") { Source = this });
                            edgeControl.CostChanged += Edge_CostChanged;
                            canvas.Children.Add(edgeControl);
                            edges.Add(edgeControl);
                        }
                    }
                }
                {
                    supportLine = new EdgeControl();
                    supportLine.MouseLeftButtonUp += Grid_MouseLeftButtonUp;
                    supportLine.MouseMove += Grid_MouseMove;
                    Panel.SetZIndex(supportLine, SupportLineZIndex);
                    var binding = new Binding("VerticesSize")
                    {
                        Source = this,
                        Converter = new SizeRatioConverter() { Ratio = 0.2 }
                    };
                    supportLine.SetBinding(EdgeControl.SizeProperty, binding);
                    supportLine.SetBinding(EdgeControl.ForegroundProperty, new Binding("PreviewBrush") { Source = this });
                    supportLine.Highlighted = true;
                    canvas.Children.Add(supportLine);
                }
                rendered = true;
            }
        }

        private void VerticeControl_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(sender is VerticeControl vc)
            {
                if(vc.IsEnabled)
                {
                    Panel.SetZIndex(vc, VerticeZIndex);
                }
                else
                {
                    Panel.SetZIndex(vc, SupportLineZIndex - 1);
                    canvas.Children.Remove(vc);
                    canvas.Children.Add(vc);
                }
            }
        }

        private void Edge_CostChanged(object sender, RoutedPropertyChangedEventArgs<int> e)
        {
            //throw new NotImplementedException();
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (verticeOrigin != null && !snapped)
            {
                var pt = e.GetPosition(canvas);
                supportLine.X2 = pt.X;
                supportLine.Y2 = pt.Y;
            }
        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (verticeOrigin != null)
            {
                // transisi dari PICKED ke ORIGIN
                Origin();
            }
        }

        private void VerticeControl_MouseLeave(object sender, MouseEventArgs e)
        {
            //#warning Kode dibawah tidak pernah dijalankan
            if (verticeOrigin != null && verticeOrigin != sender)
            {
                var vertice = sender as VerticeControl;
                // transisi dari NODE_HOVERED ke PICKED
                vertice.InputState = InputState.Origin;
                //verticeAdjacent = null;
                ReleaseSnap();
            }
        }

        private void VerticeControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (verticeOrigin != null && verticeOrigin != sender)
            {
                var vertice = sender as VerticeControl;
                // transisi dari NODE_HOVERED ke ORIGIN
                VerticeDrop(verticeOrigin, vertice);
                Origin();
                if (vertice.IsMouseOver)
                {
                    vertice.InputState = InputState.Hovered;
                }
                else
                {
                    vertice.InputState = InputState.Origin;
                }
            }
            ReleaseSnap();
        }

        private void VerticeControl_InputStateChanged(object sender, RoutedPropertyChangedEventArgs<InputState> e)
        {
            var vertice = sender as VerticeControl;
            switch (e.NewValue)
            {
                case InputState.Clicked:
                    //vertice.PreviewColor = pickedColor;
                    break;
                case InputState.Dragged:
                    if (verticeOrigin == null)
                    {
                        // transisi dari ORIGIN ke PICKED
                        verticeOrigin = vertice;
                        supportLine.X1 = vertice.CanvasLeft;
                        supportLine.Y1 = vertice.CanvasTop;
                        supportLine.X2 = vertice.CanvasLeft;
                        supportLine.Y2 = vertice.CanvasTop;
                    }
                    break;
                case InputState.Hovered:
                    if (verticeOrigin != null && verticeOrigin != vertice)
                    {
                        // transisi dari PICKED ke NODE_HOVERED
                        vertice.InputState = InputState.Dragged;
                        supportLine.X2 = vertice.CanvasLeft;
                        supportLine.Y2 = vertice.CanvasTop;
                        Snap(vertice);
                    }
                    break;
            }
        }

        private void Origin()
        {
            verticeOrigin.InputState = InputState.Origin;
            verticeOrigin = null;

            supportLine.X1 = 0;
            supportLine.X2 = 0;
            supportLine.Y1 = 0;
            supportLine.Y2 = 0;
            //verticeAdjacent = null;
        }

        private EdgeControl lastRenderedEdge;

        private void VerticeDrop(VerticeControl origin, VerticeControl adjecent)
        {
            supportLine.X1 = 0;
            supportLine.X2 = 0;
            supportLine.Y1 = 0;
            supportLine.Y2 = 0;
            if(this.FindEdge(origin, adjecent) is EdgeControl edge)
            {
                if(lastRenderedEdge != null)
                {
                    Panel.SetZIndex(lastRenderedEdge, EdgesZIndex);
                    canvas.Children.Remove(lastRenderedEdge);
                    canvas.Children.Add(lastRenderedEdge);
                }
                edge.Foreground = supportLine.Foreground;
                Panel.SetZIndex(edge, EdgesZIndex + 1);
                lastRenderedEdge = edge;
                ReleaseSnap();
            }
        }

        private void Snap(VerticeControl vertice)
        {
            if(verticeOrigin != null)
            {
                snapped = true;
                if(this.FindEdge(vertice, verticeOrigin) is EdgeControl edge)
                {
                    edge.Highlighted = true;
                }
                verticeAdjacent = vertice;
            }
        }

        private void ReleaseSnap()
        {
            if (verticeOrigin != null)
            {
                snapped = false;
                if (this.FindEdge(verticeAdjacent, verticeOrigin) is EdgeControl edge)
                {
                    edge.Highlighted = false;
                }
                verticeAdjacent = null;
            }
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

    public class CloneConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is Freezable)
            {
                return (value as Freezable).Clone();
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
