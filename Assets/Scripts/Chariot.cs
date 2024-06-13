public class Chariot : RangedUnit
{
    public override void Init(UnitType unitType, Cell cell, Civilization civ)
    {
        movePoints = 7;
        sight = 2;
        healthPoints = 15;
        attack = 10;
        defence = 5;
        range = 2;
        base.Init(unitType, cell, civ);
    }
}