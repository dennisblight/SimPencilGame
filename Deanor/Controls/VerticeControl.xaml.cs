using Deanor.Structure;
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
    /// Interaction logic for VerticeControl.xaml
    /// </summary>
    public partial class VerticeControl : UserControl, IVertice<int>
    {
        public static readonly DependencyProperty PreviewColorProperty;
        public static readonly DependencyProperty SizeProperty;
        public static readonly DependencyProperty InputStateProperty;
        public static readonly RoutedEvent InputStateChangedEvent;

        private const double TopRatioOrigin = 0.6;
        private const double MiddleRatioOrigin = 1.0;
        private const double BottomRatioOrigin = 1.0;
        private const double TopRatioAlternate = 0.4;
        private const double MiddleRatioAlternate = 1.2;
        private const double BottomRatioAlternate = 1.4;
        private const double ShadowLineRatioAlternate = 1.4;
        private const double ShadowLineRatioOrigin = 1.4;

        private Color originalColor;

        public double Size
        {
            get { return (double)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        public InputState InputState
        {
            get { return (InputState)GetValue(InputStateProperty); }
            set { SetValue(InputStateProperty, value); }
        }

        public Color? PreviewColor
        {
            get { return (Color?)GetValue(PreviewColorProperty); }
            set { SetValue(PreviewColorProperty, value); }
        }

        public double CanvasLeft
        {
            get { return Canvas.GetLeft(this); }
            set { Canvas.SetLeft(this, value); }
        }

        public double CanvasTop
        {
            get { return Canvas.GetTop(this); }
            set { Canvas.SetTop(this, value); }
        }

        public event RoutedPropertyChangedEventHandler<InputState> InputStateChanged
        {
            add { AddHandler(InputStateChangedEvent, value); }
            remove { RemoveHandler(InputStateChangedEvent, value); }
        }

        static VerticeControl()
        {
            SizeProperty = DependencyProperty.Register("Size", typeof(Double), typeof(VerticeControl), new PropertyMetadata(18.0, OnSizeChanged));
            PreviewColorProperty = DependencyProperty.Register("PreviewColor", typeof(Color?), typeof(VerticeControl), new PropertyMetadata(null, OnPreviewColorChanged));
            InputStateProperty = DependencyProperty.Register("InputState", typeof(InputState), typeof(VerticeControl), new PropertyMetadata(InputState.Origin, OnInputStateChanged));
            InputStateChangedEvent = EventManager.RegisterRoutedEvent("InputStateChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<InputState>), typeof(VerticeControl));
        }

        private static void OnPreviewColorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var verticeControl = sender as VerticeControl;
            var newValue = (Color?)e.NewValue;
            if (newValue.HasValue)
            {
                verticeControl.originalColor = (verticeControl.Foreground as SolidColorBrush).Color;
            }
        }

        private static void OnInputStateChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var verticeControl = sender as VerticeControl;
            var newValue = (InputState)e.NewValue;
            switch (newValue)
            {
                case InputState.Origin:
                    verticeControl.Origin();
                    break;
                case InputState.Hovered:
                    verticeControl.Hovered();
                    break;
                case InputState.Clicked:
                    verticeControl.Clicked();
                    break;
                case InputState.Dragged:
                    verticeControl.Dragged();
                    break;
            }

            var oldValue = (InputState)e.OldValue;
            var args = new RoutedPropertyChangedEventArgs<InputState>(oldValue, newValue);
            args.RoutedEvent = InputStateChangedEvent;
            verticeControl.RaiseEvent(args);
        }

        private static void OnSizeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var verticeControl = sender as VerticeControl;
            var newValue = -((double)e.NewValue) / 2.0;
            Canvas.SetLeft(verticeControl.topEllipse, newValue);
            Canvas.SetLeft(verticeControl.middleEllipse, newValue);
            Canvas.SetLeft(verticeControl.bottomEllipse, newValue);
            Canvas.SetTop(verticeControl.topEllipse, newValue);
            Canvas.SetTop(verticeControl.middleEllipse, newValue);
            Canvas.SetTop(verticeControl.bottomEllipse, newValue);
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            switch (InputState)
            {
                case InputState.Origin:
                    InputState = InputState.Hovered;
                    break;
                case InputState.Dragged:
                    InputState = InputState.Clicked;
                    break;
            }
        }

        protected override void OnTouchEnter(TouchEventArgs e)
        {
            base.OnTouchEnter(e);
            switch (InputState)
            {
                case InputState.Origin:
                    InputState = InputState.Hovered;
                    break;
                case InputState.Dragged:
                    InputState = InputState.Clicked;
                    break;
            }
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            switch (InputState)
            {
                case InputState.Hovered:
                    InputState = InputState.Origin;
                    break;
                case InputState.Clicked:
                    InputState = InputState.Dragged;
                    break;
            }
        }

        protected override void OnTouchLeave(TouchEventArgs e)
        {
            base.OnTouchLeave(e);
            switch (InputState)
            {
                case InputState.Hovered:
                    InputState = InputState.Origin;
                    break;
                case InputState.Clicked:
                    InputState = InputState.Dragged;
                    break;
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            switch (InputState)
            {
                case InputState.Hovered:
                    InputState = InputState.Clicked;
                    break;
            }
        }

        protected override void OnTouchDown(TouchEventArgs e)
        {
            base.OnTouchDown(e);
            switch (InputState)
            {
                case InputState.Hovered:
                    InputState = InputState.Clicked;
                    break;
            }
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            switch (InputState)
            {
                case InputState.Clicked:
                    InputState = InputState.Hovered;
                    break;
                case InputState.Dragged:
                    InputState = InputState.Origin;
                    break;
            }
        }

        protected override void OnTouchUp(TouchEventArgs e)
        {
            base.OnTouchUp(e);
            switch (InputState)
            {
                case InputState.Clicked:
                    InputState = InputState.Hovered;
                    break;
                case InputState.Dragged:
                    InputState = InputState.Origin;
                    break;
            }
        }

        private void Origin()
        {
            var sb = new Storyboard();
            sb.Children.Add(ScaleAnimation(TopRatioOrigin, TimeSpan.FromMilliseconds(50), topEllipse));
            sb.Children.Add(ScaleAnimation(MiddleRatioOrigin, TimeSpan.FromMilliseconds(50), middleEllipse));
            sb.Children.Add(ScaleAnimation(BottomRatioOrigin, TimeSpan.FromMilliseconds(50), bottomEllipse));
            if (PreviewColor != null)
            {
                //originalColor = (Foreground as SolidColorBrush).Color;
                sb.Children.Add(ForegroundColorAnimation(originalColor, TimeSpan.FromMilliseconds(50), this));
            }
            sb.Begin();
        }

        private void Hovered()
        {
            var sb = new Storyboard();
            sb.Children.Add(ScaleAnimation(TopRatioOrigin, TimeSpan.FromMilliseconds(50), topEllipse));
            sb.Children.Add(ScaleAnimation(MiddleRatioAlternate, TimeSpan.FromMilliseconds(50), middleEllipse));
            sb.Children.Add(ScaleAnimation(BottomRatioOrigin, TimeSpan.FromMilliseconds(50), bottomEllipse));
            if (PreviewColor.HasValue)
            {
                //originalColor = (Foreground as SolidColorBrush).Color;
                sb.Children.Add(ForegroundColorAnimation(originalColor, TimeSpan.FromMilliseconds(50), this));
                //MessageBox.Show(PreviewColor.ToString());
            }
            sb.Begin();
        }

        private void Clicked()
        {
            var sb = new Storyboard();
            sb.Children.Add(ScaleAnimation(TopRatioAlternate, TimeSpan.FromMilliseconds(50), topEllipse));
            sb.Children.Add(ScaleAnimation(MiddleRatioOrigin, TimeSpan.FromMilliseconds(50), middleEllipse));
            sb.Children.Add(ScaleAnimation(BottomRatioAlternate, TimeSpan.FromMilliseconds(50), bottomEllipse));
            if (PreviewColor != null)
            {
                //originalColor = (Foreground as SolidColorBrush).Color;
                sb.Children.Add(ForegroundColorAnimation(PreviewColor.Value, TimeSpan.FromMilliseconds(50), this));
            }
            sb.Begin();
        }

        private void Dragged()
        {
            var sb = new Storyboard();
            sb.Children.Add(ScaleAnimation(TopRatioAlternate, TimeSpan.FromMilliseconds(50), topEllipse));
            sb.Children.Add(ScaleAnimation(MiddleRatioOrigin, TimeSpan.FromMilliseconds(50), middleEllipse));
            sb.Children.Add(ScaleAnimation(BottomRatioAlternate, TimeSpan.FromMilliseconds(50), bottomEllipse));
            if (PreviewColor != null)
            {
                //originalColor = (Foreground as SolidColorBrush).Color;
                sb.Children.Add(ForegroundColorAnimation(PreviewColor.Value, TimeSpan.FromMilliseconds(50), this));
            }
            sb.Begin();
        }

        private Storyboard ScaleAnimation(double toValue, TimeSpan duration, DependencyObject target)
        {
            var sb = new Storyboard();
            var xscaleAnima = new DoubleAnimation(toValue, new Duration(duration));
            var yscaleAnima = new DoubleAnimation(toValue, new Duration(duration));
            Storyboard.SetTarget(xscaleAnima, target);
            Storyboard.SetTarget(yscaleAnima, target);
            Storyboard.SetTargetProperty(xscaleAnima, new PropertyPath("RenderTransform.ScaleX"));
            Storyboard.SetTargetProperty(yscaleAnima, new PropertyPath("RenderTransform.ScaleY"));
            sb.Children.Add(yscaleAnima);
            sb.Children.Add(xscaleAnima);
            return sb;
        }

        private ColorAnimation ForegroundColorAnimation(Color toValue, TimeSpan duration, DependencyObject target)
        {
            var foregroundColorAnima = new ColorAnimation(toValue, new Duration(duration));
            Storyboard.SetTarget(foregroundColorAnima, target);
            Storyboard.SetTargetProperty(foregroundColorAnima, new PropertyPath("Foreground.Color"));
            return foregroundColorAnima;
        }
    }

    public enum InputState
    {
        Origin,
        Hovered,
        Clicked,
        Dragged
    }
}
