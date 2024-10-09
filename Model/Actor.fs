namespace BattleCity.Model

open Avalonia
//open BattleCity.Infrastructure
open CommunityToolkit.Mvvm.ComponentModel

[<AbstractClass>]
type GameObject internal (location: Point) =
    inherit ObservableObject()
    let mutable _location = location

    member my.Location
        with get() = _location
        and internal set (v) = 
            if v <> _location then
                _location <- v
                my.OnPropertyChanged(nameof my.Location)

    abstract Layer: int with get
    default _.Layer = 0
