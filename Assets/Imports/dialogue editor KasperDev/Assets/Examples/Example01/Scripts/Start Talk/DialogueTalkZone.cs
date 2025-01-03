using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KasperDev.ModularComponents;

namespace KasperDev.Dialogue.Example.Ex01
{
    public class DialogueTalkZone : MonoBehaviour
    {
        [SerializeField] private DialogueTalk dialogueTalk;
        [SerializeField] private BoolVariableSO isPackageNearbySO;
        [SerializeField] private BoolVariableSO canStartDialogueSO; // New BoolVariableSO to control dialogue activation
        [SerializeField] private bool isPlayerNearby = false;

        private void Awake()
        {
            if (dialogueTalk == null)
            {
                dialogueTalk = GetComponent<DialogueTalk>();
            }
        }

        private void Update()
        {

            if (Input.GetKeyDown(KeyCode.E) &&
                dialogueTalk != null &&
                !isPackageNearbySO.Value &&
                isPlayerNearby &&
                canStartDialogueSO.Value) // Ensure dialogue can be started
            {
                dialogueTalk.StartDialogue();
            }
           
     
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Package"))
            {
                isPackageNearbySO.SetValue(true);
            }
            else if (other.CompareTag("Player"))
            {
                isPlayerNearby = true;
                canStartDialogueSO.SetValue(true);

            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Package"))
            {
                isPackageNearbySO.SetValue(false);
            }
            else if (other.CompareTag("Player"))
            {
                isPlayerNearby = false;
            }
        }
    }
}
