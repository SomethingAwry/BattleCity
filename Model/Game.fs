namespace BattleCity.Model

open Avalonia.Input
open BattleCity
open System
open System.Linq

type Game (field: IGameField) =
    inherit GameBase()
    
    let random = Random()

    override _.Tick() =
        if not field.Player.IsMoving then
            if Keyboard.IsKeyDown(Key.Up) then
                field.Player.SetTarget(Facing.North) |> ignore
            elif Keyboard.IsKeyDown(Key.Down) then
                field.Player.SetTarget(Facing.South) |> ignore
            elif Keyboard.IsKeyDown(Key.Left) then
                field.Player.SetTarget(Facing.West) |> ignore
            elif Keyboard.IsKeyDown(Key.Right) then
                field.Player.SetTarget(Facing.East) |> ignore

        field.GameObjects.OfType<Tank>()
        |> Seq.iter (fun (tank: Tank) -> 
            if not tank.IsMoving then
                if not <| tank.SetTarget tank.Facing then
                    let tankFacing = Enum.GetValues<Facing>()[random.Next(4)]
                    if not <| tank.SetTarget tankFacing then
                        tank.SetTarget () |> ignore
        )

        field.GameObjects.OfType<MovingGameObject>()
        |> Seq.iter (fun (mover: MovingGameObject) -> mover.MoveToTarget())
