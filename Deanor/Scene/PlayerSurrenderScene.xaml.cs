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
    /// Interaction logic for PlayerSurrenderScene.xaml
    /// </summary>
    public partial class PlayerSurrenderScene : UserControl, IScene
    {
        private Storyboard _sceneAnima;

        public PlayerSurrenderScene(string playerName, Color playerColor)
        {
            InitializeComponent();
            this.playerName.Text = playerName;
            this.playerName.Foreground = new SolidColorBrush(playerColor);
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

        public void ShowScene()
        {
            var sb = new Storyboard();

            var sb1 = new Storyboard();

            var opacityAnima = new DoubleAnimation();
            opacityAnima.To = 1;
            Storyboard.SetTarget(opacityAnima, this);
            Storyboard.SetTargetProperty(opacityAnima, new PropertyPath("Opacity"));

            var flagAnima = new DoubleAnimation();
            flagAnima.From = -100;
            flagAnima.To = 0;
            Storyboard.SetTarget(flagAnima, flag);
            Storyboard.SetTargetProperty(flagAnima, new PropertyPath("RenderTransform.Y"));

            sb1.SpeedRatio = 3;

            sb1.Children.Add(opacityAnima);
            sb1.Children.Add(flagAnima);

            var sb2 = new Storyboard();
            var sb3 = new Storyboard();

            var messageAnima = new DoubleAnimation();
            messageAnima.To = 1;
            Storyboard.SetTarget(messageAnima, message);
            Storyboard.SetTargetProperty(messageAnima, new PropertyPath("RenderTransform.ScaleX"));

            sb2.Children.Add(messageAnima);

            sb.Children.Add(sb1);
            sb.Children.Add(sb2);

            sb2.BeginTime = TimeSpan.FromMilliseconds(500);
            sb2.SpeedRatio = 3;

            _sceneAnima = sb;
            IsHitTestVisible = true;
            sb.Begin();
        }
    }
}
