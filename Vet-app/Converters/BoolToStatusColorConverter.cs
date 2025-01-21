using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace VeterinaryManagementSystem.Converters
{
    public class BoolToStatusColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Devuelve un color basado en el valor de IsActive
            if (value is bool isActive)
            {
                return isActive ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);
            }
            return new SolidColorBrush(Colors.Gray); // Si no es booleano, regresa gris
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
