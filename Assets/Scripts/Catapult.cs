public class Catapult : SiegeUnit
{
    public override void Init(UnitType unitType, Cell cell, Civilization civ)
    {
        movePoints = 3;
        sight = 2;
        healthPoints = 15;
        attack = 15;
        defence = 4;
        range = 2;
        base.Init(unitType, cell, civ);
    }
}