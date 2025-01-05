using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittlesmithMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float rotaSpeed = 5f;
    [SerializeField] private GameObject triggerObject; // Assign specific game object in Inspector
    [SerializeField] private LayerMask groundLayer; // Ground layer for ground check
    [SerializeField] private float groundCheckDistance = 0.1f; // Ground check distance
    [SerializeField] private Transform cameraTransform; // Assign camera transform in Inspector
    [SerializeField] private LayerMask obstructionLayer; // Layer for objects to hide
    [SerializeField] private float transparency = 0.2f; // Opacity level for obstructions

    private Animator animator;
    private Rigidbody rb;
    private float vertical;
    private float horizontal;
    private bool isHolding = false;
    private bool isGrounded = false;
    private List<GameObject> hiddenObjects = new List<GameObject>();
    private Dictionary<GameObject, Material[]> originalMaterials = new Dictionary<GameObject, Material[]>();

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate; // Smooth movement
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // Better collision detection
    }

    void Update()
    {
        InputHandler();
        CheckGround();
        CheckObstructions();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void InputHandler()
    {
        vertical = -Input.GetAxis("Vertical");
        horizontal = -Input.GetAxis("Horizontal");
    }

    private void Movement()
    {
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        Vector3 movement = new Vector3(horizontal, 0, vertical).normalized * currentSpeed;
        Vector3 velocity = rb.velocity;

        // Apply horizontal movement while preserving gravity
        rb.velocity = new Vector3(movement.x, velocity.y, movement.z);

        // Rotate player to face movement direction
        if (movement != Vector3.zero)
        {
            Vector3 direction = Vector3.RotateTowards(transform.forward, movement, rotaSpeed * Time.fixedDeltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(direction);
        }

        // Update animator parameters
        float aniMove = movement.magnitude / runSpeed; // Normalize to 0-1 range based on run speed
        animator.SetFloat("moveSpeed", aniMove);
        animator.SetBool("isHolding", isHolding);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == triggerObject && other.CompareTag("Package")) // Check for specific object and "Package" tag
        {
            isHolding = true;
            animator.SetBool("isHolding", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == triggerObject && other.CompareTag("Package"))
        {
            Collider[] colliders = Physics.OverlapSphere(triggerObject.transform.position, 0.1f);
            bool stillTouching = false;

            foreach (Collider col in colliders)
            {
                if (col.CompareTag("Package"))
                {
                    stillTouching = true;
                    break;
                }
            }

            if (!stillTouching)
            {
                isHolding = false;
                animator.SetBool("isHolding", false);
            }
        }
    }

    private void CheckGround()
    {
        // Raycast downwards to detect ground
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);

        // Apply extra downward force if not grounded
        if (!isGrounded)
        {
            rb.AddForce(Vector3.down * 20f, ForceMode.Acceleration);
        }
    }

    private void CheckObstructions()
    {
        // Restore visibility of previous hidden objects
        foreach (GameObject obj in hiddenObjects)
        {
            if (obj != null && originalMaterials.ContainsKey(obj))
            {
                Renderer renderer = obj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.materials = originalMaterials[obj]; // Restore original materials
                }
            }
        }
        hiddenObjects.Clear();
        originalMaterials.Clear();

        // Check for obstructions
        Vector3 directionToPlayer = transform.position - cameraTransform.position;
        Ray ray = new Ray(cameraTransform.position, directionToPlayer);
        RaycastHit[] hits = Physics.RaycastAll(ray, directionToPlayer.magnitude, obstructionLayer);

        foreach (RaycastHit hit in hits)
        {
            GameObject obj = hit.collider.gameObject;
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
            {
                if (!originalMaterials.ContainsKey(obj))
                {
                    originalMaterials[obj] = renderer.materials; // Store original materials

                    // Create transparent materials
                    Material[] transparentMats = new Material[renderer.materials.Length];
                    for (int i = 0; i < renderer.materials.Length; i++)
                    {
                        Material originalMaterial = renderer.materials[i];
                        Material transparentMaterial = new Material(originalMaterial)
                        {
                            color = new Color(originalMaterial.color.r, originalMaterial.color.g, originalMaterial.color.b, transparency)
                        };
                        transparentMaterial.SetFloat("_Mode", 2); // Transparent mode
                        transparentMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                        transparentMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                        transparentMaterial.SetInt("_ZWrite", 0);
                        transparentMaterial.DisableKeyword("_ALPHATEST_ON");
                        transparentMaterial.EnableKeyword("_ALPHABLEND_ON");
                        transparentMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                        transparentMaterial.renderQueue = 3000;

                        transparentMats[i] = transparentMaterial;
                    }
                    renderer.materials = transparentMats; // Apply transparent materials
                }
                hiddenObjects.Add(obj);
            }
        }
    }
}
