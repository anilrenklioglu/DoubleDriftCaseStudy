using Development.Scripts.Managers;
using Development.Scripts.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Development.Scripts.UIViews
{
    public class MainPanelView : View
    {
        [SerializeField] private Button playButton; 
        public override void Initialize()
        {
            playButton.onClick.AddListener(() => ViewManager.Show<InGameView>());
            playButton.onClick.AddListener(() => GameManager.Instance.ProgressGameStateInvoker());
        }
    }
}