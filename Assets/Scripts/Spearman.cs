public class Spearman : AntiCavalryUnit
{
    public override void Init(UnitType unitType, Cell cell, Civilization civ)
    {
        movePoints = 5;
        sight = 2;
        healthPoints = 15;
        attack = 10;
        defence = 5;
        base.Init(unitType, cell, civ);
    }
}