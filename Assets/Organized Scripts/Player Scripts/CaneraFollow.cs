using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private Transform player; // Reference to the player
    [SerializeField] private Vector3 offset;   // Position offset for the top-down view

    private void Start()
    {
        // Set the initial offset based on the camera's starting position
        offset = transform.position - player.position;
    }

    private void LateUpdate()
    {
        // Keep the camera at a fixed position above the player
        Vector3 targetPosition = player.position + offset;
        transform.position = targetPosition;
    }
}
