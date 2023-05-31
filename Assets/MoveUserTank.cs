using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class MoveUserTank : MonoBehaviour
{

    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private FixedJoystick _joystick;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _moveSpeed;
    public GameObject locationCircle;
    public bool isUserMoved;
    public MoveForward moveForward;

    void Start()
    {
        _joystick.enabled = false;
        isUserMoved = false;
        _moveSpeed = 2;
        Vector3 locationCircleRotation = new Vector3(90, 0, 0);
        locationCircle = Instantiate(locationCircle, transform.position, Quaternion.Euler(locationCircleRotation));
        locationCircle.transform.localScale = new Vector3(4, 3, 1);
    }

    private void FixedUpdate()
    {
        if (moveForward.isPlay && moveForward.audioSource.isPlaying == false)
            _joystick.enabled = true;

        locationCircle.transform.position = transform.position;
        _rigidbody.velocity = new Vector3(_joystick.Horizontal * _moveSpeed, 0, _joystick.Vertical * _moveSpeed);

        if (_joystick.Horizontal != 0 || _joystick.Vertical != 0)
        {
            isUserMoved = true;
            transform.rotation = Quaternion.LookRotation(_rigidbody.velocity);
        }
    }
}
