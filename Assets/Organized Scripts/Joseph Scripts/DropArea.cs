using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DropArea : MonoBehaviour
{
    private List<Item> currentItems = new List<Item>();
    private NPCBuyer currentBuyer;

    [SerializeField]
    private GameObject targetObject;

    [SerializeField]
    private GameObject saleNotificationPanel; // Reference to the Sale Notification Panel
    [SerializeField]
    private TMP_Text saleMessage; // Reference to the TextMeshPro text for sale message
    [SerializeField]
    private float notificationDuration = 2f; // Duration to display the notification

    void Start()
    {
        if (targetObject == null)
        {
            Debug.LogError("Target Object is not assigned!");
        }

        // Ensure the sale notification panel is hidden initially
        if (saleNotificationPanel != null)
        {
            saleNotificationPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("Sale Notification Panel is not assigned!");
        }
    }

    public class Item
    {
        public string weaponName;
        public int weaponSellPrice;
        public int weaponQuantity;

        public Item(string name, int price, int quantity)
        {
            weaponName = name;
            weaponSellPrice = price;
            weaponQuantity = quantity;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Package"))
        {
            var package = other.GetComponent<PackageData>();
            if (package != null)
            {
                var existingItem = currentItems.FirstOrDefault(item => item.weaponName == package.weaponName);
                if (existingItem != null)
                {
                    existingItem.weaponQuantity += package.weaponQuantity;
                }
                else
                {
                    currentItems.Add(new Item(package.weaponName, package.weaponSellPrice, package.weaponQuantity));
                }

                Debug.Log($"Package with {package.weaponQuantity} {package.weaponName} entered drop area.");
            }
        }
        else if (other.CompareTag("NPCBuyer"))
        {
            currentBuyer = other.GetComponent<NPCBuyer>();
            Debug.Log($"{currentBuyer.gameObject.name} has entered the DropArea.");

            SellItems(); // Automatically trigger selling
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Package"))
        {
            var package = other.GetComponent<PackageData>();
            if (package != null)
            {
                var item = currentItems.FirstOrDefault(i => i.weaponName == package.weaponName);
                if (item != null)
                {
                    currentItems.Remove(item);
                    Debug.Log($"Removed {package.weaponName} from drop area.");
                }
            }
        }
        else if (other.CompareTag("NPCBuyer"))
        {
            Debug.Log($"{currentBuyer?.gameObject.name} has left the DropArea.");
            currentBuyer = null;
        }
    }

    private void Update()
    {
        if (currentItems.Count > 0 && Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log("U key pressed.");
            TransferItems(); // Transfer all items in bulk
        }

        if (currentItems.Count > 0 && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E key pressed.");
            TransferSingleItem(); // Transfer a single item
        }
    }

    private void TransferSingleItem()
    {
        Debug.Log("Starting TransferSingleItem...");

        if (targetObject == null)
        {
            Debug.LogError("Target Object is not assigned!");
            return;
        }

        var objectPlaceable = targetObject.GetComponent<ObjectPlaceable>();
        if (objectPlaceable == null)
        {
            Debug.LogError("ObjectPlaceable component not found on Target Object!");
            return;
        }

        foreach (var item in currentItems.ToList())
        {
            var package = FindPackageWithName(item.weaponName);
            if (package == null || item.weaponQuantity <= 0 || objectPlaceable.GetSlotCount() >= 4)
                continue;

            int currentSlot = objectPlaceable.GetSlotCount();
            objectPlaceable.ReplaceSlotWithWeapon(package.weapon, currentSlot);
            objectPlaceable.IncrementSlotCount(1);

            package.RemoveWeapons(1);
            item.weaponQuantity--;

            if (item.weaponQuantity <= 0)
                currentItems.Remove(item);

            break; // Transfer only one item
        }
    }

    private void TransferItems()
    {
        Debug.Log("Starting TransferItems...");

        if (targetObject == null)
        {
            Debug.LogError("Target Object is not assigned!");
            return;
        }

        var objectPlaceable = targetObject.GetComponent<ObjectPlaceable>();
        if (objectPlaceable == null)
        {
            Debug.LogError("ObjectPlaceable component not found on Target Object!");
            return;
        }

        foreach (var item in currentItems.ToList())
        {
            var package = FindPackageWithName(item.weaponName);
            if (package == null) continue;

            while (item.weaponQuantity > 0 && objectPlaceable.GetSlotCount() < 4)
            {
                int currentSlot = objectPlaceable.GetSlotCount();
                objectPlaceable.ReplaceSlotWithWeapon(package.weapon, currentSlot);
                objectPlaceable.IncrementSlotCount(1);

                package.RemoveWeapons(1);
                item.weaponQuantity--;
            }

            if (item.weaponQuantity <= 0)
                currentItems.Remove(item);
        }
    }

    private void SellItems()
    {
        if (targetObject == null || currentBuyer == null)
        {
            Debug.LogError("Cannot sell items: Target Object or NPCBuyer is missing.");
            return;
        }

        var objectPlaceable = targetObject.GetComponent<ObjectPlaceable>();
        if (objectPlaceable == null)
        {
            Debug.LogError("ObjectPlaceable component not found on Target Object!");
            return;
        }

        var weaponsInSlots = objectPlaceable.GetWeaponsInSlots();

        Debug.Log($"[Before Sale] Slot Count: {objectPlaceable.GetSlotCount()} | Weapons in Slots: {weaponsInSlots.Length}");
        for (int i = weaponsInSlots.Length - 1; i >= 0; i--) // Iterate backwards to avoid index shifting
        {
            var slotData = weaponsInSlots[i];

            if (currentBuyer.CanAfford(slotData.weaponSellPrice))
            {
                currentBuyer.PurchaseItem(slotData.weaponSellPrice);
                ResourceManagerCode.instance.AddResource("coin", slotData.weaponSellPrice);

                ShowSaleNotification($"+{slotData.weaponSellPrice} coins");
                SoundManager.instance.PlaySound2D("select");

                objectPlaceable.ClearSlot(i);
                Debug.Log($"Slot {i} cleared successfully after sale.");
            }
            else
            {
                Debug.LogWarning($"{currentBuyer.gameObject.name} cannot afford {slotData.weaponName}.");
            }
        }

        int totalCoinsEarned = 0;

        for (int i = 0; i < weaponsInSlots.Length; i++)
        {
            var slotData = weaponsInSlots[i];

            // Check if the buyer can afford the item's price
            if (currentBuyer.CanAfford(slotData.weaponSellPrice))
            {
                Debug.Log($"Processing sale for slot {i}: Weapon = {slotData.weaponName}, Price = {slotData.weaponSellPrice}");

                // Process the sale
                currentBuyer.PurchaseItem(slotData.weaponSellPrice);
                ResourceManagerCode.instance.AddResource("coin", slotData.weaponSellPrice);
                totalCoinsEarned += slotData.weaponSellPrice;

                // Provide sale feedback
                ShowSaleNotification($"+{slotData.weaponSellPrice} coins");
                SoundManager.instance.PlaySound2D("select");

                // Clear the slot immediately after sale
                objectPlaceable.ClearSlot(i);
                Debug.Log($"Slot {i} cleared after sale.");
            }
            else
            {
                Debug.LogWarning($"{currentBuyer.gameObject.name} cannot afford {slotData.weaponName}.");
            }
        }

        Debug.Log($"[After Sale] Total Coins Earned: {totalCoinsEarned}");
        Debug.Log($"[After Sale] Slot Count: {objectPlaceable.GetSlotCount()}");
        var remainingWeapons = objectPlaceable.GetWeaponsInSlots();
        Debug.Log($"[After Sale] Remaining Weapons in Slots: {remainingWeapons.Length}");
        for (int i = 0; i < remainingWeapons.Length; i++)
        {
            Debug.Log($"Slot {i}: Weapon = {remainingWeapons[i].weaponName}, Price = {remainingWeapons[i].weaponSellPrice}");
        }
    }

    public bool IsFull()
    {
        // Example logic: Check if the total quantity of items exceeds a specific limit
        int totalQuantity = currentItems.Sum(item => item.weaponQuantity);
        return totalQuantity >= 10; // Replace 10 with your desired capacity limit
    }


    public List<Item> GetCurrentItems()
    {
        return currentItems;
    }

    public void ClearCurrentItems()
    {
        currentItems.Clear();
    }

    public void AddItemToRack(string weaponName, int weaponSellPrice, int weaponQuantity)
    {
        var existingItem = currentItems.FirstOrDefault(item => item.weaponName == weaponName);
        if (existingItem != null)
        {
            existingItem.weaponQuantity += weaponQuantity;
        }
        else
        {
            currentItems.Add(new Item(weaponName, weaponSellPrice, weaponQuantity));
        }
    }




    private void ShowSaleNotification(string message)
    {
        if (saleNotificationPanel != null && saleMessage != null)
        {
            saleMessage.text = message;
            saleNotificationPanel.SetActive(true);
            StartCoroutine(HideSaleNotification());
        }
    }

    private IEnumerator HideSaleNotification()
    {
        yield return new WaitForSeconds(notificationDuration);
        if (saleNotificationPanel != null)
            saleNotificationPanel.SetActive(false);
    }

    private PackageData FindPackageWithName(string name)
    {
        var packages = FindObjectsOfType<PackageData>();
        return packages.FirstOrDefault(pkg => pkg.weaponName == name);
    }
}
