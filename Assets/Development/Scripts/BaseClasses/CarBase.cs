using Development.Scripts.ScriptableObjects;
using Dreamteck.Splines;
using UnityEngine;

namespace Development.Scripts.BaseClasses
{
    public abstract class CarBase : MonoBehaviour
    {
        [SerializeField] private CarData carData;

        protected SplineFollower SplineFollower { get; set; }
        protected float MaximumSpeed { get; private set; }
        protected float BaseMoveSpeed { get; private set; }
        protected float MaximumSpeedDuration { get; private set; }
        protected float Acceleration { get; private set; }
        
        
        /// <summary>
        /// Initialization of car data.
        /// </summary>
        protected virtual void Initialize()
        {
            MaximumSpeed = carData.MaximumSpeed;
            MaximumSpeedDuration = carData.MaximumSpeedDuration;
            Acceleration = carData.Acceleration;
            BaseMoveSpeed = carData.BaseMoveSpeed;
        }

        protected virtual void Awake()
        {
            Initialize();
        }
        
    }
}