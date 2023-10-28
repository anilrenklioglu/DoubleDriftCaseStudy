using System;
using Development.Scripts.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Development.Scripts.Managers
{
    public class UIManager : MonoBehaviour
    {
        [Header("UI Elements")]
        [Space(4)]
        
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private Button startButton;

        private void Awake()
        {
            startButton.onClick.AddListener(StartButtonClicked);
            GameManager.OnGameStateChanged += GameStateChanged;
        }

        private void OnDestroy()
        {
            startButton.onClick.RemoveListener(StartButtonClicked);
            GameManager.OnGameStateChanged -= GameStateChanged;
        }

        

        private void StartButtonClicked()
        {
            GameManager.Instance.ProgressGameStateInvoker();
        }
        
        private void GameStateChanged(GameState currentGameState)
        {
            switch (currentGameState)
            {
                case GameState.Prepare:
                    OpenMainMenu();
                    break;
                case GameState.Playing:
                    CloseMainMenu();
                    break;
                case GameState.Won:
                    break;
                case GameState.GameOver:
                    break;
            }
        }
        
        private void OpenMainMenu()
        {
            mainMenuPanel.SetActive(true);
        }
        
        private void CloseMainMenu()
        {
            mainMenuPanel.SetActive(false);
        }
        
    }
}