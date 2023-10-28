using Development.Scripts.ScriptableObjects;
using Dreamteck.Splines;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Development.Scripts.BaseClasses
{
    [RequireComponent(typeof(SplineFollower))]
    public abstract class CarBase : MonoBehaviour
    {
        [SerializeField] private CarData carData;

        protected SplineFollower SplineFollower { get; private set; }
        protected float MaximumSpeed { get; private set; }
        protected float BaseMoveSpeed { get; private set; }
        protected float MaximumSpeedDuration { get; private set; }
        protected float Acceleration { get; private set; }
        
        /// <summary>
        /// Initialization of car data.
        /// </summary>
        private void Initialize()
        {
            MaximumSpeed = carData.MaximumSpeed;
            MaximumSpeedDuration = carData.MaximumSpeedDuration;
            Acceleration = carData.Acceleration;
            BaseMoveSpeed = carData.BaseMoveSpeed;
            SplineFollower = GetComponent<SplineFollower>();
            SplineFollower.followSpeed = BaseMoveSpeed;
        }

        protected virtual void Awake()
        {
            Initialize();
        }
        
    }
}