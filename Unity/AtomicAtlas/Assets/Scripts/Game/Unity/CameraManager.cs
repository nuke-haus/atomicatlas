
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraManager: MonoBehaviour
{
    public static CameraManager GlobalInstance
    {
        get;
        private set;
    }

    [SerializeField]
    private float maxSize;

    [SerializeField]
    public float minSize;

    [SerializeField]
    private EventSystem eventSystem;

    private float targetSize = 5.0f;
    private Camera mainCamera;
    private Vector2 lastPos;
    private bool isDragging;
    private Vector2 dragWorldPos;
    private Vector2 prevCameraPos;

    private const float INTERPOLATE_CAM = 0.48f;
    private const float SENSITIVITY = 0.20f;
    private const float SIZE_DELTA = 0.001f;

    void Start()
    {
        GlobalInstance = this;

        mainCamera = Camera.main;
        lastPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void Update()
    {
        if (Mathf.Abs(mainCamera.orthographicSize - targetSize) < SIZE_DELTA)
        {
            return;
        }

        var size = mainCamera.orthographicSize;
        mainCamera.orthographicSize = Mathf.SmoothStep(mainCamera.orthographicSize, targetSize, INTERPOLATE_CAM);
    }

    public void OnGUI()
    {
        if (Event.current.type == EventType.ScrollWheel)
        {
            if (!eventSystem.IsPointerOverGameObject())
            {
                MouseScroll(Event.current.delta.y);
            }
        }

        if (Event.current.type == EventType.MouseDown)
        {
            // Don't drag if we are over a UI element like a button.
            isDragging = !eventSystem.IsPointerOverGameObject();
            dragWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            prevCameraPos = mainCamera.transform.position;
        }

        if (Event.current.type == EventType.MouseDrag)
        {
            if (isDragging) MouseDrag();
        }
    }

    private void MouseDrag()
    {
        lastPos = Camera.main.transform.position;

        if (prevCameraPos != lastPos)
        {
            dragWorldPos += (lastPos - prevCameraPos);
        }

        Vector2 next_pos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        var delta = dragWorldPos - next_pos;
        var pos = mainCamera.transform.position;
        var newpos = pos + new Vector3(delta.x, delta.y, 0);

        mainCamera.transform.position = newpos;
        prevCameraPos = mainCamera.transform.position;
    }

    private void MouseScroll(float delta)
    {
        targetSize = targetSize + delta * SENSITIVITY;
        lastPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (targetSize > maxSize)
        {
            targetSize = maxSize;
        }

        if (targetSize < minSize)
        {
            targetSize = minSize;
        }
    }
}
