﻿//BS"D

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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL_WPF.TEMPLATE
{
    /// <summary>
    /// Interaction logic for OldLoginWindow.xaml
    /// </summary>
    public partial class OldLoginWindow : Window
    {
        public OldLoginWindow()
        {
            InitializeComponent();

            frame.NavigationService.Navigate(new UI.TesterInterface.TesterDetails());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void User_Name_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Frame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

        }
    }
}
