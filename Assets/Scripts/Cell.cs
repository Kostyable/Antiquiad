using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private GameObject borderPrefab;
    public HashSet<GameObject> UnitBorders;
    public HashSet<GameObject> CityBorders;
    public Vector2Int offsetCoordinates;
    public Vector2Int axialCoordinates;
    public CellType type;
    public float altitude;
    public float humidity;
    public Terrain terrain;
    public ResourceType resourceType;
    public GameObject resource;
    public Cell[] neighbors;
    public List<River> rivers;
    public Unit unit;
    public City city;
    
    public CellData CollectCellData()
    {
        CellData data = new CellData();
        data.offsetCoordinates = offsetCoordinates;
        data.axialCoordinates = axialCoordinates;
        data.type = type;
        data.altitude = altitude;
        data.humidity = humidity;
        data.terrain = terrain;
        data.resourceType = resourceType;
        if (rivers != null)
        {
            List<RiverData> riversData = new List<RiverData>();
            foreach (River river in rivers)
            {
                riversData.Add(river.CollectRiverData());
            }
            data.rivers = riversData;
        }
        if (unit != null)
        {
            data.unit = unit.CollectUnitData();
            if (data.unit.type != UnitType.Settler)
            {
                data.unit.healthPoints = ((CombatUnit)unit).healthPoints;
                data.unit.attack = ((CombatUnit)unit).attack;
                data.unit.defence = ((CombatUnit)unit).defence;
                if (data.unit.type == UnitType.Archer || data.unit.type == UnitType.Chariot
                                                      || data.unit.type == UnitType.Slinger)
                {
                    data.unit.range = ((RangedUnit)unit).range;
                }
                else if (data.unit.type == UnitType.Catapult || data.unit.type == UnitType.SiegeTower)
                {
                    data.unit.range = ((SiegeUnit)unit).range;
                }
            }
        }
        if (city != null)
        {
            data.city = city.CollectCityData();
        }
        data.IsExplored = new Dictionary<CivilizationName, bool>();
        foreach (Civilization civ in GameLogic.Civs)
        {
            bool isExploredValue = false;
            foreach (Cell cell in civ.ExploredCells)
            {
                if (cell == this)
                {
                    isExploredValue = true;
                    break;
                }
            }
            data.IsExplored.Add(civ.Name, isExploredValue);
        }
        return data;
    }
    
    public void Init(int j, int i, CellType cellType)
    {
        UnitBorders = new HashSet<GameObject>();
        CityBorders = new HashSet<GameObject>();
        offsetCoordinates = new Vector2Int(j, i);
        int x = j - (i - i % 2) / 2;
        int y = i;
        axialCoordinates = new Vector2Int(x, y);
        type = cellType;
        resourceType = ResourceType.None;
        neighbors = new Cell[6];
    }
    
    public void Init(CellData cellData)
    {
        UnitBorders = new HashSet<GameObject>();
        CityBorders = new HashSet<GameObject>();
        offsetCoordinates = cellData.offsetCoordinates;
        axialCoordinates = cellData.axialCoordinates;
        type = cellData.type;
        altitude = cellData.altitude;
        humidity = cellData.humidity;
        terrain = cellData.terrain;
        resourceType = cellData.resourceType;
        neighbors = new Cell[6];
    }
    
    public HashSet<Cell> GetCellsInRadius(int radius, bool withTerrain)
    {
        HashSet<Cell> cellsInRadius = new HashSet<Cell>();
        Dictionary<Cell, int> visitedCells = new Dictionary<Cell, int>();
        HashSet<Cell> barrierCells = new HashSet<Cell>();
        Queue<Cell> queue = new Queue<Cell>();
        bool flag = false;
        queue.Enqueue(this);
        visitedCells.Add(this, 0);
        while (queue.Count > 0)
        {
            Cell currentCell = queue.Dequeue();
            int currentRadius = visitedCells[currentCell];
            foreach (Cell neighbor in currentCell.neighbors)
            {
                if (neighbor != null && !visitedCells.ContainsKey(neighbor) && currentRadius + 1 <= radius)
                {
                    queue.Enqueue(neighbor);
                    visitedCells.Add(neighbor, currentRadius + 1);
                }
            }
        }
        cellsInRadius.Add(this);
        foreach (Cell neighbor in neighbors)
        {
            cellsInRadius.Add(neighbor);
        }
        if (withTerrain)
        {
            foreach (Cell cell in visitedCells.Keys)
            {
                foreach (Cell neighbor in cell.neighbors)
                {
                    if (neighbor != null && visitedCells.ContainsKey(neighbor) && !barrierCells.Contains(cell)
                        && visitedCells[neighbor] == visitedCells[cell] + 1)
                    {
                        if (IsBarrier(neighbor, cell))
                        {
                            barrierCells.Add(neighbor);
                        }
                    }
                }
            }
        }
        foreach (Cell cell in visitedCells.Keys)
        {
            foreach (Cell neighbor in cell.neighbors)
            {
                if (neighbor != null && !barrierCells.Contains(neighbor) && cellsInRadius.Contains(neighbor)
                    && visitedCells[neighbor] == visitedCells[cell] - 1)
                {
                    flag = true;
                    break;
                }
            }
            if (flag)
            {
                cellsInRadius.Add(cell);
                flag = false;
            }
        }
        return cellsInRadius;
    }
    
    public HashSet<Cell> GetCellsWithRadius(int radius)
    {
        HashSet<Cell> cellsAtRadius = new HashSet<Cell>();
        Dictionary<Cell, int> visitedCells = new Dictionary<Cell, int>();
        Queue<Cell> queue = new Queue<Cell>();
        queue.Enqueue(this);
        visitedCells.Add(this, 0);
        while (queue.Count > 0)
        {
            Cell currentCell = queue.Dequeue();
            int currentRadius = visitedCells[currentCell];
            if (currentRadius == radius)
            {
                cellsAtRadius.Add(currentCell);
                continue;
            }
            foreach (Cell neighbor in currentCell.neighbors)
            {
                if (neighbor != null && !visitedCells.ContainsKey(neighbor) && currentRadius + 1 <= radius)
                {
                    queue.Enqueue(neighbor);
                    visitedCells.Add(neighbor, currentRadius + 1);
                }
            }
        }
        return cellsAtRadius;
    }

    private bool IsBarrier(Cell cell1, Cell cell2)
    {
        if (((cell1.terrain == Terrain.Forest || cell1.terrain == Terrain.Hills || cell1.terrain == Terrain.DesertHills)
            && cell2.terrain != Terrain.Hills && cell2.terrain != Terrain.DesertHills)
            || (cell1.terrain == Terrain.Mountains && (cell2.terrain == Terrain.Hills
                                                       || cell2.terrain == Terrain.DesertHills)))
        {
            return true;
        }
        return false;
    }

    public void SetTerrainColor()
    {
        gameObject.GetComponent<SpriteRenderer>().color = GetTerrainColor();
        if (city != null)
        {
            city.gameObject.GetComponent<SpriteRenderer>().color = city.Owner.MainColor;
            city.transform.GetChild(0).gameObject.SetActive(true);
            if (city == city.Owner.Cities[0])
            {
                city.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().color = city.Owner.AdditionalColor;
            }
        }
        if (CityBorders.Count > 0)
        {
            foreach (Civilization civ in GameLogic.Civs)
            {
                if (civ.Borders.Contains(this))
                {
                    foreach (GameObject cityBorder in CityBorders)
                    {
                        cityBorder.GetComponent<SpriteRenderer>().color = civ.MainColor;
                    }
                }
            }
        }
    }
    
    public Color GetTerrainColor()
    {
        switch (terrain)
        {
            case Terrain.Water:
                return new Color(0.1f, 0.45f, 0.82f);
            case Terrain.Plain:
                return new Color(0.49f, 0.99f, 0f);
            case Terrain.Forest:
                return new Color(0.13f, 0.55f, 0.13f);
            case Terrain.Desert:
                return new Color(0.99f, 0.87f, 0.46f);
            case Terrain.Floodplain:
                return new Color(0.93f, 1f, 0.13f);
            case Terrain.Hills:
                return new Color(0.4f, 0.8f, 0f);
            case Terrain.DesertHills:
                return new Color(0.99f, 0.82f, 0.28f);
            default:
                return new Color(0.55f, 0.55f, 0.48f);
        }
    }

    public void SetTerraIncognitaColor()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0.75f, 0.75f, 0.75f);
    }
    
    public void SetFogOfWarColor()
    {
        Color grayColor = new Color(0.25f, 0.25f, 0.25f);
        gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp(GetTerrainColor(), grayColor, 0.5f);
        if (resource != null)
        {
            resource.GetComponent<SpriteRenderer>().color = grayColor * 2f;
        }
        if (city != null)
        {
            city.gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp(city.Owner.MainColor, grayColor,
                0.5f);
            city.transform.GetChild(0).gameObject.SetActive(false);
            if (city == city.Owner.Cities[0])
            {
                city.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().color
                    = Color.Lerp(city.Owner.AdditionalColor, grayColor, 0.5f);
            }
        }
        if (CityBorders.Count > 0)
        {
            foreach (Civilization civ in GameLogic.Civs)
            {
                if (civ.Borders != null && civ.Borders.Contains(this))
                {
                    foreach (GameObject cityBorder in CityBorders)
                    {
                        cityBorder.GetComponent<SpriteRenderer>().color = Color.Lerp(civ.MainColor, grayColor, 
                            0.5f);
                    }
                }
            }
        }
    }

    public void CreateBorder(int neighborIndex, bool isUnit)
    {
        GameObject border;
        switch (type)
        {
            case CellType.Left:
                switch (neighborIndex)
                {
                    case 0:
                        border = Instantiate(borderPrefab,
                            new Vector3(transform.position.x + 0.23f, transform.position.y + 1.11f, -3),
                            Quaternion.Euler(0, 0, 0), transform);
                        border.transform.localScale = new Vector3(0.45f, 0.1f, 1);
                        if (isUnit)
                        {
                            UnitBorders.Add(border);
                        }
                        else
                        {
                            CityBorders.Add(border);
                        }
                        break;
                    case 1:
                        border = Instantiate(borderPrefab,
                            new Vector3(transform.position.x + 0.61f, transform.position.y + 0.45f, -3),
                            Quaternion.Euler(0, 0, -77), transform);
                        border.transform.localScale = new Vector3(1.34f, 0.1f, 1);
                        if (isUnit)
                        {
                            UnitBorders.Add(border);
                        }
                        else
                        {
                            CityBorders.Add(border);
                        }
                        break;
                    case 2:
                        border = Instantiate(borderPrefab,
                            new Vector3(transform.position.x + 0.27f, transform.position.y - 0.66f, -3),
                            Quaternion.Euler(0, 0, 43), transform);
                        border.transform.localScale = new Vector3(1.34f, 0.1f, 1);
                        if (isUnit)
                        {
                            UnitBorders.Add(border);
                        }
                        else
                        {
                            CityBorders.Add(border);
                        }
                        break;
                    case 3:
                        border = Instantiate(borderPrefab,
                            new Vector3(transform.position.x - 0.49f, transform.position.y - 0.65f, -3),
                            Quaternion.Euler(0, 0, -60), transform);
                        border.transform.localScale = new Vector3(1.05f, 0.1f, 1);
                        if (isUnit)
                        {
                            UnitBorders.Add(border);
                        }
                        else
                        {
                            CityBorders.Add(border);
                        }
                        break;
                    case 4:
                        border = Instantiate(borderPrefab,
                            new Vector3(transform.position.x - 0.5f, transform.position.y + 0.26f, -3),
                            Quaternion.Euler(0, 0, 60), transform);
                        border.transform.localScale = new Vector3(1.05f, 0.1f, 1);
                        if (isUnit)
                        {
                            UnitBorders.Add(border);
                        }
                        else
                        {
                            CityBorders.Add(border);
                        }
                        break;
                    case 5:
                        border = Instantiate(borderPrefab,
                            new Vector3(transform.position.x - 0.12f, transform.position.y + 0.91f, -3),
                            Quaternion.Euler(0, 0, 60), transform);
                        border.transform.localScale = new Vector3(0.45f, 0.1f, 1);
                        if (isUnit)
                        {
                            UnitBorders.Add(border);
                        }
                        else
                        {
                            CityBorders.Add(border);
                        }
                        break;
                }
                break;
            case CellType.Right:
                switch (neighborIndex)
                {
                    case 0:
                        border = Instantiate(borderPrefab,
                            new Vector3(transform.position.x + 0.47f, transform.position.y + 0.31f, -3),
                            Quaternion.Euler(0, 0, -60), transform);
                        border.transform.localScale = new Vector3(1.05f, 0.1f, 1);
                        if (isUnit)
                        {
                            UnitBorders.Add(border);
                        }
                        else
                        {
                            CityBorders.Add(border);
                        }
                        break;
                    case 1:
                        border = Instantiate(borderPrefab,
                            new Vector3(transform.position.x + 0.84f, transform.position.y - 0.35f, -3),
                            Quaternion.Euler(0, 0, -60), transform);
                        border.transform.localScale = new Vector3(0.45f, 0.1f, 1);
                        if (isUnit)
                        {
                            UnitBorders.Add(border);
                        }
                        else
                        {
                            CityBorders.Add(border);
                        }
                        break;
                    case 2:
                        border = Instantiate(borderPrefab,
                            new Vector3(transform.position.x + 0.84f, transform.position.y - 0.75f, -3),
                            Quaternion.Euler(0, 0, 60), transform);
                        border.transform.localScale = new Vector3(0.45f, 0.1f, 1);
                        if (isUnit)
                        {
                            UnitBorders.Add(border);
                        }
                        else
                        {
                            CityBorders.Add(border);
                        }
                        break;
                    case 3:
                        border = Instantiate(borderPrefab,
                            new Vector3(transform.position.x + 0.08f, transform.position.y - 0.74f, -3),
                            Quaternion.Euler(0, 0, -17), transform);
                        border.transform.localScale = new Vector3(1.34f, 0.1f, 1);
                        if (isUnit)
                        {
                            UnitBorders.Add(border);
                        }
                        else
                        {
                            CityBorders.Add(border);
                        }
                        break;
                    case 4:
                        border = Instantiate(borderPrefab,
                            new Vector3(transform.position.x - 0.71f, transform.position.y + 0.1f, -3),
                            Quaternion.Euler(0, 0, -77), transform);
                        border.transform.localScale = new Vector3(1.34f, 0.1f, 1);
                        if (isUnit)
                        {
                            UnitBorders.Add(border);
                        }
                        else
                        {
                            CityBorders.Add(border);
                        }
                        break;
                    case 5:
                        border = Instantiate(borderPrefab,
                            new Vector3(transform.position.x - 0.32f, transform.position.y + 0.76f, -3),
                            Quaternion.Euler(0, 0, 0), transform);
                        border.transform.localScale = new Vector3(1.05f, 0.1f, 1);
                        if (isUnit)
                        {
                            UnitBorders.Add(border);
                        }
                        else
                        {
                            CityBorders.Add(border);
                        }
                        break;
                }
                break;
            case CellType.Bottom:
                switch (neighborIndex)
                {
                    case 0:
                        border = Instantiate(borderPrefab,
                            new Vector3(transform.position.x + 0.43f, transform.position.y + 0.57f, -3),
                            Quaternion.Euler(0, 0, -17), transform);
                        border.transform.localScale = new Vector3(1.34f, 0.1f, 1);
                        if (isUnit)
                        {
                            UnitBorders.Add(border);
                        }
                        else
                        {
                            CityBorders.Add(border);
                        }
                        break;
                    case 1:
                        border = Instantiate(borderPrefab,
                            new Vector3(transform.position.x + 0.81f, transform.position.y - 0.09f, -3),
                            Quaternion.Euler(0, 0, 60), transform);
                        border.transform.localScale = new Vector3(1.05f, 0.1f, 1);
                        if (isUnit)
                        {
                            UnitBorders.Add(border);
                        }
                        else
                        {
                            CityBorders.Add(border);
                        }
                        break;
                    case 2:
                        border = Instantiate(borderPrefab,
                            new Vector3(transform.position.x + 0.03f, transform.position.y - 0.55f, -3),
                            Quaternion.Euler(0, 0, 0), transform);
                        border.transform.localScale = new Vector3(1.05f, 0.1f, 1);
                        if (isUnit)
                        {
                            UnitBorders.Add(border);
                        }
                        else
                        {
                            CityBorders.Add(border);
                        }
                        break;
                    case 3:
                        border = Instantiate(borderPrefab,
                            new Vector3(transform.position.x - 0.74f, transform.position.y - 0.55f, -3),
                            Quaternion.Euler(0, 0, 0), transform);
                        border.transform.localScale = new Vector3(0.45f, 0.1f, 1);
                        if (isUnit)
                        {
                            UnitBorders.Add(border);
                        }
                        else
                        {
                            CityBorders.Add(border);
                        }
                        break;
                    case 4:
                        border = Instantiate(borderPrefab,
                            new Vector3(transform.position.x - 1.08f, transform.position.y - 0.35f, -3),
                            Quaternion.Euler(0, 0, -60), transform);
                        border.transform.localScale = new Vector3(0.45f, 0.1f, 1);
                        if (isUnit)
                        {
                            UnitBorders.Add(border);
                        }
                        else
                        {
                            CityBorders.Add(border);
                        }
                        break;
                    case 5:
                        border = Instantiate(borderPrefab,
                            new Vector3(transform.position.x - 0.7f, transform.position.y + 0.3f, -3),
                            Quaternion.Euler(0, 0, 43), transform);
                        border.transform.localScale = new Vector3(1.34f, 0.1f, 1);
                        if (isUnit)
                        {
                            UnitBorders.Add(border);
                        }
                        else
                        {
                            CityBorders.Add(border);
                        }
                        break;
                }
                break;
        }
    }
}