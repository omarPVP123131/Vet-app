using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace VeterinaryManagementSystem.Converters
{
    public class StatusBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isActive)
            {
                return isActive ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8F5E9")) 
                                : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFEBEE"));
            }
            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

