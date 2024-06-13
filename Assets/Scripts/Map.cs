using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class Map : MonoBehaviour
{
    [SerializeField] private UIController uiController;
    [SerializeField] public Vector2Int size;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private GameObject phantomCellPrefab;
    [SerializeField] private GameObject cityPrefab;
    private GameObject[,] _cells;
    private Vector2[] _cellOffsets;
    private Vector2[] _riverOffsets;
    private Vector2Int _origin;
    private Vector2 _center;
    public static float MinX;
    public static float MaxX;
    public static float MinY;
    public static float MaxY;
    public static string CurrentMessage;
    public static bool MenuEnabled;
    private int _altitudeSeed;
    private int _humiditySeed;
    private Border[,] _borders;
    private List<River> _rivers;
    private HashSet<Border> _riverSources;
    private HashSet<Border> _riverUsed;
    private HashSet<Border> _riverTempUsed;
    [Space(10)]
    [Header("Terrain")]
    [Space(10)]
    [SerializeField] private float altitudeA;
    [SerializeField] private float altitudeB;
    [SerializeField] private float altitudeC;
    [SerializeField] private int altitudeScale;
    [SerializeField] private int humidityScale;
    [Header("Water")]
    [Range(0, 100)] public int waterMinAltitude;
    [Range(0, 100)] public int waterMaxAltitude;
    [Range(0, 100)] public int waterMinHumidity;
    [Range(0, 100)] public int waterMaxHumidity;
    [SerializeField] private int waterCost;
    [Header("Desert")]
    [Range(0, 100)] public int desertMinAltitude;
    [Range(0, 100)] public int desertMaxAltitude;
    [Range(0, 100)] public int desertMinHumidity;
    [Range(0, 100)] public int desertMaxHumidity;
    [SerializeField] private int desertCost;
    [Header("Floodplain")]
    [SerializeField] private int floodplainCost;
    [Header("Plain")]
    [Range(0, 100)] public int plainMinAltitude;
    [Range(0, 100)] public int plainMaxAltitude;
    [Range(0, 100)] public int plainMinHumidity;
    [Range(0, 100)] public int plainMaxHumidity;
    [SerializeField] private int plainCost;
    [Header("Forest")]
    [Range(0, 100)] public int forestMinAltitude;
    [Range(0, 100)] public int forestMaxAltitude;
    [Range(0, 100)] public int forestMinHumidity;
    [Range(0, 100)] public int forestMaxHumidity;
    [SerializeField] private int forestCost;
    [Header("Desert Hills")]
    [Range(0, 100)] public int desertHillsMinAltitude;
    [Range(0, 100)] public int desertHillsMaxAltitude;
    [Range(0, 100)] public int desertHillsMinHumidity;
    [Range(0, 100)] public int desertHillsMaxHumidity;
    [SerializeField] private int desertHillsCost;
    [Header("Hills")]
    [Range(0, 100)] public int hillsMinAltitude;
    [Range(0, 100)] public int hillsMaxAltitude;
    [Range(0, 100)] public int hillsMinHumidity;
    [Range(0, 100)] public int hillsMaxHumidity;
    [SerializeField] private int hillsCost;
    [Header("Mountains")]
    [Range(0, 100)] public int mountainsMinAltitude;
    [Range(0, 100)] public int mountainsMaxAltitude;
    [Range(0, 100)] public int mountainsMinHumidity;
    [Range(0, 100)] public int mountainsMaxHumidity;
    [SerializeField] private int mountainsCost;
    [Space(10)]
    [Header("Rivers")]
    [Space(10)]
    [SerializeField] private int riverMaxCount;
    [SerializeField] private int riverBetweenMinDistance;
    [SerializeField] private int riverGenAttempts;
    [SerializeField] private int riverCost;
    [SerializeField] private GameObject riverPrefab;
    [Range(0, 100)] public int riverSourcesMinAltitude;
    [Range(0, 100)] public int riverSourcesMaxAltitude;
    [Space(10)]
    [Header("Resources")]
    [Space(10)]
    [SerializeField] private GameObject[] resourcesPrefabs;
    [Header("Water")]
    [Range(0, 100)] public int waterFish;
    [Header("Desert")]
    [Range(0, 100)] public int desertStone;
    [Range(0, 100)] public int desertGold;
    [Header("Floodplain")]
    [Range(0, 100)] public int floodplainWheat;
    [Range(0, 100)] public int floodplainCattle;
    [Range(0, 100)] public int floodplainHorses;
    [Header("Plain")]
    [Range(0, 100)] public int plainWheat;
    [Range(0, 100)] public int plainCattle;
    [Range(0, 100)] public int plainStone;
    [Range(0, 100)] public int plainGold;
    [Range(0, 100)] public int plainHorses;
    [Header("Forest")]
    [Range(0, 100)] public int forestWood;
    [Header("Desert Hills")]
    [Range(0, 100)] public int desertHillsStone;
    [Range(0, 100)] public int desertHillsGold;
    [Range(0, 100)] public int desertHillsCopper;
    [Range(0, 100)] public int desertHillsIron;
    [Header("Hills")]
    [Range(0, 100)] public int hillsStone;
    [Range(0, 100)] public int hillsGold;
    [Range(0, 100)] public int hillsCattle;
    [Range(0, 100)] public int hillsCopper;
    [Range(0, 100)] public int hillsIron;
    [Space(10)]
    [Header("Units")]
    [Space(10)]
    [SerializeField] private GameObject[] egyptUnitsPrefabs;
    [SerializeField] private GameObject[] greeceUnitsPrefabs;
    [SerializeField] private GameObject[] mesopotamiaUnitsPrefabs;
    [Space(10)]
    [Header("Units Costs")]
    [Space(10)]
    [Header("Settler")]
    [SerializeField] private int settlerFood;
    [SerializeField] private int settlerWood;
    [SerializeField] private int settlerStone;
    [SerializeField] private int settlerGold;
    [SerializeField] private int settlerHorses;
    [SerializeField] private int settlerBronze;
    [SerializeField] private int settlerIron;
    [Header("Scout")]
    [SerializeField] private int scoutFood;
    [SerializeField] private int scoutWood;
    [SerializeField] private int scoutStone;
    [SerializeField] private int scoutGold;
    [SerializeField] private int scoutHorses;
    [SerializeField] private int scoutBronze;
    [SerializeField] private int scoutIron;
    [Header("Warrior")]
    [SerializeField] private int warriorFood;
    [SerializeField] private int warriorWood;
    [SerializeField] private int warriorStone;
    [SerializeField] private int warriorGold;
    [SerializeField] private int warriorHorses;
    [SerializeField] private int warriorBronze;
    [SerializeField] private int warriorIron;
    [Header("Slinger")]
    [SerializeField] private int slingerFood;
    [SerializeField] private int slingerWood;
    [SerializeField] private int slingerStone;
    [SerializeField] private int slingerGold;
    [SerializeField] private int slingerHorses;
    [SerializeField] private int slingerBronze;
    [SerializeField] private int slingerIron;
    [Header("Archer")]
    [SerializeField] private int archerFood;
    [SerializeField] private int archerWood;
    [SerializeField] private int archerStone;
    [SerializeField] private int archerGold;
    [SerializeField] private int archerHorses;
    [SerializeField] private int archerBronze;
    [SerializeField] private int archerIron;
    [Header("Chariot")]
    [SerializeField] private int chariotFood;
    [SerializeField] private int chariotWood;
    [SerializeField] private int chariotStone;
    [SerializeField] private int chariotGold;
    [SerializeField] private int chariotHorses;
    [SerializeField] private int chariotBronze;
    [SerializeField] private int chariotIron;
    [Header("Spearman")]
    [SerializeField] private int spearmanFood;
    [SerializeField] private int spearmanWood;
    [SerializeField] private int spearmanStone;
    [SerializeField] private int spearmanGold;
    [SerializeField] private int spearmanHorses;
    [SerializeField] private int spearmanBronze;
    [SerializeField] private int spearmanIron;
    [Header("Hoplite")]
    [SerializeField] private int hopliteFood;
    [SerializeField] private int hopliteWood;
    [SerializeField] private int hopliteStone;
    [SerializeField] private int hopliteGold;
    [SerializeField] private int hopliteHorses;
    [SerializeField] private int hopliteBronze;
    [SerializeField] private int hopliteIron;
    [Header("Horseman")]
    [SerializeField] private int horsemanFood;
    [SerializeField] private int horsemanWood;
    [SerializeField] private int horsemanStone;
    [SerializeField] private int horsemanGold;
    [SerializeField] private int horsemanHorses;
    [SerializeField] private int horsemanBronze;
    [SerializeField] private int horsemanIron;
    [Header("Swordsman")]
    [SerializeField] private int swordsmanFood;
    [SerializeField] private int swordsmanWood;
    [SerializeField] private int swordsmanStone;
    [SerializeField] private int swordsmanGold;
    [SerializeField] private int swordsmanHorses;
    [SerializeField] private int swordsmanBronze;
    [SerializeField] private int swordsmanIron;
    [Header("Catapult")]
    [SerializeField] private int catapultFood;
    [SerializeField] private int catapultWood;
    [SerializeField] private int catapultStone;
    [SerializeField] private int catapultGold;
    [SerializeField] private int catapultHorses;
    [SerializeField] private int catapultBronze;
    [SerializeField] private int catapultIron;
    [Header("Siege Tower")]
    [SerializeField] private int siegeTowerFood;
    [SerializeField] private int siegeTowerWood;
    [SerializeField] private int siegeTowerStone;
    [SerializeField] private int siegeTowerGold;
    [SerializeField] private int siegeTowerHorses;
    [SerializeField] private int siegeTowerBronze;
    [SerializeField] private int siegeTowerIron;
    [Space(10)]
    [Header("Buildings Costs")]
    [Space(10)]
    [Header("Farm")]
    [SerializeField] private int farmFood;
    [SerializeField] private int farmWood;
    [SerializeField] private int farmStone;
    [SerializeField] private int farmGold;
    [SerializeField] private int farmHorses;
    [SerializeField] private int farmBronze;
    [SerializeField] private int farmIron;
    [Header("Pasture")]
    [SerializeField] private int pastureFood;
    [SerializeField] private int pastureWood;
    [SerializeField] private int pastureStone;
    [SerializeField] private int pastureGold;
    [SerializeField] private int pastureHorses;
    [SerializeField] private int pastureBronze;
    [SerializeField] private int pastureIron;
    [Header("Fishing Boat")]
    [SerializeField] private int fishingBoatFood;
    [SerializeField] private int fishingBoatWood;
    [SerializeField] private int fishingBoatStone;
    [SerializeField] private int fishingBoatGold;
    [SerializeField] private int fishingBoatHorses;
    [SerializeField] private int fishingBoatBronze;
    [SerializeField] private int fishingBoatIron;
    [Header("Lumber Mill")]
    [SerializeField] private int lumberMillFood;
    [SerializeField] private int lumberMillWood;
    [SerializeField] private int lumberMillStone;
    [SerializeField] private int lumberMillGold;
    [SerializeField] private int lumberMillHorses;
    [SerializeField] private int lumberMillBronze;
    [SerializeField] private int lumberMillIron;
    [Header("Quarry")]
    [SerializeField] private int quarryFood;
    [SerializeField] private int quarryWood;
    [SerializeField] private int quarryStone;
    [SerializeField] private int quarryGold;
    [SerializeField] private int quarryHorses;
    [SerializeField] private int quarryBronze;
    [SerializeField] private int quarryIron;
    [Header("Gold Mine")]
    [SerializeField] private int goldMineFood;
    [SerializeField] private int goldMineWood;
    [SerializeField] private int goldMineStone;
    [SerializeField] private int goldMineGold;
    [SerializeField] private int goldMineHorses;
    [SerializeField] private int goldMineBronze;
    [SerializeField] private int goldMineIron;
    [Header("Stable")]
    [SerializeField] private int stableFood;
    [SerializeField] private int stableWood;
    [SerializeField] private int stableStone;
    [SerializeField] private int stableGold;
    [SerializeField] private int stableHorses;
    [SerializeField] private int stableBronze;
    [SerializeField] private int stableIron;
    [Header("Mine")]
    [SerializeField] private int mineFood;
    [SerializeField] private int mineWood;
    [SerializeField] private int mineStone;
    [SerializeField] private int mineGold;
    [SerializeField] private int mineHorses;
    [SerializeField] private int mineBronze;
    [SerializeField] private int mineIron;
    [Header("Blacksmith")]
    [SerializeField] private int blacksmithFood;
    [SerializeField] private int blacksmithWood;
    [SerializeField] private int blacksmithStone;
    [SerializeField] private int blacksmithGold;
    [SerializeField] private int blacksmithHorses;
    [SerializeField] private int blacksmithBronze;
    [SerializeField] private int blacksmithIron;
    [Header("Walls")]
    [SerializeField] private int wallsFood;
    [SerializeField] private int wallsWood;
    [SerializeField] private int wallsStone;
    [SerializeField] private int wallsGold;
    [SerializeField] private int wallsHorses;
    [SerializeField] private int wallsBronze;
    [SerializeField] private int wallsIron;
    [Space(10)]
    [Header("Cities")]
    [Space(10)]
    [SerializeField] private int cityExpansionRadius;
    [SerializeField] private int expansionFoodCount;
    [Space(10)]
    [Header("Modifiers")]
    [SerializeField] private float ordinaryResourcesModifier;
    [SerializeField] private float strategicResourcesModifier;
    [SerializeField] private float capitalModifier;
    [SerializeField] private float riverModifier;
    [Space(10)]
    private Cell _goalCell;
    private GameObject _clickedObject;
    private GameObject _focusedObject;
    private float _longPressDuration;
    private float _pressStartTime;
    [NonSerialized] public static Unit SelectedUnit;
    [NonSerialized] public static City SelectedCity;
    private Vector2 _touchPosition;
    private float _touchDelta;

    private void Awake()
    {
        if (GameLogic.LoadGame)
        {
            size = ES3.Load<Vector2Int>("size");
        }
        _longPressDuration = 0.25f;
        _touchDelta = 50f;
        _cellOffsets = new Vector2[3];
        _cellOffsets[0] = new Vector2(4.56f, 1.31f);
        _cellOffsets[1] = new Vector2(1.32f, 0.35f);
        _cellOffsets[2] = new Vector2(3.25f, 0.35f);
        _riverOffsets = new Vector2[9];
        _riverOffsets[0] = new Vector2(0.61f, 0.45f);
        _riverOffsets[1] = new Vector2(0.85f, -0.35f);
        _riverOffsets[2] = new Vector2(0.81f, -0.09f);
        _riverOffsets[3] = new Vector2(0.27f, -0.65f);
        _riverOffsets[4] = new Vector2(0.08f, -0.75f);
        _riverOffsets[5] = new Vector2(0.84f, -0.74f);
        _riverOffsets[6] = new Vector2(-0.74f, -0.55f);
        _riverOffsets[7] = new Vector2(0.03f, -0.55f);
        _riverOffsets[8] = new Vector2(-0.5f, -0.66f);
        _origin.y = size.y / 2;
        _origin.x = size.x / 2 - size.y / 4;
        _center = GetCellCoordinates(_origin.x + (_origin.x - 1) / 2, _origin.y);
        gameObject.GetComponent<Transform>().position -= (Vector3)_center;
        Vector2 minCoord = GetCellCoordinates(0, 0);
        Vector2 maxCoord = GetCellCoordinates(size.x - 1, size.y - 1);
        MinX = minCoord.x - 1.5f;
        MaxY = minCoord.y + 1.5f;
        MaxX = maxCoord.x + 1.75f;
        MinY = maxCoord.y - 1.85f;
        MenuEnabled = false;
        CreatePhantomCells();
        _cells = new GameObject[size.y, size.x];
        _borders = new Border[size.y * 2 - 1, size.x * 2 - 1];
        _rivers = new List<River>();
        _riverSources = new HashSet<Border>();
        _riverUsed = new HashSet<Border>();
        _riverTempUsed = new HashSet<Border>();
        if (GameLogic.LoadGame)
        {
            LoadGame();
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
                _cells[i, j] = Instantiate(cellPrefab, GetCellCoordinates(j, i),
                    GetCellAngle(j, i), transform);
                _cells[i, j].GetComponent<Cell>().Init(j, i, GetCellType(j, i));
            }
        }
        for (int i = 0; i < size.y; i++)
        {
            for (int j = 0; j < size.x; j++)
            {
                SetCellNeighbors(_cells[i, j].GetComponent<Cell>());
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
        _altitudeSeed = Random.Range(1, 100000);
        _humiditySeed = Random.Range(1, 100000);
        GenerateTerrain();
        GenerateRivers();
        GenerateResources();
        GenerateCivilizations();
    }
    
    public void LoadGame()
    {
        if (Camera.main != null)
        {
            Camera.main.transform.position = ES3.Load<Vector3>("cameraPosition");
            Camera.main.transform.rotation = ES3.Load<Quaternion>("cameraRotation");
            Camera.main.transform.localScale = ES3.Load<Vector3>("cameraScale");
            Camera.main.orthographicSize = ES3.Load<float>("cameraSize");
        }
        GameLogicData gameLogicData = ES3.Load<GameLogicData>("gameLogic");
        GameLogic.Result = null;
        GameLogic.SelectedCiv = gameLogicData.selectedCiv;
        GameLogic.Civs = new Civilization[gameLogicData.civs.Length];
        GameLogic.AIs = new AI[gameLogicData.civs.Length - 1];
        for (int i = 0; i < gameLogicData.civs.Length; i++)
        {
            GameLogic.Civs[i] = new Civilization(gameLogicData.civs[i]);
            if (i != 0)
            {
                GameLogic.AIs[i - 1] = new AI(this, GameLogic.Civs[i]);
            }
        }
        GameLogic.Turn = gameLogicData.turn;
        GameLogic.WaterCost = gameLogicData.waterCost;
        GameLogic.PlainCost = gameLogicData.plainCost;
        GameLogic.DesertCost = gameLogicData.desertCost;
        GameLogic.FloodplainCost = gameLogicData.floodplainCost;
        GameLogic.ForestCost = gameLogicData.forestCost;
        GameLogic.HillsCost = gameLogicData.hillsCost;
        GameLogic.DesertHillsCost = gameLogicData.desertHillsCost;
        GameLogic.MountainsCost = gameLogicData.mountainsCost;
        GameLogic.RiverCost = gameLogicData.riverCost;
        GameLogic.UnitsCosts = gameLogicData.UnitsCosts;
        GameLogic.BuildingsCosts = gameLogicData.BuildingsCosts;
        GameLogic.CityExpansionRadius = gameLogicData.cityExpansionRadius;
        GameLogic.ExpansionFoodCount = gameLogicData.expansionFoodCount;
        GameLogic.OrdinaryResourcesModifier = gameLogicData.ordinaryResourcesModifier;
        GameLogic.StrategicResourcesModifier = gameLogicData.strategicResourcesModifier;
        GameLogic.CapitalModifier = gameLogicData.capitalModifier;
        GameLogic.RiverModifier = gameLogicData.riverModifier;
        CellData[,] cellsData = ES3.Load<CellData[,]>("cells");
        for (int i = 0; i < size.y; i++)
        {
            for (int j = 0; j < size.x; j++)
            {
                _cells[i, j] = Instantiate(cellPrefab, GetCellCoordinates(j, i),
                    GetCellAngle(j, i), transform);
                _cells[i, j].GetComponent<Cell>().Init(cellsData[i, j]);
                _cells[i, j].GetComponent<Cell>().SetTerraIncognitaColor();
                if (cellsData[i, j].resourceType != ResourceType.None)
                {
                    switch (cellsData[i, j].resourceType)
                    {
                        case ResourceType.Wheat:
                            _cells[i, j].GetComponent<Cell>().resource =
                                Instantiate(resourcesPrefabs[0],
                                    new Vector3(_cells[i, j].transform.position.x,
                                        _cells[i, j].transform.position.y, -1), Quaternion.identity, transform);
                            break;
                        case ResourceType.Cattle:
                            _cells[i, j].GetComponent<Cell>().resource =
                                Instantiate(resourcesPrefabs[1],
                                    new Vector3(_cells[i, j].transform.position.x,
                                        _cells[i, j].transform.position.y, -1), Quaternion.identity, transform);
                            break;
                        case ResourceType.Fish:
                            _cells[i, j].GetComponent<Cell>().resource =
                                Instantiate(resourcesPrefabs[2],
                                    new Vector3(_cells[i, j].transform.position.x,
                                        _cells[i, j].transform.position.y, -1), Quaternion.identity, transform);
                            break;
                        case ResourceType.Wood:
                            _cells[i, j].GetComponent<Cell>().resource =
                                Instantiate(resourcesPrefabs[3],
                                    new Vector3(_cells[i, j].transform.position.x,
                                        _cells[i, j].transform.position.y, -1), Quaternion.identity, transform);
                            break;
                        case ResourceType.Stone:
                            _cells[i, j].GetComponent<Cell>().resource =
                                Instantiate(resourcesPrefabs[4],
                                    new Vector3(_cells[i, j].transform.position.x,
                                        _cells[i, j].transform.position.y, -1), Quaternion.identity, transform);
                            break;
                        case ResourceType.Gold:
                            _cells[i, j].GetComponent<Cell>().resource =
                                Instantiate(resourcesPrefabs[5],
                                    new Vector3(_cells[i, j].transform.position.x,
                                        _cells[i, j].transform.position.y, -1), Quaternion.identity, transform);
                            break;
                        case ResourceType.Horses:
                            _cells[i, j].GetComponent<Cell>().resource =
                                Instantiate(resourcesPrefabs[6],
                                    new Vector3(_cells[i, j].transform.position.x,
                                        _cells[i, j].transform.position.y, -1), Quaternion.identity, transform);
                            break;
                        case ResourceType.Copper:
                            _cells[i, j].GetComponent<Cell>().resource =
                                Instantiate(resourcesPrefabs[7],
                                    new Vector3(_cells[i, j].transform.position.x,
                                        _cells[i, j].transform.position.y, -1), Quaternion.identity, transform);
                            break;
                        case ResourceType.Iron:
                            _cells[i, j].GetComponent<Cell>().resource =
                                Instantiate(resourcesPrefabs[8],
                                    new Vector3(_cells[i, j].transform.position.x,
                                        _cells[i, j].transform.position.y, -1), Quaternion.identity, transform);
                            break;
                    }
                    _cells[i, j].GetComponent<Cell>().resource.SetActive(false);
                }
                if (cellsData[i, j].rivers.Count > 0)
                {
                    foreach (RiverData riverData in cellsData[i, j].rivers)
                    {
                        River river = _rivers.FirstOrDefault(river => river.coordinates == riverData.coordinates);
                        if (river == null)
                        {
                            Vector2Int coord = riverData.coordinates;
                            river = Instantiate(riverPrefab,
                                GetRiverCoordinates(coord.x, coord.y),
                                GetRiverAngle(coord.x, coord.y), transform).GetComponent<River>();
                            river.transform.localScale = new Vector3(GetRiverLength(coord.x, coord.y), 0.20f,
                                -1f);
                            river.Init(riverData);
                            river.cell1 = _cells[i, j].GetComponent<Cell>();
                            _cells[i, j].GetComponent<Cell>().rivers.Add(river);
                            _rivers.Add(river);
                        }
                        else
                        {
                            river.cell2 = _cells[i, j].GetComponent<Cell>();
                            _cells[i, j].GetComponent<Cell>().rivers.Add(river);
                        }
                    }
                }
            }
        }
        for (int i = 0; i < size.y; i++)
        {
            for (int j = 0; j < size.x; j++)
            {
                SetCellNeighbors(_cells[i, j].GetComponent<Cell>());
            }
        }
        foreach (River river in _rivers)
        {
            SetRiverNeighbors(river);
            river.gameObject.SetActive(false);
        }
        for (int i = 0; i < size.y; i++)
        {
            for (int j = 0; j < size.x; j++)
            {
                if (cellsData[i, j].unit != null)
                {
                    Civilization civ =
                        GameLogic.Civs.FirstOrDefault(civ => civ.Name == cellsData[i, j].unit.owner);
                    CreateUnit(cellsData[i, j].unit, _cells[i, j].GetComponent<Cell>(), civ);
                }
                if (cellsData[i, j].city != null
                    && cellsData[i, j].city.coreCellCoordinates == cellsData[i, j].offsetCoordinates)
                {
                    Civilization civ =
                        GameLogic.Civs.FirstOrDefault(civ => civ.Name == cellsData[i, j].city.owner);
                    CreateCity(cellsData[i,j].city, _cells[i, j].GetComponent<Cell>(), civ);
                }
                foreach (Civilization civ in GameLogic.Civs)
                {
                    if (cellsData[i, j].IsExplored[civ.Name])
                    {
                        if (civ.Name == GameLogic.SelectedCiv)
                        {
                            if (_cells[i, j].GetComponent<Cell>().rivers.Count > 0)
                            {
                                foreach (River river in _cells[i, j].GetComponent<Cell>().rivers)
                                {
                                    if (cellsData[river.cell1.offsetCoordinates.y,
                                            river.cell1.offsetCoordinates.x].IsExplored[GameLogic.SelectedCiv]
                                        && cellsData[river.cell2.offsetCoordinates.y,
                                            river.cell2.offsetCoordinates.x].IsExplored[GameLogic.SelectedCiv])
                                    {
                                        river.gameObject.SetActive(true);
                                    }
                                }
                            }
                            if (_cells[i, j].GetComponent<Cell>().resource != null)
                            {
                                _cells[i, j].GetComponent<Cell>().resource.SetActive(true);
                            }
                        }
                        civ.ExploredCells.Add(_cells[i, j].GetComponent<Cell>());
                    }
                }
            }
        }
        GameLogic.Civs[0].CreateFogOfWar();
        uiController.SetCurrentValues();
    }
    
    public void SaveGame()
    {
        if (Camera.main != null)
        {
            ES3.Save("cameraPosition", Camera.main.transform.position);
            ES3.Save("cameraRotation", Camera.main.transform.rotation);
            ES3.Save("cameraScale", Camera.main.transform.localScale);
            ES3.Save("cameraSize", Camera.main.orthographicSize);
        }
        ES3.Save("gameLogic", GameLogic.CollectGameLogicData());
        CellData[,] cellsData = new CellData[size.y, size.x];
        for (int i = 0; i < size.y; i++)
        {
            for (int j = 0; j < size.x; j++)
            {
                cellsData[i, j] = _cells[i, j].GetComponent<Cell>().CollectCellData();
            }
        }
        ES3.Save("cells", cellsData);
        ES3.Save("size", new Vector2Int(size.x, size.y));
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
            Collider2D collider = Physics2D.OverlapPoint(touchPosition);
            if (collider != null)
            {
                GameObject touchedObject = collider.gameObject;
                return touchedObject;
            }
        }
        return null;
    }
    
    private Vector2 GetCellCoordinates(int x, int y)
    {
        if (y % 2 == 0)
        {
            switch (x % 3)
            {
                case 0:
                    return new Vector2(_cellOffsets[0].x * (x / 3), -(_cellOffsets[0].y * y));
                case 1:
                    return new Vector2(_cellOffsets[0].x * (x / 3) + _cellOffsets[1].x,
                        -(_cellOffsets[0].y * y) + _cellOffsets[1].y);
                default:
                    return new Vector2(_cellOffsets[0].x * (x / 3) + _cellOffsets[2].x,
                        -(_cellOffsets[0].y * y) + _cellOffsets[2].y);
            }
        }
        switch ((x + 2) % 3)
        {
            case 0:
                return new Vector2(_cellOffsets[0].x * ((x + 2) / 3) - _cellOffsets[0].x / 2,
                    -(_cellOffsets[0].y * y));
            case 1:
                return new Vector2(_cellOffsets[0].x * ((x + 2) / 3) + _cellOffsets[1].x - _cellOffsets[0].x / 2,
                    -(_cellOffsets[0].y * y) + _cellOffsets[1].y);
            default:
                return new Vector2(_cellOffsets[0].x * ((x + 2) / 3) + _cellOffsets[2].x - _cellOffsets[0].x / 2,
                    -(_cellOffsets[0].y * y) + _cellOffsets[2].y);
        }
    }
    
    private Quaternion GetCellAngle(int x, int y)
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
    
    private void SetCellNeighbors(Cell cell)
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
        return _cells[y, x].GetComponent<Cell>();
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

    private void GenerateTerrain()
    {
        for (int i = 0; i < size.y; i++)
        {
            for (int j = 0; j < size.x; j++)
            {
                float x = (j + _altitudeSeed) / (float)altitudeScale;
                float y = (i + _altitudeSeed) / (float)altitudeScale;
                _cells[i, j].GetComponent<Cell>().altitude = Mathf.PerlinNoise(x, y) + altitudeA - altitudeB
                    * (float)Math.Pow(GetDistance(j, i), altitudeC);
                x = (j + _humiditySeed) / (float)humidityScale;
                y = (i + _humiditySeed) / (float)humidityScale;
                _cells[i, j].GetComponent<Cell>().humidity = Mathf.PerlinNoise(x, y);
            }
        }
        NormalizeCells();
        SelectCellTerrain(waterMinAltitude, waterMaxAltitude, waterMinHumidity,
            waterMaxHumidity, Terrain.Water);
        SelectCellTerrain(desertMinAltitude, desertMaxAltitude, desertMinHumidity,
            desertMaxHumidity, Terrain.Desert);
        SelectCellTerrain(plainMinAltitude, plainMaxAltitude, plainMinHumidity,
            plainMaxHumidity, Terrain.Plain);
        SelectCellTerrain(forestMinAltitude, forestMaxAltitude, forestMinHumidity,
            forestMaxHumidity, Terrain.Forest);
        SelectCellTerrain(desertHillsMinAltitude, desertHillsMaxAltitude,
            desertHillsMinHumidity, desertHillsMaxHumidity, Terrain.DesertHills);
        SelectCellTerrain(hillsMinAltitude, hillsMaxAltitude, hillsMinHumidity,
            hillsMaxHumidity, Terrain.Hills);
        SelectCellTerrain(mountainsMinAltitude, mountainsMaxAltitude,
            mountainsMinHumidity, mountainsMaxHumidity, Terrain.Mountains);
        CreateWaterBorder();
    }
    
    private float GetDistance(int x, int y)
    {
        float distX = Math.Abs(x - size.x / 2) / (float)(size.x / 2);
        float distY = Math.Abs(y - size.y / 2) / (float)(size.y / 2);
        return (float)Math.Sqrt(Math.Pow(distX, 2) + Math.Pow(distY, 2));
    }
    
    private void NormalizeCells()
    {
        float minAltitude = float.MaxValue;
        float maxAltitude = float.MinValue;
        float minHumidity = float.MaxValue;
        float maxHumidity = float.MinValue;
        foreach (GameObject cellObject in _cells)
        {
            Cell cell = cellObject.GetComponent<Cell>();
            if (cell != null)
            {
                minAltitude = Mathf.Min(minAltitude, cell.altitude);
                maxAltitude = Mathf.Max(maxAltitude, cell.altitude);
                minHumidity = Mathf.Min(minHumidity, cell.humidity);
                maxHumidity = Mathf.Max(maxHumidity, cell.humidity);
            }
        }
        foreach (GameObject cellObject in _cells)
        {
            Cell cell = cellObject.GetComponent<Cell>();
            if (cell != null)
            {
                cell.altitude = NormalizeValue(cell.altitude, minAltitude, maxAltitude);
                cell.humidity = NormalizeValue(cell.humidity, minHumidity, maxHumidity);
            }
        }
    }

    private float NormalizeValue(float value, float min, float max)
    {
        return (value - min) / (max - min);
    }
    
    private void SelectCellTerrain(int startPercent1, int endPercent1, int startPercent2, int endPercent2,
        Terrain terrain)
    {
        Cell[] cellArray = _cells.Cast<GameObject>().Select(go => go.GetComponent<Cell>()).ToArray();
        Array.Sort(cellArray, (x, y) => x.altitude.CompareTo(y.altitude));
        int startIdx1 = (int)(cellArray.Length * (startPercent1 / 100f));
        int endIdx1 = (int)(cellArray.Length * (endPercent1 / 100f));
        Cell[] selectedCellsByAltitude = cellArray.Skip(startIdx1).Take(endIdx1 - startIdx1).ToArray();
        Array.Sort(cellArray, (x, y) => x.humidity.CompareTo(y.humidity));
        int startIdx2 = (int)(cellArray.Length * (startPercent2 / 100f));
        int endIdx2 = (int)(cellArray.Length * (endPercent2 / 100f));
        Cell[] selectedCellsByHumidity = cellArray.Skip(startIdx2).Take(endIdx2 - startIdx2).ToArray();
        Cell[] finalSelectedCells = selectedCellsByAltitude.Intersect(selectedCellsByHumidity).ToArray();
        foreach (var cell in finalSelectedCells)
        {
            cell.terrain = terrain;
            cell.SetTerraIncognitaColor();
        }
    }

    private void CreateWaterBorder()
    {
        for (int i = 0; i < size.y; i++)
        {
            for (int j = 0; j < size.x; j++)
            {
                if (i % 2 == 0 && (j <= 1 || j == size.x - 1) || i % 2 != 0 && (j == 0 || j >= size.x - 2)
                                                              || i == 0 || i == size.y - 1)
                {
                    _cells[i, j].GetComponent<Cell>().terrain = Terrain.Water;
                    _cells[i, j].GetComponent<Cell>().SetTerraIncognitaColor();
                }
            }
        }
    }

    private void GenerateRivers()
    {
        for (int i = 0; i < _borders.GetLength(0); i++)
        {
            if (i % 2 == 0)
            {
                for (int j = 0; j < size.x - 1; j++)
                {
                    _borders[i, j * 2] = new Border(new Vector2Int(j * 2, i),
                        _cells[i / 2, j].GetComponent<Cell>(),
                        _cells[i / 2, j + 1].GetComponent<Cell>());
                }
            }
            else
            {
                if ((i - 1) % 4 == 0)
                {
                    for (int j = 0; j < _borders.GetLength(1); j++)
                    {
                        _borders[i, j] = new Border(new Vector2Int(j, i),
                            _cells[i / 2, j - j / 2].GetComponent<Cell>(),
                            _cells[i - i / 2, j / 2].GetComponent<Cell>());
                    }
                }
                else
                {
                    for (int j = 0; j < _borders.GetLength(1); j++)
                    {
                        _borders[i, j] = new Border(new Vector2Int(j, i),
                            _cells[i / 2, j - (j + 1) / 2].GetComponent<Cell>(),
                            _cells[i - i / 2, (j + 1) / 2].GetComponent<Cell>());
                    }
                }
            }
        }
        for (int i = 0; i < _borders.GetLength(0); i++)
        {
            for (int j = 0; j < _borders.GetLength(1); j++)
            {
                if (_borders[i, j] != null)
                {
                    SetBorderNeighbors(_borders[i, j]);
                }
            }
        }
        SelectRiverSources(riverSourcesMinAltitude, riverSourcesMaxAltitude);
        bool result;
        int attempts = 0;
        int k = 0;
        Border source;
        while (_riverSources.Count > 0 && k < riverMaxCount && attempts < riverGenAttempts)
        {
            source = _riverSources.ElementAt(Random.Range(0, _riverSources.Count));
            result = SetRiverPath(source, k);
            _riverSources.Remove(source);
            if (!result)
            {
                attempts++;
            }
            else
            {
                k++;
            }
        }
        foreach (River river in _rivers)
        {
            river.gameObject.SetActive(false);
        }
    }
    
    private Vector3 GetRiverCoordinates(int x, int y)
    {
        if (y % 2 == 0)
        {
            if (y % 4 == 0)
            {
                switch (x / 2 % 3)
                {
                    case 0:
                        return new Vector3(_cells[y / 2, x / 2].GetComponent<Transform>().position.x +
                                           _riverOffsets[0].x,
                            _cells[y / 2, x / 2].GetComponent<Transform>().position.y + _riverOffsets[0].y, -1);
                    case 1:
                        return new Vector3(_cells[y / 2, x / 2].GetComponent<Transform>().position.x +
                                           _riverOffsets[1].x,
                            _cells[y / 2, x / 2].GetComponent<Transform>().position.y + _riverOffsets[1].y, -1);
                    default:
                        return new Vector3(_cells[y / 2, x / 2].GetComponent<Transform>().position.x +
                                           _riverOffsets[2].x,
                            _cells[y / 2, x / 2].GetComponent<Transform>().position.y + _riverOffsets[2].y, -1);
                }
            }
            switch (x / 2 % 3)
            {
                case 0:
                    return new Vector3(_cells[y / 2, x / 2].GetComponent<Transform>().position.x + _riverOffsets[2].x,
                        _cells[y / 2, x / 2].GetComponent<Transform>().position.y + _riverOffsets[2].y, -1);
                case 1:
                    return new Vector3(_cells[y / 2, x / 2].GetComponent<Transform>().position.x + _riverOffsets[0].x,
                        _cells[y / 2, x / 2].GetComponent<Transform>().position.y + _riverOffsets[0].y, -1);
                default:
                    return new Vector3(_cells[y / 2, x / 2].GetComponent<Transform>().position.x + _riverOffsets[1].x,
                        _cells[y / 2, x / 2].GetComponent<Transform>().position.y + _riverOffsets[1].y, -1);
            }
        }
        if ((y - 1) % 4 == 0)
        {
            switch (x % 6)
            {
                case 0:
                    return new Vector3(_cells[y / 2, (x + 1) / 2].GetComponent<Transform>().position.x +
                                       _riverOffsets[3].x,
                        _cells[y / 2, (x + 1) / 2].GetComponent<Transform>().position.y + _riverOffsets[3].y, -1);
                case 1:
                    return new Vector3(_cells[y / 2, (x + 1) / 2].GetComponent<Transform>().position.x +
                                       _riverOffsets[4].x,
                        _cells[y / 2, (x + 1) / 2].GetComponent<Transform>().position.y + _riverOffsets[4].y, -1);
                case 2:
                    return new Vector3(_cells[y / 2, (x + 1) / 2].GetComponent<Transform>().position.x +
                                       _riverOffsets[5].x,
                        _cells[y / 2, (x + 1) / 2].GetComponent<Transform>().position.y + _riverOffsets[5].y, -1);
                case 3:
                    return new Vector3(_cells[y / 2, (x + 1) / 2].GetComponent<Transform>().position.x +
                                       _riverOffsets[6].x,
                        _cells[y / 2, (x + 1) / 2].GetComponent<Transform>().position.y + _riverOffsets[6].y, -1);
                case 4:
                    return new Vector3(_cells[y / 2, (x + 1) / 2].GetComponent<Transform>().position.x +
                                       _riverOffsets[7].x,
                        _cells[y / 2, (x + 1) / 2].GetComponent<Transform>().position.y + _riverOffsets[7].y, -1);
                default:
                    return new Vector3(_cells[y / 2, (x + 1) / 2].GetComponent<Transform>().position.x +
                                       _riverOffsets[8].x,
                        _cells[y / 2, (x + 1) / 2].GetComponent<Transform>().position.y + _riverOffsets[8].y, -1);
            }
        }
        switch (x % 6)
        {
            case 0:
                return new Vector3(_cells[y / 2, x / 2].GetComponent<Transform>().position.x + _riverOffsets[6].x,
                    _cells[y / 2, x / 2].GetComponent<Transform>().position.y + _riverOffsets[6].y, -1);
            case 1:
                return new Vector3(_cells[y / 2, x / 2].GetComponent<Transform>().position.x + _riverOffsets[7].x,
                    _cells[y / 2, x / 2].GetComponent<Transform>().position.y + _riverOffsets[7].y, -1);
            case 2:
                return new Vector3(_cells[y / 2, x / 2].GetComponent<Transform>().position.x + _riverOffsets[8].x,
                    _cells[y / 2, x / 2].GetComponent<Transform>().position.y + _riverOffsets[8].y, -1);
            case 3:
                return new Vector3(_cells[y / 2, x / 2].GetComponent<Transform>().position.x + _riverOffsets[3].x,
                    _cells[y / 2, x / 2].GetComponent<Transform>().position.y + _riverOffsets[3].y, -1);
            case 4:
                return new Vector3(_cells[y / 2, x / 2].GetComponent<Transform>().position.x + _riverOffsets[4].x,
                    _cells[y / 2, x / 2].GetComponent<Transform>().position.y + _riverOffsets[4].y, -1);
            default:
                return new Vector3(_cells[y / 2, x / 2].GetComponent<Transform>().position.x + _riverOffsets[5].x,
                    _cells[y / 2, x / 2].GetComponent<Transform>().position.y + _riverOffsets[5].y, -1);
        }
    }
    
    private Quaternion GetRiverAngle(int x, int y)
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
    
    private float GetRiverLength(int x, int y)
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
    
    private void SetBorderNeighbors(Border border)
    {
        if (border.Coordinates.y % 2 == 0)
        {
            if (border.Coordinates.y % 4 == 0)
            {
                border.Neighbors[0] = IsBorderExist(border.Coordinates.x + 1, border.Coordinates.y - 1);
                border.Neighbors[1] = IsBorderExist(border.Coordinates.x + 1, border.Coordinates.y + 1);
                border.Neighbors[2] = IsBorderExist(border.Coordinates.x, border.Coordinates.y + 1);
                border.Neighbors[3] = IsBorderExist(border.Coordinates.x, border.Coordinates.y - 1);
            }
            else
            {
                border.Neighbors[0] = IsBorderExist(border.Coordinates.x + 2, border.Coordinates.y - 1);
                border.Neighbors[1] = IsBorderExist(border.Coordinates.x + 2, border.Coordinates.y + 1);
                border.Neighbors[2] = IsBorderExist(border.Coordinates.x + 1, border.Coordinates.y + 1);
                border.Neighbors[3] = IsBorderExist(border.Coordinates.x + 1, border.Coordinates.y - 1);
            }
        }
        else
        {
            if ((border.Coordinates.y - 1) % 4 == 0)
            {
                border.Neighbors[0] = IsBorderExist(border.Coordinates.x / 2 * 2, border.Coordinates.y - 1);
                border.Neighbors[1] = IsBorderExist(border.Coordinates.x + 1, border.Coordinates.y);
                border.Neighbors[2] = IsBorderExist((border.Coordinates.x - 1) / 2 * 2, border.Coordinates.y + 1);
                border.Neighbors[3] = IsBorderExist(border.Coordinates.x - 1, border.Coordinates.y);
            }
            else
            {
                border.Neighbors[0] = IsBorderExist(border.Coordinates.x + 1, border.Coordinates.y);
                border.Neighbors[1] = IsBorderExist(border.Coordinates.x / 2 * 2, border.Coordinates.y + 1);
                border.Neighbors[2] = IsBorderExist(border.Coordinates.x - 1, border.Coordinates.y);
                border.Neighbors[3] = IsBorderExist((border.Coordinates.x - 1) / 2 * 2, border.Coordinates.y - 1);
            }
        }
    }
    
    private Border IsBorderExist(int x, int y)
    {
        if (x < 0 || ((x >= _borders.GetLength(1) - 2 || x % 2 != 0) && y % 2 == 0)
                  || (x >= _borders.GetLength(1) && x % 2 != 0) || y < 0 || y >= _borders.GetLength(0))
        {
            return null;
        }
        return _borders[y, x];
    }
    
    private void SetRiverNeighbors(River river)
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
        River river = _rivers.FirstOrDefault(river => river.coordinates.x == x && river.coordinates.y == y);
        if (river != null)
        {
            return river;
        }
        return null;
    }
    
    private void SelectRiverSources(int startPercent, int endPercent)
    {
        Border[] borderArray = _borders.Cast<Border>().Where(b => b != null).ToArray();
        Array.Sort(borderArray, (x, y) => x.Altitude.CompareTo(y.Altitude));
        int startIdx = (int)(borderArray.Length * (startPercent / 100f));
        int endIdx = (int)(borderArray.Length * (endPercent / 100f));
        Border[] selectedBorders = borderArray.Skip(startIdx).Take(endIdx - startIdx).ToArray();
        foreach (var border in selectedBorders)
        {
            if (border.Cell1.terrain == border.Cell2.terrain && border.Cell1.terrain != Terrain.Water
                                                             && border.Cell1.terrain != Terrain.Mountains)
            {
                _riverSources.Add(border);
            }
        }
    }

    private bool SetRiverPath(Border border, int id)
    {
        _riverTempUsed.Clear();
        List<Border> riverPath = new List<Border>();
        riverPath.Add(border);
        bool flag = true;
        int startIndex = _rivers.Count;
        while (flag)
        {
            if (riverPath.Count == 0)
            {
                return false;
            }
            border = SetRiverDirection(riverPath.Last());
            if (border is not null)
            {
                _riverTempUsed.Add(border);
                riverPath.Add(border);
            }
            else
            {
                border = riverPath.Last();
                for (int i = 0; i < 4; i++)
                {
                    if (border.Neighbors[i] != null && (border.Neighbors[i].Cell1.terrain == Terrain.Water ||
                        border.Neighbors[i].Cell2.terrain == Terrain.Water))
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    _riverTempUsed.Add(border);
                    riverPath.RemoveAt(riverPath.Count - 1);
                }
            }
        }
        foreach (Border b in riverPath)
        {
            GameObject river = Instantiate(riverPrefab, 
                GetRiverCoordinates(b.Coordinates.x, b.Coordinates.y),
                GetRiverAngle(b.Coordinates.x, b.Coordinates.y), transform);
            river.GetComponent<Transform>().localScale =
                new Vector3(GetRiverLength(b.Coordinates.x, b.Coordinates.y), 0.20f, -1f);
            river.GetComponent<River>().Init(id + 1, b);
            _rivers.Add(river.GetComponent<River>());
            UseBordersInRadius(b, riverBetweenMinDistance);
            b.Cell1.rivers.Add(river.GetComponent<River>());
            b.Cell2.rivers.Add(river.GetComponent<River>());
            if (b.Cell1.terrain == Terrain.Desert)
            {
                b.Cell1.terrain = Terrain.Floodplain;
            }
            if (b.Cell2.terrain == Terrain.Desert)
            {
                b.Cell2.terrain = Terrain.Floodplain;
            }
        }
        for (int i = 0; i < riverPath.Count; i++)
        {
            if (i == 0)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (riverPath[i].Neighbors[j] != null
                        && _rivers[startIndex + 1].coordinates == riverPath[i].Neighbors[j].Coordinates)
                    {
                        _rivers[startIndex].neighbors[j] = _rivers[startIndex + 1];
                        break;
                    }
                }
            }
            else if (i == riverPath.Count - 1)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (riverPath[i].Neighbors[j] != null
                        && _rivers[^2].coordinates == riverPath[i].Neighbors[j].Coordinates)
                    {
                        _rivers[^1].neighbors[j] = _rivers[^2];
                        break;
                    }
                }
            }
            else
            {
                for (int j = 0; j < 4; j++)
                {
                    if (riverPath[i].Neighbors[j] != null)
                    {
                        if (_rivers[startIndex + i - 1].coordinates == riverPath[i].Neighbors[j].Coordinates)
                        {
                            _rivers[startIndex + i].neighbors[j] = _rivers[startIndex + i - 1];
                        }
                        if (_rivers[startIndex + i + 1].coordinates == riverPath[i].Neighbors[j].Coordinates)
                        {
                            _rivers[startIndex + i].neighbors[j] = _rivers[startIndex + i + 1];
                        }
                    }
                }
            }
        }
        return true;
    }

    private Border SetRiverDirection(Border border)
    {
        float min = border.Altitude;
        int index = -1;
        for (int i = 0; i < 4; i++)
        {
            if (border.Neighbors[i] != null && border.Neighbors[i].Altitude < min
                                            && !_riverUsed.Contains(border.Neighbors[i])
                                            && !_riverTempUsed.Contains(border.Neighbors[i])
                                            && border.Neighbors[i].Cell1.terrain != Terrain.Water
                                            && border.Neighbors[i].Cell2.terrain != Terrain.Water)
            {
                min = border.Neighbors[i].Altitude;
                index = i;
            }
        }
        if (index == -1)
        {
            return null;
        }
        return border.Neighbors[index];
    }
    
    private void UseBordersInRadius(Border border, int radius)
    {
        if (radius < 0)
        {
            return;
        }
        _riverUsed.Add(border);
        _riverSources.Remove(border);
        foreach (Border neighbor in border.Neighbors)
        {
            if (neighbor != null)
            {
                UseBordersInRadius(neighbor, radius - 1);
            }
        }
    }

    private void GenerateResources()
    {
        int random;
        GameObject resource;
        Vector2 position;
        foreach (GameObject cell in _cells)
        {
            random = Random.Range(0, 100);
            resource = null;
            position = cell.GetComponent<Transform>().position;
            switch (cell.GetComponent<Cell>().terrain)
            {
                case Terrain.Water:
                    if (random < waterFish)
                    {
                        resource = Instantiate(resourcesPrefabs[2],
                            new Vector3(position.x, position.y, -1), Quaternion.identity,
                            transform);
                        cell.GetComponent<Cell>().resourceType = ResourceType.Fish;
                    }
                    break;
                case Terrain.Desert:
                    if (random < desertStone)
                    {
                        resource = Instantiate(resourcesPrefabs[4],
                            new Vector3(position.x, position.y, -1), Quaternion.identity,
                            transform);
                        cell.GetComponent<Cell>().resourceType = ResourceType.Stone;
                    }
                    else if (random < desertGold)
                    {
                        resource = Instantiate(resourcesPrefabs[5],
                            new Vector3(position.x, position.y, -1), Quaternion.identity,
                            transform);
                        cell.GetComponent<Cell>().resourceType = ResourceType.Gold;
                    }
                    break;
                case Terrain.Floodplain:
                    if (random < floodplainWheat)
                    {
                        resource = Instantiate(resourcesPrefabs[0], new Vector3(position.x, position.y, -1),
                            Quaternion.identity, transform);
                        cell.GetComponent<Cell>().resourceType = ResourceType.Wheat;
                    }
                    else if (random < floodplainCattle)
                    {
                        resource = Instantiate(resourcesPrefabs[1], new Vector3(position.x, position.y, -1),
                            Quaternion.identity, transform);
                        cell.GetComponent<Cell>().resourceType = ResourceType.Cattle;
                    }
                    else if (random < floodplainHorses)
                    {
                        resource = Instantiate(resourcesPrefabs[6], new Vector3(position.x, position.y, -1),
                            Quaternion.identity, transform);
                        cell.GetComponent<Cell>().resourceType = ResourceType.Horses;
                    }
                    break;
                case Terrain.Plain:
                    if (random < plainWheat)
                    {
                        resource = Instantiate(resourcesPrefabs[0], new Vector3(position.x, position.y, -1),
                            Quaternion.identity, transform);
                        cell.GetComponent<Cell>().resourceType = ResourceType.Wheat;
                    }
                    else if (random < plainCattle)
                    {
                        resource = Instantiate(resourcesPrefabs[1], new Vector3(position.x, position.y, -1),
                            Quaternion.identity, transform);
                        cell.GetComponent<Cell>().resourceType = ResourceType.Cattle;
                    }
                    else if (random < plainStone)
                    {
                        resource = Instantiate(resourcesPrefabs[4], new Vector3(position.x, position.y, -1),
                            Quaternion.identity, transform);
                        cell.GetComponent<Cell>().resourceType = ResourceType.Stone;
                    }
                    else if (random < plainGold)
                    {
                        resource = Instantiate(resourcesPrefabs[5], new Vector3(position.x, position.y, -1),
                            Quaternion.identity, transform);
                        cell.GetComponent<Cell>().resourceType = ResourceType.Gold;
                    }
                    else if (random < plainHorses)
                    {
                        resource = Instantiate(resourcesPrefabs[6], new Vector3(position.x, position.y, -1),
                            Quaternion.identity, transform);
                        cell.GetComponent<Cell>().resourceType = ResourceType.Horses;
                    }
                    break;
                case Terrain.Forest:
                    if (random < forestWood)
                    {
                        resource = Instantiate(resourcesPrefabs[3], new Vector3(position.x, position.y, -1),
                            Quaternion.identity, transform);
                        cell.GetComponent<Cell>().resourceType = ResourceType.Wood;
                    }
                    break;
                case Terrain.DesertHills:
                    if (random < desertHillsStone)
                    {
                        resource = Instantiate(resourcesPrefabs[4], new Vector3(position.x, position.y, -1),
                            Quaternion.identity, transform);
                        cell.GetComponent<Cell>().resourceType = ResourceType.Stone;
                    }
                    else if (random < desertHillsGold)
                    {
                        resource = Instantiate(resourcesPrefabs[5], new Vector3(position.x, position.y, -1),
                            Quaternion.identity, transform);
                        cell.GetComponent<Cell>().resourceType = ResourceType.Gold;
                    }
                    else if (random < desertHillsCopper)
                    {
                        resource = Instantiate(resourcesPrefabs[7], new Vector3(position.x, position.y, -1),
                            Quaternion.identity, transform);
                        cell.GetComponent<Cell>().resourceType = ResourceType.Copper;
                    }
                    else if (random < desertHillsIron)
                    {
                        resource = Instantiate(resourcesPrefabs[8], new Vector3(position.x, position.y, -1),
                            Quaternion.identity, transform);
                        cell.GetComponent<Cell>().resourceType = ResourceType.Iron;
                    }
                    break;
                case Terrain.Hills:
                    if (random < hillsStone)
                    {
                        resource = Instantiate(resourcesPrefabs[4], new Vector3(position.x, position.y, -1),
                            Quaternion.identity, transform);
                        cell.GetComponent<Cell>().resourceType = ResourceType.Stone;
                    }
                    else if (random < hillsGold)
                    {
                        resource = Instantiate(resourcesPrefabs[5], new Vector3(position.x, position.y, -1),
                            Quaternion.identity, transform);
                        cell.GetComponent<Cell>().resourceType = ResourceType.Gold;
                    }
                    else if (random < hillsCattle)
                    {
                        resource = Instantiate(resourcesPrefabs[1], new Vector3(position.x, position.y, -1),
                            Quaternion.identity, transform);
                        cell.GetComponent<Cell>().resourceType = ResourceType.Cattle;
                    }
                    else if (random < hillsCopper)
                    {
                        resource = Instantiate(resourcesPrefabs[7], new Vector3(position.x, position.y, -1),
                            Quaternion.identity, transform);
                        cell.GetComponent<Cell>().resourceType = ResourceType.Copper;
                    }
                    else if (random < hillsIron)
                    {
                        resource = Instantiate(resourcesPrefabs[8], new Vector3(position.x, position.y, -1),
                            Quaternion.identity, transform);
                        cell.GetComponent<Cell>().resourceType = ResourceType.Iron;
                    }
                    break;
            }
            if (resource != null)
            {
                resource.SetActive(false);
                cell.GetComponent<Cell>().resource = resource;
            }
        }
    }

    private void GenerateCivilizations()
    {
        CivilizationName[] names = (CivilizationName[])Enum.GetValues(typeof(CivilizationName));
        int index = Array.IndexOf(names, GameLogic.SelectedCiv);
        CivilizationName temp = names[index];
        Array.Copy(names, 0, names, 1, index);
        names[0] = temp;
        List<Cell> spawnCells = new List<Cell>();
        foreach (GameObject go in _cells)
        {
            Cell cell = go.GetComponent<Cell>();
            if (cell.rivers.Count > 0 && cell.terrain != Terrain.Hills && cell.terrain != Terrain.DesertHills)
            {
                spawnCells.Add(cell.GetComponent<Cell>());
            }
        }
        int random;
        Cell selectedCell;
        int riverId;
        List<Cell> path;
        float distance;
        for (int i = 0; i < names.Length; i++)
        {
            GameLogic.Civs[i] = new Civilization(names[i]);
            random = Random.Range(0, spawnCells.Count);
            selectedCell = spawnCells[random];
            riverId = selectedCell.rivers[0].id;
            CreateUnit(UnitType.Settler, selectedCell, GameLogic.Civs[i]);
            for (int j = spawnCells.Count - 1; j >= 0; j--)
            {
                path = Pathfinder.FindPath(selectedCell, spawnCells[j], true,
                    float.PositiveInfinity, GameLogic.SelectedCiv, null);
                if (path != null)
                {
                    distance = Pathfinder.CalculateTotalPathLength(path, false);
                }
                else
                {
                    distance = 0;
                }
                if (spawnCells[j].rivers[0].id == riverId || distance < 20)
                {
                    spawnCells.RemoveAt(j);
                }
            }
            if (i != 0)
            {
                GameLogic.AIs[i - 1] = new AI(this, GameLogic.Civs[i]);
            }
        }
        _focusedObject = GameLogic.Civs[0].Units[0].gameObject;
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
    
    public void CreateCity(CityData cityData, Cell cell, Civilization civ)
    {
        Vector2 position = cell.transform.position;
        City city = Instantiate(cityPrefab, new Vector3(position.x, position.y, -3), 
            Quaternion.identity, transform).GetComponent<City>();
        HashSet<Cell> cells = new HashSet<Cell>();
        foreach (GameObject go in _cells)
        {
            if (cityData.CellsCoordinates.Contains(go.GetComponent<Cell>().offsetCoordinates))
            {
                cells.Add(go.GetComponent<Cell>());
            }
        }
        city.Init(cityData, cell, cells, civ);
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
            if (!GameLogic.Civs[0].ExploredCells.Contains(cell))
            {
                city.gameObject.SetActive(false);
            }
        }
    }
}