public class Swordsman : MeleeUnit
{
    public override void Init(UnitType unitType, Cell cell, Civilization civ)
    {
        movePoints = 5;
        sight = 2;
        healthPoints = 17;
        attack = 12;
        defence = 7;
        base.Init(unitType, cell, civ);
    }
}