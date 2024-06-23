using System.Linq;
using UnityEngine;

public class SaveLoader : MonoBehaviour
{
    [SerializeField] private Map map;
    
    public void SaveGame()
    {
        if (Camera.main != null)
        {
            ES3.Save("cameraPosition", Camera.main.transform.position);
            ES3.Save("cameraRotation", Camera.main.transform.rotation);
            ES3.Save("cameraScale", Camera.main.transform.localScale);
            ES3.Save("cameraSize", Camera.main.orthographicSize);
        }
        ES3.Save("gameLogic", GameLogic.CollectGameLogicData());
        CellData[,] cellsData = new CellData[map.size.y, map.size.x];
        for (int i = 0; i < map.size.y; i++)
        {
            for (int j = 0; j < map.size.x; j++)
            {
                cellsData[i, j] = map.Cells[i, j].CollectCellData();
            }
        }
        ES3.Save("cells", cellsData);
        ES3.Save("size", new Vector2Int(map.size.x, map.size.y));
    }
    
    public void LoadGame()
    {
        if (Camera.main != null)
        {
            Camera.main.transform.position = ES3.Load<Vector3>("cameraPosition");
            Camera.main.transform.rotation = ES3.Load<Quaternion>("cameraRotation");
            Camera.main.transform.localScale = ES3.Load<Vector3>("cameraScale");
            Camera.main.orthographicSize = ES3.Load<float>("cameraSize");
        }
        GameLogicData gameLogicData = ES3.Load<GameLogicData>("gameLogic");
        GameLogic.Result = null;
        GameLogic.SelectedCiv = gameLogicData.selectedCiv;
        GameLogic.Civs = new Civilization[gameLogicData.civs.Length];
        GameLogic.AIs = new AI[gameLogicData.civs.Length - 1];
        for (int i = 0; i < gameLogicData.civs.Length; i++)
        {
            GameLogic.Civs[i] = new Civilization(gameLogicData.civs[i]);
            if (i != 0)
            {
                GameLogic.AIs[i - 1] = new AI(map, GameLogic.Civs[i]);
            }
        }
        GameLogic.Turn = gameLogicData.turn;
        GameLogic.WaterCost = gameLogicData.waterCost;
        GameLogic.PlainCost = gameLogicData.plainCost;
        GameLogic.DesertCost = gameLogicData.desertCost;
        GameLogic.FloodplainCost = gameLogicData.floodplainCost;
        GameLogic.ForestCost = gameLogicData.forestCost;
        GameLogic.HillsCost = gameLogicData.hillsCost;
        GameLogic.DesertHillsCost = gameLogicData.desertHillsCost;
        GameLogic.MountainsCost = gameLogicData.mountainsCost;
        GameLogic.RiverCost = gameLogicData.riverCost;
        GameLogic.UnitsCosts = gameLogicData.UnitsCosts;
        GameLogic.BuildingsCosts = gameLogicData.BuildingsCosts;
        GameLogic.CityExpansionRadius = gameLogicData.cityExpansionRadius;
        GameLogic.ExpansionFoodCount = gameLogicData.expansionFoodCount;
        GameLogic.OrdinaryResourcesModifier = gameLogicData.ordinaryResourcesModifier;
        GameLogic.StrategicResourcesModifier = gameLogicData.strategicResourcesModifier;
        GameLogic.CapitalModifier = gameLogicData.capitalModifier;
        GameLogic.RiverModifier = gameLogicData.riverModifier;
        CellData[,] cellsData = ES3.Load<CellData[,]>("cells");
        for (int i = 0; i < map.size.y; i++)
        {
            for (int j = 0; j < map.size.x; j++)
            {
                GameObject cell = Instantiate(map.cellPrefab, map.GetCellCoordinates(j, i),
                    map.GetCellAngle(j, i), transform);
                cell.GetComponent<Cell>().Init(cellsData[i, j]);
                cell.GetComponent<Cell>().SetTerraIncognitaColor();
                map.Cells[i, j] = cell.GetComponent<Cell>();
                if (cellsData[i, j].resourceType != ResourceType.None)
                {
                    switch (cellsData[i, j].resourceType)
                    {
                        case ResourceType.Wheat:
                            map.Cells[i, j].resource =
                                Instantiate(map.resourcesPrefabs[0],
                                    new Vector3(map.Cells[i, j].transform.position.x,
                                        map.Cells[i, j].transform.position.y, -1), Quaternion.identity, transform);
                            break;
                        case ResourceType.Cattle:
                            map.Cells[i, j].resource =
                                Instantiate(map.resourcesPrefabs[1],
                                    new Vector3(map.Cells[i, j].transform.position.x,
                                        map.Cells[i, j].transform.position.y, -1), Quaternion.identity, transform);
                            break;
                        case ResourceType.Fish:
                            map.Cells[i, j].resource =
                                Instantiate(map.resourcesPrefabs[2],
                                    new Vector3(map.Cells[i, j].transform.position.x,
                                        map.Cells[i, j].transform.position.y, -1), Quaternion.identity, transform);
                            break;
                        case ResourceType.Wood:
                            map.Cells[i, j].resource =
                                Instantiate(map.resourcesPrefabs[3],
                                    new Vector3(map.Cells[i, j].transform.position.x,
                                        map.Cells[i, j].transform.position.y, -1), Quaternion.identity, transform);
                            break;
                        case ResourceType.Stone:
                            map.Cells[i, j].resource =
                                Instantiate(map.resourcesPrefabs[4],
                                    new Vector3(map.Cells[i, j].transform.position.x,
                                        map.Cells[i, j].transform.position.y, -1), Quaternion.identity, transform);
                            break;
                        case ResourceType.Gold:
                            map.Cells[i, j].resource =
                                Instantiate(map.resourcesPrefabs[5],
                                    new Vector3(map.Cells[i, j].transform.position.x,
                                        map.Cells[i, j].transform.position.y, -1), Quaternion.identity, transform);
                            break;
                        case ResourceType.Horses:
                            map.Cells[i, j].resource =
                                Instantiate(map.resourcesPrefabs[6],
                                    new Vector3(map.Cells[i, j].transform.position.x,
                                        map.Cells[i, j].transform.position.y, -1), Quaternion.identity, transform);
                            break;
                        case ResourceType.Copper:
                            map.Cells[i, j].resource =
                                Instantiate(map.resourcesPrefabs[7],
                                    new Vector3(map.Cells[i, j].transform.position.x,
                                        map.Cells[i, j].transform.position.y, -1), Quaternion.identity, transform);
                            break;
                        case ResourceType.Iron:
                            map.Cells[i, j].resource =
                                Instantiate(map.resourcesPrefabs[8],
                                    new Vector3(map.Cells[i, j].transform.position.x,
                                        map.Cells[i, j].transform.position.y, -1), Quaternion.identity, transform);
                            break;
                    }
                    map.Cells[i, j].resource.SetActive(false);
                }
                if (cellsData[i, j].rivers.Count > 0)
                {
                    foreach (RiverData riverData in cellsData[i, j].rivers)
                    {
                        River river = map.Rivers.FirstOrDefault(river => river.coordinates == riverData.coordinates);
                        if (river == null)
                        {
                            Vector2Int coordinates = riverData.coordinates;
                            river = Instantiate(map.riverPrefab, map.GetRiverCoordinates(coordinates.x,
                                    coordinates.y), map.GetRiverAngle(coordinates.x, coordinates.y),
                                transform).GetComponent<River>();
                            river.transform.localScale = new Vector3(map.GetRiverLength(coordinates.x, coordinates.y),
                                0.20f, -1f);
                            river.Init(riverData);
                            river.cell1 = map.Cells[i, j];
                            map.Cells[i, j].rivers.Add(river);
                            map.Rivers.Add(river);
                        }
                        else
                        {
                            river.cell2 = map.Cells[i, j];
                            map.Cells[i, j].rivers.Add(river);
                        }
                    }
                }
            }
        }
        for (int i = 0; i < map.size.y; i++)
        {
            for (int j = 0; j < map.size.x; j++)
            {
                map.SetCellNeighbors(map.Cells[i, j]);
            }
        }
        foreach (River river in map.Rivers)
        {
            map.SetRiverNeighbors(river);
            river.gameObject.SetActive(false);
        }
        for (int i = 0; i < map.size.y; i++)
        {
            for (int j = 0; j < map.size.x; j++)
            {
                if (cellsData[i, j].unit != null)
                {
                    Civilization civ =
                        GameLogic.Civs.FirstOrDefault(civ => civ.Name == cellsData[i, j].unit.owner);
                    map.CreateUnit(cellsData[i, j].unit, map.Cells[i, j], civ);
                }
                if (cellsData[i, j].city != null
                    && cellsData[i, j].city.coreCellCoordinates == cellsData[i, j].offsetCoordinates)
                {
                    Civilization civ =
                        GameLogic.Civs.FirstOrDefault(civ => civ.Name == cellsData[i, j].city.owner);
                    map.CreateCity(cellsData[i,j].city, map.Cells[i, j], civ);
                }
                foreach (Civilization civ in GameLogic.Civs)
                {
                    if (cellsData[i, j].IsExplored[civ.Name])
                    {
                        if (civ.Name == GameLogic.SelectedCiv)
                        {
                            if (map.Cells[i, j].rivers.Count > 0)
                            {
                                foreach (River river in map.Cells[i, j].rivers)
                                {
                                    if (cellsData[river.cell1.offsetCoordinates.y,
                                            river.cell1.offsetCoordinates.x].IsExplored[GameLogic.SelectedCiv]
                                        && cellsData[river.cell2.offsetCoordinates.y,
                                            river.cell2.offsetCoordinates.x].IsExplored[GameLogic.SelectedCiv])
                                    {
                                        river.gameObject.SetActive(true);
                                    }
                                }
                            }
                            if (map.Cells[i, j].resource != null)
                            {
                                map.Cells[i, j].resource.SetActive(true);
                            }
                        }
                        civ.ExploredCells.Add(map.Cells[i, j]);
                    }
                }
            }
        }
    }
}