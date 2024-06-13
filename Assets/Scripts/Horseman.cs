public class Horseman : CavalryUnit
{
    public override void Init(UnitType unitType, Cell cell, Civilization civ)
    {
        movePoints = 7;
        sight = 2;
        healthPoints = 17;
        attack = 12;
        defence = 6;
        base.Init(unitType, cell, civ);
    }
}