using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveIsraelTank : MonoBehaviour
{
    public NavMeshAgent agent;
    public Vector3 destination;
    public MoveUserTank userTank;

    private void FixedUpdate()
    {
        if (userTank.isUserMoved)
        {
            agent.SetDestination(destination);
        }
    }
}
