using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Civilization
{
    public CivilizationName Name;
    public Color MainColor;
    public Color AdditionalColor;
    public Dictionary<ResourceType, float> ResourcesValues;
    public Dictionary<ResourceType, float> ResourcesAdds;
    public List<Unit> Units;
    public List<City> Cities;
    public HashSet<Cell> OwnedCells;
    public HashSet<Cell> ExploredCells;
    public HashSet<Cell> VisibleCells;
    public HashSet<Cell> FogOfWarCells;
    public HashSet<Cell> Borders;
    public HashSet<Unit> EnemyUnits;
    public HashSet<City> EnemyCities;

    public Civilization(CivilizationName name)
    {
        Name = name;
        switch (name)
        {
            case CivilizationName.Egypt:
                MainColor = new Color(0.63f, 0f, 0f);
                AdditionalColor = new Color(1f, 1f, 0.5f);
                break;
            case CivilizationName.Greece:
                MainColor = new Color(0f, 0.75f, 1f);
                AdditionalColor = new Color(1f, 1f, 1f);
                break;
            case CivilizationName.Mesopotamia:
                MainColor = new Color(0.13f, 0.13f, 0.5f);
                AdditionalColor = new Color(1f, 0.5f, 0f);
                break;
        }

        ResourcesValues = new Dictionary<ResourceType, float>
        {
            { ResourceType.Food, 0f },
            { ResourceType.Wood, 0f },
            { ResourceType.Stone, 0f },
            { ResourceType.Gold, 0f },
            { ResourceType.Horses, 0f },
            { ResourceType.Bronze, 0f },
            { ResourceType.Iron, 0f }
        };
        ResourcesAdds = new Dictionary<ResourceType, float>
        {
            { ResourceType.Food, 0f },
            { ResourceType.Wood, 0f },
            { ResourceType.Stone, 0f },
            { ResourceType.Gold, 0f },
            { ResourceType.Horses, 0f },
            { ResourceType.Bronze, 0f },
            { ResourceType.Iron, 0f }
        };
        Units = new List<Unit>();
        Cities = new List<City>();
        ExploredCells = new HashSet<Cell>();
        OwnedCells = new HashSet<Cell>();
        Borders = new HashSet<Cell>();
        EnemyUnits = new HashSet<Unit>();
        EnemyCities = new HashSet<City>();
    }

    public void CreateFogOfWar()
    {
        CheckEnemyObjects();
        FogOfWarCells = Enumerable.ToHashSet(ExploredCells.Except(VisibleCells));
        if (Name == GameLogic.Civs[0].Name)
        {
            foreach (Cell cell in FogOfWarCells)
            {
                cell.SetFogOfWarColor();
                foreach (River river in cell.rivers)
                {
                    if (river.cell1 == cell || FogOfWarCells.Contains(river.cell2)
                                            || river.cell2 == cell || FogOfWarCells.Contains(river.cell1))
                    {
                        river.SetFogOfWarColor();
                    }
                }
            }
        }
    }

    public void CheckEnemyObjects()
    {
        VisibleCells = Enumerable.ToHashSet(Units.SelectMany(unit => unit.cellsInSight));
        VisibleCells.UnionWith(Enumerable.ToHashSet(Cities.SelectMany(city => city.cellsInSight)));
        HashSet<Unit> unitsToRemove = new HashSet<Unit>();
        foreach (Unit unit in EnemyUnits)
        {
            if (!VisibleCells.Any(cell => cell.unit != null && cell.unit == unit))
            {
                unitsToRemove.Add(unit);
            }
        }
        foreach (Unit unitToRemove in unitsToRemove)
        {
            if (Name == GameLogic.SelectedCiv)
            {
                unitToRemove.gameObject.SetActive(false);
            }
            EnemyUnits.Remove(unitToRemove);
        }
        foreach (Cell cell in VisibleCells)
        {
            if (cell.unit != null && cell.unit.Owner.Name != Name)
            {
                if (Name == GameLogic.SelectedCiv)
                {
                    cell.unit.gameObject.SetActive(true);
                }
                EnemyUnits.Add(cell.unit);
            }
            if (cell.city != null && cell.city.Owner.Name != Name)
            {
                if (Name == GameLogic.SelectedCiv)
                {
                    cell.city.gameObject.SetActive(true);
                }
                EnemyCities.Add(cell.city);
            }
        }
    }

    public void SetResourcesAdds()
    {
        foreach (ResourceType resource in ResourcesAdds.Keys.ToList())
        {
            ResourcesAdds[resource] = 0f;
        }
        foreach (City city in Cities)
        {
            ResourcesAdds[ResourceType.Food] += (int)(city.resourcesCount[ResourceType.Wheat]
                * city.resourcesModifiers[ResourceType.Wheat] + city.resourcesCount[ResourceType.Cattle]
                * city.resourcesModifiers[ResourceType.Cattle] + city.resourcesCount[ResourceType.Fish]
                * city.resourcesModifiers[ResourceType.Fish]);
            ResourcesAdds[ResourceType.Wood] += (int)(city.resourcesCount[ResourceType.Wood]
                                                * city.resourcesModifiers[ResourceType.Wood]);
            ResourcesAdds[ResourceType.Stone] += (int)(city.resourcesCount[ResourceType.Stone]
                                                 * city.resourcesModifiers[ResourceType.Stone]);
            ResourcesAdds[ResourceType.Gold] += (int)(city.resourcesCount[ResourceType.Gold]
                                                * city.resourcesModifiers[ResourceType.Gold]);
            ResourcesAdds[ResourceType.Horses] += (int)(city.resourcesCount[ResourceType.Horses]
                                                  * city.resourcesModifiers[ResourceType.Horses]);
            ResourcesAdds[ResourceType.Bronze] += (int)(city.resourcesCount[ResourceType.Copper]
                                                  * city.resourcesModifiers[ResourceType.Copper]);
            ResourcesAdds[ResourceType.Iron] += (int)(city.resourcesCount[ResourceType.Iron]
                                                  * city.resourcesModifiers[ResourceType.Iron]);
            if (city == Cities[0])
            {
                ResourcesAdds[ResourceType.Food] += GameLogic.CapitalModifier;
            }
            if (city.coreCell.rivers.Count > 0)
            {
                ResourcesAdds[ResourceType.Food] = (int)(ResourcesAdds[ResourceType.Food] * GameLogic.RiverModifier);
            }
        }
        if (Name == GameLogic.SelectedCiv)
        {
            GameObject.Find("Main Camera").GetComponent<UIController>().SetCurrentAdds();
        }
    }
    
    public void CreateBorders()
    {
        foreach (Cell cell in OwnedCells)
        {
            for (int i = 0; i < cell.neighbors.Length; i++)
            {
                if (cell.neighbors[i] != null && !OwnedCells.Contains(cell.neighbors[i]))
                {
                    cell.CreateBorder(i, false);
                    Borders.Add(cell);
                }
            }
            foreach (GameObject border in cell.cityBorders)
            {
                border.GetComponent<SpriteRenderer>().color = MainColor;
            }
        }
    }
    
    public void DeleteBorders()
    {
        foreach (Cell cell in Borders)
        {
            foreach (GameObject border in cell.cityBorders)
            {
                Object.Destroy(border);
            }
            cell.cityBorders.Clear();
        }
        Borders.Clear();
    }
}