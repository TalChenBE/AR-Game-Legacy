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
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(x, 0, z);

        transform.Translate(movement * speed * Time.deltaTime);


        if (x != 0)
        {
            // rotate the object according to its movment in the Horizontal axis
            int side = x < 0 ? -1 : 1;
            transform.Rotate(0, rotationAngle * side, 0);
        }
    }
}
