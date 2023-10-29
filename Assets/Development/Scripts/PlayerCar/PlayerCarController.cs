using System;
using DG.Tweening;
using Development.Scripts.BaseClasses;
using Development.Scripts.Managers;
using Development.Scripts.Utilities;
using Dreamteck.Splines;
using UnityEngine;

namespace Development.Scripts.PlayerCar
{
    public class PlayerCarController : CarBase
    {
       [Header("Variables and references for movement")]
       [Space(4)]
       
        [SerializeField] private GameObject carBody; 
        [SerializeField] private GameObject carLeftTire;
        [SerializeField] private GameObject carRightTire;
        [SerializeField] private float xMoveSpeed =0.1f; // Maximum angle the car can reach while drifting.
        
        
        [Header("Variables for drifting")]
        [Space(4)]
        
        [SerializeField] private float maxDriftAngle = 25f; // Maximum angle the car can reach while drifting.
        [SerializeField] private float rotationDuration = 1f; // How quickly the car reaches the maximum drift angle.
        [SerializeField] private Ease rotationEase = Ease.OutQuart; // Type of easing for the rotation animation.
        
        // Internal variables for handling movement.
        private InputReader _inputReader;
        private float _velocity = 0.0f;
        private float _smoothTime = 0.2f;
        private float _targetSpeed;

        // Input check timing
        private float inputCheckInterval = 0.1f;
        private float timeSinceLastCheck = 0.0f;

        // Positional boundaries for movement.
        private float _maxOffset = 2f;
        private float _minOffset = -2f;
        
        private Vector3 _initialPosition;

        protected override void Awake()
        {
            base.Awake();
            SplineFollower = GetComponent<SplineFollower>();
            SplineFollower.followSpeed = BaseMoveSpeed;
            _inputReader = new InputReader();
            
            GameManager.Instance.OnGameStateChanged += HandleCarStates;
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnGameStateChanged -= HandleCarStates;
        }

        private void Start()
        {
            _initialPosition = transform.position;
        }

        private void HandleCarStates(GameState currentGameState)
        {
            if (currentGameState == GameState.Playing)
            {
                SplineFollower.follow = true;
            }

            switch (currentGameState)
            {
                case GameState.Prepare:
                    
                    break;
                
                case GameState.Playing:
                    SplineFollower.follow = true;
                    break;
                
                case GameState.Won:
                    SplineFollower.follow = false;
                    break;
                
                case GameState.GameOver:
                    transform.position = _initialPosition;
                    SplineFollower.follow = false;
                    carBody.SetActive(false);
                    transform.position = _initialPosition;
                    carBody.transform.rotation = Quaternion.Euler(Vector3.zero);
                    transform.rotation = Quaternion.Euler(Vector3.zero);
                    carBody.SetActive(true);
                    SplineFollower.motion.offset = Vector2.zero;
                    SplineFollower.Rebuild();
                    break;
            }
        }

        private void Update()
        {
            if(GameManager.Instance.CurrentState != GameState.Playing) return;
            
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

            float targetAngle = inputX > 0 ? -maxDriftAngle : (inputX < 0 ? maxDriftAngle : 0f);
            carBody.transform.DORotate(new Vector3(0f, targetAngle, 0f), rotationDuration).SetEase(rotationEase);

            if (inputX != 0)
            {
                float tireRotationAngle = inputX > 0 ? 35f : -35f; // Angles for tire rotation based on input direction.
        
                carLeftTire.transform.DOLocalRotate(new Vector3(0f, tireRotationAngle, 0f), rotationDuration).SetEase(rotationEase);
                carRightTire.transform.DOLocalRotate(new Vector3(0f, tireRotationAngle, 0f), rotationDuration).SetEase(rotationEase);
            }
            else 
            {
                // Reset the tire rotation when there's no input, ensuring they go back to the initial rotation.
                carLeftTire.transform.DOLocalRotate(Vector3.zero, rotationDuration).SetEase(rotationEase);
                carRightTire.transform.DOLocalRotate(Vector3.zero, rotationDuration).SetEase(rotationEase);
            }
            
            float direction = inputX != 0 ? inputX / Mathf.Abs(inputX) : 0; // Ensures that we only get -1, 0, or 1.
            SplineFollower.motion.offset += new Vector2(xMoveSpeed * direction, 0f);

            // We make sure the car doesn't go off the tracks.
            SplineFollower.motion.offset = new Vector2(
                Mathf.Clamp(SplineFollower.motion.offset.x, _minOffset, _maxOffset),
                SplineFollower.motion.offset.y
            );
        }
        
    }
}