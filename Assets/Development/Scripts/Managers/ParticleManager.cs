using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Development.Scripts.Managers
{
    public class ParticleManager : MonoBehaviour
    {
        public static ParticleManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            else
            {
                Destroy(gameObject);
            }
        }

        public void PlayParticle(GameObject particle)
        {
            ParticleSystem system = particle.GetComponent<ParticleSystem>();
            
            system.gameObject.SetActive(true);
            
            if (system != null)
            {
                system.Play();
            }

            StartCoroutine(WaitForParticle(system));

        }

        private IEnumerator WaitForParticle(ParticleSystem system)
        {
            while (system.isPlaying)
            {
                yield return null;
            }
            
        }
    }
}