using UnityEngine;

[CreateAssetMenu(fileName = "Blueprint", menuName = "ScriptableObjects/Blueprint")]
public class Blueprint : ScriptableObject
{
    public Sprite image; // Gambar blueprint
    public string blueprintName; // Nama blueprint
    public Weapon weaponToUnlock; // Weapon yang akan di-unlock
    public int buyPrice; // Harga blueprint dalam Gold
    public int chapter;

    // Mengecek apakah blueprint bisa dibeli berdasarkan chapter
    public bool CanPurchase(int currentChapter)
    {
        return currentChapter >= weaponToUnlock.whenToUnlock;
    }

    // Fungsi untuk membeli blueprint dan unlock weapon
    public void Purchase()
    {
        weaponToUnlock.UnlockWeapon();
        ResourceManagerCode.instance.SpendResource("coin", buyPrice);
    }
}
