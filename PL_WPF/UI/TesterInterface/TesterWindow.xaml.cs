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
using static BE.Configuration;

namespace PL_WPF.UI.TesterInterface
{
    /// <summary>
    /// Interaction logic for TesterWindow.xaml
    /// </summary>
    public partial class TesterWindow : Window
    {
        Tester tester;
        BL.IBL bl = BL.FactorySingleton.Instance;


        public TesterWindow(Tester tester)
        {
            if (tester == null)
                throw new CasingException(false, new ArgumentNullException("Cannot show null"));

            InitializeComponent();

            DataContext = this.tester = tester;


            genderComboBox.ItemsSource = Enum.GetValues(typeof(Gender));

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
            City.Text = tester.Address.City;
            HouseNumber.Text = tester.Address.HouseNumber.ToString();
            Street.Text = tester.Address.Street;

            for (int i = 0; i < WORKING_DAYS_A_WEEK; i++)
            {
                for (int j = 0; j < WORKING_HOURS_A_DAY; j++)
                {
                    CheckBox checkBox = new CheckBox
                    {
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        IsChecked = tester.WorkingHours[(DayOfWeek)i, (int)(j + BEGINNING_OF_A_WORKING_DAY)]
                    };
                    //checkBox.SetBinding(CheckBox.IsCheckedProperty,
                    //    new Binding("IsChecked")
                    //    {
                    //        Source = markAllCheckBox,
                    //        Mode = BindingMode.OneWay
                    //    });

                    Grid.SetColumn(checkBox, i + 1);
                    Grid.SetRow(checkBox, j + 1);

                    scheduleGrid.Children.Add(checkBox);
                }
            }


            Refresh_Button_Click(Refresh_Button, new RoutedEventArgs());

        }


        private void UpdateButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                try
                {
                    tester.Name = new Person.PersonName(lastNameTextBox.Text, firstNameTextBox.Text);
                    tester.Address = new Address(Street.Text, uint.Parse(HouseNumber.Text), City.Text);

                    foreach (var item in scheduleGrid.Children)
                        if (item is CheckBox checkBox/* && checkBox != markAllCheckBox*/)
                            tester.WorkingHours[(DayOfWeek)(Grid.GetColumn(checkBox) - 1), (int)(Grid.GetRow(checkBox) - 1 + BEGINNING_OF_A_WORKING_DAY)] = (bool)checkBox.IsChecked;

                    tester.VehicleTypesExpertise = 0;
                    foreach (ListBoxItem itemExpertise in vehicleTypeExpertiseListBox.SelectedItems)
                        tester.VehicleTypesExpertise |= (Vehicle)Tools.GetEnumAccordingToUserDisplay(typeof(Vehicle), itemExpertise.Content as string);  //tester.VehicleTypeExpertise = tester.VehicleTypeExpertise.AddFlag(expertise);
                }
                catch (Exception ex)
                {
                    throw new CasingException(true, ex);
                }


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

        private void Refresh_Button_Click(object sender, RoutedEventArgs e)
        {
            List<Test> theTestsOfTheTester = bl.GetTests(test => test.TesterID == tester.ID);
            FutureTestsDataGrid.ItemsSource = theTestsOfTheTester.FindAll(test => test.IsDone() == false);
            TestsDataGrid.ItemsSource = theTestsOfTheTester.FindAll(test => test.IsDone() == true && test.IsPass == null);
            //FutureTestsDataGrid.ItemsSource = bl.GetTests(test => test.TesterID == tester.ID && test.IsDone() == false);
            //TestsDataGrid.ItemsSource = bl.GetTests(test => test.TesterID == tester.ID && test.IsDone() == true && test.IsPass == null);
        }

        private void TestsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (!(TestsDataGrid.SelectedItem is Test test))
                return;

            if (test.IsPass == null) // TODO if refresh in sendclick so delete the if
            {
                GradingTest.Refresh();
                GradingTest.IsEnabled = true;
            }
        }

        private void GradingTest_SendClick(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.UpdateTest((TestsDataGrid.SelectedItem as Test).Code, new Test.Criteria(GradingTest.KeepDistance.IsChecked, GradingTest.BackParking.IsChecked, GradingTest.UsingViewMirrors.IsChecked, GradingTest.Signaling.IsChecked, GradingTest.IntegrationIntoMovement.IsChecked, GradingTest.ObeyParkSigns.IsChecked), (bool)GradingTest.IsPass.IsChecked, GradingTest.Note.Text);
                GradingTest.Refresh();
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



