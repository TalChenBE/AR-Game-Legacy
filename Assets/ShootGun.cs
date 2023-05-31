using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootGun : MonoBehaviour
{
    public GameObject prefeb;
    public GameObject smallExplosion;
    public GameObject rocketTrail;
    public float initXRotation;
    [SerializeField] public Button shootBtn;
    public float speed;
    public float yRotation;
    public MoveForward moveForward;
    void Start()
    {
        shootBtn.enabled = false;
        /*  Quaternion rotations = transform.rotation;
           transform.rotation = Quaternion.AngleAxis(r, toEnemy);*/
        shootBtn.onClick.AddListener(createTankBullet);
        // Set the emission rate of the particle system
    }

    private void FixedUpdate()
    {
        if (moveForward.isPlay && moveForward.audioSource.isPlaying == false)
            shootBtn.enabled = true;
    }

    void createTankBullet()
    {
        Vector3 gunRotation = transform.rotation.eulerAngles;
        gunRotation.y += 90;
        Vector3 gunPosition = transform.position;
        GameObject roccket = Instantiate(prefeb, gunPosition, Quaternion.Euler(gunRotation));
        GameObject createdExplosion = Instantiate(smallExplosion, gunPosition, Quaternion.Euler(gunRotation));
        MeshRenderer mr = roccket.GetComponent<MeshRenderer>();
        Color color = mr.material.color;
        color.a = 0.0f;
        FollowRocket a = roccket.AddComponent<FollowRocket>();
        a.StartObject(rocketTrail, speed);
        Destroy(createdExplosion, 0.5f);
        roccket.tag = "tank_bullet";
        Rigidbody roccektRigid = roccket.AddComponent<Rigidbody>();
        roccket.AddComponent<BoxCollider>();
        roccektRigid.AddForce(-transform.up * speed);
        Destroy(roccket, 1);
        Destroy(a.createdRocketTrail, 2.5f);
    }

   /* // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 gunRotation = transform.rotation.eulerAngles;
            gunRotation.y += 90;
           Vector3 gunPosition = transform.position;
            *//* Debug.Log($"gunRotation={gunRotation}");

            if (90 == gunRotation.y)
                gunPosition.z += 4;

            if (90 < gunRotation.y && gunRotation.y < 180)
            {
                gunPosition.z += 4 * Mathf.Cos(gunRotation.y - 90);
                gunPosition.x += 4 * Mathf.Sin(gunRotation.y - 90);
            }
*/

          /*  gunPosition.z += 4 * Mathf.Cos(gunRotation.y);
            gunPosition.x += 4 * Mathf.Sin(gunRotation.y);
            Debug.Log($"gunPosition={gunPosition}");*/

            /*        Vector3 normalizedPos = new Vector3(4.5f * Mathf.Cos(gunRotation.y), gunPosition.y, 4.5f * Mathf.Sin(gunRotation.y));
                    normalizedPos.x += gunPosition.x;
                    normalizedPos.z += gunPosition.z;*/




            /*            Debug.Log($"gunPosition={gunPosition}");
                        Vector3 normalizedPos = gunPosition.normalized;
                        Debug.Log($"normalizedPos={normalizedPos}");
                        normalizedPos.x *= 4.5f * gunPosition.x;
                        normalizedPos.z *= 4.5f * gunPosition.z;*//*

            GameObject roccket = Instantiate(prefeb, gunPosition, Quaternion.Euler(gunRotation));
            roccket.tag = "tank_bullet";
            Rigidbody roccektRigid = roccket.AddComponent<Rigidbody>();
            roccket.AddComponent<BoxCollider>();
            roccektRigid.AddForce(transform.forward * speed);
            Destroy(roccket, 1);
        }
    }*/
}
