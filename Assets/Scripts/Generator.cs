using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Generator : MonoBehaviour
{
    [SerializeField] private Map map;
    private int _altitudeSeed;
    private int _humiditySeed;
    public Border[,] Borders;
    private HashSet<Border> _riverSources;
    private HashSet<Border> _riverUsed;
    private HashSet<Border> _riverTempUsed;
    [Space(10)]
    [Header("Terrain")]
    [Space(10)]
    public float altitudeA;
    public float altitudeB;
    public float altitudeC;
    public int altitudeScale;
    public int humidityScale;
    [Header("Water")]
    [Range(0, 100)] public int waterMinAltitude;
    [Range(0, 100)] public int waterMaxAltitude;
    [Range(0, 100)] public int waterMinHumidity;
    [Range(0, 100)] public int waterMaxHumidity;
    [Header("Desert")]
    [Range(0, 100)] public int desertMinAltitude;
    [Range(0, 100)] public int desertMaxAltitude;
    [Range(0, 100)] public int desertMinHumidity;
    [Range(0, 100)] public int desertMaxHumidity;
    [Header("Plain")]
    [Range(0, 100)] public int plainMinAltitude;
    [Range(0, 100)] public int plainMaxAltitude;
    [Range(0, 100)] public int plainMinHumidity;
    [Range(0, 100)] public int plainMaxHumidity;
    [Header("Forest")]
    [Range(0, 100)] public int forestMinAltitude;
    [Range(0, 100)] public int forestMaxAltitude;
    [Range(0, 100)] public int forestMinHumidity;
    [Range(0, 100)] public int forestMaxHumidity;
    [Header("Desert Hills")]
    [Range(0, 100)] public int desertHillsMinAltitude;
    [Range(0, 100)] public int desertHillsMaxAltitude;
    [Range(0, 100)] public int desertHillsMinHumidity;
    [Range(0, 100)] public int desertHillsMaxHumidity;
    [Header("Hills")]
    [Range(0, 100)] public int hillsMinAltitude;
    [Range(0, 100)] public int hillsMaxAltitude;
    [Range(0, 100)] public int hillsMinHumidity;
    [Range(0, 100)] public int hillsMaxHumidity;
    [Header("Mountains")]
    [Range(0, 100)] public int mountainsMinAltitude;
    [Range(0, 100)] public int mountainsMaxAltitude;
    [Range(0, 100)] public int mountainsMinHumidity;
    [Range(0, 100)] public int mountainsMaxHumidity;
    [Space(10)]
    [Header("Rivers")]
    [Space(10)]
    public int riverMaxCount;
    public int riverBetweenMinDistance;
    public int riverGenAttempts;
    [Range(0, 100)] public int riverSourcesMinAltitude;
    [Range(0, 100)] public int riverSourcesMaxAltitude;
    [Space(10)]
    [Header("Resources")]
    [Space(10)]
    [Header("Water")]
    [Range(0, 100)] public int waterFish;
    [Header("Desert")]
    [Range(0, 100)] public int desertStone;
    [Range(0, 100)] public int desertGold;
    [Header("Floodplain")]
    [Range(0, 100)] public int floodplainWheat;
    [Range(0, 100)] public int floodplainCattle;
    [Range(0, 100)] public int floodplainHorses;
    [Header("Plain")]
    [Range(0, 100)] public int plainWheat;
    [Range(0, 100)] public int plainCattle;
    [Range(0, 100)] public int plainStone;
    [Range(0, 100)] public int plainGold;
    [Range(0, 100)] public int plainHorses;
    [Header("Forest")]
    [Range(0, 100)] public int forestWood;
    [Header("Desert Hills")]
    [Range(0, 100)] public int desertHillsStone;
    [Range(0, 100)] public int desertHillsGold;
    [Range(0, 100)] public int desertHillsCopper;
    [Range(0, 100)] public int desertHillsIron;
    [Header("Hills")]
    [Range(0, 100)] public int hillsStone;
    [Range(0, 100)] public int hillsGold;
    [Range(0, 100)] public int hillsCattle;
    [Range(0, 100)] public int hillsCopper;
    [Range(0, 100)] public int hillsIron;

    public void GenerateTerrain()
    {
        _altitudeSeed = Random.Range(1, 100000);
        _humiditySeed = Random.Range(1, 100000);
        for (int i = 0; i < map.size.y; i++)
        {
            for (int j = 0; j < map.size.x; j++)
            {
                float x = (j + _altitudeSeed) / (float)altitudeScale;
                float y = (i + _altitudeSeed) / (float)altitudeScale;
                map.Cells[i, j].altitude = Mathf.PerlinNoise(x, y) + altitudeA - altitudeB
                    * (float)Math.Pow(GetDistance(j, i), altitudeC);
                x = (j + _humiditySeed) / (float)humidityScale;
                y = (i + _humiditySeed) / (float)humidityScale;
                map.Cells[i, j].humidity = Mathf.PerlinNoise(x, y);
            }
        }
        NormalizeCells();
        SelectCellTerrain(waterMinAltitude, waterMaxAltitude,
            waterMinHumidity, waterMaxHumidity, Terrain.Water);
        SelectCellTerrain(desertMinAltitude, desertMaxAltitude,
            desertMinHumidity, desertMaxHumidity, Terrain.Desert);
        SelectCellTerrain(plainMinAltitude, plainMaxAltitude,
            plainMinHumidity, plainMaxHumidity, Terrain.Plain);
        SelectCellTerrain(forestMinAltitude, forestMaxAltitude,
            forestMinHumidity, forestMaxHumidity, Terrain.Forest);
        SelectCellTerrain(desertHillsMinAltitude, desertHillsMaxAltitude,
            desertHillsMinHumidity, desertHillsMaxHumidity, Terrain.DesertHills);
        SelectCellTerrain(hillsMinAltitude, hillsMaxAltitude,
            hillsMinHumidity, hillsMaxHumidity, Terrain.Hills);
        SelectCellTerrain(mountainsMinAltitude, mountainsMaxAltitude,
            mountainsMinHumidity, mountainsMaxHumidity, Terrain.Mountains);
        CreateWaterBorder();
    }
    
    public float GetDistance(int x, int y)
    {
        float distX = Math.Abs(x - map.size.x / 2) / (float)(map.size.x / 2);
        float distY = Math.Abs(y - map.size.y / 2) / (float)(map.size.y / 2);
        return (float)Math.Sqrt(Math.Pow(distX, 2) + Math.Pow(distY, 2));
    }
    
    public void NormalizeCells()
    {
        float minAltitude = float.MaxValue;
        float maxAltitude = float.MinValue;
        float minHumidity = float.MaxValue;
        float maxHumidity = float.MinValue;
        foreach (Cell cell in map.Cells)
        {
            if (cell != null)
            {
                minAltitude = Mathf.Min(minAltitude, cell.altitude);
                maxAltitude = Mathf.Max(maxAltitude, cell.altitude);
                minHumidity = Mathf.Min(minHumidity, cell.humidity);
                maxHumidity = Mathf.Max(maxHumidity, cell.humidity);
            }
        }
        foreach (Cell cell in map.Cells)
        {
            if (cell != null)
            {
                cell.altitude = NormalizeValue(cell.altitude, minAltitude, maxAltitude);
                cell.humidity = NormalizeValue(cell.humidity, minHumidity, maxHumidity);
            }
        }
    }

    public float NormalizeValue(float value, float min, float max)
    {
        return (value - min) / (max - min);
    }
    
    public void SelectCellTerrain(int startPercent1, int endPercent1, int startPercent2, int endPercent2,
        Terrain terrain)
    {
        Cell[] cellsArray = map.Cells.Cast<Cell>().ToArray();
        Array.Sort(cellsArray, (x, y) => x.altitude.CompareTo(y.altitude));
        int startIdx1 = (int)(cellsArray.Length * (startPercent1 / 100f));
        int endIdx1 = (int)(cellsArray.Length * (endPercent1 / 100f));
        Cell[] selectedCellsByAltitude = cellsArray.Skip(startIdx1).Take(endIdx1 - startIdx1).ToArray();
        Array.Sort(cellsArray, (x, y) => x.humidity.CompareTo(y.humidity));
        int startIdx2 = (int)(cellsArray.Length * (startPercent2 / 100f));
        int endIdx2 = (int)(cellsArray.Length * (endPercent2 / 100f));
        Cell[] selectedCellsByHumidity = cellsArray.Skip(startIdx2).Take(endIdx2 - startIdx2).ToArray();
        Cell[] finalSelectedCells = selectedCellsByAltitude.Intersect(selectedCellsByHumidity).ToArray();
        foreach (Cell cell in finalSelectedCells)
        {
            cell.terrain = terrain;
            cell.SetTerraIncognitaColor();
        }
    }

    public void CreateWaterBorder()
    {
        for (int i = 0; i < map.size.y; i++)
        {
            for (int j = 0; j < map.size.x; j++)
            {
                if (i % 2 == 0 && (j <= 1 || j == map.size.x - 1) || i % 2 != 0 && (j == 0 || j >= map.size.x - 2)
                                                                  || i == 0 || i == map.size.y - 1)
                {
                    map.Cells[i, j].terrain = Terrain.Water;
                    map.Cells[i, j].SetTerraIncognitaColor();
                }
            }
        }
    }
    
    public void GenerateRivers()
    {
        Borders = new Border[map.size.y * 2 - 1, map.size.x * 2 - 1];
        _riverSources = new HashSet<Border>();
        _riverUsed = new HashSet<Border>();
        _riverTempUsed = new HashSet<Border>();
        for (int i = 0; i < Borders.GetLength(0); i++)
        {
            if (i % 2 == 0)
            {
                for (int j = 0; j < map.size.x - 1; j++)
                {
                    Borders[i, j * 2] = new Border(new Vector2Int(j * 2, i),
                        map.Cells[i / 2, j], map.Cells[i / 2, j + 1]);
                }
            }
            else
            {
                if ((i - 1) % 4 == 0)
                {
                    for (int j = 0; j < Borders.GetLength(1); j++)
                    {
                        Borders[i, j] = new Border(new Vector2Int(j, i),
                            map.Cells[i / 2, j - j / 2],
                            map.Cells[i - i / 2, j / 2]);
                    }
                }
                else
                {
                    for (int j = 0; j < Borders.GetLength(1); j++)
                    {
                        Borders[i, j] = new Border(new Vector2Int(j, i),
                            map.Cells[i / 2, j - (j + 1) / 2],
                            map.Cells[i - i / 2, (j + 1) / 2]);
                    }
                }
            }
        }
        for (int i = 0; i < Borders.GetLength(0); i++)
        {
            for (int j = 0; j < Borders.GetLength(1); j++)
            {
                if (Borders[i, j] != null)
                {
                    SetBorderNeighbors(Borders[i, j]);
                }
            }
        }
        SelectRiverSources(riverSourcesMinAltitude, riverSourcesMaxAltitude);
        bool result;
        int attempts = 0;
        int k = 0;
        Border source;
        while (_riverSources.Count > 0 && k < riverMaxCount && attempts < riverGenAttempts)
        {
            source = _riverSources.ElementAt(Random.Range(0, _riverSources.Count));
            result = SetRiverPath(source, k);
            _riverSources.Remove(source);
            if (!result)
            {
                attempts++;
            }
            else
            {
                k++;
            }
        }
        foreach (River river in map.Rivers)
        {
            river.gameObject.SetActive(false);
        }
    }
    
    public void SetBorderNeighbors(Border border)
    {
        if (border.Coordinates.y % 2 == 0)
        {
            if (border.Coordinates.y % 4 == 0)
            {
                border.Neighbors[0] = IsBorderExist(border.Coordinates.x + 1, border.Coordinates.y - 1);
                border.Neighbors[1] = IsBorderExist(border.Coordinates.x + 1, border.Coordinates.y + 1);
                border.Neighbors[2] = IsBorderExist(border.Coordinates.x, border.Coordinates.y + 1);
                border.Neighbors[3] = IsBorderExist(border.Coordinates.x, border.Coordinates.y - 1);
            }
            else
            {
                border.Neighbors[0] = IsBorderExist(border.Coordinates.x + 2, border.Coordinates.y - 1);
                border.Neighbors[1] = IsBorderExist(border.Coordinates.x + 2, border.Coordinates.y + 1);
                border.Neighbors[2] = IsBorderExist(border.Coordinates.x + 1, border.Coordinates.y + 1);
                border.Neighbors[3] = IsBorderExist(border.Coordinates.x + 1, border.Coordinates.y - 1);
            }
        }
        else
        {
            if ((border.Coordinates.y - 1) % 4 == 0)
            {
                border.Neighbors[0] = IsBorderExist(border.Coordinates.x / 2 * 2, border.Coordinates.y - 1);
                border.Neighbors[1] = IsBorderExist(border.Coordinates.x + 1, border.Coordinates.y);
                border.Neighbors[2] = IsBorderExist((border.Coordinates.x - 1) / 2 * 2, border.Coordinates.y + 1);
                border.Neighbors[3] = IsBorderExist(border.Coordinates.x - 1, border.Coordinates.y);
            }
            else
            {
                border.Neighbors[0] = IsBorderExist(border.Coordinates.x + 1, border.Coordinates.y);
                border.Neighbors[1] = IsBorderExist(border.Coordinates.x / 2 * 2, border.Coordinates.y + 1);
                border.Neighbors[2] = IsBorderExist(border.Coordinates.x - 1, border.Coordinates.y);
                border.Neighbors[3] = IsBorderExist((border.Coordinates.x - 1) / 2 * 2, border.Coordinates.y - 1);
            }
        }
    }
    
    public Border IsBorderExist(int x, int y)
    {
        if (x < 0 || ((x >= Borders.GetLength(1) - 2 || x % 2 != 0) && y % 2 == 0)
                  || (x >= Borders.GetLength(1) && x % 2 != 0) || y < 0 || y >= Borders.GetLength(0))
        {
            return null;
        }
        return Borders[y, x];
    }
    
    public void SelectRiverSources(int startPercent, int endPercent)
    {
        Border[] borderArray = Borders.Cast<Border>().Where(b => b != null).ToArray();
        Array.Sort(borderArray, (x, y) => x.Altitude.CompareTo(y.Altitude));
        int startIdx = (int)(borderArray.Length * (startPercent / 100f));
        int endIdx = (int)(borderArray.Length * (endPercent / 100f));
        Border[] selectedBorders = borderArray.Skip(startIdx).Take(endIdx - startIdx).ToArray();
        foreach (var border in selectedBorders)
        {
            if (border.Cell1.terrain == border.Cell2.terrain && border.Cell1.terrain != Terrain.Water
                                                             && border.Cell1.terrain != Terrain.Mountains)
            {
                _riverSources.Add(border);
            }
        }
    }

    public bool SetRiverPath(Border border, int id)
    {
        _riverTempUsed.Clear();
        List<Border> riverPath = new List<Border>();
        riverPath.Add(border);
        bool flag = true;
        int startIndex = map.Rivers.Count;
        while (flag)
        {
            if (riverPath.Count == 0)
            {
                return false;
            }
            border = SetRiverDirection(riverPath.Last());
            if (border is not null)
            {
                _riverTempUsed.Add(border);
                riverPath.Add(border);
            }
            else
            {
                border = riverPath.Last();
                for (int i = 0; i < 4; i++)
                {
                    if (border.Neighbors[i] != null && (border.Neighbors[i].Cell1.terrain == Terrain.Water ||
                        border.Neighbors[i].Cell2.terrain == Terrain.Water))
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    _riverTempUsed.Add(border);
                    riverPath.RemoveAt(riverPath.Count - 1);
                }
            }
        }
        foreach (Border b in riverPath)
        {
            GameObject river = Instantiate(map.riverPrefab, 
                map.GetRiverCoordinates(b.Coordinates.x, b.Coordinates.y),
                map.GetRiverAngle(b.Coordinates.x, b.Coordinates.y), transform);
            river.GetComponent<Transform>().localScale =
                new Vector3(map.GetRiverLength(b.Coordinates.x, b.Coordinates.y), 0.20f, -1f);
            river.GetComponent<River>().Init(id + 1, b);
            map.Rivers.Add(river.GetComponent<River>());
            UseBordersInRadius(b, riverBetweenMinDistance);
            b.Cell1.rivers.Add(river.GetComponent<River>());
            b.Cell2.rivers.Add(river.GetComponent<River>());
            if (b.Cell1.terrain == Terrain.Desert)
            {
                b.Cell1.terrain = Terrain.Floodplain;
            }
            if (b.Cell2.terrain == Terrain.Desert)
            {
                b.Cell2.terrain = Terrain.Floodplain;
            }
        }
        for (int i = 0; i < riverPath.Count; i++)
        {
            if (i == 0)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (riverPath[i].Neighbors[j] != null
                        && map.Rivers[startIndex + 1].coordinates == riverPath[i].Neighbors[j].Coordinates)
                    {
                        map.Rivers[startIndex].neighbors[j] = map.Rivers[startIndex + 1];
                        break;
                    }
                }
            }
            else if (i == riverPath.Count - 1)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (riverPath[i].Neighbors[j] != null
                        && map.Rivers[^2].coordinates == riverPath[i].Neighbors[j].Coordinates)
                    {
                        map.Rivers[^1].neighbors[j] = map.Rivers[^2];
                        break;
                    }
                }
            }
            else
            {
                for (int j = 0; j < 4; j++)
                {
                    if (riverPath[i].Neighbors[j] != null)
                    {
                        if (map.Rivers[startIndex + i - 1].coordinates == riverPath[i].Neighbors[j].Coordinates)
                        {
                            map.Rivers[startIndex + i].neighbors[j] = map.Rivers[startIndex + i - 1];
                        }
                        if (map.Rivers[startIndex + i + 1].coordinates == riverPath[i].Neighbors[j].Coordinates)
                        {
                            map.Rivers[startIndex + i].neighbors[j] = map.Rivers[startIndex + i + 1];
                        }
                    }
                }
            }
        }
        return true;
    }

    public Border SetRiverDirection(Border border)
    {
        float min = border.Altitude;
        int index = -1;
        for (int i = 0; i < 4; i++)
        {
            if (border.Neighbors[i] != null && border.Neighbors[i].Altitude < min
                                            && !_riverUsed.Contains(border.Neighbors[i])
                                            && !_riverTempUsed.Contains(border.Neighbors[i])
                                            && border.Neighbors[i].Cell1.terrain != Terrain.Water
                                            && border.Neighbors[i].Cell2.terrain != Terrain.Water)
            {
                min = border.Neighbors[i].Altitude;
                index = i;
            }
        }
        if (index == -1)
        {
            return null;
        }
        return border.Neighbors[index];
    }
    
    public void UseBordersInRadius(Border border, int radius)
    {
        if (radius < 0)
        {
            return;
        }
        _riverUsed.Add(border);
        _riverSources.Remove(border);
        foreach (Border neighbor in border.Neighbors)
        {
            if (neighbor != null)
            {
                UseBordersInRadius(neighbor, radius - 1);
            }
        }
    }
    
    public void GenerateResources()
    {
        int random;
        GameObject resource;
        Vector2 position;
        foreach (Cell cell in map.Cells)
        {
            random = Random.Range(0, 100);
            resource = null;
            position = cell.transform.position;
            switch (cell.terrain)
            {
                case Terrain.Water:
                    if (random < waterFish)
                    {
                        resource = Instantiate(map.resourcesPrefabs[2],
                            new Vector3(position.x, position.y, -1), Quaternion.identity,
                            transform);
                        cell.resourceType = ResourceType.Fish;
                    }
                    break;
                case Terrain.Desert:
                    if (random < desertStone)
                    {
                        resource = Instantiate(map.resourcesPrefabs[4],
                            new Vector3(position.x, position.y, -1), Quaternion.identity,
                            transform);
                        cell.resourceType = ResourceType.Stone;
                    }
                    else if (random < desertGold)
                    {
                        resource = Instantiate(map.resourcesPrefabs[5],
                            new Vector3(position.x, position.y, -1), Quaternion.identity,
                            transform);
                        cell.resourceType = ResourceType.Gold;
                    }
                    break;
                case Terrain.Floodplain:
                    if (random < floodplainWheat)
                    {
                        resource = Instantiate(map.resourcesPrefabs[0],
                            new Vector3(position.x, position.y, -1), Quaternion.identity, transform);
                        cell.resourceType = ResourceType.Wheat;
                    }
                    else if (random < floodplainCattle)
                    {
                        resource = Instantiate(map.resourcesPrefabs[1],
                            new Vector3(position.x, position.y, -1), Quaternion.identity, transform);
                        cell.resourceType = ResourceType.Cattle;
                    }
                    else if (random < floodplainHorses)
                    {
                        resource = Instantiate(map.resourcesPrefabs[6],
                            new Vector3(position.x, position.y, -1), Quaternion.identity, transform);
                        cell.resourceType = ResourceType.Horses;
                    }
                    break;
                case Terrain.Plain:
                    if (random < plainWheat)
                    {
                        resource = Instantiate(map.resourcesPrefabs[0],
                            new Vector3(position.x, position.y, -1), Quaternion.identity, transform);
                        cell.resourceType = ResourceType.Wheat;
                    }
                    else if (random < plainCattle)
                    {
                        resource = Instantiate(map.resourcesPrefabs[1],
                            new Vector3(position.x, position.y, -1), Quaternion.identity, transform);
                        cell.resourceType = ResourceType.Cattle;
                    }
                    else if (random < plainStone)
                    {
                        resource = Instantiate(map.resourcesPrefabs[4],
                            new Vector3(position.x, position.y, -1), Quaternion.identity, transform);
                        cell.resourceType = ResourceType.Stone;
                    }
                    else if (random < plainGold)
                    {
                        resource = Instantiate(map.resourcesPrefabs[5],
                            new Vector3(position.x, position.y, -1), Quaternion.identity, transform);
                        cell.resourceType = ResourceType.Gold;
                    }
                    else if (random < plainHorses)
                    {
                        resource = Instantiate(map.resourcesPrefabs[6],
                            new Vector3(position.x, position.y, -1), Quaternion.identity, transform);
                        cell.resourceType = ResourceType.Horses;
                    }
                    break;
                case Terrain.Forest:
                    if (random < forestWood)
                    {
                        resource = Instantiate(map.resourcesPrefabs[3],
                            new Vector3(position.x, position.y, -1), Quaternion.identity, transform);
                        cell.resourceType = ResourceType.Wood;
                    }
                    break;
                case Terrain.DesertHills:
                    if (random < desertHillsStone)
                    {
                        resource = Instantiate(map.resourcesPrefabs[4],
                            new Vector3(position.x, position.y, -1), Quaternion.identity, transform);
                        cell.resourceType = ResourceType.Stone;
                    }
                    else if (random < desertHillsGold)
                    {
                        resource = Instantiate(map.resourcesPrefabs[5],
                            new Vector3(position.x, position.y, -1), Quaternion.identity, transform);
                        cell.resourceType = ResourceType.Gold;
                    }
                    else if (random < desertHillsCopper)
                    {
                        resource = Instantiate(map.resourcesPrefabs[7],
                            new Vector3(position.x, position.y, -1), Quaternion.identity, transform);
                        cell.resourceType = ResourceType.Copper;
                    }
                    else if (random < desertHillsIron)
                    {
                        resource = Instantiate(map.resourcesPrefabs[8],
                            new Vector3(position.x, position.y, -1), Quaternion.identity, transform);
                        cell.resourceType = ResourceType.Iron;
                    }
                    break;
                case Terrain.Hills:
                    if (random < hillsStone)
                    {
                        resource = Instantiate(map.resourcesPrefabs[4],
                            new Vector3(position.x, position.y, -1), Quaternion.identity, transform);
                        cell.resourceType = ResourceType.Stone;
                    }
                    else if (random < hillsGold)
                    {
                        resource = Instantiate(map.resourcesPrefabs[5],
                            new Vector3(position.x, position.y, -1), Quaternion.identity, transform);
                        cell.resourceType = ResourceType.Gold;
                    }
                    else if (random < hillsCattle)
                    {
                        resource = Instantiate(map.resourcesPrefabs[1],
                            new Vector3(position.x, position.y, -1), Quaternion.identity, transform);
                        cell.resourceType = ResourceType.Cattle;
                    }
                    else if (random < hillsCopper)
                    {
                        resource = Instantiate(map.resourcesPrefabs[7],
                            new Vector3(position.x, position.y, -1), Quaternion.identity, transform);
                        cell.resourceType = ResourceType.Copper;
                    }
                    else if (random < hillsIron)
                    {
                        resource = Instantiate(map.resourcesPrefabs[8],
                            new Vector3(position.x, position.y, -1), Quaternion.identity, transform);
                        cell.resourceType = ResourceType.Iron;
                    }
                    break;
            }
            if (resource != null)
            {
                resource.SetActive(false);
                cell.resource = resource;
            }
        }
    }

    public void GenerateCivilizations()
    {
        CivilizationName[] names = (CivilizationName[]) Enum.GetValues(typeof(CivilizationName));
        int index = Array.IndexOf(names, GameLogic.SelectedCiv);
        CivilizationName temp = names[index];
        Array.Copy(names, 0, names, 1, index);
        names[0] = temp;
        List<Cell> spawnCells = new List<Cell>();
        foreach (Cell cell in map.Cells)
        {
            if (cell.rivers.Count > 0 && cell.terrain != Terrain.Hills && cell.terrain != Terrain.DesertHills)
            {
                spawnCells.Add(cell);
            }
        }
        int random;
        Cell selectedCell;
        int riverId;
        List<Cell> path;
        float distance;
        for (int i = 0; i < names.Length; i++)
        {
            GameLogic.Civs[i] = new Civilization(names[i]);
            random = Random.Range(0, spawnCells.Count);
            selectedCell = spawnCells[random];
            riverId = selectedCell.rivers[0].id;
            map.CreateUnit(UnitType.Settler, selectedCell, GameLogic.Civs[i]);
            for (int j = spawnCells.Count - 1; j >= 0; j--)
            {
                path = Pathfinder.FindPath(selectedCell, spawnCells[j], true,
                    float.PositiveInfinity, GameLogic.SelectedCiv, null);
                if (path != null)
                {
                    distance = Pathfinder.CalculateTotalPathLength(path, false);
                }
                else
                {
                    distance = 0;
                }
                if (spawnCells[j].rivers[0].id == riverId || distance < 20)
                {
                    spawnCells.RemoveAt(j);
                }
            }
            if (i != 0)
            {
                GameLogic.AIs[i - 1] = new AI(map, GameLogic.Civs[i]);
            }
        }
    }
}