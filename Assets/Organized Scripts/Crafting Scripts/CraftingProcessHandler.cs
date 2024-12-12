using UnityEngine;
using System.Collections;

public class CraftingProcessHandler : MonoBehaviour
{
    public delegate void CraftingCompleted();
    public static event CraftingCompleted OnCraftingCompleted;

    public GameObject packagePrefab;
    public Transform packageSpawnPoint;

    private bool isProcessing = false;

    /// <summary>
    /// Starts the crafting process for a weapon.
    /// </summary>
    /// <param name="quantity">Quantity to craft</param>
    /// <param name="weapon">The Weapon ScriptableObject</param>
    public void StartCrafting(int quantity, Weapon weapon)
    {
        if (isProcessing)
        {
            Debug.LogWarning("Crafting process already in progress!");
            return;
        }

        if (weapon == null || quantity <= 0)
        {
            Debug.LogWarning("Invalid weapon or quantity specified!");
            return;
        }

        StartCoroutine(CraftingRoutine(quantity, weapon));
    }

    /// <summary>
    /// Handles the crafting process coroutine.
    /// </summary>
    /// <param name="quantity">Quantity to craft</param>
    /// <param name="weapon">The Weapon ScriptableObject</param>
    private IEnumerator CraftingRoutine(int quantity, Weapon weapon)
    {
        isProcessing = true;

        Debug.Log($"Crafting {quantity}x {weapon.weaponName}...");

        // Simulate crafting time
        yield return new WaitForSeconds(2f);

        // Instantiate the crafted package
        InstantiatePackage(quantity, weapon);

        yield return new WaitForSeconds(2f);

        isProcessing = false;

        // Notify listeners that crafting is complete
        OnCraftingCompleted?.Invoke();
    }

    /// <summary>
    /// Instantiates a package containing the crafted weapons.
    /// </summary>
    /// <param name="quantity">Quantity to craft</param>
    /// <param name="weapon">The Weapon ScriptableObject</param>
    private void InstantiatePackage(int quantity, Weapon weapon)
    {
        if (packagePrefab != null && packageSpawnPoint != null)
        {
            // Create the package object at the spawn point
            GameObject package = Instantiate(packagePrefab, packageSpawnPoint.position, packageSpawnPoint.rotation);
            if (package != null)
            {
                // Configure the package data
                PackageData packageData = package.GetComponent<PackageData>();
                if (packageData != null)
                {
                    packageData.SetData(quantity, weapon);
                }
                else
                {
                    Debug.LogWarning("PackageData component is missing on the package prefab.");
                }
            }
            else
            {
                Debug.LogWarning("Failed to instantiate the package.");
            }
        }
        else
        {
            Debug.LogWarning("Package prefab or spawn point is not assigned!");
        }
    }
}
