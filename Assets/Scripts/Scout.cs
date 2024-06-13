using System.Linq;

public class Scout : CombatUnit
{
    public override void Init(UnitType unitType, Cell cell, Civilization civ)
    {
        movePoints = 7;
        sight = 3;
        healthPoints = 10;
        attack = 5;
        defence = 2;
        base.Init(unitType, cell, civ);
    }
    public override bool Attack(Unit unit)
    {
        if (currentCell.neighbors.Contains(unit.currentCell) && !isAttack)
        {
            isAttack = true;
            currentMovePoints = 0;
            if (Owner.Name == GameLogic.SelectedCiv)
            {
                DeleteBorders();
                CreateBorders();
            }
            if (unit.type == UnitType.Settler)
            {
                Destroy(unit);
            }
            else if (unit is CombatUnit)
            {
                CombatUnit combatUnit = (CombatUnit)unit;
                int damage = (int)(attack - combatUnit.defence);
                if (damage >= 0)
                {
                    combatUnit.Damagable(damage);
                }
                else
                {
                    Damagable(-damage);
                }
            }
            return true;
        }
        return false;
    }
    
    public override bool Attack(City city)
    {
        if (currentCell.neighbors.Contains(city.coreCell) && !isAttack)
        {
            isAttack = true;
            currentMovePoints = 0;
            if (Owner.Name == GameLogic.SelectedCiv)
            {
                DeleteBorders();
                CreateBorders();
            }
            int damage = (int)(attack - city.defence);
            if (damage >= 0)
            {
                city.Damagable(damage, Owner);
            }
            else
            {
                Damagable(-damage);
            }
            return true;
        }
        return false;
    }
}