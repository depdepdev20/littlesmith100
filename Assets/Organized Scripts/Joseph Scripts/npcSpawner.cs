using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcSpawner : MonoBehaviour
{
    [Header("NPC Settings")]
    [SerializeField] private GameObject[] npcPrefabs; // Array to hold multiple NPC prefabs
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private int maxNpcCount = 5;
    [SerializeField] private Transform[] destinations; // Array of destinations for NPCs

    [Header("Difficulty Settings")]
    public bool isHardMode; // Toggle for hard mode

    [Header("Spawn Timing (Easy Mode)")]
    [SerializeField] private int[] easyOpenTimes = new int[7] { 8, 8, 8, 8, 8, 9, 9 };
    [SerializeField] private int[] easyCloseTimes = new int[7] { 17, 17, 17, 17, 17, 15, 15 };

    [Header("Spawn Timing (Hard Mode)")]
    private int[] hardOpenTimes;
    private int[] hardCloseTimes;

    private List<GameObject> activeNpcs = new List<GameObject>();
    private bool isSpawning = false; // Ensure only one coroutine runs at a time

    private void Start()
    {
        // Generate hard mode times by modifying easy mode times
        hardOpenTimes = (int[])easyOpenTimes.Clone(); // Opening times remain the same
        hardCloseTimes = new int[easyCloseTimes.Length];

        for (int i = 0; i < easyCloseTimes.Length; i++)
        {
            hardCloseTimes[i] = Mathf.Max(easyCloseTimes[i] - 2, 0); // Close 2 hours earlier, minimum 0
        }
    }

    private void Update()
    {
        int totalDays = TimeManager.Instance.GetTotalDays();
        int currentDayIndex = (totalDays - 1) % 7; // Calculate day of the week (0 = Sunday, ..., 6 = Saturday)
        int currentHour = TimeManager.Instance.GetTimestamp().hour;

        // Get open and close times based on difficulty mode
        int openTime = isHardMode ? hardOpenTimes[currentDayIndex] : easyOpenTimes[currentDayIndex];
        int closeTime = isHardMode ? hardCloseTimes[currentDayIndex] : easyCloseTimes[currentDayIndex];

        // Check spawn conditions
        if (currentHour >= openTime && currentHour < closeTime && activeNpcs.Count < maxNpcCount && !isSpawning)
        {
            StartCoroutine(SpawnNpcWithDelay());
        }
    }

    private IEnumerator SpawnNpcWithDelay()
    {
        isSpawning = true;

        // Random wait time between 2 and 4 seconds
        float waitTime = Random.Range(2f, 4f);
        yield return new WaitForSeconds(waitTime);

        // Check again to avoid over-spawning
        if (activeNpcs.Count < maxNpcCount)
        {
            SpawnNpc();
        }

        isSpawning = false;
    }

    private void SpawnNpc()
    {
        if (npcPrefabs == null || npcPrefabs.Length == 0 || spawnPoint == null)
        {
            Debug.LogError("No NPC prefabs assigned or spawn point is not set!");
            return;
        }

        // Select a random NPC prefab from the array
        GameObject selectedNpcPrefab = npcPrefabs[Random.Range(0, npcPrefabs.Length)];

        // Instantiate the selected NPC prefab at the spawn point
        GameObject newNpc = Instantiate(selectedNpcPrefab, spawnPoint.position, spawnPoint.rotation);

        if (newNpc != null)
        {
            NPCWalk npcWalk = newNpc.GetComponent<NPCWalk>();
            if (npcWalk != null)
            {
                npcWalk.SetDestinations(destinations);
            }

            activeNpcs.Add(newNpc);
        }
    }

    private void DespawnAllNpcs()
    {
        foreach (GameObject npc in activeNpcs)
        {
            if (npc != null)
            {
                Destroy(npc);
            }
        }
        activeNpcs.Clear();
    }
}
