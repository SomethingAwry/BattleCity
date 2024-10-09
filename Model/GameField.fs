namespace rec BattleCity.Model

open Avalonia
open System
open System.Collections.ObjectModel
open System.Linq

type CellLocation = { X: int; Y: int } with
    member me.ToPoint() =
        Point(GameField.CellSize * float me.X, GameField.CellSize * float me.Y)


[<AbstractClass>]
type MovingGameObject internal (field: GameField, location: CellLocation, facing: Facing) =
    inherit GameObject(location.ToPoint())

    let mutable _cellLocation = location
    let mutable _targetCellLocation = location
    let mutable _facing = facing

    let getDirection current target =
        if target.X < current.X then Facing.West
        elif target.X > current.X then Facing.East
        elif target.Y < current.Y then Facing.North
        else Facing.South

    let maybeMoveIt (pos: Point) speed facing : Point * (Point -> bool) =
        let cellSize: float = GameField.CellSize
        match facing with
        | Facing.North ->
            pos.WithY(pos.Y - speed), fun p -> (p.Y / cellSize) <= _targetCellLocation.Y
        | Facing.South ->
            pos.WithY(pos.Y + speed), fun p -> (p.Y / GameField.CellSize) >= _targetCellLocation.Y
        | Facing.West ->
            pos.WithX(pos.X - speed), fun p -> (p.X / GameField.CellSize) <= _targetCellLocation.X
        | _ ->
            pos.WithX(pos.X + speed), fun p -> (p.X / GameField.CellSize) >= _targetCellLocation.X

    override _.Layer = 1

    member me.Facing
        with get() = _facing
        and set (v) =
            me.SetProperty(& _facing, v) |> ignore

    member me.CellLocation
        with get() = _cellLocation
        and set (v) =
            if me.SetProperty(& _cellLocation, v) then
                me.OnPropertyChanged(nameof me.IsMoving)

    member me.TargetCellLocation
        with get() = _targetCellLocation
        and set (v) =
            if me.SetProperty(& _targetCellLocation, v) then
                me.OnPropertyChanged(nameof me.IsMoving)

    member _.IsMoving = _targetCellLocation <> _cellLocation

    abstract SpeedFactor: float
    default _.SpeedFactor = 1.0 / 15.0

    member me.SetTarget (loc: CellLocation) : bool =
        if me.IsMoving then
            //We are the bear rolling from the hill
            raise <| InvalidOperationException("Unable to change direction while moving")
        elif loc = _cellLocation then
            true
        else
            me.Facing <- getDirection _cellLocation loc
            if loc.X < 0 || loc.Y < 0 then false
            elif loc.X >= field.Width || loc.Y >= field.Height then false
            elif not <| field.Tiles(loc.X, loc.Y).IsPassable then false
            else
                let otherwiseOccupied =
                    field.GameObjects.OfType<MovingGameObject>()
                    |> Seq.exists (fun t ->
                        t <> me && (t.CellLocation = loc || t.TargetCellLocation = loc)
                    )
                if otherwiseOccupied then false
                else
                    me.TargetCellLocation <- loc
                    true

    member _.GetTileAtDirection (facing: Facing) : CellLocation =
        match facing with
        | Facing.North -> { _cellLocation with Y = _cellLocation.Y - 1 }
        | Facing.South -> { _cellLocation with Y = _cellLocation.Y + 1 }
        | Facing.West -> { _cellLocation with X = _cellLocation.X - 1 }
        | _ -> { _cellLocation with X = _cellLocation.X + 1 }

    member me.SetTarget (?facing: Facing) : bool =
        let target =
            match facing with
            | Some face -> me.GetTileAtDirection(face)
            | None -> _cellLocation
        me.SetTarget target

    member me.SetLocation (loc: CellLocation) : unit =
        me.CellLocation <- loc
        me.Location <- loc.ToPoint()

    member my.MoveToTarget () : unit =
        if _targetCellLocation <> _cellLocation then
            let speed =
                let theTile = field.Tiles(_cellLocation.X, _cellLocation.Y)
                let targetTile = field.Tiles(_targetCellLocation.X, _targetCellLocation.Y)
                let avgSpeed: float = (theTile.Speed + targetTile.Speed) / 2.0
                GameField.CellSize * avgSpeed * my.SpeedFactor
            let newPos, fxCheck = maybeMoveIt my.Location speed (getDirection _cellLocation _targetCellLocation)
            my.Location <- newPos
            if fxCheck newPos then my.SetLocation _targetCellLocation


type Tank (field: GameField, location: CellLocation, facing: Facing, speed: float) =
    inherit MovingGameObject(field, location, facing)

    override _.SpeedFactor = speed * base.SpeedFactor


type Player(field: GameField, location: CellLocation, facing: Facing) =
    inherit MovingGameObject(field, location, facing)


type GameField (width: int, height: int) as this =
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

    let player = Player(this, { X = width / 2; Y = height / 2 }, Facing.East)

    let gameObjects = ObservableCollection<GameObject>()

    do
        tiles |> Array2D.iter gameObjects.Add
        player |> gameObjects.Add

        Seq.initInfinite (fun _ -> random.Next (width - 1), random.Next (height - 1))
        |> Seq.filter (fun (x,y) -> tiles[x, y].IsPassable)
        |> Seq.truncate 10
        |> Seq.iter (fun (x,y) ->
            let tankFacing = Enum.GetValues<Facing>()[random.Next(4)]
            Tank(this, { X = x; Y = y }, tankFacing, random.NextDouble() * 4.0 + 1.0)
            |> gameObjects.Add
        )

    static member CellSize : float = cellSize
    member _.Width = width
    member _.Height = height

    static member private designOnly = lazy GameField(20, 15)
    static member DesignInstance = GameField.designOnly.Value
    member _.GameObjects: ObservableCollection<GameObject> = gameObjects
    member _.Tiles with get(x, y) : TerrainTile = tiles[x, y]
    member _.Player = player
