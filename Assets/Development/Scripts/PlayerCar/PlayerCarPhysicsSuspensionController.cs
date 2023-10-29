using System;
using UnityEngine;

namespace Development.Scripts.PlayerCar
{
    public class PlayerCarPhysicsSuspensionController : MonoBehaviour
    {
        [SerializeField] private LayerMask roadLayer;
        [SerializeField] private Transform FLWheel;
        
        [SerializeField] private  Transform FRWheel;
        [SerializeField] private Transform BLWheel;
        [SerializeField] private Transform BRWheel;
        
        [SerializeField] private float verticalOffset = 0;
        [SerializeField] private float verticalWheelVisualOffset = 0;
        [SerializeField] private float maxDistance = 0.5f;
        [SerializeField] private float minDistance = -0.5f;
        [SerializeField] private float springConstant = 1;
        [SerializeField] private float maxForce = 10;
        [SerializeField] private float dampingConstant = 0.1f;

        Rigidbody _rb;
        
        float FRLastDistance = 0;
        float BLLastDistance = 0;
        float BRLastDistance = 0;
        float FLLastDistance = 0;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        { 
            ApplyWheelSupport(FLWheel, FLLastDistance);
            ApplyWheelSupport(FRWheel, FRLastDistance);
            ApplyWheelSupport(BLWheel, BLLastDistance);
            ApplyWheelSupport(BRWheel, BRLastDistance);
    
            FLLastDistance = GetWheelDistanceFromRest(FLWheel);
            FRLastDistance = GetWheelDistanceFromRest(FRWheel);
            BLLastDistance = GetWheelDistanceFromRest(BLWheel);
            BRLastDistance = GetWheelDistanceFromRest(BRWheel);
        }
        private void ApplyWheelSupport(Transform wheel, float lastDistance)
        {
            Vector3 supportForce = GetWheelSupportForce(wheel, lastDistance);
            _rb.AddForceAtPosition(supportForce, wheel.position);
            Debug.DrawRay(wheel.position, supportForce);
        }

        public Vector3 GetWheelSupportForce(Transform wheel, float lastDistance)
        {
            float currentDistance = GetWheelDistanceFromRest(wheel);
            float dampingForce = ((currentDistance - lastDistance) / Time.fixedDeltaTime) * dampingConstant;
            float magnitude = Mathf.Min(Mathf.Max(0, -currentDistance * springConstant - dampingForce), maxForce);
            return magnitude * GetUpDir();
        }
        
        private float GetWheelDistanceFromRest(Transform wheel)
        {
            RaycastHit hit;
            float rayLength = maxDistance - minDistance;
            Vector3 rayOrigin = (wheel.position - GetUpDir() * verticalOffset) - GetUpDir() * minDistance;
            bool hasHit = Physics.Raycast(rayOrigin, -GetUpDir(), out hit, rayLength, roadLayer);
            float distanceFromRest = hasHit ? hit.distance + minDistance : 0;
            if (hasHit)
            {
                wheel.GetChild(0).transform.position = hit.point + GetUpDir() * verticalWheelVisualOffset;
                wheel.GetChild(0).transform.localPosition += Vector3.up / 2;
            }
            return distanceFromRest;
        }
        
        private Vector3 GetUpDir()
        {
            return transform.up;
        }
    }
}