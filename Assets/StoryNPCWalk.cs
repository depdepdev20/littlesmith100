using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StoryNPCWalk : MonoBehaviour
{
    [Header("Navigation")]
    public NavMeshAgent navAgent;
    private Queue<Transform> destinationsQueue = new Queue<Transform>();
    private Transform currentDestination;
    private bool isWalking = false;
    private bool isIdling = false;

    [Header("Interaction Colliders")]
    public Collider submissionCollider;
    public Collider talkingCollider;

    [Header("Idle Settings")]
    public Vector3 idleFacingDirection = Vector3.forward;

    [Header("Animation Controllers")]
    public RuntimeAnimatorController walkingAnimatorController;
    public RuntimeAnimatorController idleAnimatorController;

    [Header("Animation")]
    public Animator animator;

    private void Start()
    {
        if (navAgent == null)
        {
            navAgent = GetComponent<NavMeshAgent>();
        }

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (submissionCollider == null || talkingCollider == null)
        {
            Debug.LogWarning("One or both interaction colliders are not assigned!");
        }
    }

    public void SetDestinations(Transform[] preIdleDestinations, Transform idleDestination, Transform[] postIdleDestinations)
    {
        destinationsQueue.Clear();

        if (preIdleDestinations != null)
        {
            foreach (var destination in preIdleDestinations)
            {
                destinationsQueue.Enqueue(destination);
            }
        }

        if (idleDestination != null)
        {
            destinationsQueue.Enqueue(idleDestination);
        }

        if (postIdleDestinations != null)
        {
            foreach (var destination in postIdleDestinations)
            {
                destinationsQueue.Enqueue(destination);
            }
        }

        SetInteractionEnabled(false);

        StartCoroutine(WalkToDestinations());
    }

    private IEnumerator WalkToDestinations()
    {
        isWalking = true;
        UpdateAnimation();

        while (destinationsQueue.Count > 0)
        {
            currentDestination = destinationsQueue.Dequeue();
            navAgent.SetDestination(currentDestination.position);

            while (navAgent.pathPending || navAgent.remainingDistance > navAgent.stoppingDistance)
            {
                yield return null;
            }

            yield return new WaitForSeconds(0.1f);

            if (navAgent.velocity.sqrMagnitude > 0.01f)
            {
                yield return new WaitUntil(() => navAgent.velocity.sqrMagnitude <= 0.01f);
            }

            if (destinationsQueue.Count == 0)
            {
                StartIdling();
                yield break;
            }
        }

        isWalking = false;
        UpdateAnimation();
    }

    private void StartIdling()
    {
        isIdling = true;

        navAgent.isStopped = true;
        navAgent.updateRotation = false;
        navAgent.velocity = Vector3.zero;

        Quaternion targetRotation = Quaternion.LookRotation(idleFacingDirection, Vector3.up);
        transform.rotation = targetRotation;

        navAgent.ResetPath();
        SetInteractionEnabled(true);

        // Switch to idle animation controller
        if (idleAnimatorController != null && animator.runtimeAnimatorController != idleAnimatorController)
        {
            animator.runtimeAnimatorController = idleAnimatorController;
        }

        UpdateAnimation();
    }

    public void ResumeWalking(Transform[] postIdleDestinations)
    {
        destinationsQueue.Clear();

        if (postIdleDestinations != null)
        {
            foreach (var destination in postIdleDestinations)
            {
                destinationsQueue.Enqueue(destination);
            }
        }

        navAgent.updateRotation = true;

        isIdling = false;
        SetInteractionEnabled(false);

        // Switch back to walking animation controller
        if (walkingAnimatorController != null && animator.runtimeAnimatorController != walkingAnimatorController)
        {
            animator.runtimeAnimatorController = walkingAnimatorController;
        }

        UpdateAnimation();
        StartCoroutine(WalkToDestinations());
    }

    private void SetInteractionEnabled(bool isEnabled)
    {
        if (submissionCollider != null)
        {
            submissionCollider.enabled = isEnabled;
        }

        if (talkingCollider != null)
        {
            talkingCollider.enabled = isEnabled;
        }
    }

    private void UpdateAnimation()
    {
        if (animator == null) return;

        animator.SetBool("IsWalking", isWalking);
        animator.SetBool("IsIdling", isIdling);
    }

    public bool IsMoving()
    {
        return navAgent.velocity.sqrMagnitude > 1f;
    }

    public NavMeshAgent GetNavMeshAgent()
    {
        return navAgent;
    }

    public Vector3 GetCurrentDestination()
    {
        return currentDestination != null ? currentDestination.position : Vector3.zero;
    }
}
