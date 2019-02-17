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
using static BE.Configuration;

namespace PL_WPF.UI.TesterInterface
{
    /// <summary>
    /// Interaction logic for TesterRegisteraionWindow.xaml
    /// </summary>
    public partial class TesterRegisteraionWindow : Window
    {
        Tester tester = new Tester();
        BL.IBL bl = BL.FactorySingleton.Instance;


        public TesterRegisteraionWindow()
        {
            InitializeComponent();

            tester.Birthdate = new DateTime(1900, 1, 1);

            genderComboBox.ItemsSource = Enum.GetValues(typeof(Gender));
            vehicleTypeExpertiseListBox.ItemsSource = Functions.VehiclesToDisplayStrings();

            for (int i = 0; i < WORKING_DAYS_A_WEEK; i++)
            {
                for (int j = 0; j < WORKING_HOURS_A_DAY; j++)
                {
                    CheckBox checkBox = new CheckBox
                    {
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                    };
                    checkBox.SetBinding(CheckBox.IsCheckedProperty,
                        new Binding("IsChecked")
                        {
                            Source = markAllCheckBox,
                            Mode = BindingMode.OneWay
                        });

                    Grid.SetColumn(checkBox, i + 1);
                    Grid.SetRow(checkBox, j + 1);

                    scheduleGrid.Children.Add(checkBox);
                }
            }

            DataContext = tester;


            //this.Loaded += TesterRegisteraionWindow_Loaded;
        }


        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (passwordBoxNew.Password != passwordBoxAuthentication.Password)
                    throw new CasingException(true, new Exception("The authentication password not correct."));

                if (bl.GetTrainee(iDTextBox.Text) != null)
                    throw new CasingException(true, new Exception("Alredy exist"));

                try
                {

                    tester.Name = new Person.PersonName(lastNameTextBox.Text, firstNameTextBox.Text);
                    tester.Address = new Address(Street.Text, uint.Parse(HouseNumber.Text), City.Text);
                    tester.Password = passwordBoxNew.Password;

                }
                catch (Exception ex)
                {
                    throw new CasingException(true, ex);
                }

                foreach (var item in scheduleGrid.Children)
                    if (item is CheckBox checkBox && checkBox != markAllCheckBox)
                        tester.WorkingHours[(DayOfWeek)(Grid.GetColumn(checkBox) - 1), (int)(Grid.GetRow(checkBox) - 1 + BEGINNING_OF_A_WORKING_DAY)] = (bool)checkBox.IsChecked;

                foreach (string expertise in vehicleTypeExpertiseListBox.SelectedItems)
                    tester.VehicleTypesExpertise |= (Vehicle)Tools.GetEnumAccordingToUserDisplay(typeof(Vehicle), expertise);  //tester.VehicleTypeExpertise = tester.VehicleTypeExpertise.AddFlag(expertise);


                iDTextBox.GetBindingExpression(Xceed.Wpf.Toolkit.MaskedTextBox.TextProperty).UpdateSource();

                bl.AddTester(tester);

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


        //private void TesterRegisteraionWindow_Loaded(object sender, RoutedEventArgs e)
        //{
        //}
    }
}