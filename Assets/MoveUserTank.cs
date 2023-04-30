using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUserTank : MonoBehaviour
{
    public float speed;
    public int rotationAngle;

    // Start is called before the first frame update
    void Start()
    {
        speed = 2;
        rotationAngle = 1;

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentPosition = this.GetComponent<Rigidbody>().transform.position;
        Vector3 nextPosition = currentPosition;

        if (Input.GetKey(KeyCode.DownArrow)) {
            nextPosition.z -= 1;
            this.GetComponent<Rigidbody>().transform.position = Vector3.MoveTowards(currentPosition, nextPosition, speed*Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.UpArrow)) {
            nextPosition.z += 1;
            this.GetComponent<Rigidbody>().transform.position = Vector3.MoveTowards(currentPosition, nextPosition, speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            transform.Rotate(0, rotationAngle, 0);
        }
        if (Input.GetKey(KeyCode.LeftArrow)) {
            transform.Rotate(0, -rotationAngle, 0);
        }
    }
}
