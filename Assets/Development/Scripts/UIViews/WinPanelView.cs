using Development.Scripts.Managers;
using Development.Scripts.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Development.Scripts.UIViews
{
    public class WinPanelView : View
    {
        [SerializeField] private Button playAgainButton;
        public override void Initialize()
        {
            playAgainButton.onClick.AddListener(() => ViewManager.Show<MainPanelView>());
            playAgainButton.onClick.AddListener(() => GameManager.Instance.ProgressGameStateInvoker());
            playAgainButton.onClick.AddListener(() => GameManager.Instance.RestartGame());
        }
    }
}