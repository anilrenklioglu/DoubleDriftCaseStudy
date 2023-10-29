using System.Collections.Generic;
using UnityEngine;

namespace Development.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CarPhysicsData", menuName = "Scriptable Objects/Cars/CarPhysicsData", order = 0)]
    public class CarPhysicsData : ScriptableObject
    {
        [Header("Car Physics Values for Drifting")]
        [Space(4)]
        
        [SerializeField] private float throttle;
        [SerializeField] private float drag;
        [SerializeField] private float driveForce;
        [SerializeField] private float activeVisualSteeringAngleEffect;
        [SerializeField] private float maxVisualSteeringSpeed;
        [SerializeField] private float maxVisualSteeringAngle;
        [SerializeField] private float maxAngularAcceleration;
        [SerializeField] private float driftAngleThreshold;
        
        public float Throttle => throttle;
        public float Drag => drag;
        public float DriveForce => driveForce;
        public float ActiveVisualSteeringAngleEffect => activeVisualSteeringAngleEffect;
        public float MaxVisualSteeringSpeed => maxVisualSteeringSpeed;
        public float MaxVisualSteeringAngle => maxVisualSteeringAngle;
        public float MaxAngularAcceleration => maxAngularAcceleration;  
        public float DriftAngleThreshold => driftAngleThreshold;
        
    }
}