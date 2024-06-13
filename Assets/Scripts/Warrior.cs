public class Warrior : MeleeUnit
{
    public override void Init(UnitType unitType, Cell cell, Civilization civ)
    {
        movePoints = 5;
        sight = 2;
        healthPoints = 12;
        attack = 7;
        defence = 4;
        base.Init(unitType, cell, civ);
    }
}