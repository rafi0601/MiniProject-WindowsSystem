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

            SelectionChanged += DateTextBlock_SelectionChanged;
        }

        private void DateTextBlock_SelectionChanged(object sender, EventArgs e)
        {
            dateTextBlock.Text = DateTime.ToString();
        }

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
            SelectionChanged(this, new EventArgs());
        }

        public event EventHandler SelectionChanged;
    }

    //public class DateTimeToDateString_Converter : IValueConverter
    //{
    //    object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if (!(value is DateTime date)) throw new Exception();
    //        return date.ToShortTimeString();
    //    }

    //    object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
