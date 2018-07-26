using Deanor.AI;
using Deanor.Controls;
using Deanor.Pages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Deanor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public const string MainPageKey = "mainPage";
        public const string SettingPageKey = "settingPage";
        public const string GamePageKey = "gamePage";
        public const string HelpPageKey = "helpPage";

        public static IGameParameters GameParameters
        {
            get { return SettingSection as IGameParameters; }
        }

        private static GamePage gamePage;
        public static GamePage GamePage
        {
            get
            {
                if (gamePage == null)
                    gamePage = new GamePage();
                return gamePage;
            }
        }

        private static MainPage mainPage;
        public static MainPage MainPage
        {
            get
            {
                if (mainPage == null)
                    mainPage = new MainPage();
                return mainPage;
            }
        }

        private static MainSection mainSection;
        public static MainSection MainSection
        {
            get
            {
                if (mainSection == null)
                    mainSection = new MainSection();
                return mainSection;
            }
        }

        private static SettingSection settingSection;
        public static SettingSection SettingSection
        {
            get
            {
                if (settingSection == null)
                    settingSection = new SettingSection();
                return settingSection;
            }
        }

        private static string currentPage = null;

        public static void Navigate(string pageKey)
        {
            var cw = (Current.MainWindow as MainWindow);
            if (currentPage == null)
            {
                MainPage.contentGrid.Children.Add(MainSection);
                MainPage.contentGrid.Children.Add(SettingSection);
                cw.mainGrid.Children.Add(MainPage);
                cw.mainGrid.Children.Add(GamePage);
                (GamePage.RenderTransform as TranslateTransform).Y = -cw.ActualHeight;
                (SettingSection.RenderTransform as TranslateTransform).X = cw.ActualWidth;
                var rand = new Random();
                var r1 = rand.Next(18);
                var r2 = rand.Next(17);
                var r3 = rand.Next(7);
                GameParameters.Player1ColorIndex = r1;
                GameParameters.Player2ColorIndex = r2 < r1 ? r2 : (r2 + 1);
                GameParameters.VerticeCount = (r3 + 4);
                GameParameters.AILevel = 1;
                currentPage = pageKey;
            }
            else if(pageKey != currentPage)
            {
                switch(currentPage)
                {
                    case MainPageKey:
                        {
                            switch(pageKey)
                            {
                                case SettingPageKey:
                                    if(GameParameters.VersusPlayer)
                                    {
                                        SettingSection.aiLevelContainer.Visibility = Visibility.Hidden;
                                        SettingSection.firstTextBlock.Text = "Player 1";
                                        SettingSection.secondTextBlock.Text = "Player 2";
                                        Grid.SetColumnSpan(SettingSection.verticeCountContainer, 2);
                                    }
                                    else
                                    {
                                        SettingSection.aiLevelContainer.Visibility = Visibility.Visible;
                                        SettingSection.firstTextBlock.Text = "Player";
                                        SettingSection.secondTextBlock.Text = "Computer";
                                        Grid.SetColumnSpan(SettingSection.verticeCountContainer, 1);
                                    }
                                    TransitionLeft(MainPage.contentGrid, MainSection, SettingSection).Begin();
                                    break;
                                case HelpPageKey:
                                    /* Not Implemented Yet */
                                    break;
                            }
                        }
                        break;
                    case SettingPageKey:
                        {
                            switch (pageKey)
                            {
                                case MainPageKey:
                                    TransitionRight(MainPage.contentGrid, SettingSection, MainSection).Begin();
                                    break;
                                case GamePageKey:
                                    var anima = TransitionDown(cw.mainGrid, MainPage, GamePage);
                                    GamePage.CreateGameControl();
                                    anima.Completed += (o, e) => GamePage.Render();
                                    anima.Begin();
                                    break;
                            }
                        }
                        break;
                    case GamePageKey:
                        {
                            switch (pageKey)
                            {
                                case MainPageKey:
                                    (MainSection.RenderTransform as TranslateTransform).X = 0;
                                    (SettingSection.RenderTransform as TranslateTransform).X = cw.mainGrid.ActualWidth;
                                    TransitionUp(cw.mainGrid, GamePage, MainPage).Begin();
                                    break;
                                case SettingPageKey:
                                    (MainSection.RenderTransform as TranslateTransform).X = -cw.mainGrid.ActualWidth;
                                    (SettingSection.RenderTransform as TranslateTransform).X = 0;
                                    TransitionUp(cw.mainGrid, GamePage, MainPage).Begin();
                                    break;
                            }
                        }
                        break;
                    case HelpPageKey:
                        {
                            switch (pageKey)
                            {
                                case MainPageKey:
                                    break;
                            }
                        }
                        break;
                }
                currentPage = pageKey;
                Debug.WriteLine(currentPage);
            }
        }

        private static readonly Duration NormalDuration = new Duration(TimeSpan.FromMilliseconds(500));

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
