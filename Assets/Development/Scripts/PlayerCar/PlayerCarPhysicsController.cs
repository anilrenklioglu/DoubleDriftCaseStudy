using System;
using System.Collections.Generic;
using Development.Scripts.Managers;
using Development.Scripts.ScriptableObjects;
using Development.Scripts.Utilities;
using UnityEngine;

namespace Development.Scripts.PlayerCar
{
    public class PlayerCarPhysicsController : MonoBehaviour
    {
        [SerializeField] private CarPhysicsData _carPhysicsData;
        
        private Rigidbody _rb;
        private InputReader _inputReader;
        
        private float throttle;
        private float drag;
        private float driveForce;
        private float activeVisualSteeringAngleEffect;
        private float maxVisualSteeringSpeed;
        private float maxVisualSteeringAngle;
        private float maxAngularAcceleration;
        private float driftAngleThreshold;
        
        [Header("Car Body Components")]
        [Space(4)]
        
        [SerializeField] private List<Transform> steeringWheels;
        [SerializeField] private Transform centerOfMass;
        [SerializeField] private Transform groundTrigger;
        [SerializeField] private LayerMask wheelCollidables;
        private void Init()
        {
            throttle = _carPhysicsData.Throttle;
            drag = _carPhysicsData.Drag;
            driveForce = _carPhysicsData.DriveForce;
            activeVisualSteeringAngleEffect = _carPhysicsData.ActiveVisualSteeringAngleEffect;
            maxVisualSteeringSpeed = _carPhysicsData.MaxVisualSteeringSpeed;
            maxVisualSteeringAngle = _carPhysicsData.MaxVisualSteeringAngle;
            maxAngularAcceleration = _carPhysicsData.MaxAngularAcceleration;
            driftAngleThreshold = _carPhysicsData.DriftAngleThreshold;
        }

        private void Awake()
        {
            Init();
            
            _rb = GetComponent<Rigidbody>();
            _inputReader = new InputReader();
        }


        private void Update()
        {
            if (GameManager.Instance.CurrentState != GameState.Playing) return;
          
            float wheelAngle = -Vector3.Angle(_rb.velocity.normalized, GetDriveDirection()) * Vector3.Cross(_rb.velocity.normalized, GetDriveDirection()).y;
        
            wheelAngle = Mathf.Min(Mathf.Max(-maxVisualSteeringAngle, wheelAngle), maxVisualSteeringAngle);
        
            PointDriveWheelsAt(wheelAngle);
        
            ClampPosition();

        }

        //Adjusts the car's position in the game world, specifically the x-coordinate
        private void ClampPosition()
        {
            Vector3 position = transform.position;

            position.x = Mathf.Clamp(position.x, -2.5f, 2.5f);

            transform.position = position;
        }
        //Calculate the car's drift angle, which is the angle between the car's velocity and the direction it's facing
        private float GetRawDriftAngle()
        {
            if (! WheelsGrounded()) return 0;
            return Vector3.Angle(_rb.velocity.normalized, GetDriveDirection()) * Vector3.Cross(_rb.velocity.normalized, GetDriveDirection()).y;
        }

        private float GetDriftAngle() 
        {
            return GetRawDriftAngle();
        } 

        //Checks if the drift angle exceeds a certain threshold
        public bool IsFrontWheelDrift()
        {
            return Mathf.Abs(GetDriftAngle()) > maxVisualSteeringAngle;
        }

        private bool IsDrifting()
        {
            return Mathf.Abs(GetDriftAngle()) > driftAngleThreshold;
        }

        private void FixedUpdate()
        {
            if (GameManager.Instance.CurrentState != GameState.Playing) return;
            
            _rb.centerOfMass = centerOfMass.localPosition;
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
                Quaternion currentRotation = transform.rotation;
                Quaternion leftBoundaryRotation = Quaternion.Euler(0, -45, 0);
                Quaternion rightBoundaryRotation = Quaternion.Euler(0, 45, 0);

                Quaternion targetRotation = (Quaternion.Angle(currentRotation, leftBoundaryRotation) < Quaternion.Angle(currentRotation, rightBoundaryRotation)) ? leftBoundaryRotation : rightBoundaryRotation;

                Quaternion newRotation = Quaternion.RotateTowards(currentRotation, targetRotation, maxAngularAcceleration * Time.fixedDeltaTime);

                if (Quaternion.Angle(newRotation, leftBoundaryRotation) >= 0 && Quaternion.Angle(newRotation, rightBoundaryRotation) <= 0)
                {
                    transform.rotation = newRotation;
                }
            } 
        }

        //Gradually adjusts the steering wheels' orientation towards a target angle, creating a smooth steering visual effect.
        private void PointDriveWheelsAt(float targetAngle)
        {
            foreach (Transform wheel in steeringWheels)
            {
                float currentAngle = wheel.localEulerAngles.y;
                float change = ((((targetAngle - currentAngle) % 360) + 540) % 360) - 180;
                float newAngle = currentAngle + change * Time.deltaTime * maxVisualSteeringSpeed;
                wheel.localEulerAngles = new Vector3(0, newAngle, 0);
            }
        }
        
        //Checks whether the drive wheels are in contact with the ground, influencing whether certain forces or controls should be applied
        public bool WheelsGrounded()
        {
            return Physics.OverlapBox(groundTrigger.position, groundTrigger.localScale / 2, Quaternion.identity, wheelCollidables).Length > 0;
        }
        
        //Computes the angular acceleration for steering based on input and certain physics parameters. This helps in turning the car.
        private float GetSteeringAngularAcceleration()
        {
            return GetSteering() * maxAngularAcceleration * Mathf.PI / 180;
        }

        private float GetSteering()
        {
            Vector3 currentInput = _inputReader.GetCurrentInput();
            return Mathf.Clamp(currentInput.x, -1, 1);
        }
        
        Vector3 GetDriveDirection()
        {
            return _rb.transform.forward.normalized;
        }

        private float GetDriveForce()
        {
            return driveForce * throttle;
        }

        /// <summary>
        /// The reason for squaring the velocity here is based on the physics formula for drag force, which is proportional to the square of speed.
        /// This relationship implies that as the car goes faster, the resistance (drag force) increases exponentially
        /// </summary>
        /// <returns></returns>
        
        //Computes the resistance force acting against the car based on its velocity. It simulates real-world drag or air resistance.
        private float GetDragForce()
        {
            return Mathf.Pow(_rb.velocity.magnitude, 2) * drag;
        }
    }
}