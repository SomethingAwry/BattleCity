namespace BattleCity.Model

open Avalonia
open System
open System.Collections.ObjectModel

type GameField (width: int, height: int) as this =
    let random = Random()

    let player = Player(this, Point(float width / 2.0, float height / 2.0), Facing.East)

    let gameObjects = ObservableCollection<GameObject>()

    do
        player |> gameObjects.Add

        Seq.initInfinite (fun _ -> random.Next (width - 1), random.Next (height - 1))
        //|> Seq.filter (fun (x,y) -> tiles[x, y].IsPassable)
        |> Seq.truncate 10
        |> Seq.iter (fun (x,y) ->
            let tankFacing = Facing.Directions[random.Next(4)]
            Tank(this, Point(float x, float y), tankFacing, random.NextDouble() * 4.0 + 1.0)
            |> gameObjects.Add
        )

    member _.Width = width
    member _.Height = height
    member _.GameObjects = gameObjects

    static member private designOnly = lazy GameField(640, 480)
    static member DesignInstance = GameField.designOnly.Value

    interface IGameField with
        member me.Width = me.Width
        member me.Height = me.Height
        member me.GameObjects = me.GameObjects
        member _.Player = player