//BS"D

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

namespace PL_WPF.UI.AdminInterface
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        BL.IBL bl = BL.Singleton.Instance;

        public AdminWindow()
        {
            InitializeComponent();
        }

        private void TraineesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Expertise_Selected(object sender, RoutedEventArgs e)
        {
            TestersDataGrid.ItemsSource = bl.TestersByExpertise(true);
        }
    }
}
