namespace BattleCity.Model

open Avalonia

type CellLocation = {
    X: int
    Y: int
}
with
    member me.ToPoint() =
        Point(GameField.CellSize * me.X, GameField.CellSize * me.Y)
