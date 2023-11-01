using UnityEngine;

public class Pentagon : MonoBehaviour
{
    public Vector2Int offsetCoordinates;
    public Vector2Int axialCoordinates;
    public Vector3Int cubicCoordinates;
    public PentaType pentaType;
    public Terrain terrain;
    public Pentagon[] neighbors;

    public void Init(int j, int i, PentaType type)
    {
        offsetCoordinates = new Vector2Int(j, i);
        int x = j - (i - i % 2) / 2;
        int z = i;
        int y = -x - z;
        axialCoordinates = new Vector2Int(x, z);
        cubicCoordinates = new Vector3Int(x, y, z);
        pentaType = type;
        neighbors = new Pentagon[6];
    }
}