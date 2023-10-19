using UnityEngine;

public class Pentagon : MonoBehaviour
{
    public Vector2Int axialCoordinates;
    public Vector2 cubicCoordinates;
    public Terrain terrain;
    public PentaType pentaType;
    public Pentagon[] neighbors;

    public void Init(Vector2Int coord, PentaType type)
    {
        axialCoordinates = coord;
        pentaType = type;
        cubicCoordinates =
            new Vector3(axialCoordinates.x, axialCoordinates.y, 0 - axialCoordinates.x - axialCoordinates.y);
    }
}