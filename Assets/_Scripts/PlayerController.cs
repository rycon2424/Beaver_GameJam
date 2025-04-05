using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [Header("Player and NavMesh")]
    public NavMeshAgent playerAgent;
    public Camera mainCamera;
    public Transform rayPoint;
    public float interactableRange = 1.5f;

    [Header("Camera Offset Settings")]
    public Vector3 cameraOffset = new Vector3(0, 10f, -8f);        // offset from player
    public Vector3 cameraRotationEuler = new Vector3(45f, 0f, 0f); // rotation in degrees

    [Header("Camera Follow Smoothing")]
    public float smoothSpeed = 5f;

    [Header("Camera Zoom Settings")]
    public float zoomSpeed = 2f;
    public float minZoomY = 5f;
    public float maxZoomY = 20f;
    [Space]
    public LayerMask cameraRayMask;

    private PlayerAnimation playerAnim;
    private bool moving;
    private IInteractable currentInteractableTarget;
    private Transform interactablePosition;

    private void Start()
    {
        playerAnim = GetComponent<PlayerAnimation>();
        playerAnim.SetRunning(false);
    }

    private void Update()
    {
        HandleAnimation();
        HandleMovement();
        HandleZoom(); // New: handle zooming
    }

    private void LateUpdate()
    {
        FollowPlayerWithCamera();
    }

    private void HandleAnimation()
    {
        if (moving == false && playerAgent.hasPath)
        {
            moving = true;
            playerAnim.SetRunning(true);
        }

        if (moving == true && playerAgent.hasPath == false)
        {
            moving = false;
            playerAnim.SetRunning(false);
        }
    }

    private void HandleMovement()
    {
        if (interactablePosition)
        {
            if (Vector3.Distance(rayPoint.position, interactablePosition.transform.position) <= interactableRange)
            {
                playerAgent.SetDestination(playerAgent.transform.position);
                currentInteractableTarget.OnInteract(transform);

                currentInteractableTarget = null;
                interactablePosition = null;
            }
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, cameraRayMask))
            {
                playerAgent.SetDestination(hit.point);

                currentInteractableTarget = hit.collider.gameObject.GetComponent<IInteractable>();

                if (currentInteractableTarget != null)
                {
                    interactablePosition = hit.collider.gameObject.transform;
                    
                    if (Vector3.Distance(rayPoint.position, hit.collider.gameObject.transform.position) <= interactableRange)
                    {
                        playerAgent.SetDestination(playerAgent.transform.position);
                        currentInteractableTarget.OnInteract(transform);
                    }
                    
                    return;
                }

                interactablePosition = null;

                playerAgent.SetDestination(hit.point);
            }
        }
    }

    private void HandleZoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Abs(scrollInput) > 0.01f)
        {
            cameraOffset.y -= scrollInput * zoomSpeed;
            cameraOffset.z += scrollInput * zoomSpeed; // optional: maintain angle

            cameraOffset.y = Mathf.Clamp(cameraOffset.y, minZoomY, maxZoomY);
            cameraOffset.z = Mathf.Clamp(cameraOffset.z, -maxZoomY, -minZoomY); // adjust z clamp as needed
        }
    }

    private void FollowPlayerWithCamera()
    {
        if (!playerAgent || !mainCamera)
            return;

        Vector3 desiredPosition = playerAgent.transform.position + cameraOffset;
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        mainCamera.transform.rotation = Quaternion.Euler(cameraRotationEuler);
    }
}
