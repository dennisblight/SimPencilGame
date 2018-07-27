using Experiment.Pages;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Self => Application.Current.MainWindow as MainWindow;

        private static MainPage mainPage;
        public static MainPage MainPage
        {
            get
            {
                if (mainPage == null)
                {
                    mainPage = new MainPage();
                }
                return mainPage;
            }
        }

        private static HelpPage helpPage;
        public static HelpPage HelpPage
        {
            get
            {
                if (helpPage == null)
                {
                    helpPage = new HelpPage();
                }
                return helpPage;
            }
        }

        private static GamePage gamePage;
        public static GamePage GamePage
        {
            get
            {
                if (gamePage == null)
                {
                    gamePage = new GamePage();
                }
                return gamePage;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Navigate(currentPage);
        }

        public const string MainPageKey = "mainPage";
        public const string HelpPageKey = "helpPage";
        public const string RoomPageKey = "roomPage";
        public const string GamePageKey = "gamePage";
        public const string GameSettingPageKey = "gameSettingPage";

        private string currentPage;
        public string CurrentPage
        {
            get { return currentPage; }
        }

        public void Navigate(string page)
        {
            if (string.IsNullOrEmpty(page))
            {
                mainGrid.Children.Add(MainPage);
                currentPage = MainPageKey;
                return;
            }
            if (page == CurrentPage) return;
            
            switch(page)
            {
                case MainPageKey:
                    // Navigate from GamePage to MainPage
                    if (CurrentPage == GamePageKey)
                    {
                        MainPage.Navigate(MainPage.MainSectionKey);
                        if (!mainGrid.Children.Contains(MainPage))
                            mainGrid.Children.Add(MainPage);
                        var sb = Anima.TransitionUp(mainGrid, GamePage, MainPage);
                        sb.Completed += (o, e) => { mainGrid.Children.Remove(GamePage); };
                        sb.Begin();
                    }
                    // Navigate from HelpPage to MainPage
                    else if (CurrentPage == HelpPageKey)
                    {
                        if (!mainGrid.Children.Contains(MainPage))
                            mainGrid.Children.Add(MainPage);
                        var sb = Anima.TransitionDown(mainGrid, HelpPage, MainPage);
                        sb.Completed += (o, e) => { mainGrid.Children.Remove(HelpPage); };
                        sb.Begin();
                    }
                    break;
                case HelpPageKey:
                    // Navigate from MainPage to HelpPage
                    if (CurrentPage == MainPageKey)
                    {
                        if (!mainGrid.Children.Contains(HelpPage))
                            mainGrid.Children.Add(HelpPage);
                        var sb = Anima.TransitionUp(mainGrid, MainPage, HelpPage);
                        sb.Completed += (o, e) => { mainGrid.Children.Remove(MainPage); };
                        sb.Begin();
                    }
                    break;
                case GameSettingPageKey:
                    // Navigate from MainPage to GameSetting
                    if (CurrentPage == MainPageKey)
                    {
                        MainPage.Navigate(MainPage.GameSettingSectionKey);
                    }
                    // Navigate from GamePage to GameSetting
                    else if (CurrentPage == GamePageKey)
                    {
                        MainPage.Navigate(MainPage.GameSettingSectionKey);
                        if (!mainGrid.Children.Contains(MainPage))
                            mainGrid.Children.Add(MainPage);
                        var sb = Anima.TransitionUp(mainGrid, GamePage, MainPage);
                        sb.Completed += (o, e) => { mainGrid.Children.Remove(GamePage); };
                        sb.Begin();
                    }
                    break;
                case GamePageKey:
                    // Navigate from MainPage to GamePage
                    if (CurrentPage == GameSettingPageKey || CurrentPage == MainPageKey)
                    {
                        if (!mainGrid.Children.Contains(GamePage))
                            mainGrid.Children.Add(GamePage);
                        var sb = Anima.TransitionDown(mainGrid, MainPage, GamePage);
                        sb.Completed += (o, e) => { mainGrid.Children.Remove(MainPage); };
                        sb.Begin();
                    }
                    break;
            }
            currentPage = page;
        }
    }
}
