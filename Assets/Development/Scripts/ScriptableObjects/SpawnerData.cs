using UnityEngine;

namespace Development.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SpawnerData", menuName = "Scriptable Objects/Spawners/SpawnerData", order = 0)]
    public class SpawnerData : ScriptableObject
    {
        [Header("Spawner Settings")]
        
        [SerializeField] private float spawnInterval;
        [SerializeField] private float minimumSpawnDistance;
        [SerializeField] private float maximumSpawnDistance;
            
        public float SpawnInterval => spawnInterval;
        public float MinimumSpawnDistance => minimumSpawnDistance;
        public float MaximumSpawnDistance => maximumSpawnDistance;
    }
}