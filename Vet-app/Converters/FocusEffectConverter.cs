using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Effects;

namespace VeterinaryManagementSystem.Converters
{
    public class FocusEffectConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isFocused && isFocused)
            {
                return new DropShadowEffect
                {
                    Color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#2196F3"),
                    Direction = 0,
                    ShadowDepth = 0,
                    BlurRadius = 4,
                    Opacity = 0.4
                };
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

