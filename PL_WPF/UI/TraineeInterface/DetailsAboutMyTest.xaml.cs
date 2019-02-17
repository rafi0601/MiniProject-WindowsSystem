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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL_WPF.UI.TraineeInterface
{
    /// <summary>
    /// Interaction logic for DetailsAboutMyTest.xaml
    /// </summary>
    public partial class DetailsAboutMyTest : UserControl
    {
        public DetailsAboutMyTest()
        {
            InitializeComponent();
        }


        public event RoutedEventHandler RefreshButtonClick
        {
            add { Refresh_Button.Click += value; }
            remove { Refresh_Button.Click -= value; }
        }



        private void Refresh_Button_Click(object sender, RoutedEventArgs e)
        {
            // RefreshButtonClick(this, new RoutedEventArgs());
        }
    }
}
