using UnityEngine;

public class GameplayUIManager : MonoBehaviour
{
    [SerializeField] Canvas _canvas;
    [SerializeField] VoidEventChannelSO _pauseToggleEventChannel;

    void OnEnable()
    {
        _pauseToggleEventChannel.OnEventRaised += ToggleCanvas;
    }

    void OnDisable()
    {
        _pauseToggleEventChannel.OnEventRaised -= ToggleCanvas;
    }
    
    void ToggleCanvas()
    {
        _canvas.enabled = !_canvas.enabled;
    }
}
