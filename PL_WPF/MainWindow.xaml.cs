﻿//BS"D

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BE;

namespace PL_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BL.IBL bl = BL.FactorySingleton.Instance;

        public MainWindow()
        {
            InitializeComponent();

            //Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("he-IL");

            new LoginWindow().ShowDialog();

            try
            {
                if (bl.GetTester("323947747") == null)
                    bl.AddTester(new Tester("323947747", new Person.PersonName { FirstName = "Shmuel", LastName = "Garber" }, new DateTime(1950, 7, 13), Gender.male, "0547424870", new Address { Street = "יפו", HouseNumber = 5, City = "ירושלים" }, "1", 10, 30, Vehicle.tractor, new Schedule(new bool[,] { { true, true, true, true, true, true, true }, { true, true, true, true, true, true, true }, { true, true, true, true, true, true, true }, { true, true, true, true, true, true, true }, { true, true, true, true, true, true, true } }), 100));
                if (bl.GetTester("322680083") == null)
                    bl.AddTester(new Tester("322680083", new Person.PersonName { FirstName = "Refael", LastName = "Goldis" }, new DateTime(1949, 5, 12), Gender.male, "0556824870", new Address { Street = "יפו", HouseNumber = 20, City = "ירושלים" }, "1", 6, 16, Vehicle.privateCar, new Schedule(new bool[,] { { false, false, true, true, true, true, true }, { true, true, true, true, true, true, true }, { true, true, true, true, true, true, true }, { true, true, true, true, true, true, true }, { true, true, true, true, true, true, true } }), 100));


                if (bl.GetTrainee("212384507") == null)
                    bl.AddTrainee(new Trainee("212384507", new Person.PersonName { FirstName = "Yael", LastName = "katri" }, new DateTime(1995, 10, 6), Gender.female, "0541234567", new Address { Street = "יפו", HouseNumber = 2, City = "ירושלים" }, "1", Vehicle.privateCar, Gearbox.manual, "TheBest", new Person.PersonName { FirstName = "Shmuel", LastName = "Garber" }, 34, default));
                if (bl.GetTrainee("323947739") == null)
                    bl.AddTrainee(new Trainee("323947739", new Person.PersonName { FirstName = "Asaf", LastName = "Levi" }, new DateTime(1948, 10, 6), Gender.female, "0541234567", new Address { Street = "יפו", HouseNumber = 17, City = "ירושלים" }, "1", Vehicle.privateCar, Gearbox.manual, "TheBest", new Person.PersonName { FirstName = "Shmuel", LastName = "Garber" }, 30, default));

                ////bl.AddTest(trainee1, new DateTime(2019, 2, 6, 10, 0, 0), trainee1.Address, trainee1.VehicleTypeTraining);
                ////bl.AddTest(trainee2, new DateTime(2019, 2, 6, 11, 0, 0), trainee2.Address, trainee2.VehicleTypeTraining);


                new UI.AdminInterface.AdminWindow().Show();

                new UI.TesterInterface.TesterWindow(bl.GetTester("323947747")).Show();
                new UI.TesterInterface.TesterWindow(bl.GetTester("322680083")).Show();

                new UI.TraineeInterface.TraineeWindow(bl.GetTrainee("212384507")).Show();
                new UI.TraineeInterface.TraineeWindow(bl.GetTrainee("323947739")).Show();

                new UI.TesterInterface.TesterRegisteraionWindow().Show();
                new UI.TraineeInterface.TraineeRegisteraionWindow().Show();

                Close();
            }
            catch (CasingException ex) when (ex.DisplayToUser)
            {
                Functions.ShowMessageToUser(ex);
                Close();
            }
            catch (Exception ex)
            {
                Functions.SendMailToAdmin(ex);
                Close();
            }
            Close();
        }
    }
}