namespace BattleCity.Model

open Avalonia.Threading
open System

[<AbstractClass>]
type GameBase internal() as this =
    [<Literal>]
    let TicksPerSecond = 60
    let mutable currentTick = 0L
    let timer = DispatcherTimer(Interval = TimeSpan(0, 0, 0, 0, 1000 / TicksPerSecond))
    do timer.Tick |> Event.add (fun _ -> this.DoTick())

    abstract Tick: unit -> unit
    member _.CurrentTick with get() = currentTick

    member private my.DoTick() =
        my.Tick()
        currentTick <- currentTick + 1L

    member _.Start() = timer.IsEnabled <- true
    member _.Stop() = timer.IsEnabled <- false
