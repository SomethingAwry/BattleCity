namespace BattleCity.Infrastructure;

using Avalonia.Data.Converters;
using Model;
using System;
using System.Globalization;

public class CellToScreenConverter : IValueConverter {
    public static CellToScreenConverter Instance { get; } = new();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        System.Convert.ToDouble(value) * GameField.CellSize;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}