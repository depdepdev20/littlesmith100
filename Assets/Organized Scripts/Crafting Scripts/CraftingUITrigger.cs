using UnityEngine;
using System.Collections;

public class CraftingUITrigger : MonoBehaviour
{
    public GameObject craftingUICanvas; // Assign the Crafting UI Canvas in the Inspector
    private bool isNearCraftingTable = false; // deket crafting table
    private bool isOpened_UI = false;  // kalo UI lagi dibuka 
    [SerializeField] private GameObject speechBubble;
    
    private Vector3 originalPosition;
    private Coroutine bounceCoroutine;

    private void Awake()
    {
        if (speechBubble != null)
        {
            speechBubble.SetActive(false);
            originalPosition = speechBubble.transform.localPosition; // Simpan posisi awal
        }
    }

    void Start()
    {
        // Ensure the Crafting UI is initially closed
        if (craftingUICanvas != null)
        {
            craftingUICanvas.SetActive(false);
        }
    }

    void Update()
    {
        // Check if the player is in the trigger area and presses "E" to open the Crafting UI
        if (isNearCraftingTable && !isOpened_UI)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                OpenCraftingUI();
            }
        }

        // Check if the Crafting UI is open and "Esc" is pressed to close it
        if (isOpened_UI && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseCraftingUI();
        }
    }

    private void OpenCraftingUI()
    {
        if (craftingUICanvas != null)
        {
            craftingUICanvas.SetActive(true);
            isOpened_UI = true;
        }
    }

    private void CloseCraftingUI()
    {
        if (craftingUICanvas != null)
        {
            craftingUICanvas.SetActive(false);
            isOpened_UI = false;
        }
    }

    // Detect when the player enters the trigger zone
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CraftingTable"))
        {
            isNearCraftingTable = true;
            speechBubble.SetActive(true);

            // Mulai animasi naik-turun
            if (bounceCoroutine == null && speechBubble != null)
            {
                bounceCoroutine = StartCoroutine(BounceSpeechBubble());
            }

            Debug.Log("player isNearCraftingTable");
        }
    }

    // Detect when the player exits the trigger zone
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CraftingTable"))
        {
            isNearCraftingTable = false;
            speechBubble.SetActive(false);

            // Hentikan animasi dan reset posisi
            if (bounceCoroutine != null)
            {
                StopCoroutine(bounceCoroutine);
                bounceCoroutine = null;
                speechBubble.transform.localPosition = originalPosition; // Reset posisi
            }

            CloseCraftingUI();  // Ensure the UI closes when the player leaves the area
        }
    }

    // Coroutine untuk animasi naik-turun
    private IEnumerator BounceSpeechBubble()
    {
        float bounceHeight = 0.1f; // Tinggi naik-turun
        float bounceSpeed = 2f;    // Kecepatan animasi

        while (true)
        {
            float yOffset = Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;
            speechBubble.transform.localPosition = originalPosition + new Vector3(0, yOffset, 0);
            yield return null; // Tunggu frame berikutnya
        }
    }
}
