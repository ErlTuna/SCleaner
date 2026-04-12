using System;
using UnityEngine;

public class SettingsMenu : MonoBehaviour, IMenuSection
{   
    public static Action OnSettingsOpened;
    [SerializeField] GameObject _defaultSelected;
    [SerializeField] CanvasGroup _canvasGroup;
    public bool IsVisible { get; set;}
    public void Show()
    {
        if (_canvasGroup)
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            if (_defaultSelected != null)
                UISelector.instance.SetSelected(_defaultSelected);
            IsVisible = true;

            OnSettingsOpened?.Invoke();
        }

        
    }

    public void Hide()
    {
        if (_canvasGroup)
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            UISelector.instance.SetSelected(null);
            IsVisible = false;
        }
    }
}
