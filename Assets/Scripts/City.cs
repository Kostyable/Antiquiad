using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class City : MonoBehaviour
{
    [SerializeField] private GameObject capitalPrefab;
    [SerializeField] private Sprite[] EgyptUnitsSprites;
    [SerializeField] private Sprite[] GreeceUnitsSprites;
    [SerializeField] private Sprite[] MesopotamiaUnitsSprites;
    public string cityName;
    public Civilization Owner;
    public Cell coreCell;
    public HashSet<Cell> cells;
    public HashSet<Cell> cellsInSight;
    public Dictionary<ResourceType, int> resourcesCount;
    public Dictionary<ResourceType, float> resourcesModifiers;
    public float totalFoodCount;
    public HashSet<BuildingType> buildings;
    public float healthPoints;
    public float currentHealthPoints;
    public float defence;
    public int currentExpansionRadius;
    public int expansionCellsCount;
    
    public void Init(Cell cell, Civilization owner)
    {
        healthPoints = 20;
        currentHealthPoints = healthPoints;
        defence = 5;
        Owner = owner;
        SetCityName();
        Destroy(cell.resource);
        cell.resourceType = null;
        coreCell = cell;
        if (coreCell.terrain == Terrain.Forest)
        {
            coreCell.terrain = Terrain.Plain;
            if (Owner.Name == GameLogic.SelectedCiv)
            {
                coreCell.SetTerrainColor();
            }
        }
        cells = cell.GetCellsInRadius(1, false);
        cellsInSight = new HashSet<Cell>();
        resourcesCount = new Dictionary<ResourceType, int>
        {
            { ResourceType.Wheat, 0 },
            { ResourceType.Cattle, 0 },
            { ResourceType.Fish, 0 },
            { ResourceType.Wood, 0 },
            { ResourceType.Stone, 0 },
            { ResourceType.Gold, 0 },
            { ResourceType.Horses, 0 },
            { ResourceType.Copper, 0 },
            { ResourceType.Iron, 0 }
        };
        resourcesModifiers = new Dictionary<ResourceType, float>
        {
            { ResourceType.Wheat, 1f },
            { ResourceType.Cattle, 1f },
            { ResourceType.Fish, 1f },
            { ResourceType.Wood, 1f },
            { ResourceType.Stone, 1f },
            { ResourceType.Gold, 1f },
            { ResourceType.Horses, 0f },
            { ResourceType.Copper, 0f },
            { ResourceType.Iron, 0f }
        };
        buildings = new HashSet<BuildingType>();
        SetCellsInSight();
        Owner.OwnedCells.UnionWith(cells);
        Owner.DeleteBorders();
        Owner.CreateBorders();
        coreCell.city = this;
        currentExpansionRadius = 2;
    }
    
    public void Damagable(int damage, Civilization civ)
    {
        currentHealthPoints -= damage;
        if (currentHealthPoints <= 0)
        {
            if (this == GameLogic.Civs[0].Cities[0])
            {
                GameLogic.Result = false;
                SceneManager.LoadScene("Game Over");
            }
            for (int i = 1; i < GameLogic.Civs.Length; i++)
            {
                if (this == GameLogic.Civs[i].Cities[0])
                {
                    GameLogic.Result = true;
                    SceneManager.LoadScene("Game Over");
                }
            }
            if (buildings.Contains(BuildingType.Walls))
            {
                buildings.Remove(BuildingType.Walls);
                defence -= 5;
            }
            currentHealthPoints = healthPoints;
            if (coreCell.unit != null)
            {
                Destroy(coreCell.unit.gameObject);
            }
            Owner.OwnedCells.ExceptWith(cells);
            Owner.Cities.Remove(this);
            Owner.CreateFogOfWar();
            Owner.DeleteBorders();
            Owner.CreateBorders();
            Owner.SetResourcesAdds();
            Owner = civ;
            Owner.OwnedCells.UnionWith(cells);
            Owner.Cities.Add(this);
            Owner.CreateFogOfWar();
            Owner.DeleteBorders();
            Owner.CreateBorders();
            SetResourcesCount();
            coreCell.SetTerrainColor();
        }
    }

    public void SetCityName()
    {
        switch (Owner.Name)
        {
            case CivilizationName.Egypt:
                cityName = ((EgyptCityName)Owner.Cities.Count).ToString();
                break;
            case CivilizationName.Greece:
                cityName = ((GreeceCityName)Owner.Cities.Count).ToString();
                break;
            case CivilizationName.Mesopotamia:
                cityName = ((MesopotamiaCityName)Owner.Cities.Count).ToString();
                break;
        }
    }

    public void SetCellsInSight()
    {
        foreach (Cell cell in cells)
        {
            foreach (Cell neighbor in cell.neighbors)
            {
                if (neighbor != null)
                {
                    cellsInSight.Add(neighbor);
                }
            }
        }
        Owner.ExploredCells.UnionWith(cellsInSight);
        if (Owner.Name == GameLogic.SelectedCiv)
        {
            foreach (Cell c in cellsInSight)
            {
                c.SetTerrainColor();
                if (c.resource != null)
                {
                    c.resource.SetActive(true);
                    c.resource.GetComponent<SpriteRenderer>().color = Color.white;
                }
                foreach (River river in c.rivers)
                {
                    if ((river.cell1 == c && Owner.ExploredCells.Contains(river.cell2))
                        || (river.cell2 == c && Owner.ExploredCells.Contains(river.cell1)))
                    {
                        river.gameObject.SetActive(true);
                    }
                    if ((river.cell1 == c && cellsInSight.Contains(river.cell2))
                        || (river.cell2 == c && cellsInSight.Contains(river.cell1)))
                    {
                        river.SetDefaultColor();
                    }
                }
            }
        }
    }
    
    public void SetCityUnit()
    {
        if (coreCell.unit != null)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
            switch (Owner.Name)
            {
                case CivilizationName.Egypt:
                    if (coreCell.unit.type == UnitType.Chariot)
                    {
                        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = EgyptUnitsSprites[4];
                    }
                    else
                    {
                        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite
                            = EgyptUnitsSprites[(int)coreCell.unit.type];
                    }
                    break;
                case CivilizationName.Greece:
                    if (coreCell.unit.type == UnitType.Hoplite)
                    {
                        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = GreeceUnitsSprites[5];
                    }
                    else
                    {
                        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite
                            = GreeceUnitsSprites[(int)coreCell.unit.type];
                    }
                    break;
                case CivilizationName.Mesopotamia:
                    if (coreCell.unit.type == UnitType.SiegeTower)
                    {
                        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = MesopotamiaUnitsSprites[8];
                    }
                    else
                    {
                        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite
                            = MesopotamiaUnitsSprites[(int)coreCell.unit.type];
                    }
                    break;
            }
        }
        else
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.clear;
        }
    }

    public void DrawCapital()
    {
        Vector2 position = coreCell.GetComponent<Transform>().position;
        GameObject capital = Instantiate(capitalPrefab, new Vector3(position.x, position.y, -4),
            quaternion.identity, transform);
        capital.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        capital.GetComponent<SpriteRenderer>().color = Owner.AdditionalColor;
    }

    public void SetCityColor()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Owner.MainColor;
    }

    public void SetResourcesCount()
    {
        foreach (ResourceType resource in resourcesCount.Keys.ToList())
        {
            resourcesCount[resource] = 0;
        }
        foreach (Cell cell in cells)
        {
            if (cell.resourceType != null)
            {
                resourcesCount[cell.resourceType.Value]++;
            }
        }
        Owner.SetResourcesAdds();
    }

    public void Expand()
    {
        if (currentExpansionRadius > GameLogic.CityExpansionRadius)
        {
            return;
        }
        HashSet<Cell> expansionCells = coreCell.GetCellsWithRadius(currentExpansionRadius);
        foreach (Civilization civ in GameLogic.Civs)
        {
            foreach (City city in civ.Cities)
            {
                foreach (Cell cell in expansionCells.ToList())
                {
                    if (city.cells.Contains(cell))
                    {
                        expansionCells.Remove(cell);
                    }
                }
            }
        }
        foreach (Cell cell in expansionCells.ToList())
        {
            bool flag = false;
            foreach (Cell neighbor in cell.neighbors)
            {
                if (cells.Contains(neighbor))
                {
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                expansionCells.Remove(cell);
            }
        }
        if (expansionCells.Count == 0)
        {
            currentExpansionRadius++;
            Expand();
        }
        else
        {
            Cell cell = expansionCells.ToList()[Random.Range(0, expansionCells.Count)];
            cells.Add(cell);
            SetCellsInSight();
            Owner.OwnedCells.UnionWith(cells);
            Owner.DeleteBorders();
            Owner.CreateBorders();
            SetResourcesCount();
            expansionCellsCount++;
        }
    }

    public void Build(BuildingType building, ResourceType? resource)
    {
        Map.SelectedCity.buildings.Add(building);
        if (resource != null)
        {
            if (resource.Value == ResourceType.Horses || resource.Value == ResourceType.Copper
                | resource.Value == ResourceType.Iron)
            {
                resourcesModifiers[resource.Value] = GameLogic.StrategicResourcesModifier;
            }
            else
            {
                resourcesModifiers[resource.Value] *= GameLogic.OrdinaryResourcesModifier;
            }
            SetResourcesCount();
        }
        else
        {
            if (building == BuildingType.Walls)
            {
                defence += 5;
            }
        }
    }
}