public class Slinger : RangedUnit
{
    public override void Init(UnitType unitType, Cell cell, Civilization civ)
    {
        movePoints = 5;
        sight = 2;
        healthPoints = 12;
        attack = 7;
        defence = 3;
        range = 1;
        base.Init(unitType, cell, civ);
    }
}