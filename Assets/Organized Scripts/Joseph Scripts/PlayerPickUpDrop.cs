using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerPickUpDrop : MonoBehaviour
{
    [SerializeField]
    private Transform playerFaceTransform;
    [SerializeField]
    private Transform objectGrabPointTransform;
    [SerializeField]
    private LayerMask pickUpLayerMask;

    private ObjectGrabbable objectGrabbable;

    private void Update()
    {
        float pickUpDistance = 2f;

        // Draw the raycast for debugging
        Debug.DrawRay(playerFaceTransform.position, playerFaceTransform.forward * pickUpDistance, Color.green);

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (objectGrabbable == null)
            {
                // Attempt to pick up an object
                if (Physics.Raycast(playerFaceTransform.position, playerFaceTransform.forward, out RaycastHit raycastHit, pickUpDistance, pickUpLayerMask))
                {
                    if (raycastHit.transform.TryGetComponent(out objectGrabbable))
                    {
                        objectGrabbable.Grab(objectGrabPointTransform);
                        Debug.Log($"Picked up: {objectGrabbable}");
                    }
                }
            }
            else
            {
                // Drop the currently held object
                objectGrabbable.Drop();
                objectGrabbable = null;
                Debug.Log("Dropped the object.");
            }
        }
    }
}
