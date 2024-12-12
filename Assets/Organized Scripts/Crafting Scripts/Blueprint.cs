/*using UnityEngine;

[System.Serializable]
public class Blueprint
{
    public Sprite image; 
    public string blueprintName; 
    public Weapon weaponToUnlock; //weapon yg diunlock 
    public int buyPrice; //harga (Gold)

    public bool CanPurchase(int currentChapter)// Mengecek apakah blueprint bisa dibeli berdasarkan chapter
    {
        return currentChapter >= weaponToUnlock.whenToUnlock;
    }

    public void Purchase()
    {
        weaponToUnlock.UnlockWeapon(); //meng-unlock weapon
    }
}
*/


using UnityEngine;

[CreateAssetMenu(fileName = "Blueprint", menuName = "ScriptableObjects/Blueprint")]
public class Blueprint : ScriptableObject
{
    public Sprite image; // Gambar blueprint
    public string blueprintName; // Nama blueprint
    public Weapon weaponToUnlock; // Weapon yang akan di-unlock
    public int buyPrice; // Harga blueprint dalam Gold

    // Mengecek apakah blueprint bisa dibeli berdasarkan chapter
    public bool CanPurchase(int currentChapter)
    {
        return currentChapter >= weaponToUnlock.whenToUnlock;
    }

    // Fungsi untuk membeli blueprint dan unlock weapon
    public void Purchase()
    {
        weaponToUnlock.UnlockWeapon();
    }
}
