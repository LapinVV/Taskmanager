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

            string? s = value as string;

            if (string.IsNullOrWhiteSpace(s))
                return false;

            return true;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            // Возвращаем DO NOTHING — больше НИКАКИХ ошибок nullable
            return Binding.DoNothing;
        }
    }
}