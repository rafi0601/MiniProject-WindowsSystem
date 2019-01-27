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
    /// Interaction logic for TesterWindow.xaml
    /// </summary>
    public partial class TesterWindow : Window
    {
        Tester tester;
        BL.IBL bl;

        public TesterWindow(Tester tester)
        {
            InitializeComponent();

            bl = BL.Singleton.Instance;
            this.tester = tester;
            DataContext = tester;

            genderComboBox.ItemsSource = Enum.GetValues(typeof(Gender));//.SplitByUpperAndLower();
            vehicleTypeExpertiseComboBox.ItemsSource = Enum.GetValues(typeof(Vehicle));//.SplitByUpperAndLower();

            firstNameTextBox.Text = tester.Name.FirstName;
            lastNameTextBox.Text = tester.Name.LastName;
            Street.Text = tester.Address.Street;
            HouseNumber.Text = tester.Address.HouseNumber.ToString();
            City.Text = tester.Address.City;

            CheckBox01.IsChecked = tester.WorkingHours[4, 0];
            CheckBox02.IsChecked = tester.WorkingHours[4, 1];
            CheckBox03.IsChecked = tester.WorkingHours[4, 2];
            CheckBox04.IsChecked = tester.WorkingHours[4, 3];
            CheckBox05.IsChecked = tester.WorkingHours[4, 4];
            CheckBox06.IsChecked = tester.WorkingHours[4, 5];
            CheckBox07.IsChecked = tester.WorkingHours[4, 6];
            CheckBox11.IsChecked = tester.WorkingHours[3, 0];
            CheckBox12.IsChecked = tester.WorkingHours[3, 1];
            CheckBox13.IsChecked = tester.WorkingHours[3, 2];
            CheckBox14.IsChecked = tester.WorkingHours[3, 3];
            CheckBox15.IsChecked = tester.WorkingHours[3, 4];
            CheckBox16.IsChecked = tester.WorkingHours[3, 5];
            CheckBox17.IsChecked = tester.WorkingHours[3, 6];
            CheckBox21.IsChecked = tester.WorkingHours[2, 0];
            CheckBox22.IsChecked = tester.WorkingHours[2, 1];
            CheckBox23.IsChecked = tester.WorkingHours[2, 2];
            CheckBox24.IsChecked = tester.WorkingHours[2, 3];
            CheckBox25.IsChecked = tester.WorkingHours[2, 4];
            CheckBox26.IsChecked = tester.WorkingHours[2, 5];
            CheckBox27.IsChecked = tester.WorkingHours[2, 6];
            CheckBox31.IsChecked = tester.WorkingHours[1, 0];
            CheckBox32.IsChecked = tester.WorkingHours[1, 1];
            CheckBox33.IsChecked = tester.WorkingHours[1, 2];
            CheckBox34.IsChecked = tester.WorkingHours[1, 3];
            CheckBox35.IsChecked = tester.WorkingHours[1, 4];
            CheckBox36.IsChecked = tester.WorkingHours[1, 5];
            CheckBox37.IsChecked = tester.WorkingHours[1, 6];
            CheckBox41.IsChecked = tester.WorkingHours[0, 0];
            CheckBox42.IsChecked = tester.WorkingHours[0, 1];
            CheckBox43.IsChecked = tester.WorkingHours[0, 2];
            CheckBox44.IsChecked = tester.WorkingHours[0, 3];
            CheckBox45.IsChecked = tester.WorkingHours[0, 4];
            CheckBox46.IsChecked = tester.WorkingHours[0, 5];
            CheckBox47.IsChecked = tester.WorkingHours[0, 6];

            FutureTestsDataGrid.ItemsSource = bl.GetTests(t=>t.IDTester == tester.ID && t.IsDone() == false);
            TestsDataGrid.ItemsSource = bl.GetTests(t=>t.IDTester == tester.ID && t.IsDone() == true);
            //TestsDataGrid.ItemsSource = tester.MyTests;
        }

        private void UpdateButtonClick(object sender, RoutedEventArgs e)
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

                bl.UpdateTester(tester);
                new UI.TesterInterface.TesterWindow(tester).Show();
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
                        bl.RemoveTester(tester.ID);
                        Close();
                        tester = new Tester();
                        DataContext = tester;
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.No);
            }
        }

        private void Refrash_Button_Click(object sender, RoutedEventArgs e)
        {
            FutureTestsDataGrid.ItemsSource = bl.GetTests(t => t.IDTester == tester.ID && t.IsDone() == false);
            TestsDataGrid.ItemsSource = bl.GetTests(t => t.IDTester == tester.ID && t.IsDone() == true);
        }
    }
}
