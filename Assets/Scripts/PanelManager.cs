using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] uiPrefabs;
    [SerializeField] private Sprite[] resourcesSprites;
    private float _panelWidth;
    private RectTransform _panelRect;
    private HorizontalLayoutGroup _layoutGroup;

    private void Start()
    {
        _layoutGroup = GetComponent<HorizontalLayoutGroup>();
        _panelRect = GetComponent<RectTransform>();
        _panelWidth = -_layoutGroup.spacing;
    }

    public void AddItems(params int[] indexes)
    {
        for (int i = 0; i < indexes.Length; i++)
        {
            GameObject prefab = uiPrefabs[indexes[i]];
            GameObject item = Instantiate(prefab, transform);
            RectTransform itemRect = item.GetComponent<RectTransform>();
            _panelWidth += itemRect.rect.width + _layoutGroup.spacing;
            if (indexes[i] <= 21)
            {
                item = Instantiate(uiPrefabs[22], transform);
                itemRect = item.GetComponent<RectTransform>();
                _panelWidth += itemRect.rect.width + _layoutGroup.spacing;
                Dictionary<ResourceType, int> resourcesCosts = new Dictionary<ResourceType, int>();
                if (indexes[i] <= 11)
                {
                    foreach (ResourceType resource in GameLogic.UnitsCosts[(UnitType)indexes[i]].Keys)
                    {
                        if (GameLogic.UnitsCosts[(UnitType)indexes[i]][resource] != 0)
                        {
                            resourcesCosts.Add(resource, GameLogic.UnitsCosts[(UnitType)indexes[i]][resource]);
                        }
                    }
                }
                else
                {
                    foreach (ResourceType resource in GameLogic.BuildingsCosts[(BuildingType)(indexes[i] - 12)].Keys)
                    {
                        if (GameLogic.BuildingsCosts[(BuildingType)(indexes[i] - 12)][resource] != 0)
                        {
                            resourcesCosts.Add(resource,
                                GameLogic.BuildingsCosts[(BuildingType)(indexes[i] - 12)][resource]);
                        }
                    }
                }
                if (resourcesCosts.Count >= 1)
                {
                    item.transform.GetChild(0).GetComponent<Image>().sprite
                        = resourcesSprites[(int) resourcesCosts.ElementAt(0).Key];
                    item.transform.GetChild(0).GetComponent<Image>().color = Color.white;
                    item.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text
                        = resourcesCosts.ElementAt(0).Value.ToString();
                    if (GameLogic.Civs[0].ResourcesValues[resourcesCosts.ElementAt(0).Key]
                        < resourcesCosts.ElementAt(0).Value)
                    {
                        item.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.red;
                    }
                    else
                    {
                        item.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.green;
                    }
                }
                if (resourcesCosts.Count >= 2)
                {
                    item.transform.GetChild(2).GetComponent<Image>().sprite
                        = resourcesSprites[(int) resourcesCosts.ElementAt(1).Key];
                    item.transform.GetChild(2).GetComponent<Image>().color = Color.white;
                    item.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text
                        = resourcesCosts.ElementAt(1).Value.ToString();
                    if (GameLogic.Civs[0].ResourcesValues[resourcesCosts.ElementAt(1).Key]
                        < resourcesCosts.ElementAt(1).Value)
                    {
                        item.transform.GetChild(3).GetComponent<TextMeshProUGUI>().color = Color.red;
                    }
                    else
                    {
                        item.transform.GetChild(3).GetComponent<TextMeshProUGUI>().color = Color.green;
                    }
                }
                if (resourcesCosts.Count >= 3)
                {
                    item.transform.GetChild(4).GetComponent<Image>().sprite
                        = resourcesSprites[(int) resourcesCosts.ElementAt(2).Key];
                    item.transform.GetChild(4).GetComponent<Image>().color = Color.white;
                    item.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text
                        = resourcesCosts.ElementAt(2).Value.ToString();
                    if (GameLogic.Civs[0].ResourcesValues[resourcesCosts.ElementAt(2).Key]
                        < resourcesCosts.ElementAt(2).Value)
                    {
                        item.transform.GetChild(5).GetComponent<TextMeshProUGUI>().color = Color.red;
                    }
                    else
                    {
                        item.transform.GetChild(5).GetComponent<TextMeshProUGUI>().color = Color.green;
                    }
                }
                if (resourcesCosts.Count >= 4)
                {
                    item.transform.GetChild(6).GetComponent<Image>().sprite
                        = resourcesSprites[(int) resourcesCosts.ElementAt(3).Key];
                    item.transform.GetChild(6).GetComponent<Image>().color = Color.white;
                    item.transform.GetChild(7).GetComponent<TextMeshProUGUI>().text
                        = resourcesCosts.ElementAt(3).Value.ToString();
                    if (GameLogic.Civs[0].ResourcesValues[resourcesCosts.ElementAt(3).Key]
                        < resourcesCosts.ElementAt(3).Value)
                    {
                        item.transform.GetChild(7).GetComponent<TextMeshProUGUI>().color = Color.red;
                    }
                    else
                    {
                        item.transform.GetChild(7).GetComponent<TextMeshProUGUI>().color = Color.green;
                    }
                }
            }
        }
        if (_panelRect != null)
        {
            _panelRect.sizeDelta = new Vector2(_panelWidth, _panelRect.sizeDelta.y);
        }
    }
    
    public void Clear()
    {
        if (transform != null)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
        _panelWidth = 0f;
        if (_panelRect != null)
        {
            _panelRect.sizeDelta = new Vector2(_panelWidth, _panelRect.sizeDelta.y);
        }
    }
}