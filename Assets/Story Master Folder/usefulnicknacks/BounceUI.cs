using UnityEngine;
using System.Collections;


public class BounceUI : MonoBehaviour
{
    [Header("Bounce Settings")]
    [SerializeField] private float bounceHeight = 0.1f; // Maximum height of the bounce
    [SerializeField] private float bounceSpeed = 2f;    // Speed of the bounce animation

    private Vector3 originalPosition;
    private Coroutine bounceCoroutine;

    private void Awake()
    {
        // Save the original position of the UI element
        originalPosition = transform.localPosition;
    }

    private void OnEnable()
    {
        // Start the bounce animation when the object is enabled
        if (bounceCoroutine == null)
        {
            bounceCoroutine = StartCoroutine(Bounce());
        }
    }

    private void OnDisable()
    {
        // Stop the bounce animation when the object is disabled
        if (bounceCoroutine != null)
        {
            StopCoroutine(bounceCoroutine);
            bounceCoroutine = null;
        }
        // Reset the position to the original position
        transform.localPosition = originalPosition;
    }

    private IEnumerator Bounce()
    {
        while (true)
        {
            // Calculate the new Y offset based on a sine wave
            float yOffset = Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;
            // Apply the offset to the object's position
            transform.localPosition = originalPosition + new Vector3(0, yOffset, 0);
            // Wait until the next frame
            yield return null;
        }
    }
}
