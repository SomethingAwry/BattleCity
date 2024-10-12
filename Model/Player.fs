namespace BattleCity.Model

open Avalonia

type Player(field: IGameField, location: Point, facing: Facing) =
    inherit MovingGameObject(field, location, facing)
    override _.Layer = 2
