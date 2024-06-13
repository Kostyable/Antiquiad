using UnityEngine;

public class River : MonoBehaviour
{
    public int id;
    public Vector2Int coordinates;
    public Cell cell1;
    public Cell cell2;
    public River[] neighbors;

    public RiverData CollectRiverData()
    {
        RiverData data = new RiverData();
        data.id = id;
        data.coordinates = coordinates;
        return data;
    }
    
    public void Init(int riverID, Border border)
    {
        id = riverID;
        coordinates = border.Coordinates;
        cell1 = border.Cell1;
        cell2 = border.Cell2;
        neighbors = new River[4];
    }
    
    public void Init(RiverData riverData)
    {
        id = riverData.id;
        coordinates = riverData.coordinates;
        neighbors = new River[4];
    }
    
    public void SetDefaultColor()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0.1f, 0.45f, 0.82f);
    }
    
    public void SetFogOfWarColor()
    {
        Color currentColor = new Color(0.1f, 0.45f, 0.82f);
        Color grayColor = new Color(0.25f, 0.25f, 0.25f);
        gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp(currentColor, grayColor, 0.5f);
    }
}