using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance;

    private string saveFilePath;

    [SerializeField]
    private DropArea[] dropAreas;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        saveFilePath = Path.Combine(Application.persistentDataPath, "savefile.json");
    }

    [System.Serializable]
    public class ResourceEntry
    {
        public string resourceName;
        public int resourceValue;
    }

    [System.Serializable]
    public class WeaponRackItem
    {
        public string weaponName;
        public int weaponSellPrice;
        public int weaponQuantity;
    }

    [System.Serializable]
    public class SaveData
    {
        public int day;
        public int coins;
        public List<ResourceEntry> resources;
        public int currentChapter;
        public Dictionary<string, bool> weaponUnlocks; // Key-value pair for weapon unlock states
        public List<WeaponRackItem> weaponRackItems;  // Weapon rack data
    }


    public void SaveGame()
    {
        // Check for null references
        if (TimeManager.Instance == null) Debug.LogError("TimeManager.Instance is null!");
        if (ResourceManagerCode.instance == null) Debug.LogError("ResourceManagerCode.instance is null!");
        if (ChapterManager.instance == null) Debug.LogError("ChapterManager.instance is null!");
        if (WeaponManager.Instance == null) Debug.LogError("WeaponManager.Instance is null!");

        // Create SaveData object
        SaveData data = new SaveData
        {
            day = TimeManager.Instance.GetTotalDays(),
            coins = ResourceManagerCode.instance.GetResourceValue("coin"),
            resources = new List<ResourceEntry>
        {
            new ResourceEntry { resourceName = "wood", resourceValue = ResourceManagerCode.instance.GetResourceValue("wood") },
            new ResourceEntry { resourceName = "iron", resourceValue = ResourceManagerCode.instance.GetResourceValue("iron") },
            new ResourceEntry { resourceName = "silver", resourceValue = ResourceManagerCode.instance.GetResourceValue("silver") },
            new ResourceEntry { resourceName = "copper", resourceValue = ResourceManagerCode.instance.GetResourceValue("copper") },
            new ResourceEntry { resourceName = "emerald", resourceValue = ResourceManagerCode.instance.GetResourceValue("emerald") },
            new ResourceEntry { resourceName = "diamond", resourceValue = ResourceManagerCode.instance.GetResourceValue("diamond") },
            new ResourceEntry { resourceName = "platinum", resourceValue = ResourceManagerCode.instance.GetResourceValue("platinum") },
            new ResourceEntry { resourceName = "orichalcum", resourceValue = ResourceManagerCode.instance.GetResourceValue("orichalcum") },
            new ResourceEntry { resourceName = "amethyst", resourceValue = ResourceManagerCode.instance.GetResourceValue("amethyst") },
            new ResourceEntry { resourceName = "obsidian", resourceValue = ResourceManagerCode.instance.GetResourceValue("obsidian") }
        },
            currentChapter = ChapterManager.instance.CurrentChapter,
            weaponUnlocks = new Dictionary<string, bool>(),
            weaponRackItems = new List<WeaponRackItem>()
        };

        foreach (var dropArea in dropAreas)
        {
            if (dropArea == null) continue; // Skip if null
            foreach (var item in dropArea.GetCurrentItems())
            {
                data.weaponRackItems.Add(new WeaponRackItem
                {
                    weaponName = item.weaponName,
                    weaponSellPrice = item.weaponSellPrice,
                    weaponQuantity = item.weaponQuantity
                });
            }
        }

        // Populate weapon unlock states
        foreach (string weaponName in WeaponManager.Instance.GetAllWeaponNames())
        {
            data.weaponUnlocks[weaponName] = WeaponManager.Instance.IsWeaponUnlocked(weaponName);
        }

        // Serialize data to JSON and write to file
        try
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(saveFilePath, json);
            Debug.Log($"Game saved successfully to {saveFilePath}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to save game: {ex.Message}");
        }
    }
    public void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            // Restore general data
            TimeManager.Instance.SetTotalDays(data.day);
            ResourceManagerCode.instance.SetResourceValue("coin", data.coins);

            // Restore resources
            foreach (var resource in data.resources)
            {
                ResourceManagerCode.instance.SetResourceValue(resource.resourceName, resource.resourceValue);
            }

            ChapterManager.instance.SetCurrentChapter(data.currentChapter);

            // Restore weapon unlock states
            foreach (var weaponUnlock in data.weaponUnlocks)
            {
                WeaponManager.Instance.SetWeaponUnlockState(weaponUnlock.Key, weaponUnlock.Value);
            }

            int currentDropAreaIndex = 0;
            foreach (var weaponRackItem in data.weaponRackItems)
            {
                if (dropAreas.Length == 0) break;

                var dropArea = dropAreas[currentDropAreaIndex];
                if (dropArea != null)
                {
                    dropArea.AddItemToRack(weaponRackItem.weaponName, weaponRackItem.weaponSellPrice, weaponRackItem.weaponQuantity);

                    if (dropArea.IsFull())
                    {
                        currentDropAreaIndex = (currentDropAreaIndex + 1) % dropAreas.Length;
                    }
                }
            }

            Debug.Log("Game loaded successfully.");
        }
        else
        {
            Debug.LogWarning("No save file found.");
        }
    }



    // Button Click Methods
    public void OnButtonClickSave()
    {
        Debug.Log("Save button clicked.");
        SaveGame();
    }

    public void OnButtonClickLoad()
    {
        Debug.Log("Load button clicked.");
        LoadGame();
    }
}
