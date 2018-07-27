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

namespace Experiment.Pages
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : UserControl
    {
        private static GameSettingSection gameSettingSection;
        public static GameSettingSection GameSettingSection
        {
            get
            {
                if (gameSettingSection == null)
                {
                    gameSettingSection = new GameSettingSection();
                    Grid.SetRow(gameSettingSection, 1);
                }
                return gameSettingSection;
            }
        }

        private static MainSection mainSection;
        public static MainSection MainSection
        {
            get
            {
                if (mainSection == null)
                {
                    mainSection = new MainSection();
                    Grid.SetRow(mainSection, 1);
                }
                return mainSection;
            }
        }

        public MainPage()
        {
            InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        public const string MainSectionKey = "mainSection";
        public const string GameSettingSectionKey = "gameSettingSection";

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            Navigate(CurrentPage);
        }

        private string currentPage;
        public string CurrentPage
        {
            get { return currentPage; }
        }

        public void Navigate(string page)
        {
            if(string.IsNullOrEmpty(page))
            {
                mainGrid.Children.Add(MainSection);
                currentPage = MainSectionKey;
                return;
            }
            if (page == CurrentPage) return;
            switch (page)
            {
                case MainSectionKey:
                    // Navigate from GameSetting to MainSection
                    if(CurrentPage == GameSettingSectionKey)
                    {
                        if (!mainGrid.Children.Contains(MainSection))
                            mainGrid.Children.Add(MainSection);
                        var sb = Anima.TransitionRight(mainGrid, GameSettingSection, MainSection);
                        sb.Completed += (o, e) => { mainGrid.Children.Remove(GameSettingSection); };
                        sb.Begin();
                    }
                    break;
                case GameSettingSectionKey:
                    // Navigate from MainSection to GameSetting
                    if (CurrentPage == MainSectionKey)
                    {
                        if (!mainGrid.Children.Contains(GameSettingSection))
                            mainGrid.Children.Add(GameSettingSection);
                        var sb = Anima.TransitionLeft(mainGrid, MainSection, GameSettingSection);
                        sb.Completed += (o, e) => { mainGrid.Children.Remove(MainSection); };
                        sb.Begin();
                    }
                    break;
            }
            currentPage = page;
        }
    }
}
