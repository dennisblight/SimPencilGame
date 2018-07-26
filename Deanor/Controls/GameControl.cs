using Deanor.AI;
using Deanor.Media;
using Deanor.Structure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Deanor.Controls
{
    public partial class GameControl : GraphControl
    {
        public static readonly DependencyProperty Player1ColorProperty;
        public static readonly DependencyProperty Player2ColorProperty;
        public static readonly DependencyProperty IsPlayer1TurnProperty;
        public static readonly DependencyProperty AIPlayerProperty;
        public static readonly RoutedEvent GamesEndEvent;
        public static readonly RoutedEvent TurnsEndEvent;

        public Color? Player1Color
        {
            get { return (Color?)GetValue(Player1ColorProperty); }
            set { SetValue(Player1ColorProperty, value); }
        }

        public Color? Player2Color
        {
            get { return (Color?)GetValue(Player2ColorProperty); }
            set { SetValue(Player2ColorProperty, value); }
        }

        public bool IsPlayer1Turn
        {
            get { return (bool)GetValue(IsPlayer1TurnProperty); }
            set { SetValue(IsPlayer1TurnProperty, value); }
        }

        public IGameAI AIPlayer
        {
            get { return (IGameAI)GetValue(AIPlayerProperty); }
            set { SetValue(AIPlayerProperty, value); }
        }

        public event GamesEndEventHandler GamesEnd
        {
            add { AddHandler(GamesEndEvent, value); }
            remove { RemoveHandler(GamesEndEvent, value); }
        }

        public event RoutedEventHandler TurnsEnd
        {
            add { AddHandler(TurnsEndEvent, value); }
            remove { RemoveHandler(TurnsEndEvent, value); }
        }

        static GameControl()
        {
            Player1ColorProperty = DependencyProperty.Register("Player1Color", typeof(Color?), typeof(GraphControl), new PropertyMetadata(Colors.Red));
            Player2ColorProperty = DependencyProperty.Register("Player2Color", typeof(Color?), typeof(GraphControl), new PropertyMetadata(Colors.Blue));
            IsPlayer1TurnProperty = DependencyProperty.Register("IsPlayer1Turn", typeof(bool), typeof(GraphControl), new PropertyMetadata(true, OnPlayerTurnChanged));
            AIPlayerProperty = DependencyProperty.Register("AIPlayer", typeof(IGameAI), typeof(GraphControl), new PropertyMetadata(null));
            GamesEndEvent = EventManager.RegisterRoutedEvent("GamesEnd", RoutingStrategy.Bubble, typeof(GamesEndEventHandler), typeof(GameControl));
            TurnsEndEvent = EventManager.RegisterRoutedEvent("TurnsEnd", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(GameControl));
        }

        private static void OnPlayerTurnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var o = d as GameControl;
            var nv = (bool)e.NewValue;
            o.RaiseEvent(new RoutedEventArgs(TurnsEndEvent, o));
        }

        private bool rendered;

        public void Surrender()
        {
            canvas.IsHitTestVisible = false;
            var gs = IsPlayer1Turn ? GameStatus.Player1Surrender : GameStatus.Player2Surrender;
            var args = new GamesEndEventArgs(GamesEndEvent, this, new GameResult(gs, null));
            RaiseEvent(args);
        }

        public override void Render()
        {
            base.Render();
            if (!rendered)
            {
                foreach (VerticeControl v in Vertices)
                {
                    v.InputStateChanged += Vertices_InputStateChanged;
                }
                foreach (EdgeControl e in Edges)
                {
                    e.CostChanged += Edges_CostChanged;
                }
                var binding = new MultiBinding();
                binding.Converter = new CurrentPlayerColorConverter();
                binding.Bindings.Add(new Binding("Player1Color") { Source = this });
                binding.Bindings.Add(new Binding("Player2Color") { Source = this });
                binding.Bindings.Add(new Binding("IsPlayer1Turn") { Source = this });
                SetBinding(PreviewColorProperty, binding);
                
                // Gunakan OnLoadAnimation saja untuk menjalankan animasi saat permainan dimulai..
                OnLoadAnimation();
                // Gunakan Bind saja, jika tidak menjalankan animasi..
                // Bind();
                rendered = true;
            }
        }

        public Storyboard Shuffle()
        {
            if (Vertices[0] is VerticeControl v && v.IsLoaded)
            {
                canvas.IsHitTestVisible = false;
                var tempV = new List<IVertice<int>>(Vertices);
                var pts = new Point[Vertices.Count];
                var r = new Random();
                for (int i = 0; i < pts.Length; i++)
                {
                    var len = tempV.Count;
                    var idx = r.Next() % len;
                    if(idx == i)
                    {
                        i--;
                        continue;
                    }
                    var vc = tempV[idx] as VerticeControl;
                    pts[i] = new Point(vc.CanvasLeft, vc.CanvasTop);
                    tempV.RemoveAt(idx);
                }
                var sb = new Storyboard();
                for(int i = 0; i < pts.Length; i++)
                {
                    var vc = Vertices[i] as VerticeControl;
                    sb.Children.Add(Anima.TranslateAnimation(pts[i], vc, Anima.NormalDuration));
                }
                //sb.Completed += (o, e) => { canvas.IsHitTestVisible = true; };
                //sb.Begin();
                return sb;
            }
            return null;
        }

        private void Bind()
        {
            foreach(VerticeControl v in Vertices)
            {
                foreach(EdgeControl e in v.Edges)
                {
                    var bindX = new Binding();
                    bindX.Source = v;
                    bindX.Mode = BindingMode.OneWay;
                    bindX.Path = new PropertyPath("(Canvas.Left)");
                    var bindY = new Binding();
                    bindY.Source = v;
                    bindY.Mode = BindingMode.OneWay;
                    bindY.Path = new PropertyPath("(Canvas.Top)");
                    var dpx = e.X1 == v.CanvasLeft && e.Y1 == v.CanvasTop ? EdgeControl.X1Property : EdgeControl.X2Property;
                    var dpy = dpx == EdgeControl.X1Property ? EdgeControl.Y1Property : EdgeControl.Y2Property;
                    e.SetBinding(dpx, bindX);
                    e.SetBinding(dpy, bindY);
                }
            }
        }

        public void AITurns()
        {
            var edgeID = AIPlayer.Decision(this).ID;
            var edge = this.FindEdge(edgeID) as EdgeControl;
            var v1 = edge.VerticeA as VerticeControl;
            var v2 = edge.VerticeB as VerticeControl;

            var v1Hover = v1.ToHoveredAnima();
            var v1Highlight = v1.ToHighlightAnima();
            var drawLine = DrawLineAnima(v2);
            var v2Highlight = v2.ToHighlightAnima();

            v1Hover.BeginTime = TimeSpan.FromMilliseconds(600);
            v1Highlight.BeginTime = TimeSpan.FromMilliseconds(600);
            drawLine.BeginTime = TimeSpan.FromMilliseconds(600);
            v2Highlight.BeginTime = TimeSpan.FromMilliseconds(50);

            bool _lock1 = false, _lock2 = false, _lock3 = false, _lock4 = false;
            v1Hover.Completed += (o, e) =>
            {
                if(!_lock1)
                {
                    Debug.WriteLine("v1Hover completed..");
                    v1Highlight.Begin();
                    _lock1 = true;
                }
            };
            v1Highlight.Completed += (o, e) =>
            {
                if(!_lock2)
                {
                    Debug.WriteLine("v1Highlight completed..");
                    supportLine.Visibility = Visibility.Visible;
                    supportLine.X1 = v1.CanvasLeft;
                    supportLine.Y1 = v1.CanvasTop;
                    supportLine.X2 = v1.CanvasLeft;
                    supportLine.Y2 = v1.CanvasTop;
                    drawLine.Begin();
                    _lock2 = true;
                }
            };
            drawLine.Completed += (o, e) =>
            {
                if (!_lock3)
                {
                    Debug.WriteLine("drawLine completed..");
                    edge.Highlighted = true;
                    v2Highlight.Begin();
                    _lock3 = true;
                }
            };
            
            v2Highlight.Completed += (o, e) =>
            {
                if (!_lock4)
                {
                    Debug.WriteLine("v2Highlight completed..");
                    if (lastRenderedEdge != null)
                    {
                        Panel.SetZIndex(lastRenderedEdge, EdgesZIndex);
                        canvas.Children.Remove(lastRenderedEdge);
                        canvas.Children.Add(lastRenderedEdge);
                    }
                    Panel.SetZIndex(edge, EdgesZIndex + 1);
                    lastRenderedEdge = edge;
                    edge.Highlighted = false;
                    supportLine.Visibility = Visibility.Hidden;
                    v1Hover.Remove();
                    drawLine.Remove();
                    v1Highlight.Remove();
                    v2Highlight.Remove();
                    verticeOrigin = null;
                    verticeAdjacent = null;
                    v1.ApplyPreviewColor();
                    v2.ApplyPreviewColor();

                    v1.ToOriginAnima().Begin();
                    v2.ToOriginAnima().Begin();
                    edge.Foreground = new SolidColorBrush(Player2Color.Value);
                    edge.Cost = 2;
                    _lock4 = true;
                }
            };

            v1Hover.Begin();
        }

        private void Edges_CostChanged(object sender, RoutedPropertyChangedEventArgs<int> e)
        {
            if (sender is EdgeControl edge)
            {
                var result = this.CheckWinner(edge);
                if (result.GameStatus != GameStatus.Continue)
                {
                    OnGamesEnd(result);
                }
                else
                {
                    IsPlayer1Turn ^= true;
                    var anima = Shuffle();
                    if(anima != null)
                    {
                        if(IsPlayer1Turn || AIPlayer == null)
                        {
                            anima.Completed += (o, ev) => { canvas.IsHitTestVisible = true; };
                        }
                        else
                        {
                            anima.Completed += (o, ev) =>
                            {
                                AITurns();
                                Debug.WriteLine("anima complete.. AI turns");
                            };
                        }
                        anima.Begin();
                    }
                }
            }
        }

        private void Vertices_InputStateChanged(object sender, RoutedPropertyChangedEventArgs<InputState> e)
        {
            if(sender is VerticeControl vertice)
            {
                if(vertice.InputState == InputState.Clicked)
                {
                    foreach(EdgeControl edge in vertice.Edges)
                    {
                        var adj = (vertice == edge.VerticeA ? edge.VerticeB : edge.VerticeA) as VerticeControl;
                        adj.IsEnabled = edge.Cost == 0;
                    }
                }
            }
        }

        protected virtual void OnGamesEnd(GameResult gameResult)
        {
            var set = new HashSet<VerticeControl>();
            IsEnabled = false;
            var fgColor = Colors.Black;
            if(gameResult.Edges != null)
            {
                foreach (EdgeControl f in gameResult.Edges)
                {
                    var v1 = f.VerticeA as VerticeControl;
                    var v2 = f.VerticeB as VerticeControl;
                    set.Add(v1);
                    set.Add(v2);
                    fgColor = (f.Foreground as SolidColorBrush).Color;
                    f.Highlighted = true;
                    Panel.SetZIndex(f, 85);
                }
            }
            var boomAnima = new Storyboard();
            foreach(var v in set)
            {
                v.Visibility = Visibility.Hidden;
                var nv = new VerticeControl(v.ID[0]);
                nv.Size = v.Size;
                nv.Foreground = new SolidColorBrush(fgColor);
                nv.Background = v.Background;
                nv.CanvasLeft = v.CanvasLeft;
                nv.CanvasTop = v.CanvasTop;
                Panel.SetZIndex(nv, 100);
                nv.InputState = InputState.Dragged;
                nv.IsEnabled = false;
                canvas.Children.Add(nv);

                var ell = new Ellipse();
                ell.Stroke = new SolidColorBrush(fgColor);
                ell.Width = 2;
                ell.Height = 2;
                ell.RenderTransformOrigin = new Point(0.5, 0.5);
                ell.RenderTransform = new ScaleTransform();
                Canvas.SetLeft(ell, -1 + v.CanvasLeft);
                Canvas.SetTop(ell, -1 + v.CanvasTop);
                canvas.Children.Add(ell);
                var sx = new DoubleAnimation(200, Anima.ShortDuration);
                var sy = new DoubleAnimation(200, Anima.ShortDuration);
                var op = new DoubleAnimation(0, Anima.ShortDuration);
                sx.FillBehavior = FillBehavior.Stop;
                sy.FillBehavior = FillBehavior.Stop;
                //op.FillBehavior = FillBehavior.Stop;
                Storyboard.SetTarget(sx, ell);
                Storyboard.SetTarget(sy, ell);
                Storyboard.SetTarget(op, ell);
                Storyboard.SetTargetProperty(sx, new PropertyPath("RenderTransform.ScaleX"));
                Storyboard.SetTargetProperty(sy, new PropertyPath("RenderTransform.ScaleY"));
                Storyboard.SetTargetProperty(op, new PropertyPath("Opacity"));
                boomAnima.Children.Add(sy);
                boomAnima.Children.Add(sx);
                boomAnima.Children.Add(op);
            }
            boomAnima.Completed += (o, e) => RaiseEvent(new GamesEndEventArgs(GamesEndEvent, this, gameResult));
            boomAnima.Begin();
        }

        protected override void VerticeDrop(VerticeControl origin, VerticeControl adjacent)
        {
            base.VerticeDrop(origin, adjacent);
            if(this.FindEdge(origin, adjacent) is EdgeControl edge)
            {
                edge.Cost = IsPlayer1Turn ? 1 : 2;
            }
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            foreach(VerticeControl v in Vertices)
            {
                v.IsEnabled = false;
                foreach(EdgeControl ed in v.Edges)
                {
                    if (ed.Cost == 0)
                    {
                        v.IsEnabled = true;
                        break;
                    }
                }
            }
        }
    }

    public static class GameExtentsion
    {
        public static GameResult CheckWinner(this IGraph<int> g, IEdge<int> firstFactor)
        {
            var factors = new HashSet<IEdge<int>>();
            factors.Add(firstFactor);
            foreach (var n in g.Vertices)
            {
                if(n != firstFactor.VerticeA && n != firstFactor.VerticeB)
                {
                    var e1 = g.FindEdge(n, firstFactor.VerticeA);
                    var e2 = g.FindEdge(n, firstFactor.VerticeB);
                    if (e1.Cost == firstFactor.Cost && e2.Cost == firstFactor.Cost)
                    {
                        factors.Add(e1);
                        factors.Add(e2);
                    }
                }
            }
            if(factors.Count > 2)
            {
                var possibleStatus = firstFactor.Cost == 1 ? GameStatus.Player2Win : GameStatus.Player1Win;
                return new GameResult(possibleStatus, factors);
            }
            foreach (var n in g.Edges)
            {
                if (n.Cost == 0)
                {
                    return new GameResult(GameStatus.Continue, null);
                }
            }
            return new GameResult(GameStatus.Draw, null);
        }
    }

    public class GameResult
    {
        private IEnumerable<IEdge<int>> edges;
        private GameStatus gameStatus;

        public IEnumerable<IEdge<int>> Edges => edges;

        public GameStatus GameStatus => gameStatus;
        
        public GameResult(GameStatus gameStatus, IEnumerable<IEdge<int>> edges)
        {
            this.gameStatus = gameStatus;
            this.edges = edges;
        }
    }

    public enum GameStatus
    {
        Player1Win = 1,
        Player1Surrender = 3,
        Player2Win = 2,
        Player2Surrender = 4,
        Draw = 0,
        Continue = -1,
    }

    public delegate void GamesEndEventHandler(object sender, GamesEndEventArgs args);

    public class GamesEndEventArgs : RoutedEventArgs
    {
        private readonly GameResult result;
        public GameResult GameResult => result;

        public GamesEndEventArgs(RoutedEvent routedEvent, object source, GameResult result)
            : base(routedEvent, source)
        {
            this.result = result;
        }
    }

    public class CurrentPlayerColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values[2].Equals(true) ? values[0] : values[1];
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
