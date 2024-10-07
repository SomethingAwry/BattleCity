namespace BattleCity.Infrastructure

open Avalonia.Data.Converters
open BattleCity.Model
open System

type ZIndexConverter internal () =
    static member Instance = ZIndexConverter()
    
    interface IValueConverter with
        member _.Convert (value, _, _, _) : obj =
            match value with
            | :? Player -> 2
            | :? Tank -> 1
            | _ -> 0
        member _.ConvertBack (_, _, _, _) : obj =
            raise <| NotImplementedException()
