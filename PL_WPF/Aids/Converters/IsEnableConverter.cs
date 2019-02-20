using BE;
using System;
using System.Globalization;
using System.Windows.Data;

namespace PL_WPF
{
    class IsEnableConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != Test.Criteria.NumberOfCriteria + 1)
                throw new CasingException(false, new NotSupportedException("Cannot convert it"));
            for (int i = 0; i < values.Length - 1; i++)
            {
            }
            return new object();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
