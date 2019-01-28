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
using BE;

namespace PL_WPF
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        IUserManager userManager = new UsersManager(new User());
        BL.IBL bl = BL.Singleton.Instance;

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
            new UI.TraineeInterface.TraineeRegisteraionWindow().Show();
            Close();

        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            User loginedUser = userManager.Get(idTextBox.Text);
            if (loginedUser.name is null || loginedUser.password != passwordTextBox.Password)
                throw new Exception("not exist");

            switch (loginedUser.role)
            {
                case Type type when type == typeof(Tester):
                    new UI.TesterInterface.TesterWindow(bl.GetTester(loginedUser.name)).Show();
                    break;
                case Type type when type == typeof(Trainee):
                    new UI.TraineeInterface.TraineeWindow(bl.GetTrainee(loginedUser.name)).Show();
                    break;
                default:
                    new UI.AdminInterface.AdminWindow().Show();
                    break;
            }

        }

        private void Registeration_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            switch (MessageBox.Show("Are you tester", "", MessageBoxButton.YesNo, MessageBoxImage.Question)
)
            {
                case MessageBoxResult.Yes:
                    new UI.TesterInterface.TesterRegisteraionWindow().Show();
                    break;
                case MessageBoxResult.No:
                    new UI.TraineeInterface.TraineeRegisteraionWindow().Show();
                    break;
                default:
                    break;
            }
        }
    }
}
