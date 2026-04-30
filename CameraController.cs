
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    //public boolCamera isMoveable

    public float moveSpeed = 5f;
    public float dragSensitivity = 0.002f;
    public float zoomSpeed = 5f;
    public float zoomAffectFactor = 5f;

    public float damping = 8f; // higher = stops faster

    private Vector3 previousMousePosition;
    private Vector3 velocity;

    // Cameras
    public Camera mainCamera;
    public Camera childCamera;
    public Camera additionalCamera1;
    public Camera additionalCamera2;
    public Camera additionalCamera3;

    // --- Map Bounds ---
    private float minX, maxX, minZ, maxZ;
    private float hexWidth = 1.732f;
    private float hexHeight = 2f;

    public void SetMapBounds(int width, int height)
    {
        maxZ = (height - 1) * (hexHeight * 0.75f) + 10;
        maxX = (width - 1) * hexWidth + hexWidth / 2f + 10;

        minX = -10f;
        minZ = -10f;
    }

    void Start()
    {
        ApplyCullingMasks();
    }

    void ApplyCullingMasks()
    {
        mainCamera.cullingMask = LayerMask.GetMask("Default");
        childCamera.cullingMask = LayerMask.GetMask("MainLayer");
        additionalCamera1.cullingMask = LayerMask.GetMask("UILayer");
        additionalCamera2.cullingMask = LayerMask.GetMask("TroopLayer");
        additionalCamera3.cullingMask = LayerMask.GetMask("nLayer");

    }

    void Update()
    {
        if (!IsPointerOverUI())
        {
            HandleMouseInput();
            HandleTouchInput();
        }

        // Apply momentum
        transform.Translate(velocity * Time.deltaTime, Space.World);

        // Damping
        velocity = Vector3.Lerp(velocity, Vector3.zero, damping * Time.deltaTime);

        ClampCamera();
    }

    // --------------------- MOUSE INPUT ---------------------
    void HandleMouseInput()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - previousMousePosition;

            float zoomFactor = mainCamera.orthographicSize * zoomAffectFactor;

            Vector3 impulse = new Vector3(
                -delta.x * dragSensitivity * zoomFactor,
                0,
                -delta.y * dragSensitivity * zoomFactor
            );

            velocity += impulse * moveSpeed;
        }

        previousMousePosition = Input.mousePosition;

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        AdjustZoom(-scroll * zoomSpeed);
    }
bool IsPointerOverUI()
{
    if (EventSystem.current == null) return false;

    if (Input.touchCount > 0)
    {
        return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
    }

    return EventSystem.current.IsPointerOverGameObject();
}
    // --------------------- TOUCH INPUT ---------------------
    void HandleTouchInput()
    {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector2 delta = Input.GetTouch(0).deltaPosition;
            Vector3 impulse = new Vector3(-delta.x, 0, -delta.y);
            velocity += impulse * dragSensitivity * moveSpeed;
        }

        if (Input.touchCount == 2)
        {
            Touch a = Input.GetTouch(0);
            Touch b = Input.GetTouch(1);

            float prevMag = (a.position - a.deltaPosition - (b.position - b.deltaPosition)).magnitude;
            float newMag = (a.position - b.position).magnitude;

            float diff = prevMag - newMag;
            AdjustZoom(diff * zoomSpeed * Time.deltaTime);
        }
    }

    // --------------------- ZOOM ---------------------
    void AdjustZoom(float deltaZoom)
    {
        float newSize = Mathf.Clamp(mainCamera.orthographicSize + deltaZoom, 2f, 20f);

        mainCamera.orthographicSize = newSize;
        childCamera.orthographicSize = newSize;
        additionalCamera1.orthographicSize = newSize;
        additionalCamera2.orthographicSize = newSize;
        additionalCamera3.orthographicSize = newSize;

    }

    // --------------------- CLAMP ---------------------
    void ClampCamera()
    {
        float camHeight = mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;

        float clampedX = Mathf.Clamp(transform.position.x, minX + camWidth, maxX - camWidth);
        float clampedZ = Mathf.Clamp(transform.position.z, minZ + camHeight, maxZ - camHeight);

        transform.position = new Vector3(clampedX, transform.position.y, clampedZ);

        // Kill velocity when hitting bounds
        if (transform.position.x != clampedX) velocity.x = 0;
        if (transform.position.z != clampedZ) velocity.z = 0;
    }

    // --------------------- FOCUS ---------------------
    public void FocusOnCapital(Tile capitalTile)
    {
        velocity = Vector3.zero;

        Vector3 pos = new Vector3(
            capitalTile.x * hexWidth,
            transform.position.y,
            capitalTile.y * (hexHeight * 0.75f)
        );

        transform.position = pos;
        ClampCamera();
    }
}
