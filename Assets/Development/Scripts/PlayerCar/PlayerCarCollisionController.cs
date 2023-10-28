using UnityEngine;

namespace Development.Scripts.PlayerCar
{
    public class PlayerCarCollisionController : MonoBehaviour
    {
     

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("TrafficCar"))
            {
                
            }
        }

    }
}