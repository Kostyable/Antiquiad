using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Map : MonoBehaviour
{
    [SerializeField] private Generator generator;
    [SerializeField] private UIController uiController;
    [SerializeField] private GameObject phantomCellPrefab;
    [SerializeField] private GameObject cityPrefab;
    public GameObject cellPrefab;
    public GameObject riverPrefab;
    public Vector2Int size;
    public Cell[,] Cells;
    [NonSerialized] public List<River> Rivers;
    [NonSerialized] public Vector2[] CellsOffsets;
    [NonSerialized] public Vector2[] RiversOffsets;
    private Vector2Int _origin;
    private Vector2 _center;
    public static float MinX;
    public static float MaxX;
    public static float MinY;
    public static float MaxY;
    public static string CurrentMessage;
    public static bool MenuEnabled;
    private GameObject _clickedObject;
    private GameObject _focusedObject;
    [NonSerialized] public static Unit SelectedUnit;
    [NonSerialized] public static City SelectedCity;
    private Cell _goalCell;
    private float _longPressDuration;
    private float _pressStartTime;
    private Vector2 _touchPosition;
    private float _touchDelta;
    [Space(10)]
    [Header("Terrain Costs")]
    [Space(10)]
    public int waterCost;
    public int desertCost;
    public int floodplainCost;
    public int plainCost;
    public int forestCost;
    public int desertHillsCost;
    public int hillsCost;
    public int mountainsCost;
    public int riverCost;
    [Space(10)]
    [Header("Resources")]
    [Space(10)]
    public GameObject[] resourcesPrefabs;
    [Space(10)]
    [Header("Units")]
    [Space(10)]
    public GameObject[] egyptUnitsPrefabs;
    public GameObject[] greeceUnitsPrefabs;
    public GameObject[] mesopotamiaUnitsPrefabs;
    [Space(10)]
    [Header("Units Costs")]
    [Space(10)]
    [Header("Settler")]
    public int settlerFood;
    public int settlerWood;
    public int settlerStone;
    public int settlerGold;
    public int settlerHorses;
    public int settlerBronze;
    public int settlerIron;
    [Header("Scout")]
    public int scoutFood;
    public int scoutWood;
    public int scoutStone;
    public int scoutGold;
    public int scoutHorses;
    public int scoutBronze;
    public int scoutIron;
    [Header("Warrior")]
    public int warriorFood;
    public int warriorWood;
    public int warriorStone;
    public int warriorGold;
    public int warriorHorses;
    public int warriorBronze;
    public int warriorIron;
    [Header("Slinger")]
    public int slingerFood;
    public int slingerWood;
    public int slingerStone;
    public int slingerGold;
    public int slingerHorses;
    public int slingerBronze;
    public int slingerIron;
    [Header("Archer")]
    public int archerFood;
    public int archerWood;
    public int archerStone;
    public int archerGold;
    public int archerHorses;
    public int archerBronze;
    public int archerIron;
    [Header("Chariot")]
    public int chariotFood;
    public int chariotWood;
    public int chariotStone;
    public int chariotGold;
    public int chariotHorses;
    public int chariotBronze;
    public int chariotIron;
    [Header("Spearman")]
    public int spearmanFood;
    public int spearmanWood;
    public int spearmanStone;
    public int spearmanGold;
    public int spearmanHorses;
    public int spearmanBronze;
    public int spearmanIron;
    [Header("Hoplite")]
    public int hopliteFood;
    public int hopliteWood;
    public int hopliteStone;
    public int hopliteGold;
    public int hopliteHorses;
    public int hopliteBronze;
    public int hopliteIron;
    [Header("Horseman")]
    public int horsemanFood;
    public int horsemanWood;
    public int horsemanStone;
    public int horsemanGold;
    public int horsemanHorses;
    public int horsemanBronze;
    public int horsemanIron;
    [Header("Swordsman")]
    public int swordsmanFood;
    public int swordsmanWood;
    public int swordsmanStone;
    public int swordsmanGold;
    public int swordsmanHorses;
    public int swordsmanBronze;
    public int swordsmanIron;
    [Header("Catapult")]
    public int catapultFood;
    public int catapultWood;
    public int catapultStone;
    public int catapultGold;
    public int catapultHorses;
    public int catapultBronze;
    public int catapultIron;
    [Header("Siege Tower")]
    public int siegeTowerFood;
    public int siegeTowerWood;
    public int siegeTowerStone;
    public int siegeTowerGold;
    public int siegeTowerHorses;
    public int siegeTowerBronze;
    public int siegeTowerIron;
    [Space(10)]
    [Header("Buildings Costs")]
    [Space(10)]
    [Header("Farm")]
    public int farmFood;
    public int farmWood;
    public int farmStone;
    public int farmGold;
    public int farmHorses;
    public int farmBronze;
    public int farmIron;
    [Header("Pasture")]
    public int pastureFood;
    public int pastureWood;
    public int pastureStone;
    public int pastureGold;
    public int pastureHorses;
    public int pastureBronze;
    public int pastureIron;
    [Header("Fishing Boat")]
    public int fishingBoatFood;
    public int fishingBoatWood;
    public int fishingBoatStone;
    public int fishingBoatGold;
    public int fishingBoatHorses;
    public int fishingBoatBronze;
    public int fishingBoatIron;
    [Header("Lumber Mill")]
    public int lumberMillFood;
    public int lumberMillWood;
    public int lumberMillStone;
    public int lumberMillGold;
    public int lumberMillHorses;
    public int lumberMillBronze;
    public int lumberMillIron;
    [Header("Quarry")]
    public int quarryFood;
    public int quarryWood;
    public int quarryStone;
    public int quarryGold;
    public int quarryHorses;
    public int quarryBronze;
    public int quarryIron;
    [Header("Gold Mine")]
    public int goldMineFood;
    public int goldMineWood;
    public int goldMineStone;
    public int goldMineGold;
    public int goldMineHorses;
    public int goldMineBronze;
    public int goldMineIron;
    [Header("Stable")]
    public int stableFood;
    public int stableWood;
    public int stableStone;
    public int stableGold;
    public int stableHorses;
    public int stableBronze;
    public int stableIron;
    [Header("Mine")]
    public int mineFood;
    public int mineWood;
    public int mineStone;
    public int mineGold;
    public int mineHorses;
    public int mineBronze;
    public int mineIron;
    [Header("Blacksmith")]
    public int blacksmithFood;
    public int blacksmithWood;
    public int blacksmithStone;
    public int blacksmithGold;
    public int blacksmithHorses;
    public int blacksmithBronze;
    public int blacksmithIron;
    [Header("Walls")]
    public int wallsFood;
    public int wallsWood;
    public int wallsStone;
    public int wallsGold;
    public int wallsHorses;
    public int wallsBronze;
    public int wallsIron;
    [Space(10)]
    [Header("Cities")]
    [Space(10)]
    public int cityExpansionRadius;
    public int expansionFoodCount;
    [Space(10)]
    [Header("Modifiers")]
    public float ordinaryResourcesModifier;
    public float strategicResourcesModifier;
    public float capitalModifier;
    public float riverModifier;

    private void Awake()
    {
        if (GameLogic.LoadGame)
        {
            size = ES3.Load<Vector2Int>("size");
        }
        Cells = new Cell[size.y, size.x];
        Rivers = new List<River>();
        CellsOffsets = new Vector2[3];
        CellsOffsets[0] = new Vector2(4.56f, 1.31f);
        CellsOffsets[1] = new Vector2(1.32f, 0.35f);
        CellsOffsets[2] = new Vector2(3.25f, 0.35f);
        RiversOffsets = new Vector2[9];
        RiversOffsets[0] = new Vector2(0.61f, 0.45f);
        RiversOffsets[1] = new Vector2(0.85f, -0.35f);
        RiversOffsets[2] = new Vector2(0.81f, -0.09f);
        RiversOffsets[3] = new Vector2(0.27f, -0.65f);
        RiversOffsets[4] = new Vector2(0.08f, -0.75f);
        RiversOffsets[5] = new Vector2(0.84f, -0.74f);
        RiversOffsets[6] = new Vector2(-0.74f, -0.55f);
        RiversOffsets[7] = new Vector2(0.03f, -0.55f);
        RiversOffsets[8] = new Vector2(-0.5f, -0.66f);
        _origin.y = size.y / 2;
        _origin.x = size.x / 2 - size.y / 4;
        _center = GetCellCoordinates(_origin.x + (_origin.x - 1) / 2, _origin.y);
        gameObject.GetComponent<Transform>().position -= (Vector3)_center;
        Vector2 minCoordinates = GetCellCoordinates(0, 0);
        Vector2 maxCoordinates = GetCellCoordinates(size.x - 1, size.y - 1);
        MinX = minCoordinates.x - 1.5f;
        MaxY = minCoordinates.y + 1.5f;
        MaxX = maxCoordinates.x + 1.75f;
        MinY = maxCoordinates.y - 1.85f;
        MenuEnabled = false;
        _longPressDuration = 0.25f;
        _touchDelta = 50f;
        CreatePhantomCells();
        if (GameLogic.LoadGame)
        {
            gameObject.GetComponent<SaveLoader>().LoadGame();
            GameLogic.Civs[0].CreateFogOfWar();
            uiController.SetCurrentValues();
        }
        else
        {
            Init();
        }
    }

    public void Start()
    {
        if (_focusedObject != null)
        {
            Vector3 position = _focusedObject.GetComponent<Transform>().position;
            if (Camera.main != null)
                Camera.main.GetComponent<Transform>().position = new Vector3(position.x, position.y, -10);
        }
        if (GameLogic.Tutorial)
        {
            GameObject.Find("Message").GetComponent<TutorialTrigger>().TriggerDialogue();
        }
        else
        {
            GameObject.Find("Message").SetActive(false);
        }
    }
    
    private void Update()
    {
        if (MenuEnabled)
        {
            return;
        }
        foreach (Civilization civ in GameLogic.Civs)
        {
            foreach (City city in civ.Cities)
            {
                city.SetCityUnit();
            }
            if (civ.Name != GameLogic.SelectedCiv)
            {
                foreach (Cell borderCell in civ.Borders)
                {
                    if (GameLogic.Civs[0].ExploredCells.Contains(borderCell))
                    {
                        foreach (GameObject border in borderCell.CityBorders)
                        {
                            border.SetActive(true);
                        }
                    }
                    else
                    {
                        foreach (GameObject border in borderCell.CityBorders)
                        {
                            border.SetActive(false);
                        }
                    }
                }
            }
        }
        if (SelectedUnit == null && SelectedCity == null)
        {
            uiController.UnselectObject();
        }
        if (SelectedUnit != null)
        {
            uiController.UpdateUnitInfo(SelectedUnit);
        }
        if (!IsMouseOverUI())
        {
            if (Application.platform == RuntimePlatform.WindowsPlayer
                || Application.platform == RuntimePlatform.WindowsEditor)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _clickedObject = GetObjectUnderMouse();
                    if (_clickedObject != null)
                    {
                        if (_clickedObject.CompareTag("Unit")
                            && _clickedObject.GetComponent<Unit>().Owner.Name == GameLogic.SelectedCiv)
                        {
                            SelectedCity = null;
                            if (SelectedUnit != null)
                            {
                                SelectedUnit.DeleteBorders();
                            }
                            SelectedUnit = _clickedObject.GetComponent<Unit>();
                            SelectedUnit.CreateBorders();
                            uiController.SelectObject(_clickedObject);
                        }
                        else if (_clickedObject.CompareTag("City")
                                 && _clickedObject.GetComponent<City>().Owner.Name == GameLogic.SelectedCiv)
                        {
                            if (SelectedUnit != null)
                            {
                                SelectedUnit.DeleteBorders();
                                SelectedUnit = null;
                            }
                            SelectedCity = _clickedObject.GetComponent<City>();
                            uiController.SelectObject(_clickedObject);
                            if (SelectedCity.coreCell.unit != null)
                            {
                                SelectedUnit = SelectedCity.coreCell.unit;
                                SelectedUnit.CreateBorders();
                            }
                        }
                        _clickedObject = null;
                    }
                }
                if (Input.GetMouseButtonDown(1))
                {
                    _clickedObject = GetObjectUnderMouse();
                    if (_clickedObject != null)
                    {
                        if (_clickedObject.CompareTag("Cell")
                            && SelectedUnit != null
                            && SelectedUnit.Owner.ExploredCells.Contains(_clickedObject.GetComponent<Cell>()))
                        {
                            _goalCell = _clickedObject.GetComponent<Cell>();
                            SelectedUnit.rememberedCell = _goalCell;
                            SelectedUnit.Move(_goalCell);
                            if (SelectedUnit.currentCell.city != null)
                            {
                                SelectedCity = SelectedUnit.currentCell.city;
                                uiController.SelectObject(SelectedCity.gameObject);
                            }
                            else
                            {
                                uiController.SelectObject(SelectedUnit.gameObject);
                            }
                        }
                        if (_clickedObject != null
                            && _clickedObject.CompareTag("Unit")
                            && _clickedObject.GetComponent<Unit>().Owner.Name != GameLogic.SelectedCiv
                            && SelectedUnit != null && SelectedUnit is CombatUnit)
                        {
                            if (!((CombatUnit)SelectedUnit).Attack(_clickedObject.GetComponent<Unit>()))
                            {
                                _goalCell = _clickedObject.GetComponent<Unit>().currentCell;
                                SelectedUnit.Move(_goalCell);
                                if (SelectedUnit.currentCell.city != null)
                                {
                                    SelectedCity = SelectedUnit.currentCell.city;
                                    uiController.SelectObject(SelectedCity.gameObject);
                                }
                                else
                                {
                                    uiController.SelectObject(SelectedUnit.gameObject);
                                }
                                ((CombatUnit)SelectedUnit).Attack(_clickedObject.GetComponent<Unit>());
                            }
                        }
                        if (_clickedObject != null
                            && _clickedObject.CompareTag("City")
                            && _clickedObject.GetComponent<City>().Owner.Name != GameLogic.SelectedCiv
                            && SelectedUnit != null && SelectedUnit is CombatUnit)
                        {
                            if (!((CombatUnit)SelectedUnit).Attack(_clickedObject.GetComponent<City>()))
                            {
                                _goalCell = _clickedObject.GetComponent<City>().coreCell;
                                SelectedUnit.Move(_goalCell);
                                if (SelectedUnit.currentCell.city != null)
                                {
                                    SelectedCity = SelectedUnit.currentCell.city;
                                    uiController.SelectObject(SelectedCity.gameObject);
                                }
                                else
                                {
                                    uiController.SelectObject(SelectedUnit.gameObject);
                                }
                                ((CombatUnit)SelectedUnit).Attack(_clickedObject.GetComponent<City>());
                            }
                        }
                        _clickedObject = null;
                    }
                }
            }

            else if (Application.platform == RuntimePlatform.Android)
            {
                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    _clickedObject = GetObjectUnderTouch();
                    if (_clickedObject != null)
                    {
                        if (_clickedObject.CompareTag("Unit")
                            && _clickedObject.GetComponent<Unit>().Owner.Name == GameLogic.SelectedCiv)
                        {
                            SelectedCity = null;
                            if (SelectedUnit != null)
                            {
                                SelectedUnit.DeleteBorders();
                            }
                            SelectedUnit = _clickedObject.GetComponent<Unit>();
                            SelectedUnit.CreateBorders();
                            uiController.SelectObject(_clickedObject);
                        }
                        else if (_clickedObject.CompareTag("City")
                                 && _clickedObject.GetComponent<City>().Owner.Name == GameLogic.SelectedCiv)
                        {
                            if (SelectedUnit != null)
                            {
                                SelectedUnit.DeleteBorders();
                                SelectedUnit = null;
                            }
                            SelectedCity = _clickedObject.GetComponent<City>();
                            uiController.SelectObject(_clickedObject);
                            if (SelectedCity.coreCell.unit != null)
                            {
                                SelectedUnit = SelectedCity.coreCell.unit;
                                SelectedUnit.CreateBorders();
                            }
                        }
                        _clickedObject = null;
                    }
                }
                if (Input.touchCount > 0 && SelectedUnit != null)
                {
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Began)
                    {
                        _pressStartTime = Time.time;
                        _touchPosition = touch.position;
                    }
                    if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                    {
                        _pressStartTime = 0f;
                    }
                    if (touch.phase == TouchPhase.Stationary)
                    {
                        float pressDuration = Time.time - _pressStartTime;

                        if (pressDuration >= _longPressDuration &&
                            Vector2.Distance(_touchPosition, touch.position) <= _touchDelta)
                        {
                            _pressStartTime = Time.time;
                            _clickedObject = GetObjectUnderTouch();
                            if (_clickedObject != null)
                            {
                                if (_clickedObject.CompareTag("Cell")
                                    && SelectedUnit != null
                                    && SelectedUnit.Owner.ExploredCells.Contains(_clickedObject.GetComponent<Cell>()))
                                {
                                    _goalCell = _clickedObject.GetComponent<Cell>();
                                    SelectedUnit.rememberedCell = _goalCell;
                                    SelectedUnit.Move(_goalCell);
                                    if (SelectedUnit.currentCell.city != null)
                                    {
                                        SelectedCity = SelectedUnit.currentCell.city;
                                        uiController.SelectObject(SelectedCity.gameObject);
                                    }
                                    else
                                    {
                                        uiController.SelectObject(SelectedUnit.gameObject);
                                    }
                                }
                                if (_clickedObject != null
                                    && _clickedObject.CompareTag("Unit")
                                    && _clickedObject.GetComponent<Unit>().Owner.Name != GameLogic.SelectedCiv
                                    && SelectedUnit != null && SelectedUnit is CombatUnit)
                                {
                                    if (!((CombatUnit)SelectedUnit).Attack(_clickedObject.GetComponent<Unit>()))
                                    {
                                        _goalCell = _clickedObject.GetComponent<Unit>().currentCell;
                                        SelectedUnit.Move(_goalCell);
                                        if (SelectedUnit.currentCell.city != null)
                                        {
                                            SelectedCity = SelectedUnit.currentCell.city;
                                            uiController.SelectObject(SelectedCity.gameObject);
                                        }
                                        else
                                        {
                                            uiController.SelectObject(SelectedUnit.gameObject);
                                        }
                                        ((CombatUnit)SelectedUnit).Attack(_clickedObject.GetComponent<Unit>());
                                    }
                                }
                                if (_clickedObject != null
                                    && _clickedObject.CompareTag("City")
                                    && _clickedObject.GetComponent<City>().Owner.Name != GameLogic.SelectedCiv
                                    && SelectedUnit != null && SelectedUnit is CombatUnit)
                                {
                                    if (!((CombatUnit)SelectedUnit).Attack(_clickedObject.GetComponent<City>()))
                                    {
                                        _goalCell = _clickedObject.GetComponent<City>().coreCell;
                                        SelectedUnit.Move(_goalCell);
                                        if (SelectedUnit.currentCell.city != null)
                                        {
                                            SelectedCity = SelectedUnit.currentCell.city;
                                            uiController.SelectObject(SelectedCity.gameObject);
                                        }
                                        else
                                        {
                                            uiController.SelectObject(SelectedUnit.gameObject);
                                        }
                                        ((CombatUnit)SelectedUnit).Attack(_clickedObject.GetComponent<City>());
                                    }
                                }
                                _clickedObject = null;
                            }
                        }
                    }
                }
            }
        }
    }

    public void Init()
    {
        for (int i = 0; i < size.y; i++)
        {
            for (int j = 0; j < size.x; j++)
            {
                GameObject cell = Instantiate(cellPrefab, GetCellCoordinates(j, i),
                    GetCellAngle(j, i), transform);
                cell.GetComponent<Cell>().Init(j, i, GetCellType(j, i));
                Cells[i, j] = cell.GetComponent<Cell>();
            }
        }
        for (int i = 0; i < size.y; i++)
        {
            for (int j = 0; j < size.x; j++)
            {
                SetCellNeighbors(Cells[i, j]);
            }
        }
        GameLogic.Civs = new Civilization[Enum.GetValues(typeof(CivilizationName)).Length];
        GameLogic.AIs = new AI[GameLogic.Civs.Length - 1];
        GameLogic.Turn = 1;
        GameLogic.WaterCost = waterCost;
        GameLogic.PlainCost = plainCost;
        GameLogic.DesertCost = desertCost;
        GameLogic.FloodplainCost = floodplainCost;
        GameLogic.ForestCost = forestCost;
        GameLogic.HillsCost = hillsCost;
        GameLogic.DesertHillsCost = desertHillsCost;
        GameLogic.MountainsCost = mountainsCost;
        GameLogic.RiverCost = riverCost;
        GameLogic.UnitsCosts = new Dictionary<UnitType, Dictionary<ResourceType, int>>
        {
            { 
                UnitType.Settler, new Dictionary<ResourceType, int>
                {
                    { ResourceType.Food, settlerFood },
                    { ResourceType.Wood, settlerWood },
                    { ResourceType.Stone, settlerStone },
                    { ResourceType.Gold, settlerGold },
                    { ResourceType.Horses, settlerHorses },
                    { ResourceType.Bronze, settlerBronze },
                    { ResourceType.Iron, settlerIron }
                }
            },
            {
                UnitType.Scout, new Dictionary<ResourceType, int>
                {
                    { ResourceType.Food, scoutFood },
                    { ResourceType.Wood, scoutWood },
                    { ResourceType.Stone, scoutStone },
                    { ResourceType.Gold, scoutGold },
                    { ResourceType.Horses, scoutHorses },
                    { ResourceType.Bronze, scoutBronze },
                    { ResourceType.Iron, scoutIron }
                }
            },
            {
                UnitType.Warrior, new Dictionary<ResourceType, int>
                {
                    { ResourceType.Food, warriorFood },
                    { ResourceType.Wood, warriorWood },
                    { ResourceType.Stone, warriorStone },
                    { ResourceType.Gold, warriorGold },
                    { ResourceType.Horses, warriorHorses },
                    { ResourceType.Bronze, warriorBronze },
                    { ResourceType.Iron, warriorIron }
                }
            },
            {
                UnitType.Slinger, new Dictionary<ResourceType, int>
                {
                    { ResourceType.Food, slingerFood },
                    { ResourceType.Wood, slingerWood },
                    { ResourceType.Stone, slingerStone },
                    { ResourceType.Gold, slingerGold },
                    { ResourceType.Horses, slingerHorses },
                    { ResourceType.Bronze, slingerBronze },
                    { ResourceType.Iron, slingerIron }
                }
            },
            {
                UnitType.Archer, new Dictionary<ResourceType, int>
                {
                    { ResourceType.Food, archerFood },
                    { ResourceType.Wood, archerWood },
                    { ResourceType.Stone, archerStone },
                    { ResourceType.Gold, archerGold },
                    { ResourceType.Horses, archerHorses },
                    { ResourceType.Bronze, archerBronze },
                    { ResourceType.Iron, archerIron }
                }
            },
            {
                UnitType.Chariot, new Dictionary<ResourceType, int>
                {
                    { ResourceType.Food, chariotFood },
                    { ResourceType.Wood, chariotWood },
                    { ResourceType.Stone, chariotStone },
                    { ResourceType.Gold, chariotGold },
                    { ResourceType.Horses, chariotHorses },
                    { ResourceType.Bronze, chariotBronze },
                    { ResourceType.Iron, chariotIron }
                }
            },
            {
                UnitType.Spearman, new Dictionary<ResourceType, int>
                {
                    { ResourceType.Food, spearmanFood },
                    { ResourceType.Wood, spearmanWood },
                    { ResourceType.Stone, spearmanStone },
                    { ResourceType.Gold, spearmanGold },
                    { ResourceType.Horses, spearmanHorses },
                    { ResourceType.Bronze, spearmanBronze },
                    { ResourceType.Iron, spearmanIron }
                }
            },
            {
                UnitType.Hoplite, new Dictionary<ResourceType, int>
                {
                    { ResourceType.Food, hopliteFood },
                    { ResourceType.Wood, hopliteWood },
                    { ResourceType.Stone, hopliteStone },
                    { ResourceType.Gold, hopliteGold },
                    { ResourceType.Horses, hopliteHorses },
                    { ResourceType.Bronze, hopliteBronze },
                    { ResourceType.Iron, hopliteIron }
                }
            },
            {
                UnitType.Horseman, new Dictionary<ResourceType, int>
                {
                    { ResourceType.Food, horsemanFood },
                    { ResourceType.Wood, horsemanWood },
                    { ResourceType.Stone, horsemanStone },
                    { ResourceType.Gold, horsemanGold },
                    { ResourceType.Horses, horsemanHorses },
                    { ResourceType.Bronze, horsemanBronze },
                    { ResourceType.Iron, horsemanIron }
                }
            },
            {
                UnitType.Swordsman, new Dictionary<ResourceType, int>
                {
                    { ResourceType.Food, swordsmanFood },
                    { ResourceType.Wood, swordsmanWood },
                    { ResourceType.Stone, swordsmanStone },
                    { ResourceType.Gold, swordsmanGold },
                    { ResourceType.Horses, swordsmanHorses },
                    { ResourceType.Bronze, swordsmanBronze },
                    { ResourceType.Iron, swordsmanIron }
                }
            },
            {
                UnitType.Catapult, new Dictionary<ResourceType, int>
                {
                    { ResourceType.Food, catapultFood },
                    { ResourceType.Wood, catapultWood },
                    { ResourceType.Stone, catapultStone },
                    { ResourceType.Gold, catapultGold },
                    { ResourceType.Horses, catapultHorses },
                    { ResourceType.Bronze, catapultBronze },
                    { ResourceType.Iron, catapultIron }
                }
            },
            {
                UnitType.SiegeTower, new Dictionary<ResourceType, int>
                {
                    { ResourceType.Food, siegeTowerFood },
                    { ResourceType.Wood, siegeTowerWood },
                    { ResourceType.Stone, siegeTowerStone },
                    { ResourceType.Gold, siegeTowerGold },
                    { ResourceType.Horses, siegeTowerHorses },
                    { ResourceType.Bronze, siegeTowerBronze },
                    { ResourceType.Iron, siegeTowerIron }
                }
            }
        };
        GameLogic.BuildingsCosts = new Dictionary<BuildingType, Dictionary<ResourceType, int>>
        {
            { 
                BuildingType.Farm, new Dictionary<ResourceType, int>
                {
                    { ResourceType.Food, farmFood },
                    { ResourceType.Wood, farmWood },
                    { ResourceType.Stone, farmStone },
                    { ResourceType.Gold, farmGold },
                    { ResourceType.Horses, farmHorses },
                    { ResourceType.Bronze, farmBronze },
                    { ResourceType.Iron, farmIron }
                }
            },
            {
                BuildingType.Pasture, new Dictionary<ResourceType, int>
                {
                    { ResourceType.Food, pastureFood },
                    { ResourceType.Wood, pastureWood },
                    { ResourceType.Stone, pastureStone },
                    { ResourceType.Gold, pastureGold },
                    { ResourceType.Horses, pastureHorses },
                    { ResourceType.Bronze, pastureBronze },
                    { ResourceType.Iron, pastureIron }
                }
            },
            {
                BuildingType.FishingBoat, new Dictionary<ResourceType, int>
                {
                    { ResourceType.Food, fishingBoatFood },
                    { ResourceType.Wood, fishingBoatWood },
                    { ResourceType.Stone, fishingBoatStone },
                    { ResourceType.Gold, fishingBoatGold },
                    { ResourceType.Horses, fishingBoatHorses },
                    { ResourceType.Bronze, fishingBoatBronze },
                    { ResourceType.Iron, fishingBoatIron }
                }
            },
            {
                BuildingType.LumberMill, new Dictionary<ResourceType, int>
                {
                    { ResourceType.Food, lumberMillFood },
                    { ResourceType.Wood, lumberMillWood },
                    { ResourceType.Stone, lumberMillStone },
                    { ResourceType.Gold, lumberMillGold },
                    { ResourceType.Horses, lumberMillHorses },
                    { ResourceType.Bronze, lumberMillBronze },
                    { ResourceType.Iron, lumberMillIron }
                }
            },
            {
                BuildingType.Quarry, new Dictionary<ResourceType, int>
                {
                    { ResourceType.Food, quarryFood },
                    { ResourceType.Wood, quarryWood },
                    { ResourceType.Stone, quarryStone },
                    { ResourceType.Gold, quarryGold },
                    { ResourceType.Horses, quarryHorses },
                    { ResourceType.Bronze, quarryBronze },
                    { ResourceType.Iron, quarryIron }
                }
            },
            {
                BuildingType.GoldMine, new Dictionary<ResourceType, int>
                {
                    { ResourceType.Food, goldMineFood },
                    { ResourceType.Wood, goldMineWood },
                    { ResourceType.Stone, goldMineStone },
                    { ResourceType.Gold, goldMineGold },
                    { ResourceType.Horses, goldMineHorses },
                    { ResourceType.Bronze, goldMineBronze },
                    { ResourceType.Iron, goldMineIron }
                }
            },
            {
                BuildingType.Stable, new Dictionary<ResourceType, int>
                {
                    { ResourceType.Food, stableFood },
                    { ResourceType.Wood, stableWood },
                    { ResourceType.Stone, stableStone },
                    { ResourceType.Gold, stableGold },
                    { ResourceType.Horses, stableHorses },
                    { ResourceType.Bronze, stableBronze },
                    { ResourceType.Iron, stableIron }
                }
            },
            {
                BuildingType.Mine, new Dictionary<ResourceType, int>
                {
                    { ResourceType.Food, mineFood },
                    { ResourceType.Wood, mineWood },
                    { ResourceType.Stone, mineStone },
                    { ResourceType.Gold, mineGold },
                    { ResourceType.Horses, mineHorses },
                    { ResourceType.Bronze, mineBronze },
                    { ResourceType.Iron, mineIron }
                }
            },
            {
                BuildingType.Blacksmith, new Dictionary<ResourceType, int>
                {
                    { ResourceType.Food, blacksmithFood },
                    { ResourceType.Wood, blacksmithWood },
                    { ResourceType.Stone, blacksmithStone },
                    { ResourceType.Gold, blacksmithGold },
                    { ResourceType.Horses, blacksmithHorses },
                    { ResourceType.Bronze, blacksmithBronze },
                    { ResourceType.Iron, blacksmithIron }
                }
            },
            {
                BuildingType.Walls, new Dictionary<ResourceType, int>
                {
                    { ResourceType.Food, wallsFood },
                    { ResourceType.Wood, wallsWood },
                    { ResourceType.Stone, wallsStone },
                    { ResourceType.Gold, wallsGold },
                    { ResourceType.Horses, wallsHorses },
                    { ResourceType.Bronze, wallsBronze },
                    { ResourceType.Iron, wallsIron }
                }
            }
        };
        GameLogic.CityExpansionRadius = cityExpansionRadius;
        GameLogic.ExpansionFoodCount = expansionFoodCount;
        GameLogic.OrdinaryResourcesModifier = ordinaryResourcesModifier;
        GameLogic.StrategicResourcesModifier = strategicResourcesModifier;
        GameLogic.CapitalModifier = capitalModifier;
        GameLogic.RiverModifier = riverModifier;
        generator.GenerateTerrain();
        generator.GenerateRivers();
        generator.GenerateResources();
        generator.GenerateCivilizations();
        _focusedObject = GameLogic.Civs[0].Units[0].gameObject;
    }
    
    public bool IsMouseOverUI()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    public GameObject GetObjectUnderMouse()
    {
        if (Camera.main != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit.collider != null)
            {
                GameObject clickedObject = hit.collider.gameObject;
                return clickedObject;
            }
        }
        return null;
    }
    
    public GameObject GetObjectUnderTouch()
    {
        if (Input.touchCount > 0 && Camera.main != null)
        {
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            Collider2D pointCollider = Physics2D.OverlapPoint(touchPosition);
            if (pointCollider != null)
            {
                GameObject touchedObject = pointCollider.gameObject;
                return touchedObject;
            }
        }
        return null;
    }
    
    private void CreatePhantomCells()
    {
        GameObject phantomCell;
        for (int i = -1; i < size.y + 1; i++)
        {
            if (i % 2 == 0)
            {
                phantomCell = Instantiate(phantomCellPrefab, GetCellCoordinates(-5, i),
                    GetCellAngle(-5, i), transform);
                phantomCell.GetComponent<SpriteRenderer>().color = new Color(0.75f, 0.75f, 0.75f);
            }
            else
            {
                phantomCell = Instantiate(phantomCellPrefab, GetCellCoordinates(-1, i),
                    GetCellAngle(-1, i), transform);
                phantomCell.GetComponent<SpriteRenderer>().color = new Color(0.75f, 0.75f, 0.75f);
            }
            phantomCell = Instantiate(phantomCellPrefab, GetCellCoordinates(size.x, i),
                GetCellAngle(size.x, i), transform);
            phantomCell.GetComponent<SpriteRenderer>().color = new Color(0.75f, 0.75f, 0.75f);
        }
        for (int j = 0; j < size.x; j++)
        {
            phantomCell = Instantiate(phantomCellPrefab, GetCellCoordinates(j, -1),
                GetCellAngle(j, -1), transform);
            phantomCell.GetComponent<SpriteRenderer>().color = new Color(0.75f, 0.75f, 0.75f);
            phantomCell = Instantiate(phantomCellPrefab, GetCellCoordinates(j, size.y),
                GetCellAngle(j, size.y), transform);
            phantomCell.GetComponent<SpriteRenderer>().color = new Color(0.75f, 0.75f, 0.75f);
        }
    }
    
    public Vector2 GetCellCoordinates(int x, int y)
    {
        if (y % 2 == 0)
        {
            switch (x % 3)
            {
                case 0:
                    return new Vector2(CellsOffsets[0].x * (x / 3), -(CellsOffsets[0].y * y));
                case 1:
                    return new Vector2(CellsOffsets[0].x * (x / 3) + CellsOffsets[1].x,
                        -(CellsOffsets[0].y * y) + CellsOffsets[1].y);
                default:
                    return new Vector2(CellsOffsets[0].x * (x / 3) + CellsOffsets[2].x,
                        -(CellsOffsets[0].y * y) + CellsOffsets[2].y);
            }
        }
        switch ((x + 2) % 3)
        {
            case 0:
                return new Vector2(CellsOffsets[0].x * ((x + 2) / 3) - CellsOffsets[0].x / 2,
                    -(CellsOffsets[0].y * y));
            case 1:
                return new Vector2(CellsOffsets[0].x * ((x + 2) / 3) + CellsOffsets[1].x - CellsOffsets[0].x / 2,
                    -(CellsOffsets[0].y * y) + CellsOffsets[1].y);
            default:
                return new Vector2(CellsOffsets[0].x * ((x + 2) / 3) + CellsOffsets[2].x - CellsOffsets[0].x / 2,
                    -(CellsOffsets[0].y * y) + CellsOffsets[2].y);
        }
    }
    
    public Quaternion GetCellAngle(int x, int y)
    {
        if (y % 2 == 0)
        {
            switch (x % 3)
            {
                case 0:
                    return Quaternion.Euler(0, 0, 0);
                case 1:
                    return Quaternion.Euler(0, 0, 240);
                default:
                    return Quaternion.Euler(0, 0, 120);
            }
        }
        switch (x % 3)
        {
            case 0:
                return Quaternion.Euler(0, 0, 120);
            case 1:
                return Quaternion.Euler(0, 0, 0);
            default:
                return Quaternion.Euler(0, 0, 240);
        }
    }
    
    private CellType GetCellType(int x, int y)
    {
        if (y % 2 == 0)
        {
            switch (x % 3)
            {
                case 0:
                    return CellType.Left;
                case 1:
                    return CellType.Right;
                default:
                    return CellType.Bottom;
            }
        }
        switch (x % 3)
        {
            case 0:
                return CellType.Bottom;
            case 1:
                return CellType.Left;
            default:
                return CellType.Right;
        }
    }
    
    public void SetCellNeighbors(Cell cell)
    {
        if (cell.offsetCoordinates.y % 2 == 0)
        {
            cell.neighbors[0] = IsCellExist(cell.offsetCoordinates.x, cell.offsetCoordinates.y - 1);
            cell.neighbors[1] = IsCellExist(cell.offsetCoordinates.x + 1, cell.offsetCoordinates.y);
            cell.neighbors[2] = IsCellExist(cell.offsetCoordinates.x, cell.offsetCoordinates.y + 1);
            cell.neighbors[3] = IsCellExist(cell.offsetCoordinates.x - 1, cell.offsetCoordinates.y + 1);
            cell.neighbors[4] = IsCellExist(cell.offsetCoordinates.x - 1, cell.offsetCoordinates.y);
            cell.neighbors[5] = IsCellExist(cell.offsetCoordinates.x - 1, cell.offsetCoordinates.y - 1);
        }
        else
        {
            cell.neighbors[0] = IsCellExist(cell.offsetCoordinates.x + 1, cell.offsetCoordinates.y - 1);
            cell.neighbors[1] = IsCellExist(cell.offsetCoordinates.x + 1, cell.offsetCoordinates.y);
            cell.neighbors[2] = IsCellExist(cell.offsetCoordinates.x + 1, cell.offsetCoordinates.y + 1);
            cell.neighbors[3] = IsCellExist(cell.offsetCoordinates.x, cell.offsetCoordinates.y + 1);
            cell.neighbors[4] = IsCellExist(cell.offsetCoordinates.x - 1, cell.offsetCoordinates.y);
            cell.neighbors[5] = IsCellExist(cell.offsetCoordinates.x, cell.offsetCoordinates.y - 1);
        }
    }
    
    private Cell IsCellExist(int x, int y)
    {
        if (x < 0 || x >= size.x || y < 0 || y >= size.y)
        {
            return null;
        }
        return Cells[y, x];
    }
    
    public Vector3 GetRiverCoordinates(int x, int y)
    {
        if (y % 2 == 0)
        {
            if (y % 4 == 0)
            {
                switch (x / 2 % 3)
                {
                    case 0:
                        return new Vector3(Cells[y / 2, x / 2].GetComponent<Transform>().position.x +
                                           RiversOffsets[0].x,
                            Cells[y / 2, x / 2].GetComponent<Transform>().position.y + RiversOffsets[0].y, -1);
                    case 1:
                        return new Vector3(Cells[y / 2, x / 2].GetComponent<Transform>().position.x +
                                           RiversOffsets[1].x,
                            Cells[y / 2, x / 2].GetComponent<Transform>().position.y + RiversOffsets[1].y, -1);
                    default:
                        return new Vector3(Cells[y / 2, x / 2].GetComponent<Transform>().position.x +
                                           RiversOffsets[2].x,
                            Cells[y / 2, x / 2].GetComponent<Transform>().position.y + RiversOffsets[2].y, -1);
                }
            }
            switch (x / 2 % 3)
            {
                case 0:
                    return new Vector3(Cells[y / 2, x / 2].GetComponent<Transform>().position.x + RiversOffsets[2].x,
                        Cells[y / 2, x / 2].GetComponent<Transform>().position.y + RiversOffsets[2].y, -1);
                case 1:
                    return new Vector3(Cells[y / 2, x / 2].GetComponent<Transform>().position.x + RiversOffsets[0].x,
                        Cells[y / 2, x / 2].GetComponent<Transform>().position.y + RiversOffsets[0].y, -1);
                default:
                    return new Vector3(Cells[y / 2, x / 2].GetComponent<Transform>().position.x + RiversOffsets[1].x,
                        Cells[y / 2, x / 2].GetComponent<Transform>().position.y + RiversOffsets[1].y, -1);
            }
        }
        if ((y - 1) % 4 == 0)
        {
            switch (x % 6)
            {
                case 0:
                    return new Vector3(Cells[y / 2, (x + 1) / 2].GetComponent<Transform>().position.x +
                                       RiversOffsets[3].x,
                        Cells[y / 2, (x + 1) / 2].GetComponent<Transform>().position.y + RiversOffsets[3].y, -1);
                case 1:
                    return new Vector3(Cells[y / 2, (x + 1) / 2].GetComponent<Transform>().position.x +
                                       RiversOffsets[4].x,
                        Cells[y / 2, (x + 1) / 2].GetComponent<Transform>().position.y + RiversOffsets[4].y, -1);
                case 2:
                    return new Vector3(Cells[y / 2, (x + 1) / 2].GetComponent<Transform>().position.x +
                                       RiversOffsets[5].x,
                        Cells[y / 2, (x + 1) / 2].GetComponent<Transform>().position.y + RiversOffsets[5].y, -1);
                case 3:
                    return new Vector3(Cells[y / 2, (x + 1) / 2].GetComponent<Transform>().position.x +
                                       RiversOffsets[6].x,
                        Cells[y / 2, (x + 1) / 2].GetComponent<Transform>().position.y + RiversOffsets[6].y, -1);
                case 4:
                    return new Vector3(Cells[y / 2, (x + 1) / 2].GetComponent<Transform>().position.x +
                                       RiversOffsets[7].x,
                        Cells[y / 2, (x + 1) / 2].GetComponent<Transform>().position.y + RiversOffsets[7].y, -1);
                default:
                    return new Vector3(Cells[y / 2, (x + 1) / 2].GetComponent<Transform>().position.x +
                                       RiversOffsets[8].x,
                        Cells[y / 2, (x + 1) / 2].GetComponent<Transform>().position.y + RiversOffsets[8].y, -1);
            }
        }
        switch (x % 6)
        {
            case 0:
                return new Vector3(Cells[y / 2, x / 2].GetComponent<Transform>().position.x + RiversOffsets[6].x,
                    Cells[y / 2, x / 2].GetComponent<Transform>().position.y + RiversOffsets[6].y, -1);
            case 1:
                return new Vector3(Cells[y / 2, x / 2].GetComponent<Transform>().position.x + RiversOffsets[7].x,
                    Cells[y / 2, x / 2].GetComponent<Transform>().position.y + RiversOffsets[7].y, -1);
            case 2:
                return new Vector3(Cells[y / 2, x / 2].GetComponent<Transform>().position.x + RiversOffsets[8].x,
                    Cells[y / 2, x / 2].GetComponent<Transform>().position.y + RiversOffsets[8].y, -1);
            case 3:
                return new Vector3(Cells[y / 2, x / 2].GetComponent<Transform>().position.x + RiversOffsets[3].x,
                    Cells[y / 2, x / 2].GetComponent<Transform>().position.y + RiversOffsets[3].y, -1);
            case 4:
                return new Vector3(Cells[y / 2, x / 2].GetComponent<Transform>().position.x + RiversOffsets[4].x,
                    Cells[y / 2, x / 2].GetComponent<Transform>().position.y + RiversOffsets[4].y, -1);
            default:
                return new Vector3(Cells[y / 2, x / 2].GetComponent<Transform>().position.x + RiversOffsets[5].x,
                    Cells[y / 2, x / 2].GetComponent<Transform>().position.y + RiversOffsets[5].y, -1);
        }
    }
    
    public Quaternion GetRiverAngle(int x, int y)
    {
        if (y % 2 == 0)
        {
            if (y % 4 == 0)
            {
                switch (x / 2 % 3)
                {
                    case 0:
                        return Quaternion.Euler(0, 0, -77);
                    case 1:
                        return Quaternion.Euler(0, 0, -60);
                    default:
                        return Quaternion.Euler(0, 0, 60);
                }
            }
            switch (x / 2 % 3)
            {
                case 0:
                    return Quaternion.Euler(0, 0, 60);
                case 1:
                    return Quaternion.Euler(0, 0, -77);
                default:
                    return Quaternion.Euler(0, 0, -60);
            }
        }
        if ((y - 1) % 4 == 0)
        {
            switch (x % 6)
            {
                case 0:
                    return Quaternion.Euler(0, 0, 43);
                case 1:
                    return Quaternion.Euler(0, 0, -17);
                case 2:
                    return Quaternion.Euler(0, 0, 60);
                case 3:
                    return Quaternion.Euler(0, 0, 0);
                case 4:
                    return Quaternion.Euler(0, 0, 0);
                default:
                    return Quaternion.Euler(0, 0, -60);
            }
        }
        switch (x % 6)
        {
            case 0:
                return Quaternion.Euler(0, 0, 0);
            case 1:
                return Quaternion.Euler(0, 0, 0);
            case 2:
                return Quaternion.Euler(0, 0, -60);
            case 3:
                return Quaternion.Euler(0, 0, 43);
            case 4:
                return Quaternion.Euler(0, 0, -17);
            default:
                return Quaternion.Euler(0, 0, 60);
        }
    }
    
    public float GetRiverLength(int x, int y)
    {
        if (y % 2 == 0)
        {
            if (y % 4 == 0)
            {
                switch (x / 2 % 3)
                {
                    case 0:
                        return 1.34f;
                    case 1:
                        return 0.45f;
                    default:
                        return 1.05f;
                }
            }
            switch (x / 2 % 3)
            {
                case 0:
                    return 1.05f;
                case 1:
                    return 1.34f;
                default:
                    return 0.45f;
            }
        }
        if ((y - 1) % 4 == 0)
        {
            switch (x % 6)
            {
                case 0:
                    return 1.34f;
                case 1:
                    return 1.34f;
                case 2:
                    return 0.45f;
                case 3:
                    return 0.45f;
                case 4:
                    return 1.05f;
                default:
                    return 1.05f;
            }
        }
        switch (x % 6)
        {
            case 0:
                return 0.45f;
            case 1:
                return 1.05f;
            case 2:
                return 1.05f;
            case 3:
                return 1.34f;
            case 4:
                return 1.34f;
            default:
                return 0.45f;
        }
    }
    
    public void SetRiverNeighbors(River river)
    {
        if (river.coordinates.y % 2 == 0)
        {
            if (river.coordinates.y % 4 == 0)
            {
                river.neighbors[0] = IsRiverExist(river.coordinates.x + 1, river.coordinates.y - 1);
                river.neighbors[1] = IsRiverExist(river.coordinates.x + 1, river.coordinates.y + 1);
                river.neighbors[2] = IsRiverExist(river.coordinates.x, river.coordinates.y + 1);
                river.neighbors[3] = IsRiverExist(river.coordinates.x, river.coordinates.y - 1);
            }
            else
            {
                river.neighbors[0] = IsRiverExist(river.coordinates.x + 2, river.coordinates.y - 1);
                river.neighbors[1] = IsRiverExist(river.coordinates.x + 2, river.coordinates.y + 1);
                river.neighbors[2] = IsRiverExist(river.coordinates.x + 1, river.coordinates.y + 1);
                river.neighbors[3] = IsRiverExist(river.coordinates.x + 1, river.coordinates.y - 1);
            }
        }
        else
        {
            if ((river.coordinates.y - 1) % 4 == 0)
            {
                river.neighbors[0] = IsRiverExist(river.coordinates.x / 2 * 2, river.coordinates.y - 1);
                river.neighbors[1] = IsRiverExist(river.coordinates.x + 1, river.coordinates.y);
                river.neighbors[2] = IsRiverExist((river.coordinates.x - 1) / 2 * 2, river.coordinates.y + 1);
                river.neighbors[3] = IsRiverExist(river.coordinates.x - 1, river.coordinates.y);
            }
            else
            {
                river.neighbors[0] = IsRiverExist(river.coordinates.x + 1, river.coordinates.y);
                river.neighbors[1] = IsRiverExist(river.coordinates.x / 2 * 2, river.coordinates.y + 1);
                river.neighbors[2] = IsRiverExist(river.coordinates.x - 1, river.coordinates.y);
                river.neighbors[3] = IsRiverExist((river.coordinates.x - 1) / 2 * 2, river.coordinates.y - 1);
            }
        }
    }
    
    private River IsRiverExist(int x, int y)
    {
        River river = Rivers.FirstOrDefault(river => river.coordinates.x == x && river.coordinates.y == y);
        if (river != null)
        {
            return river;
        }
        return null;
    }

    public Cell SelectCellForNewUnit(City city)
    {
        if (city.coreCell.unit == null)
        {
            return city.coreCell;
        }
        List<Cell> cells = new List<Cell>();
        foreach (Cell neighbor in city.coreCell.neighbors)
        {
            if (neighbor != null && Pathfinder.FindPath(city.coreCell, neighbor, true,
                    float.PositiveInfinity, city.Owner.Name, null) != null)
            {
                cells.Add(neighbor);
            }
        }
        foreach (Cell neighbor in cells)
        {
            if (neighbor.unit == null)
            {
                return neighbor;
            }
        }
        return null;
    }

    public void CreateUnit(UnitType unitType, Cell cell, Civilization civ)
    {
        Unit unit;
        Vector2 position = cell.GetComponent<Transform>().position;
        switch (civ.Name)
        {
            case CivilizationName.Egypt:
                if (unitType == UnitType.Chariot)
                {
                    unit = Instantiate(egyptUnitsPrefabs[4], new Vector3(position.x, position.y, -2), 
                        Quaternion.identity, transform).GetComponent<Unit>();
                }
                else
                {
                    unit = Instantiate(egyptUnitsPrefabs[(int)unitType],
                        new Vector3(position.x, position.y, -2),
                        Quaternion.identity, transform).GetComponent<Unit>();
                }
                break;
            case CivilizationName.Greece:
                if (unitType == UnitType.Hoplite)
                {
                    unit = Instantiate(greeceUnitsPrefabs[5], new Vector3(position.x, position.y, -2), 
                        Quaternion.identity, transform).GetComponent<Unit>();
                }
                else
                {
                    unit = Instantiate(greeceUnitsPrefabs[(int)unitType],
                        new Vector3(position.x, position.y, -2), 
                        Quaternion.identity, transform).GetComponent<Unit>();
                }
                break;
            default:
                if (unitType == UnitType.SiegeTower)
                {
                    unit = Instantiate(mesopotamiaUnitsPrefabs[8], new Vector3(position.x, position.y, -2), 
                        Quaternion.identity, transform).GetComponent<Unit>();
                }
                else
                {
                    unit = Instantiate(mesopotamiaUnitsPrefabs[(int)unitType],
                        new Vector3(position.x, position.y, -2), 
                        Quaternion.identity, transform).GetComponent<Unit>();
                }
                break;
        }
        unit.Init(unitType, cell, civ);
        civ.Units.Add(unit);
        if (unit.Owner.Name != GameLogic.SelectedCiv)
        {
            GameLogic.Civs[0].CreateFogOfWar();
            if (!GameLogic.Civs[0].VisibleCells.Contains(cell))
            {
                unit.gameObject.SetActive(false);
            }
        }
        else if (SelectedUnit != null)
        {
            SelectedUnit.DeleteBorders();
            SelectedUnit = null;
        }
    }
    
    public void CreateUnit(UnitData unitData, Cell cell, Civilization civ)
    {
        Unit unit;
        Vector2 position = cell.GetComponent<Transform>().position;
        switch (civ.Name)
        {
            case CivilizationName.Egypt:
                if (unitData.type == UnitType.Chariot)
                {
                    unit = Instantiate(egyptUnitsPrefabs[4], new Vector3(position.x, position.y, -2), 
                        Quaternion.identity, transform).GetComponent<Unit>();
                }
                else
                {
                    unit = Instantiate(egyptUnitsPrefabs[(int)unitData.type],
                        new Vector3(position.x, position.y, -2),
                        Quaternion.identity, transform).GetComponent<Unit>();
                }
                break;
            case CivilizationName.Greece:
                if (unitData.type == UnitType.Hoplite)
                {
                    unit = Instantiate(greeceUnitsPrefabs[5], new Vector3(position.x, position.y, -2), 
                        Quaternion.identity, transform).GetComponent<Unit>();
                }
                else
                {
                    unit = Instantiate(greeceUnitsPrefabs[(int)unitData.type],
                        new Vector3(position.x, position.y, -2), 
                        Quaternion.identity, transform).GetComponent<Unit>();
                }
                break;
            default:
                if (unitData.type == UnitType.SiegeTower)
                {
                    unit = Instantiate(mesopotamiaUnitsPrefabs[8], new Vector3(position.x, position.y, -2), 
                        Quaternion.identity, transform).GetComponent<Unit>();
                }
                else
                {
                    unit = Instantiate(mesopotamiaUnitsPrefabs[(int)unitData.type],
                        new Vector3(position.x, position.y, -2), 
                        Quaternion.identity, transform).GetComponent<Unit>();
                }
                break;
        }
        unit.Init(unitData, cell, civ);
        civ.Units.Add(unit);
        if (unit.Owner.Name != GameLogic.SelectedCiv)
        {
            GameLogic.Civs[0].CreateFogOfWar();
            if (!GameLogic.Civs[0].VisibleCells.Contains(cell))
            {
                unit.gameObject.SetActive(false);
            }
        }
        else if (SelectedUnit != null)
        {
            SelectedUnit.DeleteBorders();
            SelectedUnit = null;
        }
    }
    
    public void CreateCity(Settler settler)
    {
        Vector2 position = settler.currentCell.transform.position;
        City city = Instantiate(cityPrefab, new Vector3(position.x, position.y, -3), 
            Quaternion.identity, transform).GetComponent<City>();
        city.Init(settler.currentCell, settler.Owner);
        settler.Owner.Cities.Add(city);
        if (settler.Owner.Cities.Count == 1)
        {
            city.DrawCapital();
        }
        city.SetCityColor();
        city.SetResourcesCount();
        Destroy(settler.gameObject);
        if (settler.Owner.Name != GameLogic.SelectedCiv)
        {
            GameLogic.Civs[0].CreateFogOfWar();
            if (!GameLogic.Civs[0].ExploredCells.Contains(settler.currentCell))
            {
                city.gameObject.SetActive(false);
            }
        }
    }
    
    public void CreateCity(CityData cityData, Cell cityCell, Civilization civ)
    {
        Vector2 position = cityCell.transform.position;
        City city = Instantiate(cityPrefab, new Vector3(position.x, position.y, -3), 
            Quaternion.identity, transform).GetComponent<City>();
        HashSet<Cell> cells = new HashSet<Cell>();
        foreach (Cell cell in Cells)
        {
            if (cityData.CellsCoordinates.Contains(cell.offsetCoordinates))
            {
                cells.Add(cell);
            }
        }
        city.Init(cityData, cityCell, cells, civ);
        civ.Cities.Add(city);
        if (civ.Cities.Count == 1)
        {
            city.DrawCapital();
        }
        city.SetCityColor();
        city.SetResourcesCount();
        if (civ.Name != GameLogic.SelectedCiv)
        {
            GameLogic.Civs[0].CreateFogOfWar();
            if (!GameLogic.Civs[0].ExploredCells.Contains(cityCell))
            {
                city.gameObject.SetActive(false);
            }
        }
    }
}