using System.Collections.Generic;
using UnityEngine;

public static class Pathfinder
{
    private static Unit _unit;
    public static List<Cell> FindPath(Cell start, Cell goal, bool withCoefs, float movePoints, CivilizationName civName,
        HashSet<Cell> exploredCells)
    {
        if (start.unit != null)
        {
            _unit = start.unit;
        }
        else
        {
            _unit = null;
        }
        List<Cell> openSet = new List<Cell>();
        HashSet<Cell> closedSet = new HashSet<Cell>();
        Dictionary<Cell, Cell> cameFrom = new Dictionary<Cell, Cell>();
        Dictionary<Cell, float> gScore = new Dictionary<Cell, float>();
        Dictionary<Cell, float> fScore = new Dictionary<Cell, float>();

        openSet.Add(start);
        gScore[start] = 0;
        fScore[start] = HeuristicCostEstimate(start, goal);

        Cell closestCell = null;
        float closestDistance = float.MaxValue;

        while (openSet.Count > 0)
        {
            Cell current = GetCellWithLowestFScore(openSet, fScore);
            if (current == goal)
            {
                List<Cell> path = ReconstructPath(cameFrom, current);
                if (withCoefs)
                {
                    path = TrimPath(path, movePoints, civName);
                }
                return path;
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (Cell neighbor in current.neighbors)
            {
                if (neighbor == null || closedSet.Contains(neighbor) || (exploredCells != null
                                                                         && !exploredCells.Contains(neighbor)))
                    continue;
                float cost = CalculateCost(current, neighbor, withCoefs);
                if (cost == 0f)
                {
                    continue;
                }
                float tentativeGScore = gScore[current] + cost;
                if (!openSet.Contains(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + HeuristicCostEstimate(neighbor, goal);

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);

                    float distanceToGoal = HeuristicCostEstimate(neighbor, goal);
                    if (distanceToGoal < closestDistance)
                    {
                        closestCell = neighbor;
                        closestDistance = distanceToGoal;
                    }
                }
            }
        }

        if (closestCell != null)
        {
            List<Cell> closestPath = ReconstructPath(cameFrom, closestCell);
            if (withCoefs)
            {
                closestPath = TrimPath(closestPath, movePoints, civName);
                if (_unit != null && exploredCells != null)
                {
                    _unit.rememberedCell = closestCell;
                }
                else
                {
                    return null;
                }
            }
            return closestPath;
        }

        return null;
    }
    
    public static HashSet<Cell> FindReachableCells(Cell start, float movePoints, HashSet<Cell> exploredCells)
    {
        HashSet<Cell> reachableCells = new HashSet<Cell>();
        List<Cell> openSet = new List<Cell>();
        Dictionary<Cell, float> gScore = new Dictionary<Cell, float>();
        openSet.Add(start);
        gScore[start] = 0f;
        reachableCells.Add(start);
        while (openSet.Count > 0)
        {
            Cell current = openSet[0];
            openSet.RemoveAt(0);

            foreach (Cell neighbor in current.neighbors)
            {
                if (neighbor == null || reachableCells.Contains(neighbor) || !exploredCells.Contains(neighbor))
                    continue;

                float cost = CalculateCost(current, neighbor, true);
                float tentativeGScore = gScore[current] + cost;

                if ((neighbor == start || cost > 0f) && tentativeGScore <= movePoints
                                                     && !IsFriendlyUnit(neighbor, start))
                {
                    reachableCells.Add(neighbor);
                    openSet.Add(neighbor);
                    gScore[neighbor] = tentativeGScore;
                }
            }
        }
        return reachableCells;
    }
    
    private static bool IsFriendlyUnit(Cell cell, Cell startCell)
    {
        return cell.unit != null && cell.unit.Owner.Name == startCell.unit.Owner.Name && cell.unit != startCell.unit;
    }

    private static float HeuristicCostEstimate(Cell start, Cell goal)
    {
        int deltaX = Mathf.Abs(goal.axialCoordinates.x - start.axialCoordinates.x);
        int deltaY = Mathf.Abs(goal.axialCoordinates.y - start.axialCoordinates.y);
        int minDelta = Mathf.Min(deltaX, deltaY);
        int maxDelta = Mathf.Max(deltaX, deltaY);
        float diagonalDistance = minDelta + maxDelta - minDelta / 2;
        float diagonalMultiplier = 1.5f;
        return diagonalDistance * diagonalMultiplier;
    }

    public static float CalculateCost(Cell current, Cell neighbor, bool withCoefs)
    {
        if (!withCoefs)
        {
            return 1f;
        }
        if (_unit != null && neighbor.unit != null && neighbor.unit.Owner.Name != _unit.Owner.Name)
        {
            return 0f;
        }
        if (_unit != null && neighbor.city != null && neighbor.city.Owner.Name != _unit.Owner.Name)
        {
            return 0f;
        }
        float cost = 0f;
        switch (neighbor.terrain)
        {
            case Terrain.Water:
                cost += GameLogic.WaterCost;
                break;
            case Terrain.Plain:
                cost += GameLogic.PlainCost;
                break;
            case Terrain.Desert:
                cost += GameLogic.DesertCost;
                break;
            case Terrain.Floodplain:
                cost += GameLogic.FloodplainCost;
                break;
            case Terrain.Forest:
                cost += GameLogic.ForestCost;
                break;
            case Terrain.Hills:
                cost += GameLogic.HillsCost;
                break;
            case Terrain.DesertHills:
                cost += GameLogic.DesertHillsCost;
                break;
            case Terrain.Mountains:
                cost += GameLogic.MountainsCost;
                break;
        }
        if (cost == 0f)
        {
            return cost;
        }
        if (current.rivers != null && neighbor.rivers != null)
        {
            foreach (River river1 in current.rivers)
            {
                foreach (River river2 in neighbor.rivers)
                {
                    if (river1 == river2)
                    {
                        cost += GameLogic.RiverCost;
                    }
                }
            }
        }
        return cost;
    }

    private static Cell GetCellWithLowestFScore(List<Cell> openSet, Dictionary<Cell, float> fScore)
    {
        float minFScore = float.MaxValue;
        Cell minCell = null;
        foreach (Cell cell in openSet)
        {
            float score = fScore.ContainsKey(cell) ? fScore[cell] : float.MaxValue;
            if (score < minFScore)
            {
                minFScore = score;
                minCell = cell;
            }
        }
        return minCell;
    }

    private static List<Cell> ReconstructPath(Dictionary<Cell, Cell> cameFrom, Cell current)
    {
        List<Cell> path = new List<Cell> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Insert(0, current);
        }
        return path;
    }
    
