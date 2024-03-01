using UnityEngine;

public class Border
{
    public Vector2Int Coordinates;
    public Cell Cell1;
    public Cell Cell2;
    public float Altitude;
    public Border[] Neighbors;

    public Border(Vector2Int coordinates, Cell cell1, Cell cell2)
    {
        Coordinates = coordinates;
        Cell1 = cell1;
        Cell2 = cell2;
        Altitude = (cell1.altitude + cell2.altitude) / 2.0f;
        Neighbors = new Border[4];
    }
}