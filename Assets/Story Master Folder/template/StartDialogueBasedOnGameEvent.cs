using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KasperDev.Events;
using KasperDev.Dialogue.Example.Ex01;

public class StartDialogueBasedOnGameEvent : MonoBehaviour
{
    [SerializeField] private GameEventListener gameEventListener;
    [SerializeField] private DialogueTalk dialogueTalk;

    private void Awake()
    {
        // Use the assigned DialogueTalk component in the same GameObject
        if (dialogueTalk == null)
        {
            dialogueTalk = GetComponent<DialogueTalk>();
        }

        // Register response to GameEventListener
        if (gameEventListener != null)
        {
            gameEventListener.Response.AddListener(StartDialogue);
        }
    }

    private void StartDialogue()
    {
        if (dialogueTalk != null)
        {
            dialogueTalk.StartDialogue();
        }
    }
}
