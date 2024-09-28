namespace BattleCity.Model;

public class Player(GameField field, CellLocation location, Facing facing) : MovingGameObject(field, location, facing) {
}