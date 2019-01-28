//Bs"d

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
        Tester tester;
        BL.IBL bl;

        public TesterRegisteraionWindow()
        {
            InitializeComponent();
            bl = BL.Singleton.Instance;
            tester = new Tester();
            DataContext = tester;

            genderComboBox.ItemsSource = Enum.GetValues(typeof(Gender));
            vehicleTypeExpertiseComboBox.ItemsSource = Enum.GetValues(typeof(Vehicle));
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                tester.Name = new Name { FirstName = firstNameTextBox.Text, LastName = lastNameTextBox.Text };
                tester.Address = new Address { City = City.Text, HouseNumber = uint.Parse(HouseNumber.Text), Street = Street.Text };
                bool[,] workingHours = {
                    { (bool)CheckBox41.IsChecked, (bool)CheckBox42.IsChecked, (bool)CheckBox43.IsChecked, (bool)CheckBox44.IsChecked, (bool)CheckBox45.IsChecked, (bool)CheckBox46.IsChecked, (bool)CheckBox47.IsChecked },
                    { (bool)CheckBox31.IsChecked, (bool)CheckBox32.IsChecked, (bool)CheckBox33.IsChecked, (bool)CheckBox34.IsChecked, (bool)CheckBox35.IsChecked, (bool)CheckBox36.IsChecked, (bool)CheckBox37.IsChecked },
                    { (bool)CheckBox21.IsChecked, (bool)CheckBox22.IsChecked, (bool)CheckBox23.IsChecked, (bool)CheckBox24.IsChecked, (bool)CheckBox25.IsChecked, (bool)CheckBox26.IsChecked, (bool)CheckBox27.IsChecked },
                    { (bool)CheckBox11.IsChecked, (bool)CheckBox12.IsChecked, (bool)CheckBox13.IsChecked, (bool)CheckBox14.IsChecked, (bool)CheckBox15.IsChecked, (bool)CheckBox16.IsChecked, (bool)CheckBox17.IsChecked },
                    { (bool)CheckBox01.IsChecked, (bool)CheckBox02.IsChecked, (bool)CheckBox03.IsChecked, (bool)CheckBox04.IsChecked, (bool)CheckBox05.IsChecked, (bool)CheckBox06.IsChecked, (bool)CheckBox07.IsChecked },
                };
                tester.WorkingHours = workingHours;

                //IUserManager userManager = Singleton.Instance;
                //userManager.Add(new User() { name = iDTextBox.Text, password = passwordBoxNew.Password, role = typeof(Tester) });
                
                bl.AddTester(tester);
                new TesterWindow(tester).Show();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}