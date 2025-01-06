using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerNPCWalk : MonoBehaviour
{
    [Header("Navigation")]
    public NavMeshAgent navAgent;
    public float destinationTimeout = 10f; // Time in seconds to wait before skipping a destination

    [Header("Idle Settings")]
    public float idleDuration = 2f; // Time in seconds to idle at each destination
    public Vector3 idleFacingDirection = Vector3.forward;

    [Header("Animation")]
    public Animator animator;

    private List<Transform> destinationsList = new List<Transform>();
    private Transform destructionDestination;
    private bool isWalking = false;
    private bool isIdling = false;

    private void Start()
    {
        if (navAgent == null)
            navAgent = GetComponent<NavMeshAgent>();

        if (animator == null)
            animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Update animation state based on movement
        if (navAgent.velocity.sqrMagnitude > 0.01f && !isWalking)
        {
            isWalking = true;
            isIdling = false;
            UpdateAnimation();
        }
        else if (navAgent.velocity.sqrMagnitude <= 0.01f && !isIdling)
        {
            isWalking = false;
            isIdling = true;
            UpdateAnimation();
        }
    }

    public void SetDestinations(Transform[] destinations, Transform destructionDestination)
    {
        // Assign destinations and randomize the order
        this.destinationsList = new List<Transform>(destinations);
        ShuffleList(destinationsList);
        this.destructionDestination = destructionDestination;

        StartCoroutine(WalkToDestinations());
    }

    private IEnumerator WalkToDestinations()
    {
        foreach (var destination in destinationsList)
        {
            navAgent.SetDestination(destination.position);

            float timer = 0f;
            while (navAgent.pathPending || navAgent.remainingDistance > navAgent.stoppingDistance)
            {
                timer += Time.deltaTime;
                if (timer > destinationTimeout) break; // Skip destination if timeout is reached
                yield return null;
            }

            // Wait for idle duration if destination was reached
            if (timer <= destinationTimeout)
            {
                yield return StartCoroutine(IdleAtDestination());
            }
        }

        // After visiting all destinations, move to destruction point
        if (destructionDestination != null)
        {
            navAgent.SetDestination(destructionDestination.position);
            while (navAgent.pathPending || navAgent.remainingDistance > navAgent.stoppingDistance)
            {
                yield return null;
            }
        }

        // Destroy the NPC after reaching the destruction point
        Destroy(gameObject);
    }

    private IEnumerator IdleAtDestination()
    {
        navAgent.isStopped = true;
        navAgent.velocity = Vector3.zero;

        Quaternion targetRotation = Quaternion.LookRotation(idleFacingDirection, Vector3.up);
        transform.rotation = targetRotation;

        yield return new WaitForSeconds(idleDuration);

        navAgent.isStopped = false;
    }

    private void UpdateAnimation()
    {
        if (animator == null) return;

        animator.SetBool("IsWalking", isWalking);
        animator.SetBool("IsIdling", isIdling);
    }

    private void ShuffleList(List<Transform> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Transform temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
