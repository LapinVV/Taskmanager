using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace Course_work.Converters
{
    public class BoolToAccentConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool b && b)
            {
                return Color.FromArgb("#4CAF50"); 
            }

            return Colors.Transparent;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}