using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveIsraelTank : MonoBehaviour
{
    public float Time4Position;
    public float Time4Rotation;
    private float speed;
    private float rotationAngle;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(0.1f,5);
        rotationAngle = Random.Range(0.1f, 5);
        Time4Position = 0.1f;
        Time4Rotation = 1;

        // Call the TimerCallback method every 1 seconds
        InvokeRepeating("TimerCallbackPosition", 0, Time4Position);
        InvokeRepeating("TimerCallbackRotation", 0, Time4Rotation);
    }

    void TimerCallbackPosition()
    {
        Vector3 currentPosition = this.GetComponent<Rigidbody>().transform.position;
        Vector3 nextPosition = currentPosition;
        nextPosition.z += 1;
        this.GetComponent<Rigidbody>().transform.position = Vector3.MoveTowards(currentPosition, nextPosition, speed * Time.deltaTime);

    }

    void TimerCallbackRotation()
    {
        transform.Rotate(0, rotationAngle, 0);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
