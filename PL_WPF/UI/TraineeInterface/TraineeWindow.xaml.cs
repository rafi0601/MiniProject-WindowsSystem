//BS"D

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for TraineeWindow.xaml
    /// </summary>
    public partial class TraineeWindow : Window
    {
        Trainee trainee;
        BL.IBL bl = BL.Singleton.Instance;

        public TraineeWindow(Trainee trainee)
        {
            InitializeComponent();

            DataContext = this.trainee = trainee;

            gearboxComboBox.ItemsSource = Enum.GetValues(typeof(Gearbox));//.SplitByUpperAndLower();
            genderComboBox.ItemsSource = Enum.GetValues(typeof(Gender));//.SplitByUpperAndLower();
            vehicleListBox.ItemsSource = from vehicle in Enum.GetValues(typeof(Vehicle)).Cast<Vehicle>()
                                         select Tools.GetUserDisplayAttribute(vehicle)?.DisplayName;

            firstNameTextBox.Text = trainee.Name.FirstName;
            lastNameTextBox.Text = trainee.Name.LastName;
            Street.Text = trainee.Address.Street;
            HouseNumber.Text = trainee.Address.HouseNumber.ToString();
            City.Text = trainee.Address.City;
            TeacherFirstNameTextBox.Text = trainee.TeacherName.FirstName;
            TeacherLastNameTextBox.Text = trainee.TeacherName.LastName;

            foreach (Vehicle vehicle in Enum.GetValues(typeof(Vehicle)))
                if (trainee.VehicleTypeTraining.HasFlag(vehicle))
                    vehicleListBox.SelectedItems.Add(Tools.GetUserDisplayAttribute(vehicle)?.DisplayName);

            DadaGridOfDoneTests.SelectedItem = bl.GetTests(t => t.TraineeID == trainee.ID && t.IsPass != null);

            if (bl.GetTests(t => t.TraineeID == trainee.ID).Any())
            {
                dateTimePicker.Visibility = CheckDateButton.Visibility = ChooseLabel.Visibility = Visibility.Collapsed;
                DetailsOfMyTest.MyTestDadaGrid.ItemsSource = bl.GetTests(t => t.TraineeID == trainee.ID && t.Vehicle == trainee.VehicleTypeTraining);
                DetailsOfMyTest.Visibility = Visibility.Visible;
            }

            Grading.sendButton.Visibility = Visibility.Collapsed;
        }

        private void UpdateButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                trainee.Name = new Person.PersonName { FirstName = firstNameTextBox.Text, LastName = lastNameTextBox.Text };
                trainee.TeacherName = new Person.PersonName { FirstName = TeacherFirstNameTextBox.Text, LastName = TeacherLastNameTextBox.Text };
                trainee.Address = new Address { City = City.Text, HouseNumber = uint.Parse(HouseNumber.Text), Street = Street.Text };

                trainee.VehicleTypeTraining = 0;
                foreach (string expertise in vehicleListBox.SelectedItems)
                    trainee.VehicleTypeTraining |= (Vehicle)Tools.GetEnum(typeof(Vehicle), expertise);  //tester.VehicleTypeExpertise = tester.VehicleTypeExpertise.AddFlag(expertise);


                bl.UpdateTrainee(trainee);
                new UI.TraineeInterface.TraineeWindow(trainee).Show();
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
                        bl.RemoveTrainee(trainee);
                        Close();
                        // CHECK it is unreachable code???
                        trainee = new Trainee();
                        DataContext = trainee;
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

        //private void DateTimePicker_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        //{
        //    switch (MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Question, MessageBoxResult.No))
        //    {
        //
        //        case MessageBoxResult.OK:
        //            try
        //            {
        //                bl.AddTest(trainee, (DateTime)addTestDateTimePicker.Value, new DateTime(), trainee.Address, trainee.Vehicle);
        //                temp.Text = bl.GetTest("00000001").ToString();
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.No);
        //            }
        //            break;
        //    }
        //}

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource testViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("testViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // testViewSource.Source = [generic data source]
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource testViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("testViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // testViewSource.Source = [generic data source]
        }

        private void DateTimePicker_SelectionChanged(object sender, EventArgs e)
        {
            CheckDateButton.IsEnabled = true;
            //worker.DoWork += Worker_DoWork;
            //worker.ProgressChanged += Worker_ProgressChanged;
            //worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
        }

        //private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    try
        //    {
        //        bl.AddTest(trainee, (DateTime)AlternateDate, /*new DateTime(),*/ trainee.Address, trainee.VehicleTypeTraining);
        //        SuggestAlternateDateOfTest.Visibility = Visibility.Collapsed;
        //        DetailsOfMyTest.MyTestDadaGrid.ItemsSource = bl.GetTests(t => t.TraineeID == trainee.ID && t.Vehicle == trainee.VehicleTypeTraining && t.IsDone() == false);
        //        DetailsOfMyTest.Visibility = Visibility.Visible;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.No);
        //    }
        //}
        //
        //private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        //{
        //    try
        //    {
        //        if (AlternateDate == null)
        //        {
        //            dateTimePicker.Visibility = Visibility.Collapsed;
        //            CheckDateButton.Visibility = Visibility.Collapsed;
        //            ChooseLabel.Visibility = Visibility.Collapsed;
        //            DetailsOfMyTest.MyTestDadaGrid.ItemsSource = bl.GetTests(t => t.TraineeID == trainee.ID && t.Vehicle == trainee.VehicleTypeTraining && t.IsDone() == false);
        //            DetailsOfMyTest.Visibility = Visibility.Visible;
        //            return;
        //        }
        //        else
        //        {
        //            dateTimePicker.Visibility = Visibility.Collapsed;
        //            CheckDateButton.Visibility = Visibility.Collapsed;
        //            ChooseLabel.Visibility = Visibility.Collapsed;
        //            SuggestAlternateDateOfTest.Date.Text = AlternateDate.ToString();
        //            SuggestAlternateDateOfTest.Visibility = Visibility.Visible;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.No);
        //    }
        //}

        //private void Worker_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        AlternateDate = bl.AddTest(trainee, dateTimePicker.DateTime, /*new DateTime(),*/ trainee.Address, trainee.VehicleTypeTraining);
        //        (sender as BackgroundWorker).ReportProgress(Configuration.rand.Next(0, 100));
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.No);
        //    }
        //}

        //BackgroundWorker worker;
        private DateTime? AlternateDate;

        private async void CheckDateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Thread thread = new Thread(delegate () { AlternateDate = bl.AddTest(trainee, dateTimePicker.DateTime, /*new DateTime(),*/ trainee.Address, trainee.VehicleTypeTraining); });

                dateTimePicker.Visibility = CheckDateButton.Visibility = ChooseLabel.Visibility = Visibility.Collapsed;

                Task<DateTime?> Foo() { return bl.AddTest(trainee, dateTimePicker.DateTime, trainee.Address, trainee.VehicleTypeTraining); }
                AlternateDate = await Foo();
                if (AlternateDate == null)
                {
                    DetailsOfMyTest.MyTestDadaGrid.ItemsSource = bl.GetTests(t => t.TraineeID == trainee.ID && t.Vehicle == trainee.VehicleTypeTraining);
                    DetailsOfMyTest.Visibility = Visibility.Visible;
                    return;
                }
                else
                {
                    SuggestAlternateDateOfTest.Date.Text = AlternateDate.ToString();
                    SuggestAlternateDateOfTest.Visibility = Visibility.Visible;
                }
            }
            catch (CasingException ex) when (ex.DisplayToUser)
            {
                Functions.ShowMessageToUser(ex);
                dateTimePicker.Visibility = CheckDateButton.Visibility = ChooseLabel.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                Functions.SendMailToAdmin(ex);
                Close();
            }
        }

        private void SuggestAlternateDateOfTest_AcceptClick(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.AddTest(trainee, (DateTime)AlternateDate, trainee.Address, trainee.VehicleTypeTraining);
                SuggestAlternateDateOfTest.Visibility = Visibility.Collapsed;
                DetailsOfMyTest.MyTestDadaGrid.ItemsSource = bl.GetTests(t => t.TraineeID == trainee.ID && t.Vehicle == trainee.VehicleTypeTraining);
                DetailsOfMyTest.Visibility = Visibility.Visible;
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

        private void SuggestAlternateDateOfTest_CancelClick(object sender, RoutedEventArgs e)
        {
            dateTimePicker.Visibility = Visibility.Visible;
            CheckDateButton.Visibility = Visibility.Visible;
            ChooseLabel.Visibility = Visibility.Visible;
            SuggestAlternateDateOfTest.Visibility = Visibility.Collapsed;
        }

        private void Refrash_Button_Click(object sender, RoutedEventArgs e)
        {
            DadaGridOfDoneTests.ItemsSource = bl.GetTests(t => t.TraineeID == trainee.ID && t.IsPass != null);
        }

        private void DetailsOfMyTest_RefrashButtonClick(object sender, RoutedEventArgs e)
        {
            if (bl.GetTests(t => t.TraineeID == trainee.ID).TrueForAll(t => t.IsPass == false))
            {
                DetailsOfMyTest.Visibility = Visibility.Collapsed;
                dateTimePicker.Visibility = CheckDateButton.Visibility = ChooseLabel.Visibility = Visibility.Visible;
            }
            else
            {
                DetailsOfMyTest.MyTestDadaGrid.ItemsSource = bl.GetTests(t => t.TraineeID == trainee.ID && t.Vehicle == trainee.VehicleTypeTraining && t.IsPass == null);
            }
        }

        private void DadaGridOfDoneTests_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var test = DadaGridOfDoneTests.SelectedItem as Test;

            if (test == null)
                return;

            Grading.Note.IsEnabled = Grading.BackParking.IsEnabled = Grading.IntegrationIntoMovement.IsEnabled = Grading.IsPass.IsEnabled = Grading.KeepDistance.IsEnabled = Grading.ObeyParkSigns.IsEnabled = Grading.Signaling.IsEnabled = Grading.UsingViewMirrors.IsEnabled = false;

            Grading.BackParking.IsChecked = test.CriteriasGrades.BackParking;
            Grading.IntegrationIntoMovement.IsChecked = test.CriteriasGrades.IntegrationIntoMovement;
            Grading.IsPass.IsChecked = test.IsPass;
            Grading.KeepDistance.IsChecked = test.CriteriasGrades.KeepDistance;
            Grading.Note.Text = test.TesterNotes;
            Grading.ObeyParkSigns.IsChecked = test.CriteriasGrades.ObeyParkSigns;
            Grading.Signaling.IsChecked = test.CriteriasGrades.Signaling;
            Grading.UsingViewMirrors.IsChecked = test.CriteriasGrades.UsingViewMirrors;
        }

        private void UpdatePasswordButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (passwordBoxNew.Password != passwordBoxAuthentication.Password)
                    throw new CasingException(true, new ArgumentException("Password authentication is not equivalent to the password"));

                trainee.Password = passwordBoxNew.Password;
                bl.UpdateTrainee(trainee);
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
    }
}
