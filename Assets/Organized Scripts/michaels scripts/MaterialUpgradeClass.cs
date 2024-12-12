[System.Serializable]
public class MaterialUpgrade
{
    public int currentLevel = 0;  // Track current level for each material
    public int upgradeCost1;      // Cost for the first upgrade
    public float productionTime1; // Production time for the first upgrade
    public int upgradeCost2;      // Cost for the second upgrade
    public float productionTime2; // Production time for the second upgrade
    public int upgradeCost3;      // Cost for the third upgrade
    public float productionTime3; // Production time for the third upgrade
    public int maxMaterial;       // Max material produced at any time (this may be set depending on your game needs)

    // Constructor to initialize material upgrade data
    public MaterialUpgrade(int cost1, float prodTime1, int cost2, float prodTime2, int cost3, float prodTime3, int maxMaterial)
    {
        upgradeCost1 = cost1;
        productionTime1 = prodTime1;
        upgradeCost2 = cost2;
        productionTime2 = prodTime2;
        upgradeCost3 = cost3;
        productionTime3 = prodTime3;
        this.maxMaterial = maxMaterial;
    }

    // Method to upgrade the material and adjust its properties
    public bool UpgradeMaterial()
    {
        switch (currentLevel)
        {
            case 0:
                currentLevel++;
                maxMaterial += 100; // Example: Increase max material by 100
                productionTime1 *= 0.9f;  // Decrease production time by 10% for the first upgrade
                return true;

            case 1:
                currentLevel++;
                maxMaterial += 200; // Increase max material by 200
                productionTime2 *= 0.9f;  // Decrease production time by 10% for the second upgrade
                return true;

            case 2:
                currentLevel++;
                maxMaterial += 300; // Increase max material by 300
                productionTime3 *= 0.9f;  // Decrease production time by 10% for the third upgrade
                return true;

            default:
                return false; // Max level reached
        }
    }

    // Method to get the cost based on the current upgrade level
    public int GetUpgradeCost()
    {
        switch (currentLevel)
        {
            case 0: return upgradeCost1; // Cost for first upgrade
            case 1: return upgradeCost2; // Cost for second upgrade
            case 2: return upgradeCost3; // Cost for third upgrade
            default: return 0;           // No upgrade available
        }
    }

    // Method to get the production time based on the current upgrade level
    public float GetProductionTime()
    {
        switch (currentLevel)
        {
            case 0: return productionTime1; // Time for first upgrade
            case 1: return productionTime2; // Time for second upgrade
            case 2: return productionTime3; // Time for third upgrade
            default: return 0f;             // No production time available
        }
    }
}
