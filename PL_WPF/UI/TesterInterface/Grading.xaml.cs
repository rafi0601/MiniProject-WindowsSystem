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

namespace PL_WPF.UI.TesterInterface
{
    /// <summary>
    /// Interaction logic for Grading.xaml
    /// </summary>
    public partial class Grading : UserControl
    {
        public Grading()
        {
            InitializeComponent();
        }

        public event RoutedEventHandler SendClick;

        private void Send_Button_Click(object sender, RoutedEventArgs e)
        {
            SendClick(this, new RoutedEventArgs()); // TODO send information in the args
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
