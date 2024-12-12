using UnityEngine;
using UnityEngine.UI;
using TMPro;
using KasperDev.ModularComponents;
using static ObjectPlaceable;

public class ItemSubmissionArea : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject submissionUI; // The main UI for item submission
    [SerializeField] private Image itemImage; // Displays the required item's image
    [SerializeField] private TMP_Text itemCountText; // Displays current/required item count
    [SerializeField] private Button submitButton; // Button for submitting items
    [SerializeField] private GameObject notificationPanel; // Panel to show notifications
    [SerializeField] private TMP_Text notificationText; // Text for notifications

    [Header("Required Item")]
    [SerializeField] private string requiredItemName = "Wooden Hammer"; // The name of the required item
    [SerializeField] private int requiredItemCount = 100; // Total count needed to complete submission
    [SerializeField] private Sprite requiredItemSprite; // Sprite of the required item

    [Header("Rewards")]
    [SerializeField] private IntVariableSO coinsReward; // ScriptableObject for coins
    [SerializeField] private BoolVariableSO questCompleted; // ScriptableObject for quest completion
    [SerializeField] private BoolVariableSO pathdecided; // ScriptableObject for quest completion

    [SerializeField] private int rewardAmount =0; // Coins rewarded on quest completion

    private int currentItemCount = 0; // Tracks the number of items submitted
    private PackageData detectedPackage; // Temporarily stores the package in the submission area

    private void Start()
    {
        // Initialize UI
        submissionUI.SetActive(false);
        notificationPanel.SetActive(false);
        itemImage.sprite = requiredItemSprite;
        UpdateUI();

        // Add listener to the submit button
        submitButton.onClick.AddListener(SubmitItems);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (pathdecided.Value == true && questCompleted.Value!=true)
        {
            submissionUI.SetActive(true);
        }
        
      // Show submission UI

        // Check if a package entered
        if (other.CompareTag("Package"))
        {
            var package = other.GetComponent<PackageData>();
            submissionUI.SetActive(true);


            if (package != null)
            {
                Debug.Log($"Package detected! Name: {package.weaponName}, Quantity: {package.weaponQuantity}");
                detectedPackage = package; // Store the package
            }
            else
            {
                Debug.LogError($"Object tagged as 'Package' is missing the PackageData component: {other.name}");
            }
        }
        else
        {
            Debug.Log($"Non-package object entered the trigger: {other.name}, Tag: {other.tag}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (pathdecided.Value == true)
        {
            submissionUI.SetActive(false);
        }

        if (other.CompareTag("Package"))
        {
            submissionUI.SetActive(false); // Hide submission UI when a package exits
            detectedPackage = null; // Clear the stored package
            Debug.Log("Package exited submission area.");
        }
    }

    private void SubmitItems()
    {
        if (detectedPackage == null)
        {
            ShowNotification("No package to submit!");
            return;
        }

        if (detectedPackage.weaponName.Trim().Equals(requiredItemName.Trim(), System.StringComparison.Ordinal))
        {
            int transferableQuantity = Mathf.Min(detectedPackage.weaponQuantity, requiredItemCount - currentItemCount);
            currentItemCount += transferableQuantity;

            // Reduce the package quantity
            detectedPackage.RemoveWeapons(transferableQuantity);

            Debug.Log($"Submitted {transferableQuantity} {requiredItemName}. Progress: {currentItemCount}/{requiredItemCount}");
            UpdateUI();

            if (currentItemCount >= requiredItemCount)
            {
                CompleteSubmission();
            }
        }
        else
        {
            Debug.LogWarning($"Mismatched package! Required: '{requiredItemName}', Provided: '{detectedPackage.weaponName}'");
            ShowNotification("Cannot submit this item!");
        }
    }

    private void UpdateUI()
    {
        itemCountText.text = $"{currentItemCount}/{requiredItemCount}";
    }

    private void CompleteSubmission()
    {
        Debug.Log($"Submission Complete: {currentItemCount} {requiredItemName} submitted!");
        RewardPlayer();
        questCompleted.Value = true; // Mark the quest as completed
        ResetSubmission();
    }

    private void RewardPlayer()
    {
        if (coinsReward != null)
        {
            ResourceManagerCode.instance.AddResource("coin", coinsReward.Value);

            Debug.Log($"Player rewarded with {coinsReward.Value} coins! Total: {coinsReward.Value}");
        }
    }

    private void ResetSubmission()
    {
        currentItemCount = 0;
        UpdateUI();
        submissionUI.SetActive(false);
    }

    private void ShowNotification(string message)
    {
        notificationText.text = message;
        notificationPanel.SetActive(true);
        Invoke(nameof(HideNotification), 2f); // Hide after 2 seconds
    }

    private void HideNotification()
    {
        notificationPanel.SetActive(false);
    }
}
