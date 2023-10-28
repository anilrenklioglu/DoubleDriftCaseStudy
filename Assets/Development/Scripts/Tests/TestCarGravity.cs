using System.Collections;
using System.Collections.Generic;
using Development.Scripts.Tests;
using UnityEngine;

public class TestCarGravity : MonoBehaviour
{
    public float acceleration = 9.8f;
    public Vector3 direction = Vector3.down;
    public float maxAngle = 30;
    Rigidbody _rb;
    TestCar _car;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _car = GetComponent<TestCar>();
    }

    void FixedUpdate()
    {
        if (_car.WheelsGrounded() || Vector3.Angle(Vector3.down, -transform.up.normalized) < maxAngle)
        {
            direction = -transform.up.normalized;
        }
        _rb.velocity += direction * acceleration * Time.fixedDeltaTime;
    }
}
