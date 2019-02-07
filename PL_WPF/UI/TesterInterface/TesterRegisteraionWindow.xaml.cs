﻿//Bs"d

using BE;
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

namespace PL_WPF.UI.TesterInterface
{
    /// <summary>
    /// Interaction logic for TesterRegisteraionWindow.xaml
    /// </summary>
    public partial class TesterRegisteraionWindow : Window
    {
        Tester tester = new Tester();
        BL.IBL bl = BL.Singleton.Instance;

        public TesterRegisteraionWindow()
        {
            InitializeComponent();

            DataContext = tester;

            this.Loaded += TesterRegisteraionWindow_Loaded;


        }

        private void TesterRegisteraionWindow_Loaded(object sender, RoutedEventArgs e)
        {
            genderComboBox.ItemsSource = Enum.GetValues(typeof(Gender));
            vehicleTypeExpertiseListBox.ItemsSource = from vehicle in Enum.GetValues(typeof(Vehicle)).Cast<Vehicle>()
                                                      select Tools.GetUserDisplayAttribute(vehicle)?.DisplayName;
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (passwordBoxNew.Password != passwordBoxAuthentication.Password)
                    throw new Exception("The authentication password not correct.");

                if (bl.GetTrainee(iDTextBox.Text) != null)
                    throw new Exception("Alredy exist");

                tester.Name = new Person.PersonName { FirstName = firstNameTextBox.Text, LastName = lastNameTextBox.Text };
                tester.Address = new Address { City = City.Text, HouseNumber = uint.Parse(HouseNumber.Text), Street = Street.Text };
                tester.Password = passwordBoxNew.Password;
                tester.WorkingHours = new Schedule(new bool[,] {
                    { (bool)CheckBox11.IsChecked, (bool)CheckBox21.IsChecked, (bool)CheckBox31.IsChecked, (bool)CheckBox41.IsChecked, (bool)CheckBox51.IsChecked, (bool)CheckBox61.IsChecked, (bool)CheckBox71.IsChecked },
                    { (bool)CheckBox12.IsChecked, (bool)CheckBox22.IsChecked, (bool)CheckBox32.IsChecked, (bool)CheckBox42.IsChecked, (bool)CheckBox52.IsChecked, (bool)CheckBox62.IsChecked, (bool)CheckBox72.IsChecked },
                    { (bool)CheckBox13.IsChecked, (bool)CheckBox23.IsChecked, (bool)CheckBox33.IsChecked, (bool)CheckBox43.IsChecked, (bool)CheckBox53.IsChecked, (bool)CheckBox63.IsChecked, (bool)CheckBox73.IsChecked },
                    { (bool)CheckBox14.IsChecked, (bool)CheckBox24.IsChecked, (bool)CheckBox34.IsChecked, (bool)CheckBox44.IsChecked, (bool)CheckBox54.IsChecked, (bool)CheckBox64.IsChecked, (bool)CheckBox74.IsChecked },
                    { (bool)CheckBox15.IsChecked, (bool)CheckBox25.IsChecked, (bool)CheckBox35.IsChecked, (bool)CheckBox45.IsChecked, (bool)CheckBox55.IsChecked, (bool)CheckBox65.IsChecked, (bool)CheckBox75.IsChecked },
                });

                foreach (object expertise in vehicleTypeExpertiseListBox.SelectedItems) // IMPROVEMENT change object to string so remove cast
                    tester.VehicleTypesExpertise |= (Vehicle)Tools.GetEnum(typeof(Vehicle), (string)expertise);  //tester.VehicleTypeExpertise = tester.VehicleTypeExpertise.AddFlag(expertise);

                iDTextBox.GetBindingExpression(Xceed.Wpf.Toolkit.MaskedTextBox.TextProperty).UpdateSource();


                bl.AddTester(tester);

                new TesterWindow(tester).Show();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

    }
}