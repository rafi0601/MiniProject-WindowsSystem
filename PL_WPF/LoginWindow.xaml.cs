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
using System.Windows.Shapes;

namespace PL_WPF
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Controller_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button)) throw new Exception();

            if (sender == Shutdown_Button)
                switch (MessageBox.Show("Are you sure you want to quit?", "Vertifying pass", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No))
                {
                    case MessageBoxResult.Yes:
                        Close();
                        break;
                }
            else if (sender == Minimize_Button)
                WindowState = WindowState.Minimized;
        }


        private void Controller_MouseEnter(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button button)) throw new Exception();
            if (!(button.Content is MaterialDesignThemes.Wpf.PackIcon packIcon)) throw new Exception();
            packIcon.Foreground = Brushes.Black;


            //     (sender as MaterialDesignThemes.Wpf.PackIcon).Foreground = Brushes.Black;
        }

        private void Controller_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!(sender is Button button)) throw new Exception();
            if (!(button.Content is MaterialDesignThemes.Wpf.PackIcon packIcon)) throw new Exception();
            packIcon.Foreground = Brushes.WhiteSmoke;

            //switch(sender)
            //{
            //    case Button but:
            //        
            //        break;
            //}
            // if (!(sender is Button)) throw new Exception();
            // Shutdown_Icon.Foreground = Brushes.WhiteSmoke; //CHECK Shutdownd_Icon.ערך התחלתי
            // (sender as MaterialDesignThemes.Wpf.PackIcon).Foreground = Brushes.WhiteSmoke;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {

            //TraineeWindow traineeWindow = new TraineeWindow(new Trainee("212384507", new Name { FirstName = "Yael", LastName = "katri" }, new DateTime(1995, 10, 6), Gender.female, "0541234567", new Address { Street = "Franco", HouseNumber = 16, City = "Hadera" }, Vehicle.tractor, Gearbox.manual, "TheBest", new Name { FirstName = "Shmuel", LastName = "Garber" }, 34));
            new UI.TraineeInterface.TraineeRegisteraionWindow().Show();
            Close();

            //  new LoadingWindow().Show();
        }
    }
}
