using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace Course_work.Converters
{
    public class NullToBoolConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is null)
                return false;

            if (value is string s)
                return !string.IsNullOrWhiteSpace(s);

            return true;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}