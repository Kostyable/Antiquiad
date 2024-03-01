public class SiegeTower : SiegeUnit
{
    public override void Init(UnitType unitType, Cell cell, Civilization civ)
    {
        movePoints = 3;
        sight = 2;
        healthPoints = 15;
        attack = 17;
        defence = 5;
        base.Init(unitType, cell, civ);
    }
}