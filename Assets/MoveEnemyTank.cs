using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveEnemyTank : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform destination;
    public GameObject obj;
    public GameObject explosion;
    public GameObject fire;
    public GameObject smoke;
    private ParticleSystem particleConfig;
    private bool isHit = false;

    private void FixedUpdate()
    {
        agent.SetDestination(destination.position);
        if (isHit)
        {
            fire = Instantiate(fire, transform.position, transform.localRotation);
            smoke = Instantiate(smoke, transform.position, transform.localRotation);

            isHit = false;
        }
        if(!isHit && fire != null)
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
        if(collision.gameObject.tag == "tank_bullet")
        {
            GameObject explosionAloc = Instantiate(explosion, collision.contacts[0].point, transform.localRotation);
            Destroy(explosionAloc, 1);
            Instantiate(fire, collision.contacts[0].point, transform.localRotation);
            agent.isStopped = true;
            isHit = true;
        }
    }
}
