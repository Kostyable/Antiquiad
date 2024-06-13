using System.Collections;
using System.Collections.Generic;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private float typingSpeed;
    [SerializeField] private GameObject close;
    public TextMeshProUGUI headerText;
    public TextMeshProUGUI contentText;
    private RectTransform _rectTransform;
    private Queue<string> _messages;

    void Start()
    {
        _rectTransform = gameObject.GetComponent<RectTransform>();
        _messages = new Queue<string>();
    }

    public void StartDialogue(Tutorial tutorial)
    {
        _messages.Clear();
        foreach (string message in tutorial.messages)
        {
            _messages.Enqueue(message);
        }
        DisplayNextMessage();
    }

    public void DisplayNextMessage()
    {
        if (_messages.Count == 1)
        {
            gameObject.GetComponent<TextMeshProUGUI>().text = LocalizationManager.GetTranslation("back_to_menu");
            SetRectTransformValues(_rectTransform.anchoredPosition.x - 50, _rectTransform.anchoredPosition.y,
                _rectTransform.sizeDelta.x + 100, _rectTransform.sizeDelta.y);
            close.SetActive(true);
        }
        else if (_messages.Count == 0)
        {
            BackToMenu();
            return;
        }
        string message = _messages.Dequeue();
        headerText.text = LocalizationManager.GetTranslation(message);
        StopAllCoroutines();
        StartCoroutine(TypeSentence(message));
    }

    IEnumerator TypeSentence(string content)
    {
        Map.CurrentMessage = LocalizationManager.GetTranslation(content + "_desc");
        contentText.text = "";
        foreach (char letter in LocalizationManager.GetTranslation(content + "_desc"))
        {
            contentText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
    
    public void SetRectTransformValues(float newX, float newY, float newWidth, float newHeight)
    {
        Vector2 currentPosition = _rectTransform.anchoredPosition;
        Vector2 newSizeDelta = _rectTransform.sizeDelta;
        currentPosition.x = newX;
        currentPosition.y = newY;
        newSizeDelta.x = newWidth;
        newSizeDelta.y = newHeight;
        _rectTransform.anchoredPosition = currentPosition;
        _rectTransform.sizeDelta = newSizeDelta;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}