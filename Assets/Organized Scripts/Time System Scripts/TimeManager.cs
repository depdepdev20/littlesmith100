using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KasperDev.ModularComponents;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    [Header("Internal Clock")]
    [SerializeField] private GameTimeStamp timestamp = new GameTimeStamp(1, GameTimeStamp.Season.Spring, 6, 0, 0);
    [Tooltip("Set the current total days from the Inspector.")]
    [SerializeField] private int totalDays = 0;
    [SerializeField] private float timeScale = -1.0f;

    [Header("Debug Information")]
    [SerializeField] private string currentDayDisplay; // Human-readable day display
    [SerializeField] private string currentTimeDisplay; // Human-readable time display

    [Header("Day and Night Cycle")]
    public Transform sunTransform;
    private Light sunLight;

    public Transform moonTransform;
    private Light moonLight;

    [SerializeField] private BoolVariableSO rollthegoodending; // ScriptableObject for good ending
    [SerializeField] private BoolVariableSO gotgameover; // ScriptableObject for game over

    private List<ITimeTracker> listeners = new List<ITimeTracker>();

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

    void Start()
    {
        // Initialize lighting components
        sunLight = sunTransform.GetComponent<Light>();
        moonLight = moonTransform.GetComponent<Light>();

        //// Reset the BoolVariableSO values at the start
        //if (rollthegoodending != null)
        //    rollthegoodending.SetValue(false);

        //if (gotgameover != null)
        //    gotgameover.SetValue(false);

        //// Trigger an initial check in case `totalDays` was set via the Inspector
        //CheckGameEnding();

        UpdateInspectorDisplays(); // Update displays at the start
        StartCoroutine(TimeUpdate());
    }

    IEnumerator TimeUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(1 / timeScale);
            Tick();
        }
    }

    public GameTimeStamp GetTimestamp()
    {
        return timestamp;
    }

    public void Tick()
    {
        timestamp.UpdateClock();

        if (timestamp.IsNewDay()) // Check for a new day.
        {
            totalDays++;
            CheckGameEnding();
        }

        foreach (ITimeTracker listener in listeners)
        {
            listener.ClockUpdate(timestamp);
        }

        UpdateSunAndMoon();
        AdjustLighting();
        UpdateInspectorDisplays(); // Update Inspector displays on each tick
    }

    public float GetSecondsPerHour()
    {
        return 3600f / timeScale;
    }

    public void CheckGameEnding()
    {
        // Prevent checking if already done
        if ((rollthegoodending != null && rollthegoodending.Value) || (gotgameover != null && gotgameover.Value))
            return;

        // Get the coin count from ResourceManagerCode
        int coinCount = ResourceManagerCode.instance.GetResourceValue("coin");

        // Debug log to verify the ending checks
        Debug.Log($"Day: {totalDays}, Total Coins: {coinCount}. Checking game ending conditions...");

        if (totalDays >= 1) // Adjust to 100 or desired test condition
        {
            if (coinCount >= 100000)
            {
                if (rollthegoodending != null)
                {
                    rollthegoodending.SetValue(true);
                }
            }
            else
            {
                if (gotgameover != null)
                {
                    gotgameover.SetValue(true);
                }
            }

            Debug.Log($"Game ending check complete. Good ending: {rollthegoodending.Value}, Game Over: {gotgameover.Value}");
        }
    }

    public void SetTotalDays(int days)
    {
        totalDays = days;
        UpdateInspectorDisplays();
    }


    private void UpdateSunAndMoon()
    {
        float sunAngle = (timestamp.hour + (timestamp.minute / 60.0f)) * 15f - 90f;
        sunTransform.eulerAngles = new Vector3(sunAngle, 0, 0);

        float moonAngle = sunAngle + 180f;
        moonTransform.eulerAngles = new Vector3(moonAngle, 0, 0);
    }

    private void AdjustLighting()
    {
        // Placeholder for lighting adjustments based on time of day.
    }

    public void RegisterTracker(ITimeTracker listener)
    {
        if (!listeners.Contains(listener))
        {
            listeners.Add(listener);
        }
    }

    public void SetTimestamp(int hour, int minute, int second)
    {
        timestamp.hour = hour;
        timestamp.minute = minute;
        timestamp.second = second;

        UpdateInspectorDisplays(); // Ensure displays are updated
        UpdateSunAndMoon(); // Sync sun and moon positions
    }


    public void UnregisterTracker(ITimeTracker listener)
    {
        listeners.Remove(listener);
    }

    public int GetTotalDays()
    {
        return totalDays;
    }

    // Public method to trigger ending checks manually
    public void TriggerEndingCheck()
    {
        CheckGameEnding();
    }

    private void UpdateInspectorDisplays()
    {
        // Updates the day and time debug info in the Inspector
        currentDayDisplay = $"Day {totalDays}, Day {timestamp.day}";
        currentTimeDisplay = $"Time: {timestamp.hour:D2}:{timestamp.minute:D2}:{timestamp.second:D2}";

        // Debug.Log($"Inspector Updated - {currentDayDisplay}, {currentTimeDisplay}");
    }


}
