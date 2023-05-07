using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveIsraelTank : MonoBehaviour
{
    public NavMeshAgent agent;
    public Vector3 destination;
    public GameObject locationCircle;
    public MoveUserTank userTank;

    private void Start()
    {
        Vector3 locationCircleRotation = new Vector3(90, 0, 0);
        locationCircle = Instantiate(locationCircle, transform.position, Quaternion.Euler(locationCircleRotation));
        locationCircle.transform.localScale = new Vector3(4, 3, 1);
    }

    private void FixedUpdate()
    {
        locationCircle.transform.position = transform.position;
        if (userTank.isUserMoved)
        {
            agent.SetDestination(destination);
        }
    }
}
