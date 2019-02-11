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
        BL.IBL bl = BL.Singleton.Instance;

        public TesterWindow(Tester tester)
        {
            InitializeComponent();

            DataContext = this.tester = tester ?? throw new ArgumentException("Ther is no tester");
            //DataContext = tester;

            genderComboBox.ItemsSource = Enum.GetValues(typeof(Gender));//.SplitByUpperAndLower();
            //vehicleTypeExpertiseListBox.ItemsSource = Enum.GetValues(typeof(Vehicle));//.SplitByUpperAndLower();
            //vehicleTypeExpertiseListBox.ItemsSource = (from vehicle in Enum.GetValues(typeof(Vehicle)).Cast<Vehicle>()
            //                                          where tester.VehicleTypesExpertise.HasFlag(vehicle)
            //                                          select Tools.GetUserDisplayAttribute(vehicle)?.DisplayName ?? vehicle.ToString()).ToArray();
            foreach (Vehicle vehicle in Enum.GetValues(typeof(Vehicle)).Cast<Vehicle>())
                if (tester.VehicleTypesExpertise.HasFlag(vehicle))
                    vehicleTypeExpertiseListBox.Items.Add(new ListBoxItem() { Content = Tools.GetUserDisplayAttribute(vehicle)?.DisplayName ?? vehicle.ToString() });
            vehicleTypeExpertiseListBox.SelectAll();
            foreach (Vehicle vehicle in Enum.GetValues(typeof(Vehicle)).Cast<Vehicle>())
                if (!tester.VehicleTypesExpertise.HasFlag(vehicle))
                    vehicleTypeExpertiseListBox.Items.Add(new ListBoxItem() { Content = Tools.GetUserDisplayAttribute(vehicle)?.DisplayName ?? vehicle.ToString() });

            firstNameTextBox.Text = tester.Name.FirstName;
            lastNameTextBox.Text = tester.Name.LastName;
            Street.Text = tester.Address.Street;
            HouseNumber.Text = tester.Address.HouseNumber.ToString();
            City.Text = tester.Address.City;


            CheckBox11.IsChecked = tester.WorkingHours[0, 9];
            CheckBox12.IsChecked = tester.WorkingHours[1, 9];
            CheckBox13.IsChecked = tester.WorkingHours[2, 9];
            CheckBox14.IsChecked = tester.WorkingHours[3, 9];
            CheckBox15.IsChecked = tester.WorkingHours[4, 9];
            CheckBox21.IsChecked = tester.WorkingHours[0, 10];
            CheckBox22.IsChecked = tester.WorkingHours[1, 10];
            CheckBox23.IsChecked = tester.WorkingHours[2, 10];
            CheckBox24.IsChecked = tester.WorkingHours[3, 10];
            CheckBox25.IsChecked = tester.WorkingHours[4, 10];
            CheckBox31.IsChecked = tester.WorkingHours[0, 11];
            CheckBox32.IsChecked = tester.WorkingHours[1, 11];
            CheckBox33.IsChecked = tester.WorkingHours[2, 11];
            CheckBox34.IsChecked = tester.WorkingHours[3, 11];
            CheckBox35.IsChecked = tester.WorkingHours[4, 11];
            CheckBox41.IsChecked = tester.WorkingHours[0, 12];
            CheckBox42.IsChecked = tester.WorkingHours[1, 12];
            CheckBox43.IsChecked = tester.WorkingHours[2, 12];
            CheckBox44.IsChecked = tester.WorkingHours[3, 12];
            CheckBox45.IsChecked = tester.WorkingHours[4, 12];
            CheckBox51.IsChecked = tester.WorkingHours[0, 13];
            CheckBox52.IsChecked = tester.WorkingHours[1, 13];
            CheckBox53.IsChecked = tester.WorkingHours[2, 13];
            CheckBox54.IsChecked = tester.WorkingHours[3, 13];
            CheckBox55.IsChecked = tester.WorkingHours[4, 13];
            CheckBox61.IsChecked = tester.WorkingHours[0, 14];
            CheckBox62.IsChecked = tester.WorkingHours[1, 14];
            CheckBox63.IsChecked = tester.WorkingHours[2, 14];
            CheckBox64.IsChecked = tester.WorkingHours[3, 14];
            CheckBox65.IsChecked = tester.WorkingHours[4, 14];
            CheckBox71.IsChecked = tester.WorkingHours[0, 15];
            CheckBox72.IsChecked = tester.WorkingHours[1, 15];
            CheckBox73.IsChecked = tester.WorkingHours[2, 15];
            CheckBox74.IsChecked = tester.WorkingHours[3, 15];
            CheckBox75.IsChecked = tester.WorkingHours[4, 15];

            FutureTestsDataGrid.ItemsSource = bl.GetTests(t => t.TesterID == tester.ID && t.IsDone() == false);
            TestsDataGrid.ItemsSource = bl.GetTests(t => t.TesterID == tester.ID && t.IsDone() == true);
            //TestsDataGrid.ItemsSource = tester.MyTests;

            TesterDetails testerDetails = new TesterDetails();
            testerDetails.iDTextBox.IsEnabled = false;

        }

        private void UpdateButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                tester.Name = new Person.PersonName { FirstName = firstNameTextBox.Text, LastName = lastNameTextBox.Text };
                tester.Address = new Address { City = City.Text, HouseNumber = uint.Parse(HouseNumber.Text), Street = Street.Text };
                tester.WorkingHours = new Schedule(new bool[,] {
                    { (bool)CheckBox11.IsChecked, (bool)CheckBox21.IsChecked, (bool)CheckBox31.IsChecked, (bool)CheckBox41.IsChecked, (bool)CheckBox51.IsChecked, (bool)CheckBox61.IsChecked, (bool)CheckBox71.IsChecked },
                    { (bool)CheckBox12.IsChecked, (bool)CheckBox22.IsChecked, (bool)CheckBox32.IsChecked, (bool)CheckBox42.IsChecked, (bool)CheckBox52.IsChecked, (bool)CheckBox62.IsChecked, (bool)CheckBox72.IsChecked },
                    { (bool)CheckBox13.IsChecked, (bool)CheckBox23.IsChecked, (bool)CheckBox33.IsChecked, (bool)CheckBox43.IsChecked, (bool)CheckBox53.IsChecked, (bool)CheckBox63.IsChecked, (bool)CheckBox73.IsChecked },
                    { (bool)CheckBox14.IsChecked, (bool)CheckBox24.IsChecked, (bool)CheckBox34.IsChecked, (bool)CheckBox44.IsChecked, (bool)CheckBox54.IsChecked, (bool)CheckBox64.IsChecked, (bool)CheckBox74.IsChecked },
                    { (bool)CheckBox15.IsChecked, (bool)CheckBox25.IsChecked, (bool)CheckBox35.IsChecked, (bool)CheckBox45.IsChecked, (bool)CheckBox55.IsChecked, (bool)CheckBox65.IsChecked, (bool)CheckBox75.IsChecked },
                });

                tester.VehicleTypesExpertise = 0;
                foreach (string expertise in vehicleTypeExpertiseListBox.SelectedItems)
                    tester.VehicleTypesExpertise |= (Vehicle)Tools.GetEnum(typeof(Vehicle), expertise);  //tester.VehicleTypeExpertise = tester.VehicleTypeExpertise.AddFlag(expertise);



                bl.UpdateTester(tester);
                new UI.TesterInterface.TesterWindow(tester).Show();
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

        private void RemoveButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (MessageBox.Show("Are you sure you want to Remove your account?", "Verify deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No))
                {
                    case MessageBoxResult.Yes:
                        bl.RemoveTester(tester);
                        Close();
                        tester = new Tester();
                        DataContext = tester;
                        break;
                }
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

        private void Refrash_Button_Click(object sender, RoutedEventArgs e)
        {
            FutureTestsDataGrid.ItemsSource = bl.GetTests(t => t.TesterID == tester.ID && t.IsDone() == false);
            TestsDataGrid.ItemsSource = bl.GetTests(t => t.TesterID == tester.ID && t.IsDone() == true && t.IsPass == null);
        }

        private void TestsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var test = TestsDataGrid.SelectedItem as Test;

            if (test == null)
                return;

            if (test.IsPass == null)
            {
                GradingTest.KeepDistance.IsChecked = false;
                GradingTest.ObeyParkSigns.IsChecked = false;
                GradingTest.IntegrationIntoMovement.IsChecked = false;
                GradingTest.Signaling.IsChecked = false;
                GradingTest.BackParking.IsChecked = false;
                GradingTest.UsingViewMirrors.IsChecked = false;
                GradingTest.IsPass.IsChecked = false;
                GradingTest.IsEnabled = true;
            }
        }

        private void GradingTest_SendClick(object sender, RoutedEventArgs e)
        {
            var test = TestsDataGrid.SelectedItem as Test;
            try
            {
                bl.UpdateTest(test.Code, new Test.Criteria(GradingTest.KeepDistance.IsChecked, GradingTest.BackParking.IsChecked, GradingTest.UsingViewMirrors.IsChecked, GradingTest.Signaling.IsChecked, GradingTest.IntegrationIntoMovement.IsChecked, GradingTest.ObeyParkSigns.IsChecked), (bool)GradingTest.IsPass.IsChecked, GradingTest.Note.Text);
                GradingTest.KeepDistance.IsChecked = GradingTest.BackParking.IsChecked = GradingTest.UsingViewMirrors.IsChecked = GradingTest.Signaling.IsChecked = GradingTest.IntegrationIntoMovement.IsChecked = GradingTest.ObeyParkSigns.IsChecked = GradingTest.IsPass.IsChecked = false;
                GradingTest.Note.Text = "";
                GradingTest.IsEnabled = false;
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

        private void UpdatePasswordButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (passwordBoxNew.Password != passwordBoxAuthentication.Password)
                    throw new CasingException(true, new ArgumentException("Password authentication is not equivalent to the password"));

                tester.Password = passwordBoxNew.Password;
                bl.UpdateTester(tester);
                new TesterWindow(tester).Show();
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
    }
}



