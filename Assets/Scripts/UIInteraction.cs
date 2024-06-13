using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIInteraction : MonoBehaviour
{
    private TMP_FontAsset _originalFont;
    [SerializeField] private TMP_FontAsset fontOnEnter;
    private Vector3 _originalScale;
    [SerializeField] private GameObject message;
    [SerializeField] private GameObject menu;
    
    public void OnTextEnter()
    {
        _originalFont = GetComponent<TextMeshProUGUI>().font;
        _originalScale = transform.localScale;
        GetComponent<TextMeshProUGUI>().font = fontOnEnter;
        transform.localScale *= 1.1f;
    }
    
    public void OnTextExit()
    {
        GetComponent<TextMeshProUGUI>().font = _originalFont;
        transform.localScale = _originalScale;
    }
    
    public void OnTextWithImageEnter()
    {
        RectTransform parentRectTransform = gameObject.GetComponent<RectTransform>();
        RectTransform nameRectTransform = parentRectTransform.GetChild(0).GetComponent<RectTransform>();
        _originalFont = nameRectTransform.GetComponent<TextMeshProUGUI>().font;
        _originalScale = transform.localScale;
        nameRectTransform.GetComponent<TextMeshProUGUI>().font = fontOnEnter;
        transform.localScale *= 1.1f;
    }

    public void OnTextWithImageExit()
    {
        RectTransform parentRectTransform = gameObject.GetComponent<RectTransform>();
        RectTransform nameRectTransform = parentRectTransform.GetChild(0).GetComponent<RectTransform>();
        nameRectTransform.GetComponent<TextMeshProUGUI>().font = _originalFont;
        transform.localScale = _originalScale;
    }
    
    public void OnImageEnter()
    {
        if (Map.MenuEnabled)
        {
            return;
        }
        _originalScale = transform.localScale;
        transform.localScale *= 1.1f;
    }

    public void OnImageExit()
    {
        if (Map.MenuEnabled)
        {
            return;
        }
        transform.localScale = _originalScale;
    }

    public void OnNewGameClick()
    {
        transform.localScale /= 1.1f;
        GetComponent<TextMeshProUGUI>().font = _originalFont;
        GameObject.Find("Main Camera").GetComponent<MenuManager>().ShowNewGame();
    }

    public void OnQuitClick()
    {
        Application.Quit();
    }
    
    public void OnEgyptClick()
    {
        GameLogic.SelectedCiv = CivilizationName.Egypt;
        GameLogic.Tutorial = false;
        GameLogic.LoadGame = false;
        SceneManager.LoadScene("Loading Screen");
    }
    
    public void OnGreeceClick()
    {
        GameLogic.SelectedCiv = CivilizationName.Greece;
        GameLogic.Tutorial = false;
        GameLogic.LoadGame = false;
        SceneManager.LoadScene("Loading Screen");
    }
    
    public void OnMesopotamiaClick()
    {
        GameLogic.SelectedCiv = CivilizationName.Mesopotamia;
        GameLogic.Tutorial = false;
        GameLogic.LoadGame = false;
        SceneManager.LoadScene("Loading Screen");
    }

    public void OnTurnButtonClick()
    {
        if (Map.MenuEnabled)
        {
            return;
        }
        GameLogic.UpdateTurn();
        GameObject.Find("Main Camera").GetComponent<UIController>().SetCurrentValues();
    }
    
    public void OnFoundCityClick()
    {
        HashSet<Cell> cells = Map.SelectedUnit.currentCell.GetCellsInRadius(1, false);
        foreach (Civilization civ in GameLogic.Civs)
        {
            foreach (City city in civ.Cities)
            {
                foreach (Cell cell in city.Cells)
                {
                    if (cells.Contains(cell))
                    {
                        return;
                    }
                }
            }
        }
        if (Map.SelectedUnit.Owner.Cities.Count == 10)
        {
            return;
        }
        GameObject.Find("Map").GetComponent<Map>().CreateCity((Settler)Map.SelectedUnit);
    }
    
    public void OnDisbandUnitClick()
    {
        Destroy(Map.SelectedUnit.gameObject);
    }

    public void OnCreateSettlerClick()
    {
        Cell cell = GameObject.Find("Map").GetComponent<Map>().SelectCellForNewUnit(Map.SelectedCity);
        if (cell != null)
        {
            if (GameLogic.CompareResources(UnitType.Settler, GameLogic.Civs[0]))
            {
                GameObject.Find("Main Camera").GetComponent<UIController>().SetCurrentValues();
                GameObject.Find("Main Camera").GetComponent<UIController>().UnselectObject();
                GameObject.Find("Map").GetComponent<Map>().CreateUnit(UnitType.Settler, cell, GameLogic.Civs[0]);
            }
        }
    }
    
    public void OnCreateScoutClick()
    {
        Cell cell = GameObject.Find("Map").GetComponent<Map>().SelectCellForNewUnit(Map.SelectedCity);
        if (cell != null)
        {
            if (GameLogic.CompareResources(UnitType.Scout, GameLogic.Civs[0]))
            {
                GameObject.Find("Main Camera").GetComponent<UIController>().SetCurrentValues();
                GameObject.Find("Main Camera").GetComponent<UIController>().UnselectObject();
                GameObject.Find("Map").GetComponent<Map>().CreateUnit(UnitType.Scout, cell, GameLogic.Civs[0]);
            }
        }
    }
    
    public void OnCreateWarriorClick()
    {
        Cell cell = GameObject.Find("Map").GetComponent<Map>().SelectCellForNewUnit(Map.SelectedCity);
        if (cell != null)
        {
            if (GameLogic.CompareResources(UnitType.Warrior, GameLogic.Civs[0]))
            {
                GameObject.Find("Main Camera").GetComponent<UIController>().SetCurrentValues();
                GameObject.Find("Main Camera").GetComponent<UIController>().UnselectObject();
                GameObject.Find("Map").GetComponent<Map>().CreateUnit(UnitType.Warrior, cell, GameLogic.Civs[0]);
            }
        }
    }
    
    public void OnCreateSlingerClick()
    {
        Cell cell = GameObject.Find("Map").GetComponent<Map>().SelectCellForNewUnit(Map.SelectedCity);
        if (cell != null)
        {
            if (GameLogic.CompareResources(UnitType.Slinger, GameLogic.Civs[0]))
            {
                GameObject.Find("Main Camera").GetComponent<UIController>().SetCurrentValues();
                GameObject.Find("Main Camera").GetComponent<UIController>().UnselectObject();
                GameObject.Find("Map").GetComponent<Map>().CreateUnit(UnitType.Slinger, cell, GameLogic.Civs[0]);
            }
        }
    }
    
    public void OnCreateArcherClick()
    {
        Cell cell = GameObject.Find("Map").GetComponent<Map>().SelectCellForNewUnit(Map.SelectedCity);
        if (cell != null)
        {
            if (GameLogic.CompareResources(UnitType.Archer, GameLogic.Civs[0]))
            {
                GameObject.Find("Main Camera").GetComponent<UIController>().SetCurrentValues();
                GameObject.Find("Main Camera").GetComponent<UIController>().UnselectObject();
                GameObject.Find("Map").GetComponent<Map>().CreateUnit(UnitType.Archer, cell, GameLogic.Civs[0]);
            }
        }
    }
    
    public void OnCreateChariotClick()
    {
        Cell cell = GameObject.Find("Map").GetComponent<Map>().SelectCellForNewUnit(Map.SelectedCity);
        if (cell != null)
        {
            if (GameLogic.CompareResources(UnitType.Chariot, GameLogic.Civs[0]))
            {
                GameObject.Find("Main Camera").GetComponent<UIController>().SetCurrentValues();
                GameObject.Find("Main Camera").GetComponent<UIController>().UnselectObject();
                GameObject.Find("Map").GetComponent<Map>().CreateUnit(UnitType.Chariot, cell, GameLogic.Civs[0]);
            }
        }
    }
    
    public void OnCreateSpearmanClick()
    {
        Cell cell = GameObject.Find("Map").GetComponent<Map>().SelectCellForNewUnit(Map.SelectedCity);
        if (cell != null)
        {
            if (GameLogic.CompareResources(UnitType.Spearman, GameLogic.Civs[0]))
            {
                GameObject.Find("Main Camera").GetComponent<UIController>().SetCurrentValues();
                GameObject.Find("Main Camera").GetComponent<UIController>().UnselectObject();
                GameObject.Find("Map").GetComponent<Map>().CreateUnit(UnitType.Spearman, cell, GameLogic.Civs[0]);
            }
        }
    }
    
    public void OnCreateHopliteClick()
    {
        Cell cell = GameObject.Find("Map").GetComponent<Map>().SelectCellForNewUnit(Map.SelectedCity);
        if (cell != null)
        {
            if (GameLogic.CompareResources(UnitType.Hoplite, GameLogic.Civs[0]))
            {
                GameObject.Find("Main Camera").GetComponent<UIController>().SetCurrentValues();
                GameObject.Find("Main Camera").GetComponent<UIController>().UnselectObject();
                GameObject.Find("Map").GetComponent<Map>().CreateUnit(UnitType.Hoplite, cell, GameLogic.Civs[0]);
            }
        }
    }
    
    public void OnCreateHorsemanClick()
    {
        Cell cell = GameObject.Find("Map").GetComponent<Map>().SelectCellForNewUnit(Map.SelectedCity);
        if (cell != null)
        {
            if (GameLogic.CompareResources(UnitType.Horseman, GameLogic.Civs[0]))
            {
                GameObject.Find("Main Camera").GetComponent<UIController>().SetCurrentValues();
                GameObject.Find("Main Camera").GetComponent<UIController>().UnselectObject();
                GameObject.Find("Map").GetComponent<Map>().CreateUnit(UnitType.Horseman, cell, GameLogic.Civs[0]);
            }
        }
    }
    
    public void OnCreateSwordsmanClick()
    {
        Cell cell = GameObject.Find("Map").GetComponent<Map>().SelectCellForNewUnit(Map.SelectedCity);
        if (cell != null)
        {
            if (GameLogic.CompareResources(UnitType.Swordsman, GameLogic.Civs[0]))
            {
                GameObject.Find("Main Camera").GetComponent<UIController>().SetCurrentValues();
                GameObject.Find("Main Camera").GetComponent<UIController>().UnselectObject();
                GameObject.Find("Map").GetComponent<Map>().CreateUnit(UnitType.Swordsman, cell, GameLogic.Civs[0]);
            }
        }
    }
    
    public void OnCreateCatapultClick()
    {
        Cell cell = GameObject.Find("Map").GetComponent<Map>().SelectCellForNewUnit(Map.SelectedCity);
        if (cell != null)
        {
            if (GameLogic.CompareResources(UnitType.Catapult, GameLogic.Civs[0]))
            {
                GameObject.Find("Main Camera").GetComponent<UIController>().SetCurrentValues();
                GameObject.Find("Main Camera").GetComponent<UIController>().UnselectObject();
                GameObject.Find("Map").GetComponent<Map>().CreateUnit(UnitType.Catapult, cell, GameLogic.Civs[0]);
            }
        }
    }
    
    public void OnCreateSiegeTowerClick()
    {
        Cell cell = GameObject.Find("Map").GetComponent<Map>().SelectCellForNewUnit(Map.SelectedCity);
        if (cell != null)
        {
            if (GameLogic.CompareResources(UnitType.SiegeTower, GameLogic.Civs[0]))
            {
                GameObject.Find("Main Camera").GetComponent<UIController>().SetCurrentValues();
                GameObject.Find("Main Camera").GetComponent<UIController>().UnselectObject();
                GameObject.Find("Map").GetComponent<Map>().CreateUnit(UnitType.SiegeTower, cell, GameLogic.Civs[0]);
            }
        }
    }
    
    public void OnBuildFarmClick()
    {
        Cell cell = GameObject.Find("Map").GetComponent<Map>().SelectCellForNewUnit(Map.SelectedCity);
        if (cell != null)
        {
            if (GameLogic.CompareResources(BuildingType.Farm, GameLogic.Civs[0]))
            {
                GameObject.Find("Main Camera").GetComponent<UIController>().SetCurrentValues();
                GameObject.Find("Main Camera").GetComponent<UIController>().UnselectObject();
                Map.SelectedCity.Build(BuildingType.Farm, ResourceType.Wheat);
            }
        }
    }
    
    public void OnBuildPastureClick()
    {
        Cell cell = GameObject.Find("Map").GetComponent<Map>().SelectCellForNewUnit(Map.SelectedCity);
        if (cell != null)
        {
            if (GameLogic.CompareResources(BuildingType.Pasture, GameLogic.Civs[0]))
            {
                GameObject.Find("Main Camera").GetComponent<UIController>().SetCurrentValues();
                GameObject.Find("Main Camera").GetComponent<UIController>().UnselectObject();
                Map.SelectedCity.Build(BuildingType.Pasture, ResourceType.Cattle);
            }
        }
    }
    
    public void OnBuildFishingBoatClick()
    {
        Cell cell = GameObject.Find("Map").GetComponent<Map>().SelectCellForNewUnit(Map.SelectedCity);
        if (cell != null)
        {
            if (GameLogic.CompareResources(BuildingType.FishingBoat, GameLogic.Civs[0]))
            {
                GameObject.Find("Main Camera").GetComponent<UIController>().SetCurrentValues();
                GameObject.Find("Main Camera").GetComponent<UIController>().UnselectObject();
                Map.SelectedCity.Build(BuildingType.FishingBoat, ResourceType.Fish);
            }
        }
    }
    
    public void OnBuildLumberMillClick()
    {
        Cell cell = GameObject.Find("Map").GetComponent<Map>().SelectCellForNewUnit(Map.SelectedCity);
        if (cell != null)
        {
            if (GameLogic.CompareResources(BuildingType.LumberMill, GameLogic.Civs[0]))
            {
                GameObject.Find("Main Camera").GetComponent<UIController>().SetCurrentValues();
                GameObject.Find("Main Camera").GetComponent<UIController>().UnselectObject();
                Map.SelectedCity.Build(BuildingType.LumberMill, ResourceType.Wood);
            }
        }
    }
    
    public void OnBuildQuarryClick()
    {
        Cell cell = GameObject.Find("Map").GetComponent<Map>().SelectCellForNewUnit(Map.SelectedCity);
        if (cell != null)
        {
            if (GameLogic.CompareResources(BuildingType.Quarry, GameLogic.Civs[0]))
            {
                GameObject.Find("Main Camera").GetComponent<UIController>().SetCurrentValues();
                GameObject.Find("Main Camera").GetComponent<UIController>().UnselectObject();
                Map.SelectedCity.Build(BuildingType.Quarry, ResourceType.Stone);
            }
        }
    }
    
    public void OnBuildGoldMineClick()
    {
        Cell cell = GameObject.Find("Map").GetComponent<Map>().SelectCellForNewUnit(Map.SelectedCity);
        if (cell != null)
        {
            if (GameLogic.CompareResources(BuildingType.GoldMine, GameLogic.Civs[0]))
            {
                GameObject.Find("Main Camera").GetComponent<UIController>().SetCurrentValues();
                GameObject.Find("Main Camera").GetComponent<UIController>().UnselectObject();
                Map.SelectedCity.Build(BuildingType.GoldMine, ResourceType.Gold);
            }
        }
    }
    
    public void OnBuildStableClick()
    {
        Cell cell = GameObject.Find("Map").GetComponent<Map>().SelectCellForNewUnit(Map.SelectedCity);
        if (cell != null)
        {
            if (GameLogic.CompareResources(BuildingType.Stable, GameLogic.Civs[0]))
            {
                GameObject.Find("Main Camera").GetComponent<UIController>().SetCurrentValues();
                GameObject.Find("Main Camera").GetComponent<UIController>().UnselectObject();
                Map.SelectedCity.Build(BuildingType.Stable, ResourceType.Horses);
            }
        }
    }
    
    public void OnBuildMineClick()
    {
        Cell cell = GameObject.Find("Map").GetComponent<Map>().SelectCellForNewUnit(Map.SelectedCity);
        if (cell != null)
        {
            if (GameLogic.CompareResources(BuildingType.Mine, GameLogic.Civs[0]))
            {
                GameObject.Find("Main Camera").GetComponent<UIController>().SetCurrentValues();
                GameObject.Find("Main Camera").GetComponent<UIController>().UnselectObject();
                Map.SelectedCity.Build(BuildingType.Mine, ResourceType.Copper);
            }
        }
    }
    
    public void OnBuildBlacksmithClick()
    {
        Cell cell = GameObject.Find("Map").GetComponent<Map>().SelectCellForNewUnit(Map.SelectedCity);
        if (cell != null)
        {
            if (GameLogic.CompareResources(BuildingType.Blacksmith, GameLogic.Civs[0]))
            {
                GameObject.Find("Main Camera").GetComponent<UIController>().SetCurrentValues();
                GameObject.Find("Main Camera").GetComponent<UIController>().UnselectObject();
                Map.SelectedCity.Build(BuildingType.Blacksmith, ResourceType.Iron);
            }
        }
    }
    
    public void OnBuildWallsClick()
    {
        Cell cell = GameObject.Find("Map").GetComponent<Map>().SelectCellForNewUnit(Map.SelectedCity);
        if (cell != null)
        {
            if (GameLogic.CompareResources(BuildingType.Walls, GameLogic.Civs[0]))
            {
                GameObject.Find("Main Camera").GetComponent<UIController>().SetCurrentValues();
                GameObject.Find("Main Camera").GetComponent<UIController>().UnselectObject();
                Map.SelectedCity.Build(BuildingType.Walls, null);
            }
        }
    }

    public void OnBackClick()
    {
        transform.localScale /= 1.1f;
        GetComponent<TextMeshProUGUI>().font = _originalFont;
        GameObject.Find("Main Camera").GetComponent<MenuManager>().ShowMainMenu();
    }
    
    public void OnBackToMenuClick()
    {
        SceneManager.LoadScene("Menu");
    }

    public void OnTutorialClick()
    {
        GameLogic.Tutorial = true;
        GameLogic.LoadGame = false;
        SceneManager.LoadScene("Loading Screen");
    }

    public void CloseWindow()
    {
        transform.parent.gameObject.SetActive(false);
        GameLogic.Tutorial = false;
    }
    
    public void OnMenuButtonClick()
    {
        bool isActive = menu.activeSelf;
        if (GameLogic.Tutorial)
        {
            message.SetActive(isActive);
            message.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = Map.CurrentMessage;
        }
        menu.SetActive(!isActive);
        Map.MenuEnabled = !Map.MenuEnabled;
    }
    
    public void OnLoadClick()
    {
        if (ES3.FileExists())
        {
            Map.SelectedUnit = null;
            Map.SelectedCity = null;
            GameLogic.Tutorial = false;
            GameLogic.LoadGame = true;
            SceneManager.LoadScene("Loading Screen");
        }
    }
    
    public void OnSaveClick()
    {
        GameObject.Find("Map").GetComponent<Map>().SaveGame();
    }
}