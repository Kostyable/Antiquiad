using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    public Slider loadingSlider;
    public TextMeshProUGUI loadingText;
    public TextMeshProUGUI loadingHeader;
    public float dotInterval = 0.1f;
    private string _baseLoadingHeader = "Loading";

    void Start()
    {
        loadingHeader.text = _baseLoadingHeader;
        StartCoroutine(PlayLoadingDots());
        StartCoroutine(LoadAsyncScene());
    }

    IEnumerator LoadAsyncScene()
    {
        yield return new WaitForSeconds(dotInterval * 3);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Game");

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            loadingSlider.value = progress;
            loadingText.text = Mathf.Round(progress * 100f) + "%";
            yield return null;
        }
    }

    IEnumerator PlayLoadingDots()
    {
        while (true)
        {
            yield return new WaitForSeconds(dotInterval);
            loadingHeader.text = GetNextLoadingHeader(loadingHeader.text);
        }
    }

    string GetNextLoadingHeader(string currentHeader)
    {
        if (currentHeader.Length > _baseLoadingHeader.Length + 2)
            return _baseLoadingHeader;

        return currentHeader + ".";
    }
}