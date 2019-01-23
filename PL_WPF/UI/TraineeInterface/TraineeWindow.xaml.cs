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
            DataContext = trainee;

            gearboxComboBox.ItemsSource = Enum.GetValues(typeof(Gearbox));//.SplitByUpperAndLower();
            genderComboBox.ItemsSource = Enum.GetValues(typeof(Gender));//.SplitByUpperAndLower();
            vehicleComboBox.ItemsSource = Enum.GetValues(typeof(Vehicle));//.SplitByUpperAndLower();

            firstNameTextBox.Text = trainee.Name.FirstName;
            lastNameTextBox.Text = trainee.Name.LastName;
            Street.Text = trainee.Address.Street;
            HouseNumber.Text = trainee.Address.HouseNumber.ToString();
            City.Text = trainee.Address.City;
            TeacherFirstNameTextBox.Text = trainee.TeacherName.FirstName;
            TeacherLastNameTextBox.Text = trainee.TeacherName.LastName;

            //iDTextBox.Text = trainee.ID;
            //birthdateDatePicker.Text = trainee.Birthdate.ToString();
            //phoneNumberTextBox.Text = trainee.PhoneNumber;
            //// TODO: password
            //gearboxComboBox.Text = trainee.Gearbox.ToString();
            //vehicleComboBox.Text = trainee.Vehicle.ToString();
            //numberOfDoneLessonsTextBox.Text = trainee.NumberOfDoneLessons.ToString();
            //drivingSchoolTextBox2.Text = trainee.DrivingSchool;
        }

        private void UpdateButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                trainee.Name = new Name { FirstName = firstNameTextBox.Text, LastName = lastNameTextBox.Text };
                trainee.TeacherName = new Name { FirstName = TeacherFirstNameTextBox.Text, LastName = TeacherLastNameTextBox.Text };
                trainee.Address = new Address { City = City.Text, HouseNumber = uint.Parse(HouseNumber.Text), Street = Street.Text };

                bl.UpdateTrainee(trainee);
                new UI.TraineeInterface.TraineeWindow(trainee).Show();
                Close();
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
                        Close();
                        trainee = new Trainee();
                        DataContext = trainee;
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
