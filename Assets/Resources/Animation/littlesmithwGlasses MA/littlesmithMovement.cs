using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class littlesmithMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float rotaSpeed = 5f;
    [SerializeField] private GameObject triggerObject; // Assign specific game object in Inspector
    [SerializeField] private LayerMask groundLayer; // Ground layer for ground check
    [SerializeField] private float groundCheckDistance = 0.1f; // Ground check distance

    private Animator animator;
    private Rigidbody rb;
    private float vertical;
    private float horizontal;
    private bool isHolding = false;
    private bool isGrounded = false;

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
        // Ensure "isHolding" only turns false if the triggerObject completely exits
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
}
