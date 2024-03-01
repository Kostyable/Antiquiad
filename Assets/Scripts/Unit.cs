using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public UnitType type;
    public Cell currentCell;
    public Civilization Owner;
    public float movePoints;
    public float currentMovePoints;
    public int sight;
    public HashSet<Cell> cellsInSight;
    public HashSet<Cell> reachableCells;
    public Cell rememberedCell;
    public HashSet<Cell> borders;
    public bool isAttack;

    public virtual void Init(UnitType unitType, Cell cell, Civilization owner)
    {
        type = unitType;
        currentCell = cell;
        currentCell.unit = this;
        Owner = owner;
        currentMovePoints = movePoints;
        borders = new HashSet<Cell>();
        SetCellsInSight(cell);
    }

    public void Move(Cell goalCell)
    {
        if (rememberedCell != null)
        {
            goalCell = rememberedCell;
        }
        List<Cell> path = Pathfinder.FindPath(currentCell, goalCell, true, currentMovePoints, Owner.Name,
            Owner.ExploredCells);
        if (path != null && path.Count > 0)
        {
            if (this == Map.SelectedUnit)
            {
                DeleteBorders();
            }
            float distance = Pathfinder.CalculateTotalPathLength(path, true);
            currentCell.unit = null;
            currentCell = path[^1];
            currentCell.unit = this;
            foreach (Cell neighbor in currentCell.neighbors)
            {
                if ((neighbor.unit != null && neighbor.unit.Owner.Name != Owner.Name)
                    || (neighbor.city != null && neighbor.city.Owner.Name != Owner.Name))
                {
                    currentMovePoints = 0;
                    break;
                }
            }
            if (currentMovePoints != 0)
            {
                currentMovePoints -= distance;
            }
            Vector2 position = currentCell.GetComponent<Transform>().position;
            gameObject.GetComponent<Transform>().position = new Vector3(position.x, position.y, -2);
            for (int i = 1; i < path.Count; i++)
            {
                cellsInSight.Clear();
                SetCellsInSight(path[i]);
                Owner.CreateFogOfWar();
            }
            if (path[^1] == rememberedCell)
            {
                rememberedCell = null;
            }
            if (this == Map.SelectedUnit && path[^1] != rememberedCell)
            {
                GameObject.Find("Main Camera").GetComponent<UIController>().UpdateUnitInfo(this);
                CreateBorders();
            }
        }
    }

    public void SetCellsInSight(Cell cell)
    {
        cellsInSight = cell.GetCellsInRadius(sight, true);
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
    
    public void CreateBorders()
    {
        reachableCells = Pathfinder.FindReachableCells(currentCell, currentMovePoints, Owner.ExploredCells);
        foreach (Cell cell in reachableCells)
        {
            for (int i = 0; i < cell.neighbors.Length; i++)
            {
                if (cell.neighbors[i] != null && !reachableCells.Contains(cell.neighbors[i]))
                {
                    cell.CreateBorder(i, true);
                    borders.Add(cell);
                }
            }
            foreach (GameObject border in cell.unitBorders)
            {
                border.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }

    public void DeleteBorders()
    {
        foreach (Cell cell in borders)
        {
            foreach (GameObject border in cell.unitBorders)
            {
                Destroy(border);
            }
            cell.unitBorders.Clear();
        }
        borders.Clear();
    }

    private void OnDestroy()
    {
        Owner.Units.Remove(this);
        foreach (Civilization civ in GameLogic.Civs)
        {
            if (civ.EnemyUnits.Contains(this))
            {
                civ.EnemyUnits.Remove(this);
            }
        }
        Owner.CreateFogOfWar();
        currentCell.unit = null;
        if (this == Map.SelectedUnit)
        {
            DeleteBorders();
            if (GameObject.Find("Main Camera") != null)
            {
                GameObject.Find("Main Camera").GetComponent<UIController>().UnselectObject();
            }
        }
    }
}