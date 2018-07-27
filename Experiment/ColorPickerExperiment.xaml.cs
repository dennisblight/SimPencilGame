using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Experiment
{
    /// <summary>
    /// Interaction logic for ColorPickerExperiment.xaml
    /// </summary>
    public partial class ColorPickerExperiment : Window
    {
        public ColorPickerExperiment()
        {
            InitializeComponent();
            //Loaded += ColorPickerExperiment_Loaded;
        }

        private void ColorPickerExperiment_Loaded(object sender, RoutedEventArgs e)
        {
            f3();return;
            var canvas = new Canvas();
            var points = SpreadPoints(30).ToArray();
            for(int i = 0; i < 6; i++)
            {
                var p1 = new Polygon();
                var brush = new SolidColorBrush((Color)Application.Current.Resources[$"Color{(i + 1 + (i * 2)):D2}"]);
                p1.Fill = brush;
                p1.Points.Add(points[0 + i]);
                p1.Points.Add(points[6 + i]);
                p1.Points.Add(points[12 + i]);
                
                //canvas.Children.Add(p1);

                var p2 = new Polygon();
                brush = new SolidColorBrush((Color)Application.Current.Resources[$"Color{(i + 3 + (i * 2)):D2}"]);
                p2.Fill = brush;
                p2.Points.Add(points[0 + i]);
                p2.Points.Add(points[6 + i]);
                if (i == 5)
                    p2.Points.Add(points[12]);
                else
                    p2.Points.Add(points[13 + i]);
                //canvas.Children.Add(p2);

                var p3 = new Polygon();
                brush = new SolidColorBrush((Color)Application.Current.Resources[$"Color{(i + 2 + (i * 2)):D2}"]);
                p3.Fill = brush;
                if (i == 5)
                    p3.Points.Add(points[12]);
                else
                    p3.Points.Add(points[13 + i]);
                p3.Points.Add(points[0 + i]);
                if (i == 5)
                    p3.Points.Add(points[0]);
                else
                    p3.Points.Add(points[1 + i]);
                canvas.Children.Add(p3);

            }
            canvas.VerticalAlignment = VerticalAlignment.Center;
            canvas.HorizontalAlignment = HorizontalAlignment.Center;
            mainGrid.Children.Add(canvas);
        }

        private void f3()
        {
            var canvas = new Canvas();
            var points = HexPoints(30);
            var segments = new List<LineSegment>();
            var indexMap = new int[]
            {
                1, 4, 7, 10, 13, 16,
                18, 3, 6, 9, 12, 15,
                2, 5, 8, 11, 14, 17
            };
            for(int i = 0; i < 18; i++)
            {
                var pgeom = new PathGeometry();
                var fig = new PathFigure();
                fig.StartPoint = new Point();
                //fig.Segments.Add(ls);
                fig.Segments.Add(new LineSegment(points[(i) % 6], false));
                fig.Segments.Add(new LineSegment(points[(i + 1) % 6], false));
                pgeom.Figures.Add(fig);
                var bIndex = indexMap[i];
                var brush = new SolidColorBrush((Color)Application.Current.Resources[$"Color{bIndex:D2}"]);
                var path = new Path();
                path.Fill = brush;
                path.Data = pgeom;
                var tg = new TransformGroup();
                tg.Children.Add(new TranslateTransform());
                tg.Children.Add(new RotateTransform());
                path.RenderTransform = tg;

                if (i < 6)
                {
                    Panel.SetZIndex(path, 2);
                }
                Canvas.SetLeft(path, 0);
                Canvas.SetTop(path, 0);
                canvas.Children.Add(path);
            }

            canvas.VerticalAlignment = VerticalAlignment.Center;
            canvas.HorizontalAlignment = HorizontalAlignment.Center;
            mainGrid.Children.Add(canvas);

            var sb = new Storyboard();
            var direction = DirectionVectors(0);
            const double sqrt3Per2 = 0.86602540378443864676372317075294;
            for(int j = 0; j < 3; j++)
            {
                for (int i = 0; i < 6; i++)
                {
                    var x = i + j * 6;
                    var ttXAnima = new DoubleAnimation(direction[i].X * 3, new Duration(TimeSpan.FromMilliseconds(200)));
                    var ttYAnima = new DoubleAnimation(direction[i].Y * 3, new Duration(TimeSpan.FromMilliseconds(200)));
                    Storyboard.SetTarget(ttXAnima, canvas.Children[x]);
                    Storyboard.SetTarget(ttYAnima, canvas.Children[x]);
                    Storyboard.SetTargetProperty(ttXAnima, new PropertyPath("(Path.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.X)"));
                    Storyboard.SetTargetProperty(ttYAnima, new PropertyPath("(Path.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.Y)"));
                    ttXAnima.BeginTime = TimeSpan.FromMilliseconds(200);
                    ttYAnima.BeginTime = TimeSpan.FromMilliseconds(200);
                    sb.Children.Add(ttXAnima);
                    sb.Children.Add(ttYAnima);

                    var ptAnima = new PointAnimation((Point)Vector.Multiply(direction[i], 60 * sqrt3Per2), new Duration(TimeSpan.FromMilliseconds(200)));
                    Storyboard.SetTargetProperty(ptAnima, new PropertyPath("(Path.Data).(PathGeometry.Figures)[0].(PathFigure.StartPoint)"));
                    Storyboard.SetTarget(ptAnima, canvas.Children[x]);
                    ptAnima.BeginTime = TimeSpan.FromMilliseconds(500);
                    sb.Children.Add(ptAnima);
                }
            }

            direction = DirectionVectors(0);
            for (int i = 6; i < 18; i++)
            {
                var rt = (canvas.Children[i].RenderTransform as TransformGroup).Children[1] as RotateTransform;
                var dir = Vector.Multiply(direction[i % 6], 65 * sqrt3Per2);
                rt.CenterX = dir.X;
                rt.CenterY = dir.Y;
                //canvas.Children[i].RenderTransformOrigin = (Point)Vector.Multiply(direction[i % 6], 63 * sqrt3Per2);
                var rotAnima = new DoubleAnimation();
                rotAnima.Duration = new Duration(TimeSpan.FromMilliseconds(200));
                rotAnima.To = (i < 12) ? 60 : -60;
                Storyboard.SetTarget(rotAnima, canvas.Children[i]);
                Storyboard.SetTargetProperty(rotAnima, new PropertyPath("(Path.RenderTransform).(TransformGroup.Children)[1].(RotateTransform.Angle)"));
                rotAnima.BeginTime = TimeSpan.FromMilliseconds(800);
                sb.Children.Add(rotAnima);
            }
            //sb.AutoReverse = true;
            //sb.RepeatBehavior = RepeatBehavior.Forever;
            sb.Begin();
        }

        private void f2()
        {
            Point pt1 = new Point(10, 10);
            Point pt1to = new Point(100, 120);
            Point pt2 = new Point(100, 10);
            Point pt2to = new Point(150, 30);
            Point pt3 = new Point(50, 50);
            Point pt3to = new Point(30, 80);
            PathGeometry pgeom = new PathGeometry();
            PathFigure pfig1 = new PathFigure();
            LineSegment ls1 = new LineSegment(pt1, true);
            LineSegment ls2 = new LineSegment(pt2, true);
            LineSegment ls3 = new LineSegment(pt3, true);

            PointAnimation pa1 = new PointAnimation(pt1to, new Duration(new TimeSpan(0, 0, 4)));
            PointAnimation pa2 = new PointAnimation(pt2to, new Duration(new TimeSpan(0, 0, 4)));
            PointAnimation pa3 = new PointAnimation(pt3to, new Duration(new TimeSpan(0, 0, 4)));

            pfig1.StartPoint = pt3;
            pfig1.Segments.Add(ls1);
            pfig1.Segments.Add(ls2);
            pfig1.Segments.Add(ls3);

            pgeom.Figures.Add(pfig1);
            Path myPath = new Path();
            myPath.Stroke = Brushes.Black;
            myPath.StrokeThickness = 3;
            myPath.Fill = Brushes.Blue;
            myPath.Data = pgeom;

            // Add this to the Grid I named 'MyGrid'
            mainGrid.Children.Add(myPath);

            ls1.BeginAnimation(LineSegment.PointProperty, pa1);
            ls2.BeginAnimation(LineSegment.PointProperty, pa2);
            ls3.BeginAnimation(LineSegment.PointProperty, pa3);
            pfig1.BeginAnimation(PathFigure.StartPointProperty, pa3);
        }

        private Point[] HexPoints(double distance)
        {
            var direction = DirectionVectors(-Math.PI / 6);
            return new Point[]
            {
                (Point)Vector.Multiply(direction[0], distance),
                (Point)Vector.Multiply(direction[1], distance),
                (Point)Vector.Multiply(direction[2], distance),
                (Point)Vector.Multiply(direction[3], distance),
                (Point)Vector.Multiply(direction[4], distance),
                (Point)Vector.Multiply(direction[5], distance),
            };
        }

        private Vector[] DirectionVectors(double offset)
        {
            const double sixtyDegrees = Math.PI / 3;
            //const double offset = sixtyDegrees / 2;
            return new Vector[] 
            {
                new Vector(Math.Cos(sixtyDegrees * 0 + offset), Math.Sin(sixtyDegrees * 0 + offset)),
                new Vector(Math.Cos(sixtyDegrees * 1 + offset), Math.Sin(sixtyDegrees * 1 + offset)),
                new Vector(Math.Cos(sixtyDegrees * 2 + offset), Math.Sin(sixtyDegrees * 2 + offset)),
                new Vector(Math.Cos(sixtyDegrees * 3 + offset), Math.Sin(sixtyDegrees * 3 + offset)),
                new Vector(Math.Cos(sixtyDegrees * 4 + offset), Math.Sin(sixtyDegrees * 4 + offset)),
                new Vector(Math.Cos(sixtyDegrees * 5 + offset), Math.Sin(sixtyDegrees * 5 + offset)),
            };
        }

        private IEnumerable<Point> SpreadPoints(double distance)
        {
            var offset = Math.PI / 2.0;
            for (int i = 0; i < 6; i++)
            {
                var a = i * Math.PI / 3.0 + offset;
                var pt = new Point();
                pt.X = distance * Math.Cos(a);
                pt.Y = -distance * Math.Sin(a);
                yield return pt;
            }

            for (int i = 0; i < 6; i++)
            {
                var a = i * Math.PI / 3.0 + offset;
                var pt = new Point();
                pt.X = 2 * distance * Math.Cos(a);
                pt.Y = -2 * distance * Math.Sin(a);
                yield return pt;
            }

            offset = Math.PI / 3.0;
            for (int i = 0; i < 6; i++)
            {
                var a = i * Math.PI / 3.0 + offset;
                var pt = new Point();
                pt.X = Math.Sqrt(3) * distance * Math.Cos(a);
                pt.Y = -Math.Sqrt(3) * distance * Math.Sin(a);
                yield return pt;
            }
        }
    }
}
