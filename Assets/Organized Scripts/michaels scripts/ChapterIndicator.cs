using UnityEngine;
using TMPro;

public class ChapterIndicator : MonoBehaviour
{
    [SerializeField] private GameObject[] chapterPanels;
    [SerializeField] private TextMeshProUGUI chapterText;
    private int currentChapter;

    private void Start()
    {
        UpdateChapter();
        UpdateChapterText(currentChapter);
    
        HideAllPanels();
    }

    private void Update()
    {
        if (currentChapter != ChapterManager.instance.CurrentChapter)
        {
            UpdateChapter();
            UpdateChapterText(currentChapter);
        }
    }

    private void UpdateChapter()
    {
        currentChapter = ChapterManager.instance.CurrentChapter;
    }

    private void UpdateChapterText(int chapter)
    {
        chapterText.text = "Chapter " + chapter;
    }

    public void ShowNextChapterPanel()
    {
        int nextChapter = currentChapter + 1;

        if (nextChapter < chapterPanels.Length)
        {
            chapterPanels[nextChapter].SetActive(true);
        }
        else
        {
            Debug.LogWarning("Tidak ada panel untuk chapter berikutnya.");
        }
    }

    public void HideAllPanels()
    {
        foreach (GameObject panel in chapterPanels)
        {
            panel.SetActive(false);
        }
    }
}
