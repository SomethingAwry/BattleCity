namespace BattleCity.Model

open Avalonia

type TerrainTileType =
    | Plain = 0 //passable, shoot-thru
    | WoodWall = 1 //impassable, takes 1 shot to bring down
    | StoneWall = 2 //impassable, indestructible
    | Water = 3 //impassable, shoot-thru
    | Pavement = 4 //passable, 2x speed
    | Forest = 5 //passable at half speed, shoot-thru

type TerrainTile(location: Point, typ: TerrainTileType) =
    inherit GameObject(location)

    static let speeds = Map.ofList [
        TerrainTileType.Plain, 1.0
        TerrainTileType.WoodWall, 0.0
        TerrainTileType.StoneWall, 0.0
        TerrainTileType.Water, 0.0
        TerrainTileType.Pavement, 2.0
        TerrainTileType.Forest, 0.5
    ]

    static let shootThrus = Map.ofList [
        TerrainTileType.Plain, true
        TerrainTileType.WoodWall, false
        TerrainTileType.StoneWall, false
        TerrainTileType.Water, true
        TerrainTileType.Pavement, true
        TerrainTileType.Forest, true
    ]

    member _.Speed: float = speeds[typ]
    member _.ShootThru: bool = shootThrus[typ]
    member my.IsPassable: bool = my.Speed > 0.1
    member _.Type = typ
