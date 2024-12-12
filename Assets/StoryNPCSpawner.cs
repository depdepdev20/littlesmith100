using System.Collections;
using UnityEngine;
using KasperDev.ModularComponents;

public class StoryNPCSpawner : MonoBehaviour
{
    [Header("NPC Settings")]
    [SerializeField] private GameObject npcPrefab; // The NPC prefab to spawn
    [SerializeField] private Transform spawnPoint; // Where the NPC spawns
    [SerializeField] private Transform destination; // Destination the NPC moves to
    [SerializeField] private BoolVariableSO canSpawn; // Controls whether the NPC can spawn

    [Header("Idle Schedule")]
    [SerializeField] private int[] idleHours = { 10, 14 }; // NPC will idle at these specific hours (e.g., 10 AM and 2 PM)
    [SerializeField] private int idleDurationMin = 1; // Minimum idle time in hours
    [SerializeField] private int idleDurationMax = 3; // Maximum idle time in hours

    [Header("Chapter Management")]
    [SerializeField] private ChapterManager chapterManager; // Reference to the chapter manager

    private GameObject currentNPC; // Tracks the currently spawned NPC
    private bool isNPCActive = false; // Whether the NPC is active (spawned and moving)

    private void Update()
    {
        int currentHour = TimeManager.Instance.GetTimestamp().hour;

        // Check if NPC can spawn and conditions are met (NPC idle times), only if no NPC is currently active
        if (!isNPCActive && IsIdleTime(currentHour))
        {
            SpawnNPC();
        }

        // If the boolean becomes true and no NPC is active, stop spawning NPCs permanently
        if (canSpawn.Value && !isNPCActive)
        {
            StopSpawningPermanently();
        }
    }

    private bool IsIdleTime(int currentHour)
    {
        // Check if the current time falls within any of the idle periods
        foreach (int idleHour in idleHours)
        {
            if (currentHour == idleHour)
            {
                return true;
            }
        }
        return false;
    }

    private void SpawnNPC()
    {
        if (npcPrefab == null || spawnPoint == null || destination == null)
        {
            Debug.LogError("NPC prefab, spawn point, or destination is not assigned!");
            return;
        }

        // Instantiate the NPC
        currentNPC = Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation);
        Debug.Log("NPC spawned and heading to destination.");

        // Move the NPC to the destination
        NPCWalk npcWalk = currentNPC.GetComponent<NPCWalk>();
        if (npcWalk != null)
        {
            npcWalk.SetDestinations(new Transform[] { destination });
        }

        // Set random idle time within the specified range
        int idleDuration = Random.Range(idleDurationMin, idleDurationMax);
        Debug.Log($"NPC will idle for {idleDuration} hours.");

        // Mark NPC as active and start idle routine
        isNPCActive = true;
        StartCoroutine(NPCIdleRoutine(idleDuration));
    }

    private IEnumerator NPCIdleRoutine(int idleDuration)
    {
        // Wait for the NPC to reach the destination
        while (Vector3.Distance(currentNPC.transform.position, destination.position) > 0.5f)
        {
            yield return null;
        }

        Debug.Log($"NPC reached destination. Idling for {idleDuration} hours.");
        yield return new WaitForSeconds(idleDuration * TimeManager.Instance.GetSecondsPerHour());

        // Despawn the NPC after idle time
        DespawnNPC();
    }

    private void DespawnNPC()
    {
        if (currentNPC != null)
        {
            Destroy(currentNPC);
            Debug.Log("NPC despawned.");
        }

        isNPCActive = false; // Mark NPC as inactive after despawning
    }

    private void StopSpawningPermanently()
    {
        // Only stop spawning if no NPC is active
        if (!isNPCActive)
        {
            Debug.Log("NPC will no longer spawn at this location.");
            canSpawn.Value = false; // Disable future spawning when boolean is true

            // Increment the chapter when spawning stops permanently
            chapterManager.UnlockNextChapter();
        }
    }
}
