using UnityEngine;

public class CenterUIOnEnable : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Camera originalCamera; // Camera to switch back to when the UI is disabled
    [SerializeField] private Camera targetCamera;   // Camera to switch to when the UI is enabled

    [Header("UI Settings")]
    [SerializeField] private RectTransform uiElement; // UI Element triggering the behavior

    [Header("Player Settings")]
    [SerializeField] private GameObject playerObject; // Player GameObject with movement script

    private MonoBehaviour movementScript; // Cached movement script on the player
    private Animator playerAnimator;      // Cached Animator component on the player

    private void Awake()
    {
        // Validate camera fields
        if (originalCamera == null)
        {
            Debug.LogError("Original camera is not assigned. Please assign it in the inspector.");
        }

        if (targetCamera == null)
        {
            Debug.LogError("Target camera is not assigned. Please assign it in the inspector.");
        }

        // Validate UI element
        if (uiElement == null)
        {
            Debug.LogError("UI Element is not assigned.");
        }

        // Validate player object and cache the movement script and animator
        if (playerObject == null)
        {
            Debug.LogError("Player object is not assigned.");
        }
        else
        {
            movementScript = playerObject.GetComponent<MonoBehaviour>();
            if (movementScript == null)
            {
                Debug.LogError("No movement script found on the player object!");
            }

            playerAnimator = playerObject.GetComponent<Animator>();
            if (playerAnimator == null)
            {
                Debug.LogWarning("No Animator component found on the player object!");
            }
        }
    }

    private void OnEnable()
    {
        // Switch to the target camera
        if (targetCamera != null)
        {
            ActivateCamera(targetCamera);
        }

        // Freeze player movement and animations
        FreezePlayer();
    }

    private void OnDisable()
    {
        // Restore the original camera
        if (originalCamera != null)
        {
            ActivateCamera(originalCamera);
        }

        // Unfreeze player movement and animations
        UnfreezePlayer();
    }

    private void ActivateCamera(Camera cameraToActivate)
    {
        if (cameraToActivate != null)
        {
            // Deactivate all cameras
            Camera[] allCameras = FindObjectsOfType<Camera>();
            foreach (Camera cam in allCameras)
            {
                cam.gameObject.SetActive(false);
            }

            // Activate the selected camera
            cameraToActivate.gameObject.SetActive(true);
        }
    }

    private void FreezePlayer()
    {
        if (playerObject != null)
        {
            // Disable movement script
            var playerController = playerObject.GetComponent<LittlesmithMovement>(); // Replace with your actual movement script class name
            if (playerController != null)
            {
                playerController.enabled = false;
            }
            else
            {
                Debug.LogError("Movement script not found on player object!");
            }

            // Disable Animator
            if (playerAnimator != null)
            {
                playerAnimator.enabled = false;
            }
        }
    }

    private void UnfreezePlayer()
    {
        if (playerObject != null)
        {
            // Re-enable movement script
            var playerController = playerObject.GetComponent<LittlesmithMovement>(); // Replace with your actual movement script class name
            if (playerController != null)
            {
                playerController.enabled = true;
            }

            // Re-enable Animator
            if (playerAnimator != null)
            {
                playerAnimator.enabled = true;
            }
        }
    }
}
