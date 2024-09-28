namespace BattleCity.Infrastructure

open Avalonia.Data.Converters
open BattleCity.Model
open System

type CellToScreenConverter internal () =
    static member Instance = CellToScreenConverter()
    
    interface IValueConverter with
        member _.Convert (value, _, _, _) : obj =
            (Convert.ToDouble(value) * CellLocation.CellSize) :> obj
        member _.ConvertBack (_, _, _, _) : obj =
            raise <| NotSupportedException()
