using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPCWalk : MonoBehaviour
{
    private Transform[] destinations; // Array of destinations
    private NavMeshAgent agent;
    private int currentDestinationIndex = 0;
    private float waitDuration = 5f; // Default wait duration

    // Animation
    private Animator animator;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

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
                Transform targetDestination = destinations[currentDestinationIndex];

                if (targetDestination != null)
                {
                    // Start walking animation
                    SetAnimationState(true);

                    agent.SetDestination(targetDestination.position);
                    yield return new WaitUntil(() => agent.remainingDistance <= agent.stoppingDistance);

                    // Switch to idle animation
                    SetAnimationState(false);

                    // Wait at destination
                    yield return new WaitForSeconds(waitDuration);
                }

                // Move to the next destination
                currentDestinationIndex = (currentDestinationIndex + 1) % destinations.Length;
            }
            else
            {
                Debug.LogWarning("No destinations available.");
                yield break;
            }
        }
    }

    public void SetDestinations(Transform[] newDestinations)
    {
        destinations = newDestinations;

        if (destinations.Length > 0 && agent != null)
        {
            StopAllCoroutines();
            currentDestinationIndex = 0;
            StartCoroutine(WalkToDestinations());
        }
    }

    public void SetWaitDuration(float duration)
    {
        waitDuration = duration;
    }

    private void SetAnimationState(bool isWalking)
    {
        if (animator != null)
        {
            animator.SetBool("isWalking", isWalking);
            animator.SetBool("isIdle", !isWalking);
        }
    }
}
