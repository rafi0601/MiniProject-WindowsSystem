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
        BL.IBL bl = BL.FactorySingleton.Instance;

        public LoginWindow()
        {
            InitializeComponent();
        }


        private void Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (idTextBox.Text.ToUpper().CompareTo("ADMIN") == 0 && passwordPasswordBox.Password.ToUpper().CompareTo("ADMIN") == 0)
                {
                    new UI.AdminInterface.AdminWindow().Show();
                    idTextBox.Text = passwordPasswordBox.Password = string.Empty;
                    return;
                }

                Person person = (Person)bl.GetTester(idTextBox.Text) ?? bl.GetTrainee(idTextBox.Text);
                if (person is null || person.Password != passwordPasswordBox.Password)
                    throw new CasingException(true, new Exception("Not exist."));

                switch (person)
                {
                    case Tester tester:
                        new UI.TesterInterface.TesterWindow(tester).Show();
                        break;
                    case Trainee trainee:
                        new UI.TraineeInterface.TraineeWindow(trainee).Show();
                        break;
                    default:
                        new UI.AdminInterface.AdminWindow().Show();
                        break;
                }
                
                idTextBox.Text = passwordPasswordBox.Password = string.Empty;
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

        private void Registeration_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RoleInput roleInput = new RoleInput();
            roleInput.ShowDialog();
            //roleInput.RoleListBox = new ListBox(); //BUG how it is possible?
            switch (roleInput.RoleName)
            {
                case "Tester":
                case "tester":
                    new UI.TesterInterface.TesterRegisteraionWindow().Show();
                    break;
                case "Trainee":
                case "trainee":
                    new UI.TraineeInterface.TraineeRegisteraionWindow().Show();
                    break;
                //default:
                //    throw new CasingException(false,new Exception("No valid name of type"));
            }
        }


        private void Shutdown_Click(object sender, RoutedEventArgs e)
        {
            switch (MessageBox.Show("Are you sure you want to quit?", "Vertifying pass", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No))
            {
                case MessageBoxResult.Yes:
                    Close();
                    break;
            }
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Languages_Click(object sender, RoutedEventArgs e)
        {

        }


        //private void Controller_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    if (!(sender is Button button)) throw new Exception();
        //    if (!(button.Content is MaterialDesignThemes.Wpf.PackIcon packIcon)) throw new Exception();
        //    packIcon.Foreground = Brushes.Black;
        //}
        //
        //private void Controller_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    if (!(sender is Button button)) throw new Exception();
        //    if (!(button.Content is MaterialDesignThemes.Wpf.PackIcon packIcon)) throw new Exception();
        //    packIcon.Foreground = Brushes.WhiteSmoke;//CHECK Shutdownd_Icon.ערך התחלתי
        //}

        //private void Controller_Click(object sender, RoutedEventArgs e)
        //{
        //    if (!(sender is Button)) throw new Exception();
        //
        //    if (sender == Shutdown_Button)
        //        switch (MessageBox.Show("Are you sure you want to quit?", "Vertifying pass", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No))
        //        {
        //            case MessageBoxResult.Yes:
        //                Close();
        //                break;
        //        }
        //    else if (sender == Minimize_Button)
        //        WindowState = WindowState.Minimized;
        //}

    }

    class HelperPassword : DependencyObject
    {
        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }
    
        // Using a DependencyProperty as the backing store for Password.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(HelperPassword), new UIPropertyMetadata(string.Empty));
    
        public void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
                Password = passwordBox.Password;
            else
                throw new CasingException(false, new InvalidOperationException(nameof(sender) + " is not " + nameof(PasswordBox)));
        }
    }
}
