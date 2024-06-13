using System.Collections.Generic;

public class RangedUnit : CombatUnit
{
    public int range;
    
    public override void Init(UnitData data, Cell cell, Civilization owner)
    {
        range = data.range;
        base.Init(data, cell, owner);
    }
    
    public override bool Attack(Unit unit)
    {
        HashSet<Cell> cells = currentCell.GetCellsInRadius(range, true);
        if (cells.Contains(unit.currentCell) && !isAttack)
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
                int damage;
                if (unit is MeleeUnit)
                {
                    damage = (int)(attack + 2 - combatUnit.defence);
                }
                else
                {
                    damage = (int)(attack - combatUnit.defence);
                }
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
        HashSet<Cell> cells = currentCell.GetCellsInRadius(range, true);
        if (cells.Contains(city.coreCell) && !isAttack)
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