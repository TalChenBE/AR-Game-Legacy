using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Vector3 destination;

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(destination);
    }
}
