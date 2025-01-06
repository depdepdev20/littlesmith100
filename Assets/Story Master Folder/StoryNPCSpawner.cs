using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KasperDev.ModularComponents;

[System.Serializable]
public class NPCSchedule
{
    public Weapon weapon; // Reference to Weapon ScriptableObject
    public BoolVariableSO questSubmitted; // Reference to BoolVariableSO for quest status
    public int[] openTimes = new int[7];
    public int[] closeTimes = new int[7];
    public float waitDuration = 5f; // Duration to wait at the idle destination
    public Transform[] preIdleDestinations; // Destinations before idling
    public Transform idleDestination; // Idle destination
    public Transform[] postIdleDestinations; // Destinations after idling
}

public class StoryNPCSpawner : MonoBehaviour
{
    [Header("NPC Settings")]
    [SerializeField] private GameObject[] npcPrefabs;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private int maxNpcCount = 5;

    [Header("Schedules")]
    [SerializeField] private List<NPCSchedule> npcSchedules;

    private List<GameObject> activeNpcs = new List<GameObject>();
    private bool isSpawning = false;

    private void Update()
    {
        foreach (var schedule in npcSchedules)
        {
            if (schedule.weapon.unlocked && !schedule.questSubmitted.Value)
            {
                HandleNPCSpawning(schedule);
            }
        }
    }

    private void HandleNPCSpawning(NPCSchedule schedule)
    {
        int totalDays = TimeManager.Instance.GetTotalDays();
        int currentDayIndex = (totalDays - 1) % 7;
        int currentHour = TimeManager.Instance.GetTimestamp().hour;

        int openTime = schedule.openTimes[currentDayIndex];
        int closeTime = schedule.closeTimes[currentDayIndex];

        if (currentHour >= openTime && currentHour < closeTime && activeNpcs.Count < maxNpcCount && !isSpawning)
        {
            StartCoroutine(SpawnNpcWithDelay(schedule));
        }
    }

    private IEnumerator SpawnNpcWithDelay(NPCSchedule schedule)
    {
        isSpawning = true;
        float waitTime = Random.Range(2f, 4f);
        yield return new WaitForSeconds(waitTime);

        if (activeNpcs.Count < maxNpcCount)
        {
            SpawnNpc(schedule);
        }
        isSpawning = false;
    }

    private void SpawnNpc(NPCSchedule schedule)
    {
        if (npcPrefabs.Length == 0 || spawnPoint == null)
        {
            Debug.LogError("No NPC prefabs assigned or spawn point is not set!");
            return;
        }

        GameObject selectedNpcPrefab = npcPrefabs[Random.Range(0, npcPrefabs.Length)];
        GameObject newNpc = Instantiate(selectedNpcPrefab, spawnPoint.position, spawnPoint.rotation);

        if (newNpc != null)
        {
            NPCWalk npcWalk = newNpc.GetComponent<NPCWalk>();
            if (npcWalk != null)
            {
                // Combine destinations for pre-idle, idle, and post-idle phases
                List<Transform> allDestinations = new List<Transform>();
                allDestinations.AddRange(schedule.preIdleDestinations);
                allDestinations.Add(schedule.idleDestination);
                allDestinations.AddRange(schedule.postIdleDestinations);

                npcWalk.SetDestinations(allDestinations.ToArray());
                npcWalk.SetWaitDuration(schedule.waitDuration); // Set wait duration at the idle destination
            }
            activeNpcs.Add(newNpc);
        }
    }

    public void SubmitQuest(Weapon weapon)
    {
        foreach (var schedule in npcSchedules)
        {
            if (schedule.weapon == weapon)
            {
                schedule.questSubmitted.SetValue(true);
            }
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
