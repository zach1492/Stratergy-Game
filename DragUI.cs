using UnityEngine;
using UnityEngine.EventSystems;

public class DragUI : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    public RectTransform content;

    [Header("Drag Limits")]
    public Vector2 minPosition; // Minimum anchoredPosition
    public Vector2 maxPosition; // Maximum anchoredPosition

    [Header("Zoom")]
    public float zoomSpeedMouse = 0.1f;
    public float zoomSpeedTouch = 0.005f;
    public float minZoom = 0.5f;
    public float maxZoom = 2.5f;

    private Vector2 lastDragPos;

    void Awake()
    {
        if (content == null)
            content = GetComponent<RectTransform>();
    }

    // -------- DRAG --------
    public void OnBeginDrag(PointerEventData eventData)
    {
        lastDragPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 delta = eventData.position - lastDragPos;
        content.anchoredPosition += delta;
        lastDragPos = eventData.position;

        ClampPosition();
    }

    void ClampPosition()
    {
        Vector2 clampedPos = content.anchoredPosition;
        clampedPos.x = Mathf.Clamp(clampedPos.x, minPosition.x, maxPosition.x);
        clampedPos.y = Mathf.Clamp(clampedPos.y, minPosition.y, maxPosition.y);
        content.anchoredPosition = clampedPos;
    }

    // -------- ZOOM --------
    void Update()
    {
        HandleMouseZoom();
        HandleTouchZoom();
    }

    void HandleMouseZoom()
    {
        float scroll = Input.mouseScrollDelta.y;
        if (scroll != 0)
        {
            Zoom(scroll * zoomSpeedMouse);
        }
    }

    void HandleTouchZoom()
    {
        if (Input.touchCount == 2)
        {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            Vector2 prevPos0 = t0.position - t0.deltaPosition;
            Vector2 prevPos1 = t1.position - t1.deltaPosition;

            float prevDist = Vector2.Distance(prevPos0, prevPos1);
            float currentDist = Vector2.Distance(t0.position, t1.position);

            float diff = currentDist - prevDist;
            Zoom(diff * zoomSpeedTouch);
        }
    }

    void Zoom(float increment)
    {
        float scale = Mathf.Clamp(
            content.localScale.x + increment,
            minZoom,
            maxZoom
        );

        content.localScale = Vector3.one * scale;
    }
}
