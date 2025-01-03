using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Weapon")]
public class Weapon : ScriptableObject
{
    // Properties
    public Sprite image;               // Gambar untuk tampilan 2D
    public string weaponName;          // Nama weapon
    public GameObject modelPrefab;     // Prefab for the 3D model of the weapon
    public bool unlocked = false;      // Status unlock weapon
    public List<Ingredient> materials; // Daftar bahan untuk crafting weapon
    public int sellPrice;              // Harga jual weapon
    public int whenToUnlock;           // Chapter di mana weapon dapat di-unlock 

    // Methods
    public bool CheckIfCraftable(Inventory inventory)
    {
        // Cek jika setiap material di Inventory cukup untuk crafting
        foreach (Ingredient material in materials)
        {
            Ingredient inventoryMaterial = inventory.GetIngredient(material.name);
            if (inventoryMaterial == null || inventoryMaterial.quantity < material.quantity)
            {
                return false; // Bahan tidak cukup
            }
        }
        return true; // Semua bahan mencukupi
    }

    public bool CanUnlock(int currentChapter) // Fungsi untuk mengecek apakah weapon bisa di-unlock berdasarkan chapter
    {
        return currentChapter >= whenToUnlock;
    }

    public void UnlockWeapon() // diakses dari function blueprint
    {
        unlocked = true;

        WeaponVisibilityController controller = FindObjectOfType<WeaponVisibilityController>();
        if (controller != null)
        {
            controller.UpdateVisibility();
        }
    }
}
