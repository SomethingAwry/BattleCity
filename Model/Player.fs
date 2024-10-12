namespace BattleCity.Model

type Player(field: IGameField, location: CellLocation, facing: Facing) =
    inherit MovingGameObject(field, location, facing)
    override _.Layer = 2
