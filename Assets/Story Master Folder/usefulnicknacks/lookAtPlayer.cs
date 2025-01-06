using UnityEngine;

public class MakeNPCsLookAtPlayerOnUIEnable : MonoBehaviour
{
    [SerializeField] private string npcTag = "NPCTalking"; // The tag to find NPCs
    [SerializeField] private Transform player; // The player that the objects should look at

    private void OnEnable()
    {
        if (player == null)
        {
            Debug.LogWarning("MakeNPCsLookAtPlayerOnUIEnable: Missing reference for 'player'. Please assign the player.");
            return;
        }

        GameObject[] npcs = GameObject.FindGameObjectsWithTag(npcTag);

        if (npcs.Length == 0)
        {
            Debug.LogWarning($"MakeNPCsLookAtPlayerOnUIEnable: No objects found with tag '{npcTag}'.");
            return;
        }

        foreach (GameObject npc in npcs)
        {
            npc.transform.LookAt(player);
            Debug.Log($"{npc.name} is now looking at {player.name} because {gameObject.name} was enabled.");
        }
    }
}
