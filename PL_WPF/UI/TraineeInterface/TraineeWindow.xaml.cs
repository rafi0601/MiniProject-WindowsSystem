﻿//BS"D

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
        BL.IBL bl = BL.FactorySingleton.Instance;

        public TraineeWindow(Trainee trainee)
        {
            if (trainee == null)
                throw new CasingException(false, new ArgumentNullException("Cannot show null"));

            InitializeComponent();

            DataContext = this.trainee = trainee;
            //this.Closing += (sender, e) => Environment.Exit(Environment.ExitCode);
            this.DetailsOfMyTest.RefreshButtonClick += DetailsOfMyTest_RefreshButtonClick;

            gearboxComboBox.ItemsSource = Enum.GetValues(typeof(Gearbox));
            genderComboBox.ItemsSource = Enum.GetValues(typeof(Gender));

            firstNameTextBox.Text = trainee.Name.FirstName;
            lastNameTextBox.Text = trainee.Name.LastName;
            TeacherFirstNameTextBox.Text = trainee.TeacherName.FirstName;
            TeacherLastNameTextBox.Text = trainee.TeacherName.LastName;
            City.Text = trainee.Address.City;
            HouseNumber.Text = trainee.Address.HouseNumber.ToString();
            Street.Text = trainee.Address.Street;

            ////vehicleCheckListBox.SelectAll();
            //List<object> list = new List<object>();
            //foreach (Vehicle vehicle in Enum.GetValues(typeof(Vehicle)).Cast<Vehicle>())
            //    if (trainee.VehicleTypeTraining.HasFlag(vehicle))
            //        list.Add(vehicle);
            //vehicleListBox.SelectedItems.Add = list;


            //vehicleListBox.ItemsSource = from vehicle in Enum.GetValues(typeof(Vehicle)).Cast<Vehicle>()
            //                             where trainee.VehicleTypeTraining.HasFlag(vehicle)
            //                             select Tools.GetUserDisplayAttribute(vehicle)?.DisplayName ?? vehicle.ToString();
            foreach (Vehicle vehicle in Enum.GetValues(typeof(Vehicle)).Cast<Vehicle>())
                if (trainee.VehicleTypeTraining.HasFlag(vehicle))
                {
                    vehicleListBox.Items.Add(new ListBoxItem() { Content = Tools.GetUserDisplayAttribute(vehicle)?.DisplayName ?? vehicle.ToString() });
                    vehicleListBox.SelectedItem = vehicleListBox.Items[0];
                    break;
                }
            foreach (Vehicle vehicle in Enum.GetValues(typeof(Vehicle)).Cast<Vehicle>())
                if (!trainee.VehicleTypeTraining.HasFlag(vehicle))
                    vehicleListBox.Items.Add(new ListBoxItem() { Content = Tools.GetUserDisplayAttribute(vehicle)?.DisplayName ?? vehicle.ToString() });


            DadaGridOfDoneTests.SelectedItem = bl.GetTests(t => t.TraineeID == trainee.ID && t.IsPass != null);

            if (bl.GetTests(t => t.TraineeID == trainee.ID).Any())
            {
                //dateTimePicker.Visibility = CheckDateButton.Visibility = ChooseLabel.Visibility = Visibility.Collapsed;
                choosingControls.Visibility = Visibility.Collapsed;
                DetailsOfMyTest.MyTestDadaGrid.ItemsSource = bl.GetTests(t => t.TraineeID == trainee.ID && t.Vehicle == trainee.VehicleTypeTraining && t.IsPass == null);
                detailsControls.Visibility = Visibility.Visible;
            }

            Grading.sendButton.Visibility = Visibility.Collapsed;
        }


        private void UpdateButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                try
                {
                    trainee.Name = new Person.PersonName(lastNameTextBox.Text, firstNameTextBox.Text);
                    trainee.TeacherName = new Person.PersonName(TeacherLastNameTextBox.Text, TeacherFirstNameTextBox.Text);
                    trainee.Address = new Address(Street.Text, uint.Parse(HouseNumber.Text), City.Text);

                    trainee.VehicleTypeTraining = 0;
                    foreach (ListBoxItem itemExpertise in vehicleListBox.SelectedItems)
                        trainee.VehicleTypeTraining |= (Vehicle)Tools.GetEnumAccordingToUserDisplay(typeof(Vehicle), itemExpertise.Content as string);  //tester.VehicleTypeExpertise = tester.VehicleTypeExpertise.AddFlag(expertise);
                }
                catch (Exception ex)
                {
                    throw new CasingException(true, ex);
                }


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


        private void DateTimePicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckDateButton.IsEnabled = true;
            //worker.DoWork += Worker_DoWork;
            //worker.ProgressChanged += Worker_ProgressChanged;
            //worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
        }


        private DateTime? alternateDate;

        private async void CheckDateButton_Click_Async(object sender, RoutedEventArgs e)
        {
            try
            {
                choosingControls.Visibility = Visibility.Collapsed;// CheckDateButton.Visibility = ChooseLabel.Visibility = Visibility.Collapsed;
                //busyIndicator.Visibility = Visibility.Visible;

                DateTime dt = dateTimePicker.DateTime;
                Exception exception = null;

                await Task.Run(new Action(() =>
                {
                    try
                    {
                        alternateDate = bl.AddTest(trainee, dt, trainee.Address, trainee.VehicleTypeTraining);
                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                    }
                }));

                //busyIndicator.Visibility = Visibility.Collapsed;

                if (exception != null)
                    throw exception;

                //Thread thread = new Thread(delegate () { AlternateDate = bl.AddTest(trainee, dateTimePicker.DateTime, /*new DateTime(),*/ trainee.Address, trainee.VehicleTypeTraining); });

                if (alternateDate == null)
                {
                    DetailsOfMyTest.MyTestDadaGrid.ItemsSource = bl.GetTests(t => t.TraineeID == trainee.ID && t.Vehicle == trainee.VehicleTypeTraining && t.IsPass == null);
                    detailsControls.Visibility = Visibility.Visible;
                    return;
                }
                else
                {
                    //SuggestAlternateDateOfTest.Date.Text = alternateDate.Value.ToString("g");
                    SuggestAlternateDateOfTest.calendar.SelectedDate = alternateDate;
                    suggestControls.Visibility = Visibility.Visible;
                }
            }
            catch (CasingException ex) when (ex.DisplayToUser)
            {
                Functions.ShowMessageToUser(ex);
                choosingControls.Visibility = Visibility.Visible;
                //dateTimePicker.Visibility = CheckDateButton.Visibility = ChooseLabel.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                Functions.SendMailToAdmin(ex);
                Close();
            }
        }

        private async void SuggestAlternateDateOfTest_AcceptClick_Async(object sender, RoutedEventArgs e)
        {
            try
            {
                suggestControls.Visibility = Visibility.Collapsed;
                //busyIndicator.Visibility = Visibility.Visible;

                await Task.Run(new Action(() =>
                {
                    //בהנחה שאין בתוכנית תהליכונים אז זה יקבע בטוח
                    bl.AddTest(trainee, (DateTime)alternateDate, trainee.Address, trainee.VehicleTypeTraining);
                }));

                //busyIndicator.Visibility = Visibility.Collapsed;

                DetailsOfMyTest.MyTestDadaGrid.ItemsSource = bl.GetTests(t => t.TraineeID == trainee.ID && t.Vehicle == trainee.VehicleTypeTraining && t.IsPass == null);
                detailsControls.Visibility = Visibility.Visible;
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
            suggestControls.Visibility = Visibility.Collapsed;
            choosingControls.Visibility = Visibility.Visible;
            //dateTimePicker.Visibility = CheckDateButton.Visibility = ChooseLabel.Visibility = Visibility.Visible;
        }

        private void Refresh_Button_Click(object sender, RoutedEventArgs e)
        {
            DadaGridOfDoneTests.ItemsSource = bl.GetTests(t => t.TraineeID == trainee.ID && t.IsPass != null);
        }

        private void DetailsOfMyTest_RefreshButtonClick(object sender, RoutedEventArgs e)
        {
            List<Test> testsOfTheTrainee = bl.GetTests(test => test.TraineeID == trainee.ID);
            if (testsOfTheTrainee.TrueForAll(t => t.IsPass == false))
            {
                detailsControls.Visibility = Visibility.Collapsed;
                choosingControls.Visibility = Visibility.Visible;
                //dateTimePicker.Visibility = CheckDateButton.Visibility = ChooseLabel.Visibility = Visibility.Visible;
            }
            else
            {
                DetailsOfMyTest.MyTestDadaGrid.ItemsSource = testsOfTheTrainee.FindAll(test => test.Vehicle == trainee.VehicleTypeTraining && test.IsPass == null); //IMPROVEMENT change findall to find
            }
        }

        private void DadaGridOfDoneTests_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (!(DadaGridOfDoneTests.SelectedItem is Test test))
                return;

            Grading.Note.IsEnabled = Grading.BackParking.IsEnabled = Grading.IntegrationIntoMovement.IsEnabled = Grading.IsPass.IsEnabled = Grading.KeepDistance.IsEnabled = Grading.ObeyParkSigns.IsEnabled = Grading.Signaling.IsEnabled = Grading.UsingViewMirrors.IsEnabled = false;

            Grading.BackParking.IsChecked = test.CriteriasGrades.BackParking;
            Grading.IntegrationIntoMovement.IsChecked = test.CriteriasGrades.IntegrationIntoTraffic;
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

        private void TabItem_ToolTipOpening(object sender, ToolTipEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)sender;
            Point p = Mouse.GetPosition(element);

            if (p.X < 0 || p.X > element.ActualWidth || p.Y < 0 || p.Y > element.ActualHeight)
            {
                e.Handled = true;
            }
        }

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

    }


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