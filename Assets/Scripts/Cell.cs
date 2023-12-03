using UnityEngine;

public class Cell : MonoBehaviour
{
    public Vector2Int offsetCoordinates;
    public Vector2Int axialCoordinates;
    public Vector3Int cubicCoordinates;
    public CellType cellType;
    public float alitude;
    public float latitude;
    public Terrain terrain;
    public Cell[] neighbors;
    public bool withRiver;

    public void Init(int j, int i, CellType type)
    {
        offsetCoordinates = new Vector2Int(j, i);
        int x = j - (i - i % 2) / 2;
        int z = i;
        int y = -x - z;
        axialCoordinates = new Vector2Int(x, z);
        cubicCoordinates = new Vector3Int(x, y, z);
        cellType = type;
        neighbors = new Cell[6];
    }
}