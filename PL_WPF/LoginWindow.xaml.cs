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

        private void Controller_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!(sender is Button button)) throw new Exception();
            if (!(button.Content is MaterialDesignThemes.Wpf.PackIcon packIcon)) throw new Exception();
            packIcon.Foreground = Brushes.Black;
        }
        private void Controller_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!(sender is Button button)) throw new Exception();
            if (!(button.Content is MaterialDesignThemes.Wpf.PackIcon packIcon)) throw new Exception();
            packIcon.Foreground = Brushes.WhiteSmoke;//CHECK Shutdownd_Icon.ערך התחלתי
        }


        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    new UI.TraineeInterface.TraineeRegisteraionWindow().Show();
        //    Close();
        //
        //}

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (idTextBox.Text.ToUpper().CompareTo("ADMIN") == 0 && passwordPasswordBox.Password.ToUpper().CompareTo("ADMIN") == 0)
                {
                    new UI.AdminInterface.AdminWindow().Show();
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
                    new UI.TesterInterface.TesterRegisteraionWindow().Show();
                    break;
                case "Trainee":
                    new UI.TraineeInterface.TraineeRegisteraionWindow().Show();
                    break;
                default:
                    throw new Exception("");
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
    }

    public static class PasswordHelper
    {
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password",
            typeof(string), typeof(PasswordHelper),
            new FrameworkPropertyMetadata(string.Empty, OnPasswordPropertyChanged));

        public static readonly DependencyProperty AttachProperty =
            DependencyProperty.RegisterAttached("Attach",
            typeof(bool), typeof(PasswordHelper), new PropertyMetadata(false, Attach));

        private static readonly DependencyProperty IsUpdatingProperty =
           DependencyProperty.RegisterAttached("IsUpdating", typeof(bool),
           typeof(PasswordHelper));


        public static void SetAttach(DependencyObject dp, bool value)
        {
            dp.SetValue(AttachProperty, value);
        }

        public static bool GetAttach(DependencyObject dp)
        {
            return (bool)dp.GetValue(AttachProperty);
        }

        public static string GetPassword(DependencyObject dp)
        {
            return (string)dp.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject dp, string value)
        {
            dp.SetValue(PasswordProperty, value);
        }

        private static bool GetIsUpdating(DependencyObject dp)
        {
            return (bool)dp.GetValue(IsUpdatingProperty);
        }

        private static void SetIsUpdating(DependencyObject dp, bool value)
        {
            dp.SetValue(IsUpdatingProperty, value);
        }

        private static void OnPasswordPropertyChanged(DependencyObject sender,
            DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            passwordBox.PasswordChanged -= PasswordChanged;

            if (!(bool)GetIsUpdating(passwordBox))
            {
                passwordBox.Password = (string)e.NewValue;
            }
            passwordBox.PasswordChanged += PasswordChanged;
        }

        private static void Attach(DependencyObject sender,
            DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;

            if (passwordBox == null)
                return;

            if ((bool)e.OldValue)
            {
                passwordBox.PasswordChanged -= PasswordChanged;
            }

            if ((bool)e.NewValue)
            {
                passwordBox.PasswordChanged += PasswordChanged;
            }
        }

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            SetIsUpdating(passwordBox, true);
            SetPassword(passwordBox, passwordBox.Password);
            SetIsUpdating(passwordBox, false);
        }
    }
}
