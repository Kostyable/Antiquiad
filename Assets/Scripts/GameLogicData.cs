using System;
using System.Collections.Generic;

[Serializable]
public class GameLogicData
{
    public CivilizationName selectedCiv;
    public CivilizationData[] civs;
    public int turn;
    public float waterCost;
    public float plainCost;
    public float desertCost;
    public float floodplainCost;
    public float forestCost;
    public float hillsCost;
    public float desertHillsCost;
    public float mountainsCost;
    public float riverCost;
    public Dictionary<UnitType, Dictionary<ResourceType, int>> UnitsCosts;
    public Dictionary<BuildingType, Dictionary<ResourceType, int>> BuildingsCosts;
    public int cityExpansionRadius;
    public int expansionFoodCount;
    public float ordinaryResourcesModifier;
    public float strategicResourcesModifier;
    public float capitalModifier;
    public float riverModifier;
}