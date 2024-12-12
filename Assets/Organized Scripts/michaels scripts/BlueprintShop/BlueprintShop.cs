/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueprintShop : MonoBehaviour
{
    public List<Blueprint> blueprints; // Daftar blueprint yang dijual
    public int currentChapter;

    public void ShowBlueprints(int currentChapter)
    {
        foreach (Blueprint blueprint in blueprints)
        {
            if (blueprint.CanPurchase(currentChapter)) // Periksa apakah blueprint dapat dibeli
            {
                Debug.Log($"Blueprint {blueprint.blueprintName} available for {blueprint.buyPrice} gold.");
            }
            else
            {
                Debug.Log($"Blueprint {blueprint.blueprintName} is locked. Requires Chapter {blueprint.weaponToUnlock.whenToUnlock}.");
            }
        }
    }

    public void PurchaseBlueprint(Blueprint blueprint)
    {
        if (!blueprints.Contains(blueprint))
        {
            Debug.Log("Blueprint not found in shop.");
            return;
        }

        if (!blueprint.CanPurchase(currentChapter)) // Periksa syarat pembelian
        {
            Debug.Log("You cannot purchase this blueprint yet.");
            return;
        }

        if (ResourceManagerCode.instance.GetResourceValue("coin") >= blueprint.buyPrice) // Periksa apakah pemain punya cukup emas
        {
            ResourceManagerCode.instance.SpendResource("coin", blueprint.buyPrice); // Kurangi coin
            blueprint.Purchase(); // Unlock senjata
            Debug.Log($"Purchased {blueprint.blueprintName}.");
        }
        else
        {
            Debug.Log("Not enough gold to purchase this blueprint.");
        }
    }
}
*/


using UnityEngine;
using System.Collections.Generic;

public class BlueprintShop : MonoBehaviour
{
    public Transform blueprintSlotParent; // Parent dari slot
    public GameObject blueprintSlotPrefab; // Prefab UI slot
    public List<Blueprint> blueprints; // List semua blueprint ScriptableObject

    private void Start()
    {
        int currentChapter = 100; // testing
        Debug.Log("Calling PopulateShop...");
        PopulateShop(currentChapter);
    }

    public void PopulateShop(int currentChapter)
    {
        // Hapus semua slot lama
        foreach (Transform child in blueprintSlotParent)
        {
            Destroy(child.gameObject);
        }

        // Buat slot baru untuk setiap blueprint
        foreach (Blueprint blueprint in blueprints)
        {
            // Cek apakah blueprint bisa ditampilkan berdasarkan chapter
            if (blueprint.CanPurchase(currentChapter))
            {
                GameObject slotGO = Instantiate(blueprintSlotPrefab, blueprintSlotParent);
                BlueprintSlotUI slotUI = slotGO.GetComponent<BlueprintSlotUI>();
                slotUI.SetBlueprint(blueprint);
                Debug.Log($"Slot created for blueprint: {blueprint.name}");
            }
        }
    }
}
