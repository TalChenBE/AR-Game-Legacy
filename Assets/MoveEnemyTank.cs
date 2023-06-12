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
    public GameObject locationCircle;
    private ParticleSystem particleConfig;
    public HideVideo hideVideo;
    private bool isHit = false;

    private void Start()
    {
        Vector3 locationCircleRotation = new Vector3(90, 0, 0);
        locationCircle = Instantiate(locationCircle, transform.position, Quaternion.Euler(locationCircleRotation));
        locationCircle.transform.localScale = new Vector3(4, 3, 1);
    }

    private void FixedUpdate()
    {
        if (hideVideo.isDestroy == false)
            return;
        locationCircle.transform.position = transform.position;
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
