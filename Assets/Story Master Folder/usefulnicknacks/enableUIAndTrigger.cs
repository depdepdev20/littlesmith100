using System;
using UnityEngine;

public class EnableUIAndTriggerDialogue : MonoBehaviour
{
    [Header("UI Element")]
    [SerializeField] private GameObject uiElement; // UI element to enable

    [Header("Dialogue")]
    [SerializeField] private KasperDev.Dialogue.Example.Ex01.DialogueTalk dialogueTalk; // Reference to the DialogueTalk component

    private void OnEnable()
    {
        // Enable the UI element
        if (uiElement != null)
        {
            uiElement.SetActive(true);
        }
        else
        {
            Debug.LogWarning("UI Element is not assigned!");
        }

        // Start the dialogue
        if (dialogueTalk != null)
        {
            dialogueTalk.StartDialogue();
        }
        else
        {
            Debug.LogWarning("DialogueTalk is not assigned!");
        }
    }

    private void Start()
    {
        // Optional: Same logic in Start() if you want to trigger it only once
        OnEnable();
    }
}
