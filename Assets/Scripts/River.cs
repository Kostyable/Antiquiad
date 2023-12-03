using UnityEngine;

public class River : MonoBehaviour
{
    public int id;
    public Vector2Int coordinates;
    public Cell cell1;
    public Cell cell2;
    public float alitude;
    public River[] neighbors;

    public void Init(int riverID, Border border)
    {
        id = riverID;
        coordinates = border.Coordinates;
        cell1 = border.Cell1;
        cell2 = border.Cell2;
        alitude = border.Alitude;
        neighbors = new River[4];
    }
}