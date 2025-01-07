using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ExpenseManager : MonoBehaviour
{
    public static ExpenseManager Instance { get; private set; }

    [Header("Base Expenses")]
    [SerializeField] private int baseRent = 1000;
    [SerializeField] private int baseTax = 500;
    [SerializeField] private int baseElectricity = 300;
    [SerializeField] private int basePropertyTax = 200;
    private int lastDeductionDay = -1;

    [Header("Chapter-Specific Multipliers")]
    [SerializeField] private List<ChapterMultipliers> chapterMultipliers;

    [Header("Time Settings")]
    [SerializeField] private int expenseIntervalDays = 10;

    [Header("Scene Settings")]
    [SerializeField] private string transitionSceneName = "TransitionScene";
    [SerializeField] private string gameOverSceneName = "GameOver";

    [Header("Notification Settings")]
    [SerializeField] private GameObject expenseNotificationPanel; // Notification panel
    [SerializeField] private TMP_Text expenseMessage; // Text to display expense message
    [SerializeField] private float notificationDuration = 2f; // Duration to show the notification

    [Header("Sound Settings")]
    [SerializeField] private AudioClip notificationSound; // Sound clip for notification
    private AudioSource audioSource;

    [System.Serializable]
    public class ChapterMultipliers
    {
        public float rentMultiplier = 1.0f;
        public float taxMultiplier = 1.0f;
        public float electricityMultiplier = 1.0f;
        public float propertyTaxMultiplier = 1.0f;
    }

    private void Awake()
    {

        // Ensure the notification panel is hidden initially
        if (expenseNotificationPanel != null)
        {
            expenseNotificationPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("Expense Notification Panel is not assigned!");
        }

        // Initialize AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        StartCoroutine(DeductExpensesRoutine());
    }

    private IEnumerator DeductExpensesRoutine()
    {
        while (true)
        {
            int totalDays = TimeManager.Instance.GetTotalDays();

            if (totalDays > 0 && totalDays % expenseIntervalDays == 0 && totalDays != lastDeductionDay)
            {
                lastDeductionDay = totalDays;
                DeductExpenses();
            }

            yield return new WaitForSeconds(1);
        }
    }

    private void DeductExpenses()
    {
        int currentChapter = ChapterManager.instance.CurrentChapter - 1;
        ChapterMultipliers multipliers = GetChapterMultipliers(currentChapter);

        int totalRent = Mathf.RoundToInt(baseRent * multipliers.rentMultiplier);
        int totalTax = Mathf.RoundToInt(baseTax * multipliers.taxMultiplier);
        int totalElectricity = Mathf.RoundToInt(baseElectricity * multipliers.electricityMultiplier);
        int totalPropertyTax = Mathf.RoundToInt(basePropertyTax * multipliers.propertyTaxMultiplier);

        int totalExpenses = totalRent + totalTax + totalElectricity + totalPropertyTax;

        Debug.Log($"Chapter {currentChapter + 1} Total Expenses: {totalExpenses} coins");

        bool success = ResourceManagerCode.instance.SpendResource("coin", totalExpenses);

        if (success)
        {
            Debug.Log($"Successfully deducted {totalExpenses} coins.");
            ShowExpenseNotification($"Expenses: -{totalExpenses}");
        }
        else
        {
            Debug.LogWarning("Not enough coins to cover expenses!");
            ShowExpenseNotification("Failed to pay expenses! Not enough coins.");
        }

        CheckForGameOver();
    }

    private ChapterMultipliers GetChapterMultipliers(int chapterIndex)
    {
        if (chapterIndex < 0 || chapterIndex >= chapterMultipliers.Count)
        {
            Debug.LogWarning("Chapter index out of bounds, using default multipliers.");
            return new ChapterMultipliers();
        }

        return chapterMultipliers[chapterIndex];
    }

    private void CheckForGameOver()
    {
        int currentMoney = ResourceManagerCode.instance.GetResourceValue("coin");

        if (currentMoney <= 200)
        {
            Debug.LogWarning($"Money is critically low or zero ({currentMoney} coins)! Transitioning to Game Over...");
            TriggerGameOver();
        }
    }

    private void TriggerGameOver()
    {
        SceneTransitionData.nextScene = gameOverSceneName;
        SceneManager.LoadScene(transitionSceneName);
    }

    private void ShowExpenseNotification(string message)
    {
        if (expenseNotificationPanel != null && expenseMessage != null)
        {
            expenseMessage.text = message;

            // Play notification sound
            if (audioSource != null && notificationSound != null)
            {
                audioSource.PlayOneShot(notificationSound);
            }
            else
            {
                Debug.LogWarning("Notification sound or AudioSource is missing.");
            }

            expenseNotificationPanel.SetActive(true);
            StartCoroutine(HideExpenseNotification());
        }
    }

    private IEnumerator HideExpenseNotification()
    {
        yield return new WaitForSeconds(notificationDuration);
        if (expenseNotificationPanel != null)
            expenseNotificationPanel.SetActive(false);
    }

    public int GetLastDeductionDay()
    {
        return lastDeductionDay;
    }

    public void SetLastDeductionDay(int day)
    {
        lastDeductionDay = day;
        Debug.Log($"Last deduction day set to: {lastDeductionDay}");
    }

}
