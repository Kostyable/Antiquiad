using System.Linq;

public class MeleeUnit : CombatUnit
{
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
                int damage;
                if (unit is AntiCavalryUnit)
                {
                    damage = (int)((attack + 2 - combatUnit.defence) * IsRiver(combatUnit)
                                                                     * IsHillOrForest(combatUnit));
                }
                else
                {
                    damage = (int)((attack - combatUnit.defence) * IsRiver(combatUnit)
                                                                     * IsHillOrForest(combatUnit));
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
        if (currentCell.neighbors.Contains(city.coreCell) && !isAttack)
        {
            isAttack = true;
            currentMovePoints = 0;
            if (Owner.Name == GameLogic.SelectedCiv)
            {
                DeleteBorders();
                CreateBorders();
            }
            int damage = (int)((attack - city.defence) * IsRiver(city) * IsHillOrForest(city));
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

    public float IsRiver(Unit unit)
    {
        foreach (River river in unit.currentCell.rivers)
        {
            if (currentCell.rivers.Contains(river))
            {
                return 0.75f;
            }
        }
        return 1f;
    }
    
    public float IsRiver(City city)
    {
        foreach (River river in city.coreCell.rivers)
        {
            if (currentCell.rivers.Contains(river))
            {
                return 0.75f;
            }
        }
        return 1f;
    }
    
    public float IsHillOrForest(Unit unit)
    {
        if ((unit.currentCell.terrain == Terrain.Hills || unit.currentCell.terrain == Terrain.DesertHills)
            && (unit.currentCell.terrain == Terrain.Plain || unit.currentCell.terrain == Terrain.Desert
                                                          || unit.currentCell.terrain == Terrain.Floodplain ||
                                                          unit.currentCell.terrain == Terrain.Forest))
        {
            return 0.75f;
        }
        if (unit.currentCell.terrain == Terrain.Forest && (unit.currentCell.terrain == Terrain.Plain
                                                             || unit.currentCell.terrain == Terrain.Desert
                                                               || unit.currentCell.terrain == Terrain.Floodplain))
        {
            return 0.75f;
        }
        return 1f;
    }
    
    public float IsHillOrForest(City city)
    {
        if ((city.coreCell.terrain == Terrain.Hills || city.coreCell.terrain == Terrain.DesertHills)
            && (city.coreCell.terrain == Terrain.Plain || city.coreCell.terrain == Terrain.Desert
                                                          || city.coreCell.terrain == Terrain.Floodplain ||
                                                          city.coreCell.terrain == Terrain.Forest))
        {
            return 0.75f;
        }
        if (city.coreCell.terrain == Terrain.Forest && (city.coreCell.terrain == Terrain.Plain
                                                           || city.coreCell.terrain == Terrain.Desert
                                                           || city.coreCell.terrain == Terrain.Floodplain))
        {
            return 0.75f;
        }
        return 1f;
    }
}