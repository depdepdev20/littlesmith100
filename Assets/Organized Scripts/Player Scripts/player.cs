using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotaSpeed = 5f;

    private Animator animator;
    private Rigidbody rb;
    private float vertical;
    private float horizontal;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        InputHandler();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void InputHandler()
    {
        // Reverse the input directions
        vertical = -Input.GetAxis("Vertical");
        horizontal = -Input.GetAxis("Horizontal");
    }

    private void Movement()
    {
        // Calculate the reversed movement
        Vector3 movement = new Vector3(horizontal, 0, vertical) * moveSpeed;
        rb.velocity = movement;

        // Rotate the player to face the reversed direction
        if (movement != Vector3.zero) // Prevent rotation when there's no input
        {
            Vector3 direction = Vector3.RotateTowards(transform.forward, movement, rotaSpeed * Time.fixedDeltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(direction);
        }

        // Update animator parameter based on movement magnitude
        float aniMove = Vector3.Magnitude(movement.normalized);
        animator.SetFloat("moveSpeed", aniMove);
    }
}
