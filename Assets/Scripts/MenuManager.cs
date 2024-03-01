using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenuCanvas;
    public GameObject newGameCanvas;

    public void ShowNewGame()
    {
        mainMenuCanvas.SetActive(false);
        newGameCanvas.SetActive(true);
    }
}