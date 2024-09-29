namespace BattleCity.Model

type Player(field: GameField, location: CellLocation, facing: Facing) =
    inherit MovingGameObject(field, location, facing)
