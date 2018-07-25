using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Deanor.Pages
{
    /// <summary>
    /// Interaction logic for MainSection.xaml
    /// </summary>
    public partial class MainSection : UserControl
    {
        public MainSection()
        {
            InitializeComponent();
        }

        private void VsComputerMLBDown(object sender, MouseButtonEventArgs e)
        {
            App.SettingSection.VersusPlayer = false;
            App.Navigate(App.SettingPageKey);
        }

        private void VsPlayerMLBDown(object sender, MouseButtonEventArgs e)
        {
            App.SettingSection.VersusPlayer = true;
            App.Navigate(App.SettingPageKey);
        }

        private void HelpMLBDown(object sender, MouseButtonEventArgs e)
        {
            App.Navigate(App.HelpPageKey);
        }

        private void ExitMLBDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }
    }
}
