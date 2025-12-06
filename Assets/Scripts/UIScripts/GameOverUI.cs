using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] Canvas _canvas;
    [SerializeField] VoidEventChannelSO _gameOverEventChannel;
    [SerializeField] GameObject _defaultSelected;
    [SerializeField] TMP_Text _timeTakenTextBox;

    void Awake()
    {
        _gameOverEventChannel.OnEventRaised += ShowGameOverMenu;
        //DOTween.Init(false, true, LogBehaviour.Default);
    }

    void OnDisable()
    {
        _gameOverEventChannel.OnEventRaised -= ShowGameOverMenu;
    }

    void ShowGameOverMenu()
    {
        if (_canvas != null)
        {
            _canvas.enabled = true;
            if (_defaultSelected != null)
            EventSystem.current.SetSelectedGameObject(_defaultSelected);
        }
    }

    void AnimateTimeTaken()
    {
        
    }


}
