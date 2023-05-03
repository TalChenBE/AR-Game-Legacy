using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveEnemyTank : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform destination;

    private void FixedUpdate()
    {
        agent.SetDestination(destination.position);
    }
}
