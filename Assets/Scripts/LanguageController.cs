using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

public class LanguageController : MonoBehaviour
{
    [SerializeField] private Sprite enFlag;
    [SerializeField] private Sprite ruFlag;
    [SerializeField] private Image currentFlag;
    
    private void Start()
    {
        switch (LocalizationManager.CurrentLanguageCode)
        {
            case "en":
                currentFlag.sprite = ruFlag;
                break;
            case "ru":
                currentFlag.sprite = enFlag;
                break;
        }
    }

    public void ChangeLanguage()
    {
        switch (LocalizationManager.CurrentLanguageCode)
        {
            case "en":
                LocalizationManager.CurrentLanguageCode = "ru";
                currentFlag.sprite = enFlag;
                break;
            case "ru":
                LocalizationManager.CurrentLanguageCode = "en";
                currentFlag.sprite = ruFlag;
                break;
        }
    }
}