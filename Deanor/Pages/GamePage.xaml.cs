using Deanor.Controls;
using Deanor.Scene;
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

namespace Deanor.Pages
{
    /// <summary>
    /// Interaction logic for GamePage.xaml
    /// </summary>
    public partial class GamePage : UserControl
    {
        private const double hexHeight = 40;

        public static double HexHeight => hexHeight;
        public static double HexWidth => hexHeight * Math.Sqrt(3) / 2;

        private IScene endGameScene;

        public static PointCollection HexPoints
        {
            get
            {
                var dp = DirectionHexVector(hexHeight / 2, 60);
                var origin = new Point(hexHeight * Math.Sqrt(3) / 4, hexHeight / 2);
                return new PointCollection()
                {
                    Point.Add(origin, dp[0]),
                    Point.Add(origin, dp[1]),
                    Point.Add(origin, dp[2]),
                    Point.Add(origin, dp[3]),
                    Point.Add(origin, dp[4]),
                    Point.Add(origin, dp[5]),
                };
            }
        }

        public static PathFigureCollection HexFigures
        {
            get
            {
                var hp = HexPoints;
                return new PathFigureCollection()
                {
                    new PathFigure(hp[0], new PathSegment[]
                    {
                        new LineSegment(hp[1], false),
                        new LineSegment(hp[2], false),
                        new LineSegment(hp[3], false),
                        new LineSegment(hp[4], false),
                        new LineSegment(hp[5], false),
                    }, true)
                };
            }
        }

        public GamePage()
        {
            InitializeComponent();
        }

        private void restartButton_Click(object sender, RoutedEventArgs e)
        {
            CreateGameControl();
            Render();
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            App.Navigate(App.SettingPageKey);
        }

        private void surrenderButton_Click(object sender, RoutedEventArgs e)
        {
            currentGameControl.Surrender();
        }

        private void GameControl_GamesEnd(object sender, GamesEndEventArgs args)
        {
            switch(args.GameResult.GameStatus)
            {
                case GameStatus.Player1Win:
                    if (App.GameParameters.VersusPlayer)
                    {
                        endGameScene = new PlayerWinScene(App.GameParameters.Player1Name, App.GameParameters.Player1Color);
                    }
                    else
                    {
                        endGameScene = new PlayerWinScene();
                    }
                    p1GameState.Text = "\xf091"; // Trophy Glyph
                    p1GameState.Foreground = Brushes.Gold;
                    p1GameState.Visibility = Visibility.Visible;
                    p1GameState.ToolTip = $"{App.GameParameters.Player1Name} Win!";
                    p2GameState.Visibility = Visibility.Hidden;
                    break;
                case GameStatus.Player2Win:
                    if (App.GameParameters.VersusPlayer)
                    {
                        endGameScene = new PlayerWinScene(App.GameParameters.Player2Name, App.GameParameters.Player2Color);
                    }
                    else
                    {
                        endGameScene = new PlayerLoseScene();
                    }
                    p2GameState.Text = "\xf091"; // Trophy Glyph
                    p2GameState.Foreground = Brushes.Gold;
                    p2GameState.Visibility = Visibility.Visible;
                    p2GameState.ToolTip = $"{App.GameParameters.Player2Name} Win!";
                    p1GameState.Visibility = Visibility.Hidden;
                    break;
                case GameStatus.Player1Surrender:
                    endGameScene = new PlayerSurrenderScene(App.GameParameters.Player1Name, App.GameParameters.Player1Color);
                    p1GameState.Text = "\xf024"; // Flag Glyph
                    p1GameState.Foreground = Brushes.White;
                    p1GameState.Visibility = Visibility.Visible;
                    p1GameState.ToolTip = $"{App.GameParameters.Player1Name} Surrendered..";
                    p2GameState.Visibility = Visibility.Hidden;
                    break;
                case GameStatus.Player2Surrender:
                    endGameScene = new PlayerSurrenderScene(App.GameParameters.Player2Name, App.GameParameters.Player2Color);
                    p1GameState.Text = "\xf024"; // Flag Glyph
                    p1GameState.Foreground = Brushes.White;
                    p1GameState.Visibility = Visibility.Visible;
                    p1GameState.ToolTip = $"{App.GameParameters.Player2Name} Surrendered..";
                    p2GameState.Visibility = Visibility.Hidden;
                    break;
                case GameStatus.Draw:
                    endGameScene = new DrawScene();
                    break;
            }
            surrenderButton.Visibility = Visibility.Hidden;
            restartButton.Visibility = Visibility.Visible;
            backButton.Visibility = Visibility.Visible;
            if (endGameScene != null)
            {
                MountScene();
                endGameScene.ShowScene();
                Debug.WriteLine("scene shown");
            }
        }

