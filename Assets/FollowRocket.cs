using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowRocket : MonoBehaviour
{
    public GameObject rocketTrail;
    public GameObject createdRocketTrail;

    public void StartObject(GameObject rocketTrail, float speed)
    {
        this.rocketTrail = rocketTrail;
        Vector3 rotation = transform.localRotation.eulerAngles;
        rotation.y -= 90;
        createdRocketTrail = Instantiate(rocketTrail, transform.localPosition, Quaternion.Euler(rotation));
        ParticleSystem rocket = createdRocketTrail.GetComponent<ParticleSystem>();
        var main = rocket.main;
        main.startSpeed = 52;
        main.startLifetime = 100;
        ParticleSystem.ShapeModule curve = rocket.shape;
        curve.position = transform.localPosition;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(createdRocketTrail, 0.0001f);
    }
}
