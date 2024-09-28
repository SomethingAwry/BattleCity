namespace BattleCity.Infrastructure

open Avalonia
open Avalonia.Data.Converters
open Avalonia.Media
open BattleCity.Model
open System

type DirectionToMatrixConverter internal () =
    static member Instance = DirectionToMatrixConverter()

    interface IValueConverter with
        member _.Convert (value, _, _, _) : obj =
            let direction = value :?> Facing
            let matrix =
                match direction with
                | Facing.South -> Matrix.CreateScale(1, -1)
                | Facing.East -> Matrix.CreateRotation(1.5708)
                | Facing.West -> Matrix.CreateRotation(1.5708) * Matrix.CreateScale(-1, 1)
                | _ -> Matrix.Identity
            MatrixTransform matrix
        member _.ConvertBack (_, _, _, _) : obj =
            raise <| NotImplementedException()
