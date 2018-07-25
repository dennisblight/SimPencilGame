using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Deanor.Media
{
    public class Anima
    {
        internal static readonly Duration BlinkDuration = new Duration(TimeSpan.FromMilliseconds(70));
        internal static readonly Duration ShortDuration = new Duration(TimeSpan.FromMilliseconds(200));
        internal static readonly Duration NormalDuration = new Duration(TimeSpan.FromMilliseconds(500));

        internal static Timeline ScaleAnimation(double toValue, DependencyObject target, Duration duration)
        {
            var sb = new Storyboard();
            var xscaleAnima = new DoubleAnimation(toValue, duration);
            var yscaleAnima = new DoubleAnimation(toValue, duration);
            Storyboard.SetTarget(xscaleAnima, target);
            Storyboard.SetTarget(yscaleAnima, target);
            Storyboard.SetTargetProperty(xscaleAnima, new PropertyPath("RenderTransform.Children[0].ScaleX"));
            Storyboard.SetTargetProperty(yscaleAnima, new PropertyPath("RenderTransform.Children[0].ScaleY"));
            sb.Children.Add(yscaleAnima);
            sb.Children.Add(xscaleAnima);
            return sb;
        }

        internal static Timeline TranslateAnimation(Point toValue, DependencyObject target, Duration duration)
        {
            var sb = new Storyboard();
            var txAnima = new DoubleAnimation(toValue.X, duration);
            var tyAnima = new DoubleAnimation(toValue.Y, duration);
            Storyboard.SetTarget(txAnima, target);
            Storyboard.SetTarget(tyAnima, target);
            Storyboard.SetTargetProperty(txAnima, new PropertyPath("(Canvas.Left)"));
            Storyboard.SetTargetProperty(tyAnima, new PropertyPath("(Canvas.Top)"));
            sb.Children.Add(tyAnima);
            sb.Children.Add(txAnima);
            return sb;
        }

        internal static Timeline ForegroundAnimation(Color toValue, DependencyObject target, Duration duration)
        {
            var foregroundColorAnima = new ColorAnimation(toValue, duration);
            Storyboard.SetTarget(foregroundColorAnima, target);
            Storyboard.SetTargetProperty(foregroundColorAnima, new PropertyPath("Foreground.Color"));
            return foregroundColorAnima;
        }

        internal static Timeline StrokeThicknessAnimation(double toValue, DependencyObject target, Duration duration)
        {
            var anima = new DoubleAnimation(toValue, duration);
            Storyboard.SetTarget(anima, target);
            Storyboard.SetTargetProperty(anima, new PropertyPath("StrokeThickness"));
            return anima;
        }
    }
}
