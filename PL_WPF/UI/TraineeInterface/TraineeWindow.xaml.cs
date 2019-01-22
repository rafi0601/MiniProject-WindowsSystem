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
    /// Interaction logic for TraineeWindow.xaml
    /// </summary>
    public partial class TraineeWindow : Window
    {
        Trainee trainee;
        BL.IBL bl;

        public TraineeWindow(Trainee trainee)
        {
            InitializeComponent();

            bl = BL.Singleton.Instance;
            this.trainee = trainee;
            this.DataContext = trainee;

            gearboxComboBox.ItemsSource = Enum.GetValues(typeof(Gearbox));//.SplitByUpperAndLower();
            genderComboBox.ItemsSource = Enum.GetValues(typeof(Gender));//.SplitByUpperAndLower();
            vehicleComboBox.ItemsSource = Enum.GetValues(typeof(Vehicle));//.SplitByUpperAndLower();


            //firstNameTextBox.Text = trainee.Name.FirstName;
            //iDTextBox.Text = trainee.ID;
            //birthdateDatePicker.Text = trainee.Birthdate.ToString();
            //phoneNumberTextBox.Text = trainee.PhoneNumber;
            //// TODO: password
            //gearboxComboBox.Text = trainee.Gearbox.ToString();
            //vehicleComboBox.Text = trainee.Vehicle.ToString();
            //numberOfDoneLessonsTextBox.Text = trainee.NumberOfDoneLessons.ToString();
            //drivingSchoolTextBox2.Text = trainee.DrivingSchool;
            //this.Street.Text = trainee.Address.Street;
            //this.HouseNumber.Text = trainee.Address.HouseNumber.ToString();
            //this.City.Text = trainee.Address.City;
        }

        private void UpdateButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.UpdateTrainee(trainee);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void RemoveButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (MessageBox.Show("Are you sure you want to Remove your account?", "Verify deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No))
                {
                    case MessageBoxResult.Yes:
                        bl.RemoveTrainee(trainee.ID);
                        this.Close();
                        trainee = new Trainee();
                        this.DataContext = trainee;
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.No);
            }
        }
    }
}
