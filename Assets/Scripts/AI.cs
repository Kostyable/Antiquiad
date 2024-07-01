using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AI
{
    public Map Map;
    public Civilization Civ;
    public UnitType UnitToCreate;
    public Cell CellToExplore;
    public City CityToAttack;
    
    public AI(Map map, Civilization civ)
    {
        Map = map;
        Civ = civ;
    }

    public void FindCityPlace(Settler settler)
    {
        if (Civ.Cities.Count == 10)
        {
            return;
        }
        Dictionary<Cell, Dictionary<ResourceType, int>> places = new Dictionary<Cell, Dictionary<ResourceType, int>>();
        foreach (Cell exploredCell in Civ.ExploredCells)
        {
            if (exploredCell.Owner == null && exploredCell.neighbors.Any(cell => cell != null && cell.Owner == null))
            {
                for (int i = 1; i <= GameLogic.CityExpansionRadius; i++)
                {
                    HashSet<Cell> cells = exploredCell.GetCellsWithRadius(i);
                    foreach (Cell cell in cells)
                    {
                        if (Civ.ExploredCells.Contains(cell) && cell.Owner == null && cell.resource != null
                            && cell.unit == null)
                        {
                            if (!places.ContainsKey(exploredCell))
                            {
                                places[exploredCell] = new Dictionary<ResourceType, int>();
                            }
                            if (!places[exploredCell].ContainsKey(cell.resourceType))
                            {
                                places[exploredCell][cell.resourceType] = 0;
                            }
                            places[exploredCell][cell.resourceType]++;
                        }
                    }
                    if (i == 1 && places.TryGetValue(exploredCell, out var resources)
                               && resources.Any(resource => (resource.Key == ResourceType.Wheat
                                                             || resource.Key == ResourceType.Cattle
                                                             || resource.Key == ResourceType.Fish)
                                                            && resource.Value > 0))
                    {
                        places.Remove(exploredCell);
                        break;
                    }
                }
            }
        }
        Cell bestCell = places.OrderByDescending(place =>
                place.Value.Sum(resource => resource.Value))
            .Select(place => place.Key).FirstOrDefault();
        if (bestCell != null)
        {
            List<Cell> cells = Pathfinder.FindPath(settler.currentCell, bestCell, true,
                settler.currentMovePoints, Civ.Name, Civ.ExploredCells);
            if (bestCell == settler.currentCell)
            {
                Map.CreateCity(settler);
            }
            else
            {
                settler.Move(cells[^1]);
                if (settler.currentCell == cells[^1])
                {
                    Map.CreateCity(settler);
                }
            }
        }
    }

    public void ExploreCells(Scout scout)
    {
        CellToExplore = Map.Cells.Cast<Cell>().ToList().Except(Civ.ExploredCells).ToList()[Random.Range
            (0, Map.Cells.GetLength(0) * Map.Cells.GetLength(1) - Civ.ExploredCells.Count)];
        scout.Move(CellToExplore);
    }
    
    public void AttackCity(Scout scout)
    {
        CityToAttack = Civ.EnemyCities.ToList()[Random.Range(0, Civ.EnemyCities.Count)];
        scout.Move(CityToAttack.coreCell);
        if (scout.currentCell.neighbors.Contains(CityToAttack.coreCell))
        {
            scout.Attack(CityToAttack);
        }
    }
    
    public void SelectObjectToCreate()
    {
        List<City> cities = new List<City>();
        foreach (Civilization civ in GameLogic.Civs)
        {
            cities.AddRange(civ.Cities);
        }
        if (Civ.EnemyCities.Count < cities.Count - 1)
        {
            UnitToCreate = UnitType.Scout;
        }
        else
        {
            UnitToCreate = UnitType.Settler;
        }
    }
    
    public void MakeDecision()
    {
        foreach (Unit unit in Civ.Units)
        {
            if (unit is Settler)
            {
                FindCityPlace((Settler)unit);
            }
            if (unit is Scout)
            {
                if (Civ.EnemyCities.Count > 0)
                {
                    AttackCity((Scout)unit);
                }
                else
                {
                    ExploreCells((Scout)unit);
                }
            }
        }
        foreach (City city in Civ.Cities)
        {
            Cell cell = Map.SelectCellForNewUnit(city);
            if (cell != null)
            {
                SelectObjectToCreate();
                if (GameLogic.CompareResources(UnitToCreate, Civ))
                {
                    Map.CreateUnit(UnitToCreate, cell, Civ);
                }
            }
        }
    }
}