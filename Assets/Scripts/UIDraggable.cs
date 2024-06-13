using UnityEngine;
using UnityEngine.EventSystems;

public class UIDraggable : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private RectTransform _rectTransform;
    private bool _isDragging;
    private Vector2 _offset;

    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _isDragging = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, eventData.position,
            eventData.pressEventCamera, out _offset);
        _isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_isDragging)
        {
            Vector2 localPointerPosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform.parent as RectTransform,
                    eventData.position, eventData.pressEventCamera, out localPointerPosition))
            {
                _rectTransform.localPosition = localPointerPosition - _offset;
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isDragging = false;
    }
}