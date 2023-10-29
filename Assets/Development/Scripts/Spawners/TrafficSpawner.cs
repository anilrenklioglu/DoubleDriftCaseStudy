using Development.Scripts.AI;
using Development.Scripts.Managers;
using Development.Scripts.ScriptableObjects;
using Development.Scripts.Utilities;
using UnityEngine;

namespace Development.Scripts.Spawners
{
    public class TrafficSpawner : MonoBehaviour
    {
        [SerializeField] private SpawnerData spawnerData;
        
        private float _spawnInterval;
        private float _timeSinceLastSpawn;
        
        private void Initialize()
        {
            _spawnInterval = spawnerData.SpawnInterval;
        }
        
        private void Awake()
        {
            Initialize();
        }
        
        private void Update()
        {
            if(GameManager.Instance.CurrentState != GameState.Playing) return;
            
            _timeSinceLastSpawn += Time.deltaTime;
            
            if (_timeSinceLastSpawn >= _spawnInterval)
            {
                _timeSinceLastSpawn = 0f;
                SpawnTrafficCar();
            }
        }

        private void SpawnTrafficCar()
        {
            TrafficLine randomLine = (TrafficLine)Random.Range(0, 3);
            float spawnX = 0;

            switch (randomLine)
            {
                case TrafficLine.Left:
                    spawnX = -2.5f;
                    break;
                case TrafficLine.Middle:
                    spawnX = 0f;
                    break;
                case TrafficLine.Right:
                    spawnX = 2.5f;
                    break;
            }

            float spawnZ = GameManager.Instance.GetPlayerZPos() + Random.Range(spawnerData.MinimumSpawnDistance, spawnerData.MaximumSpawnDistance);

            if (spawnZ > 1500)
            {
                return;
            }

            Vector3 spawnPosition = new Vector3(spawnX, 0, spawnZ);

            TrafficCarController newTrafficCar = Pool.Instance.SpawnObject(spawnPosition, PoolItemType.TrafficCar_White, transform, Quaternion.identity).GetComponent<TrafficCarController>();

            TrafficManager.Instance.AddTrafficCar(newTrafficCar.gameObject);
        }



    }
}