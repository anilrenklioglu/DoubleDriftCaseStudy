using System;
using Development.Scripts.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Development.Scripts.Managers
{
    public class UIManager : MonoBehaviour
    {
        [Header("UI Panels")]
        [Space(4)]
        
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject failPanel;

        [Header("UI Button References")]
        [Space(4)]

        [SerializeField] private Button startButton;
        [SerializeField] private Button tryAgainButton;

        private void Awake()
        {
            startButton.onClick.AddListener(ButtonClicked);
            tryAgainButton.onClick.AddListener(ButtonClicked);
            GameManager.Instance.OnGameStateChanged += GameStateChanged;
        }

        private void OnDestroy()
        {
            startButton.onClick.RemoveListener(ButtonClicked);
            tryAgainButton.onClick.RemoveListener(ButtonClicked);
            GameManager.Instance.OnGameStateChanged -= GameStateChanged;
        }
        private void ButtonClicked()
        {
            GameManager.Instance.ProgressGameStateInvoker();
        }
        
        private void GameStateChanged(GameState currentGameState)
        {
            switch (currentGameState)
            {
                case GameState.Prepare:
                    OpenMainMenu();
                    CloseFailPanel();
                    break;
                case GameState.Playing:
                    CloseMainMenu();
                    break;
                case GameState.Won:
                    break;
                case GameState.GameOver:
                    OpenFailPanel();
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
        private void OpenFailPanel()
        {
            failPanel.SetActive(true);
        }
        private void CloseFailPanel()
        {
            failPanel.SetActive(false);
        }
        
    }
}