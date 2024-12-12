using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcSpawner : MonoBehaviour
{
    [Header("NPC Settings")]
    [SerializeField] private GameObject npcPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private int maxNpcCount = 5;
    [SerializeField] private Transform[] destinations; // Array of destinations for NPCs

    [Header("Spawn Timing")]
    [SerializeField] private int spawnStartHour = 8;
    [SerializeField] private int spawnEndHour = 16;

    private List<GameObject> activeNpcs = new List<GameObject>();
    private bool isSpawning = false; // Ensure only one coroutine runs at a time

    private void Update()
    {
        int currentHour = TimeManager.Instance.GetTimestamp().hour;



        // Ensure conditions are met and not already spawning
        if (currentHour >= spawnStartHour && currentHour < spawnEndHour && activeNpcs.Count < maxNpcCount && !isSpawning)
        {
            StartCoroutine(SpawnNpcWithDelay());
        }
    }

    private IEnumerator SpawnNpcWithDelay()
    {
        isSpawning = true;

        // Random wait time between 1 and 5 seconds
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
        if (npcPrefab == null || spawnPoint == null)
        {
            Debug.LogError("Prefab or spawn point is not assigned!");
            return;
        }

        GameObject newNpc = Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation);

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
