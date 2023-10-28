using System;
using Development.Scripts.Utilities;
using Dreamteck.Splines;
using UnityEngine;

namespace Development.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    { 
        public static GameManager Instance { get; private set; }
        
        [SerializeField] private GameObject player;
        
        [field: SerializeField] public GameState CurrentState { get; private set; }
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
        
        public float GetPlayerZPos()
        {
            return player.transform.position.z;
        }
        
        private void ProgressGameState()
        {
            switch (CurrentState)
            {
                case GameState.Prepare:
                    CurrentState = GameState.Playing;
                    break;
                case GameState.Playing:
                    CurrentState = GameState.GameOver;
                    break;
                case GameState.Won:
                    CurrentState = GameState.Prepare;
                    break;
                case GameState.GameOver:
                    CurrentState = GameState.Prepare;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            GameStateChanged(CurrentState);
        }
        
        public void ProgressGameStateInvoker() => ProgressGameState();
        
        #region GameManager Events

        public static Action<GameState> OnGameStateChanged;
        public static void GameStateChanged(GameState gameState) => OnGameStateChanged?.Invoke(gameState);

        #endregion
        
    }
}