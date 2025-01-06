using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PackageData : MonoBehaviour
{
    public Weapon weapon; // Reference to the Weapon ScriptableObject
    public int weaponQuantity = 0; // Current quantity in the package
    public int maxCapacity = 200; // Maximum capacity of the package

    public Image weaponImage;
    public TextMeshProUGUI detailInfo; // UI element for displaying details

    // Accessor for weaponName
    public string weaponName => weapon != null ? weapon.weaponName : "Unknown";

    // Accessor for weaponSellPrice
    public int weaponSellPrice => weapon != null ? weapon.sellPrice : 0;

    public void SetData(int quantity, Weapon weaponData) //akses lgsg dri scriptable
    {
        weapon = weaponData;
        weaponQuantity = Mathf.Clamp(quantity, 0, maxCapacity);

        if (weapon != null && weapon.image != null && weaponImage != null)
        {
            weaponImage.sprite = weapon.image; // Assign the weapon's image
        }

        UpdateDisplay();
        Debug.Log($"Package created: {weaponQuantity}x {weapon.weaponName}, Sell Price: {weapon.sellPrice}G each!");
    }

    public int AddWeapons(int amount)
    {
        int availableSpace = maxCapacity - weaponQuantity;
        int addedAmount = Mathf.Min(availableSpace, amount);
        weaponQuantity += addedAmount;

        UpdateDisplay();
        return amount - addedAmount; // Return the remaining amount
    }

    public int RemoveWeapons(int amount)
    {
        int removedAmount = Mathf.Min(weaponQuantity, amount);
        weaponQuantity -= removedAmount;

        UpdateDisplay();
        if (weaponQuantity <= 0)
        {
            Debug.Log($"Package containing {weaponName} is empty. Destroying after 2 seconds.");
            StartCoroutine(DestroyAfterDelay(2f));
        }

        return removedAmount;
    }

    private void UpdateDisplay()
    {
        if (detailInfo != null && weapon != null)
        {
            detailInfo.text = $"{weapon.weaponName}: {weaponQuantity}/{maxCapacity}";
        }
    }

    private System.Collections.IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