        private void GameControl_TurnsEnd(object sender, RoutedEventArgs e)
        {
            EndTurnsAnima().Begin();
            if(currentGameControl!= null &&! App.GameParameters.VersusPlayer)
            {
                if(currentGameControl.IsPlayer1Turn)
                    surrenderButton.IsEnabled = true;
                else
                    surrenderButton.IsEnabled = false;
            }
        }

        private void MountScene()
        {
            Grid.SetRowSpan(endGameScene.Element, 3);
            Panel.SetZIndex(endGameScene.Element, 10);
            endGameScene.Element.MouseLeftButtonDown += scene_MouseLeftButtonDown;
            mainContainer.Children.Add(endGameScene.Element);
        }

        private void scene_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            endGameScene.CloseScene();
        }

        private GameControl GameControl => mainContainer.Children.OfType<GameControl>().First();
        private GameControl currentGameControl;

        private static Vector[] DirectionHexVector(double d, double offsetDegrees)
        {
            const double d01 = Math.PI / 180;
            const double d60 = Math.PI / 3;
            var o = offsetDegrees * d01;
            
            return new Vector[]
            {
                new Vector(d * Math.Sin(o + 0 * d60), d * Math.Cos(o + 0 * d60)),
                new Vector(d * Math.Sin(o + 1 * d60), d * Math.Cos(o + 1 * d60)),
                new Vector(d * Math.Sin(o + 2 * d60), d * Math.Cos(o + 2 * d60)),
                new Vector(d * Math.Sin(o + 3 * d60), d * Math.Cos(o + 3 * d60)),
                new Vector(d * Math.Sin(o + 4 * d60), d * Math.Cos(o + 4 * d60)),
                new Vector(d * Math.Sin(o + 5 * d60), d * Math.Cos(o + 5 * d60)),
            };
        }

        public void CreateGameControl()
        {
            var p1Color = App.GameParameters.Player1Color;
            var p2Color = App.GameParameters.Player2Color;
            p1StateFill.Fill = new SolidColorBrush(p1Color);
            p2StateFill.Fill = new SolidColorBrush(p2Color);
            p1StateOutline.Stroke = new SolidColorBrush(Color.Multiply(p1Color, 1.4f));
            p2StateOutline.Stroke = new SolidColorBrush(Color.Multiply(p2Color, 1.4f));
            p1Name.Text = App.GameParameters.Player1Name;
            p2Name.Text = App.GameParameters.Player2Name;
            if (App.GameParameters.Player1First)
            {
                p1StateGlyph.Foreground = new SolidColorBrush(Color.Multiply(p1Color, 1.7f));
                p1StateFill.RenderTransform = new ScaleTransform(1, 1);
                p2StateGlyph.Foreground = new SolidColorBrush(Color.Multiply(p2Color, 0.6f));
                p2StateFill.RenderTransform = new ScaleTransform(0, 0);
            }
            else
            {
                p2StateGlyph.Foreground = new SolidColorBrush(Color.Multiply(p2Color, 1.7f));
                p2StateFill.RenderTransform = new ScaleTransform(1, 1);
                p1StateGlyph.Foreground = new SolidColorBrush(Color.Multiply(p1Color, 0.6f));
                p1StateFill.RenderTransform = new ScaleTransform(0, 0);
            }
            p1StateGlyph.Text = "\xf007";
            p2StateGlyph.Text = App.GameParameters.VersusPlayer ? "\xf007" : "\xf108";
            if (currentGameControl != null)
                mainContainer.Children.Remove(currentGameControl);
            currentGameControl = null;
            currentGameControl = new GameControl()
            {
                SpreadDistance = 180,
                VerticesSize = 27,
                Foreground = new SolidColorBrush((Color)Application.Current.Resources["Gray01"]),
                Background = Application.Current.Resources["SuedeTextureBrush"] as Brush,
                Margin = new Thickness(0, 80, 0, 0),
                VerticesCount = App.GameParameters.VerticeCount,
                IsPlayer1Turn = App.GameParameters.Player1First,
                Player1Color = App.GameParameters.Player1Color,
                Player2Color = App.GameParameters.Player2Color,
                AIPlayer = App.GameParameters.GameAI,
            };
            currentGameControl.GamesEnd += GameControl_GamesEnd;
            currentGameControl.TurnsEnd += GameControl_TurnsEnd;
            mainContainer.Children.Add(currentGameControl);
            Grid.SetRowSpan(currentGameControl, 2);
            restartButton.Visibility = Visibility.Hidden;
            backButton.Visibility = Visibility.Hidden;
            surrenderButton.Visibility = Visibility.Visible;
            p1GameState.Visibility = Visibility.Hidden;
            p2GameState.Visibility = Visibility.Hidden;
            surrenderButton.IsEnabled = true;
            if (!App.GameParameters.VersusPlayer && !App.GameParameters.Player1First)
                surrenderButton.IsEnabled = false;
            if (endGameScene != null && mainContainer.Children.Contains(endGameScene.Element))
                mainContainer.Children.Remove(endGameScene.Element);
            endGameScene = null;
        }

