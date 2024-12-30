using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace VeterinaryManagementSystem.Converters
{
    public class AllValuesNotEmptyConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.All(v => v != null && !string.IsNullOrWhiteSpace(v.ToString()));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}