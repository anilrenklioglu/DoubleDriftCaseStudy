using Development.Scripts.Managers;
using UnityEngine;

namespace Development.Scripts.PlayerCar
{
    public class PlayerCarCollisionController : MonoBehaviour
    {
        private Rigidbody rb => GetComponent<Rigidbody>();
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("TrafficCar"))
            {
                ParticleManager.Instance.PlayParticle(Pool.Instance.SpawnObject(transform.position,
                    PoolItemType.ExplosionFireballFire, null, Quaternion.identity));
            //Pool.Instance.DeactivateObject(other.gameObject, PoolItemType.TrafficCar_White);

                GameManager.Instance.ProgressGameStateInvoker();
                GameManager.Instance.SetGameWon(false);
                
                CrashAICars(other.rigidbody, other);
            }
        }

        private void CrashAICars(Rigidbody rb, Collision other)
        {
            rb.constraints = RigidbodyConstraints.None;
            this.rb.constraints = RigidbodyConstraints.None;
            rb.useGravity = true;
            this.rb.useGravity = true;
            Vector3 dir = other.GetContact(0).point - transform.position;
            dir.y *= 0.1f;
            rb.AddForceAtPosition(dir.normalized*35, other.GetContact(0).point, ForceMode.Impulse);
            this.rb.AddForceAtPosition(-dir.normalized*10, other.GetContact(0).point, ForceMode.Impulse);
        }
    }
}