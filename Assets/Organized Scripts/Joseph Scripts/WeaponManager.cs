using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance;

    // List to store all loaded weapons
    [SerializeField] private List<Weapon> weapons = new List<Weapon>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Ensure there's only one instance
        }

        // Initialize weapons by loading them from the "Resources" folder
        InitializeWeapons();
    }

    // Load all weapons from the "Resources/Weapons" folder
    private void InitializeWeapons()
    {
        Weapon[] allWeapons = Resources.LoadAll<Weapon>("Weapons"); // Folder path in Resources
        weapons.AddRange(allWeapons);
    }

    // Check if a weapon is unlocked
    public bool IsWeaponUnlocked(string weaponName)
    {
        Weapon weapon = weapons.Find(w => w.weaponName == weaponName);
        return weapon != null && weapon.unlocked;
    }

    // Set the unlock state of a specific weapon
    public void SetWeaponUnlockState(string weaponName, bool unlocked)
    {
        Weapon weapon = weapons.Find(w => w.weaponName == weaponName);
        if (weapon != null)
        {
            weapon.unlocked = unlocked;
        }
    }

    // Check if a weapon can be crafted based on the inventory
    public bool CheckIfWeaponCraftable(string weaponName, Inventory inventory)
    {
        Weapon weapon = weapons.Find(w => w.weaponName == weaponName);
        return weapon != null && weapon.CheckIfCraftable(inventory);
    }

    // Get a list of all weapon names
    public List<string> GetAllWeaponNames()
    {
        List<string> weaponNames = new List<string>();
        foreach (Weapon weapon in weapons)
        {
            weaponNames.Add(weapon.weaponName);
        }
        return weaponNames;
    }

    // Unlock a weapon and update its state
    public void UnlockWeapon(string weaponName)
    {
        Weapon weapon = weapons.Find(w => w.weaponName == weaponName);
        if (weapon != null && !weapon.unlocked)
        {
            weapon.UnlockWeapon();
        }
    }

    // Check if a weapon can be unlocked based on the current chapter
    public bool CanUnlockWeapon(string weaponName, int currentChapter)
    {
        Weapon weapon = weapons.Find(w => w.weaponName == weaponName);
        return weapon != null && weapon.CanUnlock(currentChapter);
    }
}