    public static float CalculateTotalPathLength(List<Cell> path, bool withCoefs)
    {
        float totalLength = 0f;
        for (int i = 0; i < path.Count - 1; i++)
        {
            totalLength += CalculateCost(path[i], path[i + 1], withCoefs);
        }
        return totalLength;
    }
    
    private static List<Cell> TrimPath(List<Cell> path, float movePoints, CivilizationName civName)
    {
        float remainingMovePoints = movePoints;
        List<Cell> trimmedPath = new List<Cell>();
        trimmedPath.Add(path[0]);
        for (int i = 0; i < path.Count - 1; i++)
        {
            float cost = CalculateCost(path[i], path[i + 1], true);
            if (remainingMovePoints >= cost && (!IsEnemyUnitInNeighborCells(path[i], civName)
                                            && !IsEnemyCityInNeighborCells(path[i], civName) || i == 0))
            {
                trimmedPath.Add(path[i + 1]);
                remainingMovePoints -= cost;
            }
            else if (i != 0)
            {
                break;
            }
        }
        for (int i = trimmedPath.Count - 1; i >= 0; i--)
        {
            if (trimmedPath[i].unit != null && trimmedPath[i].unit.Owner.Name == _unit.Owner.Name)
            {
                trimmedPath.RemoveAt(i);
            }
            else
            {
                break;
            }
        }
        return trimmedPath;
    }
    
    private static bool IsEnemyUnitInNeighborCells(Cell cell, CivilizationName civName)
    {
        foreach (Cell neighbor in cell.neighbors)
        {
            if (neighbor.unit != null && neighbor.unit.Owner.Name != civName && neighbor.unit is CombatUnit)
            {
                return true;
            }
        }
        return false;
    }
    
    private static bool IsEnemyCityInNeighborCells(Cell cell, CivilizationName civName)
    {
        foreach (Cell neighbor in cell.neighbors)
        {
            if (neighbor.city != null && neighbor.city.Owner.Name != civName)
            {
                return true;
            }
        }
        return false;
    }
}