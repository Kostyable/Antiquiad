using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private GameObject newGameCanvas;

    public void ShowNewGame()
    {
        mainMenuCanvas.SetActive(false);
        newGameCanvas.SetActive(true);
    }
    
    public void ShowMainMenu()
    {
        newGameCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);
    }
}