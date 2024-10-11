namespace BattleCity.Model

open Avalonia
open BattleCity.Infrastructure

[<AbstractClass>]
type GameObject internal (location: Point) =
    inherit PropertyChangedBase()
    let mutable _location = location

    member my.Location
        with get() = _location
        and internal set (v) = 
            my.SetProperty(& _location, v) |> ignore

    abstract Layer: int with get
    default _.Layer = 0
