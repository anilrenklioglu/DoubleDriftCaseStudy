using System;
using System.Collections.Generic;
using Development.Scripts.Utilities;
using UnityEngine;

namespace Development.Scripts.Tests
{
    public class TestCar : MonoBehaviour
    {
        [SerializeField] private float throttle;

        [Header("Car Body")]
        
        [SerializeField] private Transform centerOfMass;

        [SerializeField] private Transform groundTrigger;
        
        public LayerMask wheelCollidables;

        [SerializeField] private float drag;
        
        [SerializeField] private float driveForce;
        
        [Header("Suspension and Steering")]
        public float activeVisualSteeringAngleEffect = 1;
        public float maxVisualSteeringSpeed = 1;
        public float maxVisualSteeringAngle = 30;
        public float maxAngularAcceleration = 30;    // degrees per second
        public List<Transform> steeringWheels;
        
        private Rigidbody _rb;
        private InputReader _inputReader;
        public float driftAngleThreshold = 10.0f;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _inputReader = new InputReader();
        }
        
        void Update()
    {
        // Point wheels
        float wheelAngle = -Vector3.Angle(_rb.velocity.normalized, GetDriveDirection()) * Vector3.Cross(_rb.velocity.normalized, GetDriveDirection()).y;
        wheelAngle = Mathf.Min(Mathf.Max(-maxVisualSteeringAngle, wheelAngle), maxVisualSteeringAngle);
        PointDriveWheelsAt(wheelAngle);
        ClampPosition();

    }

        private void ClampPosition()
        {
            // Get the current position of the car.
            Vector3 position = transform.position;

            // Adjust the x position to be within the range -2.5 to 2.5.
            position.x = Mathf.Clamp(position.x, -2.5f, 2.5f);

            // Set the position back.
            transform.position = position;
        }
    private float GetRawDriftAngle() {
        if (! WheelsGrounded()) return 0;
        return Vector3.Angle(_rb.velocity.normalized, GetDriveDirection()) * Vector3.Cross(_rb.velocity.normalized, GetDriveDirection()).y;
    }

    public float GetDriftAngle() 
    {
        return GetRawDriftAngle();
    } 

    public bool IsFrontWheelDrift() {
        return Mathf.Abs(GetDriftAngle()) > maxVisualSteeringAngle;
    }

    public bool IsDrifting() {
        return Mathf.Abs(GetDriftAngle()) > driftAngleThreshold;
    }

    void FixedUpdate()
    {
        // Body adjustments.
        _rb.centerOfMass = centerOfMass.localPosition;  // Adjusting each frame allows for live editing in the inspector.
        _rb.AddForce(-GetDragForce() * _rb.velocity.normalized);

        // If rear wheels are on the ground, the car can apply forces for movement and steering.
        if (WheelsGrounded())
        {
            // Engine force.
            _rb.AddForce(GetDriveDirection() * GetDriveForce());

            // Steering control.
            _rb.angularVelocity += -transform.up * GetSteeringAngularAcceleration() * Time.fixedDeltaTime;
        }

        // When drifting, control the maximum rotation along the y-axis to 45 degrees.
        if (IsDrifting())
        {
            // Current rotation.
            Quaternion currentRotation = transform.rotation;

            // Determine the left and right boundary for the 45-degree clamping.
            Quaternion leftBoundaryRotation = Quaternion.Euler(0, -45, 0);
            Quaternion rightBoundaryRotation = Quaternion.Euler(0, 45, 0);

            // Choose the closest boundary to determine the direction to clamp towards.
            Quaternion targetRotation = (Quaternion.Angle(currentRotation, leftBoundaryRotation) < Quaternion.Angle(currentRotation, rightBoundaryRotation)) ? leftBoundaryRotation : rightBoundaryRotation;

            // Smoothly rotate towards the target rotation.
            Quaternion newRotation = Quaternion.RotateTowards(currentRotation, targetRotation, maxAngularAcceleration * Time.fixedDeltaTime);

            // Apply the rotation if it's within the limits, to prevent overshooting.
            if (Quaternion.Angle(newRotation, leftBoundaryRotation) >= 0 && Quaternion.Angle(newRotation, rightBoundaryRotation) <= 0)
            {
                transform.rotation = newRotation;
            }
        }
    }


    /// Point the drive wheels at angle
    /// angle relative to car direction
    /// angle = 0 means wheels point forward
    /// Does it smoothly
    void PointDriveWheelsAt(float targetAngle)
    {
        foreach (Transform wheel in steeringWheels)
        {
            float currentAngle = wheel.localEulerAngles.y;
            float change = ((((targetAngle - currentAngle) % 360) + 540) % 360) - 180;
            float newAngle = currentAngle + change * Time.deltaTime * maxVisualSteeringSpeed;
            wheel.localEulerAngles = new Vector3(0, newAngle, 0);
        }
    }

    /// Are the drive wheels grounded
    /// Can the car accelerate?
    public bool WheelsGrounded()
    {
        return Physics.OverlapBox(groundTrigger.position, groundTrigger.localScale / 2, Quaternion.identity, wheelCollidables).Length > 0;
    }

    /// How fast do we spin car?
    float GetSteeringAngularAcceleration()
    {
        return GetSteering() * maxAngularAcceleration * Mathf.PI / 180;
    }

    /// How much should we be turning?
    /// Between -1 and 1
    float GetSteering()
    {
        // Retrieve the current input from the InputReader instance
        Vector3 currentInput = _inputReader.GetCurrentInput();

        // Return the horizontal component of the input (which is X in your Vector3 structure)
        return Mathf.Clamp(currentInput.x, -1, 1);
    }

    /// What way car pointing
    /// Is normalized
    Vector3 GetDriveDirection()
    {
        return _rb.transform.forward.normalized;
    }

    /// How many beans will the car push itself with
    /// in newtown
    float GetDriveForce()
    {
        return driveForce * throttle;
    }

    /// Magnitude of drag
    /// velocity squared times drag coefficient
    /// Uses overall velocity, doesn't care about what direction car pointing
    float GetDragForce()
    {
        return Mathf.Pow(_rb.velocity.magnitude, 2) * drag;
    }
    }
}
