﻿using System;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Experiment.Pages
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.MainPage.Navigate(MainPage.GameSettingSectionKey);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow.Self.Navigate(MainWindow.HelpPageKey);
        }

        private void HexButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.MainPage.Navigate(MainPage.GameSettingSectionKey);
        }
    }
}
