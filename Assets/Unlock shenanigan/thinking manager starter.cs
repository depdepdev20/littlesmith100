using System.Collections;
using UnityEngine;

public class ThinkingManagerStarter : MonoBehaviour
{
    private ThinkingBubbleManager bubbleManager;

    void Start()
    {
        // Find the ThinkingBubbleManager in the scene
        bubbleManager = FindObjectOfType<ThinkingBubbleManager>();

        if (bubbleManager != null)
        {
            // Example: Call a coroutine to queue multiple bubbles with a delay
            StartCoroutine(QueueMultipleBubbles());
        }
        else
        {
            Debug.LogError("ThinkingBubbleManager not found in the scene!");
        }
    }

    private IEnumerator QueueMultipleBubbles()
    {
        // Wait for some initial delay if needed
        yield return new WaitForSeconds(2f);

        // Queue the first message
        bubbleManager.ShowBubble("A smithy starts with a blacksmith!");

        // Delay before the next bubble
        yield return new WaitForSeconds(2f); // Includes display duration + typing time

        // Queue the second message
        bubbleManager.ShowBubble("I have some wood that i can craft into something");
        bubbleManager.ShowBubble("I think there should be a place to craft somewhere..");




    }
}
