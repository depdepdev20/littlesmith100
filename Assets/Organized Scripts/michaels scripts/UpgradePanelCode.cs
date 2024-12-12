using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradePanelCode : MonoBehaviour
{
    public GameObject panelUpgradeGenerator; // Reference to the upgrade panel UI
    public Button upgradeButton;            // Button to trigger upgrades
    private MaterialGeneratorCode currentGene;  // Current generator being upgraded
    public TextMeshProUGUI upgradeInfoText; // Text element to display upgrade info

    void Start()
    {
        panelUpgradeGenerator.SetActive(false);

        if (upgradeButton != null)
        {
            upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
        }
    }

    // Show the upgrade panel for a specific generator
    public void ShowPanelUpGen(MaterialGeneratorCode gene)
    {
        currentGene = gene;
        panelUpgradeGenerator.SetActive(true);
        UpdateUpgradeUI();
    }

    // Hide the upgrade panel
    public void HidePanelUpGen()
    {
        panelUpgradeGenerator.SetActive(false);
        currentGene = null;
    }

    // Handle the upgrade button click
    private void OnUpgradeButtonClicked()
    {
        if (currentGene != null)
        {
            string materialName = currentGene.selectedMaterial; // Get the current selected material
            MaterialUpgrade upgradeInfo = currentGene.materialUpgrades[materialName]; // Access the upgrade info from the dictionary

            if (currentGene.UpgradeMaterial(materialName)) // Call the upgraded method
            {
                UpdateUpgradeUI(); // Update the UI after successful upgrade
            }
        }
    }

    // Update the UI with the current generator's upgrade information
    public void UpdateUpgradeUI()
    {
        if (currentGene != null)
        {
            string materialName = currentGene.selectedMaterial; // Get the current selected material
            MaterialUpgrade upgradeInfo = currentGene.materialUpgrades[materialName]; // Access the upgrade info from the dictionary

            int currentLevel = currentGene.geneLevel; // The gene level now reflects the upgrade tier
            int maxLevel =4;

            if (currentLevel < maxLevel)
            {
                int nextUpgradeCost = upgradeInfo.GetUpgradeCost(); // Use GetUpgradeCost() to retrieve the correct cost
                upgradeInfoText.text = $"Upgrade {materialName} to Level {currentLevel + 1}\nCost: {nextUpgradeCost} Coins";
                upgradeButton.interactable = true;
            }
            else
            {
                upgradeInfoText.text = $"Max Level Reached for {materialName}";
                upgradeButton.interactable = false;
            }
        }
    }

}
