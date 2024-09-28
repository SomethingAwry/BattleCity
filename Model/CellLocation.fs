namespace BattleCity.Model

open Avalonia

type CellLocation = { X: int; Y: int } with
    static member CellSize = 32.0
    member me.ToPoint() =
        Point(CellLocation.CellSize * float me.X, CellLocation.CellSize * float me.Y)
