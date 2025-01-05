using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance;

    // Dictionary to store weapons by name
    public Dictionary<string, Weapon> weapons = new Dictionary<string, Weapon>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Destroy(gameObject);
        }

        // Initialize weapons (This assumes that all weapons are loaded into the scene or a list of weapons is available)
        InitializeWeapons();
    }

    // Initialize all weapons from a predefined list or assets
    private void InitializeWeapons()
    {
        Weapon[] allWeapons = Resources.LoadAll<Weapon>("Weapons");  // Assuming weapons are in the "Weapons" folder in Resources
        foreach (Weapon weapon in allWeapons)
        {
            weapons[weapon.weaponName] = weapon;
        }
    }

    public bool IsWeaponUnlocked(string weaponName)
    {
        if (weapons.ContainsKey(weaponName))
        {
            return weapons[weaponName].unlocked;
        }
        return false;
    }

    public void SetWeaponUnlockState(string weaponName, bool unlocked)
    {
        if (weapons.ContainsKey(weaponName))
        {
            weapons[weaponName].unlocked = unlocked;
        }
    }

    public bool CheckIfWeaponCraftable(string weaponName, Inventory inventory)
    {
        if (weapons.ContainsKey(weaponName))
        {
            return weapons[weaponName].CheckIfCraftable(inventory);
        }
        return false;
    }

    // Method to retrieve all weapon names (now retrieves from weapons dictionary)
    public List<string> GetAllWeaponNames()
    {
        return new List<string>(weapons.Keys);
    }

    public void UnlockWeapon(string weaponName)
    {
        if (weapons.ContainsKey(weaponName) && !weapons[weaponName].unlocked)
        {
            // Unlock the weapon
            weapons[weaponName].UnlockWeapon();

            // Optionally, handle the visibility update if needed
            WeaponVisibilityController controller = FindObjectOfType<WeaponVisibilityController>();
            if (controller != null)
            {
                controller.UpdateVisibility();
            }
        }
    }

    // Method to check if a weapon can be unlocked based on the current chapter
    public bool CanUnlockWeapon(string weaponName, int currentChapter)
    {
        if (weapons.ContainsKey(weaponName))
        {
            return weapons[weaponName].CanUnlock(currentChapter);
        }
        return false;
    }
}
