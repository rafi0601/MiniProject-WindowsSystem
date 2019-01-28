//Bs"d

using BE;
using System;
using System.Collections.Generic;
using System.Data;
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

            CheckBox11.IsChecked = tester.WorkingHours[0, 0];
            CheckBox12.IsChecked = tester.WorkingHours[1, 0];
            CheckBox13.IsChecked = tester.WorkingHours[2, 0];
            CheckBox14.IsChecked = tester.WorkingHours[3, 0];
            CheckBox15.IsChecked = tester.WorkingHours[4, 0];
            CheckBox21.IsChecked = tester.WorkingHours[0, 1];
            CheckBox22.IsChecked = tester.WorkingHours[1, 1];
            CheckBox23.IsChecked = tester.WorkingHours[2, 1];
            CheckBox24.IsChecked = tester.WorkingHours[3, 1];
            CheckBox25.IsChecked = tester.WorkingHours[4, 1];
            CheckBox31.IsChecked = tester.WorkingHours[0, 2];
            CheckBox32.IsChecked = tester.WorkingHours[1, 2];
            CheckBox33.IsChecked = tester.WorkingHours[2, 2];
            CheckBox34.IsChecked = tester.WorkingHours[3, 2];
            CheckBox35.IsChecked = tester.WorkingHours[4, 2];
            CheckBox41.IsChecked = tester.WorkingHours[0, 3];
            CheckBox42.IsChecked = tester.WorkingHours[1, 3];
            CheckBox43.IsChecked = tester.WorkingHours[2, 3];
            CheckBox44.IsChecked = tester.WorkingHours[3, 3];
            CheckBox45.IsChecked = tester.WorkingHours[4, 3];
            CheckBox51.IsChecked = tester.WorkingHours[0, 4];
            CheckBox52.IsChecked = tester.WorkingHours[1, 4];
            CheckBox53.IsChecked = tester.WorkingHours[2, 4];
            CheckBox54.IsChecked = tester.WorkingHours[3, 4];
            CheckBox55.IsChecked = tester.WorkingHours[4, 4];
            CheckBox61.IsChecked = tester.WorkingHours[0, 5];
            CheckBox62.IsChecked = tester.WorkingHours[1, 5];
            CheckBox63.IsChecked = tester.WorkingHours[2, 5];
            CheckBox64.IsChecked = tester.WorkingHours[3, 5];
            CheckBox65.IsChecked = tester.WorkingHours[4, 5];
            CheckBox71.IsChecked = tester.WorkingHours[0, 6];
            CheckBox72.IsChecked = tester.WorkingHours[1, 6];
            CheckBox73.IsChecked = tester.WorkingHours[2, 6];
            CheckBox74.IsChecked = tester.WorkingHours[3, 6];
            CheckBox75.IsChecked = tester.WorkingHours[4, 6];

            FutureTestsDataGrid.ItemsSource = bl.GetTests(t => t.IDTester == tester.ID && t.IsDone() == false);
            TestsDataGrid.ItemsSource = bl.GetTests(t => t.IDTester == tester.ID && t.IsDone() == true);
            //TestsDataGrid.ItemsSource = tester.MyTests;
        }

        private void UpdateButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                tester.Name = new Name { FirstName = firstNameTextBox.Text, LastName = lastNameTextBox.Text };
                tester.Address = new Address { City = City.Text, HouseNumber = uint.Parse(HouseNumber.Text), Street = Street.Text };
                bool[,] workingHours = {
                    { (bool)CheckBox11.IsChecked, (bool)CheckBox21.IsChecked, (bool)CheckBox31.IsChecked, (bool)CheckBox41.IsChecked, (bool)CheckBox51.IsChecked, (bool)CheckBox61.IsChecked, (bool)CheckBox71.IsChecked },
                    { (bool)CheckBox12.IsChecked, (bool)CheckBox22.IsChecked, (bool)CheckBox32.IsChecked, (bool)CheckBox42.IsChecked, (bool)CheckBox52.IsChecked, (bool)CheckBox62.IsChecked, (bool)CheckBox72.IsChecked},
                    { (bool)CheckBox13.IsChecked, (bool)CheckBox23.IsChecked, (bool)CheckBox33.IsChecked, (bool)CheckBox43.IsChecked, (bool)CheckBox53.IsChecked, (bool)CheckBox63.IsChecked, (bool)CheckBox73.IsChecked},
                    { (bool)CheckBox14.IsChecked, (bool)CheckBox24.IsChecked, (bool)CheckBox34.IsChecked, (bool)CheckBox44.IsChecked, (bool)CheckBox54.IsChecked, (bool)CheckBox64.IsChecked, (bool)CheckBox74.IsChecked},
                    { (bool)CheckBox15.IsChecked, (bool)CheckBox25.IsChecked, (bool)CheckBox35.IsChecked, (bool)CheckBox45.IsChecked, (bool)CheckBox55.IsChecked, (bool)CheckBox65.IsChecked, (bool)CheckBox75.IsChecked},
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

        private void TestsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GradingTest.IsEnabled = true;
            // MessageBox.Show(v.Code.ToString(), "RRRRRRRRRAVVVVVVVVVVV", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.No);
        }

        private void GradingTest_SendClick(object sender, EventArgs e)
        {
            var test = TestsDataGrid.SelectedItem as Test;
            bl.UpdateTest(test.Code, new Test.Criteria(GradingTest.KeepDistance.IsChecked, GradingTest.BackParking.IsChecked, GradingTest.UsingViewMirrors.IsChecked, GradingTest.Signaling.IsChecked, GradingTest.IntegrationIntoMovement.IsChecked, GradingTest.ObeyParkSigns.IsChecked), (bool)GradingTest.IsPass.IsChecked, GradingTest.Note.Text);
            GradingTest.KeepDistance.IsChecked = null;
            GradingTest.ObeyParkSigns.IsChecked = null;
            GradingTest.IntegrationIntoMovement.IsChecked = null;
            GradingTest.Signaling.IsChecked = null;
            GradingTest.IsEnabled = false;
        }
    }
}



