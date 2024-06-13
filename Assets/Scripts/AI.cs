using UnityEngine;

public class AI
{
    public Map Map;
    public Civilization Civ;
    
    public AI(Map map, Civilization civ)
    {
        Map = map;
        Civ = civ;
    }
    
    public void MakeDecision()
    {
        if (GameLogic.Turn == 1)
        {
            Map.CreateCity((Settler)Civ.Units[0]);
        }
        Cell cell = GameObject.Find("Map").GetComponent<Map>().SelectCellForNewUnit(Civ.Cities[0]);
        if (cell != null)
        {
            if (GameLogic.CompareResources(UnitType.Scout, Civ))
            {
                Map.CreateUnit(UnitType.Scout, cell, Civ);
            }
        }
    }
}