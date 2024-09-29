namespace BattleCity.Model

open System
open System.Linq

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

    override _.Layer = 1

    member me.Facing
        with get() = _facing
        and set (v) =
            if v <> _facing then
                _facing <- v
                me.OnPropertyChanged(nameof me.Facing)

    member me.CellLocation
        with get() = _cellLocation
        and set (v) =
            if v <> _cellLocation then
                _cellLocation <- v
                me.OnPropertyChanged(nameof me.CellLocation)
                me.OnPropertyChanged(nameof me.IsMoving)

    member me.TargetCellLocation
        with get() = _targetCellLocation
        and set (v) =
            if v <> _targetCellLocation then
                _targetCellLocation <- v
                me.OnPropertyChanged(nameof me.TargetCellLocation)
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
                GameField.CellSize * (
                    field.Tiles[CellLocation.X, CellLocation.Y].Speed 
                    + field.Tiles[TargetCellLocation.X, TargetCellLocation.Y].Speed
                    ) / 2.0 * my.SpeedFactor
            let mutable pos = my.Location
            match getDirection _cellLocation _targetCellLocation with
            | Facing.North ->
                pos <- pos.WithY(pos.Y - speed)
                my.Location <- pos
                if pos.Y / GameField.CellSize <= _targetCellLocation.Y then
                    SetLocation(_targetCellLocation)
            | Facing.South ->
                pos <- pos.WithY(pos.Y + speed)
                my.Location <- pos
                if pos.Y / GameField.CellSize >= _targetCellLocation.Y then
                    SetLocation(_targetCellLocation)
            | Facing.West ->
                pos <- pos.WithX(pos.X - speed)
                my.Location <- pos
                if pos.X / GameField.CellSize <= _targetCellLocation.X then
                    SetLocation(_targetCellLocation)
            | Facing.East ->
                pos <- pos.WithX(pos.X + speed)
                my.Location <- pos
                if pos.X / GameField.CellSize >= _targetCellLocation.X then
                    SetLocation(_targetCellLocation)
