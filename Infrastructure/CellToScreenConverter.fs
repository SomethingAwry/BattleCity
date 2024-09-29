namespace BattleCity.Infrastructure

open Avalonia.Data.Converters
open BattleCity.Model
open System

type CellToScreenConverter internal () =
    interface IValueConverter with
        member _.Convert (value, targetType, parameter, culture) : obj =
            (Convert.ToDouble(value) * GameField.CellSize) :> obj
        member _.ConvertBack (value, targetType, parameter, culture) : obj =
            raise <| NotSupportedException()
        
    static member Instance = CellToScreenConverter()
