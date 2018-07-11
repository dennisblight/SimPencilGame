using Deanor.Structure;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for EdgeControl.xaml
    /// </summary>
    public partial class EdgeControl : UserControl, IEdge<int>
    {
        private const double BottomLineHighlightRatio = 2.4;
        private const double TopLineHighlightRatio = 0.8;

        public static readonly DependencyProperty CostProperty;
        public static readonly DependencyProperty X1Property;
        public static readonly DependencyProperty X2Property;
        public static readonly DependencyProperty Y1Property;
        public static readonly DependencyProperty Y2Property;
        public static readonly DependencyProperty SizeProperty;
        public static readonly DependencyProperty HighlightedProperty;
        public static readonly RoutedEvent CostChangedEvent;

        static EdgeControl()
        {
            CostProperty = DependencyProperty.Register("Cost", typeof(int), typeof(EdgeControl), new PropertyMetadata(0, OnCostChanged));
            X1Property = DependencyProperty.Register("X1", typeof(double), typeof(EdgeControl), new PropertyMetadata(0.0));
            X2Property = DependencyProperty.Register("X2", typeof(double), typeof(EdgeControl), new PropertyMetadata(0.0));
            Y1Property = DependencyProperty.Register("Y1", typeof(double), typeof(EdgeControl), new PropertyMetadata(0.0));
            Y2Property = DependencyProperty.Register("Y2", typeof(double), typeof(EdgeControl), new PropertyMetadata(0.0));
            SizeProperty = DependencyProperty.Register("Size", typeof(double), typeof(EdgeControl), new PropertyMetadata(4.0));
            HighlightedProperty = DependencyProperty.Register("Highlighted", typeof(bool), typeof(EdgeControl), new PropertyMetadata(false, OnHightlightedChanged));
            CostChangedEvent = EventManager.RegisterRoutedEvent("CostChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<int>), typeof(EdgeControl));
        }

        private static void OnHightlightedChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var edgeControl = sender as EdgeControl;
            var newValue = (bool)e.NewValue;
            if(newValue)
            {
                var sb = new Storyboard();
                sb.Children.Add(StrokeThicknessAnima(edgeControl.Size * BottomLineHighlightRatio, TimeSpan.FromMilliseconds(50), edgeControl.bottomLine));
                sb.Children.Add(StrokeThicknessAnima(edgeControl.Size * TopLineHighlightRatio, TimeSpan.FromMilliseconds(50), edgeControl.topLine));
                sb.Begin();
            }
            else
            {
                var sb = new Storyboard();
                sb.Children.Add(StrokeThicknessAnima(edgeControl.Size, TimeSpan.FromMilliseconds(50), edgeControl.bottomLine));
                sb.Children.Add(StrokeThicknessAnima(edgeControl.Size, TimeSpan.FromMilliseconds(50), edgeControl.topLine));
                sb.Begin();
            }
        }

        private static void OnCostChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var edgeControl = sender as EdgeControl;
            var args = new RoutedPropertyChangedEventArgs<int>((int)e.OldValue, (int)e.NewValue);
            args.RoutedEvent = CostChangedEvent;
            edgeControl.RaiseEvent(args);
        }

        public double X1
        {
            get { return (double)GetValue(X1Property); }
            set { SetValue(X1Property, value); }
        }

        public double X2
        {
            get { return (double)GetValue(X2Property); }
            set { SetValue(X2Property, value); }
        }

        public double Y1
        {
            get { return (double)GetValue(Y1Property); }
            set { SetValue(Y1Property, value); }
        }

        public double Y2
        {
            get { return (double)GetValue(Y2Property); }
            set { SetValue(Y2Property, value); }
        }

        public double Size
        {
            get { return (double)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        public bool Highlighted
        {
            get { return (bool)GetValue(HighlightedProperty); }
            set { SetValue(HighlightedProperty, value); }
        }

        public event RoutedPropertyChangedEventHandler<int> CostChanged
        {
            add { AddHandler(CostChangedEvent, value); }
            remove { RemoveHandler(CostChangedEvent, value); }
        }

        private static DoubleAnimation StrokeThicknessAnima(double toValue, TimeSpan duration, DependencyObject target)
        {
            var strokeThicknessAnima = new DoubleAnimation(toValue, new Duration(duration));
            Storyboard.SetTarget(strokeThicknessAnima, target);
            Storyboard.SetTargetProperty(strokeThicknessAnima, new PropertyPath("StrokeThickness"));
            return strokeThicknessAnima;
        }
    }
    
    public class SizeRatioConverter : IValueConverter
    {
        private double ratio;
        public double Ratio
        {
            get { return ratio; }
            set { ratio = value; }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value * ratio;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value / ratio;
        }
    }
}
