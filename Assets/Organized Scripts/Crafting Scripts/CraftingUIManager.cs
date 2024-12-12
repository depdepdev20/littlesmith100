using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class CraftingUIManager : MonoBehaviour
{
    public CraftingProcessHandler craftingProcessHandler;
    public GameObject craftingUICanvas;
    public Slider quantitySlider;
    public TextMeshProUGUI quantityText;
    public Transform resourceGroupParent;
    public TextMeshProUGUI weaponNameText;
    public GameObject notEnoughMaterialsPanel;
    public GameObject craftingBlockingPanel; // Drag the panel GameObject from the Unity editor


    private List<MaterialUI> materialSlots = new List<MaterialUI>();
    private WeaponButton currentSelectedButton;
    private Weapon currentWeapon;
    public void RefreshUI(bool updateOwnedCountsOnly = false)
    {
        if (currentWeapon != null)
        {
            if (updateOwnedCountsOnly)
            {
                // Refresh only owned counts for current material slots
                foreach (var slot in materialSlots)
                {
                    if (slot.gameObject.activeSelf)
                    {
                        var materialName = slot.GetMaterialName();
                        int ownedAmount = GetOwnedAmount(materialName);
                        slot.UpdateOwnedAmount(ownedAmount);
                    }
                }
            }
            else
            {
                // Update the full material slots and quantities
                DisplayWeaponMaterials(currentWeapon);
            }
        }
        else if (!updateOwnedCountsOnly)
        {
            // Handle case where no weapon is selected
            UpdateUIForNoWeaponSelected();
        }

        Debug.Log("Crafting UI refreshed.");
    }


    private void OnEnable()
    {
        CraftingProcessHandler.OnCraftingCompleted += OnCraftingCompleted;
        ResourceManagerCode.instance.OnResourcesUpdated += RefreshOwnedCounts; // Subscribe to resource updates
        RefreshUI();
    }

    private void OnDisable()
    {
        CraftingProcessHandler.OnCraftingCompleted -= OnCraftingCompleted;
        ResourceManagerCode.instance.OnResourcesUpdated -= RefreshOwnedCounts; // Unsubscribe from resource updates
    }

    public void RefreshOwnedCounts()
    {
        RefreshUI(true); // Only update owned counts without updating the full UI
    }

    private void Start()
    {
        InitializeMaterialSlots();
        UpdateUIForNoWeaponSelected();
        quantitySlider.onValueChanged.AddListener(UpdateMaterialQuantities);
        craftingBlockingPanel.SetActive(false); // Ensure the panel is hidden at the start
    }


    public void OnCraftButtonPressed()
    {
        if (currentWeapon == null)
        {
            Debug.LogWarning("No weapon selected for crafting.");
            UpdateUIForNoWeaponSelected();
            return;
        }

        int quantity = Mathf.RoundToInt(quantitySlider.value);
        if (!HasEnoughMaterials(quantity))
        {
            notEnoughMaterialsPanel.SetActive(true);
            return;
        }

        DeductMaterials(quantity);

        // Pass the Weapon ScriptableObject directly to the crafting process
        craftingProcessHandler.StartCrafting(quantity, currentWeapon);

        SetUIToCraftingState();
    }



    public void SelectWeaponButton(WeaponButton weaponButton)
    {
        if (currentSelectedButton != null)
            currentSelectedButton.SetSelected(false);

        currentSelectedButton = weaponButton;
        currentSelectedButton.SetSelected(true);

        DisplayWeaponMaterials(weaponButton.weapon);
    }

    public void DisplayWeaponMaterials(Weapon selectedWeapon)
    {
        currentWeapon = selectedWeapon;
        weaponNameText.text = selectedWeapon.weaponName;
        quantitySlider.gameObject.SetActive(true);
        quantityText.gameObject.SetActive(true);

        for (int i = 0; i < materialSlots.Count; i++)
        {
            if (i < selectedWeapon.materials.Count)
            {
                materialSlots[i].SetMaterial(selectedWeapon.materials[i], GetOwnedAmount(selectedWeapon.materials[i].name));
                materialSlots[i].gameObject.SetActive(true);
            }
            else
            {
                materialSlots[i].gameObject.SetActive(false);
            }
        }

        UpdateMaterialQuantities(quantitySlider.value);
    }

    private void UpdateUIForNoWeaponSelected()
    {
        weaponNameText.text = "No Weapon Selected";
        quantitySlider.gameObject.SetActive(false);
        quantityText.gameObject.SetActive(false);

        foreach (var slot in materialSlots)
            slot.gameObject.SetActive(false);
    }

    private void SetUIToCraftingState()
    {
        craftingBlockingPanel.SetActive(true); // Show the blocking panel
    }


    private void OnCraftingCompleted()
    {
        StartCoroutine(ResetUIAfterCrafting());
    }

    private IEnumerator ResetUIAfterCrafting()
    {
        yield return new WaitForSeconds(2f); // Wait for crafting to complete

        if (currentWeapon != null)
        {
            weaponNameText.text = currentWeapon.weaponName; // Restore the weapon's name
        }
        else
        {
            UpdateUIForNoWeaponSelected(); // Handle case where no weapon is selected
        }

        quantitySlider.interactable = true; // Re-enable the slider
        craftingBlockingPanel.SetActive(false); // Hide the blocking panel
        RefreshUI(); // Refresh the UI to update material counts and visibility
    }


    private bool HasEnoughMaterials(int quantity)
    {
        foreach (var material in currentWeapon.materials)
        {
            int ownedAmount = GetOwnedAmount(material.name);
            if (ownedAmount < material.quantity * quantity)
            {
                Debug.LogWarning($"Not enough {material.name}: Required {material.quantity * quantity}, Owned {ownedAmount}");
                return false;
            }
        }
        return true;
    }

    private void DeductMaterials(int quantity)
    {
        Dictionary<string, int> materialsToDeduct = new Dictionary<string, int>();

        foreach (var material in currentWeapon.materials)
        {
            materialsToDeduct[material.name] = material.quantity * quantity;
        }

        if (ResourceManagerCode.instance.DeductMaterials(materialsToDeduct))
        {
            Debug.Log("Materials successfully deducted!");
        }
        else
        {
            Debug.LogWarning("Failed to deduct materials. This should not happen because HasEnoughMaterials was already checked.");
        }
    }

    private void UpdateMaterialQuantities(float multiplier)
    {
        if (currentWeapon == null)
            return;

        int quantity = Mathf.RoundToInt(multiplier);
        quantityText.text = $"Quantity: {quantity}";

        int maxSliderValue = int.MaxValue;

        foreach (var material in currentWeapon.materials)
        {
            int ownedAmount = GetOwnedAmount(material.name);
            if (material.quantity > 0)
            {
                int maxMultiplier = ownedAmount / material.quantity;
                maxSliderValue = Mathf.Min(maxSliderValue, maxMultiplier);
            }
        }

        quantitySlider.maxValue = maxSliderValue;
        quantitySlider.value = Mathf.Min(quantitySlider.value, maxSliderValue);

        // Update the required amounts in the material slots
        for (int i = 0; i < materialSlots.Count; i++)
        {
            if (i < currentWeapon.materials.Count)
            {
                var material = currentWeapon.materials[i];
                int requiredAmount = material.quantity * quantity;
                materialSlots[i].UpdateRequiredAmount(requiredAmount);
            }
        }
    }

    private int GetOwnedAmount(string materialName)
    {
        return ResourceManagerCode.instance.GetResourceValue(materialName);
    }

    private void InitializeMaterialSlots()
    {
        foreach (Transform child in resourceGroupParent)
        {
            MaterialUI slot = new MaterialUI(child);
            materialSlots.Add(slot);
            child.gameObject.SetActive(false);
        }
    }


    public class MaterialUI
    {
        public GameObject gameObject;
        private TextMeshProUGUI materialNameText;
        private TextMeshProUGUI ownedText;
        private TextMeshProUGUI requiredText;
        private Image sourceImage;

        public MaterialUI(Transform parent)
        {
            gameObject = parent.gameObject;
            materialNameText = parent.GetChild(0).GetComponent<TextMeshProUGUI>();
            ownedText = parent.GetChild(1).GetComponent<TextMeshProUGUI>();
            requiredText = parent.GetChild(2).GetComponent<TextMeshProUGUI>();
            sourceImage = parent.GetChild(3).GetComponent<Image>();
        }

        public void SetMaterial(Ingredient material, int ownedAmount)
        {
            materialNameText.text = material.name;
            UpdateOwnedAmount(ownedAmount);
            requiredText.text = $"{material.quantity}";
            sourceImage.sprite = material.image;
        }

        public void UpdateOwnedAmount(int ownedAmount)
        {
            ownedText.text = $"{ownedAmount}";
        }

        public void UpdateRequiredAmount(int requiredAmount)
        {
            requiredText.text = $"{requiredAmount}";
        }

        public string GetMaterialName()
        {
            return materialNameText.text;
        }
    }


}


