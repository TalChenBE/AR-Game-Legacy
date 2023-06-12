using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour
{
    public GameObject prefeb;
    public GameObject smallExplosion;
    public GameObject rocketTrail;
    public MoveEnemyTank enemyTank;
    public Collider tankColider;
    public float speed;

    void Start()
    {
        InvokeRepeating("Fire", 15f, Random.Range(5, 20));
    }

    void Fire()
    {
        if (enemyTank.hideVideo.isDestroy == false || enemyTank.agent.isStopped)
            return;

        Vector3 gunRotation = transform.rotation.eulerAngles;
        gunRotation.x = 90;
        gunRotation.y -= 90;
        Vector3 gunPosition = transform.position;
        GameObject roccket = Instantiate(prefeb, gunPosition, Quaternion.Euler(gunRotation));
        GameObject createdExplosion = Instantiate(smallExplosion, gunPosition, Quaternion.Euler(gunRotation));
        MeshRenderer mr = roccket.GetComponent<MeshRenderer>();
        Color color = mr.material.color;
        color.a = 0.0f;
        FollowRocket a = roccket.AddComponent<FollowRocket>();
        a.StartObject(rocketTrail, speed);
        Destroy(createdExplosion, 0.5f);
        roccket.tag = "tank_bullet_enemy";
        Rigidbody roccektRigid = roccket.AddComponent<Rigidbody>();
        roccket.AddComponent<BoxCollider>();
        Physics.IgnoreCollision(tankColider, roccket.GetComponent<Collider>());
        roccektRigid.AddForce(transform.forward * speed);
        Destroy(roccket, 1);
        Destroy(a.createdRocketTrail, 2.5f);
        
    }
}
