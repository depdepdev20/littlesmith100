using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPanelController : MonoBehaviour
{
    [SerializeField] GameObject canvasTutor;
    [SerializeField] private GameObject[] panels;
    private int currentPanelIndex = 0;
    public static bool GameIsPaused = false;

    private void Start()
    {
        UpdatePanelVisibility();
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void ShowNextPanel()
    {
        currentPanelIndex = (currentPanelIndex + 1) % panels.Length;
        UpdatePanelVisibility();
    }

    public void ShowPreviousPanel()
    {
        currentPanelIndex = (currentPanelIndex - 1 + panels.Length) % panels.Length;
        UpdatePanelVisibility();
    }

    private void UpdatePanelVisibility()
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(i == currentPanelIndex);
        }
    }

    public void closeUI()
    {
        canvasTutor.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
}
