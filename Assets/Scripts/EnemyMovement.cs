using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;


public class EnemyMovement : MonoBehaviour
{

    public UnityEngine.AI.NavMeshAgent agent;
    public Transform playerTransform;
    public float chaseRadius;
    public List<Transform> waypoints;
    private int currentWaypointIndex = 0;
    private bool chasingPlayer = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(playerTransform.position, transform.position);

        if (distanceToPlayer < chaseRadius)
        {
            chasingPlayer = true;
            agent.SetDestination(playerTransform.position);
        }
        else if (distanceToPlayer > chaseRadius && chasingPlayer)
        {
            chasingPlayer = false;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
        else if (!agent.pathPending && agent.remainingDistance < 0.5f && !chasingPlayer)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }

    }
}
