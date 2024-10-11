namespace BattleCity.Model

open Avalonia
open System
open System.Collections.ObjectModel
open System.Linq

type IMove =
    abstract member IsMoving: bool with get
    abstract member SetTarget: Facing -> bool

type IGameField =
    abstract member Width: int with get
    abstract member Height: int with get
    abstract member Tiles: TerrainTile[,]
    abstract member GameObjects: ObservableCollection<GameObject> with get
    abstract member Player: IMove with get

[<AbstractClass>]
type MovingGameObject internal (field: IGameField, location: CellLocation, facing: Facing) =
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
        match facing with
        | Facing.North ->
            pos.WithY(pos.Y - speed), fun p -> (p.Y / CellLocation.CellSize) <= _targetCellLocation.Y
        | Facing.South ->
            pos.WithY(pos.Y + speed), fun p -> (p.Y / CellLocation.CellSize) >= _targetCellLocation.Y
        | Facing.West ->
            pos.WithX(pos.X - speed), fun p -> (p.X / CellLocation.CellSize) <= _targetCellLocation.X
        | _ ->
            pos.WithX(pos.X + speed), fun p -> (p.X / CellLocation.CellSize) >= _targetCellLocation.X

    override _.Layer = 1

    member me.Facing
        with get() = _facing
        and set (v) =
            me.SetProperty(& _facing, v) |> ignore

    member me.CellLocation
        with get() = _cellLocation
        and set (v) =
            if me.SetProperty(& _cellLocation, v) then
                me.OnPropertyChanged <@ me.IsMoving @>

    member me.TargetCellLocation
        with get() = _targetCellLocation
        and set (v) =
            if me.SetProperty(& _targetCellLocation, v) then
                me.OnPropertyChanged <@ me.IsMoving @>

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
            elif not <| field.Tiles[loc.X, loc.Y].IsPassable then false
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

    member me.SetTarget (facing: Facing) : bool =
        me.GetTileAtDirection facing |> me.SetTarget

    member me.SetTarget () : bool =
        me.SetTarget _cellLocation
    
    member me.SetLocation (loc: CellLocation) : unit =
        me.CellLocation <- loc
        me.Location <- loc.ToPoint()

    member my.MoveToTarget () : unit =
        if _targetCellLocation <> _cellLocation then
            let speed =
                let theTile = field.Tiles[_cellLocation.X, _cellLocation.Y]
                let targetTile = field.Tiles[_targetCellLocation.X, _targetCellLocation.Y]
                let avgSpeed: float = (theTile.Speed + targetTile.Speed) / 2.0
                CellLocation.CellSize * avgSpeed * my.SpeedFactor
            let newPos, fxCheck = maybeMoveIt my.Location speed (getDirection _cellLocation _targetCellLocation)
            my.Location <- newPos
            if fxCheck newPos then my.SetLocation _targetCellLocation

    interface IMove with
        member me.IsMoving = me.IsMoving
        member me.SetTarget(facing: Facing): bool = me.SetTarget facing
