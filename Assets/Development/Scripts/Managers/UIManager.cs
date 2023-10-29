using System;
using Development.Scripts.UIViews;
using Development.Scripts.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Development.Scripts.Managers
{
    public class UIManager : MonoBehaviour
    {
        private void Start()
        {
            GameManager.Instance.OnGameStateChanged += GameStateChanged;
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnGameStateChanged -= GameStateChanged;
        }
        private void GameStateChanged(GameState currentGameState)
        {
            switch (currentGameState)
            {
                case GameState.Prepare:
                    break;
                case GameState.Playing:
                    break;
                case GameState.Won:
                    ViewManager.Show<WinPanelView>();
                    break;
                case GameState.GameOver:
                    ViewManager.Show<FailPanelView>();
                    break;
            }
        }
    }
}