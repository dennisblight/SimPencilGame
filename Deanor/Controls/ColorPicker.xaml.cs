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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Deanor.Controls
{
    /// <summary>
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : UserControl
    {
        public static readonly DependencyProperty RadiusProperty;
        public static readonly DependencyProperty PickedColorProperty;
        public static readonly DependencyProperty PickedColorIndexProperty;
        private static readonly DependencyPropertyKey PickedColorPropertyKey;

        private const double Sqrt3Per2 = 0.86602540378443864676372317075294;

        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public Color PickedColor
        {
            get { return (Color)GetValue(PickedColorProperty); }
            private set { SetValue(PickedColorPropertyKey, value); }
        }

        public int PickedColorIndex
        {
            get { return (int)GetValue(PickedColorIndexProperty); }
            set { SetValue(PickedColorIndexProperty, value); }
        }

        static ColorPicker()
        {
            PickedColorIndexProperty = DependencyProperty.Register("PickedColorIndex", typeof(int), typeof(ColorPicker), new PropertyMetadata(0, OnPickedColorIndexChanged));
            RadiusProperty = DependencyProperty.Register("Radius", typeof(double), typeof(ColorPicker), new PropertyMetadata(25.0, OnRadiusChanged));
            PickedColorPropertyKey = DependencyProperty.RegisterAttachedReadOnly("PickedColorKey", typeof(Color), typeof(ColorPicker), new PropertyMetadata(Color.FromArgb(0xff, 0xff, 0xf1, 0x00)));
            PickedColorProperty = PickedColorPropertyKey.DependencyProperty;
        }

        private static void OnPickedColorIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var o = d as ColorPicker;
            var nv = (int)e.NewValue;
            var ov = (int)e.OldValue;
            o.PickedColor = (o._colorOptionsShape[nv].Fill as SolidColorBrush).Color;
            (o._centerPolygon.Fill as SolidColorBrush).Color = o.PickedColor;
            (o._centerPolygon.Stroke as SolidColorBrush).Color = Color.Multiply(o.PickedColor, 0.6f);
            o._colorOptionsShape[ov].StrokeThickness = 0.0;
            o._colorOptionsShape[nv].StrokeThickness = 2.0;
            //o.IndexChangedAnimation(nv).Begin();
        }

        private static void OnRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var o = d as ColorPicker;
            var nv = (double)e.NewValue;
            var points = HexPoints(nv);
            var direction = DirectionVectors(0);
            for (int i = 0; i < 18; i++)
            {
                var p1 = (o._colorOptionsShape[i].Data as PathGeometry).Figures[0].Segments[0] as LineSegment;
                var p2 = (o._colorOptionsShape[i].Data as PathGeometry).Figures[0].Segments[1] as LineSegment;
                p1.Point = points[i % 6];
                p2.Point = points[(1 + i) % 6];
                if (6 <= i)
                {
                    var rt = (o._colorOptionsShape[i].RenderTransform as TransformGroup).Children[1] as RotateTransform;
                    var dir = Vector.Multiply(direction[i % 6], 2 * nv * Sqrt3Per2 + nv * 0.3);
                    rt.CenterX = dir.X;
                    rt.CenterY = dir.Y;
                }
            }
            o._centerPolygon.Points[0] = points[0];
            o._centerPolygon.Points[1] = points[1];
            o._centerPolygon.Points[2] = points[2];
            o._centerPolygon.Points[3] = points[3];
            o._centerPolygon.Points[4] = points[4];
            o._centerPolygon.Points[5] = points[5];
        }

        private Path[] _colorOptionsShape = new Path[18];
        private Polygon _centerPolygon = new Polygon();

        public ColorPicker()
        {
            InitializeComponent();

            var indexMap = new int[]
            {
                1, 4, 7, 10, 13, 16,
                18, 3, 6, 9, 12, 15,
                2, 5, 8, 11, 14, 17
            };
            for (int i = 0; i < 18; i++)
            {
                var pgeom = new PathGeometry();
                var fig = new PathFigure();
                fig.StartPoint = new Point();
                fig.IsClosed = true;
                fig.IsFilled = true;
                fig.Segments.Add(new LineSegment(new Point(), true));
                fig.Segments.Add(new LineSegment(new Point(), true));
                pgeom.Figures.Add(fig);
                var bIndex = indexMap[i];
                var brush = new SolidColorBrush((Color)Application.Current.Resources[$"Color{bIndex:D2}"]);
                var path = new Path();
                path.Fill = brush;
                path.Stroke = new SolidColorBrush(Color.Multiply(brush.Color, 0.6f));
                path.Data = pgeom;
                var tg = new TransformGroup();
                tg.Children.Add(new TranslateTransform());
                tg.Children.Add(new RotateTransform());
                path.RenderTransform = tg;

                if (i < 6)
                {
                    Panel.SetZIndex(path, 1);
                }

                path.IsHitTestVisible = false;
                canvas.Children.Add(path);
                path.MouseEnter += ColorOptionMouseEnter;
                path.MouseLeave += ColorOptionMouseLeave;
                path.MouseLeftButtonDown += ColorOptionMouseLBDown;
                path.MouseLeftButtonUp += ColorOptionMouseLBUp;
                _colorOptionsShape[i] = path;
                path.Cursor = Cursors.Hand;
                path.Tag = i;
            }


            _centerPolygon.Points.Add(new Point());
            _centerPolygon.Points.Add(new Point());
            _centerPolygon.Points.Add(new Point());
            _centerPolygon.Points.Add(new Point());
            _centerPolygon.Points.Add(new Point());
            _centerPolygon.Points.Add(new Point());
            _centerPolygon.Fill = new SolidColorBrush();
            _centerPolygon.Stroke = new SolidColorBrush();
            _centerPolygon.StrokeThickness = 2.0;
            _centerPolygon.Cursor = Cursors.Hand;
            Panel.SetZIndex(_centerPolygon, 2);

            _centerPolygon.MouseEnter += HexMouseEnter;
            _centerPolygon.MouseLeave += HexMouseLeave;
            _centerPolygon.MouseLeftButtonDown += HexMouseLButtonDown;
            _centerPolygon.MouseLeftButtonUp += HexMouseLButtonUp;

            canvas.Children.Add(_centerPolygon);

            OnRadiusChanged(this, new DependencyPropertyChangedEventArgs(RadiusProperty, 0, Radius));
            OnPickedColorIndexChanged(this, new DependencyPropertyChangedEventArgs(PickedColorIndexProperty, 0, 0));
        }

        private void ColorOptionMouseLBUp(object sender, MouseButtonEventArgs e)
        {
            var p = sender as Path;
        }

        private void ColorOptionMouseLBDown(object sender, MouseButtonEventArgs e)
        {
            var p = sender as Path;
            PickedColorIndex = (int)p.Tag;
            CloseTray();
        }

        private void ColorOptionMouseLeave(object sender, MouseEventArgs e)
        {
            var p = sender as Path;
            if (!p.Tag.Equals(PickedColorIndex))
            {
                (sender as Path).StrokeThickness = 0;
                PreviewColorAnimation(PickedColor).Begin();
            }
        }

        private void ColorOptionMouseEnter(object sender, MouseEventArgs e)
        {
            var p = sender as Path;
            if (!p.Tag.Equals(PickedColorIndex))
            {
                (sender as Path).StrokeThickness = 2.0;
                PreviewColorAnimation((p.Fill as SolidColorBrush).Color).Begin();
            }
        }

        private void HexMouseLButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void HexMouseLButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_trayOpened)
            {
                CloseTray();
            }
            else
            {
                OpenTray();
            }
        }

        private void HexMouseLeave(object sender, MouseEventArgs e)
        {
            if (!_trayOpened)
            {
                HoverAnimation(true).Begin();
            }
        }

        private void HexMouseEnter(object sender, MouseEventArgs e)
        {
            if (!_trayOpened)
            {
                HoverAnimation().Begin();
            }
        }

        private bool _trayOpened;
        private void OpenTray()
        {
            if (!_trayOpened)
            {
                OpenTrayAnimation().Begin();
                for (int i = 0; i < 18; i++)
                {
                    _colorOptionsShape[i].IsHitTestVisible = true;
                }
                _trayOpened = true;
            }
        }

        private void CloseTray()
        {
            if (_trayOpened)
            {
                for (int i = 0; i < 18; i++)
                {
                    _colorOptionsShape[i].IsHitTestVisible = false;
                }
                OpenTrayAnimation(true).Begin();
                _trayOpened = false;
            }
        }

        private void TrayClosed(object sender, EventArgs e)
        {
            if (Mouse.DirectlyOver != _centerPolygon)
            {
                HexMouseLeave(_centerPolygon, new MouseEventArgs(Mouse.PrimaryDevice, 0));
            }
        }

        private Storyboard _previewColorAnimation;
        private Storyboard PreviewColorAnimation(Color color)
        {
            if (_previewColorAnimation == null)
            {
                _previewColorAnimation = new Storyboard();

                var fillAnima = new ColorAnimation();
                fillAnima.SpeedRatio = 5;
                Storyboard.SetTarget(fillAnima, _centerPolygon);
                Storyboard.SetTargetProperty(fillAnima, new PropertyPath("(Polygon.Fill).(SolidColorBrush.Color)"));
                _previewColorAnimation.Children.Add(fillAnima);

                var strokeAnima = new ColorAnimation();
                strokeAnima.SpeedRatio = 5;
                Storyboard.SetTarget(strokeAnima, _centerPolygon);
                Storyboard.SetTargetProperty(strokeAnima, new PropertyPath("(Polygon.Stroke).(SolidColorBrush.Color)"));
                _previewColorAnimation.Children.Add(strokeAnima);
            }
            {
                var fillAnima = _previewColorAnimation.Children[0] as ColorAnimation;
                fillAnima.To = color;

                var strokeAnima = _previewColorAnimation.Children[1] as ColorAnimation;
                strokeAnima.To = Color.Multiply(color, 0.6f);
            }

            return _previewColorAnimation;
        }

        private Storyboard _hoverAnimation;
        private Storyboard HoverAnimation(bool reverse = false)
        {
            var direction = DirectionVectors(0);
            if (_hoverAnimation == null)
            {
                _hoverAnimation = new Storyboard();
                for (int j = 0; j < 3; j++)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        var x = i + j * 6;
                        var ttXAnima = new DoubleAnimation();
                        var ttYAnima = new DoubleAnimation();
                        ttXAnima.SpeedRatio = 5;
                        ttYAnima.SpeedRatio = 5;
                        Storyboard.SetTarget(ttXAnima, _colorOptionsShape[x]);
                        Storyboard.SetTarget(ttYAnima, _colorOptionsShape[x]);
                        Storyboard.SetTargetProperty(ttXAnima, new PropertyPath("(Path.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.X)"));
                        Storyboard.SetTargetProperty(ttYAnima, new PropertyPath("(Path.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.Y)"));
                        _hoverAnimation.Children.Add(ttXAnima);
                        _hoverAnimation.Children.Add(ttYAnima);
                    }
                }
            }
            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < 6; i++)
                {
                    var x = i + j * 6;

                    var ttXAnima = _hoverAnimation.Children[2 * x] as DoubleAnimation;
                    var ttYAnima = _hoverAnimation.Children[2 * x + 1] as DoubleAnimation;
                    if (reverse)
                    {
                        ttXAnima.To = direction[i].X * -Radius * 0.2;
                        ttYAnima.To = direction[i].Y * -Radius * 0.2;
                    }
                    else
                    {
                        ttXAnima.To = direction[i].X * Radius * 0.2;
                        ttYAnima.To = direction[i].Y * Radius * 0.2;
                    }
                }
            }

            return _hoverAnimation;
        }

        private Storyboard _openTrayAnimation;
        private Storyboard OpenTrayAnimation(bool reverse = false)
        {
            var direction = DirectionVectors(0);
            if (_openTrayAnimation == null)
            {
                _openTrayAnimation = new Storyboard();
                for (int j = 0; j < 3; j++)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        var x = i + j * 6;
                        var ptAnima = new PointAnimation();
                        ptAnima.SpeedRatio = 4;
                        const string ptPath = "(Path.Data).(PathGeometry.Figures)[0].(PathFigure.StartPoint)";
                        Storyboard.SetTargetProperty(ptAnima, new PropertyPath(ptPath));
                        Storyboard.SetTarget(ptAnima, canvas.Children[x]);
                        _openTrayAnimation.Children.Add(ptAnima);
                    }
                }
                for (int i = 6; i < 18; i++)
                {
                    var rotAnima = new DoubleAnimation();
                    rotAnima.SpeedRatio = 4;
                    Storyboard.SetTarget(rotAnima, canvas.Children[i]);
                    const string rotPath = "(Path.RenderTransform).(TransformGroup.Children)[1].(RotateTransform.Angle)";
                    Storyboard.SetTargetProperty(rotAnima, new PropertyPath(rotPath));
                    _openTrayAnimation.Children.Add(rotAnima);
                }
                _openTrayAnimation.Completed += TrayClosed;
            }

            if (reverse)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        var x = i + j * 6;
                        var ptAnima = _openTrayAnimation.Children[x] as PointAnimation;
                        ptAnima.To = new Point();
                        ptAnima.BeginTime = TimeSpan.FromMilliseconds(300);
                    }
                }
                for (int i = 6; i < 18; i++)
                {
                    var rotAnima = _openTrayAnimation.Children[i + 12] as DoubleAnimation;
                    rotAnima.To = 0;
                    rotAnima.BeginTime = TimeSpan.FromSeconds(0);
                }
            }
            else
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        var x = i + j * 6;
                        var ptAnima = _openTrayAnimation.Children[x] as PointAnimation;
                        ptAnima.To = (Point)Vector.Multiply(direction[i], 2 * Radius * Sqrt3Per2);
                        ptAnima.BeginTime = TimeSpan.FromMilliseconds(0);
                    }
                }
                for (int i = 6; i < 18; i++)
                {
                    var rotAnima = _openTrayAnimation.Children[i + 12] as DoubleAnimation;
                    rotAnima.To = (i < 12) ? 60 : -60;
                    rotAnima.BeginTime = TimeSpan.FromMilliseconds(300);
                }
            }

            return _openTrayAnimation;
        }

        private Storyboard _indexChangedAnimation;
        private Storyboard IndexChangedAnimation(int toIndex)
        {
            if (_indexChangedAnimation == null)
            {
                _indexChangedAnimation = new Storyboard();
                var fillAnima = new ColorAnimation();
                var strokeAnima = new ColorAnimation();
                fillAnima.SpeedRatio = 5;
                strokeAnima.SpeedRatio = 5;
                Storyboard.SetTarget(fillAnima, _centerPolygon);
                Storyboard.SetTarget(strokeAnima, _centerPolygon);
                const string pathFill = "(Polygon.Fill).(SolidColorBrush.Color)";
                const string pathStroke = "(Polygon.Stroke).(SolidColorBrush.Color)";
                Storyboard.SetTargetProperty(fillAnima, new PropertyPath(pathFill));
                Storyboard.SetTargetProperty(strokeAnima, new PropertyPath(pathStroke));
                _indexChangedAnimation.Children.Add(fillAnima);
                _indexChangedAnimation.Children.Add(strokeAnima);
            }

            var fa = _indexChangedAnimation.Children[0] as ColorAnimation;
            var sa = _indexChangedAnimation.Children[1] as ColorAnimation;
            fa.To = PickedColor;
            sa.To = Color.Multiply(PickedColor, 0.6f);

            return _indexChangedAnimation;
        }

        private static Point[] HexPoints(double distance)
        {
            var direction = DirectionVectors(-30.0);
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

        private static Vector[] DirectionVectors(double offsetDegrees)
        {
            const double d60 = Math.PI / 3;
            const double d01 = Math.PI / 180.0;
            var o = offsetDegrees * d01;
            return new Vector[]
            {
                new Vector(Math.Cos(d60 * 0 + o), Math.Sin(d60 * 0 + o)),
                new Vector(Math.Cos(d60 * 1 + o), Math.Sin(d60 * 1 + o)),
                new Vector(Math.Cos(d60 * 2 + o), Math.Sin(d60 * 2 + o)),
                new Vector(Math.Cos(d60 * 3 + o), Math.Sin(d60 * 3 + o)),
                new Vector(Math.Cos(d60 * 4 + o), Math.Sin(d60 * 4 + o)),
                new Vector(Math.Cos(d60 * 5 + o), Math.Sin(d60 * 5 + o)),
            };
        }
    }
}
