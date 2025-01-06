using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class ChapterManager : MonoBehaviour
{
    public static ChapterManager instance;

    [SerializeField] private int currentChapter = 1;
    [SerializeField] private int totalChapters = 7;

    // Event that listeners can subscribe to
    public event Action<int> OnChapterUnlocked;

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

    public int CurrentChapter => currentChapter;

    private List<TMP_Dropdown> chapterDropdowns = new List<TMP_Dropdown>();

    [System.Serializable]
    public class OptionData
    {
        public string Text; // Name of the material
        public Sprite Icon; // Icon of the material
    }

    [System.Serializable]
    public class ChapterOptions
    {
        public List<OptionData> Options; // List of options for a chapter
    }

    [Header("Chapter Options")]
    [SerializeField] private List<ChapterOptions> chapterDropdownOptions;

    private void Start()
    {
        UpdateDropdownOptions(currentChapter);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            UnlockNextChapter();
        }

        UpdateDropdownList();
    }

    public void SetCurrentChapter(int chapter)
    {
        if (chapter < 1)
        {
            Debug.LogWarning("Chapter cannot be less than 1. Setting to 1.");
            currentChapter = 1;
        }
        else if (chapter > totalChapters)
        {
            Debug.LogWarning($"Chapter cannot be greater than {totalChapters}. Setting to {totalChapters}.");
            currentChapter = totalChapters;
        }
        else
        {
            currentChapter = chapter;
        }

        Debug.Log($"Chapter set to {currentChapter}");
        UpdateDropdownOptions(currentChapter);
    }

    public void UnlockNextChapter()
    {
        if (currentChapter < totalChapters)
        {
            currentChapter++;
            Debug.Log($"Chapter increased to {currentChapter}");
            UpdateDropdownOptions(currentChapter);

            // Raise the event
            OnChapterUnlocked?.Invoke(currentChapter);
        }
        else
        {
            Debug.Log("Already at the last chapter.");
        }
    }

    private void UpdateDropdownList()
    {
        TMP_Dropdown[] allDropdowns = FindObjectsOfType<TMP_Dropdown>();

        foreach (TMP_Dropdown dropdown in allDropdowns)
        {
            if (!chapterDropdowns.Contains(dropdown))
            {
                chapterDropdowns.Add(dropdown);
                Debug.Log($"New dropdown detected: {dropdown.gameObject.name}");
                UpdateDropdownOptions(currentChapter);
            }
        }

        // Cleanup: Remove null references from the list
        chapterDropdowns.RemoveAll(dropdown => dropdown == null);
    }

    private void UpdateDropdownOptions(int chapter)
    {
        List<OptionData> availableOptions = new List<OptionData>();
        for (int i = 0; i <= chapter && i < chapterDropdownOptions.Count; i++)
        {
            availableOptions.AddRange(chapterDropdownOptions[i].Options);
        }

        foreach (TMP_Dropdown dropdown in chapterDropdowns)
        {
            if (dropdown == null) continue;

            dropdown.ClearOptions();

            List<TMP_Dropdown.OptionData> tmpOptions = new List<TMP_Dropdown.OptionData>();
            foreach (var option in availableOptions)
            {
                tmpOptions.Add(new TMP_Dropdown.OptionData(option.Text, option.Icon));
            }

            dropdown.AddOptions(tmpOptions);

            if (tmpOptions.Count > 0)
            {
                int currentValue = dropdown.value;
                if (currentValue < 0 || currentValue >= tmpOptions.Count)
                {
                    currentValue = 0;
                }

                dropdown.value = currentValue;
                dropdown.RefreshShownValue();
            }
            else
            {
                dropdown.value = -1;
                dropdown.RefreshShownValue();
            }
        }
    }
}
