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
    /// Interaction logic for SuggestAlternateDate.xaml
    /// </summary>
    public partial class SuggestAlternateDate : UserControl
    {
        public SuggestAlternateDate()
        {
            InitializeComponent();
        }

        public event RoutedEventHandler AcceptClick;// = AcceptButton.Click;

        private void Accept_Button_Click(object sender, RoutedEventArgs e)
        {
            AcceptClick(this, new RoutedEventArgs());
        }

        public event RoutedEventHandler CancelClick;

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            CancelClick(this, new RoutedEventArgs());
        }
    }
}
