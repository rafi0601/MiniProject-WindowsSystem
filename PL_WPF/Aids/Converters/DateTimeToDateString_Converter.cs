using System;
using System.Globalization;
using System.Windows.Data;

namespace PL_WPF
{
    public class DateTimeToDateString_Converter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is DateTime date)) throw new Exception();
            return date.ToShortTimeString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
