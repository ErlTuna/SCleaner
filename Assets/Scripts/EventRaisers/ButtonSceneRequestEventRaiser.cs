using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSceneRequestEventRaiser : MonoBehaviour
{
    [SerializeField] SceneRequestEventChannelSO _eventChannel;

    public void RaiseEvent(SceneLoaderRequest request)
    {
        if (_eventChannel != null)
            _eventChannel.RaiseEvent(request);
    }
}
