using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Camera activeCamera;

    private void Start()
    {
        UpdateActiveCamera();
    }

    private void OnEnable()
    {
        UpdateActiveCamera();
    }

    private void Update()
    {
        if (activeCamera == null)
        {
            UpdateActiveCamera();
        }

        // Make the UI face the active camera
        transform.LookAt(activeCamera.transform);
        transform.Rotate(0, 180, 0);
    }

    private void UpdateActiveCamera()
    {
        activeCamera = Camera.main; // Update reference to the active main camera
    }
}
