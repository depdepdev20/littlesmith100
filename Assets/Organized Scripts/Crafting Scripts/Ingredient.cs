using UnityEngine;

[System.Serializable]
public class Ingredient
{
    public Sprite image;
    public string name;
    public int quantity = 0 ; //jumlah (bs distack dalam 3D)
    public bool is_unlocked = false; //status unlock/nda 
    public int unlockChapter; // kapan diunlock (chapter)
    
    public Ingredient(Sprite image, string name, int quantity = 0, bool is_unlocked = false, int unlockChapter = 0)
    {
        this.image = image;
        this.name = name;
        this.quantity = quantity;
        this.is_unlocked = is_unlocked;
        this.unlockChapter = unlockChapter;
    }

    public void AddQuantity(int amount)
    {
        quantity += amount;
    }

    public bool IsUnlocked()
    {
        return is_unlocked;
    }

    public void Unlock()
    {
        is_unlocked = true;
    }
}
