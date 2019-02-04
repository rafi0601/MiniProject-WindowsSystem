﻿//BS"D

using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BE;

namespace PL_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [DebuggerDisplay("The main window")]
    public partial class MainWindow : Window
    {
        private BL.IBL bl = BL.Singleton.Instance;

        public MainWindow()
        {
            InitializeComponent();
            //new LoginWindow().ShowDialog();
            //new UI.TraineeInterface.TraineeWindow(new BE.Trainee()).ShowDialog();
            //new UI.TraineeInterface.TraineeWindow(bl.GetTrainee("212384507")).Show();
            //new UI.TesterInterface.TesterRegisteraionWindow().Show();
            try
            {
                bl.AddTester(new Tester("323947747", new Name { FirstName = "Shmuel", LastName = "Garber" }, new DateTime(1950, 7, 13), Gender.male, "0547424870", new Address { Street = "Hganenet", HouseNumber = 5, City = "Jerusalem" }, 10, 30, Vehicle.tractor, new bool[,] { { true, true, true, true, true, true, true }, { true, true, true, true, true, true, true }, { true, true, true, true, true, true, true }, { true, true, true, true, true, true, true }, { true, true, true, true, true, true, true } }, 100));
                bl.AddTester(new Tester("322680083", new Name { FirstName = "Refael", LastName = "Goldis" }, new DateTime(1949, 5, 12), Gender.male, "0556824870", new Address { Street = "Gordon", HouseNumber = 22, City = "Tel-Aviv" }, 6, 16, Vehicle.privateCar, new bool[,] { { true, true, true, true, true, true, true }, { true, true, true, true, true, true, true }, { true, true, true, true, true, true, true }, { true, true, true, true, true, true, true }, { true, true, true, true, true, true, true } }, 16));

                Trainee trainee1 = new Trainee("212384507", new Name { FirstName = "Yael", LastName = "katri" }, new DateTime(1995, 10, 6), Gender.female, "0541234567", new Address { Street = "Franco", HouseNumber = 16, City = "Hadera" }, Vehicle.privateCar, Gearbox.manual, "TheBest", new Name { FirstName = "Shmuel", LastName = "Garber" }, 34);
                Trainee trainee2 = new Trainee("323947739", new Name { FirstName = "Asaf", LastName = "Levi" }, new DateTime(1948, 10, 6), Gender.female, "0541234567", new Address { Street = "Hatmarim", HouseNumber = 17, City = "Eilat" }, Vehicle.privateCar, Gearbox.manual, "TheBest", new Name { FirstName = "Shmuel", LastName = "Garber" }, 34);

                bl.AddTrainee(trainee1);
                bl.AddTrainee(trainee2);

                bl.AddTest(trainee1, new DateTime(2019, 1, 27, 10, 0, 0), trainee1.Address, trainee1.VehicleTypeTraining);
                bl.AddTest(trainee2, new DateTime(2019, 1, 24, 11, 0, 0), trainee2.Address, trainee2.VehicleTypeTraining);

                new UI.TesterInterface.TesterWindow(bl.GetTester("323947747")).Show();
                new UI.TesterInterface.TesterWindow(bl.GetTester("322680083")).Show();

                new UI.TraineeInterface.TraineeWindow(trainee1).Show();
                new UI.TraineeInterface.TraineeWindow(trainee2).Show();

                //new UI.TesterInterface.TesterRegisteraionWindow().Show();
                //new UI.TraineeInterface.TraineeRegisteraionWindow().Show();

                Close();
            }
            catch(CustomException ex) when (ex.DisplayToUser)
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.No);
                Close();
            }
            
            Close();
        }
    }
}