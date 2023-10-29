﻿using System;
using System.Collections.Generic;
using Development.Scripts.Utilities;
using UnityEngine;

namespace Development.Scripts.Managers
{
    public class TrafficManager : MonoBehaviour
    {
        public static TrafficManager Instance;
        
        private List<GameObject> _trafficCars = new List<GameObject>();

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

        private void Start()
        {
            GameManager.Instance.OnGameStateChanged += RemoveTrafficCarList;
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnGameStateChanged -= RemoveTrafficCarList;
        }
        
        public void AddTrafficCar(GameObject trafficCar)
        {
            _trafficCars.Add(trafficCar);
        }
        
        public void RemoveTrafficCar(GameObject trafficCar)
        {
            _trafficCars.Remove(trafficCar);
        }
        
        private void RemoveTrafficCarList(GameState gameState)
        {
            if (gameState is GameState.Prepare or GameState.GameOver)
            {
                foreach (var car in _trafficCars)
                {
                    Pool.Instance.DeactivateObject(car, PoolItemType.TrafficCar_White);
                }
            
                _trafficCars.Clear();    
            }
        }
    }
}