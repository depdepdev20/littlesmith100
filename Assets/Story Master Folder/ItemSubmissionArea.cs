using UnityEngine;
using UnityEngine.UI;
using TMPro;
using KasperDev.ModularComponents;
using KasperDev.Events;

public class ItemSubmissionArea : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject speechBubblePanel;
    [SerializeField] private TMP_Text remainingItemText;
    [SerializeField] private Image requiredItemImage;

    [Header("Required Item")]
    [SerializeField] private string requiredItemName = "Wooden Hammer";
    [SerializeField] private int initialRequiredItemCount = 5;
    [SerializeField] private IntVariableSO remainingRequiredItemCount;
    [SerializeField] private Sprite requiredItemSprite;

    [Header("Rewards")]
    [SerializeField] private IntVariableSO coinsReward;
    [SerializeField] private BoolVariableSO questCompleted;
    [SerializeField] private BoolVariableSO pathdecided;
    [SerializeField] private BoolVariableSO isPackageNearbySO;
    [SerializeField] private BoolVariableSO canStartDialogueSO; // New BoolVariableSO to control dialogue activation

    [Header("Events")]
    [SerializeField] private GameEventSO wrongWeaponEvent;
    [SerializeField] private GameEventSO submittedEvent;

    private int currentItemCount = 0;
    private PackageData detectedPackage;

    private void Start()
    {
        remainingRequiredItemCount.Value = initialRequiredItemCount;
        requiredItemImage.sprite = requiredItemSprite;
        speechBubblePanel.SetActive(false);
        UpdateUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (pathdecided.Value && !questCompleted.Value) // Only allow interaction if the quest is not completed
        {
            if (other.CompareTag("Package"))
            {
                detectedPackage = other.GetComponent<PackageData>();
                isPackageNearbySO.Value = true;
                canStartDialogueSO.SetValue(false);
            }
            speechBubblePanel.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Package"))
        {
            detectedPackage = null;
            isPackageNearbySO.Value = false;
        }
        speechBubblePanel.SetActive(false);
        canStartDialogueSO.SetValue(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && detectedPackage != null && isPackageNearbySO.Value && !questCompleted.Value)
        {
            SubmitItems();
        }
    }

    private void SubmitItems()
    {
        if (detectedPackage == null || questCompleted.Value) return; // Prevent submission if the quest is completed

        if (detectedPackage.weaponName.Trim().Equals(requiredItemName.Trim(), System.StringComparison.Ordinal))
        {
            int transferableQuantity = Mathf.Min(detectedPackage.weaponQuantity, remainingRequiredItemCount.Value);
            currentItemCount += transferableQuantity;
            remainingRequiredItemCount.Value -= transferableQuantity;

            if (detectedPackage.weaponQuantity == transferableQuantity)
            {
                isPackageNearbySO.Value = false;
            }

            detectedPackage.RemoveWeapons(transferableQuantity);
            UpdateUI();
            submittedEvent.Raise();

            if (remainingRequiredItemCount.Value <= 0)
            {
                CompleteSubmission();
            }
        }
        else
        {
            wrongWeaponEvent.Raise();
        }
    }

    private void UpdateUI()
    {
        if (!questCompleted.Value)
        {
            remainingItemText.text = $"{remainingRequiredItemCount.Value} X";
        }
        else
        {
            remainingItemText.text = "Quest Completed!";
        }
    }

    private void CompleteSubmission()
    {
        RewardPlayer();
        questCompleted.Value = true;
        ResetSubmission();
        Debug.Log("Quest Completed. Submissions are now disabled.");
    }

    private void RewardPlayer()
    {
        if (coinsReward != null)
        {
            ResourceManagerCode.instance.AddResource("coin", coinsReward.Value);
        }
    }

    private void ResetSubmission()
    {
        currentItemCount = 0;
        remainingRequiredItemCount.Value = initialRequiredItemCount;
        speechBubblePanel.SetActive(false);
        UpdateUI();
    }
}
