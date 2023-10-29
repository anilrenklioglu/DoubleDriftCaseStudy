using Development.Scripts.Managers;
using UnityEngine;

namespace Development.Scripts.PlayerCar
{
    public class PlayerCarCollisionController : MonoBehaviour
    {
      private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("TrafficCar"))
            {
                ParticleManager.Instance.PlayParticle(Pool.Instance.SpawnObject(transform.position, PoolItemType.ExplosionFireballFire, null, Quaternion.identity));
                Pool.Instance.DeactivateObject(other.gameObject, PoolItemType.TrafficCar_White);
                
                GameManager.Instance.ProgressGameStateInvoker();
                GameManager.Instance.SetGameWon(false);
            }
        }
    }
}