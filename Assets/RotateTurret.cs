using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RotateTurret : MonoBehaviour
{
    public int rotationAngle;
    public float maxRotation;

    // Start is called before the first frame update
    void Start()
    {
        rotationAngle = 1;
        maxRotation = 45;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.X))
        {
            transform.Rotate(0, rotationAngle, 0);
        }
        if (Input.GetKey(KeyCode.Z))
        {
            transform.Rotate(0, -rotationAngle, 0);
        }
        // Clamp rotation around y-axis to a range of -45 to 45 degrees
        /*Vector3 currentRotation = transform.rotation.eulerAngles;
        currentRotation.y = Mathf.Clamp(currentRotation.y, -maxRotation, maxRotation);
        transform.rotation = Quaternion.Euler(currentRotation);*/
    }
}
