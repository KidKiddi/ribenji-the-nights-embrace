using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;


public class EnemyMovement : MonoBehaviour
{

    public UnityEngine.AI.NavMeshAgent agent;
    public Transform playerTransform;
    // Start is called before the first frame update
    void Start()
    {
       
       
    }

    // Update is called once per frame
    void Update()
    {
       agent.SetDestination(playerTransform.position);
    }
}
