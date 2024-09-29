namespace BattleCity.Infrastructure

open Avalonia.Data.Converters
open BattleCity.Model
open System
open Avalonia.Media.Imaging
open Avalonia.Platform

type TerrainTileConverter internal () =
    static let cache =
        Enum.GetValues<TerrainTileType>()
        |> Seq.map (fun t -> t, new Bitmap(AssetLoader.Open(Uri($"avares://BattleCity/Assets/{t}.png"))))
        |> Map.ofSeq

    interface IValueConverter with
        member _.Convert (value, targetType, parameter, culture) : obj =
            let tt = value :?> TerrainTileType
            cache[tt]
        member _.ConvertBack (value, targetType, parameter, culture) : obj =
            raise <| NotImplementedException()
        
    static member Instance = TerrainTileConverter()
