using UnityEngine;
using TMPro;
using I2.Loc;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private GameObject uiCanvas;
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private TextMeshProUGUI result;
    [SerializeField] private TextMeshProUGUI winner;
    
    public void ShowResult(bool gameResult)
    {
        gameOverCanvas.SetActive(true);
        if (gameResult)
        {
            result.GetComponent<LocalizationParamsManager>().SetParameterValue
                ("RESULT", LocalizationManager.GetTranslation("win"));
        }
        else
        {
            result.GetComponent<LocalizationParamsManager>().SetParameterValue
                ("RESULT", LocalizationManager.GetTranslation("lose"));
        }
    }
}