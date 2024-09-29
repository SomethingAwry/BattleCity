namespace BattleCity.Infrastructure

open Avalonia.Data.Converters
open BattleCity.Model
open System

type ZIndexConverter internal () =
    interface IValueConverter with
        member _.Convert (value, targetType, parameter, culture) : obj =
            match value with
            | :? Player -> 2
            | :? Tank -> 1
            | _ -> 0
        member _.ConvertBack (value, targetType, parameter, culture) : obj =
            raise <| NotImplementedException()
        
    static member Instance = ZIndexConverter()
