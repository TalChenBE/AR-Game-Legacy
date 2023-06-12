using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveIsraelTank : MonoBehaviour
{
    public NavMeshAgent agent;
    public Vector3 destination;
    private Vector3 dest;
    public GameObject locationCircle;
    public MoveUserTank userTank;
    public float lookRadius = 5f;

    private ParticleSystem particleConfig;
    public GameObject explosion;
    public GameObject fire;
    public GameObject smoke;

    private bool isHit = false;

    private void Start()
    {
        Vector3 locationCircleRotation = new Vector3(90, 0, 0);
        locationCircle = Instantiate(locationCircle, transform.position, Quaternion.Euler(locationCircleRotation));
        locationCircle.transform.localScale = new Vector3(4, 3, 1);
        dest = transform.localPosition;
    }

    private void FixedUpdate()
    {
        locationCircle.transform.position = transform.position;

        if (!isHit && userTank.isUserMoved)
        {
            float distance = Vector3.Distance(dest, transform.position);

            if (distance <= lookRadius)
            {
                dest = new Vector3(Random.Range(-50, 51), dest.y, Random.Range(-40, 61));
                agent.SetDestination(dest);
            }
        }

        if(isHit && fire != null)
        {
            particleConfig = fire.GetComponent<ParticleSystem>();
            var particleConfigMain = particleConfig.main;
            particleConfigMain.startSize = 5;
            fire.transform.position = transform.localPosition;
            smoke.transform.position = transform.localPosition;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "tank_bullet_enemy")
        {
            GameObject explosionAloc = Instantiate(explosion, collision.contacts[0].point, transform.localRotation);
            Destroy(explosionAloc, 1);
            Instantiate(fire, collision.contacts[0].point, transform.localRotation);
            agent.isStopped = true;
            isHit = true;
        }
    }
}
