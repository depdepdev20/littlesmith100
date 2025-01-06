using UnityEngine;
using TMPro;

public class ChapterIndicator : MonoBehaviour
{
    [Header("Chapter Panels")]
    [SerializeField] private Transform panelParent; // Parent object containing all chapter panels

    [Header("Chapter Text")]
    [SerializeField] private TMP_Text chapterText; // TextMeshProUGUI element to display the current chapter

    private int lastDisplayedChapter = -1; // Tracks the last displayed chapter to avoid redundant updates

    private void Update()
    {
        if (ChapterManager.instance == null)
        {
            Debug.LogWarning("ChapterManager instance not found!");
            return;
        }

        int currentChapter = ChapterManager.instance.CurrentChapter;

        // Update only if the chapter has changed
        if (currentChapter != lastDisplayedChapter)
        {
            lastDisplayedChapter = currentChapter;
            UpdateChapterDisplay(currentChapter);
        }
    }

    /// <summary>
    /// Updates the chapter display, including the text and panels.
    /// </summary>
    private void UpdateChapterDisplay(int chapter)
    {
        // Update the chapter text
        if (chapterText != null)
        {
            chapterText.text = $"Chapter {chapter}";
        }
        else
        {
            Debug.LogWarning("Chapter text is not assigned!");
        }

        // Show the corresponding panel
        if (panelParent == null)
        {
            Debug.LogError("Panel parent not assigned!");
            return;
        }

        for (int i = 0; i < panelParent.childCount; i++)
        {
            GameObject panel = panelParent.GetChild(i).gameObject;

            // Activate the correct panel
            if (panel.name == $"Ch {chapter} Panel")
            {
                panel.SetActive(true);

                // Optionally hide the panel after a delay
                Invoke(nameof(HideAllPanels), 3f); // Hides after 3 seconds
            }
            else
            {
                panel.SetActive(false); // Deactivate other panels
            }
        }
    }

    /// <summary>
    /// Hides all chapter panels under the parent object.
    /// </summary>
    private void HideAllPanels()
    {
        if (panelParent == null) return;

        for (int i = 0; i < panelParent.childCount; i++)
        {
            panelParent.GetChild(i).gameObject.SetActive(false);
        }
    }
}
