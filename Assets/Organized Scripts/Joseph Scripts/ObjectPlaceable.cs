using System.Linq;
using UnityEngine;

public class ObjectPlaceable : MonoBehaviour
{
    [SerializeField]
    private GameObject[] slots; // Array of slot objects
    private SlotData[] slotDataArray; // Stores data for each slot
    private int slotCount = 0; // Current count of active slots

    void Start()
    {
        if (slots == null || slots.Length == 0)
        {
            Debug.LogError("Slots array is not assigned or empty!");
            return;
        }

        // Initialize slot data array
        slotDataArray = new SlotData[slots.Length];
        for (int i = 0; i < slotDataArray.Length; i++)
        {
            slotDataArray[i] = new SlotData(); // Initialize empty slot data
        }

        // Ensure all slots are disabled at the start
        slotCount = 0;
        UpdateSlots();
    }

    public void IncrementSlotCount(int amount)
    {
        slotCount = Mathf.Clamp(slotCount + amount, 0, slots.Length);
        UpdateSlots();
    }

    public void DecrementSlotCount(int amount)
    {
        slotCount = Mathf.Clamp(slotCount - amount, 0, slots.Length);
        UpdateSlots();
    }

    public void ReplaceSlotWithWeapon(Weapon weaponData, int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Length)
        {
            Debug.LogError($"Slot index {slotIndex} is out of range.");
            return;
        }

        // Update slot data
        slotDataArray[slotIndex].weaponName = weaponData.name;
        slotDataArray[slotIndex].weaponSellPrice = weaponData.sellPrice;

        // Update the visuals
        Transform slotTransform = slots[slotIndex].transform;
        foreach (Transform child in slotTransform.GetComponentsInChildren<Transform>(true))
        {
            child.gameObject.SetActive(false);
        }

        Transform matchingChild = null;
        foreach (Transform child in slotTransform.GetComponentsInChildren<Transform>(true))
        {
            if (child.gameObject.name == weaponData.modelPrefab.name)
            {
                matchingChild = child;
                break;
            }
        }

        if (matchingChild != null)
        {
            matchingChild.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError($"No matching prefab for weapon {weaponData.name} found.");
        }
    }

    public SlotData[] GetWeaponsInSlots()
    {
        // Return only active slots with valid data
        return slotDataArray
            .Where((data, index) => index < slotCount && !string.IsNullOrEmpty(data.weaponName) && data.weaponSellPrice > 0)
            .ToArray();
    }

    private void UpdateSlots()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < slotCount)
            {
                slots[i].SetActive(true);
            }
            else
            {
                slots[i].SetActive(false);
                slotDataArray[i] = new SlotData(); // Reset slot data
            }
        }
    }

    public int GetSlotCount()
    {
        return slotCount;
    }

    public void ClearSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Length)
        {
            Debug.LogError($"Slot index {slotIndex} is out of range.");
            return;
        }

        // Reset slot data
        slotDataArray[slotIndex] = new SlotData();

        // Reset visuals
        Transform slotTransform = slots[slotIndex].transform;
        foreach (Transform child in slotTransform.GetComponentsInChildren<Transform>(true))
        {
            child.gameObject.SetActive(false);
        }

        // Optionally deactivate the slot itself (for visual clarity)
        slots[slotIndex].SetActive(false);

        // Adjust slot count if clearing the last active slot
        if (slotIndex == slotCount - 1)
        {
            slotCount = Mathf.Max(0, slotCount - 1);
        }
    }

    public class SlotData
    {
        public string weaponName;
        public int weaponSellPrice;

        public SlotData()
        {
            weaponName = string.Empty;
            weaponSellPrice = 0;
        }
    }
}
