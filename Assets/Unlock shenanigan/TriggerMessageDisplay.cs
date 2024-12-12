using UnityEngine;
using System.Collections; 

public class TriggerMessageDisplay : MonoBehaviour
{
    private ThinkingBubbleManager bubbleManager;
    private int triggerCount = 0; // Keeps track of the number of times the trigger has been activated
    public string[] messages; // Array to hold messages
    public float messageDelay = 2f; // Delay before showing each message

    void Start()
    {
        // Find the ThinkingBubbleManager in the scene
        bubbleManager = FindObjectOfType<ThinkingBubbleManager>();

        if (bubbleManager == null)
        {
            Debug.LogError("ThinkingBubbleManager not found in the scene!");
        }
    }

    // This function is called when another collider enters the trigger area
    private void OnTriggerEnter(Collider other)
    {
        // Check if the player or relevant object has entered the trigger area
        if (other.CompareTag("Player")) // You can adjust this if needed
        {
            // Make sure to display the message only the first and second times the player triggers the area
            if (triggerCount < 1)
            {
                StartCoroutine(DisplayMessages());
                triggerCount++;
            }
        }
    }

    private IEnumerator DisplayMessages()
    {
        // Check if there are messages and trigger them
        if (messages.Length > 0)
        {
            for (int i = 0; i < messages.Length; i++)
            {
                // Show the current message
                bubbleManager.ShowBubble(messages[i]);

                // Wait for the specified delay before showing the next message
                yield return new WaitForSeconds(messageDelay);
            }
        }
    }
}
