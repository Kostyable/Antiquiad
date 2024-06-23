using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Map map;
    private Camera _mainCamera;
    private Vector2 _lastTouchPosition;
    private bool _isPinching;
    private float _initialPinchDistance;
    private Vector3 _lastMousePosition;
    private Vector2 _lastTouchPosition0;
    private Vector2 _lastTouchPosition1;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float dragSpeed;
    [SerializeField] private float minZoom;
    private float _maxZoom;
    private float _minX;
    private float _maxX;
    private float _minY;
    private float _maxY;
    [SerializeField] private RectTransform topInterfaceElement;
    [SerializeField] private RectTransform bottomInterfaceElement;

    private void Awake()
    {
        _mainCamera = Camera.main;
        zoomSpeed = 1f;
        dragSpeed = 2f;
        minZoom = 5f;
        if (_mainCamera != null) _mainCamera.orthographicSize = minZoom;
    }
    
    private void Start()
    {
        _minX = Map.MinX;
        _maxX = Map.MaxX;
        _minY = Map.MinY;
        _maxY = Map.MaxY;
    }
    
    private void Update()
    {
        if (Map.MenuEnabled)
        {
            return;
        }
        CalculateMaxZoomValue();
        HandleZoom();
        Vector2 topInterfaceSize = GetWorldSize(topInterfaceElement);
        Vector2 bottomInterfaceSize = GetWorldSize(bottomInterfaceElement);
        _maxY = Map.MaxY + topInterfaceSize.y;
        _minY = Map.MinY - bottomInterfaceSize.y;
    }
    
    private void LateUpdate()
    {
        if (Map.MenuEnabled)
        {
            return;
        }
        HandleDrag();
        LimitCameraToBounds();
    }
    
    private bool IsCursorInsideScreenBounds(Vector3 cursorPosition)
    {
        return cursorPosition.x >= 0 && cursorPosition.x <= Screen.width &&
               cursorPosition.y >= 0 && cursorPosition.y <= Screen.height;
    }
    
    private Vector2 GetWorldSize(RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        float width = Mathf.Abs(corners[2].x - corners[0].x);
        float height = Mathf.Abs(corners[2].y - corners[0].y);

        return new Vector2(width, height);
    }

    private void CalculateMaxZoomValue()
    {
        float cameraWidth = Mathf.Abs(_maxX - _minX);
        float cameraHeight = Mathf.Abs(_maxY - _minY);

        float maxZoomX = cameraWidth / 2f / _mainCamera.aspect;
        float maxZoomY = cameraHeight / 2f;

        _maxZoom = Mathf.Min(maxZoomX, maxZoomY);
    }

    private void HandleZoom()
    {
        if (!map.IsMouseOverUI() && IsCursorInsideScreenBounds(Input.mousePosition))
        {
            if (Application.platform == RuntimePlatform.WindowsPlayer
                || Application.platform == RuntimePlatform.WindowsEditor)
            {
                float zoomDelta = Input.mouseScrollDelta.y * zoomSpeed;
                float newZoom = Mathf.Clamp(_mainCamera.orthographicSize - zoomDelta, minZoom, _maxZoom);
                Vector3 mousePosition = Input.mousePosition;
                Vector3 mouseWorldPosition
                    = _mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y,
                        _mainCamera.nearClipPlane));
                Vector3 zoomOffset = (mouseWorldPosition - transform.position)
                                     * (newZoom / _mainCamera.orthographicSize - 1f);
                _mainCamera.orthographicSize = newZoom;
                transform.position -= zoomOffset;
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                if (Input.touchCount == 2)
                {
                    Touch touch0 = Input.GetTouch(0);
                    Touch touch1 = Input.GetTouch(1);
                    if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began)
                    {
                        _isPinching = true;
                        _initialPinchDistance = Vector2.Distance(touch0.position, touch1.position);
                    }
                    if (_isPinching && (touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved))
                    {
                        float currentPinchDistance = Vector2.Distance(touch0.position, touch1.position);
                        float deltaDistance = currentPinchDistance - _initialPinchDistance;
                        float zoomDelta = deltaDistance * zoomSpeed * Time.deltaTime;
                        float newZoom = Mathf.Clamp(_mainCamera.orthographicSize - zoomDelta, minZoom, _maxZoom);
                        Vector2 avgTouchPos = (touch0.position + touch1.position) * 0.5f;
                        Vector3 touchWorldPosition
                            = _mainCamera.ScreenToWorldPoint(new Vector3(avgTouchPos.x,
                                avgTouchPos.y, _mainCamera.nearClipPlane));
                        Vector3 zoomOffset = (touchWorldPosition - transform.position)
                                             * (newZoom / _mainCamera.orthographicSize - 1f);
                        _mainCamera.orthographicSize = newZoom;
                        transform.position -= zoomOffset;
                        _initialPinchDistance = currentPinchDistance;
                    }
                }
                else
                {
                    _isPinching = false;
                }
            }
        }
    }

    private void HandleDrag()
    {
        if (!map.IsMouseOverUI() && IsCursorInsideScreenBounds(Input.mousePosition))
        {
            if (Application.platform == RuntimePlatform.WindowsPlayer
                || Application.platform == RuntimePlatform.WindowsEditor)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _lastMousePosition = Input.mousePosition;
                }
                else if (Input.GetMouseButton(0))
                {
                    Vector3 mouseDelta = Input.mousePosition - _lastMousePosition;

                    float dragX = -mouseDelta.x * dragSpeed * _mainCamera.orthographicSize / Screen.height;
                    float dragY = -mouseDelta.y * dragSpeed * _mainCamera.orthographicSize / Screen.height;

                    float newX = Mathf.Clamp(transform.position.x + dragX, _minX, _maxX);
                    float newY = Mathf.Clamp(transform.position.y + dragY, _minY, _maxY);

                    transform.position = new Vector3(newX, newY, transform.position.z);

                    _lastMousePosition = Input.mousePosition;
                }
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                if (Input.touchCount == 1)
                {
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Began)
                    {
                        _lastTouchPosition = touch.position;
                    }
                    else if (touch.phase == TouchPhase.Moved)
                    {
                        Vector2 touchDelta = touch.position - _lastTouchPosition;
                        float dragX = -touchDelta.x * dragSpeed * _mainCamera.orthographicSize / Screen.height;
                        float dragY = -touchDelta.y * dragSpeed * _mainCamera.orthographicSize / Screen.height;
                        float newX = Mathf.Clamp(transform.position.x + dragX, _minX, _maxX);
                        float newY = Mathf.Clamp(transform.position.y + dragY, _minY, _maxY);
                        transform.position = new Vector3(newX, newY, transform.position.z);
                        _lastTouchPosition = touch.position;
                    }
                }
            }
        }
    }

    private void LimitCameraToBounds()
    {
        float cameraHalfWidth = _mainCamera.orthographicSize * _mainCamera.aspect;
        float cameraHalfHeight = _mainCamera.orthographicSize;
        float clampedX = Mathf.Clamp(transform.position.x, _minX + cameraHalfWidth,
            _maxX - cameraHalfWidth);
        float clampedY = Mathf.Clamp(transform.position.y, _minY + cameraHalfHeight,
            _maxY - cameraHalfHeight);
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}