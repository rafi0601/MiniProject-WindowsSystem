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
            //vehicleComboBox.ItemsSource = Enum.GetValues(typeof(Vehicle));

        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                trainee.Name = new Name { FirstName = firstNameTextBox.Text, LastName = lastNameTextBox.Text };
                trainee.TeacherName = new Name { FirstName = TeacherFirstNameTextBox.Text, LastName = TeacherLastNameTextBox.Text };
                trainee.Address = new Address { City = City.Text, HouseNumber = uint.Parse(HouseNumber.Text), Street = Street.Text };

                bl.AddTrainee(trainee);
                Singleton.Instance.Add(new User() { name = iDTextBox.Text, password = passwordBoxNew.Password, role = typeof(Trainee) });

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


            vehicleComboBox.ItemsSource = from vehicle in Enum.GetValues(typeof(Vehicle)).Cast<Vehicle>()
                                          select GetAttribute(vehicle)?.DisplayName;
            UserDisplayAttribute GetAttribute(Vehicle vehicle)
            {
                var arr = vehicle.GetType().GetField(vehicle.ToString()).GetCustomAttributes(false);
                if (arr.Length == 1)
                    return (UserDisplayAttribute)arr[0];
                return null;
            }
            Vehicle? GetEnum(string display)
            {
                foreach (Vehicle vehicle in Enum.GetValues(typeof(Vehicle)))
                {
                    UserDisplayAttribute attribute = GetAttribute(vehicle);
                    if (attribute != null && attribute.DisplayName == display)
                        return vehicle;
                }
                return null;
            }
        }
    }
}
