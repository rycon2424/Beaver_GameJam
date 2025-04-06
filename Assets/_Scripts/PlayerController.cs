using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [Header("Player and NavMesh")]
    public NavMeshAgent playerAgent;
    public Camera mainCamera;
    public Transform rayPoint;
    public float interactableRange = 1.5f;
    public GameObject walkDirectionIndicator;
    [SerializeField] float defaultMovementSpeed = 5.5f;
    [SerializeField] float woodDrag = 0.1f;

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
    [Space]
    [ReadOnly]
    public bool lockPlayer;

    private PlayerAnimation playerAnim;
    public bool moving;
    private IInteractable currentInteractableTarget;
    private Transform interactableNotInRange;

    private void Awake()
    {
        lockPlayer = true;
    }

    private void Start()
    {
        playerAnim = GetComponent<PlayerAnimation>();
        playerAnim.SetRunning(false);

        walkDirectionIndicator.transform.parent = null;
    }

    public void UpdateMovementSpeed(int logs)
    {
        if (logs == 0)
            playerAgent.speed = defaultMovementSpeed;
        else
            playerAgent.speed = defaultMovementSpeed * (1f - Math.Clamp(woodDrag * logs, 0.1f, 1));
    }

    private void Update()
    {
        if (lockPlayer)
            return;

        HandleAnimation();
        HandleMovement();
        HandleZoom(); // New: handle zooming
    }

    private void LateUpdate()
    {
        if (lockPlayer)
            return;

        FollowPlayerWithCamera();
    }

    private void HandleAnimation()
    {
        walkDirectionIndicator.transform.position = playerAgent.destination;

        if (moving == false && playerAgent.hasPath)
        {
            moving = true;
            playerAnim.SetRunning(true);
            walkDirectionIndicator.SetActive(true);
        }

        if (moving == true && playerAgent.hasPath == false)
        {
            moving = false;
            playerAnim.SetRunning(false);
            walkDirectionIndicator.SetActive(false);
        }
    }

    private void HandleMovement()
    {
        if (interactableNotInRange) // walk till in range
        {
            if (Vector3.Distance(rayPoint.position, interactableNotInRange.transform.position) <= interactableRange)
            {
                playerAgent.SetDestination(playerAgent.transform.position);
                currentInteractableTarget.OnInteract(transform);

                currentInteractableTarget = null;
                interactableNotInRange = null;
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
                    if (Vector3.Distance(rayPoint.position, hit.collider.gameObject.transform.position) <= interactableRange)
                    {
                        playerAgent.SetDestination(playerAgent.transform.position);
                        currentInteractableTarget.OnInteract(transform);
                    }
                    else
                    {
                        interactableNotInRange = hit.collider.gameObject.transform;
                    }

                    return;
                }

                interactableNotInRange = null;

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
