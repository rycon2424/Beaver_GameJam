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

    private PlayerAnimation playerAnim;
    private bool moving;

    private void Start()
    {
        playerAnim = GetComponent<PlayerAnimation>();
        playerAnim.SetRunning(false);
    }

    private void Update()
    {
        HandleAnimation();
        HandleMovement();
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

    private void LateUpdate()
    {
        FollowPlayerWithCamera();
    }

    private void HandleMovement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                playerAgent.SetDestination(hit.point);

                IInteractable interactable = hit.collider.gameObject.GetComponent<IInteractable>();

                if (interactable != null)
                {
                    if (Vector3.Distance(rayPoint.position, hit.collider.gameObject.transform.position) <= interactableRange)
                    {
                        playerAgent.SetDestination(playerAgent.transform.position);
                        interactable.OnInteract(transform);
                        return;
                    }
                }

                playerAgent.SetDestination(hit.point);
            }
        }
    }

    private void FollowPlayerWithCamera()
    {
        if (!playerAgent || !mainCamera)
            return;

        // Set desired position based on static offset (world-relative to player)
        Vector3 desiredPosition = playerAgent.transform.position + cameraOffset;
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Set fixed rotation
        mainCamera.transform.rotation = Quaternion.Euler(cameraRotationEuler);
    }
}
