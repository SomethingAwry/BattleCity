namespace BattleCity.Model

type Tank (field: GameField, location: CellLocation, facing: Facing, speed: float) =
    inherit MovingGameObject(field, location, facing)

    override _.SpeedFactor = speed * base.SpeedFactor
