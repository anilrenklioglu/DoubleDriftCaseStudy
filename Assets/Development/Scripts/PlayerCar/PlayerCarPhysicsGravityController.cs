using Development.Scripts.Tests;
using UnityEngine;

namespace Development.Scripts.PlayerCar
{
    public class PlayerCarPhysicsGravityController : MonoBehaviour
    {
        [SerializeField] private float acceleration = 9.8f;
        [SerializeField] private Vector3 direction = Vector3.down;
        [SerializeField] private float maxAngle = 30;
        
        private Rigidbody _rb;
        private PlayerCarPhysicsController _car;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _car = GetComponent<PlayerCarPhysicsController>();
        }

        private void FixedUpdate()
        {
            if (_car.WheelsGrounded() || Vector3.Angle(Vector3.down, -transform.up.normalized) < maxAngle)
            {
                direction = -transform.up.normalized;
            }
            _rb.velocity += direction * acceleration * Time.fixedDeltaTime;
        }
    }
}