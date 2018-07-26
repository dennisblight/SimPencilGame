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

namespace Deanor.Scene
{
    /// <summary>
    /// Interaction logic for PlayerWinScene.xaml
    /// </summary>
    public partial class PlayerWinScene : UserControl, IScene
    {
        public PlayerWinScene()
        {
            InitializeComponent();
        }

        public PlayerWinScene(string playerName, Color playerColor)
            : this()
        {
            PlayerName = playerName;
            PlayerColor = playerColor;
            VersusPlayer = true;
        }

        public UserControl Element => this;
        
        public bool VersusPlayer { get; set; }
        public string PlayerName { get; set; }
        public Color PlayerColor { get; set; }

        private Storyboard _sceneAnima;
        public void ShowScene()
        {
            var msg = VersusPlayer ? message2 : message;
            if(VersusPlayer)
            {
                playerName.Text = PlayerName;
                playerName.Foreground = new SolidColorBrush(PlayerColor);
                message.Visibility = Visibility.Hidden;
                message2.Visibility = Visibility.Visible;
            }
            else
            {
                message2.Visibility = Visibility.Hidden;
                message.Visibility = Visibility.Visible;
            }

            var sb = new Storyboard();

            var sb1 = new Storyboard();

            var opacityAnima = new DoubleAnimation();
            opacityAnima.To = 1;
            Storyboard.SetTarget(opacityAnima, this);
            Storyboard.SetTargetProperty(opacityAnima, new PropertyPath("Opacity"));

            var trophyAnima = new DoubleAnimation();
            trophyAnima.From = -100;
            trophyAnima.To = 0;
            Storyboard.SetTarget(trophyAnima, trophy);
            Storyboard.SetTargetProperty(trophyAnima, new PropertyPath("RenderTransform.Y"));

            sb1.SpeedRatio = 3;

            sb1.Children.Add(opacityAnima);
            sb1.Children.Add(trophyAnima);

            var sb2 = new Storyboard();
            var sb3 = new Storyboard();

            var messageAnima = new DoubleAnimation();
            messageAnima.To = 1;
            Storyboard.SetTarget(messageAnima, msg);
            Storyboard.SetTargetProperty(messageAnima, new PropertyPath("RenderTransform.ScaleX"));

            var dv = DirectionVector(150);
            var stars = new TextBlock[] { star1, star2, star3, star4, star5 };

            sb2.Children.Add(messageAnima);

            for (int i = 0; i < 5; i++)
            {
                var starTrXAnima = new DoubleAnimation();
                starTrXAnima.To = dv[i].X - stars[i].ActualWidth / 2;
                Storyboard.SetTarget(starTrXAnima, stars[i]);
                Storyboard.SetTargetProperty(starTrXAnima, new PropertyPath("(Canvas.Left)"));

                var starTrYAnima = new DoubleAnimation();
                starTrYAnima.To = dv[i].Y - stars[i].ActualHeight / 2;
                Storyboard.SetTarget(starTrYAnima, stars[i]);
                Storyboard.SetTargetProperty(starTrYAnima, new PropertyPath("(Canvas.Top)"));

                var starScXAnima = new DoubleAnimation();
                starScXAnima.To = 2;
                Storyboard.SetTarget(starScXAnima, stars[i]);
                Storyboard.SetTargetProperty(starScXAnima, new PropertyPath("RenderTransform.Children[0].ScaleX"));

                var starScYAnima = new DoubleAnimation();
                starScYAnima.To = 2;
                Storyboard.SetTarget(starScYAnima, stars[i]);
                Storyboard.SetTargetProperty(starScYAnima, new PropertyPath("RenderTransform.Children[0].ScaleY"));

                sb2.Children.Add(starTrXAnima);
                sb2.Children.Add(starTrYAnima);
                sb2.Children.Add(starScXAnima);
                sb2.Children.Add(starScYAnima);

                var starRotAnima = new DoubleAnimation();
                starRotAnima.To = 360;
                starRotAnima.RepeatBehavior = RepeatBehavior.Forever;
                Storyboard.SetTarget(starRotAnima, stars[i]);
                Storyboard.SetTargetProperty(starRotAnima, new PropertyPath("RenderTransform.Children[1].Angle"));

                sb3.Children.Add(starRotAnima);
            }

            sb2.BeginTime = TimeSpan.FromMilliseconds(500);
            sb2.SpeedRatio = 3;

            //sb3.RepeatBehavior = RepeatBehavior.Forever;
            sb3.SpeedRatio = 1;

            sb.Children.Add(sb1);
            sb.Children.Add(sb2);
            sb.Children.Add(sb3);

            _sceneAnima = sb;
            IsHitTestVisible = true;
            sb.Begin();
        }

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

        private Vector[] DirectionVector(double d)
        {
            const double d45 = 0.78539816339744830961566084581988;
            return new Vector[]
            {
                new Vector(d * Math.Cos(d45 * 0), -d * Math.Sin(d45 * 0)),
                new Vector(d * Math.Cos(d45 * 1), -d * Math.Sin(d45 * 1)),
                new Vector(d * Math.Cos(d45 * 2), -d * Math.Sin(d45 * 2)),
                new Vector(d * Math.Cos(d45 * 3), -d * Math.Sin(d45 * 3)),
                new Vector(d * Math.Cos(d45 * 4), -d * Math.Sin(d45 * 4)),
            };
        }
    }
}
