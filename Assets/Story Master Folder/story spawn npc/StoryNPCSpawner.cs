using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KasperDev.ModularComponents;

[System.Serializable]
public class NPCSchedule
{
    public Weapon weapon; // Reference to Weapon ScriptableObject
    public BoolVariableSO questSubmitted; // BoolVariableSO for quest status
    public int spawnHour = 0; // Time (hour) when NPC should spawn
    public int departureHour = 0; // Time (hour) when NPC should leave idle position
    public Transform[] preIdleDestinations; // Path to idle destination
    public Transform idleDestination; // Idle location
    public Transform[] postIdleDestinations; // Path after idling
}

public class StoryNPCSpawner : MonoBehaviour
{
    [Header("NPC Settings")]
    [SerializeField] private GameObject[] npcPrefabs;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private int maxNpcCount = 5;

    [Header("Schedules")]
    [SerializeField] private List<NPCSchedule> npcSchedules;

    [Header("Animation Controllers")]
    [SerializeField] private AnimatorOverrideController walkingAnimatorController;
    [SerializeField] private AnimatorOverrideController idlingAnimatorController;

    private List<GameObject> activeNpcs = new List<GameObject>();
    private bool isSpawning = false;

    private void Update()
    {
        foreach (var schedule in npcSchedules)
        {
            // Only proceed if the weapon is unlocked and the quest is not submitted
            if (schedule.weapon.unlocked && !schedule.questSubmitted.Value)
            {
                HandleNPCSpawning(schedule);
            }
        }
    }

    private void HandleNPCSpawning(NPCSchedule schedule)
    {
        int currentHour = TimeManager.Instance.GetTimestamp().hour;

        // Check if it's the right time to spawn the NPC
        if (currentHour == schedule.spawnHour && activeNpcs.Count < maxNpcCount && !isSpawning)
        {
            StartCoroutine(SpawnNpcWithSchedule(schedule));
        }
    }

    private IEnumerator SpawnNpcWithSchedule(NPCSchedule schedule)
    {
        isSpawning = true;
        yield return new WaitForSeconds(Random.Range(2f, 4f));

        if (activeNpcs.Count < maxNpcCount)
        {
            GameObject npc = SpawnNpc(schedule);
            if (npc != null)
            {
                StartCoroutine(HandleNPCSchedule(npc, schedule));
            }
        }

        isSpawning = false;
    }

    private GameObject SpawnNpc(NPCSchedule schedule)
    {
        if (npcPrefabs.Length == 0 || spawnPoint == null)
        {
            Debug.LogError("No NPC prefabs assigned or spawn point is not set!");
            return null;
        }

        GameObject selectedNpcPrefab = npcPrefabs[Random.Range(0, npcPrefabs.Length)];
        GameObject newNpc = Instantiate(selectedNpcPrefab, spawnPoint.position, spawnPoint.rotation);
        if (newNpc != null) activeNpcs.Add(newNpc);
        return newNpc;
    }

    private IEnumerator HandleNPCSchedule(GameObject npc, NPCSchedule schedule)
    {
        StoryNPCWalk storyNpcWalk = npc.GetComponent<StoryNPCWalk>();
        if (storyNpcWalk == null)
        {
            Debug.LogError("NPC does not have a StoryNPCWalk component!");
            yield break;
        }

        // Subscribe to events
        storyNpcWalk.OnStartWalking += () => SwitchAnimationController(storyNpcWalk, walkingAnimatorController);
        storyNpcWalk.OnStartIdling += () => SwitchAnimationController(storyNpcWalk, idlingAnimatorController);

        // Assign destinations and start the walk routine to idle location
        storyNpcWalk.SetDestinations(schedule.preIdleDestinations, schedule.idleDestination, null);

        // Wait until the NPC reaches the idle destination
        while (Vector3.Distance(storyNpcWalk.GetCurrentDestination(), schedule.idleDestination.position) > storyNpcWalk.GetNavMeshAgent().stoppingDistance)
        {
            yield return null;
        }

        // Wait until the departure time
        while (TimeManager.Instance.GetTimestamp().hour < schedule.departureHour)
        {
            yield return null;
        }

        // Start NPC moving to the final destination
        storyNpcWalk.ResumeWalking(schedule.postIdleDestinations);

        // Wait until the NPC finishes its journey
        while (storyNpcWalk.IsMoving())
        {
            yield return null;
        }

        // At this point, NPC has completed its final path. Remove it from active list but do not destroy it.
        activeNpcs.Remove(npc);
        Debug.Log($"{npc.name} has completed its route and awaits collider-based destruction.");

        // Unsubscribe from events to avoid memory leaks
        storyNpcWalk.OnStartWalking -= () => SwitchAnimationController(storyNpcWalk, walkingAnimatorController);
        storyNpcWalk.OnStartIdling -= () => SwitchAnimationController(storyNpcWalk, idlingAnimatorController);
    }

    private void SwitchAnimationController(StoryNPCWalk storyNpcWalk, AnimatorOverrideController controller)
    {
        if (storyNpcWalk.animator != null && controller != null)
        {
            storyNpcWalk.animator.runtimeAnimatorController = controller;
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
            if (npc != null) Destroy(npc);
        }
        activeNpcs.Clear();
    }
}
