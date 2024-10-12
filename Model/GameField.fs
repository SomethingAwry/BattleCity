namespace BattleCity.Model

open Avalonia
open System
open System.Collections.ObjectModel

type GameField (width: int, height: int) as this =
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
        TerrainTile(Point(float x * CellLocation.CellSize, float y * CellLocation.CellSize), getTypeForCoords(x, y))
    )

    let player = Player(this, { X = width / 2; Y = height / 2 }, Facing.East)

    let gameObjects = ObservableCollection<GameObject>()

    do
        tiles |> Array2D.iter gameObjects.Add
        player |> gameObjects.Add

        Seq.initInfinite (fun _ -> random.Next (width - 1), random.Next (height - 1))
        |> Seq.filter (fun (x,y) -> tiles[x, y].IsPassable)
        |> Seq.truncate 10
        |> Seq.iter (fun (x,y) ->
            let tankFacing = Facing.Directions[random.Next(4)]
            Tank(this, { X = x; Y = y }, tankFacing, random.NextDouble() * 4.0 + 1.0)
            |> gameObjects.Add
        )

    member _.Width = width
    member _.Height = height
    member _.GameObjects = gameObjects

    static member private designOnly = lazy GameField(20, 15)
    static member DesignInstance = GameField.designOnly.Value

    interface IGameField with
        member me.Width = me.Width
        member me.Height = me.Height
        member _.Tiles = tiles
        member me.GameObjects = me.GameObjects
        member _.Player = player