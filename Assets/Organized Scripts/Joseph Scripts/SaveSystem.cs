using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance;

    private string saveFilePath;
    [SerializeField] private Weapon[] allWeapons; // Reference to all weapon ScriptableObjects
    [SerializeField] private DropArea dropArea; // Reference to the DropArea assigned in the Inspector
    private const string tempLoadFilePath = "load_flag.temp"; 



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

    public void Update()
    {
        string tempFilePath = Path.Combine(Application.persistentDataPath, tempLoadFilePath);
        if (File.Exists(tempFilePath))
        {
            Debug.Log("Load signal detected. Loading game...");
            LoadGame();
            File.Delete(tempFilePath); // Remove the temp file after processing
        }
    }

    [System.Serializable]
    public class ResourceEntry
    {
        public string resourceName;
        public int resourceValue;
    }

    [System.Serializable]
    public class WeaponEntry
    {
        public string weaponName;
        public bool unlocked;
    }

    [System.Serializable]
    public class ItemEntry
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
        public List<WeaponEntry> weapons; 
        public List<ItemEntry> dropAreaItems;

        public int timestampHour;
        public int timestampMinute;
        public int timestampSecond;
        public int lastDeductionDay; 

    }

    public void SaveGame()
    {
        if (TimeManager.Instance == null) Debug.LogError("TimeManager.Instance is null!");
        if (ResourceManagerCode.instance == null) Debug.LogError("ResourceManagerCode.instance is null!");
        if (ChapterManager.instance == null) Debug.LogError("ChapterManager.instance is null!");
        if (WeaponManager.Instance == null) Debug.LogError("WeaponManager.Instance is null!");
        if (TimeManager.Instance == null) Debug.LogError("TimeManager.Instance is null!");

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
            weapons = new List<WeaponEntry>
            {
                new WeaponEntry { weaponName = "Bow", unlocked = WeaponManager.Instance.IsWeaponUnlocked("Amethyst Chaospiercer") },
                new WeaponEntry { weaponName = "Bow", unlocked = WeaponManager.Instance.IsWeaponUnlocked("Amethyst Spear") },
                new WeaponEntry { weaponName = "Carpenter's Mallet", unlocked = WeaponManager.Instance.IsWeaponUnlocked("Carpenter's Mallet") },
                new WeaponEntry { weaponName = "Chaos Highlord", unlocked = WeaponManager.Instance.IsWeaponUnlocked("Chaos Highlord") },
                new WeaponEntry { weaponName = "Copper Healing Staff", unlocked = WeaponManager.Instance.IsWeaponUnlocked("Copper Healing Staff") },
                new WeaponEntry { weaponName = "Iron Dagger", unlocked = WeaponManager.Instance.IsWeaponUnlocked("Iron Dagger") },
                new WeaponEntry { weaponName = "Iron Spear", unlocked = WeaponManager.Instance.IsWeaponUnlocked("Iron Spear") },
                new WeaponEntry { weaponName = "Iron Sword", unlocked = WeaponManager.Instance.IsWeaponUnlocked("Iron Sword") },
                new WeaponEntry { weaponName = "Pickaxe", unlocked = WeaponManager.Instance.IsWeaponUnlocked("Pickaxe") },
                new WeaponEntry { weaponName = "Radiant Sword", unlocked = WeaponManager.Instance.IsWeaponUnlocked("Radiant Sword") },
                new WeaponEntry { weaponName = "Radiant Vanguard", unlocked = WeaponManager.Instance.IsWeaponUnlocked("Radiant Vanguard") },
                new WeaponEntry { weaponName = "Aetherium Rune Spear", unlocked = WeaponManager.Instance.IsWeaponUnlocked("Aetherium Rune Spear") },
                new WeaponEntry { weaponName = "Aetherium Rune Staff", unlocked = WeaponManager.Instance.IsWeaponUnlocked("Aetherium Rune Staff") },
                new WeaponEntry { weaponName = "Aetherium Rune Sword", unlocked = WeaponManager.Instance.IsWeaponUnlocked("Aetherium Rune Sword") },
                new WeaponEntry { weaponName = "Silver Dagger", unlocked = WeaponManager.Instance.IsWeaponUnlocked("Silver Dagger") },
                new WeaponEntry { weaponName = "Silver Sword", unlocked = WeaponManager.Instance.IsWeaponUnlocked("Silver Sword") },
                new WeaponEntry { weaponName = "Wooden Hammer", unlocked = WeaponManager.Instance.IsWeaponUnlocked("Wooden Hammer") }
            },
            dropAreaItems = dropArea != null
                ? dropArea.GetCurrentItems().ConvertAll(item => new ItemEntry
                {
                    weaponName = item.weaponName,
                    weaponSellPrice = item.weaponSellPrice,
                    weaponQuantity = item.weaponQuantity
                })
                : new List<ItemEntry>(),

            // Save timestamp
            timestampHour = TimeManager.Instance.GetTimestamp().hour,
            timestampMinute = TimeManager.Instance.GetTimestamp().minute,
            timestampSecond = TimeManager.Instance.GetTimestamp().second,
            lastDeductionDay = ExpenseManager.Instance.GetLastDeductionDay() // Save lastDeductionDay


        };

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

            // Restore weapon states
            foreach (var weaponEntry in data.weapons)
            {
                WeaponManager.Instance.SetWeaponUnlockState(weaponEntry.weaponName, weaponEntry.unlocked);
            }

            if (dropArea != null)
            {
                dropArea.ClearItems();
                foreach (var itemEntry in data.dropAreaItems)
                {
                    dropArea.AddItem(new DropArea.Item(
                        itemEntry.weaponName,
                        itemEntry.weaponSellPrice,
                        itemEntry.weaponQuantity
                    ));
                }
            }

            TimeManager.Instance.SetTimestamp(data.timestampHour, data.timestampMinute, data.timestampSecond);


            Debug.Log("Game loaded successfully.");
        }
        else
        {
            Debug.LogWarning("No save file found.");
        }
    }

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
