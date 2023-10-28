using UnityEngine;

namespace Development.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SpawnerData", menuName = "Scriptable Objects/Spawners/SpawnerData", order = 0)]
    public class SpawnerData : ScriptableObject
    {
        [Header("Spawner Settings")]
        
        [SerializeField] private float spawnInterval;
        [SerializeField] private float spawnRate;
            
        public float SpawnInterval => spawnInterval;

        public float SpawnRate => spawnRate;
    }
}