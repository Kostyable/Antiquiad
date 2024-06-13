using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CellData
{
    public Vector2Int offsetCoordinates;
    public Vector2Int axialCoordinates;
    public CellType type;
    public float altitude;
    public float humidity;
    public Terrain terrain;
    public ResourceType resourceType;
    public List<RiverData> rivers;
    public UnitData unit;
    public CityData city;
    public Dictionary<CivilizationName, bool> IsExplored;
}