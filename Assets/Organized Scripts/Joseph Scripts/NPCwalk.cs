using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPCWalk : MonoBehaviour
{
    private Transform[] destinations; // Array of potential destinations
    private NavMeshAgent agent;
    private int currentDestinationIndex = 0;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Start walking only if destinations are set
        if (destinations != null && destinations.Length > 0)
        {
            StartCoroutine(WalkToDestinations());
        }
        else
        {
            Debug.LogWarning("No destinations set for NPC.");
        }
    }

    private IEnumerator WalkToDestinations()
    {
        while (true)
        {
            if (destinations.Length > 0)
            {
                Transform targetDestination;

                // Determine the destination
                if (currentDestinationIndex < 3) // First 3 destinations
                {
                    targetDestination = destinations[currentDestinationIndex];
                }
                else if (currentDestinationIndex >= destinations.Length - 3) // Last 3 destinations
                {
                    int fixedIndex = currentDestinationIndex - (destinations.Length - 3);
                    targetDestination = destinations[destinations.Length - 3 + fixedIndex];
                }
                else // Random destinations for middle ones
                {
                    int randomIndex = Random.Range(3, destinations.Length - 3);
                    targetDestination = destinations[randomIndex];
                }

                // Ensure the target destination is valid
                if (targetDestination != null)
                {
                    agent.SetDestination(targetDestination.position);
                    Debug.Log($"Heading to destination {targetDestination.name}");

                    float elapsedTime = 0f;
                    while (elapsedTime < 7f && !agent.pathPending && agent.remainingDistance > agent.stoppingDistance)
                    {
                        elapsedTime += Time.deltaTime;
                        yield return null; // Wait for the next frame
                    }

                    if (agent.remainingDistance <= agent.stoppingDistance)
                    {
                        Debug.Log($"Reached destination: {targetDestination.name}");
                    }
                    else
                    {
                        Debug.LogWarning($"Failed to reach destination {targetDestination.name} within 7 seconds.");
                    }
                }
                else
                {
                    Debug.LogWarning($"Destination {currentDestinationIndex} is null. Skipping...");
                }

                // Move to the next destination index or stop if all destinations are done
                if (currentDestinationIndex < destinations.Length - 1)
                {
                    currentDestinationIndex++;
                }
                else
                {
                    Debug.Log("Reached the last destination.");
                    yield break; // Stop the coroutine
                }

                // Wait for a short delay before moving to the next destination
                yield return new WaitForSeconds(3f);
            }
            else
            {
                Debug.LogWarning("No destinations available.");
                yield break; // Stop the coroutine
            }
        }
    }

    // Method to set destinations dynamically
    public void SetDestinations(Transform[] newDestinations)
    {
        destinations = newDestinations;

        // Restart walking if needed
        if (destinations.Length > 0 && agent != null)
        {
            StopAllCoroutines();
            currentDestinationIndex = 0; // Reset destination index
            StartCoroutine(WalkToDestinations());
        }
    }
}


