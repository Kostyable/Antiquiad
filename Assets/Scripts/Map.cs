using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Map : MonoBehaviour
{
    [SerializeField] private Vector2Int size;
    private GameObject[,] _grid;
    private Vector2 _offsetLeft;
    private Vector2 _offsetRight;
    private Vector2 _offsetBottom;
    private Vector2Int _origin;
    private Vector2 _center;
    private int _seed;
    private int _seed2;
    private float _scale;
    [SerializeField] private int smoothingDegree;
    [SerializeField] private GameObject pentagonPrefab;

    private void Awake()
    {
        _offsetLeft = new Vector2(4.56f, 1.31f);
        _offsetRight = new Vector2(1.32f, 0.35f);
        _offsetBottom = new Vector2(3.25f, 0.35f);
        _seed = Random.Range(1, 100000);
        _seed2 = Random.Range(1, 100000);
        _scale = 20f;
        _origin.y = size.y / 2;
        _origin.x = size.x / 2 - size.y / 4;
        _center = ToRealCoordinates(_origin.x + (int)((_origin.x - 1) / 2), _origin.y);
        _grid = new GameObject[size.y, size.x];
        for (int i = 0; i < size.y; i++)
        {
            for (int j = 0; j < size.x; j++)
            {
                _grid[i, j] = Instantiate(pentagonPrefab, ToRealCoordinates(j, i), GetAngle(j, i), transform);
                _grid[i, j].GetComponent<Pentagon>().Init(ToAxialCoordinates(j, i), GetType(j, i));
            }
        }
        gameObject.GetComponent<Transform>().position -= (Vector3)_center; 
        for (int i = 0; i < size.y; i++)
        {
            for (int j = 0; j < size.x; j++)
            {
                SetNeighbors(_grid[i, j].GetComponent<Pentagon>());
            }
        }
        Generate();
        Smooth();
    }

    private Vector2Int ToAxialCoordinates(int j, int i)
    {
        return new Vector2Int(_origin.x + _origin.y - j - i + (int)(i / 2), i - _origin.y);
    }
    
    private Vector2 ToRealCoordinates(int x, int y)
    {
        if (y % 2 == 0)
        {
            switch (x % 3)
            {
                case 0:
                    return new Vector2(_offsetLeft.x * (int)(x / 3), -(_offsetLeft.y * y));
                case 1:
                    return new Vector2(_offsetLeft.x * (int)(x / 3) + _offsetRight.x, -(_offsetLeft.y * y) + _offsetRight.y);
                default:
                    return new Vector2(_offsetLeft.x * (int)(x / 3) + _offsetBottom.x, -(_offsetLeft.y * y) + _offsetBottom.y);
            }
        }
        switch ((x + 2) % 3)
        {
            case 0:
                return new Vector2(_offsetLeft.x * (int)((x + 2) / 3) - _offsetLeft.x / 2, -(_offsetLeft.y * y));
            case 1:
                return new Vector2(_offsetLeft.x * (int)((x + 2) / 3) + _offsetRight.x - _offsetLeft.x / 2, -(_offsetLeft.y * y) + _offsetRight.y);
            default:
                return new Vector2(_offsetLeft.x * (int)((x + 2) / 3) + _offsetBottom.x - _offsetLeft.x / 2, -(_offsetLeft.y * y) + _offsetBottom.y);
        }
    }
    
    private PentaType GetType(int x, int y)
    {
        if (y % 2 == 0)
        {
            switch (x % 3)
            {
                case 0:
                    return PentaType.Left;
                case 1:
                    return PentaType.Right;
                default:
                    return PentaType.Bottom;
            }
        }
        switch (x % 3)
        {
            case 0:
                return PentaType.Bottom;
            case 1:
                return PentaType.Left;
            default:
                return PentaType.Right;
        }
    }
        
    private Quaternion GetAngle(int x, int y)
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

    private void Generate()
    {
        float x;
        float y;
        float[,] alitudes = new float[size.y, size.x];
        float[,] latitudes = new float[size.y, size.x];
        float minal = 2f;
        float maxal = -1f;
        float minlat = 2f;
        float maxlat = -1f;
        for (int i = 0; i < size.y; i++)
        {
            for (int j = 0; j < size.x; j++)
            {
                x = (j + _seed) / _scale;
                y = (i + _seed) / _scale;
                alitudes[i, j] = Mathf.PerlinNoise(x, y) + 0.3f - 0.3f * (float)Math.Pow(GetDistance(j, i), 2f);
                x = (j + _seed2) / _scale;
                y = (i + _seed2) / _scale;
                latitudes[i, j] = (Mathf.PerlinNoise(x, y) + 1f) / 2f - (float)GetLatitude(i) + 0.3f;
                if (alitudes[i, j] < 0f)
                {
                    alitudes[i, j] = 0f;
                }
                else if (alitudes[i, j] > 1f)
                {
                    alitudes[i, j] = 1f;
                }
                if (latitudes[i, j] < 0f)
                {
                    latitudes[i, j] = 0f;
                }
                else if (latitudes[i, j] > 1f)
                {
                    latitudes[i, j] = 1f;
                }
                if (alitudes[i, j] < minal)
                {
                    minal = alitudes[i, j];
                }
                else if (alitudes[i, j] > maxal)
                {
                    maxal = alitudes[i, j];
                }
                if (latitudes[i, j] < minlat)
                {
                    minlat = latitudes[i, j];
                }
                else if (latitudes[i, j] > maxlat)
                {
                    maxlat = latitudes[i, j];
                }
            }
        }
        for (int i = 0; i < size.y; i++)
        {
            for (int j = 0; j < size.x; j++)
            {
                if (alitudes[i, j] < minal + 0.5f)
                {
                    _grid[i, j].GetComponent<Pentagon>().terrain = Terrain.Water;
                }
                else if (alitudes[i, j] > maxal - 0.07f)
                {
                    _grid[i, j].GetComponent<Pentagon>().terrain = Terrain.Mountains;
                }
                else
                {
                    if (latitudes[i, j] < minlat + 0.4f)
                    {
                        _grid[i, j].GetComponent<Pentagon>().terrain = Terrain.Forest;
                    }
                    else if (latitudes[i, j] > maxlat - 0.05f)
                    {
                        _grid[i, j].GetComponent<Pentagon>().terrain = Terrain.Desert;
                    }
                    else
                    {
                        _grid[i, j].GetComponent<Pentagon>().terrain = Terrain.Plain;
                    }
                }
                SetSprite(_grid[i, j]);
            }
        }
    }
    
    private double GetDistance(int x, int y)
    {
        double distX = Math.Abs(x - size.x / 2 + 1) / (double)(size.x / 2);
        double distY = Math.Abs(y - size.y / 2 + 1) / (double)(size.y / 2);
        return Math.Sqrt(Math.Pow(distX, 2) + Math.Pow(distY, 2));
    }
    
    private double GetLatitude(int y)
    {
        double currentY = Math.Abs(y - size.y / 2 + 1) / (double)(size.y / 2);
        return currentY;
    }

    private void SetNeighbors(Pentagon pentagon)
    {
        pentagon.neighbors = new Pentagon[6];
        if (pentagon.axialCoordinates.y == (int)(-size.y / 2))
        {
            if (pentagon.axialCoordinates.x == size.y - (int)(size.y / 4) - (int)(pentagon.axialCoordinates.y / 2))
            {
                pentagon.neighbors[1] = FindNeighbor(new Vector2Int(pentagon.axialCoordinates.x - 1, pentagon.axialCoordinates.y));
                pentagon.neighbors[2] = FindNeighbor(new Vector2Int(pentagon.axialCoordinates.x - 1, pentagon.axialCoordinates.y + 1));
            }
            else if (pentagon.axialCoordinates.x == -size.y + (int)(size.y / 4) - (int)(pentagon.axialCoordinates.y / 2))
            {
                pentagon.neighbors[2] = FindNeighbor(new Vector2Int(pentagon.axialCoordinates.x - 1, pentagon.axialCoordinates.y + 1));
                pentagon.neighbors[3] = FindNeighbor(new Vector2Int(pentagon.axialCoordinates.x, pentagon.axialCoordinates.y + 1));
                pentagon.neighbors[4] = FindNeighbor(new Vector2Int(pentagon.axialCoordinates.x + 1, pentagon.axialCoordinates.y));
            }
            else
            {
                pentagon.neighbors[1] = FindNeighbor(new Vector2Int(pentagon.axialCoordinates.x - 1, pentagon.axialCoordinates.y));
                pentagon.neighbors[2] = FindNeighbor(new Vector2Int(pentagon.axialCoordinates.x - 1, pentagon.axialCoordinates.y + 1));
                pentagon.neighbors[3] = FindNeighbor(new Vector2Int(pentagon.axialCoordinates.x, pentagon.axialCoordinates.y + 1));
                pentagon.neighbors[4] = FindNeighbor(new Vector2Int(pentagon.axialCoordinates.x + 1, pentagon.axialCoordinates.y));
            }
        }
        else if (pentagon.axialCoordinates.y == (int)(size.y / 2))
        {
            if (pentagon.axialCoordinates.x == size.y - (int)(size.y / 4) - (int)(pentagon.axialCoordinates.y / 2))
            {
                pentagon.neighbors[0] = FindNeighbor(new Vector2Int(pentagon.axialCoordinates.x, pentagon.axialCoordinates.y - 1));
                pentagon.neighbors[1] = FindNeighbor(new Vector2Int(pentagon.axialCoordinates.x - 1, pentagon.axialCoordinates.y));
            }
            else if (pentagon.axialCoordinates.x == -size.y + (int)(size.y / 4) - (int)(pentagon.axialCoordinates.y / 2))
            {
                pentagon.neighbors[0] = FindNeighbor(new Vector2Int(pentagon.axialCoordinates.x, pentagon.axialCoordinates.y - 1));
                pentagon.neighbors[4] = FindNeighbor(new Vector2Int(pentagon.axialCoordinates.x + 1, pentagon.axialCoordinates.y));
                pentagon.neighbors[5] = FindNeighbor(new Vector2Int(pentagon.axialCoordinates.x + 1, pentagon.axialCoordinates.y - 1));
            }
            else
            {
                pentagon.neighbors[0] = FindNeighbor(new Vector2Int(pentagon.axialCoordinates.x, pentagon.axialCoordinates.y - 1));
                pentagon.neighbors[1] = FindNeighbor(new Vector2Int(pentagon.axialCoordinates.x - 1, pentagon.axialCoordinates.y));
                pentagon.neighbors[4] = FindNeighbor(new Vector2Int(pentagon.axialCoordinates.x + 1, pentagon.axialCoordinates.y));
                pentagon.neighbors[5] = FindNeighbor(new Vector2Int(pentagon.axialCoordinates.x + 1, pentagon.axialCoordinates.y - 1));
            }
        }
        else
        {
            if (pentagon.axialCoordinates.x == size.y - (int)(size.y / 4) - (int)(pentagon.axialCoordinates.y / 2))
            {
                if (pentagon.axialCoordinates.y % 2 == 0)
                {
                    pentagon.neighbors[0] = FindNeighbor(new Vector2Int(pentagon.axialCoordinates.x, pentagon.axialCoordinates.y - 1));
                    pentagon.neighbors[1] = FindNeighbor(new Vector2Int(pentagon.axialCoordinates.x - 1, pentagon.axialCoordinates.y));
                    pentagon.neighbors[2] = FindNeighbor(new Vector2Int(pentagon.axialCoordinates.x - 1, pentagon.axialCoordinates.y + 1));
                }
                else
                {
                    pentagon.neighbors[0] = FindNeighbor(new Vector2Int(pentagon.axialCoordinates.x, pentagon.axialCoordinates.y - 1));
                    pentagon.neighbors[1] = FindNeighbor(new Vector2Int(pentagon.axialCoordinates.x - 1, pentagon.axialCoordinates.y));
                    pentagon.neighbors[2] = FindNeighbor(new Vector2Int(pentagon.axialCoordinates.x - 1, pentagon.axialCoordinates.y + 1));
                    pentagon.neighbors[3] = FindNeighbor(new Vector2Int(pentagon.axialCoordinates.x, pentagon.axialCoordinates.y + 1));
                    pentagon.neighbors[5] = FindNeighbor(new Vector2Int(pentagon.axialCoordinates.x + 1, pentagon.axialCoordinates.y - 1));
                }
            }
            else if (pentagon.axialCoordinates.x == -size.y + (int)(size.y / 4) - (int)(pentagon.axialCoordinates.y / 2))
            {
                if (pentagon.axialCoordinates.y % 2 == 0)
                {
                    pentagon.neighbors[0] = FindNeighbor(new Vector2Int(pentagon.axialCoordinates.x, pentagon.axialCoordinates.y - 1));
                    pentagon.neighbors[2] = FindNeighbor(new Vector2Int(pentagon.axialCoordinates.x - 1, pentagon.axialCoordinates.y + 1));
                    pentagon.neighbors[3] = FindNeighbor(new Vector2Int(pentagon.axialCoordinates.x, pentagon.axialCoordinates.y + 1));
                    pentagon.neighbors[4] = FindNeighbor(new Vector2Int(pentagon.axialCoordinates.x + 1, pentagon.axialCoordinates.y));
                    pentagon.neighbors[5] = FindNeighbor(new Vector2Int(pentagon.axialCoordinates.x + 1, pentagon.axialCoordinates.y - 1));
                }
                else
                {
                    pentagon.neighbors[3] = FindNeighbor(new Vector2Int(pentagon.axialCoordinates.x, pentagon.axialCoordinates.y + 1));
                    pentagon.neighbors[4] = FindNeighbor(new Vector2Int(pentagon.axialCoordinates.x + 1, pentagon.axialCoordinates.y));
                    pentagon.neighbors[5] = FindNeighbor(new Vector2Int(pentagon.axialCoordinates.x + 1, pentagon.axialCoordinates.y - 1));
                }
            }
            else
            {
                pentagon.neighbors[0] = FindNeighbor(new Vector2Int(pentagon.axialCoordinates.x, pentagon.axialCoordinates.y - 1));
                pentagon.neighbors[1] = FindNeighbor(new Vector2Int(pentagon.axialCoordinates.x - 1, pentagon.axialCoordinates.y));
                pentagon.neighbors[2] = FindNeighbor(new Vector2Int(pentagon.axialCoordinates.x - 1, pentagon.axialCoordinates.y + 1));
                pentagon.neighbors[3] = FindNeighbor(new Vector2Int(pentagon.axialCoordinates.x, pentagon.axialCoordinates.y + 1));
                pentagon.neighbors[4] = FindNeighbor(new Vector2Int(pentagon.axialCoordinates.x + 1, pentagon.axialCoordinates.y));
                pentagon.neighbors[5] = FindNeighbor(new Vector2Int(pentagon.axialCoordinates.x + 1, pentagon.axialCoordinates.y - 1));
            }
        }
    }
    
    private Pentagon FindNeighbor(Vector2Int axialCoordinates)
    {
        for (int i = 0; i < size.y; i++)
        {
            for (int j = 0; j < size.x; j++)
            {
                if (_grid[i, j].GetComponent<Pentagon>().axialCoordinates == axialCoordinates)
                {
                    return _grid[i, j].GetComponent<Pentagon>();
                }
            }
        }
        return _grid[0, 0].GetComponent<Pentagon>();
    }

    private void Smooth()
    {
        for (int i = 0; i < smoothingDegree; i++)
        {
            foreach (GameObject cell in _grid)
            {
                Pentagon pentagon = cell.GetComponent<Pentagon>();
                if (pentagon.pentaType == PentaType.Left)
                {
                    if (pentagon.neighbors[0] != null && pentagon.neighbors[1] != null && pentagon.neighbors[2] != null
                        && pentagon.terrain != pentagon.neighbors[0].terrain
                        && pentagon.terrain != pentagon.neighbors[1].terrain
                        && pentagon.terrain != pentagon.neighbors[2].terrain)
                    {
                        pentagon.terrain = pentagon.neighbors[0].terrain;
                    }
                    else if (pentagon.neighbors[1] != null && pentagon.neighbors[2] != null && pentagon.neighbors[3] != null
                        && pentagon.terrain != pentagon.neighbors[1].terrain
                        && pentagon.terrain != pentagon.neighbors[2].terrain
                        && pentagon.terrain != pentagon.neighbors[3].terrain)
                    {
                        pentagon.terrain = pentagon.neighbors[1].terrain;
                    }
                    else if (pentagon.neighbors[2] != null && pentagon.neighbors[3] != null && pentagon.neighbors[4] != null
                             && pentagon.terrain != pentagon.neighbors[2].terrain
                             && pentagon.terrain != pentagon.neighbors[3].terrain
                             && pentagon.terrain != pentagon.neighbors[4].terrain)
                    {
                        pentagon.terrain = pentagon.neighbors[2].terrain;
                    }
                    else if (pentagon.neighbors[5] != null && pentagon.neighbors[0] != null && pentagon.neighbors[1] != null
                             && pentagon.terrain != pentagon.neighbors[5].terrain
                             && pentagon.terrain != pentagon.neighbors[0].terrain
                             && pentagon.terrain != pentagon.neighbors[1].terrain)
                    {
                        pentagon.terrain = pentagon.neighbors[5].terrain;
                    }
                }
                else if (pentagon.pentaType == PentaType.Right)
                {
                    if (pentagon.neighbors[1] != null && pentagon.neighbors[2] != null && pentagon.neighbors[3] != null
                        && pentagon.terrain != pentagon.neighbors[1].terrain
                        && pentagon.terrain != pentagon.neighbors[2].terrain
                        && pentagon.terrain != pentagon.neighbors[3].terrain)
                    {
                        pentagon.terrain = pentagon.neighbors[1].terrain;
                    }
                    else if (pentagon.neighbors[2] != null && pentagon.neighbors[3] != null && pentagon.neighbors[4] != null
                        && pentagon.terrain != pentagon.neighbors[2].terrain
                        && pentagon.terrain != pentagon.neighbors[3].terrain
                        && pentagon.terrain != pentagon.neighbors[4].terrain)
                    {
                        pentagon.terrain = pentagon.neighbors[2].terrain;
                    }
                    else if (pentagon.neighbors[3] != null && pentagon.neighbors[4] != null && pentagon.neighbors[5] != null
                             && pentagon.terrain != pentagon.neighbors[3].terrain
                             && pentagon.terrain != pentagon.neighbors[4].terrain
                             && pentagon.terrain != pentagon.neighbors[5].terrain)
                    {
                        pentagon.terrain = pentagon.neighbors[3].terrain;
                    }
                    else if (pentagon.neighbors[4] != null && pentagon.neighbors[5] != null && pentagon.neighbors[0] != null
                             && pentagon.terrain != pentagon.neighbors[4].terrain
                             && pentagon.terrain != pentagon.neighbors[5].terrain
                             && pentagon.terrain != pentagon.neighbors[0].terrain)
                    {
                        pentagon.terrain = pentagon.neighbors[4].terrain;
                    }
                }
                else
                {
                    if (pentagon.neighbors[0] != null && pentagon.neighbors[1] != null && pentagon.neighbors[2] != null
                        && pentagon.terrain != pentagon.neighbors[0].terrain
                        && pentagon.terrain != pentagon.neighbors[1].terrain
                        && pentagon.terrain != pentagon.neighbors[2].terrain)
                    {
                        pentagon.terrain = pentagon.neighbors[0].terrain;
                    }
                    else if (pentagon.neighbors[3] != null && pentagon.neighbors[4] != null && pentagon.neighbors[5] != null
                        && pentagon.terrain != pentagon.neighbors[3].terrain
                        && pentagon.terrain != pentagon.neighbors[4].terrain
                        && pentagon.terrain != pentagon.neighbors[5].terrain)
                    {
                        pentagon.terrain = pentagon.neighbors[3].terrain;
                    }
                    else if (pentagon.neighbors[4] != null && pentagon.neighbors[5] != null && pentagon.neighbors[0] != null
                             && pentagon.terrain != pentagon.neighbors[4].terrain
                             && pentagon.terrain != pentagon.neighbors[5].terrain
                             && pentagon.terrain != pentagon.neighbors[0].terrain)
                    {
                        pentagon.terrain = pentagon.neighbors[4].terrain;
                    }
                    else if (pentagon.neighbors[5] != null && pentagon.neighbors[0] != null && pentagon.neighbors[1] != null
                             && pentagon.terrain != pentagon.neighbors[5].terrain
                             && pentagon.terrain != pentagon.neighbors[0].terrain
                             && pentagon.terrain != pentagon.neighbors[1].terrain)
                    {
                        pentagon.terrain = pentagon.neighbors[5].terrain;
                    }
                }
				SetSprite(cell);
            }
        }
    }

    private void SetSprite(GameObject cell)
    {
        switch (cell.GetComponent<Pentagon>().terrain)
        {
            case Terrain.Water:
                cell.GetComponent<SpriteRenderer>().color = new Color(0f, 0.55f, 0.94f);
                break;
            case Terrain.Plain:
                cell.GetComponent<SpriteRenderer>().color = new Color(0.49f, 0.99f, 0f);
                break;
            case Terrain.Mountains:
                cell.GetComponent<SpriteRenderer>().color = new Color(0.55f, 0.55f, 0.48f);
                break;
            case Terrain.Desert:
                cell.GetComponent<SpriteRenderer>().color = new Color(0.99f, 0.87f, 0.46f);
                break;
            case Terrain.Forest:
                cell.GetComponent<SpriteRenderer>().color = new Color(0.13f, 0.55f, 0.13f);
                break;
        }
    }
}