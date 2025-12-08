using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Course_work.Converters
{
    public class PriorityToColorConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo? culture)
        {
            if (value is not string p)
                return Colors.Transparent;

            return p switch
{
    "HighUrgent" => Color.FromArgb("#d55043"),   // красный
    "High"       => Color.FromArgb("#FFF09515"),   // оранжевый
    "Urgent"     => Color.FromArgb("#FFBD59"),   // жёлтый
    "Low"        => Color.FromArgb("#BBBBBB"),   // серый
    _            => Colors.Transparent
};
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo? culture)
        {
            // Обратное преобразование не используется в UI — возвращаем значение по умолчанию
            return "Low";
        }
    }
}