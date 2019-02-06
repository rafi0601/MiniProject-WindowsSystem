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
using BE;

namespace PL_WPF.UI.TraineeInterface
{
    /// <summary>
    /// Interaction logic for TraineeRegisteraionWindow.xaml
    /// </summary>
    public partial class TraineeRegisteraionWindow : Window
    {
        Trainee trainee;
        BL.IBL bl = BL.Singleton.Instance;

        public TraineeRegisteraionWindow()
        {
            InitializeComponent();
            try
            {
                trainee = new Trainee();
                DataContext = trainee;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (passwordBoxNew.Password != passwordBoxAuthentication.Password)
                    throw new Exception("The authentication password not correct.");

                if (bl.GetTrainee(iDTextBox.Text) != null)
                    throw new Exception("Alredy exist");

                trainee.Name = new Name { FirstName = firstNameTextBox.Text, LastName = lastNameTextBox.Text };
                trainee.TeacherName = new Name { FirstName = TeacherFirstNameTextBox.Text, LastName = TeacherLastNameTextBox.Text };
                trainee.Address = new Address { City = City.Text, HouseNumber = uint.Parse(HouseNumber.Text), Street = Street.Text };
                trainee.Password = passwordBoxNew.Password;

                foreach (string expertise in vehicleListBox.SelectedItems)
                    trainee.VehicleTypeTraining |= (Vehicle)Tools.GetEnum(typeof(Vehicle), expertise);  //tester.VehicleTypeExpertise = tester.VehicleTypeExpertise.AddFlag(expertise);

                bl.AddTrainee(trainee);

                new TraineeWindow(trainee).Show();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            gearboxComboBox.ItemsSource = Enum.GetValues(typeof(Gearbox));
            genderComboBox.ItemsSource = Enum.GetValues(typeof(Gender));
            vehicleListBox.ItemsSource = from vehicle in Enum.GetValues(typeof(Vehicle)).Cast<Vehicle>()
                                          select Tools.GetUserDisplayAttribute(vehicle)?.DisplayName;
            //vehicleComboBox.ItemsSource = Enum.GetValues(typeof(Vehicle));
        }
    }
}
