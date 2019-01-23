//BS"D

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
        private BL.IBL bl = BL.Singleton.Instance;

        public MainWindow()
        {
            InitializeComponent();
            //new LoginWindow().ShowDialog();
            //new UI.TraineeInterface.TraineeWindow(new BE.Trainee()).ShowDialog();



            bl.AddTester(new Tester("323947747", new Name { FirstName = "Shmuel", LastName = "Garber" }, new DateTime(1950, 7, 13), Gender.male, "0547424870", new Address { Street = "Hganenet", HouseNumber = 5, City = "Jerusalem" }, 10, 30, Vehicle.tractor, new bool[,] { { true, false, false, false, false, false, true, false }, { true, false, false, true, false, false, false, false }, { true, false, false, false, false, false, false, false }, { true, false, false, false, false, false, false, false }, { true, false, false, false, false, false, false, false } }, 20));
            bl.AddTester(new Tester("322680083", new Name { FirstName = "Refael", LastName = "Goldis" }, new DateTime(1949, 5, 12), Gender.male, "0556824870", new Address { Street = "Gordon", HouseNumber = 22, City = "Tel-Aviv" }, 6, 16, Vehicle.heavyTruck, new bool[,] { { true, true, false, true, false, false, false, true }, { true, false, false, true, false, true, false, false }, { true, false, false, false, false, false, false, true }, { true, false, false, true, false, false, false, false }, { true, false, false, false, false, false, false, false } }, 16));
            bl.AddTrainee(new Trainee("212384507", new Name { FirstName = "Yael", LastName = "katri" }, new DateTime(1995, 10, 6), Gender.female, "0541234567", new Address { Street = "Franco", HouseNumber = 16, City = "Hadera" }, Vehicle.tractor, Gearbox.manual, "TheBest", new Name { FirstName = "Shmuel", LastName = "Garber" }, 34));
            bl.AddTrainee(new Trainee("323947739", new Name { FirstName = "Asaf", LastName = "Levi" }, new DateTime(1948, 10, 6), Gender.female, "0541234567", new Address { Street = "Hatmarim", HouseNumber = 16, City = "Eilat" }, Vehicle.heavyTruck, Gearbox.manual, "TheBest", new Name { FirstName = "Shmuel", LastName = "Garber" }, 34));


            new UI.TraineeInterface.TraineeWindow(bl.GetTrainee("212384507")).Show();

            Close();
        }

        private void DateTimePicker_SelectionChanged(object sender, EventArgs e)
        {

        }
    }
}
