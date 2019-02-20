using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace PL_WPF
{
    /// <summary>
    /// Interaction logic for DateTimePicker.xaml
    /// </summary>
    public partial class DateTimePicker : UserControl
    {
        public DateTimePicker()
        {
            InitializeComponent();

            SelectionChanged += (sender, e) => dateTextBlock.Text = DateTime.ToString(); ; // TODO another to string
        }


        public event SelectionChangedEventHandler SelectionChanged;



        public DateTime DateTime
        {
            get
            {
                DateTime date = calendar.SelectedDate.Value;
                DateTime time = DateTime.ParseExact(((ListBoxItem)hoursListBox.SelectedItem).Content.ToString(), "HH:mm", null);
                return new DateTime(date.Year, date.Month, date.Day, time.Hour, 0, 0);
            }
        }


        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            hoursListBox.IsEnabled = true;
        }

        private void HoursListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectionChanged?.Invoke(this, e); // UNDONE new args(...)
        }




    }



}
