public class Settler : Unit
{
    public override void Init(UnitType unitType, Cell cell, Civilization civ)
    {
        movePoints = 5;
        sight = 2;
        base.Init(unitType, cell, civ);
    }
}