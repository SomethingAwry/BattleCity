namespace BattleCity.Infrastructure;

using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Model;
using System;
using System.Globalization;

internal class DirectionToMatrixConverter : IValueConverter {
    public static DirectionToMatrixConverter Instance { get; } = new();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        new MatrixTransform((Facing)value switch
        {
            Facing.South => Matrix.CreateScale(1, -1),
            Facing.East => Matrix.CreateRotation(1.5708),
            Facing.West => Matrix.CreateRotation(1.5708) * Matrix.CreateScale(-1, 1),
            _ => Matrix.Identity
        });

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}