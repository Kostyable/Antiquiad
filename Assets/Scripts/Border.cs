using UnityEngine;

public class Border
{
    public Vector2Int Coordinates;
    public Cell Cell1;
    public Cell Cell2;
    public float Alitude;
    public Border[] Neighbors;

    public Border(Vector2Int coordinates, Cell cell1, Cell cell2)
    {
        Coordinates = coordinates;
        Cell1 = cell1;
        Cell2 = cell2;
        Alitude = (cell1.alitude + cell2.alitude) / 2.0f;
        Neighbors = new Border[4];
    }
}