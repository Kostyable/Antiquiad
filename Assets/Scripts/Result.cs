using UnityEngine;
using TMPro;
using I2.Loc;

public class Result : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI result;
    void Start()
    {
        if (GameLogic.Result == true)
        {
            result.GetComponent<LocalizationParamsManager>().SetParameterValue
                ("RESULT", LocalizationManager.GetTranslation("win"));
        }
        else if (GameLogic.Result == false)
        {
            result.GetComponent<LocalizationParamsManager>().SetParameterValue
                ("RESULT", LocalizationManager.GetTranslation("lose"));
        }
        GameLogic.Result = null;
    }
}