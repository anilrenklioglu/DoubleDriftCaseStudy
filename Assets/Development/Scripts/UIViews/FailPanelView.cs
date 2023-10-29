using Development.Scripts.Managers;
using Development.Scripts.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Development.Scripts.UIViews
{
    public class FailPanelView : View
    {
        [SerializeField] private Button tryAgainButton;
        public override void Initialize()
        {
            tryAgainButton.onClick.AddListener(() => ViewManager.Show<MainPanelView>());
            tryAgainButton.onClick.AddListener(() => GameManager.Instance.ProgressGameStateInvoker());
            tryAgainButton.onClick.AddListener(() => GameManager.Instance.RestartGame());
        }
    }
}