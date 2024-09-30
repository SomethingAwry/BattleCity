using Avalonia;

namespace BattleCity.Model;

public readonly record struct CellLocation(int X, int Y)
{
    public Point ToPoint() =>
        new(GameField.CellSize * X, GameField.CellSize * Y);
}