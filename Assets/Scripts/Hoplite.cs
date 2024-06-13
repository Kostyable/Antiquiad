public class Hoplite : AntiCavalryUnit
{
    public override void Init(UnitType unitType, Cell cell, Civilization civ)
    {
        movePoints = 5;
        sight = 2;
        healthPoints = 15;
        attack = 12;
        defence = 6;
        base.Init(unitType, cell, civ);
    }
}