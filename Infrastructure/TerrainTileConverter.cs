namespace BattleCity.Infrastructure;

using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

public class TerrainTileConverter : IValueConverter {
    private static Dictionary<TerrainTileType, Bitmap> _cache;
    public static TerrainTileConverter Instance { get; } = new();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
        GetCache()[(TerrainTileType)value];

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotImplementedException();

    private static Dictionary<TerrainTileType, Bitmap> GetCache() =>
        _cache ??= Enum.GetValues(typeof(TerrainTileType)).OfType<TerrainTileType>().ToDictionary(
            t => t,
            t => new Bitmap(AssetLoader.Open(new Uri($"avares://BattleCity/Assets/{t}.png"))));
}