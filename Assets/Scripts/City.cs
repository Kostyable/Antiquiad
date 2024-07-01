using System.Collections.Generic;
using System.Linq;
using I2.Loc;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class City : MonoBehaviour
{
    [SerializeField] private GameObject capitalPrefab;
    [SerializeField] private Sprite[] egyptUnitsSprites;
    [SerializeField] private Sprite[] greeceUnitsSprites;
    [SerializeField] private Sprite[] mesopotamiaUnitsSprites;
    public string cityName;
    public Civilization Owner;
    public bool isCapital;
    public Cell coreCell;
    public HashSet<Cell> Cells;
    public HashSet<Cell> CellsInSight;
    public Dictionary<ResourceType, int> ResourcesCount;
    public Dictionary<ResourceType, float> ResourcesModifiers;
    public float totalFoodCount;
    public HashSet<BuildingType> Buildings;
    public float healthPoints;
    public float currentHealthPoints;
    public float defence;
    public int currentExpansionRadius;
    public int expansionCellsCount;
    
    public CityData CollectCityData()
    {
        CityData data = new CityData();
        data.cityName = cityName;
        data.owner = Owner.Name;
        data.isCapital = isCapital;
        data.coreCellCoordinates = coreCell.offsetCoordinates;
        data.CellsCoordinates = new HashSet<Vector2Int>();
        foreach (Cell cell in Cells)
        {
            data.CellsCoordinates.Add(cell.offsetCoordinates);
        }
        data.ResourcesCount = ResourcesCount;
        data.ResourcesModifiers = ResourcesModifiers;
        data.totalFoodCount = totalFoodCount;
        data.Buildings = Buildings;
        data.healthPoints = healthPoints;
        data.currentHealthPoints = currentHealthPoints;
        data.defence = defence;
        data.currentExpansionRadius = currentExpansionRadius;
        data.expansionCellsCount = expansionCellsCount;
        return data;
    }
    
    public void Init(Cell cell, Civilization owner, bool isCapitalCity)
    {
        healthPoints = 20;
        currentHealthPoints = healthPoints;
        defence = 0;
        Owner = owner;
        isCapital = isCapitalCity;
        SetCityName();
        Destroy(cell.resource);
        cell.resourceType = ResourceType.None;
        coreCell = cell;
        if (coreCell.terrain == Terrain.Forest)
        {
            coreCell.terrain = Terrain.Plain;
            if (Owner.Name == GameLogic.SelectedCiv)
            {
                coreCell.SetTerrainColor();
            }
        }
        Cells = cell.GetCellsInRadius(1, false);
        foreach (Cell c in Cells)
        {
            c.Owner = Owner;
        }
        CellsInSight = new HashSet<Cell>();
        ResourcesCount = new Dictionary<ResourceType, int>
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
        ResourcesModifiers = new Dictionary<ResourceType, float>
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
        Buildings = new HashSet<BuildingType>();
        SetCellsInSight();
        Owner.OwnedCells.UnionWith(Cells);
        Owner.DeleteBorders();
        Owner.CreateBorders();
        coreCell.city = this;
        currentExpansionRadius = 2;
    }
    
    public void Init(CityData cityData, Cell cell, HashSet<Cell> cells, Civilization owner)
    {
        cityName = cityData.cityName;
        Owner = owner;
        isCapital = cityData.isCapital;
        coreCell = cell;
        Cells = cells;
        foreach (Cell c in Cells)
        {
            c.Owner = Owner;
        }
        CellsInSight = new HashSet<Cell>();
        ResourcesCount = cityData.ResourcesCount;
        ResourcesModifiers = cityData.ResourcesModifiers;
        totalFoodCount = cityData.totalFoodCount;
        Buildings = cityData.Buildings;
        healthPoints = cityData.healthPoints;
        currentHealthPoints = cityData.currentHealthPoints;
        defence = cityData.defence;
        currentExpansionRadius = cityData.currentExpansionRadius;
        expansionCellsCount = cityData.expansionCellsCount;
        SetCellsInSight();
        Owner.OwnedCells.UnionWith(Cells);
        Owner.DeleteBorders();
        Owner.CreateBorders();
        coreCell.city = this;
    }
    
    public void Damagable(int damage, Civilization civ)
    {
        currentHealthPoints -= damage;
        if (currentHealthPoints <= 0)
        {
            foreach (Cell cell in Cells)
            {
                cell.Owner = civ;
            }
            if (Owner == GameLogic.Civs[0] && isCapital)
            {
                GameObject.Find("Main Camera").GetComponent<GameOverController>().ShowResult(false);
            }
            for (int i = 1; i < GameLogic.Civs.Length; i++)
            {
                if (Owner == GameLogic.Civs[i] && isCapital)
                {
                    if (civ.Name == GameLogic.SelectedCiv)
                    {
                        GameObject.Find("Main Camera").GetComponent<GameOverController>().ShowResult(true);
                    }
                    else
                    {
                        GameObject.Find("Main Camera").GetComponent<GameOverController>().ShowResult(false);
                    }
                }
            }
            if (Buildings.Contains(BuildingType.Walls))
            {
                Buildings.Remove(BuildingType.Walls);
                defence -= 5;
            }
            currentHealthPoints = healthPoints;
            if (coreCell.unit != null)
            {
                Destroy(coreCell.unit.gameObject);
            }
            Owner.OwnedCells.ExceptWith(Cells);
            Owner.Cities.Remove(this);
            Owner.CreateFogOfWar();
            Owner.DeleteBorders();
            Owner.CreateBorders();
            Owner.SetResourcesAdds();
            Owner = civ;
            Owner.OwnedCells.UnionWith(Cells);
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
                cityName = LocalizationManager.GetTranslation
                    (((EgyptCityName)Owner.Cities.Count).ToString().ToLower());
                break;
            case CivilizationName.Greece:
                cityName = LocalizationManager.GetTranslation
                    (((GreeceCityName)Owner.Cities.Count).ToString().ToLower());
                break;
            case CivilizationName.Mesopotamia:
                cityName = LocalizationManager.GetTranslation
                    (((MesopotamiaCityName)Owner.Cities.Count).ToString().ToLower());
                break;
        }
    }

    public void SetCellsInSight()
    {
        foreach (Cell cell in Cells)
        {
            foreach (Cell neighbor in cell.neighbors)
            {
                if (neighbor != null)
                {
                    CellsInSight.Add(neighbor);
                }
            }
        }
        Owner.ExploredCells.UnionWith(CellsInSight);
        if (Owner.Name == GameLogic.SelectedCiv)
        {
            foreach (Cell c in CellsInSight)
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
                    if ((river.cell1 == c && CellsInSight.Contains(river.cell2))
                        || (river.cell2 == c && CellsInSight.Contains(river.cell1)))
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
                        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = egyptUnitsSprites[4];
                    }
                    else
                    {
                        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite
                            = egyptUnitsSprites[(int)coreCell.unit.type];
                    }
                    break;
                case CivilizationName.Greece:
                    if (coreCell.unit.type == UnitType.Hoplite)
                    {
                        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = greeceUnitsSprites[5];
                    }
                    else
                    {
                        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite
                            = greeceUnitsSprites[(int)coreCell.unit.type];
                    }
                    break;
                case CivilizationName.Mesopotamia:
                    if (coreCell.unit.type == UnitType.SiegeTower)
                    {
                        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = mesopotamiaUnitsSprites[8];
                    }
                    else
                    {
                        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite
                            = mesopotamiaUnitsSprites[(int)coreCell.unit.type];
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
        foreach (ResourceType resource in ResourcesCount.Keys.ToList())
        {
            ResourcesCount[resource] = 0;
        }
        foreach (Cell cell in Cells)
        {
            if (cell.resourceType != ResourceType.None)
            {
                ResourcesCount[cell.resourceType]++;
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
                    if (city.Cells.Contains(cell))
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
                if (Cells.Contains(neighbor))
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
            cell.Owner = Owner;
            Cells.Add(cell);
            SetCellsInSight();
            Owner.OwnedCells.UnionWith(Cells);
            Owner.DeleteBorders();
            Owner.CreateBorders();
            SetResourcesCount();
            expansionCellsCount++;
        }
    }

    public void Build(BuildingType building, ResourceType? resource)
    {
        Map.SelectedCity.Buildings.Add(building);
        if (resource != null)
        {
            if (resource.Value == ResourceType.Horses || resource.Value == ResourceType.Copper
                | resource.Value == ResourceType.Iron)
            {
                ResourcesModifiers[resource.Value] = GameLogic.StrategicResourcesModifier;
            }
            else
            {
                ResourcesModifiers[resource.Value] *= GameLogic.OrdinaryResourcesModifier;
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