using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ThinkingBubbleManager : MonoBehaviour
{
    [Header("Bubble Settings")]
    [SerializeField] private GameObject bubblePanel; // Panel containing the bubble
    [SerializeField] private TMP_Text bubbleText; // Text component inside the panel
    [SerializeField] private float typingSpeed = 0.05f; // Time between each character
    [SerializeField] private float bubbleDisplayDuration = 5f; // Duration each bubble stays on screen
    [SerializeField] private int maxCharactersPerBubble = 30; // Max characters per bubble

    private Queue<string> messageQueue = new Queue<string>(); // Queue for multiple messages
    private bool isDisplayingMessage = false;

    private void Start()
    {
        if (bubblePanel == null || bubbleText == null)
        {
            Debug.LogError("Bubble Panel or Text Component is not assigned in the Inspector!");
            return;
        }

        bubblePanel.SetActive(false);
    }

    /// <summary>
    /// Displays a thinking bubble with the provided message.
    /// </summary>
    public void ShowBubble(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            Debug.LogWarning("Empty or null message passed to ShowBubble.");
            return;
        }

        // Split long messages into multiple parts and enqueue
        foreach (var chunk in SplitMessageIntoChunks(message, maxCharactersPerBubble))
        {
            messageQueue.Enqueue(chunk);
        }

        if (!isDisplayingMessage)
        {
            StartCoroutine(DisplayMessageQueue());
        }
    }

    private IEnumerator DisplayMessageQueue()
    {
        isDisplayingMessage = true;

        while (messageQueue.Count > 0)
        {
            string nextMessage = messageQueue.Dequeue();
            yield return StartCoroutine(TypeText(nextMessage));
            yield return new WaitForSeconds(bubbleDisplayDuration);
        }

        bubblePanel.SetActive(false);
        isDisplayingMessage = false;
    }

    private IEnumerator TypeText(string message)
    {
        bubblePanel.SetActive(true);
        bubbleText.text = "";

        foreach (char c in message)
        {
            bubbleText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    /// <summary>
    /// Splits a long message into smaller chunks based on the max character limit.
    /// </summary>
    private List<string> SplitMessageIntoChunks(string message, int maxCharacters)
    {
        List<string> chunks = new List<string>();

        while (message.Length > 0)
        {
            if (message.Length <= maxCharacters)
            {
                chunks.Add(message);
                break;
            }

            int splitIndex = message.LastIndexOf(' ', maxCharacters);
            if (splitIndex == -1) splitIndex = maxCharacters;

            chunks.Add(message.Substring(0, splitIndex).Trim());
            message = message.Substring(splitIndex).Trim();
        }

        return chunks;
    }

    /// <summary>
    /// Triggers an event-based thinking bubble with a specific message.
    /// </summary>
    public void TriggerEvent(string eventName)
    {
        switch (eventName)
        {
            case "CraftingTable":
                ShowBubble("You discovered the crafting table! Keep exploring!");
                break;
            case "FirstPackage":
                ShowBubble("You created your first package! Well done!");
                break;
            case "Generator":
                ShowBubble("You successfully built the generator!");
                break;
            default:
                Debug.LogWarning($"Unknown event triggered: {eventName}");
                break;
        }
    }

    /// <summary>
    /// Triggers a thinking bubble after a specified delay.
    /// </summary>
    public IEnumerator ShowBubbleAfterDelay(string message, float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowBubble(message);
    }


    private IEnumerator DelayedBubbleCoroutine(string message, float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowBubble(message);
    }
}
