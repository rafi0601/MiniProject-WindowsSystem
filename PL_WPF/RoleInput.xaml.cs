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
        string RoleName;
        public RoleInput(ref string roleName)
        {
            InitializeComponent();
        }

        private void RoleListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RoleName = RoleListBox.SelectedItem.ToString();
            Close();
        }

        public event EventHandler SelectionChanged; // CHECK which args to use

    }
}
