using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExpenseManager : MonoBehaviour
{
    [Header("Base Expenses")]
    [SerializeField] private int baseRent = 1000;
    [SerializeField] private int baseTax = 500;
    [SerializeField] private int baseElectricity = 300;
    [SerializeField] private int basePropertyTax = 200;

    [Header("Chapter-Specific Multipliers")]
    [SerializeField] private List<ChapterMultipliers> chapterMultipliers;

    [Header("Time Settings")]
    [SerializeField] private int expenseIntervalDays = 10; 

    [Header("Scene Settings")]
    [SerializeField] private string transitionSceneName = "TransitionScene"; 
    [SerializeField] private string gameOverSceneName = "GameOver"; 

    [System.Serializable]
    public class ChapterMultipliers
    {
        public float rentMultiplier = 1.0f;
        public float taxMultiplier = 1.0f;
        public float electricityMultiplier = 1.0f;
        public float propertyTaxMultiplier = 1.0f;
    }
    
    private void Start()
    {
        StartCoroutine(DeductExpensesRoutine());
    }

    private IEnumerator DeductExpensesRoutine()
    {
        int lastDeductionDay = -1;

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
        }
        else
        {
            Debug.LogWarning("Not enough coins to cover expenses!");
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
}
