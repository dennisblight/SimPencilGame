using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Deanor.Controls
{
    public class HexButton : FrameworkElement
    {
        public static readonly DependencyProperty TextColorProperty;
        public static readonly DependencyProperty AccentColorProperty;
        public static readonly DependencyProperty GlyphProperty;
        public static readonly DependencyProperty TextProperty;
        private static readonly DependencyProperty InnerHexRadiusProperty;
        private static readonly DependencyProperty GlyphColorProperty;
        private static readonly DependencyProperty TextScaleProperty;

        public Color TextColor 
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        public Color AccentColor
        {
            get { return (Color)GetValue(AccentColorProperty); }
            set { SetValue(AccentColorProperty, value); }
        }

        public Char Glyph
        {
            get { return (Char)GetValue(GlyphProperty); }
            set { SetValue(GlyphProperty, value); }
        }

        public String Text
        {
            get { return (String)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        private double InnerHexRadius
        {
            get { return (double)GetValue(InnerHexRadiusProperty); }
            set { SetValue(InnerHexRadiusProperty, value); }
        }

        private Color GlyphColor
        {
            get { return (Color)GetValue(GlyphColorProperty); }
            set { SetValue(GlyphColorProperty, value); }
        }

        private double TextScale
        {
            get { return (double)GetValue(TextScaleProperty); }
            set { SetValue(TextScaleProperty, value); }
        }

        static HexButton()
        {
            TextColorProperty = DependencyProperty.Register("TextColor", typeof(Color), typeof(HexButton), new PropertyMetadata(Colors.Black, OnTextColorChanged));
            AccentColorProperty = DependencyProperty.Register("AccentColor", typeof(Color), typeof(HexButton), new PropertyMetadata(Colors.Red, OnAccentBrushChanged));
            TextProperty = DependencyProperty.Register("Text", typeof(String), typeof(HexButton), new PropertyMetadata(""));
            GlyphProperty = DependencyProperty.Register("Glyph", typeof(Char), typeof(HexButton), new PropertyMetadata('\xf060'));
            InnerHexRadiusProperty = DependencyProperty.Register("_InnerHexRadius", typeof(Double), typeof(HexButton), new PropertyMetadata(0.8, OnInnerRadiusChanged));
            GlyphColorProperty = DependencyProperty.Register("_GlyphColor", typeof(Color), typeof(HexButton), new PropertyMetadata(Colors.Red, OnGlyphColorChanged));
            TextScaleProperty = DependencyProperty.Register("_TextScale", typeof(double), typeof(HexButton), new PropertyMetadata(1.0, OnTextScaleChanged));
        }

        private static void OnTextScaleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var btn = (HexButton)d;
            btn.InvalidateVisual();
        }

        private static void OnGlyphColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var btn = (HexButton)d;
            btn.InvalidateVisual();
        }

        private static void OnInnerRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var btn = (HexButton)d;
            btn.InvalidateVisual();
        }

        private static void OnTextColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var btn = (HexButton)d;
            btn.InvalidateVisual();
        }

        private static void OnAccentBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var btn = (HexButton)d;
            btn.InvalidateVisual();
        }

        public HexButton()
        {
            var bind = new Binding();
            bind.Source = this;
            bind.Path = new PropertyPath("AccentColor");
            SetBinding(GlyphColorProperty, bind);
            Cursor = Cursors.Hand;
        }

        private SolidColorBrush _accentBrush = new SolidColorBrush();
        private SolidColorBrush _glyphBrush = new SolidColorBrush();
        private Pen _strokePen = new Pen(Brushes.Gray, 1);
        private Pen _hairlinePen = new Pen(Brushes.Black, 0);
        private static Typeface fontAwesome;
        private static Typeface FontAwesome
        {
            get
            {
                if (fontAwesome == null)
                {
                    fontAwesome = new Typeface(Application.Current.Resources["FontAwesome"] as FontFamily, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);
                }
                return fontAwesome;
            }
        }

        protected override void OnRender(DrawingContext dc)
        {
            _accentBrush.Color = AccentColor;
            {
                var or = GetRightHexOrigin();
                var ol = GetLeftHexOrigin();
                var distance = 0.8 * OnePerSqr3 * ActualHeight;
                var dv = DistanceVectors(distance);

                var contentHex = new StreamGeometry();
                using (var gc = contentHex.Open())
                {
                    gc.BeginFigure(Point.Add(or, dv[0]), true, true);
                    gc.PolyLineTo(new PointCollection()
                    {
                        Point.Add(or, dv[1]),
                        Point.Add(ol, dv[2]),
                        Point.Add(ol, dv[3]),
                        Point.Add(ol, dv[4]),
                        Point.Add(or, dv[5]),
                    }
                    , true, true);
                }
                dc.DrawGeometry(Brushes.White, new Pen(_accentBrush, 3), contentHex);


                var halfHex = new StreamGeometry();
                var offset = new Vector(OnePerSqr3 * ActualHeight - distance, 0);
                using (var gc = halfHex.Open())
                {
                    gc.BeginFigure(Point.Add(or, dv[0]), true, true);
                    gc.PolyLineTo(new PointCollection()
                    {
                        Point.Add(or, dv[1]),
                        Point.Subtract(Point.Add(or, dv[1]), offset),
                        Point.Subtract(Point.Add(or, dv[0]), offset),
                        Point.Subtract(Point.Add(or, dv[5]), offset),
                        Point.Add(or, dv[5])
                    }, false, true);
                }
                dc.DrawGeometry(_accentBrush, _hairlinePen, halfHex);
            }


            {
                var bigHex = new StreamGeometry();
                using (var gc = bigHex.Open())
                {
                    var hp = DistanceVectors(OnePerSqr3 * ActualHeight);
                    var o = GetLeftHexOrigin();
                    gc.BeginFigure(Point.Add(o, hp[0]), true, true);
                    gc.PolyLineTo(new PointCollection()
                    {
                        Point.Add(o, hp[1]),
                        Point.Add(o, hp[2]),
                        Point.Add(o, hp[3]),
                        Point.Add(o, hp[4]),
                        Point.Add(o, hp[5]),
                    }, false, true);
                }
                dc.DrawGeometry(_accentBrush, _hairlinePen, bigHex);
            }

            {
                var innerHex = new StreamGeometry();
                using (var gc = innerHex.Open())
                {
                    var hp = DistanceVectors(InnerHexRadius * OnePerSqr3 * ActualHeight);
                    var o = GetLeftHexOrigin();
                    gc.BeginFigure(Point.Add(o, hp[0]), true, true);
                    gc.PolyLineTo(new PointCollection()
                    {
                        Point.Add(o, hp[1]),
                        Point.Add(o, hp[2]),
                        Point.Add(o, hp[3]),
                        Point.Add(o, hp[4]),
                        Point.Add(o, hp[5]),
                    }, false, true);
                }
                dc.DrawGeometry(Brushes.White, _hairlinePen, innerHex);
            }

            {
                _glyphBrush.Color = GlyphColor;
                var ft = new FormattedText(Glyph.ToString(), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, FontAwesome, ActualHeight / 2, _glyphBrush);
                dc.DrawText(ft, Point.Subtract(GetLeftHexOrigin(), new Vector(ft.Width / 2, ft.Height / 2)));
            }

            {
                var tf = new Typeface(new FontFamily("Sergoe UI"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal);
                var ft = new FormattedText(Text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, tf, TextScale * ActualHeight / 3, _accentBrush);
                var cr = GetContentRect();
                var offset = new Vector((cr.Width - ft.Width) / 2, (cr.Height - ft.Height) / 2);
                dc.DrawText(ft, Point.Add(cr.TopLeft, offset));
            }
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            Storyboard sb = new Storyboard();
            var scaleAnima = new DoubleAnimation(0, new Duration(TimeSpan.FromMilliseconds(200)));
            var textScaleAnima = new DoubleAnimation(1.1, new Duration(TimeSpan.FromMilliseconds(100)));
            var colorAnima = new ColorAnimation(Colors.White, new Duration(TimeSpan.FromMilliseconds(200)));

            Storyboard.SetTarget(scaleAnima, this);
            Storyboard.SetTarget(textScaleAnima, this);
            Storyboard.SetTarget(colorAnima, this);

            Storyboard.SetTargetProperty(scaleAnima, new PropertyPath("_InnerHexRadius"));
            Storyboard.SetTargetProperty(textScaleAnima, new PropertyPath("_TextScale"));
            Storyboard.SetTargetProperty(colorAnima, new PropertyPath("_GlyphColor"));

            sb.Children.Add(scaleAnima);
            sb.Children.Add(textScaleAnima);
            sb.Children.Add(colorAnima);

            sb.Begin();
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            Storyboard sb = new Storyboard();
            var scaleAnima = new DoubleAnimation(0.8, new Duration(TimeSpan.FromMilliseconds(200)));
            var textScaleAnima = new DoubleAnimation(1, new Duration(TimeSpan.FromMilliseconds(100)));
            var colorAnima = new ColorAnimation(AccentColor, new Duration(TimeSpan.FromMilliseconds(200)));

            Storyboard.SetTarget(scaleAnima, this);
            Storyboard.SetTarget(textScaleAnima, this);
            Storyboard.SetTarget(colorAnima, this);
            Storyboard.SetTargetProperty(scaleAnima, new PropertyPath("_InnerHexRadius"));
            Storyboard.SetTargetProperty(textScaleAnima, new PropertyPath("_TextScale"));
            Storyboard.SetTargetProperty(colorAnima, new PropertyPath("_GlyphColor"));

            sb.Children.Add(scaleAnima);
            sb.Children.Add(textScaleAnima);
            sb.Children.Add(colorAnima);

            sb.Begin();
        }

        // The E value: 0.57735026918962576450914878050196
        private const double OnePerSqr3 = 0.57735026918962576450914878050196;
        private const double HalfPerSqr3 = 0.28867513459481288225457439025098;
        private const double Cos30 = 0.86602540378443864676372317075294;
        private const double Half = 0.5;
        private const double SixtyDegrees = Math.PI / 3;

        private Vector[] _distanceVector;
        private Vector[] DistanceVectors(double d)
        {
            if (_distanceVector == null)
            {
                _distanceVector = new Vector[6];
            }

            _distanceVector[0] = new Vector(d * Math.Cos(0), d * Math.Sin(0));
            _distanceVector[1] = new Vector(d * Math.Cos(SixtyDegrees), d * Math.Sin(SixtyDegrees));
            _distanceVector[2] = new Vector(d * Math.Cos(2 * SixtyDegrees), d * Math.Sin(2 * SixtyDegrees));
            _distanceVector[3] = new Vector(d * Math.Cos(Math.PI), d * Math.Sin(Math.PI));
            _distanceVector[4] = new Vector(d * Math.Cos(4 * SixtyDegrees), d * Math.Sin(4 * SixtyDegrees));
            _distanceVector[5] = new Vector(d * Math.Cos(5 * SixtyDegrees), d * Math.Sin(5 * SixtyDegrees));
            return _distanceVector;
        }

        private Point GetLeftHexOrigin()
        {
            return new Point(ActualHeight * OnePerSqr3, ActualHeight / 2);
        }

        private Point GetRightHexOrigin()
        {
            return new Point(ActualWidth - (ActualHeight * OnePerSqr3 * 0.8), ActualHeight / 2);
        }

        private Rect GetContentRect()
        {
            var or = GetRightHexOrigin();
            var ol = GetLeftHexOrigin();
            var pt1 = new Point(2 * ActualHeight * OnePerSqr3, 0);
            var pt2 = new Point(ActualWidth - 0.6 * ActualHeight * OnePerSqr3, ActualHeight);
            return new Rect(pt1, pt2);
        }
    }
}
