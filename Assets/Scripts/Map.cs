using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Map : MonoBehaviour
{
    [SerializeField] private Vector2Int size;
    private GameObject[,] _cells;
    private Vector2[] _cellOffsets;
    private Vector2[] _riverOffsets;
    private Vector2Int _origin;
    private Vector2 _center;
    private int _seed;
    private int _seed2;
    private float _scale;
    private float _scale2;
    private Border[,] _borders;
    private List<River> _rivers;
    private HashSet<Border> _riverSources;
    private HashSet<Border> _riverUsed;
    private HashSet<Border> _riverTempUsed;
    [SerializeField] private int riverMaxCount;
    [SerializeField] private int riverBetweenMinDistance;
    [SerializeField] private int riverGenAttempts;
    [SerializeField] private int smoothingDegree;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private GameObject riverPrefab;

    private void Awake()
    {
        _cells = new GameObject[size.y, size.x];
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
        _seed = Random.Range(1, 100000);
        _seed2 = Random.Range(1, 100000);
        _scale = 20f;
        _scale2 = 25f;
        _borders = new Border[size.y * 2 - 1, size.x * 2 - 1];
        _rivers = new List<River>();
        _riverSources = new HashSet<Border>();
        _riverUsed = new HashSet<Border>();
        _riverTempUsed = new HashSet<Border>();
        for (int i = 0; i < size.y; i++)
        {
            for (int j = 0; j < size.x; j++)
            {
                _cells[i, j] = Instantiate(cellPrefab, GetCellCoordinates(j, i),
                    GetCellAngle(j, i), transform);
                _cells[i, j].GetComponent<Cell>().Init(j, i, GetCellType(j, i));
            }
        }
        gameObject.GetComponent<Transform>().position -= (Vector3)_center; 
        for (int i = 0; i < size.y; i++)
        {
            for (int j = 0; j < size.x; j++)
            {
                SetCellNeighbors(_cells[i, j].GetComponent<Cell>());
            }
        }
        Generate();
        GenerateRivers();
        //Smooth();
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

    private void Generate()
    {
        float minal = 2f;
        float maxal = -1f;
        float minlat = 2f;
        float maxlat = -1f;
        for (int i = 0; i < size.y; i++)
        {
            for (int j = 0; j < size.x; j++)
            {
                var x = (j + _seed) / _scale;
                var y = (i + _seed) / _scale;
                _cells[i, j].GetComponent<Cell>().alitude = Mathf.PerlinNoise(x, y) + 0.3f
                                                            - 0.3f * (float)Math.Pow(GetDistance(j, i), 2f);
                x = (j + _seed2) / _scale2;
                y = (i + _seed2) / _scale2;
                _cells[i, j].GetComponent<Cell>().latitude = Mathf.PerlinNoise(x, y);
                if (_cells[i, j].GetComponent<Cell>().alitude < 0f)
                {
                    _cells[i, j].GetComponent<Cell>().alitude = 0f;
                }
                else if (_cells[i, j].GetComponent<Cell>().alitude > 1f)
                {
                    _cells[i, j].GetComponent<Cell>().alitude = 1f;
                }
                if (_cells[i, j].GetComponent<Cell>().latitude < 0f)
                {
                    _cells[i, j].GetComponent<Cell>().latitude = 0f;
                }
                else if (_cells[i, j].GetComponent<Cell>().latitude > 1f)
                {
                    _cells[i, j].GetComponent<Cell>().latitude = 1f;
                }
                if (_cells[i, j].GetComponent<Cell>().alitude < minal)
                {
                    minal = _cells[i, j].GetComponent<Cell>().alitude;
                }
                else if (_cells[i, j].GetComponent<Cell>().alitude > maxal)
                {
                    maxal = _cells[i, j].GetComponent<Cell>().alitude;
                }
                if (_cells[i, j].GetComponent<Cell>().latitude < minlat)
                {
                    minlat = _cells[i, j].GetComponent<Cell>().latitude;
                }
                else if (_cells[i, j].GetComponent<Cell>().latitude > maxlat)
                {
                    maxlat = _cells[i, j].GetComponent<Cell>().latitude;
                }
            }
        }
        for (int i = 0; i < size.y; i++)
        {
            for (int j = 0; j < size.x; j++)
            {
                if (_cells[i, j].GetComponent<Cell>().alitude < minal + 0.5f)
                {
                    _cells[i, j].GetComponent<Cell>().terrain = Terrain.Water;
                }
                else if (_cells[i, j].GetComponent<Cell>().alitude > maxal - 0.02f)
                {
                    _cells[i, j].GetComponent<Cell>().terrain = Terrain.Mountains;
                }
                else if (_cells[i, j].GetComponent<Cell>().alitude > maxal - 0.1f)
                {
                    if (_cells[i, j].GetComponent<Cell>().latitude < minlat + 0.3f)
                    {
                        _cells[i, j].GetComponent<Cell>().terrain = Terrain.ForestHills;
                    }
                    else if (_cells[i, j].GetComponent<Cell>().latitude > maxlat - 0.2f)
                    {
                        _cells[i, j].GetComponent<Cell>().terrain = Terrain.DesertHills;
                    }
                    else
                    {
                        _cells[i, j].GetComponent<Cell>().terrain = Terrain.Hills;
                    }
                }
                else
                {
                    if (_cells[i, j].GetComponent<Cell>().latitude < minlat + 0.3f)
                    {
                        _cells[i, j].GetComponent<Cell>().terrain = Terrain.Forest;
                    }
                    else if (_cells[i, j].GetComponent<Cell>().latitude > maxlat - 0.2f)
                    {
                        _cells[i, j].GetComponent<Cell>().terrain = Terrain.Desert;
                    }
                    else
                    {
                        _cells[i, j].GetComponent<Cell>().terrain = Terrain.Plain;
                    }
                }
                SetCellSprite(_cells[i, j]);
            }
        }
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
        for (int i = 0; i < _borders.GetLength(0); i++)
        {
            for (int j = 0; j < _borders.GetLength(1); j++)
            {
                if (_borders[i, j] != null)
                {
                    if (_borders[i, j].Cell1.alitude > maxal - 0.15f && _borders[i, j].Cell2.alitude > maxal - 0.15f
                        && _borders[i, j].Cell1.terrain == _borders[i, j].Cell2.terrain
                        && _borders[i, j].Cell1.terrain != Terrain.Mountains)
                    {
                        _riverSources.Add(_borders[i, j]);
                    }
                }
            }
        }
    }
    
    private float GetDistance(int x, int y)
    {
        float distX = Math.Abs(x - size.x / 2) / (float)(size.x / 2);
        float distY = Math.Abs(y - size.y / 2) / (float)(size.y / 2);
        return (float)Math.Sqrt(Math.Pow(distX, 2) + Math.Pow(distY, 2));
    }

    private void GenerateRivers()
    {
        bool result;
        int attempts = 0;
        int i = 0;
        Border source;
        while (_riverSources.Count > 0 && i < riverMaxCount && attempts < riverGenAttempts)
        {
            source = _riverSources.ElementAt(Random.Range(0, _riverSources.Count));
            result = SetRiverPath(source, i);
            _riverSources.Remove(source);
            if (!result)
            {
                attempts++;
            }
            else
            {
                i++;
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
            UseBordersInRadius(b, riverBetweenMinDistance);
            b.Cell1.withRiver = true;
            b.Cell2.withRiver = true;
            GameObject river = Instantiate(riverPrefab, 
                GetRiverCoordinates(b.Coordinates.x, b.Coordinates.y),
                GetRiverAngle(b.Coordinates.x, b.Coordinates.y), transform);
            river.GetComponent<Transform>().localScale =
                new Vector3(GetRiverLength(b.Coordinates.x, b.Coordinates.y), 0.2f, -1f);
            river.GetComponent<River>().Init(id + 1, b);
            _rivers.Add(river.GetComponent<River>());
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
        float min = border.Alitude;
        int index = -1;
        for (int i = 0; i < 4; i++)
        {
            if (border.Neighbors[i] != null && border.Neighbors[i].Alitude < min
                                            && !_riverUsed.Contains(border.Neighbors[i])
                                            && !_riverTempUsed.Contains(border.Neighbors[i])
                                            && border.Neighbors[i].Cell1.terrain != Terrain.Water
                                            && border.Neighbors[i].Cell2.terrain != Terrain.Water)
            {
                min = border.Neighbors[i].Alitude;
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

    private void Smooth()
    {
        for (int i = 0; i < smoothingDegree; i++)
        {
            foreach (GameObject obj in _cells)
            {
                Cell cell = obj.GetComponent<Cell>();
                if (cell.cellType == CellType.Left)
                {
                    if (cell.neighbors[0] != null && cell.neighbors[1] != null && cell.neighbors[2] != null
                        && cell.terrain != cell.neighbors[0].terrain
                        && cell.terrain != cell.neighbors[1].terrain
                        && cell.terrain != cell.neighbors[2].terrain)
                    {
                        cell.terrain = cell.neighbors[0].terrain;
                    }
                    else if (cell.neighbors[1] != null && cell.neighbors[2] != null && cell.neighbors[3] != null
                        && cell.terrain != cell.neighbors[1].terrain
                        && cell.terrain != cell.neighbors[2].terrain
                        && cell.terrain != cell.neighbors[3].terrain)
                    {
                        cell.terrain = cell.neighbors[1].terrain;
                    }
                    else if (cell.neighbors[2] != null && cell.neighbors[3] != null && cell.neighbors[4] != null
                             && cell.terrain != cell.neighbors[2].terrain
                             && cell.terrain != cell.neighbors[3].terrain
                             && cell.terrain != cell.neighbors[4].terrain)
                    {
                        cell.terrain = cell.neighbors[2].terrain;
                    }
                    else if (cell.neighbors[5] != null && cell.neighbors[0] != null && cell.neighbors[1] != null
                             && cell.terrain != cell.neighbors[5].terrain
                             && cell.terrain != cell.neighbors[0].terrain
                             && cell.terrain != cell.neighbors[1].terrain)
                    {
                        cell.terrain = cell.neighbors[5].terrain;
                    }
                }
                else if (cell.cellType == CellType.Right)
                {
                    if (cell.neighbors[1] != null && cell.neighbors[2] != null && cell.neighbors[3] != null
                        && cell.terrain != cell.neighbors[1].terrain
                        && cell.terrain != cell.neighbors[2].terrain
                        && cell.terrain != cell.neighbors[3].terrain)
                    {
                        cell.terrain = cell.neighbors[1].terrain;
                    }
                    else if (cell.neighbors[2] != null && cell.neighbors[3] != null && cell.neighbors[4] != null
                        && cell.terrain != cell.neighbors[2].terrain
                        && cell.terrain != cell.neighbors[3].terrain
                        && cell.terrain != cell.neighbors[4].terrain)
                    {
                        cell.terrain = cell.neighbors[2].terrain;
                    }
                    else if (cell.neighbors[3] != null && cell.neighbors[4] != null && cell.neighbors[5] != null
                             && cell.terrain != cell.neighbors[3].terrain
                             && cell.terrain != cell.neighbors[4].terrain
                             && cell.terrain != cell.neighbors[5].terrain)
                    {
                        cell.terrain = cell.neighbors[3].terrain;
                    }
                    else if (cell.neighbors[4] != null && cell.neighbors[5] != null && cell.neighbors[0] != null
                             && cell.terrain != cell.neighbors[4].terrain
                             && cell.terrain != cell.neighbors[5].terrain
                             && cell.terrain != cell.neighbors[0].terrain)
                    {
                        cell.terrain = cell.neighbors[4].terrain;
                    }
                }
                else
                {
                    if (cell.neighbors[0] != null && cell.neighbors[1] != null && cell.neighbors[2] != null
                        && cell.terrain != cell.neighbors[0].terrain
                        && cell.terrain != cell.neighbors[1].terrain
                        && cell.terrain != cell.neighbors[2].terrain)
                    {
                        cell.terrain = cell.neighbors[0].terrain;
                    }
                    else if (cell.neighbors[3] != null && cell.neighbors[4] != null && cell.neighbors[5] != null
                        && cell.terrain != cell.neighbors[3].terrain
                        && cell.terrain != cell.neighbors[4].terrain
                        && cell.terrain != cell.neighbors[5].terrain)
                    {
                        cell.terrain = cell.neighbors[3].terrain;
                    }
                    else if (cell.neighbors[4] != null && cell.neighbors[5] != null && cell.neighbors[0] != null
                             && cell.terrain != cell.neighbors[4].terrain
                             && cell.terrain != cell.neighbors[5].terrain
                             && cell.terrain != cell.neighbors[0].terrain)
                    {
                        cell.terrain = cell.neighbors[4].terrain;
                    }
                    else if (cell.neighbors[5] != null && cell.neighbors[0] != null && cell.neighbors[1] != null
                             && cell.terrain != cell.neighbors[5].terrain
                             && cell.terrain != cell.neighbors[0].terrain
                             && cell.terrain != cell.neighbors[1].terrain)
                    {
                        cell.terrain = cell.neighbors[5].terrain;
                    }
                }
				SetCellSprite(obj);
            }
        }
    }

    private void SetCellSprite(GameObject cell)
    {
        switch (cell.GetComponent<Cell>().terrain)
        {
            case Terrain.Water:
                cell.GetComponent<SpriteRenderer>().color = new Color(0f, 0.55f, 0.94f);
                break;
            case Terrain.Plain:
                cell.GetComponent<SpriteRenderer>().color = new Color(0.49f, 0.99f, 0f);
                break;
            case Terrain.Forest:
                cell.GetComponent<SpriteRenderer>().color = new Color(0.13f, 0.55f, 0.13f);
                break;
            case Terrain.Desert:
                cell.GetComponent<SpriteRenderer>().color = new Color(0.99f, 0.87f, 0.46f);
                break;
            case Terrain.Hills:
                cell.GetComponent<SpriteRenderer>().color = new Color(0.4f, 0.8f, 0f);
                break;
            case Terrain.ForestHills:
                cell.GetComponent<SpriteRenderer>().color = new Color(0.09f, 0.39f, 0.09f);
                break;
            case Terrain.DesertHills:
                cell.GetComponent<SpriteRenderer>().color = new Color(0.99f, 0.82f, 0.28f);
                break;
            case Terrain.Mountains:
                cell.GetComponent<SpriteRenderer>().color = new Color(0.55f, 0.55f, 0.48f);
                break;
        }
    }
}