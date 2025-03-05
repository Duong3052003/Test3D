using Cinemachine;
using UnityEngine;

public class VirtualCameraSetting : MonoBehaviour
{
    public static VirtualCameraSetting Instance { get; private set; }

    private CinemachineVirtualCamera virtualCamera;
    private float currentFOV;
    private float targetFOV;
    private float zoomSpeed = 2f;
    private bool isZooming = false;

    private Quaternion targetRotation;
    private float rotationSpeed = 5f;
    private bool isRotating = false;

    private Vector3 targetPosition;
    private float moveSpeed = 3f;
    private bool isMoving = false;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        currentFOV = virtualCamera.m_Lens.FieldOfView;
        targetFOV = currentFOV;
        targetRotation = transform.rotation;
        targetPosition = transform.position;
    }

    private void Update()
    {
        SmoothZoom();
        SmoothRotate();
        SmoothMove();
    }

    private void SmoothZoom()
    {
        if (!isZooming) return;

        currentFOV = Mathf.Lerp(currentFOV, targetFOV, Time.deltaTime * zoomSpeed);
        virtualCamera.m_Lens.FieldOfView = currentFOV;

        if (Mathf.Abs(currentFOV - targetFOV) < 0.1f)
        {
            virtualCamera.m_Lens.FieldOfView = targetFOV;
            isZooming = false;
        }
    }

    private void SmoothRotate()
    {
        if (!isRotating) return;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
        {
            transform.rotation = targetRotation;
            isRotating = false;
        }
    }

    private void SmoothMove()
    {
        if (!isMoving) return;

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);

        if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
        {
            transform.position = targetPosition;
            isMoving = false;
        }
    }

    public void ChangeFieldOfView(float newFOV)
    {
        targetFOV = newFOV;
        isZooming = true;
    }

    public void Rotate(Vector3 eulerAngles)
    {
        targetRotation = Quaternion.Euler(eulerAngles);
        isRotating = true;
    }

    public void MoveTo(Vector3 newPosition)
    {
        targetPosition = newPosition;
        isMoving = true;
    }

    public void FollowTarget(Transform target)
    {
        virtualCamera.Follow = target;
    }

    public void StopFollowTarget()
    {
        virtualCamera.Follow = null;
    }
}
