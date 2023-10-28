using System.Collections.Generic;
using Development.Scripts.BaseClasses;
using Development.Scripts.Managers;
using Development.Scripts.Utilities;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

namespace Development.Scripts.AI
{
    public class TrafficCarController : CarBase
    {
        [SerializeField] private FOV trafficSensor;
        [SerializeField] private GameObject carBody;

        private float _leftTrackThreshold = -.5f;
        private float _rightTrackThreshold = .5f;
        private float changeLineInterval = .5f;
       
        private bool isChangingLanes = false; // Flag indicating whether the car is changing lanes.
        
        private Vector3 targetLanePosition; // Target position when changing lanes.
        
        private void OnValidate()
        {
            if (trafficSensor == null)
            {
                trafficSensor = GetComponent<FOV>();
            }
        }

        private void Update()
        {
            if(GameManager.Instance.CurrentState != GameState.Playing) return;

            // If the car is changing lanes, continue moving towards the target position.
            if (isChangingLanes)
            {
                // Calculate the lateral step for lane changing.
                float lateralStep = BaseMoveSpeed * Time.deltaTime;
        
                // Move the car laterally towards the target X position.
                Vector3 newPosition = transform.position;
                newPosition.x = Mathf.MoveTowards(newPosition.x, targetLanePosition.x, lateralStep);
                transform.position = newPosition;

                // Check if the car has reached the target X position.
                if (Mathf.Abs(transform.position.x - targetLanePosition.x) < 0.001f)
                {
                    isChangingLanes = false; // Reset the flag when lane change is completed.
                }
            }
            else
            {
                ChangeLineTraffic(); // Check if we need to start changing lanes.
            }

            // Continue moving forward. This is separate from the lateral lane-changing movement.
            transform.Translate(Time.deltaTime * BaseMoveSpeed * Vector3.forward);
        }


        private void ChangeLineTraffic()
        {
            if (isChangingLanes) 
                return; // If currently changing lanes, do not proceed to check for other cars.

            if (trafficSensor.visibleTargets.Count == 0)
            {
                // No cars in sight, continue to move forward.
            }
            else
            {
                // Cars are visible, decide on lane change based on current position.
                switch (GetCurrentTrafficLine())
                {
                    case TrafficLine.Left:
                        ChangeLaneSmoothly(true);
                        break;

                    case TrafficLine.Middle:
                        DecideDirectionInMiddle();
                        break;

                    case TrafficLine.Right:
                        ChangeLaneSmoothly(false);
                        break;

                    default:
                        // Optional: Handle cases where the current line isn't identified.
                        break;
                }
            }
        }

        private void DecideDirectionInMiddle()
        {
            int random = Random.Range(0, 2);
            if (random == 0)
            {
                ChangeLaneSmoothly(false);
            }
            else
            {
                ChangeLaneSmoothly(true);
            }
        }

        private TrafficLine? GetCurrentTrafficLine()
        {
            var x = transform.position.x;
            float deadZone = 0.1f;

            if (x <= _leftTrackThreshold - deadZone)
            {
                return TrafficLine.Left;
            }
            else if (x > _leftTrackThreshold + deadZone && x < _rightTrackThreshold - deadZone)
            {
                return TrafficLine.Middle;
            }
            else if (x >= _rightTrackThreshold + deadZone)
            {
                return TrafficLine.Right;
            }

            return null; // Unidentified line position.
        }

        private void ChangeLaneSmoothly(bool movingRight)
        {
            // Set the flag when starting to change lanes.
            isChangingLanes = true;

            // Determine the new position based on the direction of lane change.
            float laneOffset = 2.5f; // The distance to move left or right when changing lanes.
            float newPosX = transform.position.x + (movingRight ? laneOffset : -laneOffset);

            // Clamp the new position to ensure it's within the road boundaries, if necessary.
            newPosX = Mathf.Clamp(newPosX, -2.5f, 2.5f);  // Assumes road width boundaries are -2.5 and 2.5.

            // Set the target position to the new X coordinate, while keeping the Y and Z coordinates the same.
            targetLanePosition = new Vector3(newPosX, transform.position.y, transform.position.z);

           
            /*float rotationAngle = movingRight ? -25f : 25f; // Change this value to adjust the tilt.
            carBody.transform.localEulerAngles = new Vector3(0f, rotationAngle, 0f);*/

            // After a delay or at the end of the movement, you might want to reset the car's rotation.
            // This logic would go where you handle the completion of the lane change.
        }
    }
}
