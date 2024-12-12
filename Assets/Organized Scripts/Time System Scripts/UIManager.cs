using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class UIManager : MonoBehaviour, ITimeTracker
{
    public static UIManager Instance { get; private set; }
    public TMP_Text timeText;
    public TMP_Text dayCountText; // TextMeshPro element to display the day count

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        TimeManager.Instance.RegisterTracker(this); // Register this script as a time tracker
    }

    public void ClockUpdate(GameTimeStamp timestamp)
    {
        // Update the time text with formatted time from GameTimeStamp
        timeText.text = timestamp.GetFormattedTime();

        // Update the day count text with the total day count
        dayCountText.text = $"Day {TimeManager.Instance.GetTotalDays()}";
    }
}
