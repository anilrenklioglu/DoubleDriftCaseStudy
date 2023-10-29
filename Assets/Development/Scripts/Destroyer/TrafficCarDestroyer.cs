using Development.Scripts.Managers;
using UnityEngine;

namespace Development.Scripts.Destroyer
{
    public class TrafficCarDestroyer : MonoBehaviour
    { 
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("TrafficCar"))
            {
                TrafficManager.Instance.RemoveTrafficCar(other.gameObject);
                Pool.Instance.DeactivateObject(other.gameObject, PoolItemType.TrafficCar_White);
                
            }
        }
    }
}