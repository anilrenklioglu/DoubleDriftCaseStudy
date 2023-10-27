using System;
using DG.Tweening;
using Development.Scripts.BaseClasses;
using Development.Scripts.Utilities;
using UnityEngine;

namespace Development.Scripts.PlayerCar
{
    public class PlayerCarController : CarBase
    {
        // Variables for car movement
        [SerializeField] private GameObject carBody; // The physical object of the car that will be rotated during drifting.

        // Tweaking variables for car control, expose them in the editor so you can modify without changing the code.
        [Header("Car Control for Drifting")]
        [SerializeField] private float maxDriftAngle = 25f; // Maximum angle the car can reach while drifting.
        [SerializeField] private float rotationDuration = 0.3f; // How quickly the car reaches the maximum drift angle.
        [SerializeField] private Ease rotationEase = Ease.OutQuart; // Type of easing for the rotation animation.

        [Header("Car Tires")] 
        [SerializeField] private GameObject carLeftTire;
        [SerializeField] private GameObject carRightTire;
        
        // Internal variables for handling movement
        private InputReader _inputReader;
        private float _velocity = 0.0f;
        private float _smoothTime = 0.2f;
        private float _targetSpeed;

        // Input check timing
        private float inputCheckInterval = 0.1f;
        private float timeSinceLastCheck = 0.0f;

        // Positional boundaries for movement
        private float _maxOffset = 2f;
        private float _minOffset = -2f;

        protected override void Awake()
        {
            base.Awake();
            _inputReader = new InputReader();
        }

        private void Update()
        {
            timeSinceLastCheck += Time.deltaTime;

            if (timeSinceLastCheck >= inputCheckInterval)
            {
                CheckInput();
                timeSinceLastCheck = 0f;
            }

            SplineFollower.followSpeed = Mathf.SmoothDamp(SplineFollower.followSpeed, _targetSpeed, ref _velocity, _smoothTime);
            HandleLateralMovement();
        }

        private void CheckInput()
        {
            bool isTouched = _inputReader.IsScreenTouched();

            if (isTouched)
            {
                _targetSpeed = Mathf.Min(SplineFollower.followSpeed + Acceleration, MaximumSpeed);
            }
            else
            {
                _targetSpeed = BaseMoveSpeed;
            }
        }

        private void HandleLateralMovement()
        {
            float inputX = _inputReader.GetCurrentInput().x;

            // Determine the direction for the car's rotation based on input.
            float targetAngle = inputX > 0 ? -maxDriftAngle : (inputX < 0 ? maxDriftAngle : 0f);
            carBody.transform.DORotate(new Vector3(0f, targetAngle, 0f), rotationDuration).SetEase(rotationEase);

            // Rotate the tires based on the input direction.
            if (inputX != 0) // If there is input on the x-axis, rotate the tires.
            {
                // Determine the rotation direction for the tires.
                float tireRotationAngle = inputX > 0 ? -25f : 25f; // Angles for tire rotation based on input direction.
        
                // Apply the rotation to the tires using DOTween for smooth transition.
                carLeftTire.transform.DOLocalRotate(new Vector3(0f, tireRotationAngle, 0f), rotationDuration).SetEase(rotationEase);
                carRightTire.transform.DOLocalRotate(new Vector3(0f, tireRotationAngle, 0f), rotationDuration).SetEase(rotationEase);
            }
            else // If there is no input, return the tires to their default position.
            {
                // Reset the tire rotation when there's no input, ensuring they go back to the initial rotation.
                carLeftTire.transform.DOLocalRotate(Vector3.zero, rotationDuration).SetEase(rotationEase);
                carRightTire.transform.DOLocalRotate(Vector3.zero, rotationDuration).SetEase(rotationEase);
            }

            // Depending on the direction, we move the car to the right or left.
            float direction = inputX != 0 ? inputX / Mathf.Abs(inputX) : 0; // Ensures that we only get -1, 0, or 1.
            SplineFollower.motion.offset += new Vector2(0.05f * direction, 0f);

            // We make sure the car doesn't go off the tracks.
            SplineFollower.motion.offset = new Vector2(
                Mathf.Clamp(SplineFollower.motion.offset.x, _minOffset, _maxOffset),
                SplineFollower.motion.offset.y
            );
        }

    }
}
