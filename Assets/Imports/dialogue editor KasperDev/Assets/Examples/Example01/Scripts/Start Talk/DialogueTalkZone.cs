using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace KasperDev.Dialogue.Example.Ex01
{
    public class DialogueTalkZone : MonoBehaviour
    {
        [SerializeField] private GameObject speechBubble;
        [SerializeField] private KeyCode talkKey = KeyCode.E;
        [SerializeField] private TextMeshProUGUI keyInputText;

        private DialogueTalk dialogueTalk;

        private void Awake()
        {
            speechBubble.SetActive(false);
            dialogueTalk = GetComponent<DialogueTalk>();
        }

        void Update()
        {
            if (Input.GetKeyDown(talkKey) && speechBubble.activeSelf)
            {
                dialogueTalk.StartDialogue();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                speechBubble.SetActive(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                speechBubble.SetActive(false);
            }
        }
    }
}
