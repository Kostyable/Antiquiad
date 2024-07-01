using System.Collections.Generic;

public static class GameLogic
{
    public static CivilizationName SelectedCiv;
    public static bool Tutorial;
    public static bool LoadGame;
    public static Civilization[] Civs;
    public static AI[] AIs;
    public static int Turn;
    public static float WaterCost;
    public static float PlainCost;
    public static float DesertCost;
    public static float FloodplainCost;
    public static float ForestCost;
    public static float HillsCost;
    public static float DesertHillsCost;
    public static float MountainsCost;
    public static float RiverCost;
    public static Dictionary<UnitType, Dictionary<ResourceType, int>> UnitsCosts;
    public static Dictionary<BuildingType, Dictionary<ResourceType, int>> BuildingsCosts;
    public static int CityExpansionRadius;
    public static int ExpansionFoodCount;
    public static float OrdinaryResourcesModifier;
    public static float StrategicResourcesModifier;
    public static float CapitalModifier;
    public static float RiverModifier;

    public static GameLogicData CollectGameLogicData()
    {
        GameLogicData data = new GameLogicData();
        data.selectedCiv = SelectedCiv;
        data.civs = new CivilizationData[Civs.Length];
        for (int i = 0; i < Civs.Length; i++)
        {
            data.civs[i] = Civs[i].CollectCivilizationData();
        }
        data.turn = Turn;
        data.waterCost = WaterCost;
        data.plainCost = PlainCost;
        data.desertCost = DesertCost;
        data.floodplainCost = FloodplainCost;
        data.forestCost = ForestCost;
        data.hillsCost = HillsCost;
        data.desertHillsCost = DesertHillsCost;
        data.mountainsCost = MountainsCost;
        data.riverCost = RiverCost;
        data.UnitsCosts = UnitsCosts;
        data.BuildingsCosts = BuildingsCosts;
        data.cityExpansionRadius = CityExpansionRadius;
        data.expansionFoodCount = ExpansionFoodCount;
        data.ordinaryResourcesModifier = OrdinaryResourcesModifier;
        data.strategicResourcesModifier = StrategicResourcesModifier;
        data.capitalModifier = CapitalModifier;
        data.riverModifier = RiverModifier;
        return data;
    }

    public static void UpdateTurn()
    {
        if (Map.SelectedUnit != null)
        {
            Map.SelectedUnit.DeleteBorders();
            Map.SelectedUnit = null;
        }
        Map.SelectedCity = null;
        for (int i = 0; i < AIs.Length; i++)
        {
            AIs[i].Civ.CreateFogOfWar();
            AIs[i].MakeDecision();
        }
        foreach (Civilization civ in Civs)
        {
            civ.CheckEnemyObjects();
            foreach (ResourceType resourceType in civ.ResourcesAdds.Keys)
            {
                civ.ResourcesValues[resourceType] += civ.ResourcesAdds[resourceType];
            }
            foreach (Unit unit in civ.Units)
            {
                if (unit.rememberedCell != null)
                {
                    unit.Move(unit.rememberedCell);
                }
                unit.currentMovePoints = unit.movePoints;
                unit.isAttack = false;
            }
            foreach (City city in civ.Cities)
            {
                float cityFoodCount = 0f;
                cityFoodCount += (int)(city.ResourcesCount[ResourceType.Wheat]
                    * city.ResourcesModifiers[ResourceType.Wheat] + city.ResourcesCount[ResourceType.Cattle]
                    * city.ResourcesModifiers[ResourceType.Cattle] + city.ResourcesCount[ResourceType.Fish]
                    * city.ResourcesModifiers[ResourceType.Fish]);
                if (city.isCapital)
                {
                    cityFoodCount += CapitalModifier;
                }
                if (city.coreCell.rivers.Count > 0)
                {
                    cityFoodCount = (int)(cityFoodCount * RiverModifier);
                }
                city.totalFoodCount += cityFoodCount;
                if (city.totalFoodCount >= city.expansionCellsCount * ExpansionFoodCount + ExpansionFoodCount
                    && city.currentExpansionRadius <= CityExpansionRadius)
                {
                    city.Expand();
                }
            }
        }
        Turn++;
        Civs[0].CreateFogOfWar();
    }

    public static bool CompareResources(UnitType unitType, Civilization civ)
    {
        bool flag = false;
        foreach (ResourceType resourceType in UnitsCosts[unitType].Keys)
        {
            if (civ.ResourcesValues[resourceType] < UnitsCosts[unitType][resourceType])
            {
                flag = true;
                break;
            }
        }
        if (flag)
        {
            return false;
        }
        foreach (ResourceType resourceType in UnitsCosts[unitType].Keys)
        {
            civ.ResourcesValues[resourceType] -= UnitsCosts[unitType][resourceType];
        }
        return true;
    }
    
    public static bool CompareResources(BuildingType buildingType, Civilization civ)
    {
        bool flag = false;
        foreach (ResourceType resourceType in BuildingsCosts[buildingType].Keys)
        {
            if (civ.ResourcesValues[resourceType] < BuildingsCosts[buildingType][resourceType])
            {
                flag = true;
                break;
            }
        }
        if (flag)
        {
            return false;
        }
        foreach (ResourceType resourceType in BuildingsCosts[buildingType].Keys)
        {
            civ.ResourcesValues[resourceType] -= BuildingsCosts[buildingType][resourceType];
        }
        return true;
    }
}