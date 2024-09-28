namespace BattleCity.Model;

public class Tank(GameField field, CellLocation location, Facing facing, double speed) : MovingGameObject(field, location, facing) {
    private readonly double _speed = speed;

    protected override double SpeedFactor => _speed * base.SpeedFactor;
}