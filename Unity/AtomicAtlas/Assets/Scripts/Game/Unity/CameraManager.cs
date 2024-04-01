
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public enum CameraMode
{
    PLANE,
    FULL_3D
}

public class CameraManager: MonoBehaviour
{
    public static CameraManager GlobalInstance
    {
        get;
        private set;
    }

    [SerializeField]
    private float maxCameraZ;

    [SerializeField]
    public float minCameraZ;

    [SerializeField]
    private EventSystem eventSystem;

    private float targetCameraZ = -1000.0f;
    private Camera mainCamera;
    private Vector3 lastDragWorldPos;
    private Vector3 lastCameraPos;
    private bool isDragging = false;
    private CameraMode cameraMode = CameraMode.PLANE;

    private const float INTERPOLATE_CAM_Z = 0.45f;
    private const float ZOOM_SENSITIVITY = 1.75f;
    private const float SIZE_DELTA = 0.001f;

    void Start()
    {
        GlobalInstance = this;

        mainCamera = Camera.main;
        lastCameraPos = mainCamera.transform.position;
    }

    void Update()
    {
        //if (Mathf.Abs(mainCamera.orthographicSize - targetSize) < SIZE_DELTA)
        //{
        //    return;
        //}

        if (Mathf.Abs(mainCamera.transform.position.z - targetCameraZ) < SIZE_DELTA)
        {
            return;
        }

        var cameraZ = Mathf.SmoothStep(mainCamera.transform.position.z, targetCameraZ, INTERPOLATE_CAM_Z);
        mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, cameraZ);
        //mainCamera.orthographicSize = Mathf.SmoothStep(mainCamera.orthographicSize, targetSize, INTERPOLATE_CAM);
    }

    public void OnGUI()
    {
        if (cameraMode == CameraMode.PLANE)
        {
            if (Event.current.type == EventType.ScrollWheel)
            {
                if (!eventSystem.IsPointerOverGameObject())
                {
                    MouseScroll(Event.current.delta.y);
                }
            }

            if (Event.current.type == EventType.MouseDrag)
            {
                if (!eventSystem.IsPointerOverGameObject())
                {
                    Vector3 mousePos = Event.current.mousePosition;
                    mousePos.y = mainCamera.pixelHeight - mousePos.y;
                    mousePos.z = Mathf.Abs(mainCamera.transform.position.z);

                    MouseDrag(mousePos);
                }
            }

            if (Event.current.type == EventType.MouseUp)
            {
                isDragging = false;
            }
        }
        else
        {
            // 3d controls
        }
    }

    private void MouseDrag(Vector3 mousePos)
    {
        if (!isDragging)
        {
            isDragging = true;
            lastDragWorldPos = mainCamera.ScreenToWorldPoint(mousePos);
        }

        if (lastCameraPos != mainCamera.transform.position)
        {
            lastDragWorldPos += mainCamera.transform.position - lastCameraPos;
        }

        var nextPos = mainCamera.ScreenToWorldPoint(mousePos);
        var delta = lastDragWorldPos - nextPos;
        var newPos = mainCamera.transform.position + new Vector3(delta.x, delta.y, 0);

        mainCamera.transform.position = newPos;
        lastCameraPos = newPos;
    }

    private void MouseScroll(float delta)
    {
        targetCameraZ = targetCameraZ - delta * ZOOM_SENSITIVITY;

        if (targetCameraZ > maxCameraZ)
        {
            targetCameraZ = maxCameraZ;
        }

        if (targetCameraZ < minCameraZ)
        {
            targetCameraZ = minCameraZ;
        }
    }
}
