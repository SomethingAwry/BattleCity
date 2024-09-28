namespace BattleCity.Model;

using Avalonia;

public readonly record struct CellLocation(int X, int Y) {
    public Point ToPoint() {
        return new(GameField.CellSize * X, GameField.CellSize * Y);
    }
}