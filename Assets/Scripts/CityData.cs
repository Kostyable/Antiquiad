using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CityData
{
    public string cityName;
    public CivilizationName owner;
    public bool isCapital;
    public Vector2Int coreCellCoordinates;
    public HashSet<Vector2Int> CellsCoordinates;
    public Dictionary<ResourceType, int> ResourcesCount;
    public Dictionary<ResourceType, float> ResourcesModifiers;
    public float totalFoodCount;
    public HashSet<BuildingType> Buildings;
    public float healthPoints;
    public float currentHealthPoints;
    public float defence;
    public int currentExpansionRadius;
    public int expansionCellsCount;
}