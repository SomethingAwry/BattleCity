namespace BattleCity.Model

open Avalonia

type Tank (field: IGameField, location: Point, facing: Facing, speed: float) =
    inherit MovingGameObject(field, location, facing)
    override _.Layer = 1
