using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace VeterinaryManagementSystem.Converters
{
    public class StatusForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isActive)
            {
                return isActive ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4CAF50")) 
                                : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F44336"));
            }
            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

