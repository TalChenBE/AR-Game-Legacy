using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowRocket : MonoBehaviour
{
    public GameObject rocketTrail;
    public GameObject createdRocketTrail;

    public void StartObject(GameObject rocketTrail, float speed)
    {
        float initXRotation = 89.98f;
        this.rocketTrail = rocketTrail;
        Vector3 rotation = transform.localRotation.eulerAngles;
        rotation.y += 90;
        rotation.x -= initXRotation;
        ParticleSystem rocket = rocketTrail.GetComponent<ParticleSystem>();
        var main = rocket.main;
        main.duration = 50;
        main.startSpeed = 52;
        main.startLifetime = 250;
        ParticleSystem.ShapeModule curve = rocket.shape;
        curve.position = transform.localPosition;
        createdRocketTrail = Instantiate(rocketTrail, transform.localPosition, Quaternion.Euler(rotation));
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(createdRocketTrail, 0.0001f);
    }
}
