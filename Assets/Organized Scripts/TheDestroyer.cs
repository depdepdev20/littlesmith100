using UnityEngine;

public class TheDestroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NPCBuyer"))
        {
            Debug.Log($"Destroying object with tag NPCBuyer: {other.gameObject.name}");
            Destroy(other.gameObject); // Destroy the object
        }

    }

}
