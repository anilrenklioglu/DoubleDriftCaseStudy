using UnityEngine;

namespace Development.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CarData", menuName = "Scriptable Objects/Cars/CarData", order = 0)]
    public class CarData : ScriptableObject
    {
        [SerializeField] private float maximumSpeed;
        [SerializeField] private float baseMoveSpeed;
        [SerializeField] private float maximumSpeedDuration;
        [SerializeField] private float acceleration;
        public float MaximumSpeed => maximumSpeed;
        public float BaseMoveSpeed => baseMoveSpeed;
        public float MaximumSpeedDuration => maximumSpeedDuration;
        public float Acceleration => acceleration;
        

        
    }
}