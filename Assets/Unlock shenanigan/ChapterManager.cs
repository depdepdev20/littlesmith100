using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChapterManager : MonoBehaviour
{
    [SerializeField] private int currentChapter = 1;
    [SerializeField] private int totalChapters = 7;

    // List to store all dropdowns
    private List<TMP_Dropdown> chapterDropdowns = new List<TMP_Dropdown>();

    // Dropdown options per chapter
    private Dictionary<int, List<string>> chapterDropdownOptions = new Dictionary<int, List<string>>()
    {
        { 0, new List<string> { "wood" } },
        { 1, new List<string> { "wood", "iron" } },
        { 2, new List<string> { "wood", "iron", "copper", "silver" } },
        { 3, new List<string> { "wood", "iron", "copper", "silver", "emerald", "diamond" } },
        { 4, new List<string> { "wood", "iron", "copper", "silver", "emerald", "diamond", "platinum", "orichalcum" } },
        { 5, new List<string> { "wood", "iron", "copper", "silver", "emerald", "diamond", "platinum", "orichalcum", "amethyst", "obsidian" } }
    };

    // Find all dropdowns at the start
    private void Start()
    {
        UpdateDropdownList();
        UpdateDropdownOptions(currentChapter);
    }

    // Continuously check for new dropdowns and update their options
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            UnlockNextChapter();
        }

        // Update dropdowns every frame
        UpdateDropdownList();
        UpdateDropdownOptions(currentChapter);
    }

    // Unlock the next chapter
    public void UnlockNextChapter()
    {
        if (currentChapter < totalChapters)
        {
            currentChapter++;
            Debug.Log($"Chapter increased to {currentChapter}");
        }
        else
        {
            Debug.Log("Already at the last chapter.");
        }
    }

    // Find all dropdowns dynamically in the scene
    private void UpdateDropdownList()
    {
        TMP_Dropdown[] allDropdowns = FindObjectsOfType<TMP_Dropdown>();

        // Add new dropdowns to the list
        foreach (TMP_Dropdown dropdown in allDropdowns)
        {
            if (!chapterDropdowns.Contains(dropdown))
            {
                chapterDropdowns.Add(dropdown);
                Debug.Log($"New dropdown detected: {dropdown.gameObject.name}");
            }
        }
    }

    // Update dropdown options for all dropdowns
    private void UpdateDropdownOptions(int chapter)
    {
        List<string> options = chapterDropdownOptions.ContainsKey(chapter) ? chapterDropdownOptions[chapter] : new List<string>();

        foreach (TMP_Dropdown dropdown in chapterDropdowns)
        {
            if (dropdown == null) continue;

            dropdown.ClearOptions();
            dropdown.AddOptions(options.Count > 0 ? options : new List<string> { "Default Option" });
            dropdown.RefreshShownValue();
            LayoutRebuilder.ForceRebuildLayoutImmediate(dropdown.transform as RectTransform);
        }
    }
}
