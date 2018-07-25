using Deanor.Media;
using Deanor.Structure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public void ApplyColor(Color color)
        {
            //(Foreground as SolidColorBrush).Color = color;
            Foreground = new SolidColorBrush(color);
            originalColor = color;
        }

        public void ApplyPreviewColor()
        {
            if(PreviewColor.HasValue)
            {
                Foreground = new SolidColorBrush(PreviewColor.Value);
                //(Foreground as SolidColorBrush).Color = PreviewColor.Value;
                originalColor = PreviewColor.Value;
            }
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

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);
            switch (InputState)
            {
                case InputState.Hovered:
                    InputState = InputState.Clicked;
                    break;
            }
        }

        protected override void OnPreviewTouchDown(TouchEventArgs e)
        {
            base.OnPreviewTouchDown(e);
            switch (InputState)
            {
                case InputState.Hovered:
                    InputState = InputState.Clicked;
                    break;
            }
        }

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonUp(e);
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

        protected override void OnPreviewTouchUp(TouchEventArgs e)
        {
            base.OnPreviewTouchUp(e);
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
            var sb = ToOriginAnima();
            //if (PreviewColor != null)
            //{
            //    sb.Children.Add(Anima.ForegroundAnimation(originalColor, this, Anima.BlinkDuration));
            //}
            sb.Begin();
        }

        private void Hovered()
        {
            var sb = ToHoveredAnima();
            //if (PreviewColor.HasValue)
            //{
            //    sb.Children.Add(Anima.ForegroundAnimation(originalColor, this, Anima.BlinkDuration));
            //}
            sb.Begin();
        }

        private void Clicked()
        {
            var sb = ToHighlightAnima();
            //if (PreviewColor != null)
            //{
            //    sb.Children.Add(Anima.ForegroundAnimation(PreviewColor.Value, this, Anima.BlinkDuration));
            //}
            sb.Begin();
        }

        private void Dragged()
        {
            var sb = ToHighlightAnima();
            //if (PreviewColor != null)
            //{
            //    sb.Children.Add(Anima.ForegroundAnimation(PreviewColor.Value, this, Anima.BlinkDuration));
            //}
            sb.Begin();
        }

        public Storyboard ToOriginAnima()
        {
            var sb = new Storyboard();
            sb.Children.Add(Anima.ScaleAnimation(TopRatioOrigin, topEllipse, Anima.BlinkDuration));
            sb.Children.Add(Anima.ScaleAnimation(MiddleRatioOrigin, middleEllipse, Anima.BlinkDuration));
            sb.Children.Add(Anima.ScaleAnimation(BottomRatioOrigin, bottomEllipse, Anima.BlinkDuration));
            if (PreviewColor != null)
            {
                sb.Children.Add(Anima.ForegroundAnimation(originalColor, this, Anima.BlinkDuration));
            }
            return sb;
        }

        public Storyboard ToHighlightAnima()
        {
            var sb = new Storyboard();
            sb.Children.Add(Anima.ScaleAnimation(TopRatioAlternate, topEllipse, Anima.BlinkDuration));
            sb.Children.Add(Anima.ScaleAnimation(MiddleRatioOrigin, middleEllipse, Anima.BlinkDuration));
            sb.Children.Add(Anima.ScaleAnimation(BottomRatioAlternate, bottomEllipse, Anima.BlinkDuration));
            if (PreviewColor != null)
            {
                sb.Children.Add(Anima.ForegroundAnimation(PreviewColor.Value, this, Anima.BlinkDuration));
            }
            return sb;
        }

        public Storyboard ToHoveredAnima()
        {
            var sb = new Storyboard();
            sb.Children.Add(Anima.ScaleAnimation(TopRatioOrigin, topEllipse, Anima.BlinkDuration));
            sb.Children.Add(Anima.ScaleAnimation(MiddleRatioAlternate, middleEllipse, Anima.BlinkDuration));
            sb.Children.Add(Anima.ScaleAnimation(BottomRatioOrigin, bottomEllipse, Anima.BlinkDuration));
            if (PreviewColor.HasValue)
            {
                sb.Children.Add(Anima.ForegroundAnimation(originalColor, this, Anima.BlinkDuration));
            }
            return sb;
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
