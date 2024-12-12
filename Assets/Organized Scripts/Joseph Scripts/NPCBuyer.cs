using UnityEngine;

public class NPCBuyer : MonoBehaviour
{
    private int money; // Amount of money the NPC has

    void Start()
    {
        money = Random.Range(5,20); // NPC starts with a random amount of money
        Debug.Log($"{gameObject.name} has {money} money.");
    }

    // Check if the NPC can afford an item
    public bool CanAfford(int itemPrice)
    {
        return money >= itemPrice;
    }

    // Deduct money when purchasing an item
    public bool PurchaseItem(int itemPrice)
    {
        if (CanAfford(itemPrice))
        {
            money -= itemPrice;
            Debug.Log($"{gameObject.name} purchased an item for {itemPrice} money. Remaining money: {money}");
            return true;
        }
        Debug.Log($"{gameObject.name} cannot afford an item priced at {itemPrice}. Current money: {money}");
        return false;
    }

    // Check the NPC's current money balance
    public int GetMoney()
    {
        return money;
    }
}
