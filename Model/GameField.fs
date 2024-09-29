namespace BattleCity.Model

open Avalonia
open BattleCity.Infrastructure
open System
open System.Collections.ObjectModel

type GameField (width: int, height: int) as this =
    inherit PropertyChangedBase()

    static let cellSize = 32.0
    let random = Random()

    let getTypeForCoords (x: int, y: int) =
        if x / 2 = width / 4 then TerrainTileType.Pavement
        elif y / 2 = height / 4 then TerrainTileType.Water
        elif x * y = 0 then TerrainTileType.StoneWall
        elif (x + 1 - width) * (y + 1 - height) = 0 then TerrainTileType.WoodWall
        //elif random.NextDouble() < 0.1 then TerrainTileType.WoodWall
        elif random.NextDouble() < 0.3 then TerrainTileType.Forest
        else TerrainTileType.Plain

    let tiles = Array2D.init width height (fun x y -> 
        TerrainTile(Point(float x * cellSize, float y * cellSize), getTypeForCoords(x, y))
    )

    let player = Player(this, CellLocation(width / 2, height / 2), Facing.East)

    let gameObjects = ObservableCollection<GameObject>()

    do
        tiles |> Array2D.iter gameObjects.Add
        player |> gameObjects.Add

        Seq.initInfinite (fun _ -> random.Next (width - 1), random.Next (height - 1))
        |> Seq.filter (fun (x,y) -> tiles[x, y].IsPassable)
        |> Seq.truncate 10
        |> Seq.iter (fun (x,y) ->
            let tankFacing = Enum.GetValues<Facing>()[random.Next(4)]
            Tank(this, CellLocation(x, y), tankFacing, random.NextDouble() * 4.0 + 1.0)
            |> gameObjects.Add
        )

    new() = GameField(20, 15)
    static member CellSize = cellSize
    member _.Width = width
    member _.Height = height

    static member DesignInstance: GameField = GameField()
    member _.GameObjects = gameObjects
    member _.Tiles = tiles
    member _.Player = player
