using UnityEngine;
using TMPro;

public class Result : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI result;
    void Start()
    {
        result.text = PlayerPrefs.GetString("Result");
        PlayerPrefs.DeleteKey("Result");
        PlayerPrefs.Save();
    }
}
