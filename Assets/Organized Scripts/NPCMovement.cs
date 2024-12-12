using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
    NavMeshAgent agent;
    public Transform[] waypoints;     // Waypoints for the NPC's forward path
    int waypointIndex;
    Vector3 target;

    private void Start()
    {
      agent=GetComponent<NavMeshAgent>();
        UpdateDestination();
    }
    private void Update()
    {
        if(Vector3.Distance(transform.position,target)<1)
        {
            IterateWaypointIndex();
            UpdateDestination();
        }
    }
    void UpdateDestination()
    {
        target=waypoints[waypointIndex].position;
        agent.SetDestination(target);
    }
    void IterateWaypointIndex()
    {
        waypointIndex++;
        if(waypointIndex >= waypoints.Length)
        {
            waypointIndex = 0;
        }
    }
}
