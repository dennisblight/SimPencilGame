using Deanor.AI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for SettingSection.xaml
    /// </summary>
    public partial class SettingSection : UserControl, IGameParameters
    {
        private int verticeCount = 6;
        private int aiLevel = -1;
        private bool player1First = true;

        public bool VersusPlayer { get; set; }

        public int VerticeCount
        {
            get { return verticeCount; }
            set
            {
                verticeCount = value;
                (verticeCountRBG.Children[value - 4] as RadioButton).IsChecked = true;
            }
        }

        public int AILevel
        {
            get { return aiLevel; }
            set
            {
                aiLevel = value;
                if (aiLevel == 0) rbAiLevel0.IsChecked = true;
                else if (aiLevel == 1) rbAiLevel1.IsChecked = true;
                else if (aiLevel == 2) rbAiLevel2.IsChecked = true;
            }
        }

        public IGameAI GameAI
        {
            get
            {
                if(! VersusPlayer)
                {
                    if (AILevel == 0) return new NoobAI();
                    if (AILevel == 1) return new NormalAI();
                    if (AILevel == 2) return new ProAI();
                }
                return null;
            }
        }

        public bool Player1First
        {
            get { return player1First; }
            set
            {
                player1First = value;
                if (player1First) rbPlayer1First.IsChecked = true;
                else rbPlayer2First.IsChecked = true;
            }
        }

        public Color Player1Color
        {
            get
            {
                return firstColorPicker.PickedColor;
            }
        }

        public Color Player2Color
        {
            get
            {
                return secondColorPicker.PickedColor;
            }
        }

        public int Player1ColorIndex
        {
            get => firstColorPicker.PickedColorIndex;
            set => firstColorPicker.PickedColorIndex = value;
        }

        public int Player2ColorIndex
        {
            get => secondColorPicker.PickedColorIndex;
            set => secondColorPicker.PickedColorIndex = value;
        }

        public string Player1Name => VersusPlayer ? "Player 1" : "Player";

        public string Player2Name => VersusPlayer ? "Player 2" : GameAI.Name;

        public SettingSection()
        {
            InitializeComponent();
        }

        private void VerticeCountRadioButtonChecked(object sender, RoutedEventArgs e)
        {
            var rb = (sender as RadioButton);
            verticeCount = int.Parse(rb.Content.ToString());
        }

        private void AiLevelRadioButtonChecked(object sender, RoutedEventArgs e)
        {
            var rb = (sender as RadioButton);
            if (rb == rbAiLevel0) aiLevel = 0;
            if (rb == rbAiLevel1) aiLevel = 1;
            if (rb == rbAiLevel2) aiLevel = 2;
        }

        private void PlayFirstChecked(object sender, RoutedEventArgs e)
        {
            var rb = (sender as RadioButton);
            if (rb == rbPlayer1First) player1First = true;
            if (rb == rbPlayer2First) player1First = false;
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            App.Navigate(App.MainPageKey);
        }

        private void PlayButtonClick(object sender, RoutedEventArgs e)
        {
            App.Navigate(App.GamePageKey);
        }
    }

    public interface IGameParameters
    {
        Color Player1Color { get; }
        Color Player2Color { get; }
        int Player1ColorIndex { get; set; }
        int Player2ColorIndex { get; set; }
        int AILevel { get; set; }
        int VerticeCount { get; set; }
        IGameAI GameAI { get; }
        bool VersusPlayer { get; set; }
        bool Player1First { get; set; }
        string Player1Name { get; }
        string Player2Name { get; }
    }
}
