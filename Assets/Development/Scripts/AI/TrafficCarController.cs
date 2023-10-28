using System;
using Development.Scripts.BaseClasses;
using Development.Scripts.Utilities;
using DG.Tweening;
using UnityEngine;

namespace Development.Scripts.AI
{
    public class TrafficCarController : CarBase
    {
        [SerializeField] private FOV trafficSensor;
        [SerializeField] private GameObject carBody;
        
        private float _leftTrackThreshold = -.5f;
        private float _rightTrackThreshold = .5f;
        private float changeLineInterval = .5f;

        private void OnValidate()
        {
            trafficSensor = GetComponent<FOV>();
        }

        private void Update()
        {
           ChangeLineTraffic();
        }

        private void ChangeLineTraffic()
        {
            // Check if there are no visible targets.
            if (trafficSensor.visibleTargets.Count == 0)
            {
                // No cars in sight, continue to move forward. You need to define how your car moves forward.
                MoveForward(); 
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
                        // Optional: Add logic for random lane changing or based on certain conditions.
                        DecideDirectionInMiddle();
                        break;
                
                    case TrafficLine.Right:
                        ChangeLaneSmoothly(false);// Define your method to move the car to the left.
                        break;

                    default:
                        // Optional: Handle cases where the current line isn't identified.
                        break;
                }
            }
        }

        private void MoveForward()
        {
            // Logic for moving forward. This could be continuing along a path, increasing speed, etc.
        }

        private void DecideDirectionInMiddle()
        {
            // Logic for deciding direction when in the middle lane. This can involve random choice or conditions based on game logic.
            // For instance, you could randomly choose to move left or right, or decide based on positions of other cars, etc.
            int random = UnityEngine.Random.Range(0, 2);

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

            return null;
        }
       
        
        private void ChangeLaneSmoothly(bool movingRight)
        {
            // Determine the new offset after the lane change.
            float laneOffset = 1.5f; // The distance to move left or right when changing lanes.
            float newOffsetX = SplineFollower.motion.offset.x + (movingRight ? laneOffset : -laneOffset);

            // Clamp the new offset to ensure it's within the track limits.
            newOffsetX = Mathf.Clamp(newOffsetX, -2.5f, 2.5f);

            // Animate the car's lateral movement smoothly to the new position.
            DOTween.To(() => SplineFollower.motion.offset, x => SplineFollower.motion.offset = x, new Vector2(newOffsetX, SplineFollower.motion.offset.y), changeLineInterval)
                .SetEase(Ease.Linear) // Use any easing type you prefer.
                .OnComplete(() =>
                {
                    // Optional: Execute any logic after the lane change is complete.
                });

            // Determine the rotation angle based on the movement direction.
            float rotationAngle = movingRight ? -15f : 15f; // Adjust the angle value to your preference.

            // Create a rotation effect on the car's body to give the impression of a more dynamic lane change.
            carBody.transform.DOLocalRotate(new Vector3(0f, rotationAngle, 0f), changeLineInterval / 2) // Half duration for rotation.
                .SetEase(Ease.Linear) // This ease type creates a nice, smooth transition effect.
                .OnComplete(() =>
                {
                    // Then rotate back to the original angle.
                    carBody.transform.DOLocalRotate(Vector3.zero, changeLineInterval / 2); // The other half duration to rotate back.
                });

            // If you have separate tire objects and you want them to rotate (like in steering), you can apply similar logic to them.
        }
    }
    
    public enum TrafficLine
    {
        Left,
        Middle,
        Right
    }
}