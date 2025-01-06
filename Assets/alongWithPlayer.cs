using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlongWithPlayer : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float rotationSpeed = 5f;

    private float vertical;
    private float horizontal;

    private void Update()
    {
        HandleInput();
        MoveAndRotate();
    }

    private void HandleInput()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
    }

    private void MoveAndRotate()
    {
        // Move the player forward/backward and sideways
        Vector3 movement = new Vector3(horizontal, 0, vertical).normalized * walkSpeed * Time.deltaTime;
        transform.Translate(movement, Space.World);

        // Rotate the player to face the movement direction
        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
