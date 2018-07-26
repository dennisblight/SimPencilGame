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

namespace Deanor.Scene
{
    /// <summary>
    /// Interaction logic for PlayerLoseScene.xaml
    /// </summary>
    public partial class PlayerLoseScene : UserControl, IScene
    {
        public PlayerLoseScene()
        {
            InitializeComponent();
        }

        public UserControl Element => this;

        public void CloseScene()
        {
            if (_sceneAnima != null)
            {
                _sceneAnima.Stop();
                _sceneAnima.Remove();

                var sb = new Storyboard();

                var opacityAnima = new DoubleAnimation();
                opacityAnima.To = 0;
                Storyboard.SetTarget(opacityAnima, this);
                Storyboard.SetTargetProperty(opacityAnima, new PropertyPath("Opacity"));

                sb.Children.Add(opacityAnima);

                sb.Completed += (o, e) =>
                {
                    sb.Remove();
                    IsHitTestVisible = false;
                };

                sb.SpeedRatio = 3;

                sb.Begin();
            }
        }

        private Storyboard _sceneAnima;
        public void ShowScene()
        {
            var sb1 = new Storyboard();

            var opacityAnima = new DoubleAnimation();
            opacityAnima.To = 1;
            Storyboard.SetTarget(opacityAnima, this);
            Storyboard.SetTargetProperty(opacityAnima, new PropertyPath("Opacity"));

            var thumbDownAnima = new DoubleAnimation();
            thumbDownAnima.From = -100;
            thumbDownAnima.To = 0;
            Storyboard.SetTarget(thumbDownAnima, thumbDown);
            Storyboard.SetTargetProperty(thumbDownAnima, new PropertyPath("RenderTransform.Y"));

            sb1.SpeedRatio = 3;

            sb1.Children.Add(opacityAnima);
            sb1.Children.Add(thumbDownAnima);

            var sb2 = new Storyboard();

            var messageAnima = new DoubleAnimation();
            messageAnima.To = 1;
            Storyboard.SetTarget(messageAnima, message);
            Storyboard.SetTargetProperty(messageAnima, new PropertyPath("RenderTransform.ScaleX"));

            sb2.BeginTime = TimeSpan.FromMilliseconds(500);
            sb2.SpeedRatio = 3;

            sb2.Children.Add(messageAnima);

            var sb = new Storyboard();
            sb.Children.Add(sb1);
            sb.Children.Add(sb2);

            _sceneAnima = sb;
            IsHitTestVisible = true;
            sb.Begin();
        }
    }
}
