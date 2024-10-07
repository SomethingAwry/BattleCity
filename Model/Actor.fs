namespace BattleCity.Model

open Avalonia
open CommunityToolkit.Mvvm.ComponentModel

[<AbstractClass>]
type GameObject internal (location: Point) =
    inherit ObservableObject()
    let mutable _location = location

    member my.Location
        with get() = _location
        and internal set (v) = 
            // if v <> _location then
            //     _location <- v
            //     my.OnPropertyChanged(nameof my.Location)
            my.SetProperty(ref _location, v) |> ignore

    abstract Layer: int with get
    default _.Layer = 0
