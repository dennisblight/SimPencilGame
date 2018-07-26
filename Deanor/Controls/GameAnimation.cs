using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Deanor.Controls
{
    public partial class GameControl : GraphControl
    {
        private Storyboard DrawLineAnima(VerticeControl v2)
        {
            var sb = new Storyboard();
            var trXAnima = new DoubleAnimation();
            trXAnima.To = v2.CanvasLeft;
            trXAnima.By = 1;
            Storyboard.SetTarget(trXAnima, supportLine);
            Storyboard.SetTargetProperty(trXAnima, new PropertyPath("X2"));
            var trYAnima = new DoubleAnimation();
            trYAnima.To = v2.CanvasTop;
            trYAnima.By = 1;
            Storyboard.SetTarget(trYAnima, supportLine);
            Storyboard.SetTargetProperty(trYAnima, new PropertyPath("Y2"));
            sb.Children.Add(trXAnima);
            sb.Children.Add(trYAnima);
            return sb;
        }

        private void OnLoadAnimation()
        {
            var sb = new Storyboard();
            var partDuration = new Duration(TimeSpan.FromMilliseconds(400));
            var beginTime = TimeSpan.Zero;
            foreach(VerticeControl n in Vertices)
            {
                n.Tag = new Point(n.CanvasLeft, n.CanvasTop);
                var trXAnima = new DoubleAnimation(0, 0, partDuration);
                trXAnima.BeginTime = beginTime;
                trXAnima.EasingFunction = new QuadraticEase();
                var trYAnima = new DoubleAnimation(0, -SpreadDistance, partDuration);
                trYAnima.BeginTime = beginTime;
                trYAnima.EasingFunction = new QuadraticEase();

                Storyboard.SetTarget(trXAnima, n);
                Storyboard.SetTarget(trYAnima, n);
                Storyboard.SetTargetProperty(trXAnima, new PropertyPath("(Canvas.Left)"));
                Storyboard.SetTargetProperty(trYAnima, new PropertyPath("(Canvas.Top)"));

                sb.Children.Add(trXAnima);
                sb.Children.Add(trYAnima);
                sb.RemoveRequested += (o, e) => 
                {
                    var pt = (Point)n.Tag;
                    n.CanvasLeft = pt.X;
                    n.CanvasTop = pt.Y;
                    TryBind();
                };
            }
            beginTime += partDuration.TimeSpan;
            int i = 0;
            double angle = 360.0 / VerticesCount;
            foreach (VerticeControl n in Vertices)
            {
                var tr = (n.RenderTransform as TransformGroup).Children[2] as RotateTransform;
                tr.CenterY = SpreadDistance;
                var rotAnima = new DoubleAnimation(0, angle * i, partDuration);
                rotAnima.EasingFunction = new CircleEase();
                rotAnima.BeginTime = beginTime;

                Storyboard.SetTarget(rotAnima, n);
                Storyboard.SetTargetProperty(rotAnima, new PropertyPath("RenderTransform.Children[2].Angle"));

                sb.Children.Add(rotAnima);
                rotAnima.RemoveRequested += (o, e) => 
                {
                    tr.Angle = 0;
                    tr.CenterY = 0;
                    TryBind();
                };
                i++;
            }

            beginTime += partDuration.TimeSpan;
            Random r = new Random();
            foreach (EdgeControl edge in Edges)
            {
                edge.Tag = new double[] { edge.X1, edge.X2, edge.Y1, edge.Y2 };
                var n1 = (r.NextDouble() > 0.5 ? edge.VerticeA : edge.VerticeB) as VerticeControl;
                var n2 = (edge.VerticeA == n1 ? edge.VerticeB : edge.VerticeA) as VerticeControl;
                var trXAnima = new DoubleAnimation(n1.CanvasLeft, n2.CanvasLeft, partDuration);
                var trYAnima = new DoubleAnimation(n1.CanvasTop, n2.CanvasTop, partDuration);
                trXAnima.BeginTime = beginTime;
                trYAnima.BeginTime = beginTime;
                trXAnima.EasingFunction = new SineEase();
                trYAnima.EasingFunction = new SineEase();
                Storyboard.SetTarget(trXAnima, edge);
                Storyboard.SetTarget(trYAnima, edge);
                Storyboard.SetTargetProperty(trXAnima, new PropertyPath("X2"));
                Storyboard.SetTargetProperty(trYAnima, new PropertyPath("Y2"));

                sb.Children.Add(trXAnima);
                sb.Children.Add(trYAnima);
                edge.X1 = n1.CanvasLeft;
                edge.X2 = n1.CanvasLeft;
                edge.Y1 = n1.CanvasTop;
                edge.Y2 = n1.CanvasTop;
                trXAnima.RemoveRequested += (o, e) =>
                {
                    var dpo = Storyboard.GetTarget((o as AnimationClock).Timeline) as EdgeControl;
                    var arr = ((double[])dpo.Tag);
                    dpo.X1 = arr[0];
                    dpo.X2 = arr[1];
                    dpo.Y1 = arr[2];
                    dpo.Y2 = arr[3];
                    TryBind();
                };
                trYAnima.RemoveRequested += (o, e) =>
                {
                    var dpo = Storyboard.GetTarget((o as AnimationClock).Timeline) as EdgeControl;
                    var arr = ((double[])dpo.Tag);
                    dpo.X1 = arr[0];
                    dpo.X2 = arr[1];
                    dpo.Y1 = arr[2];
                    dpo.Y2 = arr[3];
                    TryBind();
                };
            }

            sb.Completed += (o, e) =>
            {
                sb.Remove();
                if(!_exclusiveLock)
                {
                    if (!IsPlayer1Turn && AIPlayer != null)
                    {
                        AITurns();
                        canvas.IsHitTestVisible = false;
                    }
                    Debug.WriteLine("Sakali weh atuh euy..");
                    _exclusiveLock = true;
                }
            };

            sb.Begin();
        }

        private bool _exclusiveLock = false;
        private int _removeCount = 0;
        private void TryBind()
        {
            _removeCount++;
            if(_removeCount == 2 * (Edges.Count + Vertices.Count))
            {
                Bind();
            }
        }
    }
}
