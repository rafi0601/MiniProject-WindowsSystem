using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using BE;

namespace PL_WPF
{
    class VisibilitiesToVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            foreach (object item in values)
            {
                if (item == DependencyProperty.UnsetValue)
                    return DependencyProperty.UnsetValue;

                if (!(item is Visibility visibility))
                    throw new CasingException(false, new Exception("Cannot convert it"));

                if (visibility == Visibility.Visible)
                    return Visibility.Collapsed;
            }

            return Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException(); //CHECK if needed
        }
    }
}
