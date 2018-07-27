using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Experiment
{
    public class Anima
    {
        public static readonly Duration NormalDuration = new Duration(TimeSpan.FromMilliseconds(500));

        public static Storyboard TransitionLeft(Panel panel, UserControl source, UserControl target)
        {
            var sb = new Storyboard();
            var trx1 = new DoubleAnimation(source.ActualWidth, 0, NormalDuration);
            var trx2 = new DoubleAnimation(0, -panel.ActualWidth, NormalDuration);
            Storyboard.SetTarget(trx1, target);
            Storyboard.SetTarget(trx2, source);
            Storyboard.SetTargetProperty(trx1, new PropertyPath("RenderTransform.X"));
            Storyboard.SetTargetProperty(trx2, new PropertyPath("RenderTransform.X"));
            sb.Children.Add(trx1);
            sb.Children.Add(trx2);
            return sb;
        }

        public static Storyboard TransitionRight(Panel panel, UserControl source, UserControl target)
        {
            var sb = new Storyboard();
            var trx1 = new DoubleAnimation(-source.ActualWidth, 0, NormalDuration);
            var trx2 = new DoubleAnimation(0, panel.ActualWidth, NormalDuration);
            Storyboard.SetTarget(trx1, target);
            Storyboard.SetTarget(trx2, source);
            Storyboard.SetTargetProperty(trx1, new PropertyPath("RenderTransform.X"));
            Storyboard.SetTargetProperty(trx2, new PropertyPath("RenderTransform.X"));
            sb.Children.Add(trx1);
            sb.Children.Add(trx2);
            return sb;
        }

        public static Storyboard TransitionDown(Panel panel, UserControl source, UserControl target)
        {
            var sb = new Storyboard();
            var try1 = new DoubleAnimation(-panel.ActualHeight, 0, NormalDuration);
            var try2 = new DoubleAnimation(0, panel.ActualHeight, NormalDuration);
            Storyboard.SetTarget(try1, target);
            Storyboard.SetTarget(try2, source);
            Storyboard.SetTargetProperty(try1, new PropertyPath("RenderTransform.Y"));
            Storyboard.SetTargetProperty(try2, new PropertyPath("RenderTransform.Y"));
            sb.Children.Add(try1);
            sb.Children.Add(try2);
            return sb;
        }

        public static Storyboard TransitionUp(Panel panel, UserControl source, UserControl target)
        {
            var sb = new Storyboard();
            var try1 = new DoubleAnimation(panel.ActualHeight, 0, NormalDuration);
            var try2 = new DoubleAnimation(0, -panel.ActualHeight, NormalDuration);
            Storyboard.SetTarget(try1, target);
            Storyboard.SetTarget(try2, source);
            Storyboard.SetTargetProperty(try1, new PropertyPath("RenderTransform.Y"));
            Storyboard.SetTargetProperty(try2, new PropertyPath("RenderTransform.Y"));
            sb.Children.Add(try1);
            sb.Children.Add(try2);
            return sb;
        }
    }
}
