using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security;
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

namespace PL_WPF
{
    /// <summary>
    /// Interaction logic for DependencyPasswordBox.xaml
    /// </summary>
    public partial class DependencyPasswordBox : UserControl, INotifyPropertyChanged
    {
        public DependencyPasswordBox()
        {
            InitializeComponent();
            passwordBox.PasswordChanged += PasswordPasswordBox_PasswordChanged;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void PasswordPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            //Password = passwordPasswordBox.Password;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Password)));
        }



        //public string Password
        //{
        //    get { return (string)GetValue(PasswordProperty); }
        //    set { SetValue(PasswordProperty, value); }
        //}
        //
        //// Using a DependencyProperty as the backing store for Password.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty PasswordProperty =
        //    DependencyProperty.Register("Password", typeof(string), typeof(DependencyPasswordBox), new PropertyMetadata(string.Empty, PasswordPropertyChangedCallback));
        //
        //private static void PasswordPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    DependencyPasswordBox dependencyPasswordBox = d as DependencyPasswordBox;
        //    dependencyPasswordBox.passwordBox.Password = dependencyPasswordBox.Password;
        //}
        //public static object PasswordCoerceValueCallback(DependencyObject d, object baseValue)
        //{
        //    string password = (baseValue as string);
        //    DependencyPasswordBox dependencyPasswordBox = (d as DependencyPasswordBox);
        //    return password.Length <= dependencyPasswordBox.MaxLength ? baseValue : password.Substring(dependencyPasswordBox.MaxLength);
        //    //return (d as DependencyPasswordBox).passwordBox.Password = baseValue as string;
        //}




        public string Password
        {
            get => passwordBox.Password;

            set
            {
                passwordBox.Password = value;
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Password)));
            }
        }

        public int MaxLength
        {
            get => passwordBox.MaxLength;

            set
            {
                passwordBox.MaxLength = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MaxLength)));
            }
        }

        public Style PasswordBoxStyle
        {
            get => passwordBox.Style;
            set
            {
                passwordBox.Style = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PasswordBoxStyle)));
            }
        }

        [Obsolete("Use " + nameof(PasswordBoxStyle) + " instead", true)] // Doesnt work as I expected :(
        public new Style Style
        {
            get; set;
        }


        //public Style PasswordBoxStyle
        //{
        //    get { return (Style)GetValue(PasswordBoxStyleProperty); }
        //    set { passwordPasswordBox.Style = value; SetValue(PasswordBoxStyleProperty, value); }
        //}
        //
        //// Using a DependencyProperty as the backing store for PasswordBoxStyle.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty PasswordBoxStyleProperty =
        //    DependencyProperty.Register("PasswordBoxStyle", typeof(Style), typeof(DependencyPasswordBox), new PropertyMetadata(null));



        //public bool IsSelectionActive { get => passwordPasswordBox.IsSelectionActive; }
        //public Brush CaretBrush { get => passwordPasswordBox.CaretBrush; set => passwordPasswordBox.CaretBrush = value; }
        //public double SelectionOpacity { get=>passwordPasswordBox.SelectionOpacity; set=>passwordPasswordBox.SelectionOpacity=value; }
        //public Brush SelectionBrush { get; set; }
        //public char PasswordChar { get; set; }
        //public SecureString SecurePassword { get; }
        //public bool IsInactiveSelectionHighlightEnabled { get; set; }
        //
        //public void Clear();
        //public override void OnApplyTemplate();
        //public void Paste();
        //public void SelectAll();
    }
}
