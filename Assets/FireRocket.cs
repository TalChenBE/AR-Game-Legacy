using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRocket : MonoBehaviour
{
    private Rigidbody rigidbody;
    public Transform target;
    private bool isMove;

    // Start is called before the first frame update
    void Start()
    {
        /*Vector3 offset = new Vector3(0, 0, 4);
        transform.position = target.position + offset;*/
        rigidbody = this.GetComponent<Rigidbody>();
        isMove = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (isMove && Input.GetKey(KeyCode.Space))
        {
            rigidbody.velocity = new Vector3(0, 0, 20);
            rigidbody.useGravity = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject != target)
        {
            isMove = false;
        }
    }
}
