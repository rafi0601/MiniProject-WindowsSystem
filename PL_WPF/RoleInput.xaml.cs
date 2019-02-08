//Bs"d

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
    /// Interaction logic for RoleInput.xaml
    /// </summary>
    public partial class RoleInput : Window
    {
        public string RoleName;

        public RoleInput()
        {
            InitializeComponent();
        }

        private void RoleListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //sender as ListBox).Selecte......
            RoleName = (RoleListBox.SelectedItem as ListBoxItem).Content as string;
            Close();
        }

    }
}