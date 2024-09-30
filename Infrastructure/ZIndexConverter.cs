using System;
using System.Globalization;
using BattleCity.Model;
using Avalonia.Data.Converters;

namespace BattleCity.Infrastructure;

internal class ZIndexConverter : IValueConverter
{
    public static ZIndexConverter Instance { get; } = new();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        value switch
        {
            Player => 2,
            Tank => 1,
            _ => 0
        };

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}