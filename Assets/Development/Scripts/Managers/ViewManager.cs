using System.Collections.Generic;
using Development.Scripts.Utilities;
using UnityEngine;

namespace Development.Scripts.Managers
{
    public class ViewManager : MonoBehaviour
    {
        private static ViewManager _instance;

        [SerializeField] private View startingUView;
        
        [SerializeField] private View[] uiViews;
        
        private View _currentGameUIView;

        private readonly Stack<View> _history = new Stack<View>();
        
        public static T GetView<T>() where T : View
        {
            for (int i = 0; i < _instance.uiViews.Length; i++)
            {
                if (_instance.uiViews[i] is T)
                {
                    return _instance.uiViews[i] as T;
                }
            }

            return null;
        }

        public static void Show<T>(bool remember = true) where T : View
        {
            for (int i = 0; i < _instance.uiViews.Length; i++)
            {
                if (_instance.uiViews[i] is T)
                {
                    if (_instance._currentGameUIView != null)
                    {
                        if (remember)
                        {
                            _instance._history.Push(_instance._currentGameUIView);
                        }
                        
                        _instance._currentGameUIView.Hide();
                    }
                    
                    _instance.uiViews[i].Show();
                    
                    _instance._currentGameUIView = _instance.uiViews[i];
                }
            }
        }
        
        public static void Show(View view, bool remember = true)
        {
            if (_instance._currentGameUIView != null)
            {
                if (remember)
                {
                    _instance._history.Push(_instance._currentGameUIView);
                }
                
                _instance._currentGameUIView.Hide();
            }
            
            view.Show();
            
            _instance._currentGameUIView = view;
        }

        public static void ShowLast()
        {
            if (_instance._history.Count != 0)
            {
                Show(_instance._history.Pop(), false);
            }
        }

        private void Awake() => _instance = this;

        private void Start()
        {
            for (int i = 0; i < uiViews.Length; i++)
            {
                uiViews[i].Initialize();
                uiViews[i].Hide();
            }
            
            if(startingUView != null)
                Show(startingUView, true);
        }
    }
}