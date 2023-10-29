using System;
using Development.Scripts.Utilities;
using Dreamteck.Splines;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Development.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    { 
        public static GameManager Instance { get; private set; }
        
        [SerializeField] private SplineComputer splineComputer;
        [SerializeField] private GameObject player;
        
        [field: SerializeField] public GameState CurrentState { get; private set; }
        
        private bool _isGameWon;
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
            CurrentState = GameState.Prepare;
        }
        
        public void SetGameWon(bool isGameWon)
        {
            _isGameWon = isGameWon;
        }
        
        public float GetPlayerZPos()
        {
            return player.transform.position.z;
        }
        
        private void ProgressGameState()
        {
            CurrentState = CurrentState switch
            {
                GameState.Prepare => GameState.Playing,
                GameState.Playing => _isGameWon ? GameState.Won : GameState.GameOver,
                GameState.Won => GameState.Prepare,
                GameState.GameOver => GameState.Prepare,
                _ => throw new ArgumentOutOfRangeException()
            };

            GameStateChanged(CurrentState);
        }
        
        public void RestartGame()
        {
            OnGameEnd?.Invoke();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        public void ProgressGameStateInvoker() => ProgressGameState();
        
        #region GameManager Events

        public Action<GameState> OnGameStateChanged;
        public void GameStateChanged(GameState gameState) => OnGameStateChanged?.Invoke(gameState);

        public static event Action OnGameEnd;

        #endregion

    }
}