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

namespace Experiment
{
    /// <summary>
    /// Interaction logic for NodeControl.xaml
    /// </summary>
    public partial class NodeControl : UserControl
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

        public event RoutedPropertyChangedEventHandler<InputState> InputStateChanged
        {
            add { AddHandler(InputStateChangedEvent, value); }
            remove { RemoveHandler(InputStateChangedEvent, value); }
        }

        public NodeControl()
        {
            InitializeComponent();
        }

        static NodeControl()
        {
            SizeProperty = DependencyProperty.Register("Size", typeof(Double), typeof(NodeControl), new PropertyMetadata(18.0, OnSizeChanged));
            PreviewColorProperty = DependencyProperty.Register("PreviewColor", typeof(Color?), typeof(NodeControl), new PropertyMetadata(null, OnPreviewColorChanged));
            InputStateProperty = DependencyProperty.Register("InputState", typeof(InputState), typeof(NodeControl), new PropertyMetadata(InputState.Origin, OnInputStateChanged));
            InputStateChangedEvent = EventManager.RegisterRoutedEvent("InputStateChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<InputState>), typeof(NodeControl));
        }

        private static void OnPreviewColorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var nodeControl = sender as NodeControl;
            var newValue = (Color?)e.NewValue;
            if(newValue != null)
            {
                nodeControl.originalColor = (nodeControl.Foreground as SolidColorBrush).Color;
            }
        }

        private static void OnInputStateChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var nodeControl = sender as NodeControl;
            var newValue = (InputState)e.NewValue;
            switch (newValue)
            {
                case InputState.Origin:
                    nodeControl.Origin();
                    break;
                case InputState.Hovered:
                    nodeControl.Hovered();
                    break;
                case InputState.Clicked:
                    nodeControl.Clicked();
                    break;
                case InputState.Dragged:
                    nodeControl.Dragged();
                    break;
            }

            var oldValue = (InputState)e.OldValue;
            var args = new RoutedPropertyChangedEventArgs<InputState>(oldValue, newValue);
            args.RoutedEvent = InputStateChangedEvent;
            nodeControl.RaiseEvent(args);
        }

        private static void OnSizeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var nodeControl = sender as NodeControl;
            var newValue = -((double)e.NewValue) / 2.0;
            Canvas.SetLeft(nodeControl.topEllipse, newValue);
            Canvas.SetLeft(nodeControl.middleEllipse, newValue);
            Canvas.SetLeft(nodeControl.bottomEllipse, newValue);
            Canvas.SetTop(nodeControl.topEllipse, newValue);
            Canvas.SetTop(nodeControl.middleEllipse, newValue);
            Canvas.SetTop(nodeControl.bottomEllipse, newValue);
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            switch(InputState)
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

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
        }

        private void Origin()
        {
            var sb = new Storyboard();
            sb.Children.Add(ScaleAnimation(TopRatioOrigin, TimeSpan.FromMilliseconds(50), topEllipse));
            sb.Children.Add(ScaleAnimation(MiddleRatioOrigin, TimeSpan.FromMilliseconds(50), middleEllipse));
            sb.Children.Add(ScaleAnimation(BottomRatioOrigin, TimeSpan.FromMilliseconds(50), bottomEllipse));
            if(PreviewColor != null)
            {
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
            if (PreviewColor != null)
            {
                sb.Children.Add(ForegroundColorAnimation(originalColor, TimeSpan.FromMilliseconds(50), this));
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

        private DoubleAnimation StrokeThicknessAnimation(double toValue, TimeSpan duration, DependencyObject target)
        {
            var strokeThicknessAnima = new DoubleAnimation(toValue, new Duration(duration));
            Storyboard.SetTarget(strokeThicknessAnima, target);
            Storyboard.SetTargetProperty(strokeThicknessAnima, new PropertyPath("StrokeThickness"));
            return strokeThicknessAnima;
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
