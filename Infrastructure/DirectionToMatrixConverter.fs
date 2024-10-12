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
            value :?> float |> Matrix.CreateRotation |> MatrixTransform :> obj
        member _.ConvertBack (_, _, _, _) : obj =
            raise <| NotImplementedException()
