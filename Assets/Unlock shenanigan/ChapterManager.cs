using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChapterManager : MonoBehaviour
{
    public static ChapterManager instance;


    [SerializeField] private int currentChapter = 1;
    [SerializeField] private int totalChapters = 7;

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

    public int CurrentChapter
    {
        get { return currentChapter; }
    }

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

    public void UnlockNextChapter()
    {
        if (currentChapter < totalChapters)
        {
            currentChapter++;
            Debug.Log($"Chapter increased to {currentChapter}");
            UpdateDropdownOptions(currentChapter);
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
        // Accumulate options up to the current chapter
        List<OptionData> availableOptions = new List<OptionData>();
        for (int i = 0; i <= chapter && i < chapterDropdownOptions.Count; i++)
        {
            availableOptions.AddRange(chapterDropdownOptions[i].Options);
        }

        foreach (TMP_Dropdown dropdown in chapterDropdowns)
        {
            if (dropdown == null) continue;

            Debug.Log($"Updating dropdown: {dropdown.name}");

            // Clear existing options
            dropdown.ClearOptions();

            // Convert accumulated options to TMP_Dropdown.OptionData
            List<TMP_Dropdown.OptionData> tmpOptions = new List<TMP_Dropdown.OptionData>();
            foreach (var option in availableOptions)
            {
                tmpOptions.Add(new TMP_Dropdown.OptionData(option.Text, option.Icon));
            }

            // Add new options
            dropdown.AddOptions(tmpOptions);
            Debug.Log($"Added {tmpOptions.Count} options to {dropdown.name}");

            // Set the dropdown's current value only if it's valid
            if (tmpOptions.Count > 0)
            {
                int currentValue = dropdown.value;
                if (currentValue < 0 || currentValue >= tmpOptions.Count)
                {
                    currentValue = 0; // Default to the first option if the current value is invalid
                }

                dropdown.value = currentValue;
                dropdown.RefreshShownValue();

                Debug.Log($"Dropdown {dropdown.name} value set to: {currentValue}");

                // Update the caption image based on the current selection
                if (dropdown.captionImage != null)
                {
                    TMP_Dropdown.OptionData selectedOption = tmpOptions[dropdown.value];
                    dropdown.captionImage.sprite = selectedOption.image;
                    dropdown.captionImage.enabled = selectedOption.image != null;

                    if (selectedOption.image != null)
                    {
                        Debug.Log($"Caption image for {dropdown.name} updated to: {selectedOption.image.name}");
                    }
                    else
                    {
                        Debug.Log($"No image for selected option in {dropdown.name}, hiding caption image.");
                    }
                }
            }
            else
            {
                Debug.Log($"No options available for {dropdown.name}");
                dropdown.value = -1; // No valid options
                dropdown.RefreshShownValue();
            }
        }
    }
}
