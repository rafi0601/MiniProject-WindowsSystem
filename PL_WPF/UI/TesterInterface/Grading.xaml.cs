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
using BE;

namespace PL_WPF.UI.TesterInterface
{
    /// <summary>
    /// Interaction logic for Grading.xaml
    /// </summary>
    public partial class Grading : UserControl
    {
        public Grading() // TODO (Criteria ctiteria)
        {
            InitializeComponent();
            //Note.Width=this.child.max
        }


        public void Refresh()
        {
            KeepDistance.IsChecked =
                ObeyParkSigns.IsChecked =
                IntegrationIntoMovement.IsChecked =
                Signaling.IsChecked =
                BackParking.IsChecked =
                UsingViewMirrors.IsChecked =
                IsPass.IsChecked = false;
            Note.Text = "";
        }


        public event RoutedEventHandler SendClick;

        private void Send_Button_Click(object sender, RoutedEventArgs e)
        {
            SendClick(this, new RoutedEventArgs()); // TODO send information in the args
        }



        public Test.Criteria Criteria { get; } = new Test.Criteria();

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            // TODO update criteria
        }
    }
}
