using UnityEngine;

namespace Development.Scripts.Destroyer
{
    public class TrafficCarDestroyer : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("TrafficCar"))
            {
                Debug.Log("TrafficCarDestroyer: OnTriggerEnter");
                Pool.Instance.DeactivateObject(other.gameObject, PoolItemType.TrafficCar_White);
            }
        }
    }
}