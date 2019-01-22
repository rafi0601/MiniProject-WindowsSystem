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
using BE;

namespace PL_WPF.UI.TraineeInterface
{
    /// <summary>
    /// Interaction logic for TraineeRegisteraionWindow.xaml
    /// </summary>
    public partial class TraineeRegisteraionWindow : Window
    {
        Trainee trainee;
        BL.IBL bl;

        public TraineeRegisteraionWindow()
        {
            InitializeComponent();

            bl = BL.Singleton.Instance;
            trainee = new Trainee();
            DataContext = trainee;

            gearboxComboBox.ItemsSource = Enum.GetValues(typeof(Gearbox));
            genderComboBox.ItemsSource = Enum.GetValues(typeof(Gender));
            vehicleComboBox.ItemsSource = Enum.GetValues(typeof(Vehicle));

        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.AddTrainee(trainee);
                new TraineeWindow(trainee).Show();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
