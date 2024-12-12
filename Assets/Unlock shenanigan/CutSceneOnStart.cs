using UnityEngine;

public class CutsceneOnStart : MonoBehaviour
{
    public GameObject player;           // Reference to the player (to freeze/unfreeze)
    public float freezeDuration = 5f;   // Duration to freeze the player

    private void Start()
    {
        // Trigger the cutscene when the game starts
        StartCutscene();
    }

    // Starts the cutscene
    public void StartCutscene()
    {
        // Freeze the player by disabling their movement
        FreezePlayer();

        // Optionally, you can wait for the freeze duration before unfreezing the player
        Invoke("UnfreezePlayer", freezeDuration); // Unfreeze after freezeDuration seconds
    }

    // Freezes the player's movement by disabling the player controls
    private void FreezePlayer()
    {
        if (player != null)
        {
            var playerController = player.GetComponent<player>(); // Replace PlayerController with your actual movement script
            if (playerController != null)
            {
                playerController.enabled = false; // Disable movement
            }
            else
            {
                Debug.LogError("PlayerController script not found on player object!");
            }
        }
        else
        {
            Debug.LogError("Player object is not assigned!");
        }
    }

    // Unfreezes the player and resumes movement
    private void UnfreezePlayer()
    {
        if (player != null)
        {
            var playerController = player.GetComponent<player>(); // Replace PlayerController with your actual movement script
            if (playerController != null)
            {
                playerController.enabled = true; // Re-enable movement
            }
        }

        Debug.Log("Cutscene Ended, Player Unfrozen!");
    }
}
