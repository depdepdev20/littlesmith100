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

    private void Update(){
        if (Input.GetKeyDown(KeyCode.P)){
            if (objectGrabbable == null){
                // not
                float pickUpDistance = 2f;
                if (Physics.Raycast(playerFaceTransform.position,playerFaceTransform.forward, out RaycastHit raycastHit, pickUpDistance, pickUpLayerMask)){
                    if (raycastHit.transform.TryGetComponent(out objectGrabbable)){
                        objectGrabbable.Grab(objectGrabPointTransform);
                        Debug.Log(objectGrabbable);
                    }
                }
            } 
            else {
                objectGrabbable.Drop();
                objectGrabbable = null;
            }
        }
    }
}
