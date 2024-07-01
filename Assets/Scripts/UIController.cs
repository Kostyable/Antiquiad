using System.Collections.Generic;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Image currentFlag;
    [SerializeField] private GameObject selectedObject;
    [SerializeField] private GameObject unitDesc;
    [SerializeField] private GameObject cityDesc;
    [SerializeField] private GameObject turnButtonArea;
    private TextMeshProUGUI _selectedName;
    private Image _selectedImage;
    private PanelManager _panel;
    [Space(10)]
    [Header("Food")]
    [Space(10)]
    [SerializeField] private TextMeshProUGUI foodValue;
    [SerializeField] private TextMeshProUGUI foodAdd;
    [Space(10)]
    [Header("Wood")]
    [Space(10)]
    [SerializeField] private TextMeshProUGUI woodValue;
    [SerializeField] private TextMeshProUGUI woodAdd;
    [Space(10)]
    [Header("Stone")]
    [Space(10)]
    [SerializeField] private TextMeshProUGUI stoneValue;
    [SerializeField] private TextMeshProUGUI stoneAdd;
    [Space(10)]
    [Header("Gold")]
    [Space(10)]
    [SerializeField] private TextMeshProUGUI goldValue;
    [SerializeField] private TextMeshProUGUI goldAdd;
    [Space(10)]
    [Header("Horses")]
    [Space(10)]
    [SerializeField] private TextMeshProUGUI horsesValue;
    [SerializeField] private TextMeshProUGUI horsesAdd;
    [Space(10)]
    [Header("Bronze")]
    [Space(10)]
    [SerializeField] private TextMeshProUGUI bronzeValue;
    [SerializeField] private TextMeshProUGUI bronzeAdd;
    [Space(10)]
    [Header("Iron")]
    [Space(10)]
    [SerializeField] private TextMeshProUGUI ironValue;
    [SerializeField] private TextMeshProUGUI ironAdd;
    [Space(10)]
    [Header("Civilizations")]
    [Space(10)]
    [SerializeField] private Sprite[] civilizationsFlags;
    [Space(10)]
    [Header("Units")]
    [Space(10)]
    [SerializeField] private Sprite[] unitsSprites;
    [Space(10)]
    [Header("Cities")]
    [Space(10)]
    [SerializeField] private Sprite[] civilizationsEmblems;
    
    private void Start()
    {
        GameObject.Find("Turn Value").GetComponent<LocalizationParamsManager>().SetParameterValue
            ("TURN VALUE", GameLogic.Turn.ToString());
        currentFlag.sprite = civilizationsFlags[(int)GameLogic.SelectedCiv];
        _selectedName = selectedObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        _selectedImage = selectedObject.transform.GetChild(1).gameObject.GetComponent<Image>();
        _panel = GameObject.Find("Panel").GetComponent<PanelManager>();
    }

    public void SetCurrentValues()
    {
        GameObject.Find("Turn Value").GetComponent<LocalizationParamsManager>().SetParameterValue
            ("TURN VALUE", GameLogic.Turn.ToString());
        foodValue.text = "" + GameLogic.Civs[0].ResourcesValues[ResourceType.Food];
        woodValue.text = "" + GameLogic.Civs[0].ResourcesValues[ResourceType.Wood];
        stoneValue.text = "" + GameLogic.Civs[0].ResourcesValues[ResourceType.Stone];
        goldValue.text = "" + GameLogic.Civs[0].ResourcesValues[ResourceType.Gold];
        horsesValue.text = "" + GameLogic.Civs[0].ResourcesValues[ResourceType.Horses];
        bronzeValue.text = "" + GameLogic.Civs[0].ResourcesValues[ResourceType.Bronze];
        ironValue.text = "" + GameLogic.Civs[0].ResourcesValues[ResourceType.Iron];
    }
    
    public void SetCurrentAdds()
    {
        foodAdd.text = "+" + GameLogic.Civs[0].ResourcesAdds[ResourceType.Food];
        woodAdd.text = "+" + GameLogic.Civs[0].ResourcesAdds[ResourceType.Wood];
        stoneAdd.text = "+" + GameLogic.Civs[0].ResourcesAdds[ResourceType.Stone];
        goldAdd.text = "+" + GameLogic.Civs[0].ResourcesAdds[ResourceType.Gold];
        horsesAdd.text = "+" + GameLogic.Civs[0].ResourcesAdds[ResourceType.Horses];
        bronzeAdd.text = "+" + GameLogic.Civs[0].ResourcesAdds[ResourceType.Bronze];
        ironAdd.text = "+" + GameLogic.Civs[0].ResourcesAdds[ResourceType.Iron];
    }

    public void SelectObject(GameObject go)
    {
        turnButtonArea.SetActive(true);
        _selectedImage.color = Color.white;
        _panel.Clear();
        if (go.CompareTag("Unit"))
        {
            if (go.GetComponent<Unit>().type == UnitType.Settler)
            {
                _selectedName.text = LocalizationManager.GetTranslation("settler");
                _selectedImage.sprite = unitsSprites[0];
                _panel.AddItems(23, 24);
            }
            else if (go.GetComponent<Unit>().type == UnitType.Scout)
            {
                _selectedName.text = LocalizationManager.GetTranslation("scout");
                _selectedImage.sprite = unitsSprites[1];
                _panel.AddItems(24);
            }
            else if (go.GetComponent<Unit>().type == UnitType.Warrior)
            {
                _selectedName.text = LocalizationManager.GetTranslation("warrior");
                _selectedImage.sprite = unitsSprites[2];
                _panel.AddItems(24);
            }
            else if (go.GetComponent<Unit>().type == UnitType.Slinger)
            {
                _selectedName.text = LocalizationManager.GetTranslation("slinger");
                _selectedImage.sprite = unitsSprites[3];
                _panel.AddItems(24);
            }
            else if (go.GetComponent<Unit>().type == UnitType.Archer)
            {
                _selectedName.text = LocalizationManager.GetTranslation("archer");
                _selectedImage.sprite = unitsSprites[4];
                _panel.AddItems(24);
            }
            else if (go.GetComponent<Unit>().type == UnitType.Chariot)
            {
                _selectedName.text = LocalizationManager.GetTranslation("chariot");
                _selectedImage.sprite = unitsSprites[5];
                _panel.AddItems(24);
            }
            else if (go.GetComponent<Unit>().type == UnitType.Spearman)
            {
                _selectedName.text = LocalizationManager.GetTranslation("spearman");
                _selectedImage.sprite = unitsSprites[6];
                _panel.AddItems(24);
            }
            else if (go.GetComponent<Unit>().type == UnitType.Hoplite)
            {
                _selectedName.text = LocalizationManager.GetTranslation("hoplite");
                _selectedImage.sprite = unitsSprites[7];
                _panel.AddItems(24);
            }
            else if (go.GetComponent<Unit>().type == UnitType.Horseman)
            {
                _selectedName.text = LocalizationManager.GetTranslation("horseman");
                _selectedImage.sprite = unitsSprites[8];
                _panel.AddItems(24);
            }
            else if (go.GetComponent<Unit>().type == UnitType.Swordsman)
            {
                _selectedName.text = LocalizationManager.GetTranslation("swordsman");
                _selectedImage.sprite = unitsSprites[9];
                _panel.AddItems(24);
            }
            else if (go.GetComponent<Unit>().type == UnitType.Catapult)
            {
                _selectedName.text = LocalizationManager.GetTranslation("catapult");
                _selectedImage.sprite = unitsSprites[10];
                _panel.AddItems(24);
            }
            else if (go.GetComponent<Unit>().type == UnitType.SiegeTower)
            {
                _selectedName.text = LocalizationManager.GetTranslation("siege_tower");
                _selectedImage.sprite = unitsSprites[11];
                _panel.AddItems(24);
            }
            cityDesc.SetActive(false);
            unitDesc.SetActive(true);
            foreach (Transform child in unitDesc.transform)
            {
                child.gameObject.SetActive(false);
            }
            for (int i = 0; i < 4; i++)
            {
                unitDesc.transform.GetChild(i).gameObject.SetActive(true);
            }
            unitDesc.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text
                = go.GetComponent<Unit>().currentMovePoints.ToString();
            unitDesc.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text
                = go.GetComponent<Unit>().sight.ToString();
            if (go.GetComponent<Unit>() is CombatUnit)
            {
                for (int i = 4; i < 10; i++)
                {
                    unitDesc.transform.GetChild(i).gameObject.SetActive(true);
                }
                unitDesc.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text
                    = go.GetComponent<CombatUnit>().healthPoints.ToString();
                unitDesc.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text
                    = go.GetComponent<CombatUnit>().attack.ToString();
                unitDesc.transform.GetChild(8).GetComponent<TextMeshProUGUI>().text
                    = go.GetComponent<CombatUnit>().defence.ToString();
                if (go.GetComponent<Unit>() is RangedUnit)
                {
                    unitDesc.transform.GetChild(10).gameObject.SetActive(true);
                    unitDesc.transform.GetChild(11).gameObject.SetActive(true);
                    unitDesc.transform.GetChild(10).GetComponent<TextMeshProUGUI>().text
                        = go.GetComponent<RangedUnit>().range.ToString();
                }
                else if (go.GetComponent<Unit>() is SiegeUnit)
                {
                    unitDesc.transform.GetChild(10).gameObject.SetActive(true);
                    unitDesc.transform.GetChild(11).gameObject.SetActive(true);
                    unitDesc.transform.GetChild(10).GetComponent<TextMeshProUGUI>().text
                        = go.GetComponent<SiegeUnit>().range.ToString();
                }
            }
        }
        if (go.CompareTag("City"))
        {
            _selectedName.text = go.GetComponent<City>().cityName;
            _selectedImage.sprite = civilizationsEmblems[(int)GameLogic.SelectedCiv];
            unitDesc.SetActive(false);
            cityDesc.SetActive(true);
            Unit unit = go.GetComponent<City>().coreCell.unit;
            foreach (Transform child in cityDesc.transform)
            {
                child.gameObject.SetActive(false);
            }
            for (int i = 0; i < 4; i++)
            {
                cityDesc.transform.GetChild(i).gameObject.SetActive(true);
            }
            cityDesc.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text
                = go.GetComponent<City>().healthPoints.ToString();
            cityDesc.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text
                = go.GetComponent<City>().defence.ToString();
            if (unit != null && unit is CombatUnit)
            {
                for (int i = 4; i < 10; i++)
                {
                    cityDesc.transform.GetChild(i).gameObject.SetActive(true);
                }
                cityDesc.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text
                    = unit.GetComponent<CombatUnit>().currentMovePoints.ToString();
                cityDesc.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text
                    = unit.GetComponent<CombatUnit>().sight.ToString();
                cityDesc.transform.GetChild(8).GetComponent<TextMeshProUGUI>().text
                    = unit.GetComponent<CombatUnit>().attack.ToString();
                if (unit is RangedUnit)
                {
                    cityDesc.transform.GetChild(10).gameObject.SetActive(true);
                    cityDesc.transform.GetChild(11).gameObject.SetActive(true);
                    cityDesc.transform.GetChild(10).GetComponent<TextMeshProUGUI>().text
                        = unit.GetComponent<RangedUnit>().range.ToString();
                }
                else if (unit is SiegeUnit)
                {
                    cityDesc.transform.GetChild(10).gameObject.SetActive(true);
                    cityDesc.transform.GetChild(11).gameObject.SetActive(true);
                    cityDesc.transform.GetChild(10).GetComponent<TextMeshProUGUI>().text
                        = unit.GetComponent<SiegeUnit>().range.ToString();
                }
            }
            switch (GameLogic.SelectedCiv)
            {
                case CivilizationName.Egypt:
                    _panel.AddItems(0, 1, 2, 3, 9, 5, 6, 7, 8);
                    break;
                case CivilizationName.Greece:
                    _panel.AddItems(0, 1, 2, 3, 4, 10, 6, 7, 8);
                    break;
                case CivilizationName.Mesopotamia:
                    _panel.AddItems(0, 1, 2, 3, 4, 5, 6, 7, 11);
                    break;
            }
            List<int> buildingIndexes = new List<int>
            {
                12, 13, 14, 15, 16, 17, 18, 19, 20, 21
            };
            foreach (BuildingType building in Map.SelectedCity.Buildings)
            {
                buildingIndexes.Remove((int)building + 12);
            }
            foreach (int index in buildingIndexes)
            {
                _panel.AddItems(index);
            }
        }
    }

    public void UpdateUnitInfo(Unit unit)
    {
        unitDesc.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = unit.currentMovePoints.ToString();
        if (unit is CombatUnit)
        {
            unitDesc.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text
                = ((CombatUnit)unit).healthPoints.ToString();
        }
    }
    
    public void UpdateCityInfo(City city)
    {
        cityDesc.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = city.currentHealthPoints.ToString();
    }

    public void UnselectObject()
    {
        if (turnButtonArea != null)
        {
            turnButtonArea.SetActive(false);
        }
        if (_selectedImage != null)
        {
            _selectedImage.color = Color.clear;
        }
        if (_selectedName != null)
        {
            _selectedName.text = "";
        }
        if (unitDesc != null)
        {
            unitDesc.SetActive(false);
        }
        if (cityDesc != null)
        {
            cityDesc.SetActive(false);
        }
        if (_panel != null)
        {
            _panel.Clear();
        }
    }
}