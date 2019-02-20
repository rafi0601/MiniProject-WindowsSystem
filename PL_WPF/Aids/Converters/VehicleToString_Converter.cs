using BE;
using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace PL_WPF
{
    public class VehicleToString_Converter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Vehicle vehicle)) throw new Exception();
            StringBuilder result = new StringBuilder();
            foreach (Vehicle flagVehicle in Enum.GetValues(typeof(Vehicle)).Cast<Vehicle>())
                if (vehicle.HasFlag(flagVehicle))
                    result.Append((Tools.GetUserDisplayAttribute(flagVehicle)?.DisplayName ?? flagVehicle.ToString()) + ", ");
            if (result.Length != 0)
                result.Remove(result.Length - 2, 2);
            return result.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException(); // UNDONE
        }
    }
}
