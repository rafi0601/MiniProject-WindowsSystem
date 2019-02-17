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
        Trainee trainee = new Trainee();
        BL.IBL bl = BL.FactorySingleton.Instance;

        public TraineeRegisteraionWindow()
        {
            InitializeComponent();

            trainee.Birthdate = new DateTime(1900, 1, 1);

            gearboxComboBox.ItemsSource = Enum.GetValues(typeof(Gearbox));
            genderComboBox.ItemsSource = Enum.GetValues(typeof(Gender));
            vehicleListBox.ItemsSource = Functions.VehiclesToDisplayStrings();

            DataContext = trainee;
        }


        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (passwordBoxNew.Password != passwordBoxAuthentication.Password)
                    throw new CasingException(true, new Exception("The authentication password not correct."));

                if (bl.GetTrainee(iDTextBox.Text) != null)
                    throw new CasingException(true, new Exception("This id already exists in the system"));

                try
                {
                    trainee.Name = new Person.PersonName(lastNameTextBox.Text, firstNameTextBox.Text);
                    trainee.TeacherName = new Person.PersonName(TeacherLastNameTextBox.Text, TeacherFirstNameTextBox.Text);
                    trainee.Address = new Address(Street.Text, uint.Parse(HouseNumber.Text), City.Text);
                    trainee.Password = passwordBoxNew.Password;
                }
                catch (Exception ex)
                {
                    throw new CasingException(true, ex);
                }


                foreach (string expertise in vehicleListBox.SelectedItems)
                    trainee.VehicleTypeTraining |= (Vehicle)Tools.GetEnumAccordingToUserDisplay(typeof(Vehicle), expertise);  //tester.VehicleTypeExpertise = tester.VehicleTypeExpertise.AddFlag(expertise);

                bl.AddTrainee(trainee);

                new TraineeWindow(trainee).Show();
                Close();
            }
            catch (CasingException ex) when (ex.DisplayToUser)
            {
                Functions.ShowMessageToUser(ex);
            }
            catch (Exception ex)
            {
                Functions.SendMailToAdmin(ex);
                Close();
            }
        }


        //private void Window_Loaded(object sender, RoutedEventArgs e)
        //{
        //
        //}
    }
}
