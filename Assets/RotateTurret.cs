using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTurret : MonoBehaviour
{
    public int rotationAngle;

    // Start is called before the first frame update
    void Start()
    {
        rotationAngle = 1;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0, rotationAngle, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, -rotationAngle, 0);
        }
    }
}
