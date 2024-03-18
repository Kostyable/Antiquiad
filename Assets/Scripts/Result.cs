using UnityEngine;
using TMPro;

public class Result : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI result;
    
    void Start()
    {
        if (GameLogic.Result == true)
        {
            result.text = "YOU WIN!";
        }
        else if (GameLogic.Result == false)
        {
            result.text = "YOU LOSE!";
        }
        GameLogic.Result = null;
    }
}