        public void Render()
        {
            if(currentGameControl != null)
                currentGameControl.Render();
        }

        private Storyboard EndTurnsAnima()
        {
            var sb = new Storyboard();

            if(currentGameControl != null)
            {
                var p1FillXAnima = new DoubleAnimation();
                p1FillXAnima.To = GameControl.IsPlayer1Turn ? 1 : 0;
                Storyboard.SetTarget(p1FillXAnima, p1StateFill);
                Storyboard.SetTargetProperty(p1FillXAnima, new PropertyPath("RenderTransform.ScaleX"));
                sb.Children.Add(p1FillXAnima);

                var p1FillYAnima = new DoubleAnimation();
                p1FillYAnima.To = GameControl.IsPlayer1Turn ? 1 : 0;
                Storyboard.SetTarget(p1FillYAnima, p1StateFill);
                Storyboard.SetTargetProperty(p1FillYAnima, new PropertyPath("RenderTransform.ScaleY"));
                sb.Children.Add(p1FillYAnima);

                var p2FillXAnima = new DoubleAnimation();
                p2FillXAnima.To = GameControl.IsPlayer1Turn ? 0 : 1;
                Storyboard.SetTarget(p2FillXAnima, p2StateFill);
                Storyboard.SetTargetProperty(p2FillXAnima, new PropertyPath("RenderTransform.ScaleX"));
                sb.Children.Add(p2FillXAnima);

                var p2FillYAnima = new DoubleAnimation();
                p2FillYAnima.To = GameControl.IsPlayer1Turn ? 0 : 1;
                Storyboard.SetTarget(p2FillYAnima, p2StateFill);
                Storyboard.SetTargetProperty(p2FillYAnima, new PropertyPath("RenderTransform.ScaleY"));
                sb.Children.Add(p2FillYAnima);

                var p1GlyphAnima = new ColorAnimation();
                p1GlyphAnima.To = Color.Multiply(GameControl.Player1Color.Value, GameControl.IsPlayer1Turn ? 1.7f : 0.6f);
                Storyboard.SetTarget(p1GlyphAnima, p1StateGlyph);
                Storyboard.SetTargetProperty(p1GlyphAnima, new PropertyPath("Foreground.Color"));
                sb.Children.Add(p1GlyphAnima);

                var p2GlyphAnima = new ColorAnimation();
                p2GlyphAnima.To = Color.Multiply(GameControl.Player2Color.Value, GameControl.IsPlayer1Turn ? 0.6f : 1.7f);
                Storyboard.SetTarget(p2GlyphAnima, p2StateGlyph);
                Storyboard.SetTargetProperty(p2GlyphAnima, new PropertyPath("Foreground.Color"));
                sb.Children.Add(p2GlyphAnima);

                sb.SpeedRatio = 5;
            }
            return sb;
        }
    }
}
