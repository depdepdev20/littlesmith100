using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class MaterialGeneratorCode : MonoBehaviour
{
    public TMP_Dropdown materialDropdown;
    public TextMeshProUGUI resourceCountText;
    public Button claimButton;

    public int geneLevel = 1;
    public int maxLevel = 5;

    public string selectedMaterial;
    private int producedAmount = 0;
    private bool isProducing = false;

    private CanvasChooseMaterialCode panelChooseMaterial;
    private UpgradePanelCode panelUpgrade;

    // Data structure to hold material-specific upgrade info
    public Dictionary<string, MaterialUpgrade> materialUpgrades = new Dictionary<string, MaterialUpgrade>();

    void Start()
    {
        panelUpgrade = FindObjectOfType<UpgradePanelCode>();
        panelChooseMaterial = FindObjectOfType<CanvasChooseMaterialCode>();

        // Initialize the material upgrade data (could be moved to a configuration table)
        materialUpgrades.Add("wood", new MaterialUpgrade(1500, 8f, 3000, 6f, 6000, 4f, 100));
        materialUpgrades.Add("iron", new MaterialUpgrade(750, 10f, 1500, 8f, 3000, 6f, 100));
        materialUpgrades.Add("copper", new MaterialUpgrade(1000, 6f, 2000, 4f, 4000, 3f, 100));
        materialUpgrades.Add("ruby", new MaterialUpgrade(5000, 16f, 10000, 12f, 20000, 8f, 100));
        materialUpgrades.Add("obsidian", new MaterialUpgrade(250000, 64f, 500000, 48f, 1000000, 32f, 100));
        materialUpgrades.Add("silver", new MaterialUpgrade(2500, 12f, 5000, 10f, 10000, 8f, 100));
        materialUpgrades.Add("emerald", new MaterialUpgrade(10000, 20f, 20000, 15f, 40000, 10f, 100));
        materialUpgrades.Add("diamond", new MaterialUpgrade(30000, 24f, 60000, 18f, 120000, 12f, 100));
        materialUpgrades.Add("platinum", new MaterialUpgrade(15000, 32f, 30000, 24f, 60000, 16f, 100));
        materialUpgrades.Add("orichalcum", new MaterialUpgrade(25000, 36f, 50000, 27f, 100000, 18f, 100));
        materialUpgrades.Add("amethyst", new MaterialUpgrade(150000, 48f, 300000, 36f, 600000, 24f, 100));

        // Initialize the selected material and update the UI
        UpdateSelectedMaterial();

        materialDropdown.onValueChanged.AddListener(delegate { UpdateSelectedMaterial(); });
        claimButton.onClick.AddListener(ClaimResource);

        StartProduction();
    }

    private void UpdateSelectedMaterial()
    {
        // Reset upgrades when changing material
        selectedMaterial = materialDropdown.options[materialDropdown.value].text;

        // Reset geneLevel and producedAmount for the new material
        geneLevel = 1;  // Reset to starting level
        producedAmount = 0;  // Reset produced amount

        // Reset production if already running
        if (isProducing)
        {
            StopCoroutine(ProduceMaterial());
            isProducing = false;  // Stop the current production
        }

        // Start the production process again with the new material
        StartProduction();

        // Update the UI with the new material
        UpdateResourceUI();
        panelUpgrade.UpdateUpgradeUI();
    }


    private void UpdateResourceUI()
    {
        resourceCountText.text = $"{selectedMaterial}: {producedAmount}/{materialUpgrades[selectedMaterial].maxMaterial}";
    }

    private IEnumerator ProduceMaterial()
    {
        isProducing = true;

        // Determine which production time to use based on the material's current upgrade level
        MaterialUpgrade upgrade = materialUpgrades[selectedMaterial];
        float productionTime = 0f;

        // Use different production time based on the current level
        switch (upgrade.currentLevel)
        {
            case 1:
                productionTime = upgrade.productionTime1;
                break;
            case 2:
                productionTime = upgrade.productionTime2;
                break;
            case 3:
                productionTime = upgrade.productionTime3;
                break;
            default:
                productionTime = upgrade.productionTime1; // Default to the first production time
                break;
        }

        while (producedAmount < upgrade.maxMaterial)
        {
            yield return new WaitForSeconds(productionTime); // Wait based on the production time

            producedAmount++;
            UpdateResourceUI();
        }

        isProducing = false;
    }

    public void StartProduction()
    {
        if (!isProducing)
        {
            StartCoroutine(ProduceMaterial());
        }
    }

    public void ClaimResource()
    {
        if (producedAmount > 0)
        {
            ResourceManagerCode.instance.AddResource(selectedMaterial, producedAmount);
            producedAmount = 0;
            UpdateResourceUI();

            StartProduction();
        }
        else
        {
            Debug.LogWarning("No resource to claim");
        }
    }

    // New method to handle upgrading a material
    public bool UpgradeMaterial(string materialName)
    {
        if (materialUpgrades.ContainsKey(materialName))
        {
            MaterialUpgrade upgrade = materialUpgrades[materialName];

            // Get the upgrade cost using the GetUpgradeCost() method
            int cost = upgrade.GetUpgradeCost();

            // Deduct coins using the ResourceManager's SpendResource method
            if (ResourceManagerCode.instance.SpendResource("coin", cost))
            {
                bool upgraded = upgrade.UpgradeMaterial();

                if (upgraded)
                {
                    // Increment the geneLevel after a successful upgrade
                    geneLevel++;
                    return true; // Successful upgrade
                }
            }
        }

        return false; // Upgrade failed
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            panelChooseMaterial.SetPlayerNear(true, this);
            panelUpgrade.ShowPanelUpGen(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            panelChooseMaterial.SetPlayerNear(false, this);
            panelUpgrade.HidePanelUpGen();
        }
    }
}
