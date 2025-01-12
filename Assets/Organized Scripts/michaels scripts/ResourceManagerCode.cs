using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManagerCode : MonoBehaviour
{
    public static ResourceManagerCode instance;

    // Resource values
    private int coins = 10000;
    private int wood = 222;
    private int iron = 222;
    private int silver = 222; // New material
    private int copper = 222; // New material
    private int emerald = 222;
    private int diamond = 222;
    private int platinum = 222;
    private int orichalcum = 222;
    private int amethyst = 222;
    private int obsidian = 222;
    public event Action OnResourcesUpdated;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddResource(string resourceType, int value)
    {
        switch (resourceType.ToLower())
        {
            case "coin":
                coins += value;
                break;
            case "iron":
                iron += value;
                break;
            case "diamond":
                diamond += value;
                break;
            case "wood":
                wood += value;
                break;
            case "orichalcum":
                orichalcum += value;
                break;
            case "platinum":
                platinum += value;
                break;
            case "emerald":
                emerald += value;
                break;
            case "amethyst":
                amethyst += value;
                break;
            case "obsidian":
                obsidian += value;
                break;
            case "silver": // New case
                silver += value;
                break;
            case "copper": // New case
                copper += value;
                break;
            default:
                Debug.LogWarning("Resource tidak ada");
                break;
        }
        OnResourcesUpdated?.Invoke();
    }

    public bool SpendResource(string resourceType, int value)
    {
        switch (resourceType.ToLower())
        {
            case "coin":
                if (coins >= value)
                {
                    coins -= value;
                    return true;
                }
                break;
            case "iron":
                if (iron >= value)
                {
                    iron -= value;
                    return true;
                }
                break;
            case "diamond":
                if (diamond >= value)
                {
                    diamond -= value;
                    return true;
                }
                break;
            case "wood":
                if (wood >= value)
                {
                    wood -= value;
                    return true;
                }
                break;
            case "orichalcum":
                if (orichalcum >= value)
                {
                    orichalcum -= value;
                    return true;
                }
                break;
            case "platinum":
                if (platinum >= value)
                {
                    platinum -= value;
                    return true;
                }
                break;
            case "emerald":
                if (emerald >= value)
                {
                    emerald -= value;
                    return true;
                }
                break;
            case "amethyst":
                if (amethyst >= value)
                {
                    amethyst -= value;
                    return true;
                }
                break;
            case "obsidian":
                if (obsidian >= value)
                {
                    obsidian -= value;
                    return true;
                }
                break;
            case "silver": // New case
                if (silver >= value)
                {
                    silver -= value;
                    return true;
                }
                break;
            case "copper": // New case
                if (copper >= value)
                {
                    copper -= value;
                    return true;
                }
                break;
            default:
                Debug.LogWarning("Resource tidak ada");
                break;
        }
        return false;
    }

    public int GetResourceValue(string resourceType)
    {
        switch (resourceType.ToLower())
        {
            case "coin":
                return coins;
            case "iron":
                return iron;
            case "diamond":
                return diamond;
            case "wood":
                return wood;
            case "orichalcum":
                return orichalcum;
            case "emerald":
                return emerald;
            case "platinum":
                return platinum;
            case "amethyst":
                return amethyst;
            case "obsidian":
                return obsidian;
            case "silver": // New case
                return silver;
            case "copper": // New case
                return copper;
            default:
                Debug.LogWarning("Resource tidak ada");
                return 0;
        }
    }
    public void SetResourceValue(string resourceType, int value)
    {
        switch (resourceType.ToLower())
        {
            case "coin":
                coins = value;
                break;
            case "iron":
                iron = value;
                break;
            case "diamond":
                diamond = value;
                break;
            case "wood":
                wood = value;
                break;
            case "orichalcum":
                orichalcum = value;
                break;
            case "platinum":
                platinum = value;
                break;
            case "emerald":
                emerald = value;
                break;
            case "amethyst":
                amethyst = value;
                break;
            case "obsidian":
                obsidian = value;
                break;
            case "silver":
                silver = value;
                break;
            case "copper":
                copper = value;
                break;
            default:
                Debug.LogWarning("Resource tidak ada");
                break;
        }
    }


    /// <summary>
    /// Deduct multiple materials from the resource pool.
    /// </summary>
    /// <param name="materialQuantities">Dictionary where the key is the material name and the value is the quantity to deduct.</param>
    /// <returns>True if all materials were successfully deducted, otherwise false.</returns>
    public bool DeductMaterials(Dictionary<string, int> materialQuantities)
    {
        // Check if all materials are available
        foreach (var material in materialQuantities)
        {
            if (GetResourceValue(material.Key.ToLower()) < material.Value)
            {
                Debug.LogWarning($"Not enough {material.Key}. Deduction aborted.");
                return false;
            }
        }

        // Deduct materials
        foreach (var material in materialQuantities)
        {
            SpendResource(material.Key.ToLower(), material.Value);
        }

        return true;
    }
}